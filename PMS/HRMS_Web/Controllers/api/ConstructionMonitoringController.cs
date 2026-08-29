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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConstructionMonitoringController : ControllerBase
    {
        private readonly DataBase_Context _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ApprovalBLL _approvalBLL;
        CommonBLL _commonBLL;
        public ConstructionMonitoringController(DataBase_Context db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _approvalBLL = new ApprovalBLL(_db);
            _commonBLL = new CommonBLL(_db);
        }

        [HttpGet]
        [Route("GetAllConstructionMoniteringFilterList")]
        public IActionResult GetAllConstructionMoniteringFilterList()
        {
            try
            {
                // condition might be change after approval module
                var result = _db.StockCreations.Where(x => !x.is_deleted
                                                   && x.MemberProfileId != null
                                                   && x.Is_ConstructionSecurityApproved == true
                                                   && x.Is_ConstructionMonitoringRequested != true
                                                     )
                                               .ToList();
                //if (result?.Count > 0)
                //{
                //    foreach (var block in result)
                //    {
                //        block.RealStateTypeName = "N/A"//_commonBLL.GetRealEstateName(Convert.ToInt32(block.RealStateType));
                //        //block.ProjectName = _commonBLL.GetProjectName(Convert.ToInt32(block.Project));
                //        //block.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(block.Phase));
                //        //block.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(block.Category));
                //        //block.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(block.Block));
                //        //block.NatureName = _commonBLL.GetNatureName(Convert.ToInt32(block.Nature));
                //        block.TypeName = "N/A" //_commonBLL.GetTypeName(Convert.ToInt32(block.Type));
                //    }
                //}

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
                var result = _db.ConstructionMonitoring.Where(x => !x.IsDeleted)
                                                       .Include(x => x.ConstructionMonitoringStageDetail)
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
        [Route("GetFines")]
        public IActionResult GetFines(int id)
        {
            try
            {
                var monitoring = _db.ConstructionMonitoring
                                    .FirstOrDefault(x => !x.IsDeleted && x.StockCreationId == id);

                var stock = _db.StockCreations.FirstOrDefault(x => x.ID == id);
                DateTime? possessionDate = stock?.PossessionEffectDate;

                List<object> result = new List<object>();

                if (monitoring == null || possessionDate == null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "No record found",
                        Data = result
                    });
                }

                // =====================================================
                // RULE 1: Construction not started within 12 months
                // Monthly Fine = 5000
                // =====================================================
                DateTime allowedStartDate = possessionDate.Value.AddYears(1);
                DateTime startLateTill = monitoring.ConstructionStartDate ?? DateTime.Now;

                if (startLateTill > allowedStartDate)
                {
                    int lateDays = (startLateTill.Date - allowedStartDate.Date).Days;
                    int dailyFine = 5000 / 30;
                    int fine = lateDays * dailyFine;

                    // Calculate months + days
                    int months = 0;
                    DateTime tempDate = allowedStartDate;
                    while (tempDate.AddMonths(1) <= startLateTill)
                    {
                        tempDate = tempDate.AddMonths(1);
                        months++;
                    }
                    int days = (startLateTill - tempDate).Days;

                    result.Add(new
                    {
                        Description = $"Construction started late by {months} months {days} days.",
                        Amount = fine
                    });
                }

                // =====================================================
                // RULE 2: Construction not completed within 18 months
                // Monthly Fine = 10000
                // =====================================================
                if (monitoring.ConstructionStartDate != null)
                {
                    DateTime allowedCompletionDate = monitoring.ConstructionStartDate.Value.AddMonths(18);
                    DateTime completionLateTill = monitoring.ConstructionEndDate ?? DateTime.Now;

                    if (completionLateTill > allowedCompletionDate)
                    {
                        int lateDays = (completionLateTill.Date - allowedCompletionDate.Date).Days;
                        int dailyFine = 10000 / 30;
                        int fine = lateDays * dailyFine;

                        // Calculate months + days
                        int months = 0;
                        DateTime tempDate = allowedCompletionDate;
                        while (tempDate.AddMonths(1) <= completionLateTill)
                        {
                            tempDate = tempDate.AddMonths(1);
                            months++;
                        }
                        int days = (completionLateTill - tempDate).Days;

                        result.Add(new
                        {
                            Description = $"Construction not completed within 18 months. Late by {months} months {days} days.",
                            Amount = fine
                        });
                    }
                }

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



        private int MonthDiff(DateTime from, DateTime to)
        {
            return ((to.Year - from.Year) * 12) + to.Month - from.Month;
        }


        [HttpGet]
        [Route("Get")]
        public IActionResult Get(int id)
        {
            try
            {
                var result = _db.ConstructionMonitoring.Where(x => !x.IsDeleted && x.Id == id)
                                                                      .Include(x => x.ConstructionMonitoringStageDetail)
                                                                      .Include(x => x.ViolationCM)
                                                                      .Include(x => x.SiteServicesCM)
                                                                      .Include(x => x.StackingCM)
                                                                      .Include(x => x.YardStickCM)
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

        [HttpGet]
        [Route("GetConstructionMonitoringList")]
        public IActionResult GetConstructionMonitoringList(int id)
        {
            try
            {
                var result = _db.ConstructionMonitoring.Where(x => !x.IsDeleted && x.Id == id)
                                                                      .Include(x => x.ConstructionMonitoringStageDetail)
                                                                      .Include(x=>x.ViolationCM)
                                                                      .Include(x=>x.SiteServicesCM)
                                                                      .Include(x=>x.StackingCM)
                                                                      .Include(x=>x.YardStickCM)
                                                                      .Include(x => x.StockCreation)
                                                                      .ThenInclude(x => x.MemberProfile)
                                                                      .FirstOrDefault();
                if (result != null)
                {
                    result.StockCreation.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(result.StockCreation.RealStateType));
                    result.StockCreation.ProjectName = _commonBLL.GetProjectName(Convert.ToInt32(result.StockCreation.Project));
                    result.StockCreation.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(result.StockCreation.Phase));
                    result.StockCreation.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(result.StockCreation.Category));
                    result.StockCreation.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(result.StockCreation.Block));
                    result.StockCreation.NatureName = _commonBLL.GetNatureName(Convert.ToInt32(result.StockCreation.Nature));
                    result.StockCreation.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(result.StockCreation.Type));

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

        [HttpPost]
        [Route("AddNewConstructionMonitoring")]
        public async Task<IActionResult> AddNewConstructionMonitoringAsync(ConstructionMonitoring model)
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
                model.LastModifiedUserName = model.LastModifiedUserName;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;
                model.IsActive = true;
                model.IsDeleted = false;

                if (model.ConstructionMonitoringStageDetail?.Count > 0)
                {
                    foreach (var item in model.ConstructionMonitoringStageDetail)
                    {
                        var path = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";
                        item.Picture = string.IsNullOrEmpty(item.Picture) ? "" : $"{path}{await item.Picture.SaveBase64FileAsync()}";
                        item.CreatedOn = DateTime.Now;
                        item.LastModified = DateTime.Now;
                        item.ModifiedBy = model.ModifiedBy;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                if (model.ViolationCM?.Count > 0)
                {
                    foreach (var item in model.ViolationCM)
                    {
                        item.CreatedOn = DateTime.Now;
                        item.LastModified = DateTime.Now;
                        item.ModifiedBy = model.ModifiedBy;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                if (model.SiteServicesCM?.Count > 0)
                {
                    foreach (var item in model.SiteServicesCM)
                    {
                        item.CreatedOn = DateTime.Now;
                        item.LastModified = DateTime.Now;
                        item.ModifiedBy = model.ModifiedBy;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                if (model.YardStickCM?.Count > 0)
                {
                    foreach (var item in model.YardStickCM)
                    {
                        item.CreatedOn = DateTime.Now;
                        item.LastModified = DateTime.Now;
                        item.ModifiedBy = model.ModifiedBy;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                if (model.StackingCM?.Count > 0)
                {
                    foreach (var item in model.StackingCM)
                    {
                        item.CreatedOn = DateTime.Now;
                        item.LastModified = DateTime.Now;
                        item.ModifiedBy = model.ModifiedBy;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                _db.ConstructionMonitoring.Add(model);
                _db.SaveChanges();

                string message = string.Empty;

                StockCreation stockCreation = (StockCreation)_db.StockCreations.Where(x => x.ID == model.StockCreationId)
                                                                               .FirstOrDefault();

                if (stockCreation != null)
                {
                    stockCreation.Is_ConstructionMonitoringRequested = true;
                    stockCreation.ConstracutionStatus = model.ConstructionStatus;

                    if (model.ConstructionStatus == "Constructed" && stockCreation.GrancePeriodForBillGenration >= DateTime.Now.Date)
                    {
                        stockCreation.GrancePeriodForBillGenration = DateTime.Now.AddDays(-1);
                        stockCreation.DemarcationExpireOn = DateTime.Now.AddDays(-1);
                        stockCreation.LastModifiedUser = model.LastModifiedUserName;
                        stockCreation.Updated_By = Convert.ToInt32(model.ModifiedBy);
                        stockCreation.Updated_at = DateTime.Now;
                    }
                    _db.SaveChanges();

                    if (isApprovalActive == true)
                    {
                        bool result = _approvalBLL.AddNewApprovalSetup(stockCreation.ID, (int)ApprovalUIIds.ConstructionMonitoring);
                        message = "Construction Miontring added succesfully and moved for approval";
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
                        stockCreation.Is_ConstructionMonitoringApproved = true;
                        message = "Construction Monitoring added succesfully ";
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
        [Route("UpdateConstructionMonitoring")]
        public async Task<IActionResult> UpdateConstructionMonitoringAsync(ConstructionMonitoring model)
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

                var data = _db.ConstructionMonitoring.Find(model.Id);

                if (data != null)
                {
                    data.ConstructionStatus = model.ConstructionStatus;
                    data.ConstructedStatus = model.ConstructedStatus;
                    data.ConstructionStartDate = model.ConstructionStartDate;
                    data.ConstructionEndDate = model.ConstructionEndDate;
                    data.EWSConnectionStatus = model.EWSConnectionStatus;
                    data.Id = model.Id;
                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;
                    data.LastModified = DateTime.Now;

                    _db.Entry(data).State = EntityState.Modified;

                    var stock = _db.StockCreations.Find(model.StockCreationId);
                    if (stock != null)
                    {
                        stock.ConstracutionStatus = model.ConstructionStatus;
                        _db.Entry(data).State = EntityState.Modified;

                    }

                    var attachmentresult = _db.ConstructionMonitoringStageDetail.Where(x => x.ConstructionMonitoringId == model.Id).ToList();

                    foreach (var attachment in attachmentresult)
                    {
                        var existingFilePath = attachment.Picture;

                        bool fileExistsInNewModel = model.ConstructionMonitoringStageDetail.Any(x => x.Picture == existingFilePath);

                        if (!fileExistsInNewModel && !string.IsNullOrEmpty(existingFilePath))
                        {
                            existingFilePath.DeleteFile();
                        }

                        _db.ConstructionMonitoringStageDetail.Remove(attachment);
                    }

                    var path = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";

                    if (model.ConstructionMonitoringStageDetail?.Count > 0)
                    {
                           foreach (var item in model.ConstructionMonitoringStageDetail)
                        {
                            item.Picture = string.IsNullOrEmpty(item.Picture) ? "" : $"{path}{await item.Picture.SaveBase64FileAsync()}";
                            item.ConstructionMonitoringId = data.Id;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                        }

                        _db.ConstructionMonitoringStageDetail.AddRange(model.ConstructionMonitoringStageDetail);
                    }

                    var violation = _db.ViolationCM.ToList();

                    _db.RemoveRange(violation);

                    if (model.ViolationCM?.Count > 0)
                    {
                        foreach (var item in model.ViolationCM)
                        {
                            item.ConstructionMonitoringId = data.Id;
                            item.CreatedOn = DateTime.Now;
                            item.LastModified = DateTime.Now;
                            item.ModifiedBy = model.ModifiedBy;
                            item.CreatedBy = model.CreatedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.ViolationCM.AddRange(model.ViolationCM);
                    }

                    var siteServicesCM = _db.SiteServicesCM.ToList();

                    _db.RemoveRange(violation);

                    if (model.SiteServicesCM?.Count > 0)
                    {
                        foreach (var item in model.SiteServicesCM)
                        {
                            item.ConstructionMonitoringId = data.Id;
                            item.CreatedOn = DateTime.Now;
                            item.LastModified = DateTime.Now;
                            item.ModifiedBy = model.ModifiedBy;
                            item.CreatedBy = model.CreatedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }
                    }

                    var yardStickCM = _db.YardStickCM.ToList();

                    _db.RemoveRange(yardStickCM);

                    if (model.YardStickCM?.Count > 0)
                    {
                        foreach (var item in model.YardStickCM)
                        {
                            item.ConstructionMonitoringId = data.Id;
                            item.CreatedOn = DateTime.Now;
                            item.LastModified = DateTime.Now;
                            item.ModifiedBy = model.ModifiedBy;
                            item.CreatedBy = model.CreatedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.YardStickCM.AddRange(model.YardStickCM);
                    }

                    var stackingCM = _db.StackingCM.ToList();

                    _db.RemoveRange(stackingCM);

                    if (model.StackingCM?.Count > 0)
                    {
                        foreach (var item in model.StackingCM)
                        {
                            item.ConstructionMonitoringId = data.Id;
                            item.CreatedOn = DateTime.Now;
                            item.LastModified = DateTime.Now;
                            item.ModifiedBy = model.ModifiedBy;
                            item.CreatedBy = model.CreatedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.StackingCM.AddRange(model.StackingCM);
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
        [Route("DeleteConstructionMonitoring")]
        public IActionResult DeleteConstructionMonitoring(int id)
        {
            try
            {
                var model = _db.ConstructionMonitoring.Find(id);

                if (model != null)
                {
                    model.LastModified = DateTime.Now;
                    model.IsActive = false;
                    model.IsDeleted = true;

                    _db.SaveChanges();

                    var result = _db.ConstructionMonitoringStageDetail.Where(x => x.ConstructionMonitoringId == model.Id).ToList();

                    foreach (var item in result)
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
