using System;
using System.Collections.Generic;

namespace DataLayer
{
    public class UserModel
    {
        public int id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public bool MustChangePassword { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        /// <summary>Only used on create / reset; never read back from the database.</summary>
        public string Password { get; set; }
    }

    public class RoleModel
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsSystem { get; set; }
        public int UserCount { get; set; }
    }

    public class PermissionModel
    {
        public int RoleId { get; set; }
        public string FormKey { get; set; }
        public bool CanView { get; set; }
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanApprove { get; set; }
        public bool CanExport { get; set; }
        public bool CanPrint { get; set; }
    }

    public enum PermissionAction { View, Create, Edit, Delete, Approve, Export, Print }

    /// <summary>The authenticated user held in session, with resolved permissions.</summary>
    [Serializable]
    public class SessionUser
    {
        public int id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsAdministrator { get; set; }
        public bool MustChangePassword { get; set; }
        public DateTime LoginAt { get; set; }
        public Dictionary<string, PermissionModel> Permissions { get; set; }

        public SessionUser() { Permissions = new Dictionary<string, PermissionModel>(StringComparer.OrdinalIgnoreCase); }

        public string DisplayName { get { return string.IsNullOrWhiteSpace(FullName) ? Username : FullName; } }

        public string Initials
        {
            get
            {
                string n = DisplayName.Trim();
                if (n.Length == 0) return "?";
                string[] parts = n.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 1) return parts[0].Substring(0, Math.Min(2, parts[0].Length)).ToUpperInvariant();
                return (parts[0].Substring(0, 1) + parts[parts.Length - 1].Substring(0, 1)).ToUpperInvariant();
            }
        }

        public bool Can(string formKey, PermissionAction action)
        {
            if (IsAdministrator) return true;
            PermissionModel p;
            if (formKey == null || !Permissions.TryGetValue(formKey, out p)) return false;
            switch (action)
            {
                case PermissionAction.View: return p.CanView;
                case PermissionAction.Create: return p.CanCreate;
                case PermissionAction.Edit: return p.CanEdit;
                case PermissionAction.Delete: return p.CanDelete;
                case PermissionAction.Approve: return p.CanApprove;
                case PermissionAction.Export: return p.CanExport;
                case PermissionAction.Print: return p.CanPrint;
            }
            return false;
        }

        public bool CanView(string formKey) { return Can(formKey, PermissionAction.View); }
    }

    public class ActivityLogModel
    {
        public long id { get; set; }
        public DateTime OccurredAt { get; set; }
        public string Username { get; set; }
        public string Action { get; set; }     // Login, Logout, Create, Update, Delete, Approve, Reject, Import, Process, Security
        public string Module { get; set; }
        public string FormKey { get; set; }
        public string Detail { get; set; }
        public string IpAddress { get; set; }
    }

    public class ApprovalRequestModel
    {
        public int id { get; set; }
        public string RequestType { get; set; }   // e.g. UserAccount, PayrollRun, RoleChange
        public string ReferenceKey { get; set; }  // e.g. the record id in its own table
        public string Title { get; set; }
        public string Detail { get; set; }
        public string RequestedBy { get; set; }
        public DateTime RequestedAt { get; set; }
        public string Status { get; set; }        // Pending, Approved, Rejected, Returned
        public string ReviewedBy { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string ReviewComment { get; set; }
        public int CurrentStage { get; set; }      // stage the request is waiting on (1-based)
        public int StageCount { get; set; }        // stages configured for its process
        public string StageName { get; set; }      // name of the current stage, if any
        public string ProcessTitle { get; set; }   // display title of the process (Approval Setup)
        public string Payload { get; set; }        // form gate: the captured write request (JSON), replayed on approval
        public DateTime? AppliedAt { get; set; }   // form gate: when the approved change was applied
        public string AppliedBy { get; set; }
        public bool HasPayload { get { return !string.IsNullOrEmpty(Payload); } }
        public List<ApprovalHistoryModel> History { get; set; }
    }

    public class ApprovalHistoryModel
    {
        public int id { get; set; }
        public int RequestId { get; set; }
        public string Action { get; set; }        // Submitted, Approved, Rejected, Returned, Resubmitted, StageComplete
        public int? Stage { get; set; }           // stage the action belongs to
        public string ActionBy { get; set; }
        public DateTime ActionAt { get; set; }
        public string Comment { get; set; }
    }

    /// <summary>One configurable approval process (= a request type) and its stages - Security &amp; Administration → Approval Setup.</summary>
    public class ApprovalProcessModel
    {
        public string RequestType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string FormKey { get; set; }        // form gate: the registered form this process guards (null = built-in process)
        public string Actions { get; set; }        // form gate: comma list of gated actions, e.g. "Create,Edit,Delete"
        public bool IsForm { get { return !string.IsNullOrEmpty(FormKey); } }
        public bool Gates(string action) { return IsEnabled && IsForm && !string.IsNullOrEmpty(Actions) && ("," + Actions.Replace(" ", "") + ",").IndexOf("," + action + ",", StringComparison.OrdinalIgnoreCase) >= 0; }
        public List<ApprovalStageModel> Stages { get; set; }
        public ApprovalProcessModel() { Stages = new List<ApprovalStageModel>(); IsEnabled = true; }
    }

    /// <summary>One stage of an approval process: who may approve and how many approvals it needs.</summary>
    public class ApprovalStageModel
    {
        public int id { get; set; }
        public string RequestType { get; set; }
        public int StageNo { get; set; }
        public string Name { get; set; }
        public int? ApproverRoleId { get; set; }   // null = not restricted to a role
        public string ApproverRoleName { get; set; }
        public int RequiredCount { get; set; }     // approvals needed to complete the stage
        public List<int> UserIds { get; set; }     // named approvers (in addition to the role)
        public List<string> UserNames { get; set; }
        public ApprovalStageModel() { UserIds = new List<int>(); UserNames = new List<string>(); RequiredCount = 1; }
        /// <summary>No role and no named users: any user holding the Approve permission may decide.</summary>
        public bool AnyApprover { get { return !ApproverRoleId.HasValue && UserIds.Count == 0; } }
    }

    /// <summary>Outcome of <c>Approvals.Decide</c>: whether it was recorded and whether the request is now final.</summary>
    public class ApprovalDecisionResult
    {
        public bool Ok { get; set; }
        public bool Final { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
