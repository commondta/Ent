using B_DB_Context;
using B_DB_Model;
using B_Utility.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HRMS_Web.Controllers.api
{
    // My Home workspace (AI file.xlsx UI sheet / Instructions §4): summary counts
    // for the To-Dos strip. Aggregates only — the detail lives in the Alerts
    // module and the Approval inbox.
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkspaceController : ControllerBase
    {
        private readonly DataBase_Context _db;

        public WorkspaceController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("GetMyHomeSummary")]
        public IActionResult GetMyHomeSummary(int userId)
        {
            try
            {
                int alerts = _db.Notifications
                                .Count(n => n.Receivers.Any(r => r.Receiver == userId.ToString()) && n.IsViewed == false);

                int pendingApprovals = _db.TestApproval
                                          .Count(t => t.UserId == userId
                                                   && t.ApprovalStatus == "Pending"
                                                   && t.IsCancelled != true);

                int activeUsers = _db.PMSUser.Count(u => u.IsActive == true);

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = new { alerts, pendingApprovals, activeUsers }
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetActiveUsers")]
        public IActionResult GetActiveUsers()
        {
            try
            {
                var users = _db.PMSUser
                               .Where(u => u.IsActive == true)
                               .OrderBy(u => u.EMP_FULL_NAME)
                               .Select(u => new { id = u.Id, name = u.EMP_FULL_NAME })
                               .ToList();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = users
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        // Global enterprise search (AI file.xlsx N-6, NetSuite-style): one query
        // across forms (registry) and the core business records. Each group is
        // guarded on its own so one failing entity never kills the search.
        // Extensible: new record groups are added here and rendered generically
        // by the header dropdown.
        [HttpGet]
        [Route("GlobalSearch")]
        public IActionResult GlobalSearch(string q)
        {
            var term = (q ?? "").Trim();
            if (term.Length < 2)
            {
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = new { forms = new object[0], properties = new object[0], members = new object[0], dealers = new object[0] }
                });
            }

            object forms = new object[0], properties = new object[0], members = new object[0], dealers = new object[0];

            var nodes = new List<NavigationNode>();
            var byId = new Dictionary<int, NavigationNode>();
            try
            {
                nodes = _db.NavigationNodes.Where(n => n.IsActive).ToList();
                byId = nodes.ToDictionary(n => n.Id);
            }
            catch { }

            // The form each record group opens in — resolved from the registry so
            // renames follow automatically.
            object FormRef(string route)
            {
                var node = nodes.FirstOrDefault(n => n.NodeType == "Form" &&
                                                     string.Equals(n.Route, route, StringComparison.OrdinalIgnoreCase));
                return new { name = node?.DisplayName ?? route, route = node?.Route ?? route };
            }
            var propertyForm = FormRef("/Home/RegistrationNoProfile");
            var memberForm = FormRef("/Sales/MemberProfile");
            var dealerForm = FormRef("/Sales/DealerProfile");

            try
            {
                forms = nodes
                    .Where(n => n.NodeType == "Form" && n.Route != null &&
                                (n.DisplayName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                                 (n.LegacyName != null && n.LegacyName.Contains(term, StringComparison.OrdinalIgnoreCase))))
                    .Take(8)
                    .Select(n =>
                    {
                        var crumbs = new List<string>();
                        var walk = n.ParentId;
                        while (walk != null && byId.TryGetValue(walk.Value, out var p)) { crumbs.Insert(0, p.DisplayName); walk = p.ParentId; }
                        return new { name = n.DisplayName, route = n.Route, path = string.Join(" › ", crumbs) };
                    })
                    .ToList();
            }
            catch { }

            try
            {
                // profileId deep-links straight into Reference No. Profile when the
                // property has a profile row; without one the hit still opens the form.
                properties = (from s in _db.StockCreations
                              where s.is_active == true &&
                                    ((s.PropertyNo ?? "").Contains(term) || (s.RegistrationNo ?? "").Contains(term))
                              join p in _db.RegistrationNoProfile on s.ID equals p.StockCreationId into pj
                              from p in pj.DefaultIfEmpty()
                              orderby s.PropertyNo
                              select new { id = s.ID, profileId = (int?)p.Id, propertyNo = s.PropertyNo, registrationNo = s.RegistrationNo })
                             .Take(6)
                             .ToList();
            }
            catch { }

            try
            {
                members = _db.MemberProfile
                    .Where(m => m.IsActive &&
                                ((m.MemberName ?? "").Contains(term) || (m.Cnic ?? "").Contains(term)))
                    .OrderBy(m => m.MemberName)
                    .Take(6)
                    .Select(m => new { id = m.Id, name = m.MemberName, cnic = m.Cnic })
                    .ToList();
            }
            catch { }

            try
            {
                dealers = _db.Dealers
                    .Where(d => d.IsActive &&
                                ((d.PrincipalOwner ?? "").Contains(term) ||
                                 (d.EstateName ?? "").Contains(term) ||
                                 (d.CNIC ?? "").Contains(term)))
                    .OrderBy(d => d.PrincipalOwner)
                    .Take(6)
                    .Select(d => new { id = d.Id, name = (d.PrincipalOwner ?? d.EstateName), cnic = d.CNIC })
                    .ToList();
            }
            catch { }

            return Ok(new ApiResponse<object>
            {
                Code = ResponseCode.Success,
                Message = "Success",
                Data = new { forms, properties, members, dealers, propertyForm, memberForm, dealerForm }
            });
        }

        public class SendAlertRequest
        {
            public int ReceiverId { get; set; }
            public string Message { get; set; } = "";
            public string Type { get; set; } = "Non-Critical";
        }

        // Generate Alert (AI file.xlsx N-5): small box over the current screen sends a
        // notification to one user; it lands in their header bell via the existing
        // GetNotificationCount / GetAll endpoints.
        [HttpPost]
        [Route("SendAlert")]
        public IActionResult SendAlert([FromBody] SendAlertRequest request)
        {
            try
            {
                if (request == null || request.ReceiverId <= 0 || string.IsNullOrWhiteSpace(request.Message))
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "A receiver and a message are required",
                        Data = null
                    });
                }

                int senderId = 0;
                var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (claim != null) int.TryParse(claim.Value, out senderId);
                var sender = _db.PMSUser.FirstOrDefault(u => u.Id == senderId);

                var notification = new B_DB_Model.Notification
                {
                    Narration = request.Message.Trim(),
                    Type = request.Type == "Critical" ? "Critical" : "Non-Critical",
                    Sender = senderId,
                    SenderName = sender?.EMP_FULL_NAME,
                    Designation = sender?.DESIG_DESC,
                    IsViewed = false,
                    IsActive = true,
                    CreatedOn = DateTime.Now,
                    LastModified = DateTime.Now,
                    Receivers = new List<B_DB_Model.NotificationReceiver>
                    {
                        new B_DB_Model.NotificationReceiver { Receiver = request.ReceiverId.ToString() }
                    }
                };

                _db.Notifications.Add(notification);
                _db.SaveChanges();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Alert sent",
                    Data = notification.Id
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }
    }
}
