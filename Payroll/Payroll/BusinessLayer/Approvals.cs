using DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BusinessLayer
{
    /// <summary>
    /// Reusable approval framework (admin database).
    /// Any module submits a request (type + reference key + title). Each request type is an
    /// <b>approval process</b> configured under Security &amp; Administration → Approval Setup:
    /// whether approval is required at all, and one or more stages, each with its approvers
    /// (any user holding the Approve permission, a role, or named users) and how many approvals
    /// the stage needs. A request moves through the stages in order; Rejected / Returned end it
    /// (Returned can be resubmitted, restarting at stage 1). Every step is written to ApprovalHistory.
    /// Statuses: Pending, Approved, Rejected, Returned.
    /// </summary>
    public class Approvals
    {
        public const string Pending = "Pending", Approved = "Approved", Rejected = "Rejected", Returned = "Returned";

        readonly Database db;
        public Approvals(string connectionString) { db = new Database(connectionString); }

        // ======================= Requests =======================

        public int Submit(string requestType, string referenceKey, string title, string detail, string requestedBy)
        {
            return Submit(requestType, referenceKey, title, detail, requestedBy, null);
        }

        /// <summary>Submit with a captured write request (form gate) that is replayed once the request is approved.</summary>
        public int Submit(string requestType, string referenceKey, string title, string detail, string requestedBy, string payload)
        {
            DataTable dt = db.Get(@"INSERT INTO ApprovalRequest(RequestType, ReferenceKey, Title, Detail, RequestedBy, CurrentStage, Payload)
                                    VALUES(@t, @k, @ti, @d, @b, 1, @p); SELECT SCOPE_IDENTITY()",
                new SqlParameter("@t", requestType), new SqlParameter("@k", (object)referenceKey ?? DBNull.Value),
                new SqlParameter("@ti", title), new SqlParameter("@d", (object)detail ?? DBNull.Value), new SqlParameter("@b", requestedBy),
                new SqlParameter("@p", (object)payload ?? DBNull.Value));
            int id = Convert.ToInt32(dt.Rows[0][0]);
            AddHistory(id, "Submitted", requestedBy, null, 1);
            return id;
        }

        /// <summary>Puts a Returned request back into the queue, at stage 1.</summary>
        public bool Resubmit(int id, string by, string comment)
        {
            DataTable dt = db.Get(@"UPDATE ApprovalRequest SET Status='Pending', CurrentStage=1, ReviewedBy=NULL, ReviewedAt=NULL, ReviewComment=NULL
                                    WHERE id=@id AND Status='Returned' AND RequestedBy=@b; SELECT @@ROWCOUNT",
                new SqlParameter("@id", id), new SqlParameter("@b", by));
            if (Convert.ToInt32(dt.Rows[0][0]) != 1) return false;
            AddHistory(id, "Resubmitted", by, comment, 1);
            return true;
        }

        public int PendingCount()
        {
            return Convert.ToInt32(db.Get("SELECT COUNT(*) FROM ApprovalRequest WHERE Status='Pending'").Rows[0][0]);
        }

        public ApprovalRequestModel Get(int id)
        {
            List<ApprovalRequestModel> l = ToList(db.Get("SELECT * FROM ApprovalRequest WHERE id=@id", new SqlParameter("@id", id)));
            if (l.Count == 0) return null;
            l[0].History = History(id);
            return l[0];
        }

        public List<ApprovalRequestModel> List(string status, int max)
        {
            if (string.IsNullOrEmpty(status) || status == "All")
                return ToList(db.Get("SELECT TOP (@n) * FROM ApprovalRequest ORDER BY CASE Status WHEN 'Pending' THEN 0 ELSE 1 END, id DESC", new SqlParameter("@n", max)));
            return ToList(db.Get("SELECT TOP (@n) * FROM ApprovalRequest WHERE Status=@s ORDER BY id DESC", new SqlParameter("@n", max), new SqlParameter("@s", status)));
        }

        public List<ApprovalRequestModel> MyRequests(string username, int max)
        {
            return ToList(db.Get("SELECT TOP (@n) * FROM ApprovalRequest WHERE RequestedBy=@u ORDER BY id DESC", new SqlParameter("@n", max), new SqlParameter("@u", username)));
        }

        public bool HasPending(string requestType, string referenceKey)
        {
            return Convert.ToInt32(db.Get("SELECT COUNT(*) FROM ApprovalRequest WHERE RequestType=@t AND ReferenceKey=@k AND Status='Pending'",
                new SqlParameter("@t", requestType), new SqlParameter("@k", referenceKey)).Rows[0][0]) > 0;
        }

        /// <summary>
        /// May this user decide the request at its current stage? Requires the Approve permission (checked by the
        /// caller) plus the stage's approver rule: any approver, the configured role, or one of the named users.
        /// Administrators always may. A user who already approved the current stage may not approve it twice.
        /// </summary>
        public bool CanDecide(ApprovalRequestModel r, SessionUser user)
        {
            if (r == null || user == null || r.Status != Pending) return false;
            if (r.RequestedBy.Equals(user.Username, StringComparison.OrdinalIgnoreCase) && !user.IsAdministrator) return false;
            if (ApprovedThisStageBy(r.id, r.CurrentStage).Contains(user.Username, StringComparer.OrdinalIgnoreCase)) return false;
            if (user.IsAdministrator) return true;
            ApprovalStageModel stage = Stage(r.RequestType, r.CurrentStage);
            if (stage == null || stage.AnyApprover) return true;
            if (stage.ApproverRoleId.HasValue && user.RoleId == stage.ApproverRoleId) return true;
            return stage.UserIds.Contains(user.id);
        }

        /// <summary>
        /// Records a decision. Rejected / Returned end the request. Approved counts towards the current stage;
        /// when the stage has enough approvals the request advances to the next stage, or completes when it
        /// was the last one. <see cref="ApprovalDecisionResult.Final"/> tells the caller whether side effects apply.
        /// </summary>
        public ApprovalDecisionResult Decide(int id, string decision, string reviewedBy, string comment)
        {
            if (decision != Approved && decision != Rejected && decision != Returned) throw new ArgumentException("Unknown decision.", "decision");
            ApprovalRequestModel r = Get(id);
            if (r == null || r.Status != Pending) return new ApprovalDecisionResult { Ok = false, Message = "This request has already been decided." };

            if (decision != Approved)
            {
                if (!Finish(id, decision, reviewedBy, comment)) return new ApprovalDecisionResult { Ok = false, Message = "Could not update the request." };
                AddHistory(id, decision, reviewedBy, comment, r.CurrentStage);
                return new ApprovalDecisionResult { Ok = true, Final = true, Status = decision, Message = "Request " + decision.ToLower() + "." };
            }

            AddHistory(id, Approved, reviewedBy, comment, r.CurrentStage);
            ApprovalStageModel stage = Stage(r.RequestType, r.CurrentStage);
            int required = stage == null ? 1 : Math.Max(1, stage.RequiredCount);
            int have = ApprovedThisStageBy(id, r.CurrentStage).Count;
            int stages = StageCount(r.RequestType);
            string stageLabel = "stage " + r.CurrentStage + (stages > 1 ? " of " + stages : "") + (stage != null && !string.IsNullOrEmpty(stage.Name) ? " (" + stage.Name + ")" : "");

            if (have < required)
                return new ApprovalDecisionResult { Ok = true, Final = false, Status = Pending, Message = "Approval recorded: " + have + " of " + required + " for " + stageLabel + "." };

            if (r.CurrentStage < stages)
            {
                db.Set("UPDATE ApprovalRequest SET CurrentStage=@s WHERE id=@id AND Status='Pending'", new SqlParameter("@s", r.CurrentStage + 1), new SqlParameter("@id", id));
                AddHistory(id, "StageComplete", reviewedBy, stageLabel + " complete; moved to stage " + (r.CurrentStage + 1), r.CurrentStage);
                return new ApprovalDecisionResult { Ok = true, Final = false, Status = Pending, Message = "Stage " + r.CurrentStage + " approved; the request moves to stage " + (r.CurrentStage + 1) + " of " + stages + "." };
            }

            if (!Finish(id, Approved, reviewedBy, comment)) return new ApprovalDecisionResult { Ok = false, Message = "Could not update the request." };
            return new ApprovalDecisionResult { Ok = true, Final = true, Status = Approved, Message = "Request approved." };
        }

        bool Finish(int id, string status, string reviewedBy, string comment)
        {
            DataTable dt = db.Get(@"UPDATE ApprovalRequest SET Status=@s, ReviewedBy=@r, ReviewedAt=GETDATE(), ReviewComment=@c
                                    WHERE id=@id AND Status='Pending'; SELECT @@ROWCOUNT",
                new SqlParameter("@s", status), new SqlParameter("@r", reviewedBy),
                new SqlParameter("@c", (object)comment ?? DBNull.Value), new SqlParameter("@id", id));
            return Convert.ToInt32(dt.Rows[0][0]) == 1;
        }

        List<string> ApprovedThisStageBy(int requestId, int stage)
        {
            // approvals since the last (re)submission only, so a returned-and-resubmitted request starts clean
            DataTable dt = db.Get(@"SELECT DISTINCT ActionBy FROM ApprovalHistory h
                                    WHERE h.RequestId=@id AND h.Action='Approved' AND ISNULL(h.Stage,1)=@s
                                      AND h.id > ISNULL((SELECT MAX(id) FROM ApprovalHistory WHERE RequestId=@id AND Action IN ('Submitted','Resubmitted')), 0)",
                new SqlParameter("@id", requestId), new SqlParameter("@s", stage));
            return dt.Rows.Cast<DataRow>().Select(x => x[0].ToString()).ToList();
        }

        public List<ApprovalHistoryModel> History(int requestId)
        {
            DataTable dt = db.Get("SELECT * FROM ApprovalHistory WHERE RequestId=@id ORDER BY id", new SqlParameter("@id", requestId));
            List<ApprovalHistoryModel> list = new List<ApprovalHistoryModel>();
            foreach (DataRow r in dt.Rows)
                list.Add(new ApprovalHistoryModel
                {
                    id = Convert.ToInt32(r["id"]),
                    RequestId = requestId,
                    Action = r["Action"].ToString(),
                    ActionBy = r["ActionBy"].ToString(),
                    ActionAt = Convert.ToDateTime(r["ActionAt"]),
                    Comment = r["Comment"].ToString(),
                    Stage = r["Stage"] == DBNull.Value ? (int?)null : Convert.ToInt32(r["Stage"])
                });
            return list;
        }

        void AddHistory(int requestId, string action, string by, string comment, int? stage)
        {
            db.Set("INSERT INTO ApprovalHistory(RequestId, Action, ActionBy, Comment, Stage) VALUES(@r, @a, @b, @c, @s)",
                new SqlParameter("@r", requestId), new SqlParameter("@a", action), new SqlParameter("@b", by ?? "system"),
                new SqlParameter("@c", (object)comment ?? DBNull.Value), new SqlParameter("@s", (object)stage ?? DBNull.Value));
        }

        List<ApprovalRequestModel> ToList(DataTable dt)
        {
            List<ApprovalRequestModel> list = new List<ApprovalRequestModel>();
            Dictionary<string, ApprovalProcessModel> procs = null;
            foreach (DataRow r in dt.Rows)
            {
                if (procs == null) procs = Processes().ToDictionary(x => x.RequestType, StringComparer.OrdinalIgnoreCase);
                ApprovalRequestModel m = new ApprovalRequestModel
                {
                    id = Convert.ToInt32(r["id"]),
                    RequestType = r["RequestType"].ToString(),
                    ReferenceKey = r["ReferenceKey"].ToString(),
                    Title = r["Title"].ToString(),
                    Detail = r["Detail"].ToString(),
                    RequestedBy = r["RequestedBy"].ToString(),
                    RequestedAt = Convert.ToDateTime(r["RequestedAt"]),
                    Status = r["Status"].ToString(),
                    ReviewedBy = r["ReviewedBy"].ToString(),
                    ReviewedAt = r["ReviewedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["ReviewedAt"]),
                    ReviewComment = r["ReviewComment"].ToString(),
                    CurrentStage = r["CurrentStage"] == DBNull.Value ? 1 : Convert.ToInt32(r["CurrentStage"]),
                    Payload = r["Payload"].ToString(),
                    AppliedAt = r["AppliedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["AppliedAt"]),
                    AppliedBy = r["AppliedBy"].ToString()
                };
                ApprovalProcessModel p;
                if (procs.TryGetValue(m.RequestType, out p))
                {
                    m.ProcessTitle = p.Title;
                    m.StageCount = Math.Max(1, p.Stages.Count);
                    ApprovalStageModel s = p.Stages.FirstOrDefault(x => x.StageNo == m.CurrentStage);
                    m.StageName = s == null ? null : s.Name;
                }
                else { m.ProcessTitle = m.RequestType; m.StageCount = 1; }
                list.Add(m);
            }
            return list;
        }

        // ======================= Processes (Approval Setup) =======================

        /// <summary>Does this request type currently require approval? Unknown types do (safe default).</summary>
        public bool RequiresApproval(string requestType)
        {
            DataTable dt = db.Get("SELECT IsEnabled FROM ApprovalProcess WHERE RequestType=@t", new SqlParameter("@t", requestType));
            return dt.Rows.Count == 0 || Convert.ToBoolean(dt.Rows[0][0]);
        }

        public List<ApprovalProcessModel> Processes()
        {
            List<ApprovalProcessModel> list = new List<ApprovalProcessModel>();
            foreach (DataRow r in db.Get("SELECT * FROM ApprovalProcess ORDER BY Title").Rows)
                list.Add(new ApprovalProcessModel
                {
                    RequestType = r["RequestType"].ToString(),
                    Title = r["Title"].ToString(),
                    Description = r["Description"].ToString(),
                    IsEnabled = Convert.ToBoolean(r["IsEnabled"]),
                    UpdatedBy = r["UpdatedBy"].ToString(),
                    UpdatedAt = r["UpdatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["UpdatedAt"]),
                    FormKey = r["FormKey"].ToString(),
                    Actions = r["Actions"].ToString()
                });
            if (list.Count == 0) return list;

            Dictionary<int, ApprovalStageModel> byId = new Dictionary<int, ApprovalStageModel>();
            foreach (DataRow r in db.Get(@"SELECT s.*, ro.Name AS RoleName FROM ApprovalStage s LEFT JOIN Role ro ON ro.id = s.ApproverRoleId ORDER BY s.RequestType, s.StageNo").Rows)
            {
                ApprovalStageModel s = new ApprovalStageModel
                {
                    id = Convert.ToInt32(r["id"]),
                    RequestType = r["RequestType"].ToString(),
                    StageNo = Convert.ToInt32(r["StageNo"]),
                    Name = r["Name"].ToString(),
                    ApproverRoleId = r["ApproverRoleId"] == DBNull.Value ? (int?)null : Convert.ToInt32(r["ApproverRoleId"]),
                    ApproverRoleName = r["RoleName"].ToString(),
                    RequiredCount = Math.Max(1, Convert.ToInt32(r["RequiredCount"]))
                };
                byId[s.id] = s;
                ApprovalProcessModel p = list.FirstOrDefault(x => x.RequestType.Equals(s.RequestType, StringComparison.OrdinalIgnoreCase));
                if (p != null) p.Stages.Add(s);
            }
            foreach (DataRow r in db.Get(@"SELECT su.StageId, su.UserId, a.Username, a.FullName FROM ApprovalStageUser su JOIN Account a ON a.id = su.UserId ORDER BY a.Username").Rows)
            {
                ApprovalStageModel s;
                if (!byId.TryGetValue(Convert.ToInt32(r["StageId"]), out s)) continue;
                s.UserIds.Add(Convert.ToInt32(r["UserId"]));
                string full = r["FullName"].ToString();
                s.UserNames.Add(string.IsNullOrWhiteSpace(full) ? r["Username"].ToString() : full);
            }
            return list;
        }

        public ApprovalProcessModel Process(string requestType)
        {
            return Processes().FirstOrDefault(p => p.RequestType.Equals(requestType, StringComparison.OrdinalIgnoreCase));
        }

        ApprovalStageModel Stage(string requestType, int stageNo)
        {
            ApprovalProcessModel p = Process(requestType);
            return p == null ? null : p.Stages.FirstOrDefault(s => s.StageNo == stageNo);
        }

        int StageCount(string requestType)
        {
            ApprovalProcessModel p = Process(requestType);
            return p == null ? 1 : Math.Max(1, p.Stages.Count);
        }

        /// <summary>
        /// Saves a process's switch and its stages (replacing the existing stages). At least one stage is kept;
        /// stage numbers are renumbered 1..n in the order given; RequiredCount is clamped to 1..99.
        /// Pending requests keep their CurrentStage; a stage that no longer exists is treated as the last one.
        /// </summary>
        public bool SaveProcess(ApprovalProcessModel p, string by)
        {
            if (p == null || string.IsNullOrEmpty(p.RequestType)) return false;
            if (db.Get("SELECT COUNT(*) FROM ApprovalProcess WHERE RequestType=@t", new SqlParameter("@t", p.RequestType)).Rows[0][0].ToString() == "0") return false;
            List<ApprovalStageModel> stages = (p.Stages ?? new List<ApprovalStageModel>()).ToList();
            if (stages.Count == 0) stages.Add(new ApprovalStageModel { Name = "Approval", RequiredCount = 1 });

            db.Set("UPDATE ApprovalProcess SET IsEnabled=@e, Actions=COALESCE(@a, Actions), UpdatedBy=@b, UpdatedAt=GETDATE() WHERE RequestType=@t",
                new SqlParameter("@e", p.IsEnabled), new SqlParameter("@a", (object)p.Actions ?? DBNull.Value), new SqlParameter("@b", by ?? "system"), new SqlParameter("@t", p.RequestType));
            db.Set("DELETE FROM ApprovalStage WHERE RequestType=@t", new SqlParameter("@t", p.RequestType));
            int n = 0;
            foreach (ApprovalStageModel s in stages)
            {
                n++;
                string name = string.IsNullOrWhiteSpace(s.Name) ? "Stage " + n : s.Name.Trim();
                if (name.Length > 100) name = name.Substring(0, 100);
                DataTable dt = db.Get(@"INSERT INTO ApprovalStage(RequestType, StageNo, Name, ApproverRoleId, RequiredCount) VALUES(@t, @n, @nm, @r, @c); SELECT SCOPE_IDENTITY()",
                    new SqlParameter("@t", p.RequestType), new SqlParameter("@n", n), new SqlParameter("@nm", name),
                    new SqlParameter("@r", (object)s.ApproverRoleId ?? DBNull.Value), new SqlParameter("@c", Math.Min(99, Math.Max(1, s.RequiredCount))));
                int stageId = Convert.ToInt32(dt.Rows[0][0]);
                foreach (int uid in (s.UserIds ?? new List<int>()).Distinct())
                    db.Set("INSERT INTO ApprovalStageUser(StageId, UserId) SELECT @s, id FROM Account WHERE id=@u", new SqlParameter("@s", stageId), new SqlParameter("@u", uid));
            }
            // requests sitting on a stage that was removed continue on the new last stage
            db.Set("UPDATE ApprovalRequest SET CurrentStage=@n WHERE RequestType=@t AND Status='Pending' AND CurrentStage>@n", new SqlParameter("@n", n), new SqlParameter("@t", p.RequestType));
            return true;
        }

        // ======================= Form gate =======================

        /// <summary>The enabled process that gates <paramref name="action"/> on this form, or null when the form is not gated.</summary>
        public ApprovalProcessModel GateFor(string formKey, string action)
        {
            if (string.IsNullOrEmpty(formKey)) return null;
            ApprovalProcessModel p = Process(formKey);
            return p != null && p.Gates(action) ? p : null;
        }

        /// <summary>Registers (or updates) a form as an approval process. RequestType = the form key.</summary>
        public void SaveFormProcess(string formKey, string title, string actions, string by)
        {
            if (db.Get("SELECT COUNT(*) FROM ApprovalProcess WHERE RequestType=@t", new SqlParameter("@t", formKey)).Rows[0][0].ToString() == "0")
            {
                db.Set(@"INSERT INTO ApprovalProcess(RequestType, Title, Description, IsEnabled, FormKey, Actions, UpdatedBy, UpdatedAt)
                         VALUES(@t, @ti, @d, 1, @t, @a, @b, GETDATE())",
                    new SqlParameter("@t", formKey), new SqlParameter("@ti", title),
                    new SqlParameter("@d", "Changes on the " + title + " form (" + actions.Replace(",", ", ") + ") are held until approved, unless the user holds the Approve right on that form."),
                    new SqlParameter("@a", actions), new SqlParameter("@b", by ?? "system"));
                db.Set("INSERT INTO ApprovalStage(RequestType, StageNo, Name) VALUES(@t, 1, 'Approval')", new SqlParameter("@t", formKey));
            }
            else
                db.Set("UPDATE ApprovalProcess SET Actions=@a, IsEnabled=1, UpdatedBy=@b, UpdatedAt=GETDATE() WHERE RequestType=@t",
                    new SqlParameter("@a", actions), new SqlParameter("@b", by ?? "system"), new SqlParameter("@t", formKey));
        }

        /// <summary>Removes a form process (built-in processes cannot be removed). Its stages go with it; requests stay.</summary>
        public bool DeleteFormProcess(string formKey)
        {
            DataTable dt = db.Get("DELETE FROM ApprovalProcess WHERE RequestType=@t AND FormKey IS NOT NULL; SELECT @@ROWCOUNT", new SqlParameter("@t", formKey));
            return Convert.ToInt32(dt.Rows[0][0]) == 1;
        }

        /// <summary>Marks an approved form-gate request as applied (its captured write was replayed).</summary>
        public void MarkApplied(int id, string by)
        {
            db.Set("UPDATE ApprovalRequest SET AppliedAt=GETDATE(), AppliedBy=@b WHERE id=@id AND AppliedAt IS NULL", new SqlParameter("@b", by ?? "system"), new SqlParameter("@id", id));
            AddHistory(id, "Applied", by, "approved change applied", null);
        }
    }
}
