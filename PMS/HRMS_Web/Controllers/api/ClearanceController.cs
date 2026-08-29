using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using B_Utility.Common.Enums;
using HRMS_Web.Models.DTOs;
using HRMS_Web.Services.AlertService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClearanceController : ControllerBase
    {
        private readonly DataBase_Context _db;
        private readonly IAlertService _alertService;
        ApprovalBLL _approvalBLL;
        public ClearanceController(DataBase_Context db, IAlertService alertService)
        {
            _db = db;
            _alertService = alertService;
            _approvalBLL = new ApprovalBLL(_db);
        }

        [HttpGet]
        [Route("GetAllClearnceFilterList")]
        public IActionResult GetAllClearnceFilterList()
        {
            try
            {
                var newDemarcationfilterList = (from stock in _db.StockCreations
                                                     .Include(x => x.MemberProfile)
                                                     .Where(x => x.is_active == true
                                                      && x.MemberProfileId != null
                                                      && x.Is_DemarcationApproved == true
                                                      && x.Is_ClearnceRequested != true
                                                      )
                                                join dr in _db.NewDemarcationRequest
                                                on stock.ID equals dr.StockCreationId
                                                select new ClearnceFilterDTO
                                                {
                                                    ID = stock.ID,
                                                    RegistrationNo = stock.RegistrationNo,
                                                    PropertyNo = stock.PropertyNo,
                                                    MemberCode = stock.MemberProfile.Id,
                                                    MemberName = stock.MemberProfile.MemberName,
                                                    CNIC = stock.MemberProfile.Cnic,
                                                    RedesignRequest = dr.IsCancelled == true ? "Yes" : "No",
                                                })
                                                .Distinct()
                                                .ToList() ?? new List<ClearnceFilterDTO>();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = newDemarcationfilterList
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("AddClearnceRequest")]
        public IActionResult AddClearnceRequest(int requestAction, int stockId, string redesignRequest)
        {
            try
            {
                bool isApprovalActive = true;

                var approvalStatus = _db.ApprovalUI.Find((int)ApprovalUIIds.ClearanceForm);
                if (approvalStatus != null)
                {
                    if (approvalStatus.Checked != true)
                    {
                        isApprovalActive = false;
                    }
                }

                var approvalSetup = _db.ApprovalSetup.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.ClearanceForm).ToList();
                if (approvalSetup.Count <= 0 && isApprovalActive == true)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Approval setup not defined or In-active",
                        Data = null
                    });
                }

                string message = String.Empty;

                var stockCreation = _db.StockCreations.Where(x => x.ID == stockId)
                                                      .FirstOrDefault();
                if (stockCreation != null)
                {
                    if (requestAction == (int)ClearnceAction.ClearnceRequest)
                    {
                        stockCreation.Is_DemarcationApproved = true;
                        stockCreation.Is_ClearnceRequested = true;
                        stockCreation.ClearanceDate = DateTime.Now;
                        stockCreation.ClearanceOn = DateTime.Now;

                        _db.SaveChanges();

                        string narration = $"The Registraion No. {stockCreation.RegistrationNo} clearance request submitted from Building Control please proceed it";
                        _alertService.PushAlert(7, narration);

                        if (redesignRequest == "Yes")
                        {
                            stockCreation.Is_MapApprovalRequested = true;
                            stockCreation.Is_MapApprovalApproved = true;
                        }

                        if (isApprovalActive == true && redesignRequest == "No")
                        {
                            bool request = _approvalBLL.AddNewApprovalSetup(stockCreation.ID, (int)ApprovalUIIds.ClearanceForm);
                            message = "Clearnce request submit successfully and move for approval";
                            if (request)
                            {
                                return Ok(new ApiResponse<object>
                                {
                                    Code = ResponseCode.Success,
                                    Message = message,
                                });
                            }
                        }
                        else
                        {
                            stockCreation.Is_ClearnceApproved = true;
                            stockCreation.ClearanceOn = DateTime.Now;
                            _db.SaveChanges();



                            message = "Clearnce request submit successfully";

                            return Ok(new ApiResponse<object>
                            {
                                Code = ResponseCode.Success,
                                Message = message,
                            });
                        }
                    }

                    if (requestAction == (int)ClearnceAction.SendNotificationToSAP)
                    {
                        // SendNotificationToSAP
                        message = "Clearnce Notification Sent To SAP Successfully";
                    }
                    if (requestAction == (int)ClearnceAction.SendNotificationToMember)
                    {
                        // SendNotificationToMember
                        message = "Clearnce Notification Sent To Member Successfully";
                    }

                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = message,
                    //Data = response
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }
    }
}
