using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common.Enums;
using B_Utility.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using B_DB_Context;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SitePlanController : ControllerBase
    {
        private readonly DataBase_Context _db;
        ApprovalBLL _approvalBLL;
        public SitePlanController(DataBase_Context db)
        {
            _db = db;
            _approvalBLL = new ApprovalBLL(_db);
        }

        [HttpGet]
        [Route("GetAllSitePlanRequest")]
        public IActionResult GetAllSitePlanRequest()
        {
            try
            {
                var result = _db.SitePlans.Where(x => !x.IsDeleted &&
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
        [Route("GetSitePlan")]
        public IActionResult GetSitePlan(int id)
        {
            try
            {
                var result = _db.SitePlans.Where(x => !x.IsDeleted &&
                                                           x.Id == id
                                                         )
                                                        .Include(x => x.StockCreation)
                                                        .Include(x => x.MemberProfile)
                                                        .Include(x => x.SitePlanAttachments)
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
        [Route("/api/SitePlan/SaveSitePlan")]
        public IActionResult SaveSitePlan(SitePlan model)
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

                bool planExist = _db.SitePlans.Any(x => x.StockCreationId == model.StockCreationId);
                if (planExist)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Site Plan Already Exist Please update it",
                        Data = null
                    });
                }

                model.IsRequestClosed = false;
                model.IsActive = true;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = model.CreatedBy;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;

                if (model.SitePlanAttachments?.Count > 0)
                {
                    foreach (var item in model.SitePlanAttachments)
                    {
                        item.ModifiedBy = model.ModifiedBy;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModified = DateTime.Now;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                _db.SitePlans.Add(model);
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

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Documents Added",
                    Data = null
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPut]
        [Route("/api/SitePlan/UpdateSitePlan")]
        public IActionResult UpdateSitePlan(SitePlan model)
        {
            try
            {
                var data = _db.SitePlans.Find(model.Id);

                if (data != null)
                {
                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;
                    data.LastModified = DateTime.Now;

                    _db.Entry(data).State = EntityState.Modified;
                   

                    if (model.SitePlanAttachments?.Count > 0)
                    {
                        var result = _db.SitePlanAttachments.Where(x => x.SitePlanId == model.Id).ToList();

                        _db.SitePlanAttachments.RemoveRange(result);
                        
                    }

                    if (model.SitePlanAttachments?.Count > 0)
                    {
                        foreach (var item in model.SitePlanAttachments)
                        {
                            item.SitePlanId = data.Id;
                            item.ModifiedBy = item.ModifiedBy;
                            item.LastModified = DateTime.Now;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }
                        _db.SitePlanAttachments.AddRange(model.SitePlanAttachments); 
                    }

                    _db.SaveChanges();
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Documents Updated",
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
