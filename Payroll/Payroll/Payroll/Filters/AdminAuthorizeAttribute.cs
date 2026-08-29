using DataLayer;
using Payroll_HCC.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll_HCC.Filters
{
    /// <summary>
    /// Requires a signed-in user. Runs as an authorization filter, i.e. before any
    /// controller/action code executes.
    ///  - Unauthenticated browser requests are redirected to the login page (return URL kept);
    ///    unauthenticated AJAX requests get HTTP 401.
    ///  - For GET requests that map to a registered form (see FormRegistry) the user must hold
    ///    the View permission on that form, otherwise an Access Denied page is shown (403 for AJAX).
    ///  - A user flagged MustChangePassword is routed to the change-password screen first.
    /// </summary>
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session == null) return false;
            SessionUser current = httpContext.Session[App.SessionUserKey] as SessionUser;
            if (!ErpSso.Enabled) return current != null;

            string token = ErpSso.CookieToken(httpContext.Request);
            if (current != null)
            {
                // Central session must still be alive: revalidate at most every Erp:RevalidateSeconds.
                string held = httpContext.Session[ErpSso.TokenSessionKey] as string;
                if (held == null) return true; // signed in locally (Erp disabled at the time / local fallback)
                if (token != held) { httpContext.Session.Clear(); return false; }
                DateTime? checkedAt = httpContext.Session["erp_checked"] as DateTime?;
                int every; if (!int.TryParse(System.Configuration.ConfigurationManager.AppSettings["Erp:RevalidateSeconds"], out every)) every = 60;
                if (checkedAt.HasValue && (DateTime.UtcNow - checkedAt.Value).TotalSeconds < every) return true;
                try
                {
                    if (ErpSso.Validate(token) == null) { httpContext.Session.Clear(); return false; }
                    httpContext.Session["erp_checked"] = DateTime.UtcNow;
                }
                catch (Exception ex) { FileLogger.Error("ERP revalidation failed; keeping local session.", ex); }
                return true;
            }
            if (token == null) return false;
            try
            {
                var id = ErpSso.Validate(token);
                if (id == null) return false;
                return ErpSso.SignIn(httpContext, token, id) != null;
            }
            catch (Exception ex) { FileLogger.Error("ERP single sign-on failed.", ex); return false; }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext.Result != null) return; // not signed in - handled below

            // Authenticated pages must never be served from the browser cache (Back button after sign-out).
            var cache = filterContext.HttpContext.Response.Cache;
            cache.SetCacheability(HttpCacheability.NoCache);
            cache.SetNoStore();
            cache.SetExpires(System.DateTime.UtcNow.AddDays(-1));

            SessionUser user = filterContext.HttpContext.Session[App.SessionUserKey] as SessionUser;
            string controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string action = filterContext.ActionDescriptor.ActionName;

            if (user.MustChangePassword && !(controller == "Account"))
            {
                filterContext.Result = new RedirectResult("~/Account/ChangePassword?forced=1");
                return;
            }

            if (filterContext.HttpContext.Request.HttpMethod == "GET")
            {
                FormInfo form = FormRegistry.ByRoute(controller, action);
                if (form != null && !user.CanView(form.Key))
                    filterContext.Result = Deny(filterContext, form.Title);
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
                filterContext.Result = new HttpStatusCodeResult(401, "Login required");
            else
            {
                string returnUrl = filterContext.HttpContext.Request.RawUrl;
                filterContext.Result = new RedirectResult("~/Account/Login" +
                    (string.IsNullOrEmpty(returnUrl) || returnUrl == "/" ? "" : "?returnUrl=" + HttpUtility.UrlEncode(returnUrl)));
            }
        }

        internal static ActionResult Deny(ControllerContext ctx, string what)
        {
            if (ctx.HttpContext.Request.IsAjaxRequest())
                return new HttpStatusCodeResult(403, "You do not have permission for: " + what);
            ViewResult v = new ViewResult { ViewName = "AccessDenied" };
            v.ViewBag.What = what;
            ctx.HttpContext.Response.StatusCode = 403;
            return v;
        }
    }

    /// <summary>
    /// Declares the permission an action needs beyond being signed in
    /// (use on POST handlers: create/edit/delete/approve).
    /// Form gate (Approval Setup): when the form requires approval for this action and the user does not
    /// hold the Approve right on the form, the write is NOT executed - the request (URL, content type, body
    /// or form fields) is captured into an approval request and the caller gets an "approval pending" answer.
    /// Once approved, an approver replays the captured request with the <c>X-Approval-Replay</c> header
    /// (or <c>__approvalReplay</c> field); the replay runs the action and marks the request applied.
    /// </summary>
    public class RequirePermissionAttribute : ActionFilterAttribute
    {
        readonly string formKey;
        readonly PermissionAction action;

        public RequirePermissionAttribute(string formKey, PermissionAction action)
        {
            this.formKey = formKey;
            this.action = action;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!App.Can(formKey, action))
            {
                FormInfo f = FormRegistry.ByKey(formKey);
                filterContext.Result = AdminAuthorizeAttribute.Deny(filterContext, (f == null ? formKey : f.Title) + " (" + action + ")");
                return;
            }
            var req = filterContext.HttpContext.Request;

            // --- replay of an approved request by an approver ---
            string replay = req.Headers["X-Approval-Replay"] ?? req.Form["__approvalReplay"];
            int replayId;
            if (!string.IsNullOrEmpty(replay) && int.TryParse(replay, out replayId))
            {
                ApprovalRequestModel r = App.Approvals.Get(replayId);
                string problem = r == null ? "Approval request not found." :
                    !r.RequestType.Equals(formKey, StringComparison.OrdinalIgnoreCase) ? "The approval request does not belong to this form." :
                    r.Status != BusinessLayer.Approvals.Approved ? "The request is not approved (" + r.Status + ")." :
                    r.AppliedAt.HasValue ? "This change was already applied on " + r.AppliedAt.Value.ToString("dd MMM yyyy HH:mm") + "." :
                    !App.Can("Security.Approvals", PermissionAction.Approve) ? "Only an approver may apply an approved change." : null;
                if (problem != null) { filterContext.Result = new JsonResult { Data = new { ok = false, message = problem }, JsonRequestBehavior = JsonRequestBehavior.AllowGet }; return; }
                filterContext.HttpContext.Items["approvalReplay"] = r;
                return;
            }

            // --- gate: hold the write for approval ---
            if (action != PermissionAction.Create && action != PermissionAction.Edit && action != PermissionAction.Delete) return;
            if (App.Can(formKey, PermissionAction.Approve)) return;   // approvers of the form write directly
            ApprovalProcessModel gate;
            try { gate = App.Approvals.GateFor(formKey, action.ToString()); }
            catch (Exception ex) { FileLogger.Error("Approval gate lookup failed; letting the write through.", ex); return; }
            if (gate == null) return;

            if (req.Files.Count > 0)
            {
                filterContext.Result = Answer(filterContext, false, "This form requires approval and attachments cannot be held for approval. Ask an approver to enter it.");
                return;
            }

            // capture the request exactly as sent so it can be replayed
            var ser = new System.Web.Script.Serialization.JavaScriptSerializer { MaxJsonLength = int.MaxValue };
            string body = null;
            var form = new Dictionary<string, string[]>();
            bool json = (req.ContentType ?? "").IndexOf("application/json", StringComparison.OrdinalIgnoreCase) >= 0;
            if (json)
            {
                req.InputStream.Position = 0;
                using (var sr = new System.IO.StreamReader(req.InputStream, req.ContentEncoding ?? System.Text.Encoding.UTF8)) body = sr.ReadToEnd();
            }
            else
                foreach (string k in req.Form.AllKeys)
                    if (k != null && k != "__RequestVerificationToken") form[k] = req.Form.GetValues(k);
            string payload = ser.Serialize(new { Url = req.RawUrl, ContentType = json ? "application/json" : "application/x-www-form-urlencoded", Body = body, Form = form, Method = req.HttpMethod });

            FormInfo fi = FormRegistry.ByKey(formKey);
            string formTitle = fi == null ? formKey : fi.Title;
            string title = formTitle + ": " + action.ToString().ToLower();
            string detail = Summarise(json ? body : ser.Serialize(form));
            int id = App.Approvals.Submit(formKey, req.RawUrl, title, detail, App.CurrentUsername, payload);
            App.Log("Create", "Security.Approvals", "Held for approval (#" + id + "): " + title + (string.IsNullOrEmpty(detail) ? "" : " - " + detail));
            string msg = "Sent for approval (request #" + id + "): " + title + ". The change is applied once approved.";
            filterContext.Controller.TempData["Success"] = msg;
            filterContext.Result = Answer(filterContext, true, msg, id);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var r = filterContext.HttpContext.Items["approvalReplay"] as ApprovalRequestModel;
            if (r == null || filterContext.Exception != null) return;
            App.Approvals.MarkApplied(r.id, App.CurrentUsername);
            App.Log("Approve", "Security.Approvals", "Applied approved change #" + r.id + ": " + r.Title + " (requested by " + r.RequestedBy + ")");
        }

        static ActionResult Answer(ControllerContext ctx, bool ok, string message, int id = 0)
        {
            if (ctx.HttpContext.Request.IsAjaxRequest() || (ctx.HttpContext.Request.ContentType ?? "").IndexOf("json", StringComparison.OrdinalIgnoreCase) >= 0)
                return new JsonResult { Data = new { ok = ok, approvalPending = ok, id = id, message = message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            if (!ok) ctx.Controller.TempData["Error"] = message;
            string back = ctx.HttpContext.Request.UrlReferrer == null ? "~/" : ctx.HttpContext.Request.UrlReferrer.ToString();
            return new RedirectResult(back);
        }

        /// <summary>Short human summary of a captured payload: the first few scalar name/value pairs.</summary>
        static string Summarise(string json)
        {
            if (string.IsNullOrEmpty(json)) return "";
            try
            {
                var ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                var parts = new List<string>();
                Walk(ser.DeserializeObject(json), parts, 0);
                string s = string.Join("; ", parts.Take(6).ToArray());
                return s.Length > 300 ? s.Substring(0, 297) + "..." : s;
            }
            catch (Exception) { return ""; }
        }

        static void Walk(object o, List<string> parts, int depth)
        {
            if (parts.Count >= 6 || depth > 3 || o == null) return;
            var d = o as IDictionary<string, object>;
            if (d != null) { foreach (var kv in d) { if (kv.Value is IDictionary<string, object> || kv.Value is object[] || kv.Value is System.Collections.IList) Walk(kv.Value, parts, depth + 1); else if (kv.Value != null && kv.Value.ToString().Trim().Length > 0 && kv.Key.ToLower() != "password" && parts.Count < 6) parts.Add(kv.Key + " = " + Trim(kv.Value.ToString())); } return; }
            var arr = o as System.Collections.IEnumerable;
            if (arr != null && !(o is string)) { int n = 0; foreach (var x in arr) { if (n++ > 0) break; Walk(x, parts, depth + 1); } }
        }

        static string Trim(string s) { s = s.Replace("\r", " ").Replace("\n", " ").Trim(); return s.Length > 40 ? s.Substring(0, 37) + "..." : s; }
    }
}
