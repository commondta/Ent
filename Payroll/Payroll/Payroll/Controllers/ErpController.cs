using DataLayer;
using Payroll_HCC.Infrastructure;
using System;
using System.Web.Mvc;

namespace Payroll_HCC.Controllers
{
    /// <summary>
    /// ERP platform endpoints (same contract as LIMS' ErpController):
    ///  GET  /erp/touch  - pre-authenticates this app from the erp_sso cookie (called by the Applications Library).
    ///  POST /erp/verify - lets the central login verify a Payroll-native account (X-Erp-Secret shared secret).
    /// </summary>
    public class ErpController : Controller
    {
        [HttpGet]
        public ActionResult Touch()
        {
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            if (!ErpSso.Enabled) return new HttpStatusCodeResult(204);
            if (App.CurrentUser != null) return new HttpStatusCodeResult(204);
            string token = ErpSso.CookieToken(Request);
            if (token == null) return new HttpStatusCodeResult(204);
            try
            {
                var id = ErpSso.Validate(token);
                if (id != null) ErpSso.SignIn(HttpContext, token, id);
            }
            catch (Exception ex) { FileLogger.Error("erp/touch failed.", ex); }
            return new HttpStatusCodeResult(204);
        }

        [HttpPost]
        public ActionResult Verify(string username, string password)
        {
            string secret = Request.Headers["X-Erp-Secret"];
            if (string.IsNullOrEmpty(ErpSso.SharedSecret) || secret != ErpSso.SharedSecret)
                return new HttpStatusCodeResult(403);
            string reason;
            SessionUser u = App.Security.Authenticate(username, password, out reason);
            if (u == null) return Json(new { ok = false });
            return Json(new { ok = true, id = u.id, email = u.Email, name = u.DisplayName, isAdmin = u.IsAdministrator });
        }
    }
}
