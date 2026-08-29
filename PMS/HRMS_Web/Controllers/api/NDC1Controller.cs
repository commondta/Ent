using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using B_Utility.Common.Enums;
using HRMS_Web.Services.AlertService;
using HRMS_Web.Services.SMSService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NDC1Controller : ControllerBase
    {
        private readonly DataBase_Context _db;
        private readonly IAlertService alertService;
        private readonly ISMSService _sMSService;
        ApprovalBLL _approvalBLL;
        CommonBLL _commonBLL;
        public NDC1Controller(DataBase_Context db,IAlertService alertService,ISMSService sMSService)
        {
            _db = db;
            this.alertService = alertService;
            _sMSService = sMSService;
            _approvalBLL = new ApprovalBLL(_db);
            _commonBLL = new CommonBLL(_db);
        }

        // call this when user focus out from cnic field
        //[HttpGet]
        //[Route("/api/NDCRequestForMember/GetNDCRequestForMemberByCnic")]
        //public IActionResult GetNDCRequestForMemberByCnic(string cnic)
        //{
        //    try
        //    {
        //        var result = _db.MemberProfile.Where(x => !x.IsDeleted
        //                                           && x.Cnic == cnic
        //                                           && x.CnicExpiryDate <= DateTime.Now
        //                                             )
        //                                       .SingleOrDefault();
        //        if (result == null)
        //        {
        //            return Ok(new ApiResponse<object>
        //            {
        //                Code = ResponseCode.NotFound,
        //                Message = "Enter Valid Cnic",
        //                Data = null
        //            });

        //        }
        //        else
        //        {
        //            return Ok(new ApiResponse<object>
        //            {
        //                Code = ResponseCode.Success,
        //                Message = "Success",
        //                Data = result.Id
        //            });
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
        //    }
        //}

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _db.NDC1.Where(x => !x.IsDeleted)
                                                       .Include(x => x.NDC1PowerOfAttorey.Where(x => !x.IsDeleted))
                                                       .Include(x => x.NDC1Attachments.Where(x => !x.IsDeleted))
                                                       .Include(x => x.NDC1CheckList.Where(x => !x.IsDeleted))
                                                       .Include(x => x.StockCreation)
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
        [Route("GetPrint")]
        public IActionResult GetPrint(int id)
        {
            try
            {
                var result = _db.NDC1.Where(x => !x.IsDeleted &&
                                                           x.Id == id
                                                         )
                                                        .Include(x => x.StockCreation)
                                                        .Include(x => x.MemberProfile)
                                                        .Select(x => new
                                                        {
                                                            Id = x.Id,
                                                            MemberName = x.MemberProfile.MemberName,
                                                            MembershipNo = x.MemberProfile.MEMBERSHIPNO,
                                                            TransferType = x.TransferType,
                                                            DealerName = x.DealerName,
                                                            ReceivingDate = DateTime.Now.ToShortDateString(),
                                                            NDCDate = x.CreatedOn,
                                                            RegistrationNo = x.StockCreation.RegistrationNo ?? "",
                                                            PropertyNo = x.StockCreation.PropertyNo ?? "",
                                                            Phase = _db.Phases.Where(y => y.ID == Convert.ToInt32(x.StockCreation.Phase)).FirstOrDefault().Description ?? "N/A",
                                                            Sector = _db.Sectors.Where(y => y.ID == Convert.ToInt32(x.StockCreation.PrefixProperty)).FirstOrDefault().Description ?? "N/A",
                                                         }
                                                        )
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
        [Route("Get")]
        public IActionResult Get(int id)
        {
            try
            {
                var result = _db.NDC1.Where(x => !x.IsDeleted && x.Id == id)
                                                      .Include(x => x.NDC1PowerOfAttorey.Where(x => !x.IsDeleted))
                                                      .Include(x => x.NDC1Attachments.Where(x => !x.IsDeleted))
                                                      .Include(x => x.NDC1CheckList.Where(x => !x.IsDeleted))
                                                      .Include(x => x.StockCreation)
                                                      .Include(x => x.MemberProfile)
                                                      .AsSplitQuery()
                                                      .AsNoTracking()
                                                      .Select(x => new
                                                      {
                                                          x.NDC1CheckList,
                                                          x.Id,
                                                          StockId = x.StockCreation.ID,
                                                          x.StockCreation.RegistrationNo,
                                                          x.StockCreation.PropertyNo,
                                                          x.StockCreation.ActualSizeUnit,
                                                          x.MemberProfile.MemberName,
                                                          x.MemberProfileId,
                                                          x.MemberProfile.Cnic,
                                                          x.MemberProfile.CnicExpiryDate,
                                                          x.DealerCode,
                                                          x.DealerName,
                                                          x.EstateName,
                                                          x.ValidityDate,
                                                          x.SlotDate,
                                                          x.Slot,
                                                          x.NDCRequestType,
                                                          x.TransferType,
                                                          x.Outstation,
                                                          x.Day,
                                                          x.ApplyStation,
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

        [HttpPost]
        [Route("AddNewNDC1")]
        public async Task<IActionResult> AddNewNDC1Async(NDC1 model)
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
                //var approvalSetup = _db.ApprovalSetup.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.NDC1).ToList();
                //if (approvalSetup.Count <= 0)
                //{
                //    return Ok(new ApiResponse<object>
                //    {
                //        Code = ResponseCode.NotFound,
                //        Message = "Not Found",
                //        Data = "Approval setup not defined or In-active"
                //    });
                //}
                model.IsActive = true;
                model.CreatedOn = model.CreatedOn.Date.Add(DateTime.Now.TimeOfDay);
                model.CreatedBy = model.CreatedBy;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;
                model.IsGovtTaxRequested = true;

                if (model.NDC1PowerOfAttorey?.Count() > 0)
                {
                    foreach (var item in model.NDC1PowerOfAttorey)
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

                if (model.NDC1Attachments?.Count() > 0)
                {
                    foreach (var item in model.NDC1Attachments)
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

                if (model.NDC1CheckList?.Count() > 0)
                {
                    foreach (var item in model.NDC1CheckList)
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

                _db.NDC1.Add(model);
               // _db.SaveChanges(); 

                var request = _db.NDCRequestForMember.Where(x => x.StockCreationId == model.StockCreationId).ToList();
                if (request.Count() > 0)
                {
                    foreach (var item in request)
                    {
                        item.IsActive = false;
                       // _db.SaveChanges();
                    }
                }

                bool IsExist = _db.TransferHistery.Any(x => x.StockCreationId == model.StockCreationId);

                if (!IsExist)
                {

                    TransferHistery TransferHistery = new TransferHistery();
                    {
                        TransferHistery.MemberProfileId = model.MemberProfileId;
                        TransferHistery.StockCreationId = model.StockCreationId;
                        TransferHistery.Remarks = "Hosted Ownery";
                        TransferHistery.IsHold = false;
                        TransferHistery.CreatedBy = model.CreatedBy;
                        TransferHistery.CreatedOn = DateTime.Now;
                        TransferHistery.ModifiedBy = model.ModifiedBy;
                        TransferHistery.LastModifiedUserName = model.LastModifiedUserName;
                        TransferHistery.LastModified = DateTime.Now;
                        TransferHistery.IsActive = true;
                        TransferHistery.IsDeleted = false;
                        TransferHistery.IsRequestClosed = true;

                        _db.TransferHistery.Add(TransferHistery);
                        _db.SaveChanges();

                        var bookingJointMember = _db.BookingJointMember.Where(x => x.Booking.StockCreationId == model.StockCreationId)
                                                                       .ToList();

                        if (bookingJointMember?.Count() > 0)
                        {
                            foreach (var item in bookingJointMember)
                            {
                                var memberdata = _db.MemberProfile.Find(item.MemberProfileId);

                                if (memberdata != null)
                                {
                                    TransferHisteryJointMember TransferHisteryJointMember = new TransferHisteryJointMember();
                                    {
                                        TransferHisteryJointMember.TransferHisteryId = TransferHistery.Id;
                                        TransferHisteryJointMember.Name = memberdata.MemberName;
                                        TransferHisteryJointMember.Relationship = memberdata.Relationship;
                                        TransferHisteryJointMember.CNIC = memberdata.Cnic;
                                        TransferHisteryJointMember.Mobile = memberdata.Mobile;
                                        TransferHisteryJointMember.Address = memberdata.CurrentAddress;
                                        TransferHisteryJointMember.CreatedBy = model.CreatedBy;
                                        TransferHisteryJointMember.CreatedOn = DateTime.Now;
                                        TransferHisteryJointMember.ModifiedBy = model.ModifiedBy;
                                        TransferHisteryJointMember.LastModifiedUserName = model.LastModifiedUserName;
                                        TransferHisteryJointMember.LastModified = DateTime.Now;
                                        TransferHisteryJointMember.IsActive = true;
                                        TransferHisteryJointMember.IsDeleted = false;

                                        _db.TransferHisteryJointMember.Add(TransferHisteryJointMember);
                                        // _db.SaveChanges();
                                    }
                                }
                            }
                        }

                        var bookingNominee = _db.BookingNominee.Where(x => x.Booking.StockCreationId == model.StockCreationId)
                                                               .ToList();
                        if (bookingNominee?.Count() > 0)
                        {
                            foreach (var item in bookingNominee)
                            {
                                TransferHisteryNominee TransferHisteryNominee = new TransferHisteryNominee();
                                {
                                    TransferHisteryNominee.TransferHisteryId = TransferHistery.Id;
                                    TransferHisteryNominee.Name = item.Name;
                                    TransferHisteryNominee.Relationship = item.Relationship;
                                    TransferHisteryNominee.CNIC = item.CNIC;
                                    TransferHisteryNominee.Mobile = item.Mobile;
                                    TransferHisteryNominee.Address = item.Address;
                                    TransferHisteryNominee.CreatedBy = model.CreatedBy;
                                    TransferHisteryNominee.CreatedOn = DateTime.Now;
                                    TransferHisteryNominee.ModifiedBy = model.ModifiedBy;
                                    TransferHisteryNominee.LastModifiedUserName = model.LastModifiedUserName;
                                    TransferHisteryNominee.LastModified = DateTime.Now;
                                    TransferHisteryNominee.IsActive = true;
                                    TransferHisteryNominee.IsDeleted = false;

                                    _db.TransferHisteryNominee.Add(TransferHisteryNominee);
                                    // _db.SaveChanges();
                                }
                            }
                        }
                    }
                }

                var stock = _db.StockCreations.Where(x => x.ID == model.StockCreationId).Include(x => x.MemberProfile).Select(x => new { x.RegistrationNo, x.PropertyNo, x.MemberProfile.Mobile, x.MemberProfile.MemberName }).FirstOrDefault();
                string narration = $"NDC 1 of MemberName: {stock.MemberName} having ReferenceNo: {stock.RegistrationNo} submitted by {model.LastModifiedUserName}";
                alertService.PushAlert(2, narration);

                _db.SaveChanges();

            
                string message = $@"
                     Subject: NDC Status Notification
                     
                     Dear customer,  
                     
                     Your NDC has been cleared on {model.CreatedOn.ToString("dd-MM-yyyy")}.
                     Please submit your Transfer set for Transfer
                     NDC Valid Till: {model.ValidityDate}
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

                NDC1 nDC1 = (NDC1)_db.NDC1.Where(x => x.Id == model.Id).FirstOrDefault();
                if (nDC1 != null)
                {
                    nDC1.IsNDC1Requested = true;
                    _db.SaveChanges();

                    bool result = _approvalBLL.AddNewApprovalSetup(nDC1.Id, (int)ApprovalUIIds.NDC1);

                    if (result)
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "Success",
                            Data = "NDC1 added succesfully and moved for approval"
                        });
                    }
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "NDC1 Added Successfully",
                    Data = null
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPut]
        [Route("UpdateNDC1")]
        public IActionResult UpdateNDC1(NDC1 model)
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

                var data = _db.NDC1.Find(model.Id);

                if (data != null)
                {
                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;
                    data.LastModified = DateTime.Now;

                    _db.Entry(data).State = EntityState.Modified;
                  

                    if (model.NDC1PowerOfAttorey?.Count > 0)
                    {
                        var result = _db.NDC1PowerOfAttorey.Where(x => x.NDC1Id == model.Id).ToList();

                        _db.NDC1PowerOfAttorey.RemoveRange(result);
                       
                    }

                    if (model.NDC1PowerOfAttorey?.Count > 0)
                    {
                        foreach (var item in model.NDC1PowerOfAttorey)
                        {
                            item.NDC1Id = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.NDC1PowerOfAttorey.AddRange(model.NDC1PowerOfAttorey);
                       
                    }

                    if (model.NDC1Attachments?.Count > 0)
                    {
                        var result = _db.NDC1Attachments.Where(x => x.NDC1Id == model.Id).ToList();

                        _db.NDC1Attachments.RemoveRange(result);
                       
                    }

                    if (model.NDC1Attachments?.Count > 0)
                    {
                        foreach (var item in model.NDC1Attachments)
                        {
                            item.NDC1Id = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.NDC1Attachments.AddRange(model.NDC1Attachments);
                        
                    }
                    if (model.NDC1CheckList?.Count() > 0)
                    {
                        var result = _db.NDC1CheckLists.Where(x => x.NDC1Id == model.Id).ToList();

                        _db.NDC1CheckLists.RemoveRange(result);
                        
                    }

                    if (model.NDC1CheckList?.Count() > 0)
                    {
                        foreach (var item in model.NDC1CheckList)
                        {
                            item.NDC1Id = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.NDC1CheckLists.AddRange(model.NDC1CheckList);
                        
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
                    Message = "Success",
                    Data = data
                });

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpDelete]
        [Route("DeleteNDC1")]
        public IActionResult DeleteNDC1(int id)
        {
            try
            {
                var model = _db.NDC1.Find(id);

                if (model != null)
                {
                    model.ModifiedBy = model.ModifiedBy;
                    model.LastModified = DateTime.Now;
                    model.IsActive = false;
                    model.IsDeleted = true;             

                    var nDC1PowerOfAttoreys = _db.NDC1PowerOfAttorey.Where(x => x.NDC1Id == model.Id).ToList();

                    if (nDC1PowerOfAttoreys?.Count > 0)
                    {
                        foreach (var item in nDC1PowerOfAttoreys)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;
                        }
                    }

                    var nDC1Attachments = _db.NDC1Attachments.Where(x => x.NDC1Id == model.Id).ToList();

                    if (nDC1Attachments?.Count > 0)
                    {
                        foreach (var item in nDC1Attachments)
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
                    Message = "Success",
                    Data = model
                });

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        // Discard
        [HttpPost]
        [Route("Discard")]
        public IActionResult Discard(int id)
        {
            var request = _db.NDCRequestForMember.Where(x => x.Id == id).FirstOrDefault();
            if (request != null)
            {
                request.IsCanceled = true;
                request.ValidityDate = DateTime.Now.AddDays(-1);
                _db.SaveChanges();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Record canceled you can add new request",
                    Data = null
                });
            }
            return Ok(new ApiResponse<object>
            {
                Code = ResponseCode.NotFound,
                Message = "record not found",
                Data = null
            });
        }

        // update 

        [HttpPut]
        [Route("ChangeProcessingStatus")]
        public IActionResult ChangeProcessingStatus(int id)
        {
            var request = _db.NDCRequestForMember.Where(x => x.Id == id).FirstOrDefault();
            if (request != null)
            {
                request.Processing = !request.Processing;

                _db.SaveChanges();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Record Updated",
                    Data = null
                });
            }
            return Ok(new ApiResponse<object>
            {
                Code = ResponseCode.NotFound,
                Message = "record not found",
                Data = null
            });
        }
    }
}
