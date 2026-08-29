using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using B_Utility.Common.Enums;
using HRMS_Web.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;
using System.Web.Http.Results;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApprovalsController : ControllerBase
    {
        private readonly DataBase_Context _db;

        CommonBLL _commonBLL;

        public ApprovalsController(DataBase_Context db)
        {
            _db = db;
            _commonBLL = new CommonBLL(_db);
        }

        // Header badge: how many approval requests are waiting on this user
        // right now — assigned to them in the current stage and not yet actioned.
        [HttpGet]
        [Route("GetPendingApprovalCount")]
        public IActionResult GetPendingApprovalCount(int userId)
        {
            try
            {
                var count = _db.TestApproval.Count(x => x.UserId == userId
                                                     && x.Is_Assigned == true
                                                     && x.ApprovalStatus == "Pending");

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = count
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetAllApprovalUI")]
        public IActionResult GetAllApprovalUI()
        {
            try
            {

                var result = _db.ApprovalUI.Where(x => !x.IsDeleted)
                                           .ToList();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetAllActiveApprovalUIDropdown")]
        public IActionResult GetAllActiveApprovalUIDropdown()
        {
            try
            {

                var result = _db.ApprovalUI.Where(x => !x.IsDeleted && x.Checked == true)
                                           .ToList();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        [Route("ActiveApprovalUI")]
        public IActionResult ActiveApprovalUI(List<ActiveApprovalUI> dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                    });
                }

                foreach (var item in dto)
                {
                    var model = _db.ApprovalUI.Where(s => s.SerialNo == item.SerialNo && s.IsDeleted == false).FirstOrDefault();

                    if (model != null)
                    {
                        model.Checked = item.Checked;
                        model.LastModified = DateTime.Now;
                        model.ModifiedBy = 1;

                        _db.SaveChanges();
                    }
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = dto
                });
            }

            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetApprovalSetupByDocumentId")]
        public IActionResult GetApprovalSetupByDocumentId(int documentId, int stageNo)
        {
            try
            {

                var result = _db.ApprovalSetup.Where(x => !x.IsDeleted && x.ApprovalUIId == documentId && x.StageNo == stageNo)
                                              .Include(x => x.ApprovalUsers)
                                              .FirstOrDefault();
                if (result== null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Not Found",
                        Data = result
                        
                    });
                }
                else
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
                }
              
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        // AddNewApprovalSetup add new approval user against document and stageNo
        [HttpPost]
        [Route("AddNewApprovalSetup")]
        public IActionResult AddNewApprovalSetup(ApprovalSetup model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                    });
                }

                model.CreatedOn = DateTime.Now;
                model.CreatedBy = model.CreatedBy;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;
                model.IsActive = true;
                model.IsDeleted = false;

                if (model.ApprovalUsers?.Count > 0)
                {
                    foreach (var item in model.ApprovalUsers)
                    {
                        item.CreatedOn = DateTime.Now;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModified = DateTime.Now;
                        item.ModifiedBy = model.ModifiedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                _db.ApprovalSetup.Add(model);
                _db.SaveChanges();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = model
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        // UpdateApprovalSetup update approval user against document and stageNo
        [HttpPost]
        [Route("UpdateApprovalSetup")]
        public IActionResult UpdateApprovalSetup(ApprovalSetup model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                    });
                }

                var existingApprovalSetup = _db.ApprovalSetup.Include(x => x.ApprovalUsers).Where(x => x.Id == model.Id).FirstOrDefault();

                if (existingApprovalSetup != null)
                {
                    existingApprovalSetup.NumberOfApprovalRequired = model.NumberOfApprovalRequired;
                    existingApprovalSetup.LastModified = DateTime.Now;
                    existingApprovalSetup.ModifiedBy = model.ModifiedBy;
                    existingApprovalSetup.LastModifiedUserName = model.LastModifiedUserName;


                    _db.SaveChanges();
                }

                if (existingApprovalSetup?.ApprovalUsers?.Count > 0)
                {
                    foreach (var user in existingApprovalSetup.ApprovalUsers)
                    {
                        _db.ApprovalUsers.Remove(user);
                    }
                    // _db.ApprovalUsers.RemoveRange(existingApprovalSetup.ApprovalUsers);
                    _db.SaveChanges();
                }

                if (model.ApprovalUsers?.Count > 0)
                {
                    foreach (var item in model.ApprovalUsers)
                    {
                        ApprovalUsers approvalUser = new ApprovalUsers();
                        {
                            approvalUser.ApprovalSetupId = model.Id;
                            approvalUser.UserId = item.UserId;
                            approvalUser.UserDesignation = item.UserDesignation;
                            approvalUser.CreatedOn = DateTime.Now;
                            approvalUser.LastModified = DateTime.Now;
                            approvalUser.ModifiedBy = model.ModifiedBy;
                            approvalUser.LastModifiedUserName = model.LastModifiedUserName;
                            approvalUser.IsActive = true;
                            approvalUser.IsDeleted = false;
                            _db.Add(approvalUser);
                            _db.SaveChanges();
                        }
                    }
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = model
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        //AddRequestApprovalSetup new request adding and automatically configure with stages and users
        [HttpPost]
        [Route("AddRequestApprovalSetup")]
        public IActionResult AddNewApprovalSetup(AUAprrovalRequestDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                    });
                }

                var approvalSetup = _db.ApprovalSetup.Where(x => x.ApprovalUIId == dto.ApprovalUIId).ToList();

                if (approvalSetup.Count > 0)
                {
                    foreach (var approvalUIIdItem in approvalSetup)
                    {
                        var approvalUsers = _db.ApprovalUsers.Where(x => x.ApprovalSetupId == approvalUIIdItem.Id && x.IsActive == true).ToList();

                        if (approvalUsers.Count > 0)
                        {
                            foreach (var user in approvalUsers)
                            {
                                TestApproval testApproval = new TestApproval();
                                {
                                    testApproval.RequestId = dto.RequestId;
                                    testApproval.ApprovalUIId = approvalUIIdItem.ApprovalUIId;
                                    testApproval.ApprovalSetupId = approvalUIIdItem.Id;
                                    testApproval.StageNo = approvalUIIdItem.StageNo;
                                    testApproval.NumberOfApprovalRequired = approvalUIIdItem.NumberOfApprovalRequired;
                                    testApproval.UserId = user.UserId;
                                    testApproval.UserDesignation = user.UserDesignation;
                                    testApproval.ModifiedBy = user.ModifiedBy;
                                    testApproval.LastModifiedUserName = user.LastModifiedUserName;
                                    testApproval.ApprovalStatus = UHelper.ApprovalStatus(1);
                                    testApproval.CreatedOn = DateTime.Now;
                                    testApproval.IsActive = true;
                                    testApproval.IsDeleted = false;

                                    _db.TestApproval.Add(testApproval);
                                    _db.SaveChanges();
                                }
                            }
                        }
                    }

                    var currentStage = _db.TestApproval.Where(x=>x.RequestId ==  dto.RequestId && x.Is_Assigned == true).OrderByDescending(x=>x.Id).FirstOrDefault();

                    var updateIsAssigned = _db.TestApproval.Where(x => x.RequestId == dto.RequestId && x.ApprovalUIId == dto.ApprovalUIId && x.StageNo == currentStage.StageNo).ToList();

                    if (updateIsAssigned.Count > 0)
                    {
                        foreach (var user in updateIsAssigned)
                        {
                            user.Is_Assigned = true;
                            user.AssignedDateTime = DateTime.Now;
                            user.ApprovalStatus = UHelper.ApprovalStatus(2);
                            _db.SaveChanges();
                        }
                    }
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    // Data = model
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        //UpdateRequestApprovalSetup update request adding and automatically configure with stages and users
        [HttpPost]
        [Route("UpdateRequestApprovalSetup")]
        public IActionResult UpdateRequestApprovalSetup(AUAprrovalRequestDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                    });
                }

                var approvalUsers = _db.TestApproval.Where(x => x.ApprovalUIId == dto.ApprovalUIId && x.RequestId == dto.RequestId).ToList();

                if (approvalUsers.Count > 0)
                {
                    foreach (var user in approvalUsers)
                    {
                        user.Is_Assigned = false;
                        user.ApprovalStatus = UHelper.ApprovalStatus(1);
                        _db.SaveChanges();

                    }

                    var updateIsAssigned = _db.TestApproval.Where(x => x.RequestId == dto.RequestId && x.ApprovalUIId == dto.ApprovalUIId && x.StageNo == 1).ToList();

                    if (updateIsAssigned.Count > 0)
                    {
                        foreach (var user in updateIsAssigned)
                        {
                            user.Is_Assigned = true;
                            user.ApprovalStatus = UHelper.ApprovalStatus(2);
                            user.AssignedDateTime = DateTime.Now;
                            _db.SaveChanges();
                        }
                    }
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    // Data = model
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        //UpdateApprovalStatus from approval loop users and automatically move next according to NumberOfApprovalRequired
        [HttpPost]
        [Route("UpdateApprovalStatus")]
        public IActionResult UpdateApprovalStatus(RequestApprovalStatusUpdateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                    });
                }

                var updaterequeststatus = _db.TestApproval.Where(x => x.RequestId == dto.RequestId && x.ApprovalUIId == dto.ApprovalUIId && x.UserId == dto.UserId).OrderByDescending(x => x.Id).FirstOrDefault();

                if (updaterequeststatus != null)
                {
                    updaterequeststatus.ApprovalStatus = UHelper.ApprovalStatus(dto.IsApproved);
                    updaterequeststatus.ActionDateTime = DateTime.Now;
                    updaterequeststatus.LastActionComment =
                    string.IsNullOrEmpty(updaterequeststatus.LastActionComment)
                        ? $"{DateTime.Now:yyyy-MM-dd HH:mm} | Status: {updaterequeststatus.ApprovalStatus} | Comment: {dto.Comment}"
                        : $"{updaterequeststatus.LastActionComment}{Environment.NewLine}{DateTime.Now:yyyy-MM-dd HH:mm} | Status: {updaterequeststatus.ApprovalStatus} | Comment: {dto.Comment}";

                    _db.SaveChanges();

                    if (updaterequeststatus.ApprovalStatus == "Reject" || updaterequeststatus.ApprovalStatus == "Rejected")
                    {
                        ReverseAndNotify(dto);

                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "Success",
                            Data = null
                        });
                    }

                    var assignNextStage = _db.TestApproval.Where(x => x.RequestId == dto.RequestId && x.ApprovalUIId == dto.ApprovalUIId && x.StageNo == updaterequeststatus.StageNo).ToList();

                    if (assignNextStage.Count > 0)
                    {
                        int approvalCount = assignNextStage.Where(x => x.ApprovalStatus == "Approved").Count();

                        if (approvalCount >= updaterequeststatus.NumberOfApprovalRequired)
                        {
                            var updateAssignNextStage = _db.TestApproval.Where(x => x.RequestId == dto.RequestId && x.ApprovalUIId == dto.ApprovalUIId && x.StageNo == updaterequeststatus.StageNo + 1).ToList();
                            //only for ndc Member
                            //if (dto.ApprovalUIId == (int)ApprovalUIIds.NDCRequestForMember)
                            //{
                            //    NDC1CheckList nDC1CheckList = new NDC1CheckList();

                            //    nDC1CheckList.PropertyId = _commonBLL.GetStockIdFromNDCMember(dto.RequestId);
                            //    nDC1CheckList.Department = _commonBLL.GetDepartmentFromUserId(dto.UserId);
                            //    nDC1CheckList.Remarks = dto.Comment;
                            //    nDC1CheckList.Action = UHelper.ApprovalStatus(dto.IsApproved);
                            //    nDC1CheckList.NDC1Id = null;

                            //    _db.NDC1CheckLists.Add(nDC1CheckList);
                            //    _db.SaveChanges();
                            //}

                            if (updateAssignNextStage.Count > 0)
                            {
                                foreach (var item in updateAssignNextStage)
                                {
                                    item.Is_Assigned = true;
                                    item.ApprovalStatus = UHelper.ApprovalStatus(2);
                                    item.AssignedDateTime = DateTime.Now;
                                    _db.SaveChanges();
                                }
                            }

                            else
                            {
                                if (dto.ApprovalUIId == (int)ApprovalUIIds.StockCreation)
                                {
                                    //bool updated = _commonBLL.UpdateStockCreation();
                                    var stock = _db.StockCreations.Where(x => !x.is_deleted && x.is_active == true
                                    && x.Is_StockCreationRequested == true
                                    && x.ID == dto.RequestId
                                    )
                                        .ToList();

                                    if (stock?.Count > 0)
                                    {
                                        foreach (var item in stock)
                                        {
                                            item.Is_StockCreationApproved = true;
                                            item.Updated_at = DateTime.Now;
                                            item.Status = "Approved";

                                            _db.SaveChanges();
                                        }
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.PossessionAnnocement)
                                {
                                    //bool updated = _commonBLL.UpdateStockCreation();
                                    var stock = _db.StockCreations.Where(x => !x.is_deleted
                                                                   && x.is_active == true
                                                                   && x.PossessionStatus == false
                                                                   && x.Is_StockCreationApproved == true
                                                                   && x.ID == dto.RequestId
                                                                   )
                                                                  .ToList();

                                    if (stock?.Count > 0)
                                    {
                                        foreach (var item in stock)
                                        {
                                            item.Is_PossessionApproved = true;
                                            item.PossessionStatus = true;
                                            item.Updated_at = DateTime.Now;
                                            item.UnderLitigation = true;

                                            _db.SaveChanges();
                                        }
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.ClearanceForm)
                                {
                                    var stock = _db.StockCreations.Where(x => !x.is_deleted
                                                               && x.is_active == true
                                                               && x.ID == dto.RequestId
                                                               )
                                                              .FirstOrDefault();

                                    if (stock != null)
                                    {
                                        stock.Is_ClearnceApproved = true;
                                        stock.Updated_at = DateTime.Now;

                                        _db.SaveChanges();
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.MapApproval)
                                {
                                    var stock = _db.StockCreations.Where(x => !x.is_deleted
                                                               && x.is_active == true
                                                               && x.ID == dto.RequestId
                                                               )
                                                              .FirstOrDefault();

                                    if (stock != null)
                                    {
                                        stock.Is_MapApprovalApproved = true;
                                        stock.Updated_at = DateTime.Now;

                                        _db.SaveChanges();
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.DemarcationForm)
                                {
                                    var stock = _db.StockCreations.Where(x => !x.is_deleted
                                                               && x.is_active == true
                                                               && x.ID == dto.RequestId
                                                               )
                                                              .FirstOrDefault();

                                    if (stock != null)
                                    {
                                        stock.Is_DemarcationFormApproved = true;
                                        stock.Updated_at = DateTime.Now;

                                        _db.SaveChanges();
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.ConstructionSecurity)
                                {
                                    var stock = _db.StockCreations.Where(x => !x.is_deleted
                                                               && x.is_active == true
                                                               && x.ID == dto.RequestId
                                                               )
                                                              .FirstOrDefault();

                                    if (stock != null)
                                    {
                                        stock.Is_ConstructionSecurityApproved = true;
                                        stock.Updated_at = DateTime.Now;

                                        _db.SaveChanges();
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.ConstructionMonitoring)
                                {
                                    var stock = _db.StockCreations.Where(x => !x.is_deleted
                                                               && x.is_active == true
                                                               && x.ID == dto.RequestId
                                                               )
                                                              .FirstOrDefault();

                                    if (stock != null)
                                    {
                                        stock.Is_ConstructionMonitoringApproved = true;
                                        stock.Updated_at = DateTime.Now;

                                        _db.SaveChanges();
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.MemberRegistrationForm)
                                {
                                    var member = _db.MemberProfile.Where(x => !x.IsDeleted
                                                               && x.IsActive == true
                                                               && x.Id == dto.RequestId
                                                               )
                                                              .FirstOrDefault();

                                    if (member != null)
                                    {
                                        member.IsMemberProfileApproved = true;
                                        member.LastModified = DateTime.Now;

                                        _db.SaveChanges();
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.DealerRegistrationForm)
                                {
                                    var dealer = _db.Dealers.Where(x => !x.IsDeleted
                                                               && x.IsActive == true
                                                               && x.Id == dto.RequestId
                                                               )
                                                              .FirstOrDefault();

                                    if (dealer != null)
                                    {
                                        dealer.IsDealerProfileApproved = true;
                                        dealer.LastModified = DateTime.Now;

                                        _db.SaveChanges();
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.PreSale)
                                {
                                    var stockCreationId = _db.PreSale.Find(dto.RequestId).StockCreationId;
                                    var stock = _db.StockCreations.Where(x => !x.is_deleted
                                                               && x.is_active == true
                                                               && x.ID == stockCreationId
                                                               )
                                                              .FirstOrDefault();

                                    if (stock != null)
                                    {
                                        stock.IsPreSaleApproved = true;
                                        stock.Updated_at = DateTime.Now;

                                        _db.SaveChanges();
                                    }
                                }
                                if (dto.ApprovalUIId == (int)ApprovalUIIds.BookingForm)
                                {
                                    var stockCreationId = _db.Booking.Find(dto.RequestId).StockCreationId;
                                    var stock = _db.StockCreations.Where(x => !x.is_deleted
                                                               && x.is_active == true
                                                               && x.ID == stockCreationId
                                                               )
                                                              .FirstOrDefault();
                                    var bookingdata = _db.Booking.Include(x => x.BookingProcessingCharges)
                                                    .Include(x => x.BookingSchedulePaymentPlanDetail)
                                                    .Where(x => x.Id == dto.RequestId).FirstOrDefault();

                                    if (bookingdata != null)
                                    {
                                        if (bookingdata.BookingSchedulePaymentPlanDetail.Count > 0)
                                        {
                                            Response_Result response_ResultBookingSchedule = new SapIntegrationController(_db).AddServiceTypeInvoiceBookingSchedule(bookingdata, false);

                                        }

                                        if (bookingdata.BookingProcessingCharges.Count > 0)
                                        {
                                            Response_Result response_Result = new SapIntegrationController(_db).AddServiceTypeInvoiceProcessingCharges(bookingdata, false);

                                        }
                                    }
                                    if (stock != null)
                                    {
                                        stock.IsBookingApproved = true;
                                        stock.Updated_at = DateTime.Now;

                                        _db.SaveChanges();
                                    }
                                }
                                if (dto.ApprovalUIId == (int)ApprovalUIIds.NDCRequestForMember)
                                {
                                    var nDCRequestForMember = _db.NDCRequestForMember.Where(x => !x.IsDeleted
                                                               && x.IsActive == true
                                                               && x.Id == dto.RequestId
                                                               )
                                                              .FirstOrDefault();

                                    if (nDCRequestForMember != null)
                                    {
                                        nDCRequestForMember.IsNDCRequestForMemberApproved = true;
                                        nDCRequestForMember.LastModified = DateTime.Now;

                                        _db.SaveChanges();
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.FileVerification)
                                {
                                    var fileVerificationRequest = _db.FileVerificationRequests.Include("FileVerificationRequestCharges").Where(x => !x.IsDeleted
                                                               && x.IsActive == true
                                                               && x.Id == dto.RequestId
                                                               )
                                                              .FirstOrDefault();

                                    if (fileVerificationRequest != null)
                                    {
                                        fileVerificationRequest.IsFileVerificationApproved = true;
                                        fileVerificationRequest.LastModified = DateTime.Now;


                                        Response_Result response = new SapIntegrationController(_db).PostingARInvoiceForFileVerificationRequest(fileVerificationRequest);
                                        if (response.code != 0)
                                        {
                                            return Ok(new ApiResponse<object>
                                            {
                                                Code = ResponseCode.NotFound,
                                                Message = response.message,
                                                Data = null
                                            });
                                        }
                                        _db.SaveChanges();

                                        return Ok(new ApiResponse<object>
                                        {
                                            Code = ResponseCode.Success,
                                            Message = "Success",
                                            Data = null
                                        });
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.ClientFileVerification)
                                {
                                    var clientFileVerification = _db.ClientFileVerification.Where(x => !x.IsDeleted
                                                               && x.IsActive == true
                                                               && x.Id == dto.RequestId
                                                               )
                                                              .FirstOrDefault();

                                    if (clientFileVerification != null)
                                    {
                                        clientFileVerification.IsPrintEnabled = true;
                                        clientFileVerification.IsClientFileVerificationApproved = true;
                                        clientFileVerification.LastModified = DateTime.Now;

                                        _db.SaveChanges();
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.TransferReceipt)
                                {
                                    var transferReceipt = _db.TransferHistery.Where(x => !x.IsDeleted
                                                               && x.Id == dto.RequestId
                                                               )
                                                              .FirstOrDefault();

                                    if (transferReceipt != null)
                                    {
                                        transferReceipt.IsGovtProcessingTaxApproved = true;
                                        transferReceipt.LastModified = DateTime.Now;
                                        transferReceipt.IsActive = true;
                                        _db.SaveChanges();
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.DemandNote)
                                {
                                    var demandNote = _db.DemandNote.Where(x => !x.IsDeleted
                                                               && x.IsActive == true
                                                               && x.Id == dto.RequestId
                                                               )
                                                              .FirstOrDefault();

                                    if (demandNote != null)
                                    {
                                        demandNote.IsDemandNoteApproved = true;
                                        demandNote.LastModified = DateTime.Now;

                                        _db.SaveChanges();
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.Transfer)
                                {
                                    var TransferHistery = _db.TransferHistery.Where(x => !x.IsDeleted
                                                               && x.IsActive == true
                                                               && x.Id == dto.RequestId
                                                               )
                                                              .FirstOrDefault();

                                    if (TransferHistery != null)
                                    {
                                        TransferHistery.IsTransferApproved = true;
                                        TransferHistery.LastModified = DateTime.Now;

                                        _db.SaveChanges();
                                    }
                                }
                            }
                        }
                    }

                    ApprovalHistery approvalHistory = new ApprovalHistery();
                    {
                        approvalHistory.RequestId = dto.RequestId;
                        approvalHistory.ApprovalUIId = dto.ApprovalUIId;
                        approvalHistory.ActionTakenByName = "current login user name";
                        approvalHistory.ActionTakenUserRole = "current login user role";
                        approvalHistory.ActionDateTime = DateTime.Now;
                        approvalHistory.Action = UHelper.ApprovalStatus(dto.IsApproved);
                        approvalHistory.ActionComment = dto.Comment;

                        _db.ApprovalHistery.Add(approvalHistory);
                        _db.SaveChanges();
                    }

                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    // Data = dto
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        private bool ReverseAndNotify(RequestApprovalStatusUpdateDTO dto)
        {
            if (dto.ApprovalUIId == (int)ApprovalUIIds.PossessionAnnocement)
            {
                var updaterequeststatus = _db.TestApproval.Where(x => x.RequestId == dto.RequestId && x.ApprovalUIId == dto.ApprovalUIId && x.ApprovalStatus == "Reject").ToList();

                int stage = updaterequeststatus.FirstOrDefault().StageNo;

                if (stage == 1)
                {

                    var stock = _db.StockCreations.Where(x => !x.is_deleted
                                                                   && x.is_active == true
                                                                   && x.PossessionStatus == false
                                                                   && x.Is_StockCreationApproved == true
                                                                   && x.ID == dto.RequestId
                                                                   )
                                                                  .FirstOrDefault();

                    stock.PossessionEffectDate = null;
                    stock.PossessionStatus = false;
                    stock.Is_PossessionRequested = false;
                    stock.Is_PossessionApproved = false;


                    var pendingtageUsers = _db.TestApproval.Where(x => x.RequestId == dto.RequestId && x.ApprovalUIId == dto.ApprovalUIId).ToList();

                    if(pendingtageUsers.Count > 0)
                    {
                        _db.TestApproval.RemoveRange(pendingtageUsers);
                    }
                }
                else
                {
                    var currentStageUsers = _db.TestApproval.Where(x => x.RequestId == dto.RequestId && x.ApprovalUIId == dto.ApprovalUIId && x.Is_Assigned == true && x.StageNo == stage).ToList();
                    foreach (var item in currentStageUsers)
                    {
                        item.ApprovalStatus = UHelper.ApprovalStatus(1);
                    }

                    int previousStage = stage - 1;

                    var previousStageUsers = _db.TestApproval.Where(x => x.RequestId == dto.RequestId && x.ApprovalUIId == dto.ApprovalUIId && x.Is_Assigned == true && x.StageNo == previousStage).ToList();
                    
                    foreach (var item in previousStageUsers)
                    {
                        item.ApprovalStatus = UHelper.ApprovalStatus(2);
                    }
                }
            }

            if (dto.ApprovalUIId == (int)ApprovalUIIds.NDCRequestForMember)
            {
                var updaterequeststatus = _db.TestApproval.Where(x => x.RequestId == dto.RequestId && x.ApprovalUIId == dto.ApprovalUIId && x.Is_Assigned == true).ToList();

                foreach (var item in updaterequeststatus)
                {
                    item.ApprovalStatus = UHelper.ApprovalStatus(2);
                }
            }

                _db.SaveChanges();

            return true;
        }

        [HttpPost]
        [Route("GetApprovalHistory")]
        public IActionResult GetApprovalHistory(AUAprrovalRequestDTO dto)
        {
            try
            {

                var result = _db.ApprovalHistery.Where(x => x.ApprovalUIId == dto.ApprovalUIId && x.RequestId == dto.RequestId)
                                                .ToList();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        // Get Approval Inbox List
        // Code refecter in revision nedeed now only try to complete the flow
        [HttpGet]
        [Route("GetInboxStockCreationListForApprovalByUserId")]
        public IActionResult GetInboxStockCreationListForApprovalByUserId(int userId)
        {
            
            try
            {
                var result = (from stock in _db.StockCreations.Where(x => x.is_active == true
                                                                               && x.Is_StockCreationRequested == true
                                                                               && x.Is_StockCreationApproved != true
                                                                               && x.PossessionEffectDate == null)
                              join ta in _db.TestApproval.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.StockCreation)
                              on stock.ID equals ta.RequestId
                              where ta.UserId == userId

                              select new StockCreation
                              {
                                  ID = stock.ID,
                                  RegistrationNo = stock.RegistrationNo,
                                  PropertyNo = stock.PropertyNo,
                                  RealStateType = stock.RealStateType,
                                  Phase = stock.Phase,
                                  Project = stock.Project,
                                  Block = stock.Block,
                                  Category = stock.Category,
                                  Type = stock.Type,
                                  Nature = stock.Nature,
                                  ActualSize = stock.ActualSize,
                                  RequestId = ta.RequestId,
                                  ApprovalUIID = ta.ApprovalUIId
                              })
                             .ToList() ?? new List<StockCreation>();

                if (result?.Count > 0)
                {
                    foreach (var item in result)
                    {
                        item.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(item.RealStateType));
                        item.ProjectName = _commonBLL.GetProjectName(Convert.ToInt32(item.Project));
                        item.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(item.Phase));
                        item.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(item.Category));
                        item.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(item.Block));
                        item.NatureName = _commonBLL.GetNatureName(Convert.ToInt32(item.Nature));
                        item.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(item.Type));
                    }

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "Not Found",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetInboxPossessionAnnouncedListForApprovalByUserId")]
        public IActionResult GetInboxPossessionAnnouncedListForApprovalByUserId(int userId)
        {
            try
            {
                var result = (from stock in _db.StockCreations.Where(x => x.is_active == true &&
                                                                          x.Is_StockCreationApproved == true &&
                                                                          x.PossessionEffectDate != null &&
                                                                          x.Is_DemarcationRequested != true &&
                                                                          x.Is_PossessionRequested == true &&
                                                                          x.Is_PossessionApproved != true &&
                                                                          x.PossessionStatus != true)
                              join ta in _db.TestApproval.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.PossessionAnnocement && x.ApprovalStatus == "Pending")
                              on stock.ID equals ta.RequestId
                              where ta.UserId == userId

                              select new StockCreation
                              {
                                  ID = stock.ID,
                                  RegistrationNo = stock.RegistrationNo,
                                  PropertyNo = stock.PropertyNo,
                                  RealStateType = stock.RealStateType,
                                  Phase = stock.Phase,
                                  Project = stock.Project,
                                  Block = stock.Block,
                                  Category = stock.Category,
                                  Type = stock.Type,
                                  Nature = stock.Nature,
                                  ActualSize = stock.ActualSize,
                                  RequestId = ta.RequestId,
                                  ApprovalUIID = ta.ApprovalUIId
                              })
                            .ToList() ?? new List<StockCreation>();

                if (result?.Count > 0)
                {
                    foreach (var item in result)
                    {
                        item.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(item.RealStateType));
                        item.ProjectName = _commonBLL.GetProjectName(Convert.ToInt32(item.Project));
                        item.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(item.Phase));
                        item.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(item.Category));
                        item.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(item.Block));
                        item.NatureName = _commonBLL.GetNatureName(Convert.ToInt32(item.Nature));
                        item.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(item.Type));
                    }

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "Not Found",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetInboxClearnceRequestListForApprovalByUserId")]
        public IActionResult GetInboxClearnceRequestListForApprovalByUserId(int userId)
        {
            try
            {
                var result = (from stock in _db.StockCreations.Where(x => x.is_active == true
                                                      && x.Is_StockCreationApproved == true
                                                      && x.PossessionEffectDate != null
                                                      && x.Is_DemarcationRequested == true
                                                      && x.Is_ClearnceRequested == true
                                                      && x.Is_ClearnceApproved != true)
                              join ta in _db.TestApproval.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.ClearanceForm
                                   && x.IsCancelled != true)
                              on stock.ID equals ta.RequestId
                              where ta.UserId == userId

                              select new StockCreation
                              {
                                  ID = stock.ID,
                                  RegistrationNo = stock.RegistrationNo,
                                  PropertyNo = stock.PropertyNo,
                                  RealStateType = stock.RealStateType,
                                  Phase = stock.Phase,
                                  Project = stock.Project,
                                  Block = stock.Block,
                                  Category = stock.Category,
                                  Type = stock.Type,
                                  Nature = stock.Nature,
                                  ActualSize = stock.ActualSize,
                                  RequestId = ta.RequestId,
                                  ApprovalUIID = ta.ApprovalUIId
                              })
                             .ToList() ?? new List<StockCreation>();

                if (result?.Count > 0)
                {
                    foreach (var item in result)
                    {
                        item.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(item.RealStateType));
                        item.ProjectName = _commonBLL.GetProjectName(Convert.ToInt32(item.Project));
                        item.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(item.Phase));
                        item.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(item.Category));
                        item.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(item.Block));
                        item.NatureName = _commonBLL.GetNatureName(Convert.ToInt32(item.Nature));
                        item.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(item.Type));
                    }

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "Not Found",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetInboxMapApprovalListForApprovalByUserId")]
        public IActionResult GetInboxMapApprovalListForApprovalByUserId(int userId)
        {
            try
            {
                var result = (from stock in _db.StockCreations.Where(x => x.is_active == true
                                                      && x.Is_StockCreationApproved == true
                                                      && x.PossessionEffectDate != null
                                                      && x.Is_DemarcationRequested == true
                                                      && x.Is_ClearnceRequested == true
                                                      && x.Is_ClearnceApproved == true
                                                      && x.Is_MapApprovalRequested == true
                                                      && x.Is_MapApprovalApproved != true)
                              join ta in _db.TestApproval.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.MapApproval)
                              on stock.ID equals ta.RequestId
                              where ta.UserId == userId

                              select new StockCreation
                              {
                                  ID = stock.ID,
                                  RegistrationNo = stock.RegistrationNo,
                                  PropertyNo = stock.PropertyNo,
                                  RealStateType = stock.RealStateType,
                                  Phase = stock.Phase,
                                  Project = stock.Project,
                                  Block = stock.Block,
                                  Category = stock.Category,
                                  Type = stock.Type,
                                  Nature = stock.Nature,
                                  ActualSize = stock.ActualSize,
                                  RequestId = ta.RequestId,
                                  ApprovalUIID = ta.ApprovalUIId
                              })
                                                .ToList() ?? new List<StockCreation>();

                if (result?.Count > 0)
                {
                    foreach (var item in result)
                    {
                        item.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(item.RealStateType));
                        item.ProjectName = _commonBLL.GetProjectName(Convert.ToInt32(item.Project));
                        item.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(item.Phase));
                        item.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(item.Category));
                        item.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(item.Block));
                        item.NatureName = _commonBLL.GetNatureName(Convert.ToInt32(item.Nature));
                        item.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(item.Type));
                    }

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "Not Found",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetInboxDemarcationFormListForApprovalByUserId")]
        public IActionResult GetInboxDemarcationFormListForApprovalByUserId(int userId)
        {
            try
            {
                var result = (from stock in _db.StockCreations.Where(x => x.is_active == true
                                                                          && x.Is_StockCreationApproved == true
                                                                          && x.PossessionEffectDate != null
                                                                          && x.Is_DemarcationRequested == true
                                                                          && x.Is_ClearnceRequested == true
                                                                          && x.Is_ClearnceApproved == true
                                                                          && x.Is_MapApprovalRequested == true
                                                                          && x.Is_MapApprovalApproved == true
                                                                          && x.Is_DemarcationFormRequested == true
                                                                          && x.Is_DemarcationFormApproved != true)
                              join ta in _db.TestApproval.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.DemarcationForm)
                              on stock.ID equals ta.RequestId
                              where ta.UserId == userId

                              select new StockCreation
                              {
                                  ID = stock.ID,
                                  RegistrationNo = stock.RegistrationNo,
                                  PropertyNo = stock.PropertyNo,
                                  RealStateType = stock.RealStateType,
                                  Phase = stock.Phase,
                                  Project = stock.Project,
                                  Block = stock.Block,
                                  Category = stock.Category,
                                  Type = stock.Type,
                                  Nature = stock.Nature,
                                  ActualSize = stock.ActualSize,
                                  RequestId = ta.RequestId,
                                  ApprovalUIID = ta.ApprovalUIId
                              })
                               .ToList() ?? new List<StockCreation>();

                if (result?.Count > 0)
                {
                    foreach (var item in result)
                    {
                        item.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(item.RealStateType));
                        item.ProjectName = _commonBLL.GetProjectName(Convert.ToInt32(item.Project));
                        item.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(item.Phase));
                        item.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(item.Category));
                        item.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(item.Block));
                        item.NatureName = _commonBLL.GetNatureName(Convert.ToInt32(item.Nature));
                        item.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(item.Type));
                    }

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "Not Found",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetInboxConstructionSecurityListForApprovalByUserId")]
        public IActionResult GetInboxConstructionSecurityListForApprovalByUserId(int userId)
        {
            try
            {
                var result = (from stock in _db.StockCreations.Where(x => x.is_active == true
                                                      && x.Is_StockCreationApproved == true
                                                      && x.PossessionEffectDate != null
                                                      && x.Is_DemarcationRequested == true
                                                      && x.Is_ClearnceRequested == true
                                                      && x.Is_ClearnceApproved == true
                                                      && x.Is_MapApprovalRequested == true
                                                      && x.Is_MapApprovalApproved == true
                                                      && x.Is_DemarcationFormRequested == true
                                                      && x.Is_DemarcationFormApproved == true
                                                      && x.Is_ConstructionSecurityRequested == true
                                                      && x.Is_ConstructionSecurityApproved != true)
                              join ta in _db.TestApproval.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.ConstructionSecurity)
                              on stock.ID equals ta.RequestId
                              where ta.UserId == userId

                              select new StockCreation
                              {
                                  ID = stock.ID,
                                  RegistrationNo = stock.RegistrationNo,
                                  PropertyNo = stock.PropertyNo,
                                  RealStateType = stock.RealStateType,
                                  Phase = stock.Phase,
                                  Project = stock.Project,
                                  Block = stock.Block,
                                  Category = stock.Category,
                                  Type = stock.Type,
                                  Nature = stock.Nature,
                                  ActualSize = stock.ActualSize,
                                  RequestId = ta.RequestId,
                                  ApprovalUIID = ta.ApprovalUIId
                              })
                             .ToList() ?? new List<StockCreation>();

                if (result?.Count > 0)
                {
                    foreach (var item in result)
                    {
                        item.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(item.RealStateType));
                        item.ProjectName = _commonBLL.GetProjectName(Convert.ToInt32(item.Project));
                        item.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(item.Phase));
                        item.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(item.Category));
                        item.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(item.Block));
                        item.NatureName = _commonBLL.GetNatureName(Convert.ToInt32(item.Nature));
                        item.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(item.Type));
                    }

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "Not Found",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetInboxConstructionMonitoringListForApprovalByUserId")]
        public IActionResult GetInboxConstructionMonitoringListForApprovalByUserId(int userId)
        {
            try
            {
                var result = (from stock in _db.StockCreations.Where(x => x.is_active == true
                                                      && x.Is_StockCreationApproved == true
                                                      && x.PossessionEffectDate != null
                                                      && x.Is_DemarcationRequested == true
                                                      && x.Is_ClearnceRequested == true
                                                      && x.Is_ClearnceApproved == true
                                                      && x.Is_MapApprovalRequested == true
                                                      && x.Is_MapApprovalApproved == true
                                                      && x.Is_ConstructionSecurityRequested == true
                                                      && x.Is_ConstructionSecurityApproved == true
                                                      && x.Is_ConstructionMonitoringRequested == true
                                                      && x.Is_ConstructionMonitoringApproved != true)
                              join ta in _db.TestApproval.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.ConstructionMonitoring)
                              on stock.ID equals ta.RequestId
                              where ta.UserId == userId

                              select new StockCreation
                              {
                                  ID = stock.ID,
                                  RegistrationNo = stock.RegistrationNo,
                                  PropertyNo = stock.PropertyNo,
                                  RealStateType = stock.RealStateType,
                                  Phase = stock.Phase,
                                  Project = stock.Project,
                                  Block = stock.Block,
                                  Category = stock.Category,
                                  Type = stock.Type,
                                  Nature = stock.Nature,
                                  ActualSize = stock.ActualSize,
                                  RequestId = ta.RequestId,
                                  ApprovalUIID = ta.ApprovalUIId
                              })
                              .ToList() ?? new List<StockCreation>();

                if (result?.Count > 0)
                {
                    foreach (var item in result)
                    {
                        item.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(item.RealStateType));
                        item.ProjectName = _commonBLL.GetProjectName(Convert.ToInt32(item.Project));
                        item.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(item.Phase));
                        item.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(item.Category));
                        item.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(item.Block));
                        item.NatureName = _commonBLL.GetNatureName(Convert.ToInt32(item.Nature));
                        item.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(item.Type));
                    }

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "Not Found",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetInboxMemberProfileListForApprovalByUserId")]
        public IActionResult GetInboxMemberProfileListForApprovalByUserId(int userId)
        {
            try
            {
                var result = (from member in _db.MemberProfile.Where(x => x.IsActive == true
                                                      && x.IsMemberProfileRequested == true
                                                      && x.IsMemberProfileApproved != true
                                                       )
                              join ta in _db.TestApproval.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.MemberRegistrationForm)
                              on member.Id equals ta.RequestId
                              where ta.UserId == userId

                              select new
                              {
                                  member.Id,
                                  member.MemberName,
                                  member.Cnic,
                                  member.PermanentAddress,
                                  member.Mobile,
                                  ta.RequestId,
                                  ta.ApprovalUIId
                              })
                              .ToList();

                if (result?.Count > 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "Not Found",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetInboxDealerListForApprovalByUserId")]
        public IActionResult GetInboxDealerListForApprovalByUserId(int userId)
        {
            try
            {
                var result = (from dealer in _db.Dealers.Where(x => x.IsActive == true
                                                      && x.IsDealerProfileRequested == true
                                                      && x.IsDealerProfileApproved != true
                                                       )
                              join ta in _db.TestApproval.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.DealerRegistrationForm)
                              on dealer.Id equals ta.RequestId
                              where ta.UserId == userId

                              select new
                              {
                                  dealer.Id,
                                  dealer.EstateName,
                                  dealer.PrincipalOwner,
                                  dealer.CNIC,
                                  dealer.EstateAddress,
                                  ta.RequestId,
                                  ta.ApprovalUIId
                              })
                               .ToList();

                if (result?.Count > 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "Not Found",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetInboxPreSaleListForApprovalByUserId")]
        public IActionResult GetInboxPreSaleListForApprovalByUserId(int userId)
        {
            try
            {
                var result = (from preSale in _db.PreSale.Include(x=>x.StockCreation)
                                                         .Include(x => x.Dealer)
                                                         .Where(x => x.IsActive == true
                                                          )
                              join ta in _db.TestApproval.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.PreSale)
                              on preSale.Id equals ta.RequestId
                              where ta.UserId == userId 
                              && preSale.StockCreation.IsPreSaleRequested == true 
                              && preSale.StockCreation.IsPreSaleApproved != true

                              select new PreSaleInboxDTO
                              {
                                  Id = preSale.Id,
                                  MemberName = preSale.MemberName,
                                  Cnic = preSale.Cnic,
                                  DealerName = preSale.DealerName,
                                  ReferedBy = preSale.ReferedBy,
                                  MobileNo = preSale.MobileNo,
                                  RegistrationNo = preSale.StockCreation.RegistrationNo ?? "",
                                  PropertyNo =$"{_commonBLL.GetBlockName(Convert.ToInt32(preSale.StockCreation.Block))}-{preSale.StockCreation.PropertyNo ?? ""}",
                                  RequestId = ta.RequestId,
                                  ApprovalUIID = ta.ApprovalUIId
                              })
                              .ToList() ?? new List<PreSaleInboxDTO>();

                if (result?.Count > 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "Not Found",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetInboxBookingListForApprovalByUserId")]
        public IActionResult GetInboxBookingListForApprovalByUserId(int userId)
        {
            try
            {
                var result = (from booking in _db.Booking.Include(x => x.StockCreation)
                                                         .Include(x => x.Dealer)
                                                         .Where(x => x.IsActive == true
                                                          )
                              join ta in _db.TestApproval.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.BookingForm && x.ApprovalStatus == UHelper.ApprovalStatus(2))
                              on booking.Id equals ta.RequestId
                              where ta.UserId == userId
                              && booking.StockCreation.IsBookingRequested == true
                              && booking.StockCreation.IsBookingApproved != true

                              select new PreSaleInboxDTO
                              {
                                  Id = booking.Id,
                                  MemberName = booking.StockCreation.MemberProfile.MemberName,
                                  Cnic = booking.StockCreation.MemberProfile.Cnic,
                                  DealerName = booking.StockCreation.Dealer.PrincipalOwner,
                                  MobileNo = booking.StockCreation.MemberProfile.Mobile,
                                  RegistrationNo = booking.StockCreation.RegistrationNo ?? "",
                                  PropertyNo = booking.StockCreation.PropertyNo ?? "",
                                  RequestId = ta.RequestId,
                                  ApprovalUIID = ta.ApprovalUIId
                              })
                              .ToList() ?? new List<PreSaleInboxDTO>();

                if (result?.Count > 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "Not Found",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetInboxNDCRequestForMemberListForApprovalByUserId")]
        public IActionResult GetInboxNDCRequestForMemberListForApprovalByUserId(int userId)
        {
            try
            {
                var result = (from ndcmember in _db.NDCRequestForMember.Include(x => x.StockCreation)
                                                         .Include(x => x.MemberProfile)
                                                         .Where(x => x.IsActive == true
                                                          && x.IsNDCRequestForMemberRequested == true
                                                          && x.IsNDCRequestForMemberApproved != true)
                              join ta in _db.TestApproval.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.NDCRequestForMember && x.ApprovalStatus == UHelper.ApprovalStatus(2))
                              on ndcmember.Id equals ta.RequestId
                              where ta.UserId == userId

                              select new PreSaleInboxDTO
                              {
                                  Id = ndcmember.Id,
                                  MemberName = ndcmember.MemberProfile.MemberName ?? "",
                                  Cnic = ndcmember.MemberProfile.Cnic ??"",
                                  MobileNo = ndcmember.MemberProfile.Mobile ?? "",
                                  RegistrationNo = ndcmember.StockCreation.RegistrationNo ?? "",
                                  PropertyNo = ndcmember.StockCreation.PropertyNo ?? "",
                                  RequestId = ta.RequestId,
                                  ApprovalUIID = ta.ApprovalUIId
                              })
                              .ToList() ?? new List<PreSaleInboxDTO>();

                if (result?.Count > 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "Not Found",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetInboxfileVerificationListForApprovalByUserId")]
        public IActionResult GetInboxfileVerificationListForApprovalByUserId(int userId)
        {
            try
            {
                var result = (from filemember in _db.FileVerificationRequests.Include(x => x.StockCreation)
                                                         .Include(x => x.MemberProfile)
                                                         .Where(x => x.IsActive == true
                                                          && x.IsFileVerificationRequested == true
                                                          && x.IsFileVerificationApproved != true)
                              join ta in _db.TestApproval.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.FileVerification)
                              on filemember.Id equals ta.RequestId
                              where ta.UserId == userId

                              select new PreSaleInboxDTO
                              {
                                  Id = filemember.Id,
                                  MemberName = filemember.MemberProfile.MemberName ?? "",
                                  Cnic = filemember.MemberProfile.Cnic ?? "",
                                  MobileNo = filemember.MemberProfile.Mobile ?? "",
                                  RegistrationNo = filemember.StockCreation.RegistrationNo ?? "",
                                  PropertyNo = filemember.StockCreation.PropertyNo ?? "",
                                  RequestId = ta.RequestId,
                                  ApprovalUIID = ta.ApprovalUIId
                              })
                              .ToList() ?? new List<PreSaleInboxDTO>();

                if (result?.Count > 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "Not Found",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetInboxclientfileVerificationListForApprovalByUserId")]
        public IActionResult GetInboxclientfileVerificationListForApprovalByUserId(int userId)
        {
            try
            {
                var result = (from clientfilemember in _db.ClientFileVerification.Include(x => x.StockCreation)
                                                         .Include(x => x.MemberProfile)
                                                         .Where(x => x.IsActive == true
                                                          && x.IsClientFileVerificationRequested == true
                                                          && x.IsClientFileVerificationApproved != true)
                              join ta in _db.TestApproval.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.ClientFileVerification)
                              on clientfilemember.Id equals ta.RequestId
                              where ta.UserId == userId

                              select new PreSaleInboxDTO
                              {
                                  Id = clientfilemember.Id,
                                  MemberName = clientfilemember.MemberProfile.MemberName ?? "",
                                  Cnic = clientfilemember.MemberProfile.Cnic ?? "",
                                  MobileNo = clientfilemember.MemberProfile.Mobile ?? "",
                                  RegistrationNo = clientfilemember.StockCreation.RegistrationNo ?? "",
                                  PropertyNo = clientfilemember.StockCreation.PropertyNo ?? "",
                                  RequestId = ta.RequestId,
                                  ApprovalUIID = ta.ApprovalUIId
                              })
                              .ToList() ?? new List<PreSaleInboxDTO>();

                if (result?.Count > 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "Not Found",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetInboxtransferReceiptListForApprovalByUserId")]
        public IActionResult GetInboxtransferReceiptListForApprovalByUserId(int userId)
        {
            try
            {
                var result = (from transferReceipt in _db.TransferHistery.Include(x => x.StockCreation)
                                                         .Include(x => x.MemberProfile)
                                                         .Where(x => x.IsDeleted == false
                                                          && x.IsGovtProcessingTaxRequested == true
                                                          && x.IsGovtProcessingTaxApproved != true)
                              join ta in _db.TestApproval.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.TransferReceipt && x.ApprovalStatus == UHelper.ApprovalStatus(2))
                              on transferReceipt.Id equals ta.RequestId
                              where ta.UserId == userId

                              select new PreSaleInboxDTO
                              {
                                  Id = transferReceipt.Id,
                                  MemberName = transferReceipt.MemberProfile.MemberName ?? "",
                                  Cnic = transferReceipt.MemberProfile.Cnic ?? "",
                                  MobileNo = transferReceipt.MemberProfile.Mobile ?? "",
                                  RegistrationNo = transferReceipt.StockCreation.RegistrationNo ?? "",
                                  PropertyNo = transferReceipt.StockCreation.PropertyNo ?? "",
                                  RequestId = ta.RequestId,
                                  ApprovalUIID = ta.ApprovalUIId,
                                  ReciptId = transferReceipt.ReciptPrpcessingId
                              })
                              .ToList() ?? new List<PreSaleInboxDTO>();

                if (result?.Count > 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "Not Found",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetInboxDemandNoteListForApprovalByUserId")]
        public IActionResult GetInboxDemandNoteListForApprovalByUserId(int userId)
        {
            try
            {
                var result = (from demandNote in _db.DemandNote.Where(x => x.IsActive == true
                                                      && x.IsDemandNoteRequested == true
                                                      && x.IsDemandNoteApproved != true
                                                       )
                              join ta in _db.TestApproval.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.DemandNote)
                              on demandNote.Id equals ta.RequestId
                              where ta.UserId == userId

                              select new
                              {
                                  demandNote.Id,
                                  demandNote.Deparment,
                                  demandNote.RequesterName,
                                  demandNote.RequiredDate,
                                  demandNote.ValidUntill,
                                  ta.RequestId,
                                  ta.ApprovalUIId
                              })
                               .ToList();

                if (result?.Count > 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "Not Found",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetInboxTransferListForApprovalByUserId")]
        public IActionResult GetInboxTransferListForApprovalByUserId(int userId)
        {
            try
            {
                var result = (from transfer in _db.TransferHistery.Where(x => x.IsActive == true
                                                      && x.IsTransferRequested == true
                                                      && x.IsTransferApproved != true
                                                       )
                              join ta in _db.TestApproval.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.Transfer && x.ApprovalStatus == UHelper.ApprovalStatus(2))
                              on transfer.Id equals ta.RequestId
                              where ta.UserId == userId

                              select new
                              {
                                  transfer.Id,
                                  transfer.MemberProfile.MemberName,
                                  transfer.MemberProfile.Mobile,
                                  transfer.StockCreation.PropertyNo,
                                  transfer.StockCreation.RegistrationNo,
                                  ta.RequestId,
                                  ta.ApprovalUIId
                              })
                               .ToList();

                if (result?.Count > 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "Not Found",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetInboxtransferLetterListForApprovalByUserId")]
        public IActionResult GetInboxtransferLetterListForApprovalByUserId(int userId)
        {
            try
            {
                var result = (from transfer in _db.TransferHistery.Where(x => x.IsActive == true
                                                      && x.IsTransferRequested == true
                                                      && x.IsTransferApproved != true
                                                       )
                              join ta in _db.TestApproval.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.Transfer && x.ApprovalStatus == UHelper.ApprovalStatus(2))
                              on transfer.Id equals ta.RequestId
                              where ta.UserId == userId

                              select new
                              {
                                  transfer.Id,
                                  transfer.MemberProfile.MemberName,
                                  transfer.MemberProfile.Mobile,
                                  transfer.MemberProfile.Cnic,
                                  transfer.StockCreation.PropertyNo,
                                  transfer.StockCreation.RegistrationNo,
                                  ta.RequestId,
                                  ta.ApprovalUIId
                              })
                               .ToList();

                if (result?.Count > 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "Not Found",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }
    }
}
