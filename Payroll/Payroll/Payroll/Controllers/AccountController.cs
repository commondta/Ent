using DataLayer;
using Payroll_HCC.Filters;
using Payroll_HCC.Infrastructure;
using System;
using System.Web;
using System.Web.Mvc;

namespace Payroll_HCC.Controllers
{
    /// <summary>Sign in / sign out / own profile and password.</summary>
    public class AccountController : Controller
    {
        // GET: /Account/Login
        public ActionResult Login(string returnUrl, string local)
        {
            if (App.CurrentUser != null) return RedirectToAction("Index", "Home");
            if (ErpSso.Enabled && local != "1")
            {
                // A live central session must never see a login form (ERP rule): sign in from the cookie.
                string token = ErpSso.CookieToken(Request);
                if (token != null)
                {
                    try
                    {
                        var id = ErpSso.Validate(token);
                        if (id != null)
                        {
                            if (ErpSso.SignIn(HttpContext, token, id) != null)
                                return Redirect(!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) ? returnUrl : Url.Action("Index", "Home"));
                            ViewBag.NoAccess = true; // valid ERP user, but not entitled to Payroll
                            return View("NoAccess");
                        }
                    }
                    catch (Exception ex) { FileLogger.Error("ERP SSO on login failed.", ex); }
                }
                // No central session: hand over to the central login, coming back to this app afterwards.
                string back = VirtualPathUtility.ToAbsolute("~/") + (string.IsNullOrEmpty(returnUrl) ? "" : "?returnUrl=" + HttpUtility.UrlEncode(returnUrl));
                return Redirect(ErpSso.LoginUrl + "?returnUrl=" + HttpUtility.UrlEncode(back));
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string passwd, string returnUrl)
        {
            string reason;
            SessionUser user = App.Security.Authenticate(username, passwd, out reason);
            if (user == null)
            {
                try { App.Activity.Log(username ?? "", "LoginFailed", "Security", null, reason, App.ClientIp); } catch (Exception ex) { FileLogger.Error("log", ex); }
                TempData["LoginError"] = reason;
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            // Fresh session id on login (session-fixation protection): drop anything from the anonymous session.
            object activeComp = Session["activeComp"];
            Session.Clear();
            Session[App.SessionUserKey] = user;
            Session["activeComp"] = activeComp;
            App.Log("Login", null, "Signed in as " + user.RoleName);

            if (user.MustChangePassword) return RedirectToAction("ChangePassword", new { forced = 1 });
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        // POST: /Account/Logout  (POST + anti-forgery so a third-party page cannot sign the user out)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            if (App.CurrentUser != null) App.Log("Logout", null, "Signed out");
            string ssoToken = Session[ErpSso.TokenSessionKey] as string ?? ErpSso.CookieToken(Request);
            Session.Clear();
            Session.Abandon();
            // Expire the session cookie so the old id is not reused by the browser.
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", "") { Expires = DateTime.Now.AddDays(-1), HttpOnly = true });
            if (ErpSso.Enabled && ssoToken != null)
            {
                // Global sign-out: revoke the central session and let the platform clear its cookie.
                ErpSso.Revoke(ssoToken);
                return Redirect(ErpSso.LogoutUrl);
            }
            TempData["LoginInfo"] = "You have been signed out.";
            return RedirectToAction("Login");
        }

        // GET: /Account/Logout - a GET (e.g. an old bookmark) just shows the login page.
        [HttpGet]
        [ActionName("Logout")]
        public ActionResult LogoutGet()
        {
            return RedirectToAction("Login");
        }

        [AdminAuthorize]
        public ActionResult Profile()
        {
            SessionUser u = App.CurrentUser;
            ViewBag.User = App.Security.GetUser(u.id);
            ViewBag.Recent = App.Activity.Recent(15, u.Username);
            ViewBag.MyRequests = App.Approvals.MyRequests(u.Username, 10);
            return View();
        }

        [AdminAuthorize]
        public ActionResult ChangePassword(string forced)
        {
            ViewBag.Forced = IsForced(forced) || App.CurrentUser.MustChangePassword;
            return View();
        }

        [AdminAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword, string forced)
        {
            ViewBag.Forced = IsForced(forced) || App.CurrentUser.MustChangePassword;
            string error = ValidatePassword(newPassword);
            if (error == null && newPassword != confirmPassword) error = "The new password and its confirmation do not match.";
            if (error == null && !App.Security.ChangePassword(App.CurrentUser.id, currentPassword, newPassword)) error = "The current password is incorrect.";
            if (error != null)
            {
                ViewBag.Error = error;
                return View();
            }
            App.CurrentUser.MustChangePassword = false;
            App.Log("Security", null, "Changed own password");
            TempData["Success"] = "Your password has been changed.";
            return RedirectToAction("Index", "Home");
        }

        static bool IsForced(string v) { return v == "1" || string.Equals(v, "true", StringComparison.OrdinalIgnoreCase); }

        public static string ValidatePassword(string pwd)
        {
            if (string.IsNullOrEmpty(pwd) || pwd.Length < 8) return "Password must be at least 8 characters.";
            bool letter = false, digit = false;
            foreach (char c in pwd) { if (char.IsLetter(c)) letter = true; if (char.IsDigit(c)) digit = true; }
            if (!letter || !digit) return "Password must contain both letters and digits.";
            return null;
        }
    }
}
