using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using B_Utility.Common.Enums;
using HRMS_Web.Extensions;
using HRMS_Web.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using static B_Utility.BLL.ApprovalBLL;
using RequestApprovalStatusUpdateDTO = B_Utility.BLL.ApprovalBLL.RequestApprovalStatusUpdateDTO;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize]
    public class StockCreationController : ControllerBase
    {
        private readonly DataBase_Context _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ApprovalBLL _approvalBLL;
        CommonBLL _commonBLL;
        public StockCreationController(DataBase_Context db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _approvalBLL = new ApprovalBLL(_db);
            _commonBLL = new CommonBLL(_db);
        }

        //casecading dropdowns

        [HttpGet]
        public IActionResult GetProjectDropdownByRealEstateId(int realEstateId)
        {
            try
            {
                var result = _db.Projects.Where(x => x.RealStateTypeId == realEstateId && x.is_deleted != true).ToList();

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
        public IActionResult GetAllCategories()
        {
            try
            {

                var result = _db.Categories.Where(x => x.is_deleted != true).ToList();

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
        public IActionResult GetCategoryDropdownByRealEstateId(int realEstateId)
        {
            try
            {

                var result = _db.Categories.Where(x => x.RealStateTypeId == realEstateId && x.is_deleted != true).ToList();

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
        public IActionResult GetNatureDropdownByRealEstateId(int realEstateId)
        {
            Response_Result response_Results = new Response_Result();
            try
            {

                var result = _db.Natures.Where(x => x.RealStateTypeId == realEstateId && x.is_deleted != true).ToList();

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
        public IActionResult GetPropertyinRange(int from, int to)
        {

            try
            {
                var propertiesList = _db.StockCreations.Where(x => x.ID >= from && x.ID <= to && x.PossessionStatus != true).ToList();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = propertiesList
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        public IActionResult GetLastRegNo(string prefix)
        {

            try
            {
                int regnumber = 0;
                var existingStock = _db.StockCreations.ToList();

                var regnumberlist = existingStock.Where(x => x.PrefixRegistration == prefix).Select(x => x.numForRegistration).ToList();

                var newlist = regnumberlist.OrderByDescending(s => s.Value);
                if (regnumberlist.Count > 0)
                {
                    regnumber = (int)newlist.FirstOrDefault();
                }
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = regnumber
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        public IActionResult GetLastPropNo(string prefix)
        {

            try
            {
                int propnumber = 0;
                var existingStock = _db.StockCreations.ToList();

                var propnumberlist = existingStock.Where(x => x.PrefixProperty == prefix).Select(x => x.numForProperty).ToList();
                if (propnumberlist.Count > 0)
                {
                    propnumber = (int)propnumberlist.LastOrDefault();
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = propnumber
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        public IActionResult GetLastPropNoByBlock(string blockId)
        {
            try
            {
                var existingStock = _db.StockCreations.Where(x => x.Block == blockId).ToList();
                var propnumber = existingStock.Any() ? existingStock.Max(x => x.numForProperty) : 0;

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = propnumber
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        public async Task<IActionResult> GenratePossessionWithAttachments([FromBody] GeneratePossessionCreateModel model)
        {
            try
            {
                bool isApprovalActive = true;

                var approvalStatus = _db.ApprovalUI.Find((int)ApprovalUIIds.PossessionAnnocement);
                if (approvalStatus != null)
                {
                    if (approvalStatus.Checked != true)
                    {
                        isApprovalActive = false;
                    }
                }

                var approvalSetup = _db.ApprovalSetup.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.PossessionAnnocement).ToList();
                if (approvalSetup.Count <= 0 && isApprovalActive == true)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Approval setup not defined or In-active",
                        Data = null
                    });
                }

                int count = 0;

                var propertiesList = _db.StockCreations.Where(x => x.ID >= model.fromId && x.ID <= model.toId).ToList();


                if (propertiesList?.Count > 0)
                {
                    string message = string.Empty;
                    bool request = false;

                    var finder = propertiesList.FirstOrDefault();

                    foreach (var prop in propertiesList)
                    {
                        if (prop.Phase != finder?.Phase! || prop.Block != finder?.Block || prop.RealStateType != finder.RealStateType || prop.Project != finder.Project)
                        {
                            return Ok(new ApiResponse<object>
                            {
                                Code = ResponseCode.NotFound,
                                Message = "The Selected Properties do not have same features"
                            });
                        }
                    }

                    if (isApprovalActive == true)
                    {
                        foreach (var item in propertiesList)
                        {
                            item.PossessionEffectDate = model.possessionEffectDate;
                            item.GrancePeriodForBillGenration = model.possessionEffectDate.AddMonths(GetPossessionGracePeriod());
                            item.PossessionStatus = false;
                            item.Is_PossessionApproved = false;
                            item.Is_PossessionRequested = true;

                            _db.SaveChanges();

                            request = _approvalBLL.AddNewApprovalSetup(item.ID, Convert.ToInt32(ApprovalUIIds.PossessionAnnocement));
                            count++;
                        }
                        message = "Possession announced successfully and move for approval";
                    }
                    else
                    {
                        foreach (var item in propertiesList)
                        {
                            item.PossessionEffectDate = model.possessionEffectDate;
                            item.GrancePeriodForBillGenration = model.possessionEffectDate.AddMonths(GetPossessionGracePeriod());
                            item.PossessionStatus = true;
                            item.Is_PossessionRequested = true;
                            item.Is_PossessionApproved = true;
                            _db.SaveChanges();

                            count++;
                        }
                        message = "Possession announced successfully";
                    }

                    var path = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";

                    foreach (var stock in propertiesList)
                    {
                        foreach (var item in model.possessionFormAttachments)
                        {
                            item.Piture = string.IsNullOrEmpty(item.Piture) ? "" : $"{path}{await item.Piture.SaveBase64FileAsync()}";
                        }

                        foreach (var item in model.possessionFormAttachments)
                        {
                            item.Piture = string.IsNullOrEmpty(item.Piture)
                                ? ""
                                : $"{path}{await item.Piture.SaveBase64FileAsync()}";
                        }

                        if (model.possessionFormAttachments != null && model.possessionFormAttachments.Any())
                        {
                            var attachments = model.possessionFormAttachments.Select(x => new PossessionAttachment
                            {
                                Remarks = x.Remarks,
                                Piture = x.Piture,
                                StockCreationId = stock.ID   
                            }).ToList();

                            await _db.PossessionAttachments.AddRangeAsync(attachments);

                        }
                    }

                        await _db.SaveChangesAsync();

                    if (count == propertiesList.Count())
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = message
                        });
                    }
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "No possession available in range",
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        public IActionResult GenratePossession(int fromId, int toId, DateTime possessionEffectDate)
        {
            try
            {
                bool isApprovalActive = true;

                var approvalStatus = _db.ApprovalUI.Find((int)ApprovalUIIds.PossessionAnnocement);
                if (approvalStatus != null)
                {
                    if (approvalStatus.Checked != true)
                    {
                        isApprovalActive = false;
                    }
                }

                var approvalSetup = _db.ApprovalSetup.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.PossessionAnnocement).ToList();
                if (approvalSetup.Count <= 0 && isApprovalActive == true)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Approval setup not defined or In-active",
                        Data = null
                    });
                }

                int count = 0;

                var propertiesList = _db.StockCreations.Where(x => x.ID >= fromId && x.ID <= toId).ToList();


                if (propertiesList?.Count > 0)
                {
                    string message = string.Empty;
                    bool request = false;

                    var finder = propertiesList.FirstOrDefault();

                    foreach (var prop in propertiesList)
                    {
                        if (prop.Phase != finder?.Phase! || prop.Block != finder?.Block || prop.RealStateType != finder.RealStateType || prop.Project != finder.Project)
                        {
                            return Ok(new ApiResponse<object>
                            {
                                Code = ResponseCode.NotFound,
                                Message = "The Selected Properties do not have same features"
                            });
                        }
                    }

                    if (isApprovalActive == true)
                    {
                        foreach (var item in propertiesList)
                        {
                            item.PossessionEffectDate = possessionEffectDate;
                            item.GrancePeriodForBillGenration = possessionEffectDate.AddMonths(GetPossessionGracePeriod());
                            item.PossessionStatus = false;
                            item.Is_PossessionRequested = true;

                            _db.SaveChanges();

                            request = _approvalBLL.AddNewApprovalSetup(item.ID, Convert.ToInt32(ApprovalUIIds.PossessionAnnocement));
                            count++;
                        }
                        message = "Possession announced successfully and move for approval";
                    }
                    else
                    {
                        foreach (var item in propertiesList)
                        {
                            item.PossessionEffectDate = possessionEffectDate;
                            item.GrancePeriodForBillGenration = possessionEffectDate.AddMonths(GetPossessionGracePeriod());
                            item.PossessionStatus = true;
                            item.Is_PossessionRequested = true;
                            item.Is_PossessionApproved = true;
                            _db.SaveChanges();

                            count++;
                        }
                        message = "Possession announced successfully";
                    }

                    if (count == propertiesList.Count())
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = message
                        });
                    }
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "No possession available in range",
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        public int GetPossessionGracePeriod()
        {
            var gracePeriod = _db.GracePeriodSetup.SingleOrDefault()?.PossessionGracePriod;
            return gracePeriod ?? 0; // return 0 if gracePeriod is null
        }

        [HttpGet]
        public IActionResult UpdatePropertyLocation(string propertyNumber, string location, string street)
        {
            try
            {
                var property = _db.StockCreations.Where(x => x.PropertyNo == propertyNumber).SingleOrDefault();

                if (property != null)
                {
                    property.Location = location;
                    property.Street = street;

                    _db.SaveChanges();

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                    });
                }

                else
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Not Found",
                    });
                }
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        public IActionResult AddStockCreation(StockCreation block)
        {
            try
            {
                bool isApprovalActive = true;

                var approvalStatus = _db.ApprovalUI.Find((int)ApprovalUIIds.StockCreation);
                if(approvalStatus != null)
                 {
                    if(approvalStatus.Checked != true)
                    {
                        isApprovalActive = false;
                    }
                 }

                var approvalSetup = _db.ApprovalSetup.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.StockCreation).ToList();
                if (approvalSetup.Count <= 0 && isApprovalActive == true)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Approval setup not defined or In-active",
                        Data = null
                    });
                }

                StockCreationSetup stockCreationSetup = _db.StockCreationSetup.SingleOrDefault();

                if (block.ID == 0)
                {
                    int regnumber = 0;
                    int propnumber = 0;
                    int qty = Convert.ToInt32(block.Quantity);
                    var existingStock = _db.StockCreations.ToList();

                    string regprefix = block.PrefixRegistration;
                    string regpostfix = block.postfixForRegistration;
                    var regnumberlist = existingStock.Where(x => x.PrefixRegistration == regprefix && x.postfixForRegistration == regpostfix).Select(x => x.numForRegistration).ToList();
                    if (regnumberlist.Count > 0)
                    {
                        regnumber = (int)regnumberlist.LastOrDefault();
                    }
                    string propprefix = block.PrefixProperty != null && block.PrefixProperty != "" ? block.PrefixProperty.ToString() : "";
                    string proppostfix = block.postfixForProperty != null && block.postfixForProperty != "" ? block.postfixForProperty.ToString() : "";

                    var propnumberlist = existingStock.Where(x => x.PrefixProperty == propprefix && x.postfixForProperty == proppostfix).Select(x => x.numForProperty).ToList();
                    if (propnumberlist.Count > 0)
                    {
                        propnumber = (int)propnumberlist.LastOrDefault();
                    }
                    //difference

                    int diffregnumber = (int)(block.numForRegistration - regnumber);
                    int diffpropnumber = (int)(block.numForProperty - propnumber);

                    int regnewNumber = regnumber + diffregnumber;
                    int propnewNumber = propnumber + diffpropnumber;

                    List<StockCreation> stockCreationList = new List<StockCreation>();

                    for (int i = 0; i < qty; i++)
                    {
                        StockCreation stock = new StockCreation();
                        int numforreg = regnewNumber + i;
                        int numforprop = propnewNumber + i;
                        if (block.postfixForRegistration != "-1" && block.numForRegistration > 0)
                        { 
                            if (block.postfixForRegistration != "-1")
                            {
                                stock.RegistrationNo = block.PrefixRegistration + String.Format("{0:0000}", numforreg) + block.postfixForRegistration;
                            }
                            else
                            {
                                stock.RegistrationNo = block.PrefixRegistration + String.Format("{0:0000}", numforreg);
                            }
                        }

                        if (block.PrefixProperty != "-1" && block.numForProperty > 0)
                        {
                            if (block.postfixForProperty != "-1")
                            {
                                stock.PropertyNo =  String.Format("{0:0000}", numforprop) + block.postfixForProperty;
                            }
                            else
                            {
                                stock.PropertyNo =  String.Format("{0:0000}", numforprop);
                            }
                        }
                        stock.Created_at = DateTime.Now;
                        stock.Updated_at = DateTime.Now;
                        stock.is_active = true;
                        stock.is_deleted = false;
                        stock.Status = block.Status;
                        stock.User = block.User;
                        stock.ActualSize = block.ActualSize;
                        stock.ActualSizeUnit = block.ActualSizeUnit;
                        stock.ActualSize = block.ActualSize;
                        stock.Phase = block.Phase;
                        stock.Nature = block.Nature;
                        stock.Project = block.Project;
                        stock.Block = block.Block;
                        stock.PrefixRegistration = block.PrefixRegistration;
                        stock.numForRegistration = numforreg;
                        stock.postfixForRegistration = block.postfixForRegistration;
                        stock.PrefixProperty = block.PrefixProperty;
                        stock.numForProperty = numforprop;
                        stock.postfixForProperty = block.postfixForProperty;
                        stock.Category = block.Category;
                        stock.Quantity = block.Quantity;
                        stock.Finishing = block.Finishing;
                        stock.RealStateType = block.RealStateType;
                        stock.InventoryStatus = block.InventoryStatus;
                        stock.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(block.RealStateType));
                        stock.Floor = block.Floor;
                        stock.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(block.Type));
                        stock.Type = block.Type;
                        stock.Almt = block.Almt;
                        stock.PossessionStatus = false;
                        stock.UnderLitigation = false;
                        stock.Is_StockCreationRequested = true;
                        stock.ConstracutionStatus = stockCreationSetup.ConstrucationStatus ?? "Non-Constructed";
                        stock.IsBillGenerationEnabled = false;
                        stock.IsSaleTaxEnabled = false;
                        stock.IsWithHoldingTaxEnabled = false;
                        stock.PossessionStatus = stockCreationSetup.PossessionStatus ?? false;
                        stock.GrancePeriodForBillGenration = DateTime.Now.AddMonths(-2);
                        stock.GeneratorUnitType = "GU1";
                        stock.Feature = block.Feature;
                        stock.CaseCode = block.CaseCode;
                        stock.AffidavitCode = block.AffidavitCode;
                        stock.SaleDeedNo = block.SaleDeedNo;
                        stock.SaleDeedDate = block.SaleDeedDate;
                        stock.Mouza = block.Mouza;
                        stock.MembershipFee = block.MembershipFee;
                        stock.MiscCharges = block.MiscCharges;
                        stock.AllocationNo = block.AllocationNo;
                        stockCreationList.Add(stock);
                    }
                    // check confilict with existing
                    var confilictStock = _db.StockCreations.ToList();

                    if (existingStock?.Count > 0 && stockCreationList?.Count > 0)
                    {
                        List<StockCreation> confilictStockCreationList = new List<StockCreation>();

                        foreach (var stock in stockCreationList)
                        {
                            if (existingStock.Where(item => item.RegistrationNo == stock.RegistrationNo && stock.RegistrationNo != null).Any())
                            {
                                var conflictStockItem = existingStock.Where(item => item.RegistrationNo == stock.RegistrationNo).FirstOrDefault();
                                confilictStockCreationList.Add(conflictStockItem);
                            }
                            else
                            {
                                var confictstockItem = existingStock.Where(item => item.Phase == stock.Phase
                                                                  && stock.RealStateType == item.RealStateType
                                                                  && stock.Project == item.Project
                                                                  && stock.Block == item.Block
                                                                  && stock.Category == item.Category
                                                                  && stock.Type == item.Type
                                                                  && ((stock.RegistrationNo == item.RegistrationNo && stock.PropertyNo == item.PropertyNo)
                                                                  || (stock.PropertyNo == item.PropertyNo && stock.PropertyNo != null)
                                                                  || (stock.RegistrationNo == item.RegistrationNo && stock.PropertyNo == null && stock.RegistrationNo != null)
                                                                  || (stock.PropertyNo == item.PropertyNo && stock.RegistrationNo == null && stock.PropertyNo != null
                                                                  ))
                                                                  ).FirstOrDefault();
                                if (confictstockItem != null)
                                {
                                    confilictStockCreationList.Add(confictstockItem);
                                }
                            }

                        }

                        if (confilictStockCreationList.Count > 0)
                        {
                            foreach (var conflict in confilictStockCreationList)
                            {
                                conflict.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(conflict.RealStateType));
                                conflict.ProjectName = _commonBLL.GetProjectName(Convert.ToInt32(conflict.Project));
                                conflict.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(conflict.Phase));
                                conflict.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(conflict.Category));
                                conflict.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(conflict.Block));
                                conflict.NatureName = _commonBLL.GetNatureName(Convert.ToInt32(conflict.Nature));
                                conflict.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(conflict.Type));
                            }
                            return Ok(new ApiResponse<object>
                            {
                                Code = ResponseCode.Conflict,
                                Message = "This Number Already Exists",
                                Data = confilictStockCreationList
                            });
                        }
                    }

                    if ((bool)block.IsConfirmed)
                    {
                        string message = string.Empty;
                        bool request = false;

                        if (isApprovalActive == true)
                        { 
                            
                            foreach (var stockItem in stockCreationList)
                            {
                                _db.StockCreations.Add(stockItem);
                                _db.SaveChanges();
                                request = _approvalBLL.AddNewApprovalSetup(stockItem.ID, Convert.ToInt32(ApprovalUIIds.StockCreation));
                            }
                            message = "Stock Creation Successfully and moved for approval";
                        }

                        else
                        {
                            foreach (var stockItem in stockCreationList)
                            {
                                stockItem.Status = "Approved";
                                stockItem.Is_StockCreationApproved = true;
                                _db.StockCreations.Add(stockItem);
                                _db.SaveChanges();
                                var sapPostingResult = new SapIntegrationController(_db).AddSAPStock(stockItem.ID);

                            }
                            message = "Stock created successfully";
                            request = true; 
                        }

                        if (request)
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
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Confirmation,
                            Message = "Stock Creation for confimation",
                            Data = stockCreationList
                        });
                    }
                }
                else
                {
                    StockCreation obj = _db.StockCreations.Where(i => i.ID == block.ID).FirstOrDefault();
                    obj.Status = block.Status;
                    obj.User = block.User;
                    obj.ActualSize = block.ActualSize;
                    obj.ActualSizeUnit = block.ActualSizeUnit;
                    obj.ActualSize = block.ActualSize;
                    obj.Phase = block.Phase;
                    obj.Nature = block.Nature;
                    obj.Project = block.Project;
                    obj.Block = block.Block;
                    obj.Floor = block.Floor;
                    obj.Feature = block.Feature;
                    obj.Updated_at = DateTime.Now;
                    _db.Update(obj);
                    _db.SaveChanges();
                }
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        public async Task<Response_Result> PostRegistrationNumber(RegistrationNo block)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                block.Created_at = DateTime.Now;
                block.Updated_at = DateTime.Now;
                block.is_active = true;
                block.is_deleted = false;
                _db.RegistrationNos.Add(block);
                _db.SaveChanges();

                response_Results.message = "RegistrationNo Succesfully Added";
                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
            }
            catch (Exception ex)
            {
                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response_Results.message = ex.Message;
            }
            return response_Results;
        }

        [HttpPost]
        public IActionResult SaveBindProperty(List<PropertyBindingDTO> dto)
        {
            try
            {
                if (dto == null || !dto.Any())
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = null
                    });
                }

                foreach (var item in dto)
                {
                    if (dto.Where(x => x.registrationNo == item.registrationNo && !item.registrationNo.IsNullOrEmpty() && !item.propertyNo.IsNullOrEmpty()).Count() > 1)
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Conflict,
                            Message = "Duplication Not Allowed",
                            Data = null
                        });
                    }
                }
                var nullcount = 0;
                foreach (var item in dto)
                {

                    if (!item.registrationNo.IsNullOrEmpty() && !item.propertyNo.IsNullOrEmpty())
                    {
                        nullcount++;
                        StockCreation result = _db.StockCreations.Find(item.Id);
                        if (result != null && (result.RegistrationNo.IsNullOrEmpty() || result.PropertyNo.IsNullOrEmpty()))
                        {
                            string prop = result.PropertyNo;
                            string reg = result.RegistrationNo;

                            var blockId = _db.Blocks.FirstOrDefault(x=>x.Description == item.blockNo).ID;
                            var categoryId = _db.Categories.FirstOrDefault(x=>x.Description == item.category).ID;

                            if (prop.IsNullOrEmpty())
                            {
                                var stockExist = _db.StockCreations.Where(x => x.RegistrationNo == item.registrationNo && x.PropertyNo == null).FirstOrDefault();
                                var propertyAttributes = _db.StockCreations.Where(x => x.PropertyNo == item.propertyNo && x.Block == blockId.ToString() && x.Category == categoryId.ToString() && x.RegistrationNo == null).FirstOrDefault();
                                if (stockExist != null)
                                {
                                    stockExist.RegistrationNo = item.registrationNo;
                                    stockExist.PropertyNo = item.propertyNo;
                                    stockExist.PrefixProperty = propertyAttributes?.PrefixProperty;
                                    stockExist.numForProperty = propertyAttributes?.numForProperty;
                                    stockExist.postfixForProperty = propertyAttributes?.postfixForProperty;
                                    stockExist.Block = propertyAttributes?.Block;
                                    stockExist.Category = propertyAttributes?.Category;
                                    stockExist.RealStateType = propertyAttributes?.RealStateType;
                                    stockExist.ActualSize = propertyAttributes?.ActualSize;
                                    stockExist.ActualSizeUnit = propertyAttributes?.ActualSizeUnit;
                                    stockExist.Project = propertyAttributes?.Project;
                                    stockExist.Phase = propertyAttributes?.Phase;
                                    stockExist.Type = propertyAttributes?.Type;
                                    stockExist.Nature = propertyAttributes?.Nature;
                                    stockExist.Finishing = propertyAttributes?.Finishing;
                                    stockExist.Floor = propertyAttributes?.Floor;
                                    stockExist.PossessionStatus = propertyAttributes?.PossessionStatus;
                                    stockExist.ConstracutionStatus = propertyAttributes?.ConstracutionStatus;
                                    stockExist.coveredArea = propertyAttributes?.coveredArea;
                                    stockExist.LDAPlotNo = propertyAttributes?.LDAPlotNo;

                                    _db.StockCreations.Remove(propertyAttributes);
                                    _db.SaveChanges();
                                    _db.StockCreations.Update(stockExist);
                                    _db.SaveChanges();
                                    Response_Result updatePropertyInMemberProfile=new SapIntegrationController(_db).UpdatePropertyInMemberProfile(stockExist);
                                }
                            }

                            if (reg.IsNullOrEmpty())
                            {
                                var stockExist = _db.StockCreations.Where(x => x.PropertyNo == item.propertyNo && x.Block == blockId.ToString() && x.RegistrationNo == null).FirstOrDefault();

                                if (stockExist != null)
                                {
                                    stockExist.RegistrationNo = item.registrationNo;
                                    stockExist.PropertyNo = item.propertyNo;
                                    StockCreation delProp = _db.StockCreations.Where(x => x.RegistrationNo == item.registrationNo && x.PropertyNo == null).FirstOrDefault();
                                    _db.StockCreations.Remove(delProp);
                                    _db.SaveChanges();
                                    _db.StockCreations.Update(stockExist);
                                    _db.SaveChanges();
                                }
                            }

                        }
                    }
                }
                if (nullcount > 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Items not confilict with existing Saved Successfully",
                        Data = null
                    });
                }
                else
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.RecordNotFound,
                        Message = "Please Fill Relavant Data",
                        Data = null
                    });
                }

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        public IActionResult GetSingleStock(int stock)
        {
            Response_Result response_Results = new Response_Result();
            try
            {

                var result = _db.StockCreations.Where(x => x.ID == stock)
                                               .Include(x=>x.MemberProfile)
                                               .FirstOrDefault();
                if (result != null)
                {

                    result.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(result.RealStateType));
                    result.ProjectName = _commonBLL.GetProjectName(Convert.ToInt32(result.Project));
                    result.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(result.Phase));
                    result.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(result.Category));
                    result.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(result.Block));
                    result.NatureName = _commonBLL.GetNatureName(Convert.ToInt32(result.Nature));
                    result.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(result.Type));
                  
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
        public IActionResult PartialApproval(StockCeationAprrovalDTO approvalDTOs)
        {
            try
            {
                int count = 0;
                if(approvalDTOs.stockCreationResquestIds.Count > 0)
                { 
                    
                   foreach(var request in approvalDTOs.stockCreationResquestIds)
                    {
                        if (approvalDTOs.IsApproved == 4)
                        {
                            RequestApprovalStatusUpdateDTO model = new RequestApprovalStatusUpdateDTO();
                            {
                                model.RequestId = request.RequestId;
                                model.ApprovalUIId = approvalDTOs.ApprovalUIId;
                                model.UserId = approvalDTOs.UserId;
                                model.IsApproved = approvalDTOs.IsApproved;
                                model.Comment = approvalDTOs.Comment;
                            }

                            bool result = _approvalBLL.UpdateApprovalStatus(model);
                            if (result)
                            {
                                var sapPostingResult = new SapIntegrationController(_db).AddSAPStock(model.RequestId);
                            }
                        }
                        else
                        {
                            var removeStock = _db.StockCreations.Find(request.RequestId);
                            _db.StockCreations.Remove(removeStock);
                        }
                        count++;
                    }
                }
                if(approvalDTOs.stockCreationResquestIds.Count == count)
                { 
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = null
                    });
                }
                else
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "something went wrong",
                        Data = null
                    });
                }
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        public IActionResult PartialPossession(StockCeationAprrovalDTO approvalDTOs)
        {
            try
            {
                int count = 0;
                if (approvalDTOs.stockCreationResquestIds.Count > 0)
                {
                    foreach (var request in approvalDTOs.stockCreationResquestIds)
                    {
                        HRMS_Web.Models.DTOs.RequestApprovalStatusUpdateDTO model = new HRMS_Web.Models.DTOs.RequestApprovalStatusUpdateDTO();

                        model.RequestId = request.RequestId;
                        model.ApprovalUIId = approvalDTOs.ApprovalUIId;
                        model.UserId = approvalDTOs.UserId;
                        model.IsApproved = approvalDTOs.IsApproved;
                        model.Comment = approvalDTOs.Comment;

                        var approvalsController = new ApprovalsController(_db);

                        IActionResult result = approvalsController.UpdateApprovalStatus(model);

                        count++;
                    }
                }
                if (approvalDTOs.stockCreationResquestIds.Count == count)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = null
                    });
                }
                else
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "something went wrong",
                        Data = null
                    });
                }
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }
    }
}
