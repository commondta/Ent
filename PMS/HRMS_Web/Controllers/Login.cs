using Microsoft.AspNetCore.Mvc;
using B_DB_Model;

using B_Utility.Common;
using B_DB_Context;
using HRMS_Web.Models;
using System.Diagnostics.Eventing.Reader;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using HRMS_Web.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using HRMS_Web.Models.DTOs;
using HRMS_Web.Services.AlertService;
using System.Security.Cryptography;
using HRMS_Web.Services.ErpPlatform;

namespace HRMS_Web.Controllers
{
    public class Login : Controller
    {
        private readonly DataBase_Context _db;
        private readonly IConfiguration configuration;
        private readonly IAlertService alertService;
        private readonly ErpPlatformService erp;

        public Login(DataBase_Context db, IConfiguration configuration, IAlertService alertService, ErpPlatformService erp)
        {
            _db = db;
            this.configuration = configuration;
            this.alertService = alertService;
            this.erp = erp;
        }
        public IActionResult Index()
        {
            // ERP platform: one login for the whole solution. A user who is already signed in
            // centrally (arriving from LIMS's application switcher, or reopening the host) must
            // not be asked to log in again — sign the PMS session in from the central session.
            if (erp.Enabled)
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("ID"))) return RedirectToAction("Index", "Home");
                var token = Request.Cookies[erp.CookieName] ?? HttpContext.Session.GetString("erp_sso");
                var uid = erp.Validate(token);
                if (uid != null)
                {
                    if (SignInPmsFromCentral(uid.Value, token!)) return RedirectToAction("Index", "Home");
                    return RedirectToAction("Index", "Apps", new { stay = 1 });   // central user without a PMS account
                }
            }
            return View();
        }

        /// <summary>
        /// ERP platform: build the PMS session for the PMS account linked to a live central session
        /// (ERP_Platform.Users.PmsUserId) — the same session keys LoginToPortal sets, no password
        /// involved because the central session already proved it. False when the central user has
        /// no active PMS account.
        /// </summary>
        internal bool SignInPmsFromCentral(int erpUserId, string ssoToken)
        {
            try
            {
                var cu = erp.FindUserById(erpUserId);
                if (cu == null || !cu.IsActive || cu.PmsUserId == null) return false;
                var pid = cu.PmsUserId.Value;
                var pMSUser = _db.PMSUser.FirstOrDefault(x => x.Id == pid && x.IsActive == true);
                if (pMSUser == null) return false;

                HttpContext.Session.SetString("ID", pMSUser.Id.ToString());
                HttpContext.Session.SetString("EMP_CODE", pMSUser.EMP_CODE.ToString());
                HttpContext.Session.SetString("desig", pMSUser.DESIG_DESC ?? "");
                HttpContext.Session.SetString("departm", pMSUser.DEPARTMENT_DESC ?? "");
                HttpContext.Session.SetString("managerId", pMSUser.Manager_Id.ToString());
                HttpContext.Session.SetString("FullName", pMSUser.EMP_FULL_NAME ?? pMSUser.Username);
                HttpContext.Session.SetString("Permissions", JsonConvert.SerializeObject(GetUserAssignedPermission(Convert.ToInt32(pMSUser.Id))));
                try { alertService.GetNDC(); } catch (Exception) { }
                HttpContext.Session.SetString("token", UHelper.CreateJWT(pMSUser, configuration));
                HttpContext.Session.SetString("erp_sso", ssoToken);
                HttpContext.Session.SetString("erp_apps", JsonConvert.SerializeObject(erp.ApplicationsForToken(ssoToken)));
                return true;
            }
            catch (Exception) { return false; }
        }
        public IActionResult Forget()
        {
            return View();
        }
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult LoginToPortal(LoginViewModel user)
        {
            Response_Result response_Results = new Response_Result();
            var pMSUser = _db.PMSUser.Where(x => x.Username == user.Email && x.IsActive == true).FirstOrDefault();

            if (pMSUser == null || !MatchPasswordHash(user.Password, pMSUser.PasswordHash, pMSUser.PasswordKey))
            {
                // ERP platform: this is the ONE login for every solution. Not a PMS account (or not the
                // PMS password) → try the central credentials, then the other solutions' native accounts
                // (LIMS verifies over the shared secret; the credential is then stored centrally).
                if (erp.Enabled && CentralLogin(user, response_Results)) return Json(response_Results);

                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.error);
                response_Results.message = "Email Or Password You Have Entered is Wrong";

                return Json(response_Results);
            }
            else
            {
                HttpContext.Session.SetString("ID", pMSUser.Id.ToString());
                HttpContext.Session.SetString("EMP_CODE", pMSUser.EMP_CODE.ToString());
                HttpContext.Session.SetString("desig", pMSUser.DESIG_DESC);
                HttpContext.Session.SetString("departm", pMSUser.DEPARTMENT_DESC);
                HttpContext.Session.SetString("managerId", pMSUser.Manager_Id.ToString());
                HttpContext.Session.SetString("FullName", pMSUser.EMP_FULL_NAME);
                HttpContext.Session.SetString("Permissions", JsonConvert.SerializeObject(GetUserAssignedPermission(Convert.ToInt32(pMSUser.Id))));
                alertService.GetNDC();
                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                response_Results.token = UHelper.CreateJWT(pMSUser, configuration);
                HttpContext.Session.SetString("token", response_Results.token);
                // ERP platform: this is the single login for the whole solution. Open a central
                // session and hand its token to the browser; LIMS/HRMS trust that cookie (no second login).
                if (erp.Enabled)
                {
                    try
                    {
                        var ssoToken = erp.CreateSession(Convert.ToInt32(pMSUser.Id), pMSUser.Username, pMSUser.EMP_FULL_NAME,
                            HttpContext.Connection.RemoteIpAddress?.ToString(), Request.Headers["User-Agent"].ToString());
                        Response.Cookies.Append(erp.CookieName, ssoToken, SsoCookieOptions(user.RememberMe));
                        HttpContext.Session.SetString("erp_sso", ssoToken);
                        HttpContext.Session.SetString("erp_apps", JsonConvert.SerializeObject(erp.ApplicationsForToken(ssoToken)));
                        // keep the central credential in step with the PMS one (same hash scheme) so the
                        // centre can answer for this user even when PMS is not the app being opened
                        var cu = erp.FindUser(pMSUser.Username);
                        if (cu != null && pMSUser.PasswordHash != null && pMSUser.PasswordKey != null)
                            erp.StoreCredential(cu.Id, pMSUser.PasswordHash, pMSUser.PasswordKey);
                    }
                    catch (Exception) { /* PMS login still succeeds; the other applications will ask for the ERP login */ }
                }
                return Json(response_Results);
            }
        }

        /// <summary>
        /// ERP platform: sign a non-PMS account in with the single login. Central hash first, then the
        /// LIMS-native account (verified by LIMS over the shared secret and stored centrally). On success
        /// the central session + erp_sso cookie are created exactly as for a PMS user; the PMS session
        /// gets no "ID" (this user has no PMS account), so PMS pages stay closed to them while
        /// /Apps and the other applications open.
        /// </summary>
        private bool CentralLogin(LoginViewModel user, Response_Result result)
        {
            try
            {
                var username = (user.Email ?? "").Trim();
                if (username.Length == 0 || string.IsNullOrEmpty(user.Password)) return false;

                var cu = erp.FindUser(username);
                var ok = cu != null && cu.IsActive && ErpPlatformService.VerifyHmac(user.Password, cu.PasswordHash, cu.PasswordKey);
                if (!ok)
                {
                    var v = erp.VerifyWithLims(username, user.Password);
                    if (!v.ok) return false;
                    var uid = erp.EnsureCentralUser(username, v.email ?? username, v.name, v.limsUserId, "LIMS_USER");
                    erp.StoreCredential(uid, user.Password);
                    cu = erp.FindUser(username);
                    if (cu == null || !cu.IsActive) return false;
                }

                var ssoToken = erp.CreateSessionForUser(cu!.Id, HttpContext.Connection.RemoteIpAddress?.ToString(), Request.Headers["User-Agent"].ToString());
                Response.Cookies.Append(erp.CookieName, ssoToken, SsoCookieOptions(user.RememberMe));
                HttpContext.Session.SetString("erp_sso", ssoToken);
                HttpContext.Session.SetString("erp_apps", JsonConvert.SerializeObject(erp.ApplicationsForToken(ssoToken)));
                HttpContext.Session.SetString("FullName", cu.FullName ?? cu.Username);
                result.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                result.token = "";
                return true;
            }
            catch (Exception) { return false; }
        }

        /// <summary>
        /// The central SSO cookie: a browser-session cookie by default — closing the browser ends the
        /// signed-in state even though the central session row lives SessionHours (user concern
        /// 2026-08-23: reopening localhost landed straight in PMS). "Remember me" keeps it for the
        /// full central session length.
        /// </summary>
        private CookieOptions SsoCookieOptions(bool rememberMe)
        {
            var o = new CookieOptions { HttpOnly = true, Secure = Request.IsHttps, SameSite = SameSiteMode.Lax, Path = "/", IsEssential = true };
            if (rememberMe) o.Expires = DateTimeOffset.UtcNow.AddHours(erp.SessionHours);
            return o;
        }

        public IActionResult SignOut()
        {
            // SECURITY: clear the whole session — the old key-by-key removal left the JWT
            // ("token") and FullName behind after sign-out.
            // ERP platform: revoke the central session so every application signs out together.
            var ssoToken = Request.Cookies[erp.CookieName] ?? HttpContext.Session.GetString("erp_sso");
            erp.Revoke(ssoToken);
            Response.Cookies.Delete(erp.CookieName, new CookieOptions { Path = "/" });
            HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }
        private bool MatchPasswordHash(string passwordText, byte[] password, byte[] passwordKey)
        {
            using (var hmac = new HMACSHA512(passwordKey))
            {
                var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passwordText));

                for (int i = 0; i < passwordHash.Length; i++)
                {
                    if (passwordHash[i] != password[i])
                        return false;
                }

                return true;
            }
        }

        public List<AllUserPermissionsDto> GetUserAssignedPermission(int userId)
        {
            var userPermissions = (from x in _db.UserPermissionMapping
                                   where x.EMP_CODE == userId
                                   select new AllUserPermissionsDto
                                   {
                                       Id = (int)x.PermissionFormsId,
                                       CanAdd = x.CanAdd,
                                       CanDelete = x.CanDelete,
                                       CanEdit = x.CanEdit,
                                       CanView = x.CanView,
                                       Name = x.PermissionForms.Name,
                                   }).ToList();

            return userPermissions;
        }
        public class PermissionsDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Title { get; set; }
            public bool CanEdit { get; set; }
            public bool CanDelete { get; set; }
            public bool CanView { get; set; }
            public bool CanAdd { get; set; }
        }
    }
}
