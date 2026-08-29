using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using B_Utility.Common.Enums;
using HRMS_Web.Extensions;
using HRMS_Web.Models.DTOs;
using HRMS_Web.Services.AlertService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web.Http.Results;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MapApprovalController : ControllerBase
    {
        private readonly DataBase_Context _db;
        private readonly IAlertService _alertService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ApprovalBLL _approvalBLL;
        CommonBLL _commonBLL;
        public MapApprovalController(DataBase_Context db, IAlertService alertService, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _alertService = alertService;
            _httpContextAccessor = httpContextAccessor;
            _approvalBLL = new ApprovalBLL(_db);
            _commonBLL = new CommonBLL(_db);
        }

        [HttpGet]
        [Route("GetAllMapApprovalFilterListFindMode")]
        public IActionResult GetAllMapApprovalFilterListFindMode()
        {
            try
            {
                var result = _db.StockCreations.Where(x => x.is_deleted == false
                                                                    && x.MemberProfileId != null
                                                                    && x.Is_ClearnceApproved == true
                                                                    && x.Is_MapApprovalRequested == true
                                                                    && x.Is_MapApprovalApproved == true
                                                                    && x.coveredArea != null
                                                                   )
                                               .ToList();
                if (result?.Count > 0)
                {
                    foreach (var block in result)
                    {
                        block.Status = GetRedesignStatus(block.ID);
                        block.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(block.RealStateType));
                        block.ProjectName = _commonBLL.GetProjectName(Convert.ToInt32(block.Project));
                        block.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(block.Phase));
                        block.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(block.Category));
                        block.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(block.Block));
                        block.NatureName = _commonBLL.GetNatureName(Convert.ToInt32(block.Nature));
                        block.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(block.Type));
                    }
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
        [Route("GetAllMapApprovalFilterList")]
        public IActionResult GetAllMapApprovalFilterList()
        {
            try
            {
                var result = _db.StockCreations.Where(x => x.is_deleted == false
                                                                    && x.MemberProfileId != null
                                                                    && x.Is_ClearnceApproved == true
                                                                    && x.Is_MapApprovalRequested != true
                                                                   )
                                               .ToList();
                if (result?.Count > 0)
                {
                    foreach (var block in result)
                    {
                        block.Status = GetMapStatus(block.ID);
                        block.LastModifiedUser = LastModifiedUser(block.ID);
                        block.RequestId = GetNDCConstructionStatus(block.ID);
                        block.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(block.RealStateType));
                        block.ProjectName = _commonBLL.GetProjectName(Convert.ToInt32(block.Project));
                        block.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(block.Phase));
                        block.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(block.Category));
                        block.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(block.Block));
                        block.NatureName = _commonBLL.GetNatureName(Convert.ToInt32(block.Nature));
                        block.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(block.Type));
                    }
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

        private string GetMapStatus(int id)
        {
            var result = _db.MapApprovalHistery.Where(x => x.StockCreationID == id).FirstOrDefault();
            if (result == null)
            {
                return "Yet to start";
            }
            return "InProgress";
        }

        private string LastModifiedUser(int id)
        {
            return _db.MapApprovalHistery
                      .Where(x => x.StockCreationID == id)
                      .OrderBy(x => x.Id)
                      .LastOrDefault()?.LastModifiedUserName ?? "N/A";
        }


        private string GetRedesignStatus(int id)
        {
            var result = _db.NewDemarcationRequest.Where(x => x.StockCreationId == id && x.IsCancelled == true).FirstOrDefault();
            if (result == null)
            {
                return "Completed";
            }
            return "Revised-InProgress";
        }

        private int? GetNDCConstructionStatus(int id)
        {
            var result = _db.ClientFileVerification
                .Where(x => x.StockCreationId == id && x.RequestType == "NDC For Construction")
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            if (result != null)
            {
                return result.Id;
            }
            return null;
        }


        [HttpGet]
        [Route("Get")]
        public IActionResult Get(int id)
        {
            try
            {
                var result = _db.StockCreations.Where(x => !x.is_deleted
                                                                    && x.ID == id)
                                                                 .Include(x => x.MapApprovalHistory)
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
        [Route("AddMappApproval")]
        public async Task<IActionResult> AddMappApprovalAsync(List<MapApprovalHistery> model)
        {
            try
            {

                bool isApprovalActive = true;

                var approvalStatus = _db.ApprovalUI.Find((int)ApprovalUIIds.MapApproval);
                if (approvalStatus != null)
                {
                    if (approvalStatus.Checked != true)
                    {
                        isApprovalActive = false;
                    }
                }

                var approvalSetup = _db.ApprovalSetup.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.MapApproval).ToList();
                if (approvalSetup.Count <= 0 && isApprovalActive == true)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Approval setup not defined or In-active",
                        Data = null
                    });
                }

                if (model.Any(x => x.RedesignMappApproved == "No" && x.FindMode == "Yes"))
                {
                    await SaveMapApprovalAsync(model);

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Revised added Sucessfully.",
                        Data = null
                    });
                }

                if (model.Any(x => x.RedesignMappApproved == "Yes" && x.FindMode == "Yes"))
                {
                    var demarcationRequests = _db.NewDemarcationRequest.Where(x => x.StockCreationId == model.FirstOrDefault().StockCreationID && x.IsCancelled == true).ToList();
                    foreach (var item in demarcationRequests)
                    {
                        item.IsCancelled = null;
                    }

                    var lastUpdateValue = model.Where(x => x.RedesignMappApproved == "Yes").FirstOrDefault();
                    StockCreation? stock = UpdateStock(lastUpdateValue.CoveredArea, (int)lastUpdateValue.StockCreationID);
                    await SaveMapApprovalAsync(model);

                    string narration = $"The Registraion No. {stock.RegistrationNo} redesign request submitted from Map Approval please proceed it";
                    _alertService.PushAlert(8, narration);

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Revised Completed Sucessfully.",
                        Data = null
                    });
                }

                if (model.Count() > 0)
                {
                    int currentStage = (int)model.LastOrDefault().Stage;
                    int yesRecords = (int)_db.MapApprovalHistery.Where(x => x.StockCreationID == model[0].StockCreationID && x.ClientStatus == "Yes").Count();

                    if (currentStage > yesRecords + 1)
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.NotFound,
                            Message = "Next Stage is locked",
                            Data = null
                        });
                    }
                }

                bool executeCode = false;

                if (model.Count() > 0)
                {
                    int clientYes = model.Where(x => x.ClientStatus == "Yes").Count();
                    int records = (int)_db.MapApprovalHistery.Where(x => x.StockCreationID == model[0].StockCreationID).Count();
                    var lastApproval = _db.MapApprovalHistery.Where(x => x.StockCreationID == model[0].StockCreationID).OrderBy(x => x.Id).LastOrDefault();
                    int clientStage = lastApproval?.ClientStage ?? 0;
                    if (records == 0 && model.Count > 1)
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.NotFound,
                            Message = "In first attempt you can add only one row",
                            Data = null
                        });
                    }
                    else if (records == 0 && clientYes == clientStage)
                    {
                        foreach (var item in model)
                        {
                            item.ClientStage = 1;
                        }
                        executeCode = true;
                    }
                    else if (records == 0 && clientYes > clientStage)
                    {
                        foreach (var item in model)
                        {
                            item.ClientStage = 2;
                        }
                        executeCode = true;
                    }

                    else if (clientYes == clientStage)
                    {
                        foreach (var item in model)
                        {
                            item.ClientStage = clientStage + 1;
                        }
                        executeCode = true;
                    }

                    else
                    {
                        executeCode = true;
                    }
                }

                if (model.Count > 0 && executeCode)
                {
                    await SaveMapApprovalAsync(model);

                    int ID = (int)model.FirstOrDefault().StockCreationID;
                    decimal coveredArea1 = '0';
                    var stock1 = _db.StockCreations.Where(x => x.ID == ID).FirstOrDefault();

                    var area1 = model.Where(model => model.Is_MappApproved == false).FirstOrDefault();
                    if (area1 != null)
                    {
                        coveredArea1 = Convert.ToDecimal(area1.CoveredArea);
                    }
                    if (stock1 != null)
                    {
                        stock1.coveredArea = coveredArea1;

                        _db.SaveChanges();
                    }


                    string message = String.Empty;

                    if (model.Any(x => x.Is_MappApproved == true && x.Is_Checked == true))
                    {
                        decimal coveredArea = '0';
                        int Id = (int)model.FirstOrDefault().StockCreationID;
                        var area = model.Where(model => model.Is_MappApproved == true).FirstOrDefault();
                        if (area != null)
                        {
                            coveredArea = Convert.ToDecimal(area.CoveredArea);
                        }

                        StockCreation? stock = UpdateStock(coveredArea.ToString(), Id);

                        if (isApprovalActive == true)
                        {
                            bool result = _approvalBLL.AddNewApprovalSetup(Id, (int)ApprovalUIIds.MapApproval);
                            message = "Map approved succesfully and moved for approval";
                            if (result)
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
                            stock.Is_MapApprovalApproved = true;
                            _db.SaveChanges();

                            message = "Map approved succesfully";

                            return Ok(new ApiResponse<object>
                            {
                                Code = ResponseCode.Success,
                                Message = message,
                            });
                        }
                    }
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Map Approval History Saved.",
                    Data = null
                });
            }

            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        private StockCreation? UpdateStock(string coveredArea, int Id)
        {
            var stock = _db.StockCreations.Where(x => x.ID == Id).FirstOrDefault();
            if (stock != null)
            {
                stock.Is_MapApprovalRequested = true;
                stock.coveredArea = Convert.ToDecimal(coveredArea);

                _db.SaveChanges();
            }

            return stock;
        }

        private async Task SaveMapApprovalAsync(List<MapApprovalHistery> model)
        {
            var result1 = _db.MapApprovalHistery.Where(x => x.StockCreationID == model[0].StockCreationID).ToList();

            foreach (var attachment in result1)
            {
                var existingFilePath1 = attachment.Attachments;

                bool fileExistsInNewModel = model.Any(x => x.Attachments == existingFilePath1);

                if (!fileExistsInNewModel && !string.IsNullOrEmpty(existingFilePath1))
                {
                    existingFilePath1.DeleteFile();
                }

            }

            _db.MapApprovalHistery.RemoveRange(result1);


            foreach (var item in model)
            {
                MapApprovalHistery approvalHistory = new MapApprovalHistery();
                {
                    var path = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";

                    if (string.IsNullOrEmpty(item.Attachments))
                    {
                        approvalHistory.Attachments = "";
                    }
                    else if (item.Attachments.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    {
                        approvalHistory.Attachments = item.Attachments;
                    }
                    else
                    {
                        approvalHistory.Attachments = $"{path}{await item.Attachments.SaveBase64FileAsync()}";
                    }

                    approvalHistory.StockCreationID = item.StockCreationID;
                    approvalHistory.Description = item.Description;
                    approvalHistory.MapType = item.MapType;
                    approvalHistory.DateofSubmission = DateTime.Now;
                    approvalHistory.ClientStatus = item.ClientStatus;
                    approvalHistory.DateofFeedback = item.DateofFeedback;
                    approvalHistory.Is_Checked = item.Is_Checked;
                    approvalHistory.Stage = item.Stage;
                    approvalHistory.ArchRemarks = item.ArchRemarks;
                    approvalHistory.ClientRemarks = item.ClientRemarks;
                    approvalHistory.ClientStage = item.ClientStage;
                    approvalHistory.CreatedOn = item.CreatedOn;
                    approvalHistory.LastModified = item.LastModified;
                    approvalHistory.CreatedBy = item.CreatedBy;
                    approvalHistory.ModifiedBy = item.ModifiedBy;
                    approvalHistory.LastModifiedUserName = item.LastModifiedUserName;
                    approvalHistory.IsActive = true;
                    approvalHistory.IsDeleted = false;

                    _db.MapApprovalHistery.Add(approvalHistory);

                }
            }

            _db.SaveChanges();
        }
    }
}
