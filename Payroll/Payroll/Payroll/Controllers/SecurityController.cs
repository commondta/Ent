using DataLayer;
using Payroll_HCC.Filters;
using Payroll_HCC.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Payroll_HCC.Controllers
{
    /// <summary>Administration: users, roles &amp; permissions, approvals, activity log.</summary>
    [AdminAuthorize]
    public class SecurityController : Controller
    {
        const string UsersKey = "Security.Users", RolesKey = "Security.Roles", ApprovalsKey = "Security.Approvals", ApprovalSetupKey = "Security.ApprovalSetup", ActivityKey = "Security.ActivityLog";

        // ======================= Users =======================

        public ActionResult Users()
        {
            ViewBag.Users = App.Security.GetUsers();
            ViewBag.Roles = App.Security.GetRoles();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserSave(UserModel model)
        {
            bool isNew = model.id == 0;
            if (!App.Can(UsersKey, isNew ? PermissionAction.Create : PermissionAction.Edit))
                return Json(new { ok = false, message = "You do not have permission to " + (isNew ? "create" : "edit") + " users." });

            if (string.IsNullOrWhiteSpace(model.Username)) return Json(new { ok = false, message = "Username is required." });
            model.Username = model.Username.Trim();
            if (model.Username.Length > 50) return Json(new { ok = false, message = "Username is too long (max 50)." });
            if (App.Security.UsernameExists(model.Username, model.id)) return Json(new { ok = false, message = "That username is already taken." });
            if (!model.RoleId.HasValue) return Json(new { ok = false, message = "Please choose a role." });

            RoleModel role = App.Security.GetRole(model.RoleId.Value);
            if (role == null) return Json(new { ok = false, message = "Unknown role." });
            // Only an administrator may grant the Administrator role.
            if (role.IsSystem && !App.CurrentUser.IsAdministrator) return Json(new { ok = false, message = "Only an administrator can assign the Administrator role." });

            if (isNew)
            {
                string pwdError = AccountController.ValidatePassword(model.Password);
                if (pwdError != null) return Json(new { ok = false, message = pwdError });

                // Approval mechanism: a creator without the Approve permission gets the account created inactive
                // and an approval request is queued; approving it activates the account.
                bool needsApproval = !App.Can(UsersKey, PermissionAction.Approve) && App.Approvals.RequiresApproval("UserAccount");
                model.IsActive = needsApproval ? false : model.IsActive;
                model.MustChangePassword = true;
                int id = App.Security.CreateUser(model, App.CurrentUsername);
                App.Log("Create", UsersKey, "Created user '" + model.Username + "' (" + role.Name + ")" + (needsApproval ? " - awaiting approval" : ""));
                if (needsApproval)
                {
                    App.Approvals.Submit("UserAccount", id.ToString(), "New user account: " + model.Username,
                        "Role: " + role.Name + (string.IsNullOrEmpty(model.FullName) ? "" : "; Name: " + model.FullName), App.CurrentUsername);
                    return Json(new { ok = true, message = "User created and sent for approval. The account stays inactive until approved." });
                }
                return Json(new { ok = true, message = "User created. They must change the password at first sign-in." });
            }

            UserModel existing = App.Security.GetUser(model.id);
            if (existing == null) return Json(new { ok = false, message = "User not found." });
            if (existing.id == App.CurrentUser.id && (!model.IsActive || model.RoleId != existing.RoleId))
                return Json(new { ok = false, message = "You cannot deactivate yourself or change your own role." });
            if (IsLastAdministrator(existing) && (!model.IsActive || role.Name != "Administrator"))
                return Json(new { ok = false, message = "This is the last active administrator; it cannot be demoted or deactivated." });

            App.Security.UpdateUser(model);
            App.Log("Update", UsersKey, "Updated user '" + model.Username + "' (" + role.Name + (model.IsActive ? "" : ", inactive") + ")");
            return Json(new { ok = true, message = "User updated." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequirePermission(UsersKey, PermissionAction.Edit)]
        public ActionResult UserToggle(int id)
        {
            UserModel u = App.Security.GetUser(id);
            if (u == null) return Json(new { ok = false, message = "User not found." });
            if (u.id == App.CurrentUser.id) return Json(new { ok = false, message = "You cannot deactivate your own account." });
            if (u.IsActive && IsLastAdministrator(u)) return Json(new { ok = false, message = "This is the last active administrator." });
            App.Security.SetUserActive(id, !u.IsActive);
            App.Log("Security", UsersKey, (u.IsActive ? "Deactivated" : "Activated") + " user '" + u.Username + "'");
            return Json(new { ok = true, message = "User " + (u.IsActive ? "deactivated." : "activated.") });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequirePermission(UsersKey, PermissionAction.Edit)]
        public ActionResult UserResetPassword(int id, string newPassword)
        {
            UserModel u = App.Security.GetUser(id);
            if (u == null) return Json(new { ok = false, message = "User not found." });
            if (u.RoleName == "Administrator" && !App.CurrentUser.IsAdministrator) return Json(new { ok = false, message = "Only an administrator can reset an administrator's password." });
            string err = AccountController.ValidatePassword(newPassword);
            if (err != null) return Json(new { ok = false, message = err });
            App.Security.ResetPassword(id, newPassword, true);
            App.Log("Security", UsersKey, "Reset password for '" + u.Username + "'");
            return Json(new { ok = true, message = "Password reset. The user must change it at next sign-in." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequirePermission(UsersKey, PermissionAction.Delete)]
        public ActionResult UserDelete(int id)
        {
            UserModel u = App.Security.GetUser(id);
            if (u == null) return Json(new { ok = false, message = "User not found." });
            if (u.id == App.CurrentUser.id) return Json(new { ok = false, message = "You cannot delete your own account." });
            if (IsLastAdministrator(u)) return Json(new { ok = false, message = "This is the last active administrator." });
            App.Security.DeleteUser(id);
            App.Log("Delete", UsersKey, "Deleted user '" + u.Username + "'");
            return Json(new { ok = true, message = "User deleted." });
        }

        bool IsLastAdministrator(UserModel u)
        {
            return u.RoleName == "Administrator" && u.IsActive && App.Security.CountActiveAdministrators() <= 1;
        }

        // ======================= Roles & permissions =======================

        public ActionResult Roles()
        {
            ViewBag.Roles = App.Security.GetRoles();
            ViewBag.Modules = FormRegistry.Modules;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleSave(RoleModel model)
        {
            bool isNew = model.id == 0;
            if (!App.Can(RolesKey, isNew ? PermissionAction.Create : PermissionAction.Edit))
                return Json(new { ok = false, message = "You do not have permission to " + (isNew ? "create" : "edit") + " roles." });
            if (string.IsNullOrWhiteSpace(model.Name)) return Json(new { ok = false, message = "Role name is required." });
            if (App.Security.RoleNameExists(model.Name.Trim(), model.id)) return Json(new { ok = false, message = "A role with that name already exists." });
            if (!isNew)
            {
                RoleModel r = App.Security.GetRole(model.id);
                if (r == null) return Json(new { ok = false, message = "Role not found." });
                if (r.IsSystem) return Json(new { ok = false, message = "System roles cannot be renamed." });
            }
            int id = App.Security.SaveRole(model);
            App.Log(isNew ? "Create" : "Update", RolesKey, (isNew ? "Created" : "Updated") + " role '" + model.Name.Trim() + "'");
            return Json(new { ok = true, id = id, message = "Role saved." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequirePermission(RolesKey, PermissionAction.Delete)]
        public ActionResult RoleDelete(int id)
        {
            RoleModel r = App.Security.GetRole(id);
            if (r == null) return Json(new { ok = false, message = "Role not found." });
            if (!App.Security.DeleteRole(id)) return Json(new { ok = false, message = "System roles and roles that still have users cannot be deleted." });
            App.Log("Delete", RolesKey, "Deleted role '" + r.Name + "'");
            return Json(new { ok = true, message = "Role deleted." });
        }

        [HttpGet]
        public ActionResult RolePermissions(int id)
        {
            RoleModel r = App.Security.GetRole(id);
            if (r == null) return HttpNotFound();
            var perms = App.Security.GetPermissions(id).Select(p => new { p.FormKey, p.CanView, p.CanCreate, p.CanEdit, p.CanDelete, p.CanApprove, p.CanExport, p.CanPrint });
            return Json(new { role = new { r.id, r.Name, r.Description, r.IsSystem }, permissions = perms }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequirePermission(RolesKey, PermissionAction.Edit)]
        public ActionResult RolePermissionsSave(int id, List<PermissionModel> permissions)
        {
            RoleModel r = App.Security.GetRole(id);
            if (r == null) return Json(new { ok = false, message = "Role not found." });
            if (r.IsSystem) return Json(new { ok = false, message = "The Administrator role always has full access; its permissions are not editable." });
            permissions = permissions ?? new List<PermissionModel>();
            // Only registered forms are accepted.
            permissions = permissions.Where(p => FormRegistry.ByKey(p.FormKey) != null).ToList();
            App.Security.SavePermissions(id, permissions);
            App.Log("Security", RolesKey, "Saved permissions for role '" + r.Name + "' (" + permissions.Count(p => p.CanView || p.CanCreate || p.CanEdit || p.CanDelete || p.CanApprove || p.CanExport || p.CanPrint) + " forms)");
            return Json(new { ok = true, message = "Permissions saved. Users of this role see the change at their next sign-in." });
        }

        // ======================= Approvals =======================

        public ActionResult Approvals(string status = "Pending")
        {
            ViewBag.Status = status;
            List<ApprovalRequestModel> requests = App.Approvals.List(status, 300);
            ViewBag.Requests = requests;
            bool canApprove = App.Can(ApprovalsKey, PermissionAction.Approve);
            ViewBag.CanApprove = canApprove;
            // which pending requests THIS user may decide at their current stage (Approval Setup rules)
            var decidable = new HashSet<int>();
            if (canApprove)
                foreach (ApprovalRequestModel r in requests)
                    if (r.Status == BusinessLayer.Approvals.Pending && App.Approvals.CanDecide(r, App.CurrentUser)) decidable.Add(r.id);
            ViewBag.Decidable = decidable;
            ViewBag.CanSetup = App.Can(ApprovalSetupKey, PermissionAction.View);
            return View();
        }

        // ======================= Approval Setup =======================

        public ActionResult ApprovalSetup()
        {
            ViewBag.Processes = App.Approvals.Processes();
            ViewBag.Roles = App.Security.GetRoles();
            ViewBag.Users = App.Security.GetUsers();
            ViewBag.Modules = FormRegistry.Modules;
            return View();
        }

        /// <summary>Form gate: make a registered form an approval process (or change which actions it gates).</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequirePermission(ApprovalSetupKey, PermissionAction.Edit)]
        public ActionResult ApprovalSetupAddForm(string formKey, string actions)
        {
            FormInfo f = FormRegistry.ByKey(formKey ?? "");
            if (f == null) return Json(new { ok = false, message = "Unknown form." });
            if (f.Key.StartsWith("Security.", StringComparison.OrdinalIgnoreCase) || f.Key.StartsWith("Home.", StringComparison.OrdinalIgnoreCase))
                return Json(new { ok = false, message = "Administration and Home screens cannot be put under approval." });
            var wanted = (actions ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim())
                .Where(a => a == "Create" || a == "Edit" || a == "Delete").Distinct().ToList();
            if (wanted.Count == 0) return Json(new { ok = false, message = "Pick at least one action (Create, Edit or Delete)." });
            App.Approvals.SaveFormProcess(f.Key, f.Title, string.Join(",", wanted), App.CurrentUsername);
            App.Log("Security", ApprovalSetupKey, "Form '" + f.Title + "' now requires approval for " + string.Join(", ", wanted));
            return Json(new { ok = true, message = f.Title + " now requires approval for " + string.Join(", ", wanted) + ".", requestType = f.Key });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequirePermission(ApprovalSetupKey, PermissionAction.Edit)]
        public ActionResult ApprovalSetupRemove(string requestType)
        {
            ApprovalProcessModel p = App.Approvals.Process(requestType ?? "");
            if (p == null) return Json(new { ok = false, message = "Unknown approval process." });
            if (!p.IsForm) return Json(new { ok = false, message = "Built-in processes cannot be removed; switch approval off instead." });
            App.Approvals.DeleteFormProcess(p.RequestType);
            App.Log("Security", ApprovalSetupKey, "Form '" + p.Title + "' no longer requires approval");
            return Json(new { ok = true, message = p.Title + " no longer requires approval." });
        }

        /// <summary>Form gate: the captured request of an approved change, for the approver's browser to replay.</summary>
        [HttpGet]
        [RequirePermission(ApprovalsKey, PermissionAction.Approve)]
        public ActionResult ApprovalReplay(int id)
        {
            ApprovalRequestModel r = App.Approvals.Get(id);
            if (r == null || !r.HasPayload) return Json(new { ok = false, message = "Nothing to apply for this request." }, JsonRequestBehavior.AllowGet);
            if (r.Status != BusinessLayer.Approvals.Approved) return Json(new { ok = false, message = "The request is not approved." }, JsonRequestBehavior.AllowGet);
            if (r.AppliedAt.HasValue) return Json(new { ok = false, message = "Already applied on " + r.AppliedAt.Value.ToString("dd MMM yyyy HH:mm") + " by " + r.AppliedBy + "." }, JsonRequestBehavior.AllowGet);
            return Json(new { ok = true, id = r.id, replay = new System.Web.Script.Serialization.JavaScriptSerializer().DeserializeObject(r.Payload) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequirePermission(ApprovalSetupKey, PermissionAction.Edit)]
        public ActionResult ApprovalSetupSave(string requestType, bool isEnabled, string stagesJson, string actions)
        {
            ApprovalProcessModel existing = App.Approvals.Process(requestType);
            if (existing == null) return Json(new { ok = false, message = "Unknown approval process." });
            List<ApprovalStageModel> stages;
            try { stages = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<ApprovalStageModel>>(stagesJson ?? "[]") ?? new List<ApprovalStageModel>(); }
            catch (Exception) { return Json(new { ok = false, message = "The stage list could not be read." }); }
            if (stages.Count == 0) return Json(new { ok = false, message = "Keep at least one stage, or switch approval off for this process." });
            if (stages.Count > 10) return Json(new { ok = false, message = "At most 10 stages." });
            foreach (ApprovalStageModel s in stages)
                if (s.ApproverRoleId.HasValue && App.Security.GetRole(s.ApproverRoleId.Value) == null) return Json(new { ok = false, message = "Unknown role in a stage." });

            string acts = null;
            if (existing.IsForm)
            {
                var wanted = (actions ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim()).Where(a => a == "Create" || a == "Edit" || a == "Delete").Distinct().ToList();
                if (wanted.Count == 0) return Json(new { ok = false, message = "Pick at least one action (Create, Edit or Delete), or remove the form from approval." });
                acts = string.Join(",", wanted);
            }
            App.Approvals.SaveProcess(new ApprovalProcessModel { RequestType = existing.RequestType, IsEnabled = isEnabled, Stages = stages, Actions = acts }, App.CurrentUsername);
            App.Log("Security", ApprovalSetupKey, "Approval setup for '" + existing.Title + "': " + (isEnabled ? "required, " : "OFF, ") + stages.Count + " stage" + (stages.Count == 1 ? "" : "s") +
                " (" + string.Join(" → ", stages.Select(s => (string.IsNullOrWhiteSpace(s.Name) ? "stage" : s.Name.Trim()) + " ×" + Math.Max(1, s.RequiredCount))) + ")");
            return Json(new { ok = true, message = "Approval setup saved.", by = App.CurrentUsername, at = DateTime.Now.ToString("dd MMM yyyy HH:mm") });
        }

        [HttpGet]
        public ActionResult ApprovalDetail(int id)
        {
            ApprovalRequestModel r = App.Approvals.Get(id);
            if (r == null) return HttpNotFound();
            return Json(new
            {
                r.id, r.RequestType, r.ReferenceKey, r.Title, r.Detail, r.RequestedBy, RequestedAt = r.RequestedAt.ToString("dd MMM yyyy HH:mm"),
                r.Status, r.ReviewedBy, ReviewedAt = r.ReviewedAt.HasValue ? r.ReviewedAt.Value.ToString("dd MMM yyyy HH:mm") : null, r.ReviewComment,
                r.ProcessTitle, r.CurrentStage, r.StageCount, r.StageName, r.HasPayload,
                AppliedAt = r.AppliedAt.HasValue ? r.AppliedAt.Value.ToString("dd MMM yyyy HH:mm") : null, r.AppliedBy,
                history = r.History.Select(h => new { h.Action, h.ActionBy, ActionAt = h.ActionAt.ToString("dd MMM yyyy HH:mm"), h.Comment, h.Stage })
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequirePermission(ApprovalsKey, PermissionAction.Approve)]
        public ActionResult ApprovalDecide(int id, string decision, string comment)
        {
            ApprovalRequestModel r = App.Approvals.Get(id);
            if (r == null) return Json(new { ok = false, message = "Request not found." });
            if (r.Status != BusinessLayer.Approvals.Pending) return Json(new { ok = false, message = "This request has already been decided." });
            if (r.RequestedBy.Equals(App.CurrentUsername, StringComparison.OrdinalIgnoreCase) && !App.CurrentUser.IsAdministrator)
                return Json(new { ok = false, message = "You cannot approve your own request." });
            if (!App.Approvals.CanDecide(r, App.CurrentUser))
                return Json(new { ok = false, message = "This request is not waiting on you: stage " + r.CurrentStage + (string.IsNullOrEmpty(r.StageName) ? "" : " (" + r.StageName + ")") + " is assigned to other approvers, or you already approved it." });
            if (decision != BusinessLayer.Approvals.Approved && decision != BusinessLayer.Approvals.Rejected && decision != BusinessLayer.Approvals.Returned)
                return Json(new { ok = false, message = "Unknown decision." });
            if (decision != BusinessLayer.Approvals.Approved && string.IsNullOrWhiteSpace(comment))
                return Json(new { ok = false, message = "Please give a reason when rejecting or returning a request." });

            ApprovalDecisionResult res = App.Approvals.Decide(id, decision, App.CurrentUsername, comment);
            if (!res.Ok) return Json(new { ok = false, message = res.Message });
            if (res.Final) ApplyDecision(r, decision);   // side effects only once the whole process is complete
            App.Log(decision == BusinessLayer.Approvals.Approved ? "Approve" : "Reject", ApprovalsKey,
                (res.Final ? decision : "Stage " + r.CurrentStage + " approval") + ": " + r.Title + (string.IsNullOrWhiteSpace(comment) ? "" : " - " + comment));
            // form gate: hand the approver the captured request so the browser applies it right away
            object replay = null;
            if (res.Final && decision == BusinessLayer.Approvals.Approved && r.HasPayload)
                replay = new System.Web.Script.Serialization.JavaScriptSerializer().DeserializeObject(r.Payload);
            return Json(new { ok = true, message = res.Message, final = res.Final, replayId = replay == null ? 0 : r.id, replay = replay });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApprovalResubmit(int id, string comment)
        {
            if (!App.Approvals.Resubmit(id, App.CurrentUsername, comment)) return Json(new { ok = false, message = "Only your own returned requests can be resubmitted." });
            App.Log("Update", ApprovalsKey, "Resubmitted approval request #" + id);
            return Json(new { ok = true, message = "Request resubmitted." });
        }

        /// <summary>Side effects of a decision, per request type. New request types register their effect here.</summary>
        void ApplyDecision(ApprovalRequestModel r, string decision)
        {
            switch (r.RequestType)
            {
                case "UserAccount":
                    int userId;
                    if (int.TryParse(r.ReferenceKey, out userId))
                    {
                        if (decision == BusinessLayer.Approvals.Approved) App.Security.SetUserActive(userId, true);
                        else if (decision == BusinessLayer.Approvals.Rejected) App.Security.SetUserActive(userId, false);
                    }
                    break;
                // "PayrollRun": the payroll document itself is kept; its approval status is what this queue records.
            }
        }

        // ======================= Activity log =======================

        public ActionResult ActivityLog(DateTime? from, DateTime? to, string user, string act, string q)
        {
            if (!from.HasValue && !to.HasValue && string.IsNullOrEmpty(user) && string.IsNullOrEmpty(act) && string.IsNullOrEmpty(q))
                from = DateTime.Today.AddDays(-7);
            ViewBag.From = from; ViewBag.To = to; ViewBag.User = user; ViewBag.Action = act; ViewBag.Q = q;
            ViewBag.Entries = App.Activity.Query(from, to, user, act, q, 1000);
            ViewBag.Users = App.Security.GetUsers();
            ViewBag.Actions = new[] { "Login", "LoginFailed", "Logout", "Create", "Update", "Delete", "Import", "Process", "Approve", "Reject", "Security" };
            return View();
        }

        /// <summary>Header "Recent activity" dropdown feed.</summary>
        [HttpGet]
        public ActionResult RecentActivity()
        {
            bool all = App.Can(ActivityKey, PermissionAction.View);
            var items = App.Activity.Recent(8, all ? null : App.CurrentUsername)
                .Select(a => new { a.Username, a.Action, a.Module, a.Detail, When = App.TimeAgo(a.OccurredAt), Url = FormUrl(a.FormKey) });
            return Json(new { items, pending = App.Can(ApprovalsKey, PermissionAction.View) ? App.Approvals.PendingCount() : 0 }, JsonRequestBehavior.AllowGet);
        }

        static string FormUrl(string key)
        {
            FormInfo f = FormRegistry.ByKey(key);
            return f == null ? null : System.Web.VirtualPathUtility.ToAbsolute("~" + f.Url);
        }
    }
}
