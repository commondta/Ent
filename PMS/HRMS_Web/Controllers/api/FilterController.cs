using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using B_Utility.Common.Enums;
using HRMS_Web.Extensions;
using HRMS_Web.Models.DTOs;
using HRMS_Web.Models.DTOs.SPDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Data.Common;
using System.Linq;
using System.Web.Http.Results;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FilterController : ControllerBase
    {
        private readonly DataBase_Context _db;

        CommonBLL _commonBLL;
        public FilterController(DataBase_Context db)
        {
            _db = db;
            _commonBLL = new CommonBLL(_db);
        }

        private List<PropertBasicDetailsDto> GetAllProprttiesSP(int pageNumber = 1, int pageSize = 10, string? searchTerm = "")
        {
            var properties = new List<PropertBasicDetailsDto>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            var query = $"EXEC [dbo].[GetBasicPropertyDetailsSpPagination] @PageNumber = @PageNumber, @PageSize = @PageSize, @SearchTerm = @SearchTerm";
            var DataModel = _db.JsonOutPutModel.FromSqlRaw(query, pageNumberParam, pageSizeParam, searchTermParam).ToList();

            if (DataModel != null && DataModel.Any())
            {
                var jsonValue = DataModel.FirstOrDefault()?.JsonStringValue;
                if (!String.IsNullOrEmpty(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<PropertBasicDetailsDto>>(jsonValue);
                }
            }

            return properties;
        }

        /*---USE----
        -----MemberNDC---
        */

        [HttpPost]
        [Route("GetAllProprtties")]
        public IActionResult GetAllProprtties()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            var data = GetAllProprttiesSP(pageNumber, pageSize, searchValue);
            totalRecord = _db.StockCreations.Count();

            filterRecord = totalRecord;

            var returnObj = new
            {
                draw = draw,
                recordsTotal = totalRecord,
                recordsFiltered = filterRecord,
                data = data.ToList()
            };
            return Ok(returnObj);
        }

        [HttpGet]
        [Route("GetAllNotPostedInvoicesForBooking")]
        public IActionResult GetAllNotPostedInvoicesForBooking()
        {
            try
            {

                var result = (from booking in _db.Booking
                              join stock in _db.StockCreations
                              on booking.StockCreationId equals stock.ID
                              join backlog in _db.BookingBackLog
                              on booking.Id equals backlog.BookingId
                              select new
                              {
                                  booking.Id,
                                  stock.PropertyNo,
                                  stock.RegistrationNo,
                                  booking.MemberProfile.MemberName,
                                  booking.BookingPrice,
                                  backlog.BookingType,
                                 backlog.ErrorMessage,
                                 backlog.BookingChargePosted
                              })
                              .Distinct()
              .OrderByDescending(x => x.Id)
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
        [Route("GetFilterPaymentPlan")]
        public IActionResult GetFilterPaymentPlan()
        {
            try
            {
                //need join here to get value from booking DTo used as well
                var result = _db.PaymentPlanSetup.Where(x => !x.IsDeleted)
                                                 .ToList();
                if (result.Count > 0)
                {
                    foreach (var x in result)
                    {
                        x.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(x.RealEsateId));
                        x.ProjectName = _commonBLL.GetProjectName(Convert.ToInt32(x.ProjectId));
                        x.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(x.PhaseId));
                        x.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(x.CategoryId));
                        x.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(x.BlockId));
                        x.NatureName = _commonBLL.GetNatureName(Convert.ToInt32(x.NatureId));
                        x.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(x.TypeId));
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
        [Route("GetFilterPropertyResultWithMemberProfile")]
        [AllowAnonymous]
        public IActionResult GetFilterPropertyResultWithMemberProfile(int id)
        {
            try
            {
                // need join here to get value from booking DTo used as well
                var result = _db.StockCreations.Where(x => x.ID == id)
                                               .Include(x => x.MemberProfile)
                                               .Include(x => x.Dealer)
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
                    result.ConstracutionStatus = _commonBLL.GetConstrcutionStatus(Convert.ToInt32(result.ID));
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
        [Route("GetFilterPropertyResult")]
        public IActionResult GetFilterPropertyResult(int id)
        {
            try
            {
                // need join here to get value from booking DTo used as well
                var result = _db.StockCreations.Where(x => !x.is_deleted && x.ID == id)
                                               .Include(x => x.MemberProfile)
                                               .Include(x => x.Dealer)
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
                    result.ConstracutionStatus = _commonBLL.GetConstrcutionStatus(Convert.ToInt32(result.ID));
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
        [Route("GetFilterPropertyResultForNDC")]
        public IActionResult GetFilterPropertyResultForNDC()
        {
            try
            {
                var result = _db.StockCreations.Where(x => !x.is_deleted
                                                      && x.Is_StockCreationApproved == true
                                                      && x.MemberProfileId != null)
                                               .Select(x => new
                                               {
                                                   x.ID,
                                                   x.RegistrationNo,
                                                   x.PropertyNo,
                                                   x.MemberProfile.MemberName,
                                                   x.MemberProfile.Cnic,
                                                   x.Status
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
        [Route("GetProprtySetupById")]
        public IActionResult GetProprtySetupById(int stockId)
        {
            try
            {
                var result = _db.StockCreations
                    .Where(x => x.ID == stockId)
                    .Select(x => new PropertySetupDTO
                    {
                        // =========================
                        // Existing Fields
                        // =========================
                        ID = x.ID,
                        RegistrationNo = x.RegistrationNo,
                        PropertyNo = x.PropertyNo,
                        MemberName = x.MemberProfile.MemberName,
                        MemberProfileId = x.MemberProfileId,
                        Cnic = x.MemberProfile.Cnic,
                        CnicExpiryDate = (DateTime)x.MemberProfile.CnicExpiryDate,
                        RealStateType = x.RealStateType,
                        Phase = x.Phase,
                        Project = x.Project,
                        Category = x.Category,
                        Type = x.Type,
                        Nature = x.Nature,
                        Block = x.Block,
                        Sector = x.PrefixProperty,
                        ConstructionStatus = x.ConstracutionStatus,
                        PossessionStatus = x.PossessionStatus,
                        GracePeriodDate = x.GrancePeriodForBillGenration,
                        ActualSize = x.ActualSize,
                        coveredArea = x.coveredArea,
                        Feature = x.Feature,
                        DiscountPercent = x.DiscountPercent,
                        Latitude = x.Latitude,
                        Longitude = x.Longitude,
                        CaseCode = x.CaseCode,
                        AffidavitCode = x.AffidavitCode,
                        SaleDeedNo = x.SaleDeedNo,
                        SaleDeedDate = x.SaleDeedDate,
                        Mouza = x.Mouza,
                        AllocationNo = x.AllocationNo,
                        PropertyStatus = x.PropertyStatus,
                        MembershipFee = x.MembershipFee,
                        MiscCharges = x.MiscCharges,
                        PossessionNo = x.PossessionNo,

                        // =========================
                        // NEW - Transfer & Record Branch
                        // =========================
                        TransferRecordOfficerName = x.TransferRecordOfficerName,
                        TransferRecordDirectorName = x.TransferRecordDirectorName,

                        // =========================
                        // NEW - Boundary Details
                        // =========================
                        FrontSide = x.FrontSide,
                        RearSide = x.RearSide,
                        LeftSide = x.LeftSide,
                        RightSide = x.RightSide,

                        FrontBoundary = x.FrontBoundary,
                        RearBoundary = x.RearBoundary,
                        LeftBoundary = x.LeftBoundary,
                        RightBoundary = x.RightBoundary,

                        // =========================
                        // NEW - Area Details
                        // =========================
                        StandardAreaOfPlot = x.StandardAreaOfPlot,
                        AreaOfPlot = x.AreaOfPlot,
                        ExcessArea = x.ExcessArea,
                        LessArea = x.LessArea,
                        ApprovedMinSheetReferenceNo = x.ApprovedMinSheetReferenceNo,

                        // =========================
                        // NEW - Plot Features
                        // =========================
                        IsCornerPlot = x.IsCornerPlot,
                        IsParkFacing = x.IsParkFacing,
                        IsMainBoulevard = x.IsMainBoulevard,

                        // =========================
                        // NEW - Finance Branch
                        // =========================
                        DuesClearedTillDate = x.DuesClearedTillDate,
                        NdcNo = x.NdcNo,
                        NdcType = x.NdcType,
                        FinanceOfficerName = x.FinanceOfficerName,
                        FinanceDirectorName = x.FinanceDirectorName,

                        // =========================
                        // NEW - Possession / Handover
                        // =========================
                        PossessionHandedOverOn = x.PossessionHandedOverOn,
                        PossessionSurveyorName = x.PossessionSurveyorName,
                        OwnerName = x.OwnerName,
                        SurveyorName = x.SurveyorName,
                        BuildingControlDirectorName = x.BuildingControlDirectorName
                    })
                    .FirstOrDefault();

                if (result != null)
                {
                    result.CategoryName = result.Category != null
                        ? _commonBLL.GetCategoryName(Convert.ToInt32(result.Category))
                        : "N/A";

                    result.BlockName = result.Block != null
                        ? _commonBLL.GetBlockName(Convert.ToInt32(result.Block))
                        : "N/A";

                    result.TypeName = result.Type != null
                        ? _commonBLL.GetTypeName(Convert.ToInt32(result.Type))
                        : "N/A";
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

        [HttpGet]
        [Route("GetFilterPropertyResultForNDCById")]
        public IActionResult GetFilterPropertyResultForNDCById(int stockId)
        {
            try
            {
                // need join here to get value from booking DTo used as well
                var result = _db.StockCreations.Where(x => x.ID == stockId)
                                               .Select(x => new NdcFilterDto
                                               {
                                                   ID = x.ID,
                                                   RegistrationNo = x.RegistrationNo,
                                                   PropertyNo = x.PropertyNo,
                                                   MemberName = x.MemberProfile.MemberName,
                                                   MemberProfileId = x.MemberProfileId,
                                                   Cnic = x.MemberProfile.Cnic,
                                                   CnicExpiryDate = (DateTime)x.MemberProfile.CnicExpiryDate,
                                                   RealStateType = x.RealStateType,
                                                   Phase = x.Phase,
                                                   Project = x.Project,
                                                   Category = x.Category,
                                                   Type = x.Type,
                                                   Nature = x.Nature,
                                                   Block = x.Block,
                                                   Sector = x.PrefixProperty,
                                                   ConstructionStatus = x.ConstracutionStatus,
                                                   PossessionStatus = x.PossessionStatus,
                                                   GracePeriodDate = x.GrancePeriodForBillGenration,
                                                   ActualSize = x.ActualSize,
                                                   coveredArea = x.coveredArea,
                                                   Feature = x.Feature,
                                                   DiscountPercent = x.DiscountPercent,
                                                   Latitude = x.Latitude,
                                                   Longitude = x.Longitude,
                                                   CaseCode = x.CaseCode,
                                                   AffidavitCode = x.AffidavitCode,
                                                   SaleDeedNo = x.SaleDeedNo,
                                                   SaleDeedDate = x.SaleDeedDate,
                                                   Mouza = x.Mouza,
                                                   AllocationNo = x.AllocationNo,
                                                   PropertyStatus = x.PropertyStatus,
                                                   MembershipFee = x.MembershipFee,
                                                   MiscCharges = x.MiscCharges
                                               })
                                               .FirstOrDefault();

                if (result != null)
                {
                    result.CategoryName = result.Category != null ? _commonBLL.GetCategoryName(Convert.ToInt32(result.Category)) : "N/A";
                    result.BlockName = result.Block != null ?_commonBLL.GetBlockName(Convert.ToInt32(result.Block)) : "N/A";
                    result.TypeName = result.Type != null ? _commonBLL.GetTypeName(Convert.ToInt32(result.Type)) : "N/A";
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
        [Route("GetFilterPropertyResultDealerNDCByRegNo")]
        public IActionResult GetFilterPropertyResultDealerNDCByRegNo(string stockId)
        {
            try
            {
                var result = _db.StockCreations.Where(x => !x.is_deleted && x.RegistrationNo == stockId)
                                               .Select(x => new
                                               {
                                                   x.ID,
                                                   x.RegistrationNo,
                                                   x.PropertyNo,
                                                   x.MemberProfile.MemberName,
                                                   x.MemberProfileId,
                                                   x.MemberProfile.Cnic,
                                                   x.MemberProfile.CnicExpiryDate,
                                                   x.RealStateType,
                                                   x.Phase,
                                                   x.Project,
                                                   x.Category,
                                                   x.Type,
                                                   x.Nature,
                                                   x.Block
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
        [Route("GetFilterPropertyResultForNDC1")]
        public IActionResult GetFilterPropertyResultForNDC1()
        {
            try
            {
                var result = _db.NDCRequestForMember.Where(x => !x.IsDeleted &&
                                                           x.IsNDCRequestForMemberApproved == true &&
                                                           x.IsCanceled != true &&
                                                           x.IsActive != false
                                                      )
                                               .Select(x => new
                                               {
                                                   x.Id,
                                                   x.StockCreation.RegistrationNo,
                                                   x.StockCreation.PropertyNo,
                                                   x.MemberProfile.MemberName,
                                                   x.MemberProfile.Cnic,
                                                   x.CreatedOn
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
        [Route("GetFilterPropertyResultForNDC1ById")]
        public IActionResult GetFilterPropertyResultForNDC1ById(int ndcId)
        {
            try
            {
                var result = _db.NDCRequestForMember.Where(x => !x.IsDeleted && x.Id == ndcId)
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
                                                   x.DealerCode,
                                                   x.EstateName,
                                                   x.DealerName,
                                                   x.ValidityDate,
                                                   x.SlotDate,
                                                   x.SlotHour,
                                                   x.SlotMintues,
                                                   x.NDCRequestType,
                                                   x.TransferType.Description,
                                                   x.Outstation,
                                                   x.Day,
                                                   x.ApplyStation,
                                                   x.Processing,
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
        [Route("GetMemberByStockId")]
        public IActionResult GetMemberByStockId(int stockId)
        {
            try
            {
                var result = _db.StockCreations.Where(x => !x.is_deleted && x.ID == stockId)
                                              .Select(x => new
                                              {
                                                  x.MemberProfileId,
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
        [Route("GetAllAvailablePropertiesForTransfer")]
        public IActionResult GetAllAvailablePropertiesForTransfer()
        {
            try
            {
                var result = _db.TransferHistery.Where(x => !x.IsDeleted && x.IsGovtProcessingTaxApproved == true && x.IsRequestClosed != true)
                                                .Include(x => x.MemberProfile)
                                                .Include(x => x.StockCreation)
                                                .ToList()
                                                .OrderByDescending(x => x.Id)
                                                .DistinctBy(x => x.StockCreationId);

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
        [Route("GetFilterDealByDealerId")]
        public IActionResult GetFilterDealByDealerId(int id)
        {
            try
            {
                var result = _db.Deal.Where(x => !x.IsDeleted && x.DealerId == id)
                                                 .Distinct()
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
        [Route("GetFilterDealById")]
        public IActionResult GetFilterDealById(int Id)
        {
            try
            {
                var result = _db.Deal.Where(x => !x.IsDeleted && x.Id == Id)
                                                 .Distinct()
                                                 .Include(x => x.DealProperty)
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
        [Route("GetAllPropertiesAvailableForBooking")]
        public IActionResult GetAllPropertiesAvailableForBooking()
        {
            try
            {
                var result = (from pre in _db.PreSale
                              join stock in _db.StockCreations
                              on pre.StockCreationId equals stock.ID
                              where stock.IsPreSaleApproved == true
                              //&& stock.IsBookingRequested != true
                              select new
                              {
                                  pre.Id,
                                  stock.PropertyNo,
                                  stock.RegistrationNo,
                                  pre.MemberProfile.MemberName,
                                  pre.MemberProfile.MEMBERSHIPNO,
                                  pre.MemberProfile.Mobile,
                                  pre.MemberProfile.Cnic
                              }).ToList().OrderByDescending(x => x.Id);

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
        [Route("GetAllPropertiesAvailableSurrender")]
        public IActionResult GetAllPropertiesAvailableSurrender()
        {
            try
            {
                var result = (from memberNdc in _db.NDCRequestForMember.Where(x => !x.IsDeleted && x.IsCanceled != true &&
                                                                              x.Processing == true &&
                                                                              x.IsSurrenderRequested != true &&
                                                                              x.IsRequestedClosed != true &&
                                                                              x.IsNDCRequestForMemberApproved == true
                                                                              )
                                                                        .Include(x=>x.StockCreation)
                              join ndc1 in _db.NDC1.Where(x => x.IsCanceled != true && x.IsNDC1Requested == true)
                              on memberNdc.StockCreationId equals ndc1.StockCreationId

                              select new
                              {
                                  memberNdc.StockCreation.RegistrationNo,
                                  memberNdc.StockCreation.PropertyNo,
                                  memberNdc.MemberProfile.MemberName,
                                  memberNdc.MemberProfile.Mobile,
                                  memberNdc.StockCreation.ID,
                                  memberNdc.StockCreation.Status,
                                  memberNdc.CreatedOn
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
        [Route("GetAllPropertiesAvailableMeterInstallationById")]
        public IActionResult GetAllPropertiesAvailableMeterInstallationById(int id)
        {
            try
            {
                var result = _db.StockCreations.Where(x => !x.is_deleted && x.ID == id && x.MemberProfileId != null)
                                               .Include(x => x.MemberProfile)
                                               .Distinct()
                                               .Select(x => new
                                               {
                                                   x.RegistrationNo,
                                                   x.PropertyNo,
                                                   MemberCode = x.MemberProfile.Id,
                                                   x.MemberProfile.MemberName,
                                                   x.MemberProfile.Mobile,
                                                   x.ID,
                                                   x.Status,
                                                   Project = _db.Projects.Where(p => p.ID == (Convert.ToInt32(x.Project))).Select(x => x.Description).FirstOrDefault()
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
        [Route("GetAllPropertiesAvailableSurrenderById")]
        public IActionResult GetAllPropertiesAvailableSurrenderById(int id)
        {
            try
            {
                var result = _db.NDCRequestForMember.Where(x => !x.IsDeleted && x.IsCanceled != true && x.Processing == true && x.StockCreationId == id)
                                               .Include(x => x.StockCreation)
                                               .Distinct()
                                               .Select(x => new
                                               {
                                                   x.StockCreation.RegistrationNo,
                                                   x.StockCreation.PropertyNo,
                                                   MemberCode = x.MemberProfile.Id,
                                                   x.MemberProfile.MemberName,
                                                   x.MemberProfile.Mobile,
                                                   x.DealerCode,
                                                   EstateName = x.DealerCode,
                                                   x.DealerName,
                                                   x.StockCreation.ID,
                                                   x.StockCreation.ActualSize,
                                                   x.StockCreation.PossessionStatus,
                                                   x.StockCreation.ConstracutionStatus,
                                                   x.StockCreation.Status,
                                                   DealerId = _db.Dealers.Where(p => p.EstateName == x.DealerCode).Select(x => x.Id).FirstOrDefault(),
                                                   Project = _db.Projects.Where(p => p.ID == (Convert.ToInt32(x.StockCreation.Project))).Select(x => x.Description).FirstOrDefault(),
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
        [Route("GetAllSurrenderAvailableForReSurrender")]
        public IActionResult GetAllSurrenderAvailableForReSurrender()
        {
            try
            {
                var result = _db.Surrender.Where(x => !x.IsDeleted && x.IsRequestClosed != true)
                                               .Include(x => x.Dealer)
                                               .Include(x => x.StockCreation)
                                               .Distinct()
                                               .Select(x => new
                                               {
                                                   x.StockCreation.RegistrationNo,
                                                   x.StockCreation.PropertyNo,
                                                   x.Dealer.PrincipalOwner,
                                                   x.ResurrenderDate,
                                                   x.CreatedOn,
                                                   x.ExpiryDays,
                                                   x.Id,
                                                   x.Status
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
        [Route("GetSurrenderHistory")]
        public IActionResult GetSurrenderHistory(int id)
        {
            try
            {

                var result = _db.SurrenderHistery
                                .Where(x => !x.IsDeleted && x.StockCreationId == id)
                                .Include(x => x.StockCreation.MemberProfile)
                                .Include(x => x.Dealer)
                                .Select(x => new
                                {
                                    x.Id,
                                    x.StockCreation.RegistrationNo,
                                    x.StockCreation.PropertyNo,
                                    MemberName = x.StockCreation.MemberProfile.MemberName,
                                    MemberCode = x.StockCreation.MemberProfile.Id,
                                    x.StockCreationId,
                                    DealerId = x.Dealer.Id,
                                    x.DealerName,
                                    x.EstateName,
                                    x.CreatedOn,
                                    x.ExpiryDays,
                                    x.ResurrenderDate,
                                    x.Remarks,
                                    x.Status
                                })
                                .OrderByDescending(x => x.Id)
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
        [Route("GetTransferHistory")]
        public IActionResult GetTransferHistory(int id)
        {
            try
            {
                List<TransferHistoricalDataDTO> transfers = new List<TransferHistoricalDataDTO>();

                var result1 = _db.TransferHistery.Where(x => !x.IsDeleted && x.StockCreationId == id)
                                                .Select(x => new TransferHistoricalDataDTO
                                                {
                                                    BuyerName = x.MemberProfile.MemberName,
                                                    BuyerCNIC = x.MemberProfile.Cnic,
                                                    SellerName = x.SellerName,
                                                    SellerCNIC = x.SellerCnic,
                                                    TransferDate = x.LastModified,
                                                    Source = "PMS"
                                                })
                                                .OrderByDescending(x => x.TransferDate)
                                                .ToList();

                string registrationNo = _db.StockCreations.Where(x => x.ID == id).IgnoreQueryFilters().FirstOrDefault().RegistrationNo;

                var result2 = _db.TransferHistoricalData.Where(x => x.RegistrationNo == registrationNo)
                                                .Select(x => new TransferHistoricalDataDTO
                                                {
                                                    BuyerName = x.BuyerName,
                                                    BuyerCNIC = x.BuyerCNIC,
                                                    SellerName = x.SellerName,
                                                    SellerCNIC = x.SellerCNIC,
                                                    TransferDate = x.TransferDate,
                                                    Source = "HISTORICAL"
                                                })
                                                .OrderByDescending(x => x.TransferDate)
                                                .ToList();
                transfers.AddRange(result1);
                transfers.AddRange(result2);

                transfers.OrderByDescending(x => x.TransferDate);

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = transfers
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetTransferHistoryToday")]
        public IActionResult GetTransferHistory()
        {
            try
            {

                var result = _db.TransferHistery.Where(x => !x.IsDeleted && x.CreatedOn == DateTime.Now.Date)
                                                .Select(x => new
                                                {
                                                    x.MemberProfile.MemberName,
                                                    x.MemberProfile.Cnic,
                                                    x.SellerName,
                                                    x.SellerCnic,
                                                    x.LastModified,
                                                    x.StockCreation.RegistrationNo,
                                                    x.StockCreation.PropertyNo
                                                })
                                                .OrderByDescending(x => x.LastModified)
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
        [Route("GetAllPropertiesAvailableMeterInstallation")]
        public IActionResult GetAllPropertiesAvailableMeterInstallation()
        {
            try
            {
                var result = _db.StockCreations.Where(x => !x.is_deleted &&
                                               x.MemberProfileId != null &&
                                               x.PropertyNo != null &&
                                               x.PropertyNo != "" &&
                                               x.RegistrationNo != null &&
                                               x.RegistrationNo != "")
                                               .Include(x => x.MemberProfile)
                                               .Distinct()
                                               .Select(x => new
                                               {
                                                   x.RegistrationNo,
                                                   x.PropertyNo,
                                                   x.MemberProfile.MemberName,
                                                   x.MemberProfile.Mobile,
                                                   x.ID,
                                                   x.Status,
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
        [Route("GetAllMetersReadingMonthwise")]
        public IActionResult GetAllMetersReadingMonthwise()
        {
            try
            {
                var result = _db.MeterReading.Where(x => !x.IsDeleted)
                                             .Select(x => new
                                             {
                                                 x.ReadingFor,
                                                 x.Month,
                                                 x.Id
                                             })
                                             .Distinct()
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
        [Route("GetMeterNumbersForMeterReading")]
        public IActionResult GetMeterNumbersForMeterReading(int meterType)
        {
            try
            {
                var result = _db.MeterDetail.Where(x => !x.IsDeleted && x.MeterTypeId == meterType && x.Status == "Active")
                                            .Include(x => x.MeterInstallation)
                                            .Select(x => new
                                            {
                                                x.Id,
                                                x.MeterNumber,
                                                x.MeterInstallation.PropertyNo
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
        [Route("GetMeterNumbersForMeterReadingById")]
        public IActionResult GetMeterNumbersForMeterReadingById(int id)
        {
            try
            {
                var result = _db.MeterDetail.Where(x => !x.IsDeleted && x.Id == id)
                                            .Select(x => new
                                            {
                                                x.MeterNumber,
                                                x.UnitsAtInstallation,
                                                x.MeterInstallation.PropertyNo
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

        // BillGenrationInBulk 

        [HttpGet]
        [Route("GetMeterReadingForBillGenrationInBulk")]
        public IActionResult GetMeterReadingForBillGenrationInBulk(string readingFor, string month, string? fuelAjustedMonth, decimal? fuelAdjustment = 1)
        {
            try
            {
                var isExist = _db.MeterBillGeneration.Where(x => x.Month == month &&
                                                           x.BillFor == readingFor)
                                                    .FirstOrDefault();
                if (isExist != null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "Detail Already Exist! You can only view it",
                        Data = null
                    });
                }

                var current = _db.ReadingDetail.Where(x => !x.IsDeleted == true &&
                                                      x.MeterReading.Month == month &&
                                                      x.MeterReading.ReadingFor == readingFor
                                                      )
                                               .ToList();
                if (current == null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "No data found",
                        Data = null
                    });
                }

                if (!fuelAjustedMonth.IsNullOrEmpty())
                {
                    var previous = _db.ReadingDetail.Where(x => !x.IsDeleted == true &&
                                                         x.MeterReading.Month == fuelAjustedMonth
                                                          )
                                                    .Select(x => new
                                                    {
                                                        x.MeterNo,
                                                        x.UnitsConsumed,
                                                    })
                                                    .ToList();

                    if (current.Count() > 0)
                    {
                        foreach (var item in current)
                        {
                            if (item != null)
                            {
                                item.FuelAdjustedUnits = (int)(previous
                                                                .Where(x => x.MeterNo == item.MeterNo)
                                                                .Select(x => x.UnitsConsumed)
                                                                .SingleOrDefault() != default(decimal) ? previous
                                                                .Where(x => x.MeterNo == item.MeterNo)
                                                                .Select(x => x.UnitsConsumed)
                                                                .FirstOrDefault() : 0);
                            }
                        }
                    }
                }

                if (current.Count() > 0)
                {
                    foreach (var item in current)
                    {
                        if (item != null)
                        {
                            item.SaleTax = GetSaleTax(item.MeterNo);
                        }
                    }
                }

                MeterBillGeneration meterBillGeneration = new MeterBillGeneration();
                List<MeterBillGenerationDetail> meterBillGenerationDetails = new List<MeterBillGenerationDetail>();

                BulkMeterBillGenerationDto bulkMeterBillGenerationDto = new BulkMeterBillGenerationDto();
                List<MeterBillGenerationDTO> meterBillGenerationDTOs = new List<MeterBillGenerationDTO>();

                string SapAccount = null;

                foreach (var item in current)
                {
                    MeterBillGenerationDTO meterBillGenerationDTO = new MeterBillGenerationDTO();
                    meterBillGenerationDTO.LastReading = item.LastReading;
                    meterBillGenerationDTO.CurrentReading = item.CurrentReading;
                    meterBillGenerationDTO.FuelAjustmentUnits = item.FuelAdjustedUnits;
                    meterBillGenerationDTO.FuelAdjustment = (decimal)fuelAdjustment;
                    meterBillGenerationDTO.SaleTax = item.SaleTax;
                    meterBillGenerationDTO.MeterNo = item.MeterNo;
                    meterBillGenerationDTO.UnitsConsumed = item.UnitsConsumed;

                    MeterInstallationStockCreationDTO result = GetPropertyForBillGenrationByMeterNoBulk(item.MeterNo);
                    if (result != null)
                    {
                        meterBillGenerationDTO.PropertyNo = result.PropertyNo;
                        meterBillGenerationDTO.RegistrationNo = result.RegistrationNo;
                        GlobalChargeSetupDetailFilterDTO dto = new GlobalChargeSetupDetailFilterDTO();
                        {
                            dto.RealStateTypeId = Convert.ToInt32(result.RealStateType);
                            dto.ProjectId = Convert.ToInt32(result.Project);
                            dto.PhaseId = Convert.ToInt32(result.Phase);
                            dto.BlockId = Convert.ToInt32(result.Block);
                            dto.CategoryId = Convert.ToInt32(result.Category);
                            dto.PropertyTypeId = Convert.ToInt32(result.Type);
                            dto.NatureId = Convert.ToInt32(result.Nature);
                            dto.GeneratorUnitType = result.GeneratorUnitType;
                        }

                        ChargeResultDTO chargeResultDTO = GetGlobalChargeDetailBulk(dto, 2);

                        if (chargeResultDTO == null)
                        {
                            return Ok(new ApiResponse<object>
                            {
                                Code = ResponseCode.BadRequest,
                                Message = "Registration No  " + result.RegistrationNo + " charge setup issue",
                                Data = null
                            });
                        }

                        if (SapAccount == null)
                        {
                            SapAccount = _commonBLL.GetSapAccountByChargeTypeId(Convert.ToInt32(chargeResultDTO.SapAccount));
                        }
                        meterBillGenerationDTO.PerUnitRate = chargeResultDTO.PerUnitRate;
                        meterBillGenerationDTO.SAPAccount = SapAccount;
                        meterBillGenerationDTO.WTax = GetWTaxBulk(result.ID);

                        // calculation
                        decimal total = (item.UnitsConsumed * chargeResultDTO.PerUnitRate) + (item.FuelAdjustedUnits * (decimal)fuelAdjustment);
                        decimal saleTaxAmount = (decimal)(total * (meterBillGenerationDTO.SaleTax / 100));
                        decimal grossAmount = total + saleTaxAmount;
                        decimal whTaxAmount = grossAmount * (meterBillGenerationDTO.WTax / 100);
                        decimal netAmount = grossAmount;

                        meterBillGenerationDTO.SaleTaxAmount = Math.Round(saleTaxAmount, 2);
                        meterBillGenerationDTO.GrossAmount = Math.Round(grossAmount, 2);
                        meterBillGenerationDTO.WHTaxAmount = Math.Round(whTaxAmount, 2);
                        meterBillGenerationDTO.NetAmount = Math.Round(netAmount, 2);

                        if (item.UnitsConsumed <=150)
                        {
                            meterBillGenerationDTO.NetAmount = GetPhaseWiseBillAddition(item.MeterNo);
                        }

                        meterBillGenerationDTOs.Add(meterBillGenerationDTO);

                    }
                }

                bulkMeterBillGenerationDto.MeterBillGenerationDTO = meterBillGenerationDTOs;
                bulkMeterBillGenerationDto.SumOfAmount = meterBillGenerationDTOs.Sum(x => x.NetAmount);
                bulkMeterBillGenerationDto.SumOfConumedUnits = (decimal)meterBillGenerationDTOs.Sum(x => x.UnitsConsumed);

                if (current.Count() != meterBillGenerationDTOs.Count())
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "There is some issue in property billing setup check please",
                        Data = null
                    });
                }

                // save method
                meterBillGeneration.BillFor = readingFor;
                meterBillGeneration.Month = month;
                meterBillGeneration.ChargesStatus = "Draft";

                foreach (var value in meterBillGenerationDTOs)
                {
                    MeterBillGenerationDetail meterBillGenerationDetail = new MeterBillGenerationDetail();

                    decimal total = (decimal)((value.UnitsConsumed * value.PerUnitRate) + (value.FuelAjustmentUnits * (decimal)fuelAdjustment));


                    meterBillGenerationDetail.PropertyNo = value.PropertyNo;
                    meterBillGenerationDetail.RegistrationNo = value.RegistrationNo;
                    meterBillGenerationDetail.SapAccount = value.SAPAccount;
                    meterBillGenerationDetail.Month = month;
                    meterBillGenerationDetail.MeterNo = value.MeterNo;
                    meterBillGenerationDetail.PreviousReading = value.LastReading.ToString();
                    meterBillGenerationDetail.CurrentReading = value.CurrentReading.ToString();
                    meterBillGenerationDetail.TotalUnitConsumed = (decimal)value.UnitsConsumed;
                    meterBillGenerationDetail.PerUnitRate = (decimal)value.PerUnitRate;
                    meterBillGenerationDetail.FuelAdjustedUnits = (decimal)value.FuelAjustmentUnits;
                    meterBillGenerationDetail.FuelAdjustment = value.FuelAdjustment;
                    meterBillGenerationDetail.Amount = total;
                    meterBillGenerationDetail.SaleTax = (int)value.SaleTax;
                    meterBillGenerationDetail.SaleTaxAmount = value.SaleTaxAmount;
                    meterBillGenerationDetail.WTaxAmount = value.WHTaxAmount;
                    meterBillGenerationDetail.GrossAmount = value.GrossAmount;
                    meterBillGenerationDetail.Discount = value.Discount;
                    meterBillGenerationDetail.NetAmount = value.NetAmount;

                    meterBillGenerationDetails.Add(meterBillGenerationDetail);
                }

                meterBillGeneration.MeterBillGenerationDetail = meterBillGenerationDetails;

                if (meterBillGenerationDetails?.Count > 0)
                {
                    foreach (var item in meterBillGenerationDetails)
                    {
                        var meterDetail = _db.MeterDetail.Where(x => x.MeterNumber == item.MeterNo).FirstOrDefault();

                        if (meterDetail != null)
                        {
                            meterDetail.UnitsAtInstallation = decimal.Parse(item.CurrentReading);
                            _db.Entry(meterDetail).State = EntityState.Modified;
                            _db.SaveChanges();
                        }
                    }
                }


                _db.MeterBillGeneration.Add(meterBillGeneration);
                _db.SaveChanges();

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

        private decimal GetPhaseWiseBillAddition(string meterNo)
        {
            decimal rate = 0;

            int meterPhaseId = (int)_db.MeterDetail.SingleOrDefault(x => x.MeterNumber == meterNo).MeterPhaseId;
            if (meterPhaseId != 0)
            {
                rate = _db.MeterPhaseWiseRates.SingleOrDefault(x => x.MeterPhaseId == meterPhaseId).Rate;
            }
            return rate;
        }

        [HttpGet]
        [Route("GetMeterReadingForBillGenration")]
        public IActionResult GetMeterReadingForBillGenration(string readingFor, string month, string? fuelAjustedMonth)
        {
            try
            {
                var current = _db.ReadingDetail.Where(x => !x.IsDeleted == true &&
                                                     x.MeterReading.Month == month &&
                                                     x.MeterReading.ReadingFor == readingFor
                                                      )
                                               .ToList();

                if (!fuelAjustedMonth.IsNullOrEmpty())
                {
                    var previous = _db.ReadingDetail.Where(x => !x.IsDeleted == true &&
                                                         x.MeterReading.Month == fuelAjustedMonth
                                                          )
                                                   .Select(x => new
                                                   {
                                                       x.MeterNo,
                                                       x.UnitsConsumed,
                                                   })
                                                   .ToList();

                    if (current.Count() > 0)
                    {
                        foreach (var item in current)
                        {
                            if (item != null)
                            {
                                item.FuelAdjustedUnits = (int)(previous
                                                                .Where(x => x.MeterNo == item.MeterNo)
                                                                .Select(x => x.UnitsConsumed)
                                                                .SingleOrDefault() != default(decimal) ? previous
                                                                .Where(x => x.MeterNo == item.MeterNo)
                                                                .Select(x => x.UnitsConsumed)
                                                                .FirstOrDefault() : 0);
                            }
                        }
                    }
                }

                if (current.Count() > 0)
                {
                    foreach (var item in current)
                    {
                        if (item != null)
                        {
                            item.SaleTax = GetSaleTax(item.MeterNo);
                        }
                    }
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = current
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        public int GetSaleTax(string meterNo)
        {
            bool isSaleTaxEnabled = (bool)_db.MeterDetail
                            .Where(x => x.MeterNumber == meterNo)
                            .Select(x => x.MeterInstallation.StockCreation.IsSaleTaxEnabled)
                            .FirstOrDefault();
            return (int)(isSaleTaxEnabled == true ? _db.SaleTax.SingleOrDefault()?.Rate ?? 0 : 0);
        }

        [HttpPost]
        [Route("PrintBill")] // IT Tower
        public IActionResult PrintBill(ListForPrintDTO dto)
        {
            try
            {
                List<BillPrintDTO> billPrintDTO = new List<BillPrintDTO>();

                decimal billSurchargePercentage = _db.SAPOperations.FirstOrDefault().BillDiscountPercentage;

                if (dto == null || !dto.BillList.Any())
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "No bills to print",
                        Data = billPrintDTO
                    });
                }


                foreach (var item in dto.BillList)
                {
                    BillPrintDTO bill = new BillPrintDTO();

                    bill = _db.StockCreations.Where(x => x.RegistrationNo == item.RegistrationNo)
                                                      .Select(x => new BillPrintDTO
                                                      {
                                                          StockId = x.ID,
                                                          RegistrationNo = x.RegistrationNo,
                                                          BillPrintRegistrationNo = x.BillPrintRegistrationNo,
                                                          PropertyNo =string.IsNullOrEmpty(x.BillPrintPropertyNo) ? x.PropertyNo : x.BillPrintPropertyNo,
                                                          BillPrintPropertyNo = x.BillPrintPropertyNo,
                                                          Area = x.ActualSize,
                                                          UOM = x.ActualSizeUnit,
                                                          Size = $"{x.ActualSize} {x.ActualSizeUnit}",
                                                          MemberId = x.MemberProfile.Id,
                                                          MemberName = string.IsNullOrEmpty(x.BillPrintName) ? x.MemberProfile.MemberName : x.BillPrintName,
                                                          Address = string.IsNullOrEmpty(x.BillPrintAddress) ?  x.MemberProfile.PermanentAddress : x.BillPrintAddress,
                                                          MobileNo = x.MemberProfile.Mobile,
                                                          WhatsAppNo = x.MemberProfile.WhatsAppNo,
                                                          DueDate = dto.DueDate,
                                                          BillMonth = dto.BillMonth,
                                                          DocDate = dto.DocDate,
                                                          MaintenceAdvanceBillPaid = x.MaintenceAdvanceBillPaid,
                                                      })
                                                      .FirstOrDefault();

                    bill.ConsumerNo = bill.StockId.ToString();

                    string blockName = GetBlock(bill.RegistrationNo);

                    bill.PropertyNo = string.IsNullOrEmpty(bill.BillPrintPropertyNo) ? $"{blockName}-{bill.PropertyNo}" : bill.BillPrintPropertyNo ;

                    TanantDetail tanantDetail = _db.TanantDetail.Where(t => t.IsActive == true &&
                                                                       t.StockCreationID == bill.StockId
                                                                      )
                                                                .FirstOrDefault();

                    bill.TenantMember = tanantDetail?.Name ?? "N/A";
                    bill.TenantMobileNo = tanantDetail?.Mobile ?? "N/A";

                    bill.FixedChargeBillWHApplied = _db.FixedChargeBillWHApplied
                                                       .Where(x => !x.IsDeleted &&
                                                         x.RegistrationNo == item.RegistrationNo &&
                                                         x.Month == dto.BillMonth)
                                                       .Distinct()
                                                       .ToList();

                    bill.WTaxMapDTOPropertywise = bill.FixedChargeBillWHApplied
                                                      .Where(x => x.Month == dto.BillMonth &&
                                                             x.RegistrationNo == item.RegistrationNo)
                                                      .GroupBy(x => new { x.TaxCode, x.WHPercentage })
                                                      .Select(g => new WTaxMapDTOPropertywise
                                                      {
                                                          RegistrationNo = g.First().RegistrationNo,
                                                          Month = g.First().Month,
                                                          TaxCode = g.Key.TaxCode,
                                                          NetAmount = g.First().NetAmount,
                                                          WHPercentage = g.Key.WHPercentage,
                                                          Amount = g.Sum(x => x.Amount)
                                                      }).ToList();

                    if (dto.BillFor == "All" || dto.BillFor == "Electricity")
                    {
                        bill.MeterBillGenerationDetail = _db.MeterBillGenerationDetail
                                                            .Where(x => !x.IsDeleted &&
                                                              x.MeterNo == item.MeterNo &&
                                                              x.RegistrationNo == item.RegistrationNo &&
                                                              x.MeterBillGeneration.Month == dto.BillMonth)
                                                            .Distinct()
                                                            .ToList();

                        bill.GrandMeterWTaxAmount = bill.MeterBillGenerationDetail.Sum(x => x.WTaxAmount);
                        bill.GrandMeterBillAmount = bill.MeterBillGenerationDetail.Sum(x => x.NetAmount);
                        bill.Arrears = bill.MeterBillGenerationDetail.Sum(x => x.Arrears);
                        ReadingDetailDTO readingDetail = GetMeterReadingDetail(item.MeterNo, dto.BillMonth);
                        bill.MeterPicture = readingDetail.MeterPicture;
                        bill.ReadingDate = readingDetail.ReadingDate;
                        bill.FuelAdjustmentMonth = bill.MeterBillGenerationDetail.FirstOrDefault().ChargeType;
                    }

                    if (dto.BillFor == "All" || dto.BillFor == "Fixed Dues" || dto.BillFor == "Constructed" || dto.BillFor == "Non-Constructed")
                    {
                        bill.FixedChargeBillDetail = _db.FixedChargeBillDetail
                                                     .Where(x => !x.IsDeleted &&
                                                       x.FixedChargeBill.Month == dto.BillMonth &&
                                                       x.FixedChargeBill.RegistrationNo == item.RegistrationNo)
                                                     .Distinct()
                                                     .ToList();

                        bill.GrandFixedWTaxAmount = bill.FixedChargeBillDetail.Sum(x => x.WTaxAmountLine);
                        bill.GrandFixedBillAmount = bill.FixedChargeBillDetail.Sum(x => x.NetAmount);
                        var result = GetFixedArrearsAndAdvanceByRegistrationNo(item.RegistrationNo, dto.BillMonth);

                        bill.Arrears = result.Arrears;
                        bill.Advance = (int)Convert.ToInt64(result.AdvancePayment);
                        bill.MaintenceAdvanceBillPaid = (int)Convert.ToInt64(result.AdvancePayment);
                        bill.PreviousBillDetailDTO = GetFixedHistoryByRegistrationNo(item.RegistrationNo, dto.BillMonth);
                    }

                    //bill.BillBeforePRATax = bill.GrandFixedBillAmount + bill.GrandFixedWTaxAmount + bill.GrandMeterBillAmount + bill.GrandMeterWTaxAmount;
                    bill.BillBeforeDueDate = bill.GrandFixedBillAmount + bill.GrandFixedWTaxAmount + bill.GrandMeterBillAmount + bill.GrandMeterWTaxAmount;
                    bill.SaleTax = dto.BillFor == "Electricity"
                                                  ? bill.MeterBillGenerationDetail?.FirstOrDefault()?.SaleTax ?? 0
                                                  : bill.FixedChargeBillDetail?.FirstOrDefault()?.SaleTax ?? 0;

                    var billMonthDate = DateTime.ParseExact(dto.BillMonth + "-01", "yyyy-MM-dd", null);

                    if (billMonthDate > new DateTime(2025, 5, 1))
                    {
                        bill.SaleTaxAmount = (int)Math.Round(
                            (decimal)((bill.GrandFixedBillAmount + bill.GrandMeterBillAmount) * (bill.SaleTax / 100m))
                        );
                    }
                    else
                    {
                        bill.SaleTaxAmount = 0;
                    }

                    var totalBillAmount = (int)Math.Round(
                        (decimal)(bill.GrandFixedBillAmount + bill.SaleTaxAmount)
                    );

                    int arrears = (int)bill.Arrears;
                    int advance = bill.MaintenceAdvanceBillPaid;
                    int remaingArrears = arrears;

                    if (advance > 0)
                    {
                        if (advance >= arrears)
                        {
                            bill.MaintenceAdvanceBillPaid = advance - arrears;
                            remaingArrears = 0;
                        }
                        else
                        {
                            bill.MaintenceAdvanceBillPaid = arrears - advance;
                            remaingArrears = arrears - advance;
                        }
                    }

                    bill.CurrentBill = totalBillAmount;

                    int surchangeableBill = totalBillAmount;

                    if (bill.MaintenceAdvanceBillPaid > 0)
                    {
                        if (bill.MaintenceAdvanceBillPaid >= totalBillAmount)
                        {
                            bill.MaintenceAdvanceBillPaid -= totalBillAmount;
                            bill.BillBeforeDueDate = 0 + remaingArrears;

                            surchangeableBill = 0;
                        }
                        else
                        {
                            surchangeableBill = totalBillAmount - bill.MaintenceAdvanceBillPaid;
                            bill.BillBeforeDueDate = totalBillAmount - bill.MaintenceAdvanceBillPaid + remaingArrears;
                            bill.MaintenceAdvanceBillPaid = 0;
                        }
                    }
                    else
                    {
                        surchangeableBill = totalBillAmount;
                        bill.BillBeforeDueDate = totalBillAmount + remaingArrears;
                    }


                    bill.SurchargeAfterDueDate = (int)(surchangeableBill * billSurchargePercentage / 100);
                    bill.BillAfterDueDate = bill.BillBeforeDueDate + bill.SurchargeAfterDueDate;
                    

                    bill.MaintenceAdvanceBillPaid =bill.MaintenceAdvanceBillPaid * (-1);

                    bill.Remarks = dto.Remarks;
                    if (bill != null)
                    {
                        string refNo = GetUniqueRefNo(dto.BillMonth, dto.BillFor, bill.RegistrationNo, bill.MeterBillGenerationDetail.Any() ? bill.MeterBillGenerationDetail.FirstOrDefault().MeterNo : null);
                        //bill.Arrears = GetArrears(dto.BillMonth, dto.BillFor, bill.RegistrationNo, bill.MeterBillGenerationDetail.Any() ? bill.MeterBillGenerationDetail.FirstOrDefault().MeterNo : null);
                        bill.QrString = "qwertystring";//QRGenerator.GenerateQRUrl(dto.BillFor == "Constructed" ? (bill.BillBeforeDueDate - (bill.BillBeforeDueDate * mobileAppDisount / 100)).ToString() : bill.BillBeforeDueDate.ToString(), bill.DueDate.ToString(), refNo, bill.MemberId.ToString());
                        billPrintDTO.Add(bill);

                    }
                    bill.BillMonth = DateTime.Parse(bill.BillMonth).ToString("MMM-yyyy");
                    bill.RegistrationNo = string.IsNullOrEmpty(bill.BillPrintRegistrationNo) ? bill.RegistrationNo : bill.BillPrintRegistrationNo;
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = billPrintDTO
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        private ReadingDetailDTO GetMeterReadingDetail(string? meterNo, string billMonth)
        {
            var result = _db.ReadingDetail.Where(x => x.IsActive == true
                                                  && x.MeterNo == meterNo
                                               )
                                         .Include(x => x.MeterReading)
                                         .Select(x => new ReadingDetailDTO
                                         {
                                             MeterPicture = x.Picture,
                                             FuelAdjustmentMonth = x.MeterReading.Month,
                                             ReadingDate = (DateTime)x.ReadingDate
                                         })
                                         .FirstOrDefault();

            return result;
        }

        private string GetBlock(string? registrationNo)
        {
            string blockId = _db.StockCreations.FirstOrDefault(x => x.RegistrationNo == registrationNo).Block;

            return _db.Blocks.FirstOrDefault(x => x.ID == Convert.ToInt32(blockId)).Description ?? "N/A";
        }

        private int? GetArrears(string? billMonth, string? billFor, string? registrationNo, string? meterNo = null)
        {
      //      if (string.IsNullOrEmpty(registrationNo))
      //          return null;

      //      if (billFor == "Electricity")
      //          return null;

      //      var request = new QueryRequest
      //      {
      //          QueryId = billFor == "Constructed" ? _db.DynamicQueries.Where(x => x.LockedId == "MonthlyArrears_Locked").FirstOrDefault().Id : _db.DynamicQueries.Where(x => x.LockedId == "SC/NCArrears_Locked").FirstOrDefault().Id,
      //          Parameters = new Dictionary<string, string>
      //{
      //    { "Project", registrationNo }
      //}
      //      };

      //      var actionResult = new SapIntegrationController(_db).GenerateDynamicReport(request) as OkObjectResult;
      //      if (actionResult?.Value is ApiResponse<object> response && response.Code == ResponseCode.Success)
      //      {
      //          var list = response.Data as List<Dictionary<string, object>>;
      //          if (list != null && list.Count > 0 && list[0].ContainsKey("Arrears"))
      //          {
      //              var val = list[0]["Arrears"];
      //              if (val != null)
      //                  return Convert.ToInt32(Convert.ToDecimal(val));
      //          }
      //      }

            return 0;
        }

        private string? GetUniqueRefNo(string? billMonth, string? billFor, string? registrationNo, string? meterNo = null)
        {
            return _db.BillingServiceTable
                 .Include(x => x.BillingServiceDetailsTable)
                 .Where(x =>
                     x.BillFor == billFor &&
                     x.BillMonth == billMonth &&
                     x.RegistrationNo == registrationNo &&
                     (string.IsNullOrEmpty(meterNo) ||
                      x.BillingServiceDetailsTable.Any(d => d.MeterNo == meterNo))
                 )
                 .Select(x => x.UniqueReferenceNo)
                 .FirstOrDefault();
        }

        private (decimal Arrears, decimal AdvancePayment) GetFixedArrearsAndAdvanceByRegistrationNo(string registrationNo, string billMonth)
        {
            decimal arrears = 0;
            decimal advancePayment = 0;
#if !SAP_INTEGRATION
            // SAP not compiled in - same result the catch block below produces when SAP is
            // unreachable. Build with /p:SapIntegration=true to restore. See HRMS_Web.csproj.
            return (0, 0);
#else
            SAPOperationDb sap = new SAPOperationDb(_db);
            sap.ConnectToCompany();
            try
            {
                if (sap._a != 0)
                    return (0, 0);

                string query = $@"
    SELECT
        SUM(""Arrears"") AS ""Arrears"",
        SUM(""Advance Payment"") AS ""Advance Payment""
    FROM
    (
        SELECT
            IFNULL(SUM(A.""DocTotal"" - A.""PaidToDate""), 0) AS ""Arrears"",
            0 AS ""Advance Payment""
        FROM ""DHA_LIVE"".""OINV"" A
        WHERE
            A.""CANCELED"" = 'N'
            AND A.""Project"" = '{registrationNo}'
            AND IFNULL(A.""U_BillReferenceNo"", '') <> ''
            AND A.""U_BillMnth"" < '{billMonth}'
            AND (A.""DocTotal"" - A.""PaidToDate"") > 0
        UNION ALL
        SELECT
            0 AS ""Arrears"",
            IFNULL(SUM(RC.""OpenBal""), 0) AS ""Advance Payment""
        FROM ""DHA_LIVE"".""ORCT"" RC
        WHERE
            RC.""PayNoDoc"" = 'Y'
            AND RC.""Canceled"" = 'N'
            AND IFNULL(RC.""U_Source"", '') <> ''
            AND RC.""OpenBal"" > 0
            AND RC.""CardCode"" = '{registrationNo}'
    ) X";

                SAPbobsCOM.Recordset rs =
                    (SAPbobsCOM.Recordset)sap.Ocomp.GetBusinessObject(
                        SAPbobsCOM.BoObjectTypes.BoRecordset);
                rs.DoQuery(query);
                if (!rs.EoF)
                {
                    arrears = Convert.ToDecimal(
                        rs.Fields.Item("Arrears").Value ?? 0);
                    advancePayment = Convert.ToDecimal(
                        rs.Fields.Item("Advance Payment").Value ?? 0);
                }
            }
            catch
            {
                arrears = 0;
                advancePayment = 0;
            }
            finally
            {
                if (sap.Ocomp != null && sap.Ocomp.Connected)
                    sap.Ocomp.Disconnect();
            }
            return (arrears, advancePayment);
#endif
        }

        private List<PreviousBillDetailDTO> GetFixedHistoryByRegistrationNo(string registrationNo, string billMonth)
        {
#if !SAP_INTEGRATION
            // SAP not compiled in - same result the catch block below produces when SAP is
            // unreachable. Build with /p:SapIntegration=true to restore. See HRMS_Web.csproj.
            return new List<PreviousBillDetailDTO>();
#else
            SAPOperationDb sap = new SAPOperationDb(_db);
            sap.ConnectToCompany();

            try
            {


                string query = $@"
SELECT
    h.""U_BillMnth"",
    SUM(h.""DocTotal"") AS ""Amount"",
    SUM(h.""DocTotal"" - h.""PaidToDate"") AS ""Pending""
FROM OINV h
WHERE h.""CANCELED"" = 'N'
  AND h.""Project"" = '{registrationNo}'
  AND IFNULL(h.""U_BillReferenceNo"", '') <> ''
  AND h.""U_BillMnth"" <> '{billMonth}'
GROUP BY h.""U_BillMnth""
ORDER BY h.""U_BillMnth"" DESC
LIMIT 6;
";

                SAPbobsCOM.Recordset rs =
                    (SAPbobsCOM.Recordset)sap.Ocomp.GetBusinessObject(
                        SAPbobsCOM.BoObjectTypes.BoRecordset);

                rs.DoQuery(query);

                var previousBills = new List<PreviousBillDetailDTO>();

                while (!rs.EoF)
                {
                    decimal pending = Convert.ToDecimal(rs.Fields.Item("Pending").Value);

                    previousBills.Add(new PreviousBillDetailDTO
                    {
                        Month = DateTime.Parse(rs.Fields.Item("U_BillMnth").Value.ToString())
                                        .ToString("MMM-yyyy"),

                        TotalAmount = Convert.ToInt32(rs.Fields.Item("Amount").Value),

                        PendingAmount = (int?)pending,

                        Status = pending <= 0 ? "Paid" : "Unpaid"
                    });

                    rs.MoveNext();
                }

                return previousBills;
            }
            catch
            {
                return new List<PreviousBillDetailDTO>();
            }
            finally
            {
                if (sap.Ocomp != null && sap.Ocomp.Connected)
                    sap.Ocomp.Disconnect();
            }

            return new List<PreviousBillDetailDTO>();
#endif
        }


        [HttpGet]
        [Route("GetDetailsFromMeterBillGeneration")]
        public IActionResult GetDetailsFromMeterBillGeneration(string readingFor, string month, string regno)
        {
            try
            {
                var result = _db.MeterBillGeneration.Where(x => x.IsActive == true
                                                      && x.BillFor == readingFor
                                                      && x.Month == month)
                                             .Include(x => x.MeterBillGenerationDetail.Where(x => x.RegistrationNo == regno))
                                             .Distinct()
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
        [Route("GetNextChallanNumberAsync")]
        public async Task<IActionResult> GetNextChallanNumberAsync()
        {
            try
            {
                string challanNo = await _commonBLL.GetNextChallanNumberAsync("CHALLAN");

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = challanNo
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetNextChallanNumberAsyncPrintOnly")]
        public async Task<IActionResult> GetNextChallanNumberAsyncPrintOnly()
        {
            try
            {
                string challanNo = await _commonBLL.GetNextChallanNumberAsync("PRINT");

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = challanNo
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetDetailsFromFixedChargeBillGeneration")]
        public IActionResult GetDetailsFromFixedChargeBillGeneration(string readingFor, string month, string regno)
        {
            try
            {
                var result = _db.FixedChargeBill.Where(x => x.IsActive == true
                                                      && x.BillFor == readingFor
                                                      && x.Month == month
                                                      && x.RegistrationNo == regno)
                                             .Include(x => x.FixedChargeBillDetail)
                                             .Distinct()
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
        [Route("GetDetailsFromMonthlyBillGeneration")]
        public IActionResult GetDetailsFromMonthlyBillGeneration(string readingFor, string month)
        {
            try
            {
                var result = _db.MeterBillGenerationDetail.Where(x => !x.IsDeleted == true
                                                      && x.MeterBillGeneration.BillFor == readingFor
                                                      && x.MeterBillGeneration.Month == month)
                                             .GroupBy(x => x.RegistrationNo)
                                             .Select(g => new
                                             {
                                                 RegistrationNo = g.First().RegistrationNo,
                                                 PropertyNo = g.First().PropertyNo,
                                                 NetAmount = g.Sum(x => x.NetAmount)
                                             })
                                             .Distinct()
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
        [Route("GetCombineBillFromMonthlyBillGeneration")]
        public IActionResult GetCombineBillFromMonthlyBillGeneration(string readingFor, string month)
        {
            try
            {
                var fixedBills = _db.FixedChargeBillDetail
                                       .Where(x => !x.IsDeleted && x.FixedChargeBill.Month == month)
                                       .Select(x => new AllBillsDTO
                                       {
                                           RegistrationNo = x.FixedChargeBill.StockCreation.RegistrationNo,
                                           PropertyNo = x.FixedChargeBill.StockCreation.PropertyNo,
                                           //NetAmount = x.NetAmount + x.WTaxAmountLine,
                                           NetAmount = x.NetAmount
                                       });
                //.Distinct();

                var meterBills = _db.MeterBillGenerationDetail
                                        .Where(x => !x.IsDeleted && x.MeterBillGeneration.Month == month)
                                        .GroupBy(x => x.RegistrationNo)
                                        .Select(g => new AllBillsDTO
                                        {
                                            RegistrationNo = g.First().RegistrationNo,
                                            PropertyNo = g.First().PropertyNo,
                                            NetAmount = g.Sum(x => x.NetAmount)
                                        })
                                        .Distinct();

                var allBills = fixedBills.Concat(meterBills)
                    .GroupBy(x => x.RegistrationNo)
                    .Select(g => new AllBillsDTO
                    {
                        RegistrationNo = g.Key,
                        PropertyNo = g.First().PropertyNo,
                        NetAmount = g.Sum(x => x.NetAmount)
                    });

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = allBills
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetDetailsFixedDuesFromMonthlyBillGeneration")]
        public IActionResult GetDetailsFixedDuesFromMonthlyBillGeneration(string readingFor, string month)
        {
            try
            {
                var result = _db.FixedChargeBillDetail.Where(x => !x.IsDeleted == true
                                                             && x.FixedChargeBill.Month == month
                                                             && x.FixedChargeBill.BillFor == readingFor)
                                                    .Select(x => new
                                                    {
                                                        RegistrationNo = x.FixedChargeBill.StockCreation.RegistrationNo,
                                                        PropertyNo = x.FixedChargeBill.StockCreation.PropertyNo,
                                                        // NetAmount = x.NetAmount + x.WTaxAmountLine
                                                        NetAmount = x.NetAmount
                                                    })
                                                    //.Distinct()
                                                    .GroupBy(x => x.RegistrationNo)
                                                    .Select(g => new
                                                    {
                                                        RegistrationNo = g.Key,
                                                        PropertyNo = g.First().PropertyNo,
                                                        NetAmount = g.Sum(x => x.NetAmount)
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

        // MeterBillGenerationInBulk
        public MeterInstallationStockCreationDTO GetPropertyForBillGenrationByMeterNoBulk(string meterNo)
        {
            var result = _db.MeterDetail.Where(x => x.IsActive == true
                                                  && x.MeterNumber == meterNo
                                               )
                                         .Include(x => x.MeterInstallation.StockCreation)
                                         .Select(x => new MeterInstallationStockCreationDTO
                                         {
                                             ID = x.MeterInstallation.StockCreation.ID,
                                             RegistrationNo = x.MeterInstallation.StockCreation.RegistrationNo,
                                             PropertyNo = x.MeterInstallation.StockCreation.PropertyNo,
                                             RealStateType = x.MeterInstallation.StockCreation.RealStateType,
                                             Project = x.MeterInstallation.StockCreation.Project,
                                             Phase = x.MeterInstallation.StockCreation.Phase,
                                             Block = x.MeterInstallation.StockCreation.Block,
                                             Category = x.MeterInstallation.StockCreation.Category,
                                             Type = x.MeterInstallation.StockCreation.Type,
                                             Nature = x.MeterInstallation.StockCreation.Nature,
                                             GeneratorUnitType = x.MeterInstallation.StockCreation.GeneratorUnitType,
                                         })
                                         .FirstOrDefault();

            return result;
        }

        [HttpGet]
        [Route("GetPropertyForBillGenrationByMeterNo")]
        public IActionResult GetPropertyForBillGenrationByMeterNo(string meterNo)
        {
            try
            {
                var result = _db.MeterDetail.Where(x => x.IsActive == true
                                                      && x.MeterNumber == meterNo
                                                   )
                                             .Include(x => x.MeterInstallation.StockCreation)
                                             .Select(x => new MeterInstallationStockCreationDTO
                                             {
                                                 ID = x.MeterInstallation.StockCreation.ID,
                                                 RegistrationNo = x.MeterInstallation.StockCreation.RegistrationNo,
                                                 PropertyNo = x.MeterInstallation.StockCreation.PropertyNo,
                                                 RealStateType = x.MeterInstallation.StockCreation.RealStateType,
                                                 Project = x.MeterInstallation.StockCreation.Project,
                                                 Phase = x.MeterInstallation.StockCreation.Phase,
                                                 Block = x.MeterInstallation.StockCreation.Block,
                                                 Category = x.MeterInstallation.StockCreation.Category,
                                                 Type = x.MeterInstallation.StockCreation.Type,
                                                 Nature = x.MeterInstallation.StockCreation.Nature,
                                                 GeneratorUnitType = x.MeterInstallation.StockCreation.GeneratorUnitType,
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
        [Route("GetBookingPlanFromPreSale")]
        public IActionResult GetBookingPlanFromPreSale(int id)
        {
            try
            {
                var result = _db.PaymentPlan.Where(x => !x.IsDeleted &&
                                                   x.PreSale.StockCreationId == id)
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
        [Route("GetAllPMSUSERS")]
        public IActionResult GetAllPMSUSERS()
        {
            try
            {
                var result = _db.PMSUser.Select(x => new { x.Id, x.EMP_FULL_NAME }).ToList();

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
        [Route("GetAllWTaxPropertWise")]
        public IActionResult GetAllWTaxPropertWise(int stockId)
        {
            try
            {
                var result = _db.WithHoldingTaxPropertyWise.Where(x => x.StockCreationId == stockId)
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
        [Route("GetAllFixedChargesPropertWise")]
        public IActionResult GetAllFixedChargesPropertWise(int stockId)
        {
            try
            {
                var result = _db.PropertyFixedChargesSetup.Where(x => x.StockCreationId == stockId)
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

        // MeterBillGenerationInBulk
        public int GetWTaxBulk(int id)
        {
            int sumOfRates = 0;

            var wTax = _db.WithHoldingTaxPropertyWise
                         .Where(x => x.StockCreationId == id && x.IsEnabled == true)
                         .Select(x => x.Rate)
                         .ToList();

            if (wTax.Count() > 0)
            {
                sumOfRates = (int)wTax.Sum(); // convert the sum to an integer
            }

            return sumOfRates;
        }

        [HttpGet]
        [Route("GetWTax")]
        public IActionResult GetWTax(int id)
        {
            try
            {
                int sumOfRates = 0;

                var wTax = _db.WithHoldingTaxPropertyWise
                             .Where(x => x.StockCreationId == id && x.IsEnabled == true)
                             .Select(x => x.Rate)
                             .ToList();

                if (wTax.Count() > 0)
                {
                    sumOfRates = (int)wTax.Sum(); // convert the sum to an integer
                }


                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = sumOfRates
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }


        [HttpGet]
        [Route("GetAllFixedCharges")]
        public IActionResult GetAllFixedCharges(int stockId)
        {
            try
            {
                var property = _db.StockCreations.Find(stockId);

                GlobalChargeSetupDetailFixedChargFilterDTO dto = new GlobalChargeSetupDetailFixedChargFilterDTO()
                {
                    FormId = 5,
                    RealStateTypeId = Convert.ToInt32(property.RealStateType),
                    ProjectId = Convert.ToInt32(property.Project),
                    PhaseId = Convert.ToInt32(property.Phase),
                    BlockId = Convert.ToInt32(property.Block),
                    CategoryId = Convert.ToInt32(property.Category),
                    PropertyTypeId = Convert.ToInt32(property.Type),
                    NatureId = Convert.ToInt32(property.Nature),
                    PossessionStatus = property.PossessionStatus,
                    ConstructionStatus = property.ConstracutionStatus,
                    GracePeriod = property.GrancePeriodForBillGenration < DateTime.Now.Date ? false : true,
                };

                int Id = _db.FormsChargeGroup.SingleOrDefault(x => x.FormId == 5).ChargeGroupId;

                if (Id != 0)
                {
                    var result = _db.GlobalChargeDetail.Where(x => !x.IsDeleted == true
                                                             && x.GlobalChargeSetup.GlobalChargeGroupId == Id
                                                             && (x.GlobalChargeSetup.RealStateTypeId == dto.RealStateTypeId || x.GlobalChargeSetup.RealStateTypeId == null)
                                                             && (x.GlobalChargeSetup.ProjectId == dto.ProjectId || x.GlobalChargeSetup.ProjectId == null)
                                                             && (x.GlobalChargeSetup.PhaseId == dto.PhaseId || x.GlobalChargeSetup.PhaseId == null || x.GlobalChargeSetup.PhaseId == -1)
                                                             && (x.GlobalChargeSetup.BlockId == dto.BlockId || x.GlobalChargeSetup.BlockId == null)
                                                             && (x.GlobalChargeSetup.CategoryId == dto.CategoryId || x.GlobalChargeSetup.CategoryId == null)
                                                             && (x.GlobalChargeSetup.PropertyTypeId == dto.PropertyTypeId || x.GlobalChargeSetup.PropertyTypeId == null)
                                                             && (x.GlobalChargeSetup.NatureId == dto.NatureId || x.GlobalChargeSetup.NatureId == null)
                                                             && x.GlobalChargeSetup.PossessionStatus == dto.PossessionStatus
                                                             && (x.GlobalChargeSetup.ConstructionStatus == dto.ConstructionStatus || x.GlobalChargeSetup.ConstructionStatus == null)
                                                             && (x.GlobalChargeSetup.GracePeriod == dto.GracePeriod)
                                                          )
                                                   .Distinct()
                                                   .Select(x => new
                                                   {
                                                       Id = x.Id,
                                                       ChargeType = x.Description,
                                                       ChargeDes = x.GlobalChargeSetup.Description,
                                                       Rate = x.Rate,
                                                       GlobalChargeSetupId = x.GlobalChargeSetupId
                                                   })
                                                   .ToList();
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
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
        [Route("GetTanantDetail")]
        public IActionResult GetTanantDetail(int stockId)
        {
            try
            {
                var result = _db.TanantDetail.Where(x => !x.IsDeleted &&
                                                    x.StockCreationID == stockId
                                                    )
                                              .ToList()
                                              .OrderByDescending(x => x.Id);

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

        // MeterBillGenerationInBulk
        public ChargeResultDTO GetGlobalChargeDetailBulk(GlobalChargeSetupDetailFilterDTO dto, int formId)
        {
            ChargeResultDTO chargeResultDTO = new ChargeResultDTO();

            chargeResultDTO = _db.GlobalChargeSetup.Where(x => !x.IsDeleted
                               && x.GlobalChargeGroupId == formId
                               && x.RealStateTypeId == dto.RealStateTypeId
                               && x.ProjectId == dto.ProjectId
                               && x.PhaseId == dto.PhaseId
                               && x.BlockId == dto.BlockId
                               && x.CategoryId == dto.CategoryId
                               && x.PropertyTypeId == dto.PropertyTypeId
                               && x.NatureId == dto.NatureId
                               && x.GeneratorUnitType == dto.GeneratorUnitType
                               )
                           .Include(x => x.GlobalChargeDetail.Where(x => !x.IsDeleted))
                           .Select(x => new ChargeResultDTO
                           {
                               PerUnitRate = (decimal)x.GlobalChargeDetail.FirstOrDefault().Rate,
                               SapAccount = x.GlobalChargeDetail.FirstOrDefault().ChargeType,
                           })
                           .FirstOrDefault() ?? new ChargeResultDTO();

            return chargeResultDTO;
        }

        [HttpPost]
        [Route("GetGlobalChargeDetail")]
        public IActionResult GetGlobalChargeDetail(GlobalChargeSetupDetailFilterDTO dto, int formId)
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

                var globalChargeGroupIds = _db.FormsChargeGroup.Where(x => x.FormId == formId && !x.IsDeleted).Select(x => x.ChargeGroupId).ToList();

                var globalChargeSetupDetails = new List<GlobalChargeDetail>();
                var globalChargeSetupsDetail = new List<GlobalChargeSetup>();


                if (globalChargeGroupIds.Count() > 0 || globalChargeGroupIds is not null)
                {
                    foreach (var item in globalChargeGroupIds)
                    {
                        globalChargeSetupsDetail = _db.GlobalChargeSetup.Where(x => !x.IsDeleted
                                                               && x.GlobalChargeGroupId == item
                                                               && x.RealStateTypeId == dto.RealStateTypeId
                                                               && x.ProjectId == dto.ProjectId
                                                               && x.PhaseId == dto.PhaseId
                                                               && x.BlockId == dto.BlockId
                                                               && x.CategoryId == dto.CategoryId
                                                               && x.PropertyTypeId == dto.PropertyTypeId
                                                               && x.NatureId == dto.NatureId
                                                               && x.GeneratorUnitType == dto.GeneratorUnitType
                                                               )
                                                           .Include(x => x.GlobalChargeDetail.Where(x => !x.IsDeleted))
                                                           .ToList();

                        if (globalChargeSetupsDetail != null)
                        {
                            foreach (var item2 in globalChargeSetupsDetail)
                            {
                                foreach (var detail in item2.GlobalChargeDetail)
                                {
                                    detail.SapAccount = _commonBLL.GetSapAccountByChargeTypeId(Convert.ToInt32(detail.ChargeType));
                                    detail.GlobalChargeSetup = null;
                                    globalChargeSetupDetails.Add(detail);
                                }

                            }
                        }

                    }
                }
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = new { globalChargeDetail = globalChargeSetupDetails.Distinct() }
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetFixedChargeDetail")]
        public IActionResult GetFixedChargeDetail(int formId)
        {
            try
            {

                var result = _db.GlobalChargeDetail.Where(x => !x.IsDeleted
                                                       && x.GlobalChargeSetup.GlobalChargeGroupId == formId
                                                       )
                                                   .Include(x => x.GlobalChargeSetup)
                                                   .Select(x => new
                                                   {
                                                       x.GlobalChargeSetup.GlobalChargeGroup.ChargeGroupName,
                                                       x.GlobalChargeSetup.Description,
                                                       x.Rate,

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
        [Route("GetFilterMemberList")]
        public IActionResult GetFilterMemberList()
        {
            try
            {
                var result = _db.MemberProfile.Where(x => !x.IsDeleted)
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
        [AllowAnonymous]
        [Route("GetFilterMemberListById")]
        public IActionResult GetFilterMemberListById(int id)
        {
            try
            {
                var result = _db.MemberProfile.Where(x => !x.IsDeleted && x.Id == id)
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
        [Route("GetJointMembersByStockId")]
        public IActionResult GetJointMembersByStockId(int stockId)
        {
            try
            {
                bool IsExistInTransfer = _db.TransferHistery.Any(x => x.StockCreationId == stockId && x.Remarks != "Hosted Ownery");
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
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "Success",
                            Data = JointMembersTransfer
                        });
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
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = JointMembersBooking
                    });
                }

                var historicaldata = _db.JointMemberHistoricalData.Any(x => x.StockCreationId == stockId);

                if (historicaldata)
                {
                    var historicaljointMember = _db.JointMemberHistoricalData.Where(x => x.StockCreationId == stockId)
                                                                     .Select(x => new JointMemberDto
                                                                     {
                                                                         Name = x.Name,
                                                                         Cnic = x.CNIC,
                                                                         Mobile = x.Mobile
                                                                     })
                                                                     .ToList();
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = historicaljointMember
                    });
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
        [Route("GetAllProperiesForIndiviualBillGenration")]
        public IActionResult GetAllProperiesForIndiviualBillGenration()
        {
            try
            {
                var result = _db.StockCreations.Where(x => !x.is_deleted &&
                                                            x.MemberProfile != null &&
                                                            x.PropertyNo != null &&
                                                            x.PropertyNo != "" &&
                                                            x.RegistrationNo != null &&
                                                            x.RegistrationNo != ""
                                                            )
                                              .Select(x => new
                                              {
                                                  x.ID,
                                                  x.RegistrationNo,
                                                  x.PropertyNo,
                                                  x.MemberProfile.MemberName,
                                                  MemberCode = x.MemberProfile.Id
                                              })
                                              .Distinct()
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
        [Route("GetFilteredPropertyBinding")]
        public IActionResult GetFilteredPropertyBinding(int key)
        {
            try
            {
                if (key == 1)
                {
                    var result = _db.StockCreations.Where(x => !x.is_deleted
                                                    && x.PropertyNo != null
                                                    && (x.RegistrationNo == null || x.RegistrationNo == ""))
                                                   .Select(x => new
                                                   {
                                                       id = x.ID,
                                                       propertyNo = x.PropertyNo ?? "",
                                                       registrationNo = x.RegistrationNo ?? ""
                                                   })
                                                   .ToList();
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
                }

                if (key == 2)
                {
                    var result = _db.StockCreations.Where(x => !x.is_deleted
                                                    && x.RegistrationNo != null
                                                    && (x.PropertyNo == null || x.PropertyNo == ""))
                                                   .Select(x => new
                                                   {
                                                       id = x.ID,
                                                       propertyNo = x.PropertyNo ?? "",
                                                       registrationNo = x.RegistrationNo ?? ""
                                                   })
                                                   .ToList();
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
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
        [HttpGet]
        [Route("GetFilteredPropertyBindingForSearch")]
        public IActionResult GetFilteredPropertyBindingForSearch(int key)
        {
            try
            {
                if (key == 1)
                {
                    var result = _db.StockCreations.Where(x => !x.is_deleted
                                                    && x.PropertyNo != null
                                                    && (x.RegistrationNo == null || x.RegistrationNo == ""))

                                                   .ToList();
                    if (result?.Count > 0)
                    {
                        foreach (var block in result)
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

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
                }

                if (key == 2)
                {
                    var result = _db.StockCreations.Where(x => !x.is_deleted
                                                    && x.RegistrationNo != null
                                                    && (x.PropertyNo == null || x.PropertyNo == ""))
                                                   .ToList();
                    if (result?.Count > 0)
                    {
                        foreach (var block in result)
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
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
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

        [HttpGet]
        [Route("GetNDC1CheckList")]
        public IActionResult GetNDC1CheckList(int id)
        {
            try
            {
                var result = _db.TestApproval.Where(x => !x.IsDeleted && x.RequestId == id &&
                                                    x.ApprovalUIId == (int)ApprovalUIIds.NDCRequestForMember)
                                              .ToList();
                if (result.Count() > 0)
                {
                    foreach (var item in result)
                    {
                        item.UserDesignation = _commonBLL.GetDepartmentFromUserId(item.UserId);
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
        [Route("GetFileVerificationNDC1CheckList")]
        public IActionResult GetFileVerificationNDC1CheckList(int id)
        {
            try
            {
                var result = _db.TestApproval.Where(x => !x.IsDeleted && x.RequestId == id && x.ApprovalUIId == (int)ApprovalUIIds.FileVerification)
                                              .ToList();
                if (result.Count() > 0)
                {
                    foreach (var item in result)
                    {
                        item.UserDesignation = _commonBLL.GetDepartmentFromUserId(item.UserId);
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
        [Route("GetAllPostedUnpostedInvoices")]
        public IActionResult GetAllPostedUnpostedInvoices(string BillFor)
        {
            try
            {
                var result = _db.SAPBillPostingCheck.Where(x => x.Month == BillFor).ToList();
                if (result.Count > 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "No Record Found",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/api/Filter/GetFilterListForApprovalHistoryById")]
        public IActionResult GetFilterListForApprovalHistoryById(int id, int approvalUiId)
        {
            try
            {
                var result = _db.TestApproval.Where(x => x.RequestId == id && x.ApprovalUIId == approvalUiId
                                                )
                                        .Select(x => new
                                        {
                                            x.UserDesignation,
                                            x.AssignedDateTime,
                                            x.ActionDateTime,
                                            x.ApprovalStatus,
                                            x.LastActionComment
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
        [AllowAnonymous]
        [Route("/api/Filter/GetFilterAlerts")]
        public IActionResult GetFilterAlerts(int id)
        {
            try
            {
                var result = _db.Alerts.Where(x => x.RegistrationNoProfile.StockCreationId == id &&
                                                 x.Status == "Active" &&
                                                 x.FromDate <= DateTime.Now.Date.AddDays(-1) &&
                                                 x.ToDate >= DateTime.Now.Date)

                                        .Select(x => new
                                        {
                                            x.AlertName,
                                            x.AlertNarration
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
        [AllowAnonymous]
        [Route("UpdateAllFixedChargesByDate")]
        public IActionResult UpdateAllFixedChargesByDate(string gracePeriodDate, string lastModifiedUserName)
        {
            try
            {
                DateTime gracedate = Convert.ToDateTime(gracePeriodDate);

                var properties = _db.StockCreations.Where(x => x.MemberProfileId != null &&
                                                          x.PossessionStatus == true &&
                                                          x.PropertyNo != null &&
                                                          x.PropertyNo != "" &&
                                                          x.RegistrationNo != null &&
                                                          x.RegistrationNo != ""
                                                          )
                                                   .ToList();
                foreach (var item in properties)
                {

                    GlobalChargeSetupDetailFixedChargFilterDTO model = new GlobalChargeSetupDetailFixedChargFilterDTO()
                    {
                        FormId = 5,
                        RealStateTypeId = Convert.ToInt32(item.RealStateType),
                        ProjectId = Convert.ToInt32(item.Project),
                        PhaseId = Convert.ToInt32(item.Phase),
                        BlockId = Convert.ToInt32(item.Block),
                        CategoryId = Convert.ToInt32(item.Category),
                        PropertyTypeId = Convert.ToInt32(item.Type),
                        NatureId = Convert.ToInt32(item.Nature),
                        PossessionStatus = item.PossessionStatus,
                        ConstructionStatus = item.ConstracutionStatus,
                        GracePeriod = item.GrancePeriodForBillGenration < gracedate.Date ? false : true,
                    };

                    List<UpdateFixedChargeDTO> currentSetup = GetGlobalChargeDetail(model);

                    if (currentSetup.Count() > 0)
                    {
                        var existingchargeSetup = _db.PropertyFixedChargesSetup.Where(x => x.StockCreationId == item.ID).FirstOrDefault();

                        if (existingchargeSetup != null)
                        {
                            if (existingchargeSetup.GlobalChargeSetupId != currentSetup.FirstOrDefault().GlobalChargeSetupId)
                            {
                                var removeExistingSetup = _db.PropertyFixedChargesSetup.Where(x => x.StockCreationId == item.ID).ToList();
                                if (removeExistingSetup.Count() > 0)
                                {
                                    _db.PropertyFixedChargesSetup.RemoveRange(removeExistingSetup);
                                    _db.SaveChanges();
                                }

                                List<PropertyFixedChargesSetup> propertyFixedChargesSetups = new List<PropertyFixedChargesSetup>();
                                foreach (var charge in currentSetup)
                                {
                                    PropertyFixedChargesSetup propertyFixedChargesSetup = new PropertyFixedChargesSetup();

                                    propertyFixedChargesSetup.MatchId = charge.Id;
                                    propertyFixedChargesSetup.RegistrationNo = item.RegistrationNo;
                                    propertyFixedChargesSetup.PropertyNo = item.PropertyNo;
                                    propertyFixedChargesSetup.ChargeType = charge.ChargeType;
                                    propertyFixedChargesSetup.Unit = 1;
                                    propertyFixedChargesSetup.ChargeSetupRate = charge.Rate;
                                    propertyFixedChargesSetup.Rate = charge.Rate;
                                    propertyFixedChargesSetup.Discount = 0;
                                    propertyFixedChargesSetup.ChargeDes = charge.ChargeDes;
                                    propertyFixedChargesSetup.IsEnabled = true;
                                    propertyFixedChargesSetup.GlobalChargeSetupId = charge.GlobalChargeSetupId;
                                    propertyFixedChargesSetup.StockCreationId = item.ID;
                                    propertyFixedChargesSetup.CreatedOn = DateTime.Now;
                                    propertyFixedChargesSetup.LastModified = DateTime.Now;
                                    propertyFixedChargesSetup.LastModifiedUserName = lastModifiedUserName;

                                    propertyFixedChargesSetups.Add(propertyFixedChargesSetup);
                                }

                                _db.PropertyFixedChargesSetup.AddRange(propertyFixedChargesSetups);
                                _db.SaveChanges();
                            }
                        }
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
        [AllowAnonymous]
        [Route("UpdateAllFixedCharges")]
        public IActionResult UpdateAllFixedCharges()
        {
            try
            {
                var properties = _db.StockCreations.Where(x => x.MemberProfileId != null &&
                                                          x.PossessionStatus == true &&
                                                          x.PropertyNo != null &&
                                                          x.PropertyNo != "" &&
                                                          x.RegistrationNo != null &&
                                                          x.RegistrationNo != ""
                                                          )
                                                   .ToList();
                foreach (var item in properties)
                {

                    GlobalChargeSetupDetailFixedChargFilterDTO model = new GlobalChargeSetupDetailFixedChargFilterDTO()
                    {
                        FormId = 5,
                        RealStateTypeId = Convert.ToInt32(item.RealStateType),
                        ProjectId = Convert.ToInt32(item.Project),
                        PhaseId = Convert.ToInt32(item.Phase),
                        BlockId = Convert.ToInt32(item.Block),
                        CategoryId = Convert.ToInt32(item.Category),
                        PropertyTypeId = Convert.ToInt32(item.Type),
                        NatureId = Convert.ToInt32(item.Nature),
                        PossessionStatus = item.PossessionStatus,
                        ConstructionStatus = item.ConstracutionStatus,
                        GracePeriod = item.GrancePeriodForBillGenration < DateTime.Now.Date ? false : true,
                    };

                    List<UpdateFixedChargeDTO> currentSetup = GetGlobalChargeDetail(model);

                    if (currentSetup.Count() > 0)
                    {
                        var existingchargeSetup = _db.PropertyFixedChargesSetup.Where(x => x.StockCreationId == item.ID).FirstOrDefault();

                        if (existingchargeSetup != null)
                        {
                            if (existingchargeSetup.GlobalChargeSetupId != currentSetup.FirstOrDefault().GlobalChargeSetupId)
                            {
                                var removeExistingSetup = _db.PropertyFixedChargesSetup.Where(x => x.StockCreationId == item.ID).ToList();
                                if (removeExistingSetup.Count() > 0)
                                {
                                    _db.PropertyFixedChargesSetup.RemoveRange(removeExistingSetup);
                                    _db.SaveChanges();
                                }

                                List<PropertyFixedChargesSetup> propertyFixedChargesSetups = new List<PropertyFixedChargesSetup>();
                                foreach (var charge in currentSetup)
                                {
                                    PropertyFixedChargesSetup propertyFixedChargesSetup = new PropertyFixedChargesSetup();

                                    propertyFixedChargesSetup.MatchId = charge.Id;
                                    propertyFixedChargesSetup.RegistrationNo = item.RegistrationNo;
                                    propertyFixedChargesSetup.PropertyNo = item.PropertyNo;
                                    propertyFixedChargesSetup.ChargeType = charge.ChargeType;
                                    propertyFixedChargesSetup.Unit = 1;
                                    propertyFixedChargesSetup.ChargeSetupRate = charge.Rate;
                                    propertyFixedChargesSetup.Rate = charge.Rate;
                                    propertyFixedChargesSetup.Discount = 0;
                                    propertyFixedChargesSetup.ChargeDes = charge.ChargeDes;
                                    propertyFixedChargesSetup.IsEnabled = true;
                                    propertyFixedChargesSetup.GlobalChargeSetupId = charge.GlobalChargeSetupId;
                                    propertyFixedChargesSetup.StockCreationId = item.ID;
                                    propertyFixedChargesSetup.CreatedOn = DateTime.Now;
                                    propertyFixedChargesSetup.LastModified = DateTime.Now;

                                    propertyFixedChargesSetups.Add(propertyFixedChargesSetup);
                                }

                                _db.PropertyFixedChargesSetup.AddRange(propertyFixedChargesSetups);
                                _db.SaveChanges();
                            }
                        }
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

        public List<UpdateFixedChargeDTO> GetGlobalChargeDetail(GlobalChargeSetupDetailFixedChargFilterDTO dto)
        {
            int Id = _db.FormsChargeGroup.SingleOrDefault(x => x.FormId == 5).ChargeGroupId;

            List<UpdateFixedChargeDTO> updateFixedChargeDTOs = new List<UpdateFixedChargeDTO>();

            if (Id != 0)
            {
                updateFixedChargeDTOs = _db.GlobalChargeDetail.Where(x => !x.IsDeleted == true
                                                        && x.GlobalChargeSetup.GlobalChargeGroupId == Id
                                                        && (x.GlobalChargeSetup.RealStateTypeId == dto.RealStateTypeId || x.GlobalChargeSetup.RealStateTypeId == null)
                                                        && (x.GlobalChargeSetup.ProjectId == dto.ProjectId || x.GlobalChargeSetup.ProjectId == null)
                                                        && (x.GlobalChargeSetup.PhaseId == dto.PhaseId || x.GlobalChargeSetup.PhaseId == null || x.GlobalChargeSetup.PhaseId == -1)
                                                        && (x.GlobalChargeSetup.BlockId == dto.BlockId || x.GlobalChargeSetup.BlockId == null)
                                                        && (x.GlobalChargeSetup.CategoryId == dto.CategoryId || x.GlobalChargeSetup.CategoryId == null)
                                                        && (x.GlobalChargeSetup.PropertyTypeId == dto.PropertyTypeId || x.GlobalChargeSetup.PropertyTypeId == null)
                                                        && (x.GlobalChargeSetup.NatureId == dto.NatureId || x.GlobalChargeSetup.NatureId == null)
                                                        && x.GlobalChargeSetup.PossessionStatus == dto.PossessionStatus
                                                        && (x.GlobalChargeSetup.ConstructionStatus == dto.ConstructionStatus || x.GlobalChargeSetup.ConstructionStatus == null)
                                                        && (x.GlobalChargeSetup.GracePeriod == dto.GracePeriod)
                                                     )
                                              .Distinct()
                                              .Select(x => new UpdateFixedChargeDTO
                                              {
                                                  Id = x.Id,
                                                  ChargeType = x.Description,
                                                  ChargeDes = x.GlobalChargeSetup.Description,
                                                  Rate = (decimal)x.Rate,
                                                  GlobalChargeSetupId = (int)x.GlobalChargeSetupId
                                              })
                                              .ToList();
            }
            return updateFixedChargeDTOs;
        }

        [HttpGet]
        [Route("GetPropertiesForRepurchase")]
        public IActionResult GetPropertiesForRepurchase()
        {
            try
            {
                var result = _db.StockCreations.Where(x => !x.is_deleted)
                                               .Distinct()
                                               .Select(x => new
                                               {
                                                   x.ID,
                                                   x.RegistrationNo,
                                                   x.PropertyNo,
                                                   x.Dealer.PrincipalOwner,
                                                   x.MemberProfile.MemberName,
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

        //[HttpGet]
        //[Route("GetRenumberRegistration")]
        //public IActionResult GetRenumberRegistration()
        //{
        //    try
        //    {

        //        var result = _db.StockCreations.Where(x => !x.is_deleted && x.RegistrationNo != null && x.PropertyNo != null && x.MemberProfileId != null)
        //                                   .Select(x => new RenumberDto
        //                                   {
        //                                       ID = x.ID,
        //                                       RegistrationNo = x.RegistrationNo,
        //                                       PropertyNo = x.PropertyNo,
        //                                       ActualSize = x.ActualSize,
        //                                       BlockName = x.Block,
        //                                       CategoryName = x.Category,
        //                                       MemberName = x.MemberProfile.MemberName,
        //                                       Cnic = x.MemberProfile.Cnic
        //                                   })
        //                                   .ToList();

        //        var Blocks = _db.Blocks.ToList();
        //        var Categories = _db.Categories.ToList();

        //        foreach (var item in result)
        //        {
        //            if (int.TryParse(item.BlockName, out int blockId))
        //            {
        //                item.BlockName = Blocks.Where(p => p.ID == blockId).Select(x => x.Description).FirstOrDefault();
        //            }

        //            if (int.TryParse(item.CategoryName, out int categoryId))
        //            {
        //                item.CategoryName = Categories.Where(p => p.ID == categoryId).Select(x => x.Description).FirstOrDefault();
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
        [Route("GetRenumberRegistration")]
        public IActionResult GetRenumberRegistration()
        {
            try
            {
                string jsonResult = "[]";

                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "GetRenumberRegistration";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    _db.Database.OpenConnection();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            jsonResult = reader["JsonStringValue"]?.ToString();
                        }
                    }
                }

                var data = string.IsNullOrEmpty(jsonResult)
                    ? new List<RenumberDto>()
                    : JsonConvert.DeserializeObject<List<RenumberDto>>(jsonResult);

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = data
                });
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
            finally
            {
                _db.Database.CloseConnection();
            }
        }



        [HttpGet]
        [Route("GetRenumberProperty")]
        public IActionResult GetRenumberProperty()
        {
            try
            {

                var result = _db.StockCreations.Where(x => !x.is_deleted && x.PropertyNo != null
                                                    && (x.RegistrationNo == null || x.RegistrationNo == ""))
                                           .Select(x => new RenumberDto
                                           {
                                               ID = x.ID,
                                               RegistrationNo = x.RegistrationNo,
                                               PropertyNo = x.PropertyNo,
                                               ActualSize = x.ActualSize,
                                               BlockName = x.Block,
                                               CategoryName = x.Category,
                                               MemberName = "N/A",
                                               Cnic = "N/A"
                                           })
                                           .ToList();

                var Blocks = _db.Blocks.ToList();
                var Categories = _db.Categories.ToList();

                foreach (var item in result)
                {
                    item.BlockName = Blocks.Where(p => p.ID == (Convert.ToInt32(item.BlockName))).Select(x => x.Description).FirstOrDefault();
                    item.CategoryName = Categories.Where(p => p.ID == (Convert.ToInt32(item.CategoryName))).Select(x => x.Description).FirstOrDefault();
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
        [AllowAnonymous]
        [Route("CheckSetupExist")]
        public IActionResult CheckSetupExist(int id)
        {
            try
            {
                var properties = _db.StockCreations.Where(x => x.MemberProfileId != null &&
                                                          x.PossessionStatus == true &&
                                                          x.PropertyNo != null &&
                                                          x.PropertyNo != "" &&
                                                          x.RegistrationNo != null &&
                                                          x.RegistrationNo != ""
                                                          )
                                                   .ToList();

                List<string> RegistrationNosChargeSetupExist = new List<string>();
                List<string> RegistrationNosChargeSetupNotFound = new List<string>();

                foreach (var item in properties)
                {

                    GlobalChargeSetupDetailFixedChargFilterDTO model = new GlobalChargeSetupDetailFixedChargFilterDTO()
                    {
                        FormId = 5,
                        RealStateTypeId = Convert.ToInt32(item.RealStateType),
                        ProjectId = Convert.ToInt32(item.Project),
                        PhaseId = Convert.ToInt32(item.Phase),
                        BlockId = Convert.ToInt32(item.Block),
                        CategoryId = Convert.ToInt32(item.Category),
                        PropertyTypeId = Convert.ToInt32(item.Type),
                        NatureId = Convert.ToInt32(item.Nature),
                        PossessionStatus = item.PossessionStatus,
                        ConstructionStatus = item.ConstracutionStatus,
                        GracePeriod = item.GrancePeriodForBillGenration < DateTime.Now.Date ? false : true,
                    };

                    List<UpdateFixedChargeDTO> currentSetup = GetGlobalChargeDetail(model);

                    if (currentSetup.Count() > 0)
                    {
                        RegistrationNosChargeSetupExist.Add(item.RegistrationNo);
                    }
                    else
                    {
                        RegistrationNosChargeSetupNotFound.Add(item.RegistrationNo);
                    }
                }
                if (id == 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = RegistrationNosChargeSetupNotFound
                    });
                }
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = RegistrationNosChargeSetupExist
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("InsetAllFixedCharges")]
        public IActionResult InsetAllFixedCharges()
        {
            try
            {
                var properties = _db.StockCreations.Where(x => x.MemberProfileId != null &&
                                                          x.PossessionStatus == true &&
                                                          x.PropertyNo != null &&
                                                          x.PropertyNo != "" &&
                                                          x.RegistrationNo != null &&
                                                          x.RegistrationNo != ""
                                                          )
                                                    .ToList();
                foreach (var item in properties)
                {

                    GlobalChargeSetupDetailFixedChargFilterDTO model = new GlobalChargeSetupDetailFixedChargFilterDTO()
                    {
                        FormId = 5,
                        RealStateTypeId = Convert.ToInt32(item.RealStateType),
                        ProjectId = Convert.ToInt32(item.Project),
                        PhaseId = Convert.ToInt32(item.Phase),
                        BlockId = Convert.ToInt32(item.Block),
                        CategoryId = Convert.ToInt32(item.Category),
                        PropertyTypeId = Convert.ToInt32(item.Type),
                        NatureId = Convert.ToInt32(item.Nature),
                        PossessionStatus = item.PossessionStatus,
                        ConstructionStatus = item.ConstracutionStatus,
                        GracePeriod = item.GrancePeriodForBillGenration < DateTime.Now.Date ? false : true,
                    };

                    List<UpdateFixedChargeDTO> currentSetup = GetGlobalChargeDetail(model);

                    if (currentSetup.Count() > 0)
                    {
                        currentSetup.DistinctBy(x => x.ChargeType);
                        var existingchargeSetup = _db.PropertyFixedChargesSetup.Where(x => x.StockCreationId == item.ID).FirstOrDefault();

                        if (existingchargeSetup == null)
                        {

                            List<PropertyFixedChargesSetup> propertyFixedChargesSetups = new List<PropertyFixedChargesSetup>();
                            foreach (var charge in currentSetup)
                            {
                                PropertyFixedChargesSetup propertyFixedChargesSetup = new PropertyFixedChargesSetup();

                                propertyFixedChargesSetup.MatchId = charge.Id;
                                propertyFixedChargesSetup.RegistrationNo = item.RegistrationNo;
                                propertyFixedChargesSetup.PropertyNo = item.PropertyNo;
                                propertyFixedChargesSetup.ChargeType = charge.ChargeType;
                                propertyFixedChargesSetup.Unit = 1;
                                propertyFixedChargesSetup.ChargeSetupRate = charge.Rate;
                                propertyFixedChargesSetup.Rate = charge.Rate;
                                propertyFixedChargesSetup.Discount = 0;
                                propertyFixedChargesSetup.ChargeDes = charge.ChargeDes;
                                propertyFixedChargesSetup.IsEnabled = true;
                                propertyFixedChargesSetup.GlobalChargeSetupId = charge.GlobalChargeSetupId;
                                propertyFixedChargesSetup.StockCreationId = item.ID;
                                propertyFixedChargesSetup.CreatedOn = DateTime.Now;
                                propertyFixedChargesSetup.LastModified = DateTime.Now;

                                propertyFixedChargesSetups.Add(propertyFixedChargesSetup);
                            }

                            _db.PropertyFixedChargesSetup.AddRange(propertyFixedChargesSetups);
                            _db.SaveChanges();
                        }
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
    }
}
