using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using B_Utility.Common.Enums;
using HRMS_Web.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileVerificationController : ControllerBase
    {
        private readonly DataBase_Context _db;
        ApprovalBLL _approvalBLL;
        public FileVerificationController(DataBase_Context db)
        {
            _db = db;
            _approvalBLL = new ApprovalBLL(_db);
        }


        [HttpGet]
        [Route("GetClientFilebyId")]
        public IActionResult GetClientFilebyId(int id)
        {
            try
            {
                    var result = _db.ClientFileVerification.Where(x => x.Id == id)
                                                .Include(x => x.MemberProfile)
                                                .Include(x => x.StockCreation)
                                                .Select(x => new ClientFileDto
                                                {
                                                    DocNum = x.Id,
                                                    stockId = x.StockCreationId,
                                                    MemberName = x.MemberProfile.MemberName,
                                                    Relationship = x.MemberProfile.Relationship,
                                                    RelationshipWith = x.MemberProfile.RelationshipWith,
                                                    Cnic = x.MemberProfile.Cnic == null ? "N/A" : x.MemberProfile.Cnic,
                                                    Mobile = x.MemberProfile.Mobile,
                                                    Image = x.ImageURL,
                                                    PermanentAddress = x.MemberProfile.PermanentAddress,
                                                    ReceivedBy = x.ReceivedBy,
                                                    RecieverCNIC = x.RecieverCNIC,
                                                    RecieverFatherName = x.RecieverFatherName,
                                                    RecieverMobile = x.RecieverMobile,
                                                    RegistrationNo = x.StockCreation.RegistrationNo,
                                                    PropertyNo = x.StockCreation.PropertyNo,
                                                    Area = x.StockCreation.ActualSize,
                                                    UnitArea = x.StockCreation.ActualSizeUnit,
                                                    Sqft = x.StockCreation.coveredArea == null ? "N/A" : x.StockCreation.coveredArea.ToString(),
                                                    Type = _db.PropertyTypes.Where(y => y.ID == Convert.ToInt32(x.StockCreation.Type)).FirstOrDefault().Description ?? "N/A",
                                                    Block = _db.Blocks.Where(y => y.ID == Convert.ToInt32(x.StockCreation.Block)).FirstOrDefault().Description ?? "N/A",
                                                    Nature = _db.Natures.Where(y => y.ID == Convert.ToInt32(x.StockCreation.Nature)).FirstOrDefault().Description == "Plot" && x.StockCreation.ConstracutionStatus == "Constructed" ? "House" : _db.Natures.Where(y => y.ID == Convert.ToInt32(x.StockCreation.Nature)).FirstOrDefault().Description,
                                                    Floor = _db.Floors.Where(y => y.ID == Convert.ToInt32(x.StockCreation.Floor)).FirstOrDefault().Description ?? "N/A",
                                                    docDate = x.CreatedOn,
                                                })
                                                .FirstOrDefault();
                if(result != null)
                {
                    result.JointMembers = GetJointMembersByStockId((int)result.stockId);

                    var file = _db.ClientFileVerification.Find(id);
                    if (file != null)
                    {
                        file.IsFilePrint = true;
                        _db.SaveChanges();
                    }
                }
                
                return Ok(result);

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        private List<JointMemberDto> GetJointMembersByStockId(int stockId)
        {
                List<JointMemberDto> jointMembers = new List<JointMemberDto>();
                bool IsExistInTransfer = _db.TransferHistery.Any(x => x.StockCreationId == stockId);
                if (IsExistInTransfer)
                {
                    var currentPropTransfer = _db.TransferHistery.Where(x => !x.IsDeleted && x.StockCreationId == stockId)
                                                                 .OrderByDescending(x => x.Id)
                                                                 .FirstOrDefault();
                    if (currentPropTransfer != null)
                    {
                        var JointMembersTransfer = _db.TransferHisteryJointMember.Where(x => x.TransferHisteryId == currentPropTransfer.Id)
                                                                         .Select(x => new JointMemberDto
                                                                         {
                                                                             Name = x.Name,
                                                                             Cnic = x.CNIC,
                                                                             Mobile = x.Mobile
                                                                         })
                                                                         .ToList();
                        return JointMembersTransfer;
                    }
                }
                var currentPropBooking = _db.Booking.Where(x => !x.IsDeleted && x.StockCreationId == stockId)
                                                    .FirstOrDefault();
                if (currentPropBooking != null)
                {
                    var JointMembersBooking = _db.BookingJointMember.Where(x => x.BookingId == currentPropBooking.Id)
                                                                     .Select(x => new JointMemberDto
                                                                     {
                                                                         Name = x.Name,
                                                                         Cnic = x.CNIC,
                                                                         Mobile = x.Mobile
                                                                     })
                                                                     .ToList();
                    return JointMembersBooking;
                }

                return jointMembers;
        }


        [HttpGet]
        [Route("GetFilterFileVerificationForNDC1")]
        public IActionResult GetFilterFileVerificationForNDC1()
        {
            try
            {
                var result = _db.FileVerificationRequests.Where(x => !x.IsDeleted &&
                                                           x.IsFileVerificationApproved == true &&
                                                           x.IsRequestClosed != true &&
                                                           x.IsActive != false
                                                      )
                                               .Select(x => new
                                               {
                                                   x.Id,
                                                   x.StockCreation.RegistrationNo,
                                                   x.StockCreation.PropertyNo,
                                                   x.MemberProfile.MemberName,
                                                   x.MemberProfile.Cnic,
                                                   x.CreatedOn,
                                                   x.LastModified
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
        [Route("GetFilterFileVerificationNDC1ById")]
        public IActionResult GetFilterFileVerificationNDC1ById(int ndcId)
        {
            try
            {
                var result = _db.FileVerificationRequests.Where(x => !x.IsDeleted && x.Id == ndcId)
                                               .Select(x => new
                                               {
                                                   x.Id,
                                                   StockId = x.StockCreation.ID,
                                                   x.StockCreation.RegistrationNo,
                                                   x.StockCreation.PropertyNo,
                                                   x.MemberProfile.MemberName,
                                                   x.MemberProfileId,
                                                   x.MemberProfile.Cnic,
                                                   x.MemberProfile.CnicExpiryDate,
                                                   x.StockCreation.ActualSize,
                                                   x.StockCreation.PossessionStatus,
                                                   x.StockCreation.ConstracutionStatus,
                                                   BlockName = _db.Blocks.Where(p => p.ID == (Convert.ToInt32(x.StockCreation.Block))).Select(x => x.Description).FirstOrDefault(),
                                                   CategoryName = _db.Categories.Where(p => p.ID == (Convert.ToInt32(x.StockCreation.Category))).Select(x => x.Description).FirstOrDefault()
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

        [HttpGet]
        [Route("GetAllFileRequest")]
        public IActionResult GetAllFileRequest()
        {
            try
            {
                var result = _db.FileDocDupRequests.Where(x => !x.IsDeleted &&
                                                           x.IsActive == true 
                                                         )
                                                        .Select(x => new
                                                        {
                                                            x.Id,
                                                            x.StockCreation.RegistrationNo,
                                                            x.StockCreation.PropertyNo,
                                                            x.RequestType,
                                                            x.MemberProfile.MemberName,
                                                            x.MemberProfile.Cnic,
                                                            x.CreatedOn
                                                        })
                                                        .ToList()
                                                        .OrderByDescending(x => x.Id)
                                                        .DistinctBy(x => x.RegistrationNo);

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
        [Route("GeFileRequest")]
        public IActionResult GeFileRequest(int id)
        {
            try
            {
                var result = _db.FileDocDupRequests.Where(x => !x.IsDeleted &&
                                                           x.Id == id
                                                         )
                                                        .Include(x => x.StockCreation)
                                                        .Include(x => x.MemberProfile)
                                                        .Include(x => x.FileDocDupRequestedCharges)
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
        [Route("/api/FileVerification/SaveFileDocDupRequest")]
        public IActionResult SaveFileDocDupRequest(FileDocDupRequest model)
        {
            try
            {
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

                var property = _db.StockCreations.Where(x => x.ID == model.StockCreationId)
                                                 .Include(x=>x.MemberProfile)
                                                 .FirstOrDefault();
                if (property == null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Property Not Exist Please check Registration Number",
                        Data = null
                    });
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

                //Response_Result response = new SapIntegrationController(_db).PostingARInvoiceForFileRequest(model);
                //if (response.code != 0)
                //{
                //    return Ok(new ApiResponse<object>
                //    {
                //        Code = ResponseCode.NotFound,
                //        Message = response.message,
                //        Data = null
                //    });
                //}

                model.IsRequestClosed = false;
                model.IsActive = true;
                model.CreatedOn = model.CreatedOn;
                model.CreatedBy = model.CreatedBy;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;

                _db.FileDocDupRequests.Add(model);

                FileReceivingRegister register = new FileReceivingRegister();
                   register.RegisterNo = (int)Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd"));
                   register.Registration = model.StockCreation.RegistrationNo;
                   register.Plot = model.StockCreation.PropertyNo;
                   register.Block = model.Block;
                   register.Area = model.Size;
                   register.SellerName = model.RequestType;
                   register.ModifiedBy = model.ModifiedBy;
                   register.LastModifiedUserName = model.LastModifiedUserName;
                   register.CreatedBy = model.CreatedBy;
                   register.BuyerName = property.MemberProfile.MemberName;
                   register.InternalNo = UHelper.GenerateUniqueNumber();
                   register.Remarks = "";
                   register.CreatedOn = DateTime.Now;

                _db.FileReceivingRegisters.Add(register);

                _db.SaveChanges();

                string message = string.Empty;
                FileDocDupRequest fileDocDupRequest = (FileDocDupRequest)_db.FileDocDupRequests.Where(x => x.Id == model.Id)
                                                                                                      .FirstOrDefault();
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
                        fileDocDupRequest.IsFileDocDupApproved = true;
                        fileDocDupRequest.IsFileDocDupRequested = true;
                        _db.SaveChanges();

                        message = "File Verification Request added succesfully";

                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = message,
                            Data = null
                        });
                //    }
                //}

                //return Ok(new ApiResponse<object>
                //{
                //    Code = ResponseCode.Success,
                //    Message = "Success",
                //    Data = null
                //});
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetAllFileVerificationRequest")]
        public IActionResult GetAllFileVerificationRequest()
        {
            try
            {
                var result = _db.FileVerificationRequests.Where(x => !x.IsDeleted &&
                                                           x.IsActive == true
                                                         )
                                                        .Select(x => new
                                                        {
                                                            x.Id,
                                                            x.StockCreation.RegistrationNo,
                                                            x.StockCreation.PropertyNo,
                                                            x.MemberProfile.MemberName,
                                                            x.MemberProfile.Cnic
                                                        })
                                                        .ToList()
                                                        .OrderByDescending(x => x.Id)
                                                        .DistinctBy(x => x.RegistrationNo);

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
        [Route("GeFileVerificationRequest")]
        public IActionResult GeFileVerificationRequest(int id)
        {
            try
            {
                var result = _db.FileVerificationRequests.Where(x => !x.IsDeleted &&
                                                           x.Id == id
                                                         )
                                                        .Include(x => x.StockCreation)
                                                        .Include(x => x.MemberProfile)
                                                        .Include(x => x.FileVerificationRequestCharges) 
                                                        .Include(x => x.FileVerificationAttachments)
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
        [Route("/api/FileVerification/SaveFileVerification")]
        public IActionResult SaveFileVerification(FileVerificationRequest model)
        {
            try
            {
                bool isApprovalActive = true;

                var approvalStatus = _db.ApprovalUI.Find((int)ApprovalUIIds.FileVerification);
                if (approvalStatus != null)
                {
                    if (approvalStatus.Checked != true)
                    {
                        isApprovalActive = false;
                    }
                }

                var approvalSetup = _db.ApprovalSetup.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.FileVerification).ToList();
                if (approvalSetup.Count <= 0 && isApprovalActive == true)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Approval setup not defined or In-active",
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

                if (property.MemberProfileId != model.MemberProfileId)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Member Not Related With Property check Member Code",
                        Data = null
                    });
                }
               
                model.IsRequestClosed = false;
                model.IsActive = true;
                model.CreatedOn = model.CreatedOn;
                model.CreatedBy = model.CreatedBy;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;

                _db.FileVerificationRequests.Add(model);
                _db.SaveChanges();

                string message = string.Empty;
                FileVerificationRequest fileVerificationRequest = (FileVerificationRequest)_db.FileVerificationRequests.Where(x => x.Id == model.Id)
                                                                                                      .FirstOrDefault();
                if (fileVerificationRequest != null)
                {
                    fileVerificationRequest.IsFileVerificationRequested = true;
                    _db.SaveChanges();
                    if (isApprovalActive == true)
                    {
                        bool result = _approvalBLL.AddNewApprovalSetup(fileVerificationRequest.Id, (int)ApprovalUIIds.FileVerification);
                        message = "File Verification Request added succesfully and moved for approval";
                        if (result)
                        {
                            return Ok(new ApiResponse<object>
                            {
                                Code = ResponseCode.Success,
                                Message =message,
                                Data = null
                            });
                        }
                    }
                    else
                    {

                        //Response_Result response = new SapIntegrationController(_db).PostingARInvoiceForFileVerificationRequest(model);
                        //if (response.code != 0)
                        //{
                        //    return Ok(new ApiResponse<object>
                        //    {
                        //        Code = ResponseCode.Error,
                        //        Message = response.message,
                        //        Data = null
                        //    });
                        //}
                        fileVerificationRequest.IsFileVerificationApproved = true;
                        _db.SaveChanges();

                        message = "File Verification Request added succesfully";

                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = message,
                            Data = null
                        });
                    }
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = null
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        [Route("SaveFileVerificationNDC1")]
        public IActionResult SaveFileVerificationNDC1(FileVerificationNDC1 model)
        {
            try
            {
                model.IsActive = true;
                model.CreatedOn = model.CreatedOn;
                model.CreatedBy = model.CreatedBy;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;
          
                _db.FileVerificationNDC1.Add(model);
               // _db.SaveChanges();

                FileVerificationRequest fileVerificationRequest = (FileVerificationRequest)_db.FileVerificationRequests.Where(x => x.StockCreationId == model.StockCreationId)
                                                                                                                       .OrderByDescending(x=>x.Id)
                                                                                                                       .LastOrDefault();
                fileVerificationRequest.IsRequestClosed = true;
                _db.FileVerificationRequests.Update(fileVerificationRequest);
                _db.SaveChanges();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "File Verification NDC1 Success",
                    Data = model
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }



        [HttpPost]
        [Route("/api/FileVerification/SaveClientFileVerification")]
        public IActionResult SaveClientFileVerification(ClientFileVerification model)
        {
            try
            {
                bool isApprovalActive = true;

                var approvalStatus = _db.ApprovalUI.Find((int)ApprovalUIIds.ClientFileVerification);
                if (approvalStatus != null)
                {
                    if (approvalStatus.Checked != true)
                    {
                        isApprovalActive = false;
                    }
                }

                var approvalSetup = _db.ApprovalSetup.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.ClientFileVerification).ToList();
                if (approvalSetup.Count <= 0 && isApprovalActive == true)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Approval setup not defined or In-active",
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

                if (property.MemberProfileId != model.MemberProfileId)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Member Not Related With Property check Member Code",
                        Data = null
                    });
                }
                if (model.SendForApproval == true) { model.IsPrintEnabled = false; } else { model.IsPrintEnabled = true; }
                model.IsActive = true;
                model.CreatedOn = model.CreatedOn;
                model.CreatedBy = model.CreatedBy;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;

                if(model.PreviousRecordId != 0)
                { 
                  var previousRecord = _db.ClientFileVerification.FirstOrDefault(x => x.Id == model.PreviousRecordId);
                  previousRecord.IsActive = false;
                }
                _db.ClientFileVerification.Add(model);
                _db.SaveChanges();

                string message = "";
                ClientFileVerification clientFileVerification = (ClientFileVerification)_db.ClientFileVerification.Where(x => x.Id == model.Id)
                                                                                                                  .FirstOrDefault();
                if (clientFileVerification != null && model.SendForApproval == true)
                {
                    clientFileVerification.IsClientFileVerificationRequested = true;
                    _db.SaveChanges();

                    if (isApprovalActive == true)
                    {
                        bool result = _approvalBLL.AddNewApprovalSetup(clientFileVerification.Id, (int)ApprovalUIIds.ClientFileVerification);
                        message = "Client File Verification Request added succesfully and moved for approval";
                        if (result)
                        {
                            return Ok(new ApiResponse<object>
                            {
                                Code = ResponseCode.Success,
                                Message = message,
                                Data = null
                            });
                        }
                    }
                    else
                    {
                        clientFileVerification.IsPrintEnabled = true;
                        clientFileVerification.IsClientFileVerificationRequested = true;
                        clientFileVerification.IsClientFileVerificationApproved = true;
                        _db.SaveChanges();

                        message = "Client File Verification Request added succesfully";

                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = message,
                            Data = null
                        });
                    }
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = null
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetFilterRequestType")]
        public IActionResult GetFilterRequestType(string requestType)
        {
            try
            {
                var result = _db.ClientFileVerification.Where(x => !x.IsDeleted &&
                                                           x.IsActive == true &&
                                                           x.RequestType == requestType 
                                                         )
                                                        .Select(x => new
                                                        {
                                                            x.Id,
                                                            x.StockCreation.RegistrationNo,
                                                            x.StockCreation.PropertyNo,
                                                            x.RequestType,
                                                            x.ReceivedBy,
                                                            x.RecieverCNIC,
                                                            x.IsPrintEnabled,
                                                            x.IsFilePrint,
                                                            x.CreatedOn
                                                        })
                                                        .ToList()
                                                        .OrderByDescending(x=>x.Id)
                                                        .DistinctBy(x=>x.RegistrationNo);

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
        [Route("GetClientFileVerificationRequest")]
        public IActionResult GetClientFileVerificationRequest(int id)
        {
            try
            {
                var result = _db.ClientFileVerification.Where(x => !x.IsDeleted &&
                                                           x.Id == id
                                                         )
                                                        .Include(x=>x.StockCreation)
                                                        .Include(x=>x.MemberProfile)
                                                        .Include(x=>x.ClientFileVerificationAttachments)
                                                        .FirstOrDefault();
                if (result != null)
                {
                    result.CategoryName = _db.Categories.Where(x=>x.ID == (Convert.ToInt32(result.StockCreation.Category))).FirstOrDefault().Description;
                    result.BlockName = _db.Blocks.Where(x => x.ID == (Convert.ToInt32(result.StockCreation.Block))).FirstOrDefault().Description;
                }

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
        [Route("GetAuthorityLetterReceipt")]
        public IActionResult GetAuthorityLetterReceipt(int id)
        {
            try
            {
                var result = _db.ClientFileVerification.Where(x => !x.IsDeleted &&
                                                           x.Id == id
                                                         )
                                                        .Include(x => x.StockCreation)
                                                        .Include(x => x.MemberProfile)
                                                        .Select(x=> new
                                                        {
                                                            MemberName = x.ReceivedBy.IsNullOrEmpty() ? x.MemberProfile.MemberName : x.ReceivedBy,
                                                            Cnic = x.RecieverCNIC.IsNullOrEmpty() ? x.MemberProfile.Cnic : x.RecieverCNIC,
                                                            Relationship = x.RecieverMobile.IsNullOrEmpty() ? x.MemberProfile.Relationship : "S/O",
                                                            RelationshipWith = x.RecieverFatherName.IsNullOrEmpty() ? x.MemberProfile.RelationshipWith : x.RecieverFatherName,
                                                            x.StockCreation.PropertyNo,
                                                            x.StockCreation.RegistrationNo,
                                                            //x.StockCreation.BlockName
                                                            Block = _db.Blocks.Where(y => y.ID == Convert.ToInt32(x.StockCreation.Block)).FirstOrDefault().Description ?? "N/A",
                                                        }
                                                        )
                                                        .FirstOrDefault();
                if(result != null)
                {
                    var file = _db.ClientFileVerification.Find(id);
                    if(file != null)
                    {
                        file.IsFilePrint = true;
                        _db.SaveChanges();
                    }
                }
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }
    }
}
