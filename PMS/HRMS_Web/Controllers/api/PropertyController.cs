using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using HRMS_Web.Models.DTOs;
using HRMS_Web.Models.DTOs.SPDtos;
using iTextSharp.text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize]
    public class PropertyController : ControllerBase
    {
        private readonly DataBase_Context _db;
        CommonBLL _commonBLL;

        public PropertyController(DataBase_Context db)
        {
            _db = db;
            _commonBLL = new CommonBLL(_db);
        }

        [HttpPost]
        [Route("/api/Property/UpdatePropertySetup")]
        public IActionResult UpdatePropertySetup(PropertySetupUpdateRequestDto model)
        {
            try
            {
                StockCreation stockCreation = _db.StockCreations.Find(model.StockCreationId);

                stockCreation.PropertyNo = model.PropertyNo;
                stockCreation.Phase = model.PhaseId;
                stockCreation.RealStateType = model.RealEsateId;
                stockCreation.Project = model.ProjectId;
                stockCreation.Phase = model.PhaseId;
                stockCreation.Block = model.BlockId;
                stockCreation.Category = model.CategoryId;
                stockCreation.Nature = model.NatureId;
                stockCreation.Type = model.TypeId;
                stockCreation.PrefixProperty = model.PrefixProperty;
                stockCreation.PossessionStatus = model.PossessionStatus;
                stockCreation.ConstracutionStatus = model.ConstructionStatus;
                stockCreation.PropertyStatus = model.PropertyStatus;
                stockCreation.GrancePeriodForBillGenration = model.GracePeriodDate;
                stockCreation.ActualSize = model.ActualSize;
                stockCreation.coveredArea = model.coveredArea;
                stockCreation.DiscountPercent = model.DiscountPercent;
                stockCreation.Feature = model.Feature;
                stockCreation.Latitude = model.Latitude;
                stockCreation.Longitude = model.Longitude;
                stockCreation.CaseCode = model.CaseCode;
                stockCreation.AffidavitCode = model.AffidavitCode;
                stockCreation.SaleDeedNo = model.SaleDeedNo;
                stockCreation.SaleDeedDate = model.SaleDeedDate;
                stockCreation.Mouza = model.Mouza;
                stockCreation.AllocationNo = model.AllocationNo;
                stockCreation.MembershipFee = model.MembershipFee;
                stockCreation.MiscCharges = model.MiscCharges;
                stockCreation.TransferRecordOfficerName = model.TransferRecordOfficerName;
                stockCreation.TransferRecordDirectorName = model.TransferRecordDirectorName;

                stockCreation.FrontSide = model.FrontSide;
                stockCreation.RearSide = model.RearSide;
                stockCreation.LeftSide = model.LeftSide;
                stockCreation.RightSide = model.RightSide;

                stockCreation.FrontBoundary = model.FrontBoundary;
                stockCreation.RearBoundary = model.RearBoundary;
                stockCreation.LeftBoundary = model.LeftBoundary;
                stockCreation.RightBoundary = model.RightBoundary;

                stockCreation.StandardAreaOfPlot = model.StandardAreaOfPlot;
                stockCreation.AreaOfPlot = model.AreaOfPlot;
                stockCreation.ExcessArea = model.ExcessArea;
                stockCreation.LessArea = model.LessArea;

                stockCreation.ApprovedMinSheetReferenceNo = model.ApprovedMinSheetReferenceNo;

                stockCreation.IsCornerPlot = model.IsCornerPlot;
                stockCreation.IsParkFacing = model.IsParkFacing;
                stockCreation.IsMainBoulevard = model.IsMainBoulevard;

                stockCreation.SurveyorName = model.SurveyorName;
                stockCreation.BuildingControlDirectorName = model.BuildingControlDirectorName;

                stockCreation.DuesClearedTillDate = model.DuesClearedTillDate;
                stockCreation.NdcNo = model.NdcNo;
                stockCreation.NdcType = model.NdcType;

                stockCreation.FinanceOfficerName = model.FinanceOfficerName;
                stockCreation.FinanceDirectorName = model.FinanceDirectorName;

                stockCreation.PossessionHandedOverOn = model.PossessionHandedOverOn;
                stockCreation.PossessionNo = model.PossessionNo;
                stockCreation.PossessionSurveyorName = model.PossessionSurveyorName;
                stockCreation.OwnerName = model.OwnerName;
                stockCreation.Created_By = (int)model.ModifiedBy;
                stockCreation.Created_at = DateTime.Now;


                _db.StockCreations.Update(stockCreation);
                _db.SaveChanges();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Property Update Successfully",
                    Data = model
                });

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("/api/Property/GetAllPossessioned")]
        public IActionResult GetAllPossessioned(
          int draw,
          int start,
          int length,
          string? search = ""
      )
        {
            try
            {
                var query = _db.StockCreations
                    .Where(x => !x.is_deleted && x.Is_PossessionRequested == true);

                // 🔍 SEARCH (optional)
                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(x =>
                        x.RegistrationNo.Contains(search) ||
                        x.MemberProfile.MemberName.Contains(search) ||
                        x.MemberProfile.MEMBERSHIPNO.Contains(search) ||
                        x.MemberProfile.Cnic.Contains(search)
                    );
                }

                var recordsTotal = query.Count();

                var data = query
                    .OrderByDescending(x => x.ID)
                    .Skip(start)
                    .Take(length)
                    .Select(x => new 
                    {
                        Id = x.ID,
                        RegistrationNo = x.RegistrationNo,
                        MembershipNo = x.MemberProfile.MEMBERSHIPNO,
                        MemberName = x.MemberProfile != null
                            ? x.MemberProfile.MemberName
                            : string.Empty,

                        Cnic = x.MemberProfile.Cnic != null
                            ? x.MemberProfile.Cnic
                            : string.Empty,                    
                       PossessionDate = x.PossessionEffectDate.Value.ToString("dd MMM yyyy")
                    })
                    .ToList();

                return Ok(new
                {
                    draw = draw,
                    recordsTotal = recordsTotal,
                    recordsFiltered = recordsTotal,
                    data = data
                });
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        public async Task<Response_Result> SavePropertyList(PropertyList property)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                if (property.ID == 0)
                {
                    property.Created_at = DateTime.Now;
                    property.Updated_at = DateTime.Now;
                    property.is_active = true;
                    property.is_deleted = false;
                    _db.PropertyLists.Add(property);
                    _db.SaveChanges();


                    response_Results.message = "Property Definition Succesfully Added";
                    response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                }
                else
                {
                    PropertyList obj = _db.PropertyLists.Where(i => i.ID == property.ID).FirstOrDefault();
                    obj.Description = property.Description;
                    obj.PropertyNo = property.PropertyNo;
                    obj.RegistrationNo = property.RegistrationNo;
                    obj.Phase = property.Phase;
                    obj.Block = property.Block;
                    obj.Nature = property.Nature;
                    obj.Category = property.Category;
                    obj.Status = property.Status;

                    obj.Updated_at = DateTime.Now;
                    _db.Update(obj);
                    _db.SaveChanges();
                    response_Results.message = "Property Definition Succesfully Updated";
                    response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                }
            }
            catch (Exception ex)
            {

                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response_Results.message = ex.Message;

            }
            return response_Results;

        }

        [HttpGet]
        [Route("/api/Property/GetAllStockCreationApprovedPropertyLists")]
        public async Task<Response_Result> GetAllStockCreationApprovedPropertyLists()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                var blocks = _db.StockCreations.Where(x => x.is_active == true
                                                    && x.PropertyNo != ""
                                                    && x.PropertyNo != null
                                                    && x.RegistrationNo != ""
                                                    && x.RegistrationNo != null
                                                    && x.PossessionEffectDate == null
                                                    && x.PossessionStatus !=true
                                                    && x.Is_StockCreationApproved == true)
                                               .Distinct()
                                               .ToList();                                            ;
                if (blocks?.Count > 0)
                {
                    foreach (var block in blocks)
                    {
                        block.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(block.RealStateType));
                        block.ProjectName = _commonBLL.GetProjectName(Convert.ToInt32(block.Project));
                        block.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(block.Phase));
                        block.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(block.Category));
                        block.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(block.Block));
                        block.NatureName = _commonBLL.GetNatureName(Convert.ToInt32(block.Nature));
                        block.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(block.Type));
                    }
                }

                response_Results.data = blocks;
                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);

            }
            catch (Exception ex)
            {

                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response_Results.message = ex.Message;

            }
            return response_Results;
        }

        [HttpGet]
        [Route("/api/Property/GetAllPropertyLists")]
        public async Task<Response_Result> GetAllPropertyLists()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                var blocks = _db.StockCreations.Where(i => i.is_active == true).OrderByDescending(x => x.ID).Distinct().ToList();
                if (blocks?.Count > 0)
                {
                    foreach (var block in blocks)
                    {
                        block.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(block.RealStateType));
                        block.ProjectName = _commonBLL.GetProjectName(Convert.ToInt32(block.Project));
                        block.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(block.Phase));
                        block.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(block.Category));
                        block.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(block.Block));
                        block.NatureName = _commonBLL.GetNatureName(Convert.ToInt32(block.Nature));
                        block.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(block.Type));
                    }
                }

                response_Results.data = blocks;
                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);

            }
            catch (Exception ex)
            {

                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response_Results.message = ex.Message;

            }
            return response_Results;
        }

        [HttpGet]
        [Route("/api/Property/GetAllRegistrationNameAndId")]
        public async Task<Response_Result> GetAllRegistrationNameAndId()
        {
            Response_Result response_Results = new Response_Result();
            List<RegistrationNoDto> registrationList = new List<RegistrationNoDto>();
            try
            {
                var blocks = _db.StockCreations.Where(i => i.is_active == true && i.DealerId==null && i.MemberProfileId==null).OrderByDescending(x => x.ID).Distinct().ToList();
                if (blocks?.Count > 0)
                {
                    foreach (var block in blocks)
                    {
                        RegistrationNoDto registrationNo = new RegistrationNoDto();
                        registrationNo.Id = block.ID;
                        registrationNo.registrationNo=block.RegistrationNo;
                        registrationList.Add(registrationNo);
                    }
                }

                response_Results.data = registrationList;
                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);

            }
            catch (Exception ex)
            {

                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response_Results.message = ex.Message;

            }
            return response_Results;
        }
        [HttpGet]
        [Route("/api/Property/GetAllRegistrationNameAndIdAllData")]
        public async Task<Response_Result> GetAllRegistrationNameAndIdAllData()
        {
            Response_Result response_Results = new Response_Result();
            List<RegistrationNoDto> registrationList = new List<RegistrationNoDto>();
            try
            {
                var blocks = _db.StockCreations.Where(i => i.is_active == true).OrderByDescending(x => x.ID).Distinct().ToList();
                if (blocks?.Count > 0)
                {
                    foreach (var block in blocks)
                    {
                        RegistrationNoDto registrationNo = new RegistrationNoDto();
                        registrationNo.Id = block.ID;
                        registrationNo.registrationNo=block.RegistrationNo;
                        registrationList.Add(registrationNo);
                    }
                }

                response_Results.data = registrationList;
                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);

            }
            catch (Exception ex)
            {

                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response_Results.message = ex.Message;

            }
            return response_Results;
        }

        [HttpGet]
        public async Task<Response_Result> DeletePropertyList(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {


                StockCreation obj = _db.StockCreations.Where(i => i.ID == id).FirstOrDefault();
                obj.is_deleted = true;
                obj.is_active = false;
                obj.Status = "In Active";
                _db.Update(obj);
                _db.SaveChanges();
                response_Results.message = "Property Definition Deleted Successfully";
                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);

            }
            catch (Exception ex)
            {

                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response_Results.message = ex.Message;

            }
            return response_Results;

        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<Response_Result> GetSinglePropertyForBulkDeal(int? id,int dealId)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                var bulkdealproposePlans=_db.BulkDealProposePlan.Where(p => p.BulkDealId == dealId).ToList();

                StockCreation block = _db.StockCreations.Where(i => i.ID == id).FirstOrDefault();
                if (block != null)
                {
                    var category = Convert.ToInt32(block.Category);
                    var bulkdealproposePlan = bulkdealproposePlans.Where(s => s.CategoryId == category).FirstOrDefault();


                        block.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(block.RealStateType));
                        block.ProjectName = _commonBLL.GetProjectName(Convert.ToInt32(block.Project));
                        block.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(block.Phase));
                        block.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(block.Category));
                        block.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(block.Block));
                        block.NatureName = _commonBLL.GetNatureName(Convert.ToInt32(block.Nature));
                        block.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(block.Type));
                        block.BulkDealAmount = bulkdealproposePlan==null?0: bulkdealproposePlan.TotalAmount;


                }
                response_Results.data = block;

                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);

            }
            catch (Exception ex)
            {

                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response_Results.message = ex.Message;

            }
            return response_Results;

        }
    }
}
