using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CaseProfileController : ControllerBase
    {
        private readonly DataBase_Context _db;

        CommonBLL _commonBLL;
        ApprovalBLL _approvalBLL;
        public CaseProfileController(DataBase_Context db)
        {
            _db = db;
            _approvalBLL = new ApprovalBLL(_db);
            _commonBLL = new CommonBLL(_db);
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _db.CaseProfile.Where(x => !x.IsDeleted)
                                                       .Include(x => x.CaseProfileParties.Where(x => !x.IsDeleted))
                                                       .Include(x => x.CaseProfileCaseHearings.Where(x => !x.IsDeleted))
                                                       .Include(x => x.CaseProfileAppeals.Where(x => !x.IsDeleted))
                                                       .Include(x => x.CaseProfileNotices.Where(x => !x.IsDeleted))
                                                       .Include(x => x.CaseProfileAttachments.Where(x => !x.IsDeleted))
                                                       .Include(x => x.CaseType)
                                                       .Include(x => x.CaseCategory)
                                                       .Include(x => x.Forum)
                                                       .Include(x => x.LawyerData)
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
                var result = _db.CaseProfile.Where(x => !x.IsDeleted && x.Id == id)
                                                       .Include(x => x.CaseProfileParties.Where(x => !x.IsDeleted))
                                                       .Include(x => x.CaseProfileCaseHearings.Where(x => !x.IsDeleted))
                                                       .Include(x => x.CaseProfileAppeals.Where(x => !x.IsDeleted))
                                                       .Include(x => x.CaseProfileNotices.Where(x => !x.IsDeleted))
                                                       .Include(x => x.CaseProfileAttachments.Where(x => !x.IsDeleted))
                                                       .Include(x => x.CaseType)
                                                       .Include(x => x.CaseCategory)
                                                       .Include(x => x.Forum)
                                                       .Include(x => x.LawyerData)
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
        [Route("AddNewCaseProfile")]
        public IActionResult AddNewCaseProfile(CaseProfile model)
        {
            try
            {

                //bool isApprovalActive = true;

                //var approvalStatus = _db.ApprovalUI.Find((int)ApprovalUIIds.Transfer);
                //if (approvalStatus != null)
                //{
                //    if (approvalStatus.Checked != true)
                //    {
                //        isApprovalActive = false;
                //    }
                //}

                //var approvalSetup = _db.ApprovalSetup.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.Transfer).ToList();
                //if (approvalSetup.Count <= 0 && isApprovalActive == true)
                //{
                //    return Ok(new ApiResponse<object>
                //    {
                //        Code = ResponseCode.Success,
                //        Message = "Approval setup not defined or In-active",
                //        Data = null
                //    });
                //}

                model.IsActive = true;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = model.CreatedBy;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;

                if (model.CaseProfileParties?.Count > 0)
                {
                    foreach (var item in model.CaseProfileParties)
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
                if (model.CaseProfileCaseHearings?.Count > 0)
                {
                    foreach (var item in model.CaseProfileCaseHearings)
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
                if (model.CaseProfileNotices?.Count > 0)
                {
                    foreach (var item in model.CaseProfileNotices)
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
                if (model.CaseProfileAppeals?.Count > 0)
                {
                    foreach (var item in model.CaseProfileAppeals)
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
                if (model.CaseProfileAttachments?.Count > 0)
                {
                    foreach (var item in model.CaseProfileAttachments)
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

                _db.CaseProfile.Add(model);
                _db.SaveChanges();

                //string message = string.Empty;

                //TransferHistory transferHistory = (TransferHistory)_db.TransferHistory.Where(x => x.Id == model.Id)
                //                                      .FirstOrDefault();
                //if (transferHistory != null)
                //{
                //    transferHistory.IsTransferRequested = true;
                //    _db.SaveChanges();

                //    if (isApprovalActive == true)
                //    {
                //        bool result = _approvalBLL.AddNewApprovalSetup(model.Id, (int)ApprovalUIIds.Transfer);
                //        message = "Transfer added succesfully and moved for approval";
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
                //        transferHistory.IsTransferApproved = true;
                //        _db.SaveChanges();

                //        message = "Transfer added succesfully";

                //        return Ok(new ApiResponse<object>
                //        {
                //            Code = ResponseCode.Success,
                //            Message = message,
                //            Data = null
                //        });
                //    }
                //}

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

        [HttpPut]
        [Route("UpdateCaseProfile")]
        public IActionResult UpdateCaseProfile(CaseProfile model)
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

                var data = _db.CaseProfile.Find(model.Id);

                if (data != null)
                {
                    data.CaseId = model.CaseId;
                    data.CaseTitle = model.CaseTitle;
                    data.CaseTypeId = model.CaseTypeId;
                    data.CaseCategoryId = model.CaseCategoryId;
                    data.CaseFor = model.CaseFor;
                    data.LandArea = model.LandArea;
                    data.FIRReferenceNo = model.FIRReferenceNo;
                    data.AdvanceDeposit = model.AdvanceDeposit;
                    data.SettlementMark = model.SettlementMark;
                    data.Status = model.Status;
                    data.ReferenceOfSettlement = model.ReferenceOfSettlement;
                    data.ForumId = model.ForumId;
                    data.Reason = model.Reason;
                    data.LawyerDataId = model.LawyerDataId;
                    data.TermsAndConditionsOfLawyer = model.TermsAndConditionsOfLawyer;
                    data.LawyerFee = model.LawyerFee;
                    data.CourtFee = model.CourtFee;
                    data.Outcome = model.Outcome;
                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;
                    data.LastModified = DateTime.Now;

                    _db.Entry(data).State = EntityState.Modified;
                    _db.SaveChanges();


                    if (model.CaseProfileParties?.Count > 0)
                    {
                        var result = _db.CaseProfileParties.Where(x => x.CaseProfileId == model.Id).ToList();

                        _db.CaseProfileParties.RemoveRange(result);
                        _db.SaveChanges();
                    }

                    if (model.CaseProfileParties?.Count > 0)
                    {
                        foreach (var item in model.CaseProfileParties)
                        {
                            item.CaseProfileId = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.CaseProfileParties.AddRange(model.CaseProfileParties);
                        _db.SaveChanges();
                    }

                    if (model.CaseProfileCaseHearings?.Count > 0)
                    {
                        var result = _db.CaseProfileCaseHearings.Where(x => x.CaseProfileId == model.Id).ToList();

                        _db.CaseProfileCaseHearings.RemoveRange(result);
                        _db.SaveChanges();
                    }

                    if (model.CaseProfileCaseHearings?.Count > 0)
                    {
                        foreach (var item in model.CaseProfileCaseHearings)
                        {
                            item.CaseProfileId = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.CaseProfileCaseHearings.AddRange(model.CaseProfileCaseHearings);
                        _db.SaveChanges();
                    }

                    if (model.CaseProfileAppeals?.Count > 0)
                    {
                        var result = _db.CaseProfileAppeals.Where(x => x.CaseProfileId == model.Id).ToList();

                        _db.CaseProfileAppeals.RemoveRange(result);
                        _db.SaveChanges();
                    }

                    if (model.CaseProfileAppeals?.Count > 0)
                    {
                        foreach (var item in model.CaseProfileAppeals)
                        {
                            item.CaseProfileId = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.CaseProfileAppeals.AddRange(model.CaseProfileAppeals);
                        _db.SaveChanges();
                    }

                    if (model.CaseProfileNotices?.Count > 0)
                    {
                        var result = _db.CaseProfileNotices.Where(x => x.CaseProfileId == model.Id).ToList();

                        _db.CaseProfileNotices.RemoveRange(result);
                        _db.SaveChanges();
                    }

                    if (model.CaseProfileNotices?.Count > 0)
                    {
                        foreach (var item in model.CaseProfileNotices)
                        {
                            item.CaseProfileId = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.CaseProfileNotices.AddRange(model.CaseProfileNotices);
                        _db.SaveChanges();
                    }

                    if (model.CaseProfileAttachments?.Count > 0)
                    {
                        var result = _db.CaseProfileAttachments.Where(x => x.CaseProfileId == model.Id).ToList();

                        _db.CaseProfileAttachments.RemoveRange(result);
                        _db.SaveChanges();
                    }

                    if (model.CaseProfileAttachments?.Count > 0)
                    {
                        foreach (var item in model.CaseProfileAttachments)
                        {
                            item.CaseProfileId = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.CaseProfileAttachments.AddRange(model.CaseProfileAttachments);
                        _db.SaveChanges();
                    }
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
        [Route("DeleteCaseProfile")]
        public IActionResult DeleteCaseProfile(int id)
        {
            try
            {
                var model = _db.CaseProfile.Find(id);

                if (model != null)
                {
                    model.ModifiedBy = model.ModifiedBy;
                    model.LastModified = DateTime.Now;
                    model.IsActive = false;
                    model.IsDeleted = true;

                    _db.SaveChanges();

                    var caseProfileParties = _db.CaseProfileParties.Where(x => x.CaseProfileId == model.Id).ToList();

                    if (caseProfileParties?.Count > 0)
                    {
                        foreach (var item in caseProfileParties)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;
                            _db.SaveChanges();
                        }
                    }
                    var caseProfileCaseHearings = _db.CaseProfileCaseHearings.Where(x => x.CaseProfileId == model.Id).ToList();

                    if (caseProfileCaseHearings?.Count > 0)
                    {
                        foreach (var item in caseProfileCaseHearings)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;
                            _db.SaveChanges();
                        }
                    }
                    var caseProfileAppeals = _db.CaseProfileAppeals.Where(x => x.CaseProfileId == model.Id).ToList();

                    if (caseProfileAppeals?.Count > 0)
                    {
                        foreach (var item in caseProfileAppeals)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;
                            _db.SaveChanges();
                        }
                    }
                    var caseProfileNotices = _db.CaseProfileNotices.Where(x => x.CaseProfileId == model.Id).ToList();

                    if (caseProfileNotices?.Count > 0)
                    {
                        foreach (var item in caseProfileNotices)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;
                            _db.SaveChanges();
                        }
                    }
                    var caseProfileAttachments = _db.CaseProfileAttachments.Where(x => x.CaseProfileId == model.Id).ToList();

                    if (caseProfileAttachments?.Count > 0)
                    {
                        foreach (var item in caseProfileAttachments)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;
                            _db.SaveChanges();
                        }
                    }
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
    }
}
