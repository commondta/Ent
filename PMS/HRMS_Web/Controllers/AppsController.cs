using HRMS_Web.Services.ErpPlatform;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers
{
    /// <summary>
    /// ERP application selection ("My Home" entry): Login → /Apps → pick an application.
    /// Authorised by the central session (erp_sso cookie) rather than the PMS session, so a
    /// user whose only application is LIMS/HRMS can use it too. A user with exactly one
    /// application is taken straight into it.
    /// </summary>
    public class AppsController : Controller
    {
        private readonly ErpPlatformService _erp;
        public AppsController(ErpPlatformService erp) { _erp = erp; }

        private string? Token => Request.Cookies[_erp.CookieName] ?? HttpContext.Session.GetString("erp_sso");
        private bool PmsSignedIn => !string.IsNullOrEmpty(HttpContext.Session.GetString("ID"));

        // "Recently used" on the launcher: application codes this browser opened, most recent
        // first, in a plain persistent cookie — presentation state, not part of the SSO session.
        private const string RecentCookie = "erp_recent";

        private List<string> RecentCodes() =>
            (Request.Cookies[RecentCookie] ?? "")
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Where(c => c.All(char.IsLetterOrDigit))
                .Take(5).ToList();

        private void RememberRecent(string code)
        {
            var codes = RecentCodes();
            codes.RemoveAll(c => string.Equals(c, code, StringComparison.OrdinalIgnoreCase));
            codes.Insert(0, code.ToUpperInvariant());
            Response.Cookies.Append(RecentCookie, string.Join(",", codes.Take(5)), new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(90),
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Path = "/",
                IsEssential = true
            });
        }

        private List<ErpApplication>? CurrentApps()
        {
            if (!_erp.Enabled)
                return PmsSignedIn ? new List<ErpApplication> { new ErpApplication { Code = "PMS", Name = "Property Management System", BaseUrl = "/", IsActive = true, SortOrder = 1 } } : null;
            var uid = _erp.Validate(Token);
            if (uid == null) return null;
            return _erp.ApplicationsFor(uid.Value);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var apps = CurrentApps();
            if (apps == null) return RedirectToAction("Index", "Login");
            var live = apps.Where(a => a.IsActive).ToList();
            if (live.Count == 1 && !Request.Query.ContainsKey("stay"))
                return RedirectToAction("Go", new { code = live[0].Code });   // one application → straight in
            ViewBag.FullName = HttpContext.Session.GetString("FullName") ?? "";
            ViewBag.LimsPrefix = HttpContext.RequestServices.GetService<IConfiguration>()?["Erp:LimsPrefix"] ?? "/lims";
            ViewBag.PayrollPrefix = HttpContext.RequestServices.GetService<IConfiguration>()?["Erp:PayrollPrefix"] ?? "/payroll";
            ViewBag.Recent = RecentCodes();
            return View(apps);
        }

        /// <summary>Enter an application. PMS is this app; the others are on this host under their prefix.</summary>
        [HttpGet]
        public IActionResult Go(string? code, string? id)
        {
            code ??= id;   // both /Apps/Go?code=LIMS and /Apps/Go/LIMS
            var apps = CurrentApps();
            if (apps == null) return RedirectToAction("Index", "Login");
            var app = apps.FirstOrDefault(a => string.Equals(a.Code, code, StringComparison.OrdinalIgnoreCase));
            if (app == null || !app.IsActive) return RedirectToAction("Index", new { stay = 1 });
            RememberRecent(app.Code);
            if (string.Equals(app.Code, "PMS", StringComparison.OrdinalIgnoreCase))
                // not yet signed into PMS: Login/Index signs the PMS session in from the live central
                // session (no second login) and continues to Home, or falls back to /Apps?stay=1
                return PmsSignedIn ? RedirectToAction("Index", "Home") : RedirectToAction("Index", "Login");
            var url = string.IsNullOrWhiteSpace(app.BaseUrl) ? "/" : app.BaseUrl;
            if (!url.EndsWith("/")) url += "/";
            return Redirect(url);
        }
    }
}
