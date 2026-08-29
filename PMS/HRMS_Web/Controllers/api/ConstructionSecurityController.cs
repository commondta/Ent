using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using B_Utility.Common.Enums;
using HRMS_Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize]
    public class ConstructionSecurityController : ControllerBase
    {
        private readonly DataBase_Context _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ApprovalBLL _approvalBLL;
        CommonBLL _commonBLL;
        public ConstructionSecurityController(DataBase_Context db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _approvalBLL = new ApprovalBLL(_db);
            _commonBLL = new CommonBLL(_db);
        }

        //[HttpGet]
        //[Route("/api/ConstructionSecurity/GetAllConstructionSecurityFilterList")]
        //public IActionResult GetAllConstructionSecurityFilterList()
        //{
        //    try
        //    {
        //        var result = _db.StockCreations.Where(x => !x.is_deleted
        //                                           && x.MemberProfileId != null
        //                                           && x.Is_DemarcationApproved == true
        //                                           && x.Is_ConstructionSecurityRequested != true
        //                                             )
        //                                       .ToList();
        //        if (result?.Count > 0)
        //        {
        //            foreach (var block in result)
        //            {
        //                block.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(block.RealStateType));
        //                block.ProjectName = _commonBLL.GetProjectName(Convert.ToInt32(block.Project));
        //                block.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(block.Phase));
        //                block.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(block.Category));
        //                block.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(block.Block));
        //                block.NatureName = _commonBLL.GetNatureName(Convert.ToInt32(block.Nature));
        //                block.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(block.Type));
        //            }
        //        }

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
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _db.ConstructionSecurity.Where(x => !x.IsDeleted)
                                                       .Include(x => x.ConstructionSecurityLabour)
                                                       .Include(x => x.ConstructionSecurityAttachment)
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
        [Route("Get")]
        public IActionResult Get(int id)
        {
            try
            {
                var result = _db.ConstructionSecurity.Where(x => !x.IsDeleted && x.Id == id)
                                                                      .Include(x => x.ConstructionSecurityLabour)
                                                                      .Include(x => x.ConstructionSecurityAttachment)
                                                                      .Include(x => x.StockCreation)
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
        [Route("/api/ConstructionSecurity/AddNewConstructionSecurity")]
        public async Task<IActionResult> AddNewConstructionSecurityAsync(ConstructionSecurity model)
        {
            try
            {
                bool isApprovalActive = true;

                var approvalStatus = _db.ApprovalUI.Find((int)ApprovalUIIds.ConstructionSecurity);
                if (approvalStatus != null)
                {
                    if (approvalStatus.Checked != true)
                    {
                        isApprovalActive = false;
                    }
                }

                var approvalSetup = _db.ApprovalSetup.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.ConstructionSecurity).ToList();
                if (approvalSetup.Count <= 0 && isApprovalActive == true)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Approval setup not defined or In-active",
                        Data = null
                    });
                }

                model.CreatedOn = DateTime.Now;
                model.CreatedBy = model.CreatedBy;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;
                model.IsActive = true;
                model.IsDeleted = false;
                var path = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";

                if (model.ConstructionSecurityLabour?.Count > 0)
                {
                    foreach (var item in model.ConstructionSecurityLabour)
                    {
                        item.CNICAttachment = string.IsNullOrEmpty(item.CNICAttachment) ? "" : $"{path}{await item.CNICAttachment.SaveBase64FileAsync()}";

                        item.CreatedOn = DateTime.Now;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModified = DateTime.Now;
                        item.ModifiedBy = model.ModifiedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                if (model.ConstructionSecurityAttachment?.Count > 0)
                {
                    foreach (var item in model.ConstructionSecurityAttachment)
                    {
                        item.Attachment = string.IsNullOrEmpty(item.Attachment) ? "" : $"{path}{await item.Attachment.SaveBase64FileAsync()}";
                        item.CreatedOn = DateTime.Now;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModified = DateTime.Now;
                        item.ModifiedBy = model.ModifiedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                _db.ConstructionSecurity.Add(model);
                _db.SaveChanges();

                string message = string.Empty;

                StockCreation stockCreation = (StockCreation)_db.StockCreations.Where(x => x.ID == model.StockCreationId)
                                                                               .FirstOrDefault();
                if (stockCreation != null)
                {
                    stockCreation.Is_ConstructionSecurityRequested = true;
                    _db.SaveChanges();

                    if (isApprovalActive == true)
                    {
                        bool result = _approvalBLL.AddNewApprovalSetup(stockCreation.ID, (int)ApprovalUIIds.ConstructionSecurity);
                        message = "Construction Security added succesfully and moved for approval";
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
                        stockCreation.Is_ConstructionSecurityApproved = true;
                        _db.SaveChanges();
                        message = "Construction Security added succesfully";

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
                    Code = ResponseCode.NotFound,
                    Message = "Not Found",
                    Data = null
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPut]
        [Route("UpdateConstructionSecurity")]
        public IActionResult UpdateConstructionSecurity(ConstructionSecurity model)
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

                var data = _db.ConstructionSecurity.Find(model.Id);

                if (data != null)
                {
                    data.ContractorName = model.ContractorName;
                    data.CNIC = model.CNIC;
                    data.MobileNumber = model.MobileNumber;
                    data.Address = model.Address;
                    data.SurveyorName = model.SurveyorName;
                    data.SurveyorNumber = model.SurveyorNumber;
                    data.SurveyorCNIC = model.SurveyorCNIC;
                    data.SurveyorAddress = model.SurveyorAddress;
                    data.Remarks = model.Remarks;
                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;
                    data.LastModified = DateTime.Now;
                    data.IsDeleted = false;
                    data.IsActive = true;

                    _db.Entry(data).State = EntityState.Modified;


                    // Handle ConstructionSecurityLabour attachments
                    var attachmentresult = _db.ConstructionSecurityLabour.Where(x => x.ConstructionSecurityId == model.Id).ToList();

                    foreach (var attachment in attachmentresult)
                    {
                        var existingFilePath = attachment.CNICAttachment;

                        bool fileExistsInNewModel = model.ConstructionSecurityLabour.Any(x => x.CNICAttachment == existingFilePath);

                        if (!fileExistsInNewModel && !string.IsNullOrEmpty(existingFilePath))
                        {
                            existingFilePath.DeleteFile();
                        }

                        _db.ConstructionSecurityLabour.Remove(attachment);
                    }

                    if (model.ConstructionSecurityLabour?.Count > 0)
                    {
                        foreach (var item in model.ConstructionSecurityLabour)
                        {
                            item.ConstructionSecurityId = data.Id;
                            item.CreatedOn = DateTime.Now;
                            item.LastModified = DateTime.Now;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.ConstructionSecurityLabour.AddRange(model.ConstructionSecurityLabour);
                    }

                    // Handle ConstructionSecurityAttachment
                    var attachmentresult1 = _db.ConstructionSecurityAttachment.Where(x => x.ConstructionSecurityId == model.Id).ToList();

                    foreach (var attachment in attachmentresult1)
                    {
                        var existingFilePath = attachment.Attachment;

                        bool fileExistsInNewModel = model.ConstructionSecurityAttachment.Any(x => x.Attachment == existingFilePath);

                        if (!fileExistsInNewModel && !string.IsNullOrEmpty(existingFilePath))
                        {
                            existingFilePath.DeleteFile();
                        }

                        _db.ConstructionSecurityAttachment.Remove(attachment);
                    }

                    if (model.ConstructionSecurityAttachment?.Count > 0)
                    {
                        foreach (var item in model.ConstructionSecurityAttachment)
                        {
                            item.ConstructionSecurityId = data.Id;
                            item.CreatedOn = DateTime.Now;
                            item.LastModified = DateTime.Now;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.ConstructionSecurityAttachment.AddRange(model.ConstructionSecurityAttachment);
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
        [Route("DeleteConstructionSecurity")]
        public IActionResult DeleteConstructionSecurity(int id)
        {
            try
            {
                var model = _db.ConstructionSecurity.Find(id);

                if (model != null)
                {
                    model.LastModified = DateTime.Now;
                    model.IsActive = false;
                    model.IsDeleted = true;

                    _db.SaveChanges();

                    var constructionSecurityLabour = _db.ConstructionSecurityLabour.Where(x => x.ConstructionSecurityId == model.Id).ToList();

                    foreach (var item in constructionSecurityLabour)
                    {
                        item.LastModified = DateTime.Now;
                        item.IsActive = false;
                        item.IsDeleted = true;
                        _db.SaveChanges();
                    }

                    var constructionSecurityAttachment = _db.ConstructionSecurityAttachment.Where(x => x.ConstructionSecurityId == model.Id).ToList();

                    foreach (var item in constructionSecurityAttachment)
                    {
                        item.LastModified = DateTime.Now;
                        item.IsActive = false;
                        item.IsDeleted = true;
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
