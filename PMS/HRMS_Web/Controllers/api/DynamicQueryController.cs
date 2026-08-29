using B_DB_Context;
using B_DB_Model;
using B_Utility.Common;
using HRMS_Web.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Web.Controllers.api
{
    // SECURITY (#5): this controller stores and executes SQL. It was reachable with no
    // authentication at all; every action now requires a signed-in user.
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DynamicQueryController : ControllerBase
    {
        private readonly DataBase_Context _context;

        public DynamicQueryController(DataBase_Context context)
        {
            _context = context;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] DynamicQuery model)
        {
            _context.DynamicQueries.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Created", model.Id });
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var queries = await _context.DynamicQueries
                .Select(q => new { q.Id, q.Title, q.AccessUsers })
                .ToListAsync();

            return Ok(new { data = queries });
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = await _context.DynamicQueries
                .Include(q => q.Params)
                    .ThenInclude(p => p.Options)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (query == null) return NotFound();
            return Ok(new { data = query });
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] DynamicQuery updated)
        {
            var existing = await _context.DynamicQueries
                .Include(q => q.Params)
                .ThenInclude(p => p.Options)
                .FirstOrDefaultAsync(q => q.Id == updated.Id);

            if (existing == null) return NotFound();

            // Update core fields
            existing.Title = updated.Title;
            existing.SqlQuery = updated.SqlQuery;
            existing.PrintTitle = updated.PrintTitle;
            existing.Type = updated.Type;
            existing.AccessUsers = updated.AccessUsers;

            // Remove old params and options
            _context.QueryParamOptions.RemoveRange(existing.Params.SelectMany(p => p.Options));
            _context.QueryParams.RemoveRange(existing.Params);

            // Add new ones (with DataSourceQuery now supported)
            existing.Params = updated.Params;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Updated" });
        }

        [HttpPost("ExecuteParamQuery")]
        public async Task<IActionResult> ExecuteParamQuery([FromBody] SqlRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Sql))
                return BadRequest("SQL is required");

            // SECURITY (#5): this endpoint only feeds report-filter dropdowns, which are
            // single read statements returning Value/Name. Reject anything else.
            var sqlTrimmed = req.Sql.Trim();
            var startsAsRead = sqlTrimmed.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase)
                            || sqlTrimmed.StartsWith("WITH", StringComparison.OrdinalIgnoreCase);
            if (!startsAsRead || sqlTrimmed.Contains(';'))
                return BadRequest("Only a single SELECT statement is allowed here");

            var results = new List<object>();

            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = req.Sql;
                await _context.Database.OpenConnectionAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        results.Add(new
                        {
                            value = reader["Value"].ToString(),
                            name = reader["Name"].ToString()
                        });
                    }
                }
            }

            return Ok(results);
        }

        public class SqlRequest
        {
            public string Sql { get; set; }
        }
    }
}
