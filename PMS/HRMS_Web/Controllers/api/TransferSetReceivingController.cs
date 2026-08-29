using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using B_Utility.Common.Enums;
using HRMS_Web.Extensions;
using HRMS_Web.Models.DTOs;
using HRMS_Web.Services.AlertService;
using HRMS_Web.Services.SMSService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.Xml;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferSetReceivingController : ControllerBase
    {
        private readonly DataBase_Context _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAlertService alertService;
        private readonly ISMSService _sMSService;
        CommonBLL _commonBLL;

        ApprovalBLL _approvalBLL;
        public TransferSetReceivingController(DataBase_Context db,
            IHttpContextAccessor httpContextAccessor,
            IAlertService alertService, ISMSService sMSService)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            this.alertService = alertService;
            _sMSService = sMSService;
            _approvalBLL = new ApprovalBLL(_db);
            _commonBLL = new CommonBLL(_db);

        }

        [HttpGet]
        [Route("GetAllTransferSetReceiving")]
        public IActionResult GetAllTransferSetReceiving()
        {
            try
            {
                var result = _db.TransferSetReceivings.Where(x => !x.IsDeleted &&
                                                           x.IsActive == true
                                                         )
                                                        .Select(x => new
                                                        {
                                                            x.Id,
                                                            RegistrationNo = x.StockCreation.RegistrationNo ?? "N/A",
                                                            PropertyNo = x.StockCreation.PropertyNo ?? "N/A",
                                                            MemberName = x.MemberProfile.MemberName ?? "N/A",
                                                            Cnic = x.MemberProfile.Cnic ?? "N/A"
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
        [Route("GetTransferSetReceivingPrint")]
        public IActionResult GetTransferSetReceivingPrint(int id)
        {
            try
            {
                var transfer = _db.TransferSetReceivings
                    .Where(x => !x.IsDeleted && x.Id == id)
                    .Include(x => x.StockCreation)
                    .Include(x => x.MemberProfile)
                    .FirstOrDefault();

                if (transfer == null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Record not found",
                        Data = null
                    });
                }

                // 🔹 Latest NDC data by StockCreationId
                var ndc = GetNDCData((int)transfer.StockCreationId);
                var ndcClearedOn = GetNDCClreadDate((int)transfer.StockCreationId);

                var result = new
                {
                    MemberName = transfer.MemberProfile.MemberName,

                    Sector = _db.Sectors
                        .Where(y => y.ID == Convert.ToInt32(transfer.StockCreation.PrefixProperty))
                        .Select(y => y.Description)
                        .FirstOrDefault() ?? "N/A",

                    TransferType = ndc?.TransferType ?? transfer.TransferType,

                    Depositor = transfer.Depositor,

                    DealerName = ndc?.DealerName ?? transfer.DealerName,

                    DealerCode =
                        _db.Dealers
                            .Where(d => d.Id == Convert.ToInt32(transfer.DealerCode))
                            .Select(d => d.DelaerRegisrationCode)
                            .FirstOrDefault() ?? ndc?.DealerCode,

                    DepositTime = transfer.CreatedOn,
                    ReceivingDate = transfer.CreatedOn.ToString("dd-MM-yyyy"),
                    NDCDate = transfer.CreatedOn,

                    transfer.StockCreation.RegistrationNo,

                    SlotDate = ndc?.SlotDate?.ToString("dd-MM-yyyy")
                                ?? transfer.SlotDate?.ToString("dd-MM-yyyy")
                                ?? "N/A",

                    ndcClearedOn = ndcClearedOn,

                    Slot = ndc != null
                   ? $"{(int.Parse(ndc.SlotHour) > 12 ? int.Parse(ndc.SlotHour) - 12 : int.Parse(ndc.SlotHour))}:{int.Parse(ndc.SlotMintues):D2} {(int.Parse(ndc.SlotHour) >= 12 ? "PM" : "AM")}"
                   : $"{(int.Parse(transfer.SlotHour) > 12 ? int.Parse(transfer.SlotHour) - 12 : int.Parse(transfer.SlotHour))}:{int.Parse(transfer.SlotMintues):D2} {(int.Parse(transfer.SlotHour) >= 12 ? "PM" : "AM")}",

                ndc?.Day,
                    ndc?.PossessionStatus,
                    ndc?.ValidateDate,
                    ndc?.EstateName
                };

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }


        private NDCReadDto GetNDCData(int id)
        {
            var result = _db.NDCRequestForMember.Where(x => !x.IsDeleted && x.StockCreationId == id && x.IsRequestedClosed != true)
                                                .Include(x => x.TransferType)
                                                .OrderByDescending(x => x.Id)
                                                .Select(x => new NDCReadDto
                                                {
                                                    NDCRequestType = x.NDCRequestType,
                                                    TransferType = x.TransferType.Description,
                                                    SlotDate = x.SlotDate,
                                                    SlotHour = x.SlotHour,
                                                    SlotMintues = x.SlotMintues,
                                                    Day = x.Day,
                                                    PossessionStatus = x.PossessionStatus,
                                                    ValidateDate = x.ValidityDate,
                                                    DealerCode = x.DealerCode,
                                                    DealerName = x.DealerName,
                                                    EstateName = x.EstateName

                                                })
                                                .FirstOrDefault();
            return result;
        }

        private string GetNDCClreadDate(int id)
        {
            var result = _db.NDC1.Where(x => !x.IsDeleted && x.StockCreationId == id)
                                                .OrderByDescending(x => x.Id)
                                                .Select(x => x.CreatedOn)
                                                .FirstOrDefault();
            return result.ToString("dd-MM-yyyy")
                                ?? result.ToString("dd-MM-yyyy")
                                ?? "N/A";
        }

        //[HttpGet]
        //[Route("GetTransferSetReceiving")]
        //public IActionResult GetTransferSetReceiving(int id)
        //{
        //    try
        //    {
        //        var result = _db.TransferSetReceivings.Where(x => !x.IsDeleted &&
        //                                                   x.Id == id
        //                                                 )
        //                                                .Include(x => x.StockCreation)
        //                                                .Include(x => x.MemberProfile)
        //                                                .Include(x => x.TransferSetReceivingAttachments)
        //                                                .FirstOrDefault();
        //        return Ok(new ApiResponse<object>
        //        {
        //            Code = ResponseCode.Success,
        //            Message = "Success",
        //            Data = result
        //        });
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
        //    }
        //}

        [HttpGet]
        [Route("GetTransferSetReceiving")]
        public IActionResult GetTransferSetReceiving(int id)
        {
            try
            {
                var transfer = _db.TransferSetReceivings
                    .Where(x => !x.IsDeleted && x.Id == id)
                    .Include(x => x.StockCreation)
                    .Include(x => x.MemberProfile)
                    .Include(x => x.TransferSetReceivingAttachments)
                    .FirstOrDefault();

                if (transfer == null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Record not found",
                        Data = null
                    });
                }

                var ndc = GetNDCData((int)transfer.StockCreationId);

                var result = new
                {
                    id = transfer.Id,
                    transferType = transfer.TransferType,
                    depositor = transfer.Depositor,

                    dealerCode = ndc?.DealerCode ?? transfer.DealerCode,
                    dealerName = ndc?.DealerName ?? transfer.DealerName,

                    ndcRequestType = ndc?.NDCRequestType,
                    possessionStatus = ndc?.PossessionStatus,
                    estateName = ndc?.EstateName,
                    validateDate = transfer?.EffectiveDate,
                    day = ndc?.Day,

                    slotDate = ndc?.SlotDate ?? transfer.SlotDate,
                    slotHour = ndc?.SlotHour ?? transfer.SlotHour,
                    slotMintues = ndc?.SlotMintues ?? transfer.SlotMintues,

                    applyStation = transfer.ApplyStation,
                    setReceivingStatus = transfer.SetReceivingStatus,
                    propertyTaxYear = 0,
                    block = transfer.Block,
                    size = transfer.Size,
                    category = transfer.Category,

                    stockCreationId = transfer.StockCreationId,
                    memberProfileId = transfer.MemberProfileId,

                    memberProfile = new
                    {
                        id = transfer.MemberProfile.Id,
                        memberName = transfer.MemberProfile.MemberName,
                        cnic = transfer.MemberProfile.Cnic
                    },

                    stockCreation = new
                    {
                        id = transfer.StockCreation.ID,
                        registrationNo = transfer.StockCreation.RegistrationNo,
                        propertyNo = transfer.StockCreation.PropertyNo,
                        prefixProperty = transfer.StockCreation.PrefixProperty
                    },

                    attachments = transfer.TransferSetReceivingAttachments,

                    createdOn = transfer.CreatedOn
                };

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }



        [HttpPost]
        [Route("/api/TransferSetReceiving/SaveTransferSetReceiving")]
        public async Task<IActionResult> SaveTransferSetReceivingAsync(TransferSetReceiving model)
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
                //bool isApprovalActive = true;

                //var approvalStatus = _db.ApprovalUI.Find((int)ApprovalUIIds.FileVerification);
                //if (approvalStatus != null)
                //{
                //    if (approvalStatus.Checked != true)
                //    {
                //        isApprovalActive = false;
                //    }
                //}

                //var approvalSetup = _db.ApprovalSetup.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.FileVerification).ToList();
                //if (approvalSetup.Count <= 0 && isApprovalActive == true)
                //{
                //    return Ok(new ApiResponse<object>
                //    {
                //        Code = ResponseCode.NotFound,
                //        Message = "Approval setup not defined or In-active",
                //        Data = null
                //    });
                //}

                //bool planExist = _db.TransferSetReceivings.Any(x => x.StockCreationId == model.StockCreationId);
                //if (planExist)
                //{
                //    return Ok(new ApiResponse<object>
                //    {
                //        Code = ResponseCode.NotFound,
                //        Message = "Transfer Set Receiving Already Exist Please update it",
                //        Data = null
                //    });
                //}

                model.IsRequestClosed = false;
                model.IsActive = true;
                model.CreatedOn = DateTime.Now.Date.Add(DateTime.Now.TimeOfDay);
                model.CreatedBy = model.CreatedBy;
                model.LastModified = model.CreatedOn.Date.Add(DateTime.Now.TimeOfDay);
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;

                var attachmentresult = _db.TransferSetReceivingAttachments.Where(x => x.TransferSetReceiving.StockCreationId == model.StockCreationId).ToList();

                foreach (var attachment in attachmentresult)
                {
                    var existingFilePath = attachment.Document;

                    bool fileExistsInNewModel = model.TransferSetReceivingAttachments.Any(x => x.Document == existingFilePath);

                    if (!fileExistsInNewModel && !string.IsNullOrEmpty(existingFilePath))
                    {
                        existingFilePath.DeleteFile();
                    }

                    _db.TransferSetReceivingAttachments.Remove(attachment);
                }

                if (model.TransferSetReceivingAttachments?.Count > 0)
                {
                    var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";

                    foreach (var item in model.TransferSetReceivingAttachments)
                    {
                        if (!string.IsNullOrEmpty(item.Document))
                        {
                            var savedPath = await item.Document.SaveBase64FileAsync();

                            if (!savedPath.StartsWith(baseUrl, StringComparison.OrdinalIgnoreCase))
                            {
                                item.Document = $"{baseUrl}{savedPath}";
                            }
                            else
                            {
                                item.Document = savedPath;
                            }
                        }
                        else
                        {
                            item.Document = "";
                        }

                        item.ModifiedBy = model.ModifiedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.LastModified = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }

                    _db.TransferSetReceivingAttachments.AddRange(model.TransferSetReceivingAttachments);
    
                }

                
                _db.TransferSetReceivings.Add(model);

                NDC1 nDC1 = _db.NDC1.Find(model.Ndc1Id);
                if (nDC1 != null)
                {
                    nDC1.IsRequestClosed = true;
                }

                var stock = _db.StockCreations.Where(x => x.ID == model.StockCreationId).Include(x => x.MemberProfile).Select(x => new { x.RegistrationNo, x.PropertyNo, x.MemberProfile.Mobile, x.MemberProfile.MemberName }).FirstOrDefault();
                string narration = $"Transfer Set of MemberName: {stock.MemberName} having ReferenceNo: {stock.RegistrationNo} is received by {model.LastModifiedUserName}";
                alertService.PushAlert(3, narration);

                _db.SaveChanges();

                //string message = string.Empty;
                //FileVerificationRequest fileVerificationRequest = (FileVerificationRequest)_db.FileVerificationRequests.Where(x => x.Id == model.Id)
                //                                                                                      .FirstOrDefault();
                //if (fileVerificationRequest != null)
                //{
                //    fileVerificationRequest.IsFileVerificationRequested = true;
                //    _db.SaveChanges();
                //    if (isApprovalActive == true)
                //    {
                //        bool result = _approvalBLL.AddNewApprovalSetup(fileVerificationRequest.Id, (int)ApprovalUIIds.FileVerification);
                //        message = "File Verification Request added succesfully and moved for approval";
                //        if (result)
                //        {
                //            return Ok(new ApiResponse<object>
                //            {
                //                Code = ResponseCode.Success,
                //                Message = message,
                //                Data = null
                //            });
                //        }
                //    }
                //    else
                //    {
                //        fileVerificationRequest.IsFileVerificationApproved = true;
                //        _db.SaveChanges();

                //        message = "File Verification Request added succesfully";

                //        return Ok(new ApiResponse<object>
                //        {
                //            Code = ResponseCode.Success,
                //            Message = message,
                //            Data = null
                //        });
                //    }
                //}

                string message = $@"
                                 Dear member,
                                 
                                 Your Documents for Transfer has been received on {model.CreatedOn.ToString("dd-MM-yyyy")} in respect of Ref No {stock.RegistrationNo}.
                                 You will be informed if any further information is required.
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
                    Message = "Transfer Set Receiving Added",
                    Data = null
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPut]
        [Route("/api/TransferSetReceiving/UpdateTransferSetReceiving")]
        public async Task<IActionResult> UpdateTransferSetReceivingAsync(TransferSetReceiving model)
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

                var data = _db.TransferSetReceivings.Find(model.Id);

                if (data != null)
                {
                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;
                    data.Depositor = model.Depositor;
                    data.SetReceivingStatus = model.SetReceivingStatus;
                    data.LastModified = model.CreatedOn.Date.Add(DateTime.Now.TimeOfDay);

                    _db.Entry(data).State = EntityState.Modified;


                    var attachmentresult = _db.TransferSetReceivingAttachments.Where(x => x.TransferSetReceivingId == model.Id).ToList();

                    foreach (var attachment in attachmentresult)
                    {
                        var existingFilePath = attachment.Document;

                        bool fileExistsInNewModel = model.TransferSetReceivingAttachments.Any(x => x.Document == existingFilePath);

                        if (!fileExistsInNewModel && !string.IsNullOrEmpty(existingFilePath))
                        {
                            existingFilePath.DeleteFile();
                        }

                        _db.TransferSetReceivingAttachments.Remove(attachment);
                    }

                    if (model.TransferSetReceivingAttachments?.Count > 0)
                    {
                        var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";

                        foreach (var item in model.TransferSetReceivingAttachments)
                        {
                            if (!string.IsNullOrEmpty(item.Document))
                            {
                                var savedPath = await item.Document.SaveBase64FileAsync();

                                if (!savedPath.StartsWith(baseUrl, StringComparison.OrdinalIgnoreCase))
                                {
                                    item.Document = $"{baseUrl}{savedPath}";
                                }
                                else
                                {
                                    item.Document = savedPath;
                                }
                            }
                            else
                            {
                                item.Document = "";
                            }
                            item.TransferSetReceivingId = model.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.TransferSetReceivingAttachments.AddRange(model.TransferSetReceivingAttachments);

                    }
                    
                    var stock = _db.StockCreations.Where(x => x.ID == model.StockCreationId).Include(x => x.MemberProfile).Select(x => new { x.RegistrationNo, x.MemberProfile.MemberName }).FirstOrDefault();
                    string narration = $"Transfer Set of MemberName: {stock.MemberName} having ReferenceNo: {stock.RegistrationNo} is updated by {model.LastModifiedUserName}";
                    alertService.PushAlert(3, narration);

                    _db.SaveChanges();
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Transfer Set Receiving Updated",
                    Data = null
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

    }
}
