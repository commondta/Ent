using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using B_Utility.Common.Enums;
using HRMS_Web.Models.DTOs;
using HRMS_Web.Models.DTOs.SPDtos;
using HRMS_Web.Services.AlertService;
using HRMS_Web.Services.SMSService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Web.Http.Results;

namespace HRMS_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class NDCRequestForMemberController : ControllerBase
    {
        private readonly DataBase_Context _db;
        private readonly IAlertService alertService;
        private readonly ISMSService _sMSService;
        ApprovalBLL _approvalBLL;
        CommonBLL _commonBLL;
        public NDCRequestForMemberController(DataBase_Context db,IAlertService alertService,ISMSService sMSService)
        {
            _db = db;
            this.alertService = alertService;
            _sMSService = sMSService;
            _approvalBLL = new ApprovalBLL(_db);
            _commonBLL = new CommonBLL(_db);
        }

        // call this when user focus out from cnic field
        [HttpGet]
        [Route("/api/NDCRequestForMember/GetNDCRequestForMemberByCnic")]
        public IActionResult GetNDCRequestForMemberByCnic(string cnic)
        {
            try
            {
                var result = _db.MemberProfile.Where(x => !x.IsDeleted
                                                   && x.Cnic == cnic
                                                   && x.CnicExpiryDate <= DateTime.Now
                                                     )
                                               .SingleOrDefault();
                if (result == null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Enter Valid Cnic",
                        Data = null
                    });

                }
                else
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result.Id
                    });
                }
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetNDCChargeList")]
        public IActionResult GetNDCChargeList(int stockId)
        {
            try
            {
                var result = _db.NDCRequestForMember
                    .Where(x => !x.IsDeleted &&
                                x.StockCreationId == stockId &&
                                x.IsRequestedClosed != true &&
                                x.IsCanceled != true)
                    .Include(x => x.NDCRequestForMemberCharges)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();

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
        [Route("GetAllFilterNDCMember")]
        public IActionResult GetAllFilterNDCMember()
        {
            try
            {
                var result = _db.NDCRequestForMember.Where(x => !x.IsDeleted)
                                                    .Select(x => new
                                                    {
                                                        x.Id,
                                                        x.MemberProfile.MemberName,
                                                        x.MemberProfile.Cnic,
                                                        x.StockCreation.RegistrationNo,
                                                        x.StockCreation.PropertyNo,
                                                        x.IsCanceled,
                                                        x.CreatedOn
                                                    })
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
        [Route("GetAllFilterNDCDealer")]
        public IActionResult GetAllFilterNDCDealer()
        {
            try
            {
                var result = _db.NDCRequestForMember.Where(x => !x.IsDeleted && x.DealerCode != null)
                                                    .Select(x => new
                                                    {
                                                        x.Id,
                                                        x.MemberProfile.MemberName,
                                                        x.MemberProfile.Cnic,
                                                        x.StockCreation.RegistrationNo,
                                                        x.StockCreation.PropertyNo,
                                                        x.IsCanceled,
                                                        x.CreatedOn
                                                    })
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
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _db.NDCRequestForMember.Where(x => !x.IsDeleted)
                                                       .Include(x => x.NDCRequestForMemberCharges.Where(x => !x.IsDeleted))
                                                       .Include(x => x.NDCRequestForMemberAttachments.Where(x => !x.IsDeleted))
                                                       .Include(x => x.TransferType)
                                                       .Include(x => x.StockCreation)
                                                       .Include(x => x.MemberProfile)
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
        [Route("Get")]
        public IActionResult Get(int id)
        {
            try
            {
                var result = _db.NDCRequestForMember.Where(x => !x.IsDeleted && x.Id == id)
                                                       .Include(x => x.NDCRequestForMemberCharges.Where(x => !x.IsDeleted))
                                                       .Include(x => x.NDCRequestForMemberAttachments.Where(x => !x.IsDeleted))
                                                       .Include(x => x.TransferType)
                                                       .Include(x => x.StockCreation)
                                                       .Include(x => x.MemberProfile)
                                                       .FirstOrDefault();

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
        [Route("GetNDCPrint")]
        public IActionResult GetNDCPrint(int id)
        {
            try
            {
                var result = _db.NDCRequestForMember.Where(x => !x.IsDeleted && x.Id == id)
                                                       .Include(x => x.TransferType)
                                                       .Include(x => x.StockCreation)
                                                       .Include(x => x.MemberProfile)
                                                       .Select(x=> new
                                                       {
                                                           RegistrationNo = x.StockCreation.RegistrationNo ?? "N/A",
                                                           PropertyNo = x.StockCreation.PropertyNo ?? "N/A",
                                                           x.NDCRequestType,
                                                           x.CreatedOn,
                                                           x.MemberProfile.MemberName,
                                                           TimeSlot = $"{x.SlotHour} - {x.SlotMintues}"
                                                       })
                                                       .FirstOrDefault();

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
        [Route("AddNewNDCRequestForMember")]
        public async Task<IActionResult> AddNewNDCRequestForMemberAsync(NDCRequestForMember model)
        {
            try
            {
                var isSoftLockActive = _commonBLL.IsSoftLockActive((int)model.StockCreationId, (int)SoftLocks.No_Transfer);

                if (isSoftLockActive.IsFound)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = isSoftLockActive.message,
                        Data = null
                    });
                }

                bool isApprovalActive = true;

                var approvalStatus = _db.ApprovalUI.Find((int)ApprovalUIIds.NDCRequestForMember);
                if (approvalStatus != null)
                {
                    if (approvalStatus.Checked != true)
                    {
                        isApprovalActive = false;
                    }
                }

                if (model.TransferTypeID == 8 || model.TransferTypeID == 11 || model.TransferTypeID == 20 || model.OldTranser == "Yes")
                    isApprovalActive = false;


                var approvalSetup = _db.ApprovalSetup.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.NDCRequestForMember).ToList();
                if (approvalSetup.Count <= 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Approval setup not defined or In-active",
                        Data = null
                    });
                }

                

                foreach (var approvalSetupItem in approvalSetup)
                {
                    var approvalSetupId = approvalSetupItem?.Id;

                    if (approvalSetupId != null)
                    {
                        var approvalUsers = _db.ApprovalUsers
                            .Where(a => a.ApprovalSetupId == approvalSetupId)
                            .Join(
                                _db.PMSUser,
                                a => a.UserId,
                                p => p.Id,
                                (a, p) => new { ApprovalUser = a, PMSUser = p }
                            )
                            .ToList();

                        foreach (var item in approvalUsers)
                        {
                            if (item.PMSUser.DEPARTMENT_DESC == "Building Control")
                            {
                                item.ApprovalUser.IsActive = model.BCApprovalRequired == "Yes" ? true : false;
                            }
                        }
                    }
                }


                    var existing = _db.NDCRequestForMember.Where(x => x.StockCreationId == model.StockCreationId && x.IsRequestedClosed != true &&
                                                                  x.IsCanceled != true).FirstOrDefault();
                if (existing != null && existing?.ValidityDate >= DateTime.Now)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Request already exist",
                        Data = null
                    });
                }

                var property = _db.StockCreations.Where(x => x.ID == model.StockCreationId).FirstOrDefault();
                if (property == null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Property Not Exist Please check Registration Number",
                        Data = null
                    });
                }

                if (!string.IsNullOrEmpty(model.DealerCode))
                {
                    var dealer = _db.Dealers.Where(x => x.Id == Convert.ToInt32(model.DealerCode)).FirstOrDefault();
                    if (dealer == null)
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.NotFound,
                            Message = "Dealer Not Exist",
                            Data = null
                        });
                    }
                }

                if (property.MemberProfileId != model.MemberProfileId)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Member Not Related With Property check Member Code",
                        Data = null
                    });
                }

                model.ValidityDate = DateTime.Now.AddDays(120);
                model.IsActive = true;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = model.CreatedBy;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;

                if (model.NDCRequestForMemberCharges?.Count() > 0)
                {
                    foreach (var item in model.NDCRequestForMemberCharges)
                    {
                        item.ModifiedBy = model.ModifiedBy;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                if (model.NDCRequestForMemberAttachments?.Count() > 0)
                {
                    foreach (var item in model.NDCRequestForMemberAttachments)
                    {
                        item.ModifiedBy = model.ModifiedBy;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                _db.NDCRequestForMember.Add(model);

                var stock = _db.StockCreations.Where(x => x.ID == model.StockCreationId).Include(x => x.MemberProfile).Select(x => new { x.RegistrationNo,x.PropertyNo,x.MemberProfile.Mobile, x.MemberProfile.MemberName }).FirstOrDefault();
                string narration = $"NDC Request of MemberName: {stock.MemberName} having ReferenceNo: {stock.RegistrationNo} submitted by {model.LastModifiedUserName}";
                alertService.PushAlert(1, narration);

                _db.SaveChanges();

                NDCRequestForMember nDCRequestForMember = (NDCRequestForMember)_db.NDCRequestForMember.Where(x => x.Id == model.Id)
                                                                                                      .FirstOrDefault();
                if (nDCRequestForMember != null)
                {
                    nDCRequestForMember.IsNDCRequestForMemberRequested = true;

                    if (isApprovalActive)
                        _approvalBLL.AddNewApprovalSetup(nDCRequestForMember.Id, (int)ApprovalUIIds.NDCRequestForMember, model.BCApprovalRequired == "Yes" ? false : true);
                    else
                        nDCRequestForMember.IsNDCRequestForMemberApproved = true;

                    _db.SaveChanges();
                }

                string message = $@"
                                    Subject: NDC Status Notification
                                    
                                    Dear customer,  
                                    
                                    Your NDC has been received / applied on {model.CreatedOn.ToString("dd-MM-yyyy")}.
                                    
                                    NDC Type: {model.NDCRequestType}
                                    Ref No: {stock.RegistrationNo}
                                    Owner Name: {stock.MemberName}
                                    Plot No: {stock.PropertyNo}
                                    ";
                if (!string.IsNullOrEmpty(stock.Mobile))
                {
                    try
                    {
                        await _sMSService.SendSingleSmsAsync(message, stock.Mobile);
                    }
                    catch (Exception ex)
                    {
                    }
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = isApprovalActive ? "NDC Request For Member added succesfully and moved for approval" : "NDC Request For Member added succesfully",
                    Data = null
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPut]
        [Route("UpdateNDCRequestForMember")]
        public IActionResult UpdateNDCRequestForMember(NDCRequestForMember model)
        {
            try
            {
                var data = _db.NDCRequestForMember.Find(model.Id);

                if (data != null)
                {
                    data.TransferTypeID = model.TransferTypeID;
                    data.NDCRequestType = model.NDCRequestType;
                    data.Outstation = model.Outstation;
                    data.ApplyStation = model.ApplyStation;
                    data.SlotDate = model.SlotDate;
                    data.SlotHour = model.SlotHour;
                    data.SlotMintues = model.SlotMintues;
                    data.ValidityDate = model.ValidityDate;
                    data.DealerCode = model.DealerCode;
                    data.DealerName = model.DealerName;
                    data.DealerCode = model.DealerCode;
                    data.DealerName = model.DealerName;
                    data.EstateName = model.EstateName;
                    data.Processing = model.Processing;
                    data.TransferPurpose = model.TransferPurpose;
                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;
                    data.LastModified = DateTime.Now;
                    data.CreatedOn = model.CreatedOn;

                    var charges = _db.NDCRequestForMemberCharges.Where(x => x.NDCRequestForMemberId == model.Id).ToList();

                    _db.NDCRequestForMemberCharges.RemoveRange(charges);


                    if (model.NDCRequestForMemberCharges?.Count() > 0)
                    {
                        foreach (var item in model.NDCRequestForMemberCharges)
                        {
                            item.NDCRequestForMemberId = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.NDCRequestForMemberCharges.AddRange(model.NDCRequestForMemberCharges);
                    }


                    var attchments = _db.NDCRequestForMemberAttachments.Where(x => x.NDCRequestForMemberId == model.Id).ToList();

                    _db.NDCRequestForMemberAttachments.RemoveRange(attchments);


                    if (model.NDCRequestForMemberAttachments?.Count() > 0)
                    {
                        foreach (var item in model.NDCRequestForMemberAttachments)
                        {
                            item.NDCRequestForMemberId = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.NDCRequestForMemberAttachments.AddRange(model.NDCRequestForMemberAttachments);
                    }

                    _db.SaveChanges();

                    //if (model.NDCRequestForMemberCharges?.Count() > 0)
                    //{
                    //    var result = _db.NDCRequestForMemberCharges.Where(x => x.NDCRequestForMemberId == model.Id).ToList();

                    //    _db.NDCRequestForMemberCharges.RemoveRange(result);
                    //    _db.SaveChanges();
                    //}

                    //if (model.NDCRequestForMemberCharges?.Count() > 0)
                    //{
                    //    foreach (var item in model.NDCRequestForMemberCharges)
                    //    {
                    //        item.NDCRequestForMemberId = data.Id;
                    //        item.ModifiedBy = item.ModifiedBy;
                    //        item.LastModified = DateTime.Now;
                    //        item.IsActive = true;
                    //        item.IsDeleted = false;
                    //    }

                    //    _db.NDCRequestForMemberCharges.AddRange(model.NDCRequestForMemberCharges);
                    //    _db.SaveChanges();
                    //}

                    //if (model.NDCRequestForMemberAttachments?.Count() > 0)
                    //{
                    //    var result = _db.NDCRequestForMemberAttachments.Where(x => x.NDCRequestForMemberId == model.Id).ToList();

                    //    _db.NDCRequestForMemberAttachments.RemoveRange(result);
                    //    _db.SaveChanges();
                    //}

                    //if (model.NDCRequestForMemberAttachments?.Count() > 0)
                    //{
                    //    foreach (var item in model.NDCRequestForMemberAttachments)
                    //    {
                    //        item.NDCRequestForMemberId = data.Id;
                    //        item.ModifiedBy = item.ModifiedBy;
                    //        item.LastModified = DateTime.Now;
                    //        item.IsActive = true;
                    //        item.IsDeleted = false;
                    //    }

                    //    _db.NDCRequestForMemberAttachments.AddRange(model.NDCRequestForMemberAttachments);
                    //    _db.SaveChanges();
                    //}

                    var stock = _db.StockCreations.Where(x => x.ID == model.StockCreationId).Include(x => x.MemberProfile).Select(x => new { x.RegistrationNo, x.MemberProfile.MemberName }).FirstOrDefault();
                    string narration = $"NDC Request of MemberName: {stock.MemberName} having ReferenceNo: {stock.RegistrationNo} updated by {model.LastModifiedUserName}";
                    alertService.PushAlert(1, narration);
                }
                else
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Not Found",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "record updated successfully",
                    Data = null
                });

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }



        [HttpDelete]
        [Route("DeleteNDCRequestForMember")]
        public IActionResult DeleteNDCRequestForMember(int id)
        {
            try
            {
                var model = _db.NDCRequestForMember.Find(id);

                if (model != null)
                {
                    model.ModifiedBy = model.ModifiedBy;
                    model.LastModified = DateTime.Now;
                    model.IsActive = false;
                    model.IsDeleted = true;            

                    var nDCRequestForMemberCharges = _db.NDCRequestForMemberCharges.Where(x => x.NDCRequestForMemberId == model.Id).ToList();

                    if (nDCRequestForMemberCharges?.Count > 0)
                    {
                        foreach (var item in nDCRequestForMemberCharges)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;
                        }
                    }

                    var nDCRequestForMemberAttachments = _db.NDCRequestForMemberAttachments.Where(x => x.NDCRequestForMemberId == model.Id).ToList();

                    if (nDCRequestForMemberAttachments?.Count > 0)
                    {
                        foreach (var item in nDCRequestForMemberAttachments)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;
                        }
                    }

                    _db.SaveChanges();
                }
                else
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Not Found",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Soft Deleted Successfully",
                    Data = model
                });

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

    }
}

