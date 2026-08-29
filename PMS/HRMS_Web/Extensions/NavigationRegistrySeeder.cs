using B_DB_Context;
using B_DB_Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HRMS_Web.Extensions
{
    // Bootstraps the navigation/form registry (AI file.xlsx Instructions §5) at startup:
    // creates the NavigationNodes table when missing, seeds it once from
    // App_Data/navigation-seed.json, and backfills PermissionForms + per-user grants
    // for forms that were never in the legacy menu (the restored hidden forms) so
    // they can appear at all. Idempotent — safe to run on every start.
    public static class NavigationRegistrySeeder
    {
        private class SeedNode
        {
            public string type { get; set; } = "";
            public string path { get; set; } = "";
            public string parentPath { get; set; } = "";
            public string display { get; set; } = "";
            public string? legacyName { get; set; }
            public string? permissionKey { get; set; }
            public string? route { get; set; }
            public int seq { get; set; }
        }

        public static void EnsureSeeded(IServiceProvider services)
        {
            var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("NavigationRegistrySeeder");
            try
            {
                var db = services.GetRequiredService<DataBase_Context>();
                var env = services.GetRequiredService<IWebHostEnvironment>();

                db.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'dbo.NavigationNodes', N'U') IS NULL
CREATE TABLE dbo.NavigationNodes (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ParentId INT NULL,
    NodeType NVARCHAR(20) NOT NULL,
    DisplayName NVARCHAR(200) NOT NULL,
    LegacyName NVARCHAR(200) NULL,
    PermissionKey NVARCHAR(200) NULL,
    FormKey NVARCHAR(200) NULL,
    Route NVARCHAR(300) NULL,
    SequenceNo INT NOT NULL DEFAULT 0,
    Depth INT NOT NULL DEFAULT 0,
    IsVisible BIT NOT NULL DEFAULT 1,
    IsActive BIT NOT NULL DEFAULT 1
);");

                if (!db.NavigationNodes.Any())
                {
                    var seedPath = Path.Combine(env.ContentRootPath, "App_Data", "navigation-seed.json");
                    if (!File.Exists(seedPath))
                    {
                        logger.LogWarning("navigation-seed.json not found at {Path}; registry left empty", seedPath);
                        return;
                    }

                    var nodes = JsonConvert.DeserializeObject<List<SeedNode>>(File.ReadAllText(seedPath))
                                ?? new List<SeedNode>();

                    var idByPath = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                    // Depth comes from the parent chain — NOT from counting '/' in the
                    // path, because display names may themselves contain slashes
                    // (File Doc/Dup Request, Assets/Media, …).
                    var depthByPath = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                    var ordered = nodes.OrderBy(n => n.path.Count(c => c == '/')).ToList();

                    foreach (var group in ordered.GroupBy(n => n.path.Count(c => c == '/')))
                    {
                        var batch = new List<(SeedNode seed, NavigationNode entity)>();
                        foreach (var n in group)
                        {
                            int? parentId = null;
                            int depth = 0;
                            if (!string.IsNullOrEmpty(n.parentPath))
                            {
                                if (!idByPath.TryGetValue(n.parentPath, out var pid))
                                {
                                    logger.LogWarning("Seed node '{Path}' has unknown parent '{Parent}' — skipped", n.path, n.parentPath);
                                    continue;
                                }
                                parentId = pid;
                                depth = depthByPath[n.parentPath] + 1;
                            }
                            depthByPath[n.path] = depth;

                            var entity = new NavigationNode
                            {
                                ParentId = parentId,
                                NodeType = n.type,
                                DisplayName = n.display,
                                LegacyName = n.legacyName,
                                PermissionKey = n.type == "Form" ? n.permissionKey : null,
                                FormKey = MakeKey(n.type == "Form" && !string.IsNullOrEmpty(n.route) ? n.route! : n.path),
                                Route = string.IsNullOrEmpty(n.route) ? null : "/" + n.route!.TrimStart('/'),
                                SequenceNo = n.seq,
                                Depth = depth,
                                IsVisible = true,
                                IsActive = true
                            };
                            db.NavigationNodes.Add(entity);
                            batch.Add((n, entity));
                        }
                        db.SaveChanges();
                        foreach (var (seed, entity) in batch) idByPath[seed.path] = entity.Id;
                    }

                    logger.LogInformation("Navigation registry seeded: {Count} nodes", idByPath.Count);
                }

                BackfillPermissions(db, logger);
            }
            catch (Exception ex)
            {
                // The application must still start with the legacy state if seeding fails.
                logger.LogError(ex, "Navigation registry seeding failed");
            }
        }

        private static void BackfillPermissions(DataBase_Context db, ILogger logger)
        {
            var keys = db.NavigationNodes
                         .Where(n => n.NodeType == "Form" && n.PermissionKey != null)
                         .Select(n => n.PermissionKey!)
                         .Distinct()
                         .ToList();

            var existing = db.PermissionForms.Select(p => p.Name).ToList()
                             .ToHashSet(StringComparer.OrdinalIgnoreCase);
            var missing = keys.Where(k => !existing.Contains(k)).ToList();
            if (!missing.Any()) return;

            int serial = db.PermissionForms.Any() ? db.PermissionForms.Max(p => p.SerialNo) : 0;
            var added = new List<PermissionForms>();
            foreach (var key in missing)
            {
                var pf = new PermissionForms
                {
                    Name = key,
                    Title = key,
                    IsActive = true,
                    SerialNo = ++serial,
                    CreatedOn = DateTime.Now,
                    LastModified = DateTime.Now
                };
                db.PermissionForms.Add(pf);
                added.Add(pf);
            }
            db.SaveChanges();

            // Grant the new keys to every active user (currently only the admin — the
            // restored hidden forms would otherwise stay invisible to everyone).
            var userIds = db.PMSUser.Where(u => u.IsActive == true).Select(u => u.Id).ToList();
            foreach (var uid in userIds)
            {
                foreach (var pf in added)
                {
                    db.UserPermissionMapping.Add(new UserPermissionMapping
                    {
                        EMP_CODE = uid,
                        PermissionFormsId = pf.Id,
                        CanView = true,
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        IsActive = true,
                        CreatedOn = DateTime.Now,
                        LastModified = DateTime.Now
                    });
                }
            }
            db.SaveChanges();
            logger.LogInformation("Permission backfill: {Forms} new PermissionForms granted to {Users} user(s)",
                added.Count, userIds.Count);
        }

        private static string MakeKey(string source)
        {
            var chars = source.Trim().ToLowerInvariant()
                .Select(c => char.IsLetterOrDigit(c) ? c : '.')
                .ToArray();
            var key = new string(chars);
            while (key.Contains("..")) key = key.Replace("..", ".");
            return key.Trim('.');
        }
    }
}
