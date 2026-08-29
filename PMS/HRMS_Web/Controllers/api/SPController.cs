using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using CloudinaryDotNet.Actions;
using HRMS_Web.Models.DTOs.SPDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SPController : ControllerBase
    {
        private readonly DataBase_Context _db;
        public SPController(DataBase_Context db)
        {
            _db = db;
        }

        #region Reports

        [HttpPost]
        [Route("GetAllCautionReportFilters")]
        public IActionResult GetAllCautionReportFilters()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            string startDate = Request.Form["startDate"].FirstOrDefault();
            string endDate = Request.Form["endDate"].FirstOrDefault();
            string refrenceNo = Request.Form["refrenceNo"].FirstOrDefault();
            string plotNo = Request.Form["plotNo"].FirstOrDefault();
            string sector = Request.Form["sector"].FirstOrDefault();
            string Description = Request.Form["Description"].FirstOrDefault();


            var data = GetAllCautionReportFiltersSP(pageNumber, pageSize, searchValue, startDate, endDate, refrenceNo, plotNo, sector, Description);

            totalRecord = (from th in _db.SoftLock
                           join rp in _db.RegistrationNoProfile on th.RegistrationNoProfileId equals rp.Id
                           from sc in _db.StockCreations
                                .Where(sc => sc.ID == rp.StockCreationId)
                                .DefaultIfEmpty()
                           where th.IsActive && !th.IsDeleted
                                 && (string.IsNullOrEmpty(sector) || sc.PrefixProperty == sector)
                                 && (string.IsNullOrEmpty(startDate) || th.CreatedOn.Date >= DateTime.Parse(startDate))
                                 && (string.IsNullOrEmpty(endDate) || th.CreatedOn.Date <= DateTime.Parse(endDate))
                                 && (string.IsNullOrEmpty(refrenceNo) || sc.RegistrationNo == refrenceNo)
                                 && (string.IsNullOrEmpty(Description) || th.SoftLockName == Description)
                                 && (string.IsNullOrEmpty(plotNo) || sc.PropertyNo == plotNo)
                           select th).Count();

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


        private List<CautionReportDto> GetAllCautionReportFiltersSP(
          int pageNumber = 1,
          int pageSize = 10,
          string? searchTerm = "",
          string? startDate = null,
          string? endDate = null,
          string? refrenceNo = null,
          string? plotNo = null,
          string? sector = null,
          string? Description = null)
        {
            var properties = new List<CautionReportDto>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            var startDateParam = new SqlParameter("@StartDate", startDate ?? (object)DBNull.Value);
            var endDateParam = new SqlParameter("@EndDate", endDate ?? (object)DBNull.Value);
            var refrenceNoParam = new SqlParameter("@RefrenceNo", refrenceNo ?? (object)DBNull.Value);
            var plotNoParam = new SqlParameter("@PlotNo", plotNo ?? (object)DBNull.Value);
            var sectorParam = new SqlParameter("@Sector", sector ?? (object)DBNull.Value);
            var DescriptionParam = new SqlParameter("@Description", Description ?? (object)DBNull.Value);

            var query = $"EXEC [dbo].[sp_GetAllCautionReportFilters] " +
                        "@PageNumber = @PageNumber, @PageSize = @PageSize, @SearchTerm = @SearchTerm, " +
                        "@StartDate = @StartDate, @EndDate = @EndDate, @RefrenceNo = @RefrenceNo, " +
                        "@PlotNo = @PlotNo, @Sector = @Sector, @Description = @Description";

            var DataModel = _db.JsonOutPutModel.FromSqlRaw(query,
                pageNumberParam, pageSizeParam, searchTermParam, startDateParam, endDateParam, refrenceNoParam, plotNoParam, sectorParam, DescriptionParam).ToList();

            if (DataModel != null && DataModel.Any())
            {
                var jsonValue = DataModel.FirstOrDefault()?.JsonStringValue;
                if (!String.IsNullOrEmpty(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<CautionReportDto>>(jsonValue);
                }
            }

            return properties;
        }

        [HttpPost]
        [Route("GetAllRecordRoomFileInOutReportFilters")]
        public IActionResult GetAllRecordRoomFileInOutReportFilters()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            string startDate = Request.Form["startDate"].FirstOrDefault();
            string endDate = Request.Form["endDate"].FirstOrDefault();
            string refrenceNo = Request.Form["refrenceNo"].FirstOrDefault();
            string plotNo = Request.Form["plotNo"].FirstOrDefault();
            string block = Request.Form["block"].FirstOrDefault();


            var data = GetAllRecordRoomFileInOutReportFiltersSP(pageNumber, pageSize, searchValue, startDate, endDate, refrenceNo, plotNo,block);

            totalRecord = _db.StoreRoomFileMoving
                             .Where(x =>x.IsFileClosed == true &&
                                 (string.IsNullOrEmpty(startDate) || x.CreatedOn.Date >= DateTime.Parse(startDate)) &&
                                 (string.IsNullOrEmpty(endDate) || x.CreatedOn.Date <= DateTime.Parse(endDate)) &&
                                 (string.IsNullOrEmpty(refrenceNo) || x.StockCreation.RegistrationNo == refrenceNo) &&
                                 (string.IsNullOrEmpty(plotNo) || x.StockCreation.PropertyNo == plotNo)
                                 &&
                                 (string.IsNullOrEmpty(block) || x.StockCreation.Block == block))
                             .Count();

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


        private List<RecordRoomFileInOutReportDto> GetAllRecordRoomFileInOutReportFiltersSP(
          int pageNumber = 1,
          int pageSize = 10,
          string? searchTerm = "",
          string? startDate = null,
          string? endDate = null,
          string? refrenceNo = null,
          string? plotNo = null,
          string? block = null)
        {
            var properties = new List<RecordRoomFileInOutReportDto>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);
            var blockParam = new SqlParameter("@Block", block ?? (object)DBNull.Value);
            var startDateParam = new SqlParameter("@StartDate", startDate ?? (object)DBNull.Value);
            var endDateParam = new SqlParameter("@EndDate", endDate ?? (object)DBNull.Value);
            var refrenceNoParam = new SqlParameter("@RefrenceNo", refrenceNo ?? (object)DBNull.Value);
            var plotNoParam = new SqlParameter("@PlotNo", plotNo ?? (object)DBNull.Value);

            var query = $"EXEC [dbo].[sp_GetAllRecordRoomFileInOutReportFilters] " +
                        "@PageNumber = @PageNumber, @PageSize = @PageSize, @SearchTerm = @SearchTerm, " +
                        "@StartDate = @StartDate, @EndDate = @EndDate, @RefrenceNo = @RefrenceNo, " +
                        "@PlotNo = @PlotNo, @Block = @Block";

            var DataModel = _db.JsonOutPutModel.FromSqlRaw(query,
                pageNumberParam, pageSizeParam, searchTermParam, startDateParam, endDateParam, refrenceNoParam, plotNoParam, blockParam).ToList();

            if (DataModel != null && DataModel.Any())
            {
                var jsonValue = DataModel.FirstOrDefault()?.JsonStringValue;
                if (!String.IsNullOrEmpty(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<RecordRoomFileInOutReportDto>>(jsonValue);
                }
            }

            return properties;
        }

        [HttpPost]
        [Route("FileInOutSummaryReport")]
        public IActionResult FileInOutSummaryReport()
        {
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            string startDate = Request.Form["startDate"].FirstOrDefault();
            string endDate = Request.Form["endDate"].FirstOrDefault();
            string refrenceNo = Request.Form["refrenceNo"].FirstOrDefault();
            string plotNo = Request.Form["plotNo"].FirstOrDefault();
            string block = Request.Form["block"].FirstOrDefault();

            var data = GetFileInOutSummaryReportSP(searchValue, startDate, endDate, refrenceNo, plotNo, block); 
            return Ok(data.ToList());
        }



        private List<KeyValuePairDto> GetFileInOutSummaryReportSP(
    string? searchTerm = "",
    string? startDate = null,
    string? endDate = null,
    string? refrenceNo = null,
    string? plotNo = null,
    string? block = null)
        {
            var properties = new List<KeyValuePairDto>();

            // ✅ Handle NULLs safely — always supply a parameter value
            var searchTermParam = new SqlParameter("@SearchTerm", string.IsNullOrEmpty(searchTerm) ? "" : searchTerm);
            var startDateParam = new SqlParameter("@StartDate", string.IsNullOrEmpty(startDate) ? DBNull.Value : DateTime.Parse(startDate));
            var endDateParam = new SqlParameter("@EndDate", string.IsNullOrEmpty(endDate) ? DBNull.Value : DateTime.Parse(endDate));
            var refrenceNoParam = new SqlParameter("@RefrenceNo", string.IsNullOrEmpty(refrenceNo) ? "" : refrenceNo);
            var plotNoParam = new SqlParameter("@PlotNo", string.IsNullOrEmpty(plotNo) ? "" : plotNo);
            var blockParam = new SqlParameter("@Block", string.IsNullOrEmpty(block) ? "" : block);

            string query = "EXEC [dbo].[sp_FileInOutSummaryReportSP] " +
                           "@SearchTerm, @StartDate, @EndDate, @RefrenceNo, @PlotNo, @Block";

            // Execute and handle JSON result
            var dataModel = _db.JsonOutPutModel.FromSqlRaw(query,
                searchTermParam,
                startDateParam,
                endDateParam,
                refrenceNoParam,
                plotNoParam,
                blockParam
            ).ToList();

            if (dataModel != null && dataModel.Any())
            {
                var jsonValue = dataModel.FirstOrDefault()?.JsonStringValue;
                if (!string.IsNullOrWhiteSpace(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<KeyValuePairDto>>(jsonValue);
                }
            }

            return properties;
        }




        [HttpPost]
        [Route("GetAllRecordRoomReportFilters")]
        public IActionResult GetAllRecordRoomReportFilters()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            string startDate = Request.Form["startDate"].FirstOrDefault();
            string endDate = Request.Form["endDate"].FirstOrDefault();
            string refrenceNo = Request.Form["refrenceNo"].FirstOrDefault();
            string plotNo = Request.Form["plotNo"].FirstOrDefault();
            string sector = Request.Form["sector"].FirstOrDefault();
           

            var data = GetAllRecordRoomReportFiltersSP(pageNumber, pageSize, searchValue, startDate, endDate, refrenceNo, plotNo, sector);

            totalRecord = _db.StockCreations
                             .Where(x => 
                                 (string.IsNullOrEmpty(startDate) || x.Created_at.Date >= DateTime.Parse(startDate)) &&
                                 (string.IsNullOrEmpty(endDate) || x.Created_at.Date <= DateTime.Parse(endDate)) &&
                                 (string.IsNullOrEmpty(refrenceNo) || x.RegistrationNo == refrenceNo) &&
                                 (string.IsNullOrEmpty(plotNo) || x.PropertyNo == plotNo) &&
                                 (string.IsNullOrEmpty(sector) || x.PrefixProperty == sector))
                             .Count();

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


        private List<TransferSetReceivingDto> GetAllRecordRoomReportFiltersSP(
          int pageNumber = 1,
          int pageSize = 10,
          string? searchTerm = "",
          string? startDate = null,
          string? endDate = null,
          string? refrenceNo = null,
          string? plotNo = null,
          string? sector = null)
        {
            var properties = new List<TransferSetReceivingDto>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            var startDateParam = new SqlParameter("@StartDate", startDate ?? (object)DBNull.Value);
            var endDateParam = new SqlParameter("@EndDate", endDate ?? (object)DBNull.Value);
            var refrenceNoParam = new SqlParameter("@RefrenceNo", refrenceNo ?? (object)DBNull.Value);
            var plotNoParam = new SqlParameter("@PlotNo", plotNo ?? (object)DBNull.Value);
            var sectorParam = new SqlParameter("@Sector", sector ?? (object)DBNull.Value);

            var query = $"EXEC [dbo].[sp_GetAllRecordRoomReportFilters] " +
                        "@PageNumber = @PageNumber, @PageSize = @PageSize, @SearchTerm = @SearchTerm, " +
                        "@StartDate = @StartDate, @EndDate = @EndDate, @RefrenceNo = @RefrenceNo, " +
                        "@PlotNo = @PlotNo, @Sector = @Sector";

            var DataModel = _db.JsonOutPutModel.FromSqlRaw(query,
                pageNumberParam, pageSizeParam, searchTermParam, startDateParam, endDateParam, refrenceNoParam, plotNoParam, sectorParam).ToList();

            if (DataModel != null && DataModel.Any())
            {
                var jsonValue = DataModel.FirstOrDefault()?.JsonStringValue;
                if (!String.IsNullOrEmpty(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<TransferSetReceivingDto>>(jsonValue);
                }
            }

            return properties;
        }

        [HttpPost]
        [Route("GetAllTransferRevenueRequestFilters")]
        public IActionResult GetAllTransferRevenueRequestFilters()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;


            string startDate = Request.Form["startDate"].FirstOrDefault();
            string endDate = Request.Form["endDate"].FirstOrDefault();
            string refrenceNo = Request.Form["refrenceNo"].FirstOrDefault();
            string plotNo = Request.Form["plotNo"].FirstOrDefault();
            string sector = Request.Form["sector"].FirstOrDefault();
            string requestType = Request.Form["requestType"].FirstOrDefault();
            string transferType = Request.Form["transferType"].FirstOrDefault();
            string dealerName = Request.Form["dealerName"].FirstOrDefault();

            var data = GetAllTransferRevenueRequestFiltersSP(pageNumber, pageSize, searchValue, startDate, endDate, refrenceNo, plotNo, sector, requestType, transferType, dealerName);


            totalRecord = (from th in _db.TransferHistery
                           join sc in _db.StockCreations on th.StockCreationId equals sc.ID
                           from trp in _db.TransferReceiptProcessing
                                .Where(trp => trp.Id == th.ReciptPrpcessingId)
                                .DefaultIfEmpty()
                           where th.IsActive && !th.IsDeleted
                                 && !string.IsNullOrEmpty(th.SellerName)
                                 && th.IsRequestClosed == true
                                 && (string.IsNullOrEmpty(sector) || sc.PrefixProperty == sector)
                                 && (string.IsNullOrEmpty(startDate) || th.CreatedOn.Date >= DateTime.Parse(startDate))
                                 && (string.IsNullOrEmpty(endDate) || th.CreatedOn.Date <= DateTime.Parse(endDate))
                                 && (string.IsNullOrEmpty(refrenceNo) || sc.RegistrationNo == refrenceNo)
                                 && (string.IsNullOrEmpty(plotNo) || sc.PropertyNo == plotNo)
                                 && (string.IsNullOrEmpty(requestType) || trp.NDCRequestType == requestType)
                                 && (string.IsNullOrEmpty(transferType) || trp.TransferType == transferType)
                                 && (string.IsNullOrEmpty(dealerName) || th.DealerName == dealerName)
                           select th).Count();

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


        private List<NDCStateReportDto> GetAllTransferRevenueRequestFiltersSP(
          int pageNumber = 1,
          int pageSize = 10,
          string? searchTerm = "",
          string? startDate = null,
          string? endDate = null,
          string? refrenceNo = null,
          string? plotNo = null,
          string? sector = null,
          string? requestType = null,
          string? transferType = null,
          string? dealerName = null)
        {
            var properties = new List<NDCStateReportDto>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            var startDateParam = new SqlParameter("@StartDate", startDate ?? (object)DBNull.Value);
            var endDateParam = new SqlParameter("@EndDate", endDate ?? (object)DBNull.Value);
            var refrenceNoParam = new SqlParameter("@RefrenceNo", refrenceNo ?? (object)DBNull.Value);
            var plotNoParam = new SqlParameter("@PlotNo", plotNo ?? (object)DBNull.Value);
            var sectorParam = new SqlParameter("@Sector", sector ?? (object)DBNull.Value);
            var requestTypeParam = new SqlParameter("@RequestType", requestType ?? (object)DBNull.Value);
            var transferTypeParam = new SqlParameter("@TransferType", transferType ?? (object)DBNull.Value);
            var dealerNameParam = new SqlParameter("@DealerName", dealerName ?? (object)DBNull.Value);

            var query = $"EXEC [dbo].[sp_GetAllTransferRevenueRequestFilters] " +
                        "@PageNumber = @PageNumber, @PageSize = @PageSize, @SearchTerm = @SearchTerm, " +
                        "@StartDate = @StartDate, @EndDate = @EndDate, @RefrenceNo = @RefrenceNo, " +
                        "@PlotNo = @PlotNo, @Sector = @Sector, @RequestType = @RequestType,@TransferType = @TransferType, @DealerName = @DealerName";

            var DataModel = _db.JsonOutPutModel.FromSqlRaw(query,
                pageNumberParam, pageSizeParam, searchTermParam, startDateParam, endDateParam, refrenceNoParam, plotNoParam, sectorParam, requestTypeParam, transferTypeParam, dealerNameParam).ToList();

            if (DataModel != null && DataModel.Any())
            {
                var jsonValue = DataModel.FirstOrDefault()?.JsonStringValue;
                if (!String.IsNullOrEmpty(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<NDCStateReportDto>>(jsonValue);
                }
            }

            return properties;
        }

        [HttpPost]
        [Route("GetAllTransferRequestFilters")]
        public IActionResult GetAllTransferRequestFilters()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

           
            string startDate = Request.Form["startDate"].FirstOrDefault();
            string endDate = Request.Form["endDate"].FirstOrDefault();
            string refrenceNo = Request.Form["refrenceNo"].FirstOrDefault();
            string plotNo = Request.Form["plotNo"].FirstOrDefault();
            //string sector = Request.Form["sector"].FirstOrDefault();
            string phase = Request.Form["Phase"].FirstOrDefault();
            string realStateType = Request.Form["RealStateType"].FirstOrDefault();
            string category = Request.Form["Category"].FirstOrDefault();
            string block = Request.Form["Block"].FirstOrDefault();
            string type = Request.Form["Type"].FirstOrDefault();
            string nature = Request.Form["Nature"].FirstOrDefault();
            string requestType = Request.Form["requestType"].FirstOrDefault();
            string transferType = Request.Form["transferType"].FirstOrDefault();
            string dealerName = Request.Form["dealerName"].FirstOrDefault();
            int? transferTypeInt = null;
            if (int.TryParse(transferType, out int parsed))
            {
                transferTypeInt = parsed;
            }
            var data = GetAllTransferRequestFiltersSP(pageNumber, pageSize, searchValue, startDate, endDate, refrenceNo, plotNo, requestType, transferType, dealerName, phase, realStateType, category, block, type, nature);


            totalRecord = (from th in _db.TransferHistery
                           join sc in _db.StockCreations on th.StockCreationId equals sc.ID
                           from trp in _db.TransferReceiptProcessing
                                .Where(trp => trp.Id == th.ReciptPrpcessingId)
                                .DefaultIfEmpty()
                           where th.IsActive && !th.IsDeleted
                                 && !string.IsNullOrEmpty(th.SellerName)
                                 && th.IsRequestClosed == true
                                 && (string.IsNullOrEmpty(startDate) || th.CreatedOn.Date >= DateTime.Parse(startDate))
                                 && (string.IsNullOrEmpty(endDate) || th.CreatedOn.Date <= DateTime.Parse(endDate))
                                 && (string.IsNullOrEmpty(refrenceNo) || sc.RegistrationNo == refrenceNo)
                                 && (string.IsNullOrEmpty(plotNo) || sc.PropertyNo == plotNo)
                                 && (string.IsNullOrEmpty(requestType) || trp.NDCRequestType == requestType)
                                 && (string.IsNullOrEmpty(transferType) || trp.TransferType == transferType)
                                 && (string.IsNullOrEmpty(dealerName) || th.DealerName == dealerName)
                                 && (string.IsNullOrEmpty(phase) || sc.Phase == phase)
                                 && (string.IsNullOrEmpty(realStateType) || sc.RealStateType == realStateType)
                                 && (string.IsNullOrEmpty(category) || sc.Category == category)
                                 && (string.IsNullOrEmpty(block) || sc.Block == block)
                                 && (string.IsNullOrEmpty(type) || sc.Type == type)
                                 && (string.IsNullOrEmpty(nature) || sc.Nature == nature)
                           select th).Count();

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


        private List<NDCStateReportDto> GetAllTransferRequestFiltersSP(
          int pageNumber = 1,
          int pageSize = 10,
          string? searchTerm = "",
          string? startDate = null,
          string? endDate = null,
          string? refrenceNo = null,
          string? plotNo = null,
          //string? sector = null,
          string? requestType = null,
          string? transferType = null,
          string? dealerName = null,
          string? phase = null,
          string? realStateType = null,
          string? category = null,
          string? block = null,
          string? type = null,
          string? nature = null)

        {
            var properties = new List<NDCStateReportDto>();

            var parameters = new[]
            {
                new SqlParameter("@PageNumber", pageNumber),
                new SqlParameter("@PageSize", pageSize),
                new SqlParameter("@SearchTerm", string.IsNullOrEmpty(searchTerm) ? DBNull.Value : searchTerm),
                new SqlParameter("@StartDate", string.IsNullOrEmpty(startDate) ? DBNull.Value : DateTime.Parse(startDate)),
                new SqlParameter("@EndDate", string.IsNullOrEmpty(endDate) ? DBNull.Value : DateTime.Parse(endDate)),

                new SqlParameter("@RefrenceNo", string.IsNullOrEmpty(refrenceNo) ? DBNull.Value : refrenceNo), // RefrenceNo comes BEFORE Sector in SP
                new SqlParameter("@PlotNo", string.IsNullOrEmpty(plotNo) ? DBNull.Value : plotNo),

                new SqlParameter("@RequestType", string.IsNullOrEmpty(requestType) ? DBNull.Value : requestType),

                new SqlParameter("@TransferType", string.IsNullOrEmpty(transferType) ? DBNull.Value : transferType),

                new SqlParameter("@DealerName", string.IsNullOrEmpty(dealerName) ? DBNull.Value : dealerName),
                new SqlParameter("@Phase", string.IsNullOrEmpty(phase) ? DBNull.Value : phase),
                new SqlParameter("@RealStateType", string.IsNullOrEmpty(realStateType) ? DBNull.Value : realStateType),
                new SqlParameter("@Category", string.IsNullOrEmpty(category) ? DBNull.Value : category),
                new SqlParameter("@Block", string.IsNullOrEmpty(block) ? DBNull.Value : block),
                new SqlParameter("@Type", string.IsNullOrEmpty(type) ? DBNull.Value : type),
                new SqlParameter("@Nature", string.IsNullOrEmpty(nature) ? DBNull.Value : nature),
            };


            string query = "EXEC [dbo].[sp_GetAllTransferRequestFilters] " +
                "@PageNumber, @PageSize, @SearchTerm, @StartDate, @EndDate, " +
                "@RefrenceNo, @PlotNo, @RequestType, @TransferType, @DealerName, " +
                "@Phase, @RealStateType, @Category, @Block, @Type, @Nature";


            var dataModel = _db.JsonOutPutModel.FromSqlRaw(query, parameters).ToList();

            if (dataModel != null && dataModel.Any())
            {
                var jsonValue = dataModel.FirstOrDefault()?.JsonStringValue;
                if (!String.IsNullOrEmpty(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<NDCStateReportDto>>(jsonValue);
                }
            }

            return properties;
        }

        [HttpPost]
        [Route("TransferSummaryReport")]
        public IActionResult TransferSummaryReport()
        {
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "10");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            // Extract form values safely
            string startDate = Request.Form["startDate"].FirstOrDefault();
            string endDate = Request.Form["endDate"].FirstOrDefault();
            string refrenceNo = Request.Form["refrenceNo"].FirstOrDefault();
            string plotNo = Request.Form["plotNo"].FirstOrDefault();
            string sector = Request.Form["sector"].FirstOrDefault();
            string requestType = Request.Form["requestType"].FirstOrDefault();
            string transferTypeStr = Request.Form["transferType"].FirstOrDefault();
            string dealerName = Request.Form["dealerName"].FirstOrDefault();
            string phase = Request.Form["Phase"].FirstOrDefault();
            string realStateType = Request.Form["RealStateType"].FirstOrDefault();
            string category = Request.Form["Category"].FirstOrDefault();
            string block = Request.Form["Block"].FirstOrDefault();
            string type = Request.Form["Type"].FirstOrDefault();
            string nature = Request.Form["Nature"].FirstOrDefault();

           

            var data = GetTransferSummaryReportSP(pageNumber, pageSize, searchValue,
                startDate, endDate, refrenceNo, plotNo, sector, requestType,
                transferTypeStr, dealerName, phase, realStateType, category, block, type, nature);

            return Ok(data.ToList());
        }


        private List<KeyValuePairDto> GetTransferSummaryReportSP(
            int pageNumber = 1,
            int pageSize = 10,
            string? searchTerm = null,
            string? startDate = null,
            string? endDate = null,
            string? refrenceNo = null,
            string? plotNo = null,
            string? sector = null,
            string? requestType = null,
            string? transferType = null,
            string? dealerName = null,
            string? phase = null,
            string? realStateType = null,
            string? category = null,
            string? block = null,
            string? type = null,
            string? nature = null
        )
        {
            var properties = new List<KeyValuePairDto>();

            var parameters = new[]
            {
        new SqlParameter("@PageNumber", pageNumber),
        new SqlParameter("@PageSize", pageSize),
        new SqlParameter("@SearchTerm", string.IsNullOrEmpty(searchTerm) ? DBNull.Value : searchTerm),
        new SqlParameter("@StartDate", string.IsNullOrEmpty(startDate) ? DBNull.Value : DateTime.Parse(startDate)),
        new SqlParameter("@EndDate", string.IsNullOrEmpty(endDate) ? DBNull.Value : DateTime.Parse(endDate)),
        new SqlParameter("@RefrenceNo", string.IsNullOrEmpty(refrenceNo) ? DBNull.Value : refrenceNo),
        new SqlParameter("@PlotNo", string.IsNullOrEmpty(plotNo) ? DBNull.Value : plotNo),
        new SqlParameter("@Sector", string.IsNullOrEmpty(sector) ? DBNull.Value : sector),
        new SqlParameter("@RequestType", string.IsNullOrEmpty(requestType) ? DBNull.Value : requestType),
        new SqlParameter("@TransferType", string.IsNullOrEmpty(transferType)?  DBNull.Value :transferType),
        new SqlParameter("@DealerName", string.IsNullOrEmpty(dealerName) ? DBNull.Value : dealerName),
        new SqlParameter("@Phase", string.IsNullOrEmpty(phase) ? DBNull.Value : phase),
        new SqlParameter("@RealStateType", string.IsNullOrEmpty(realStateType) ? DBNull.Value : realStateType),
        new SqlParameter("@Category", string.IsNullOrEmpty(category) ? DBNull.Value : category),
        new SqlParameter("@Block", string.IsNullOrEmpty(block) ? DBNull.Value : block),
        new SqlParameter("@Type", string.IsNullOrEmpty(type) ? DBNull.Value : type),
        new SqlParameter("@Nature", string.IsNullOrEmpty(nature) ? DBNull.Value : nature),
    };

            string query = "EXEC [dbo].[sp_GetAllTransferSummary]" +
                           "@StartDate, @EndDate, @RefrenceNo, " +
                           "@PlotNo, @Sector, @RequestType, @TransferType, @DealerName, " +
                           "@Phase, @RealStateType, @Category, @Block, @Type, @Nature";

            var dataModel = _db.JsonOutPutModel.FromSqlRaw(query, parameters).ToList();

            if (dataModel != null && dataModel.Any())
            {
                var jsonValue = dataModel.FirstOrDefault()?.JsonStringValue;
                if (!string.IsNullOrWhiteSpace(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<KeyValuePairDto>>(jsonValue);
                }
            }

            return properties;
        }

        [HttpPost]
        [Route("NDCSummaryReport")]
        public IActionResult NDCSummaryReport()
        {
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "10");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            // Extract form values safely
            string startDate = Request.Form["startDate"].FirstOrDefault();
            string endDate = Request.Form["endDate"].FirstOrDefault();
            string refrenceNo = Request.Form["refrenceNo"].FirstOrDefault();
            string plotNo = Request.Form["plotNo"].FirstOrDefault();
            string sector = Request.Form["sector"].FirstOrDefault();
            string requestType = Request.Form["requestType"].FirstOrDefault();
            string transferTypeStr = Request.Form["transferType"].FirstOrDefault();
            string dealerName = Request.Form["dealerName"].FirstOrDefault();
            string phase = Request.Form["Phase"].FirstOrDefault();
            string realStateType = Request.Form["RealStateType"].FirstOrDefault();
            string category = Request.Form["Category"].FirstOrDefault();
            string block = Request.Form["Block"].FirstOrDefault();
            string type = Request.Form["Type"].FirstOrDefault();
            string nature = Request.Form["Nature"].FirstOrDefault();

            // Convert TransferType to int? since SP expects int
            int? transferType = null;
            if (int.TryParse(transferTypeStr, out int transferTypeVal))
            {
                transferType = transferTypeVal;
            }

            var data = GetNDCSummaryReportSP(pageNumber, pageSize, searchValue,
                startDate, endDate, refrenceNo, plotNo, sector, requestType,
                transferType, dealerName, phase, realStateType, category, block, type, nature);

            return Ok(data.ToList());
        }


        private List<KeyValuePairDto> GetNDCSummaryReportSP(
            int pageNumber = 1,
            int pageSize = 10,
            string? searchTerm = null,
            string? startDate = null,
            string? endDate = null,
            string? refrenceNo = null,
            string? plotNo = null,
            string? sector = null,
            string? requestType = null,
            int? transferType = null,
            string? dealerName = null,
            string? phase = null,
            string? realStateType = null,
            string? category = null,
            string? block = null,
            string? type = null,
            string? nature = null
        )
        {
            var properties = new List<KeyValuePairDto>();

            var parameters = new[]
            {
                new SqlParameter("@PageNumber", pageNumber),
                new SqlParameter("@PageSize", pageSize),
                new SqlParameter("@SearchTerm", string.IsNullOrEmpty(searchTerm) ? DBNull.Value : searchTerm),
                new SqlParameter("@StartDate",
                string.IsNullOrEmpty(startDate)
                ? DBNull.Value
                : DateTime.ParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)),
            
                new SqlParameter("@EndDate",
                string.IsNullOrEmpty(endDate)
                ? DBNull.Value
                : DateTime.ParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)),
            
                new SqlParameter("@RefrenceNo", string.IsNullOrEmpty(refrenceNo) ? DBNull.Value : refrenceNo),
                new SqlParameter("@PlotNo", string.IsNullOrEmpty(plotNo) ? DBNull.Value : plotNo),
                new SqlParameter("@Sector", string.IsNullOrEmpty(sector) ? DBNull.Value : sector),
                new SqlParameter("@RequestType", string.IsNullOrEmpty(requestType) ? DBNull.Value : requestType),
                new SqlParameter("@TransferType", transferType.HasValue ? (object)transferType.Value : DBNull.Value),
                new SqlParameter("@DealerName", string.IsNullOrEmpty(dealerName) ? DBNull.Value : dealerName),
                new SqlParameter("@Phase", string.IsNullOrEmpty(phase) ? DBNull.Value : phase),
                new SqlParameter("@RealStateType", string.IsNullOrEmpty(realStateType) ? DBNull.Value : realStateType),
                new SqlParameter("@Category", string.IsNullOrEmpty(category) ? DBNull.Value : category),
                new SqlParameter("@Block", string.IsNullOrEmpty(block) ? DBNull.Value : block),
                new SqlParameter("@Type", string.IsNullOrEmpty(type) ? DBNull.Value : type),
                new SqlParameter("@Nature", string.IsNullOrEmpty(nature) ? DBNull.Value : nature),
            };

            string query = "EXEC [dbo].[sp_GetNDCSummary] " +
                "@PageNumber, @PageSize, @SearchTerm, @StartDate, @EndDate, @RefrenceNo, " +
                "@PlotNo, @Sector, @RequestType, @TransferType, @DealerName, @Phase, " +
                "@RealStateType, @Category, @Block, @Type, @Nature";


            var dataModel = _db.JsonOutPutModel.FromSqlRaw(query, parameters).ToList();

            if (dataModel != null && dataModel.Any())
            {
                var jsonValue = dataModel.FirstOrDefault()?.JsonStringValue;
                if (!string.IsNullOrWhiteSpace(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<KeyValuePairDto>>(jsonValue);
                }
            }

            return properties;
        }

        [HttpPost]
        [Route("TransferSetSummaryReport")]
        public IActionResult TransferSetSummaryReport()
        {
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "10");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            // Extract form values safely
            string startDate = Request.Form["startDate"].FirstOrDefault();
            string endDate = Request.Form["endDate"].FirstOrDefault();
            string refrenceNo = Request.Form["refrenceNo"].FirstOrDefault();
            string plotNo = Request.Form["plotNo"].FirstOrDefault();
            string sector = Request.Form["sector"].FirstOrDefault();
            string requestType = Request.Form["requestType"].FirstOrDefault();
            string transferTypeStr = Request.Form["transferType"].FirstOrDefault();
            string dealerName = Request.Form["dealerName"].FirstOrDefault();
            string phase = Request.Form["Phase"].FirstOrDefault();
            string realStateType = Request.Form["RealStateType"].FirstOrDefault();
            string category = Request.Form["Category"].FirstOrDefault();
            string block = Request.Form["Block"].FirstOrDefault();
            string type = Request.Form["Type"].FirstOrDefault();
            string nature = Request.Form["Nature"].FirstOrDefault();

            // Convert TransferType to int? since SP expects int
            int? transferType = null;
            if (int.TryParse(transferTypeStr, out int transferTypeVal))
            {
                transferType = transferTypeVal;
            }

            var data = GetTransferSetSummaryReportSP(pageNumber, pageSize, searchValue,
                startDate, endDate, refrenceNo, plotNo, sector, requestType,
                transferType, dealerName, phase, realStateType, category, block, type, nature);

            return Ok(data.ToList());
        }

        private List<KeyValuePairDto> GetTransferSetSummaryReportSP(
            int pageNumber = 1,
            int pageSize = 10,
            string? searchTerm = null,
            string? startDate = null,
            string? endDate = null,
            string? refrenceNo = null,
            string? plotNo = null,
            string? sector = null,
            string? requestType = null,
            int? transferType = null,
            string? dealerName = null,
            string? phase = null,
            string? realStateType = null,
            string? category = null,
            string? block = null,
            string? type = null,
            string? nature = null
        )
        {
            var properties = new List<KeyValuePairDto>();

            var parameters = new[]
            {
        new SqlParameter("@PageNumber", pageNumber),
        new SqlParameter("@PageSize", pageSize),
        new SqlParameter("@SearchTerm", string.IsNullOrEmpty(searchTerm) ? DBNull.Value : searchTerm),
        new SqlParameter("@StartDate", string.IsNullOrEmpty(startDate) ? DBNull.Value : DateTime.Parse(startDate)),
        new SqlParameter("@EndDate", string.IsNullOrEmpty(endDate) ? DBNull.Value : DateTime.Parse(endDate)),
        new SqlParameter("@RefrenceNo", string.IsNullOrEmpty(refrenceNo) ? DBNull.Value : refrenceNo),
        new SqlParameter("@PlotNo", string.IsNullOrEmpty(plotNo) ? DBNull.Value : plotNo),
        new SqlParameter("@Sector", string.IsNullOrEmpty(sector) ? DBNull.Value : sector),
        new SqlParameter("@RequestType", string.IsNullOrEmpty(requestType) ? DBNull.Value : requestType),
        new SqlParameter("@TransferType", transferType.HasValue ? (object)transferType.Value : DBNull.Value),
        new SqlParameter("@DealerName", string.IsNullOrEmpty(dealerName) ? DBNull.Value : dealerName),
        new SqlParameter("@Phase", string.IsNullOrEmpty(phase) ? DBNull.Value : phase),
        new SqlParameter("@RealStateType", string.IsNullOrEmpty(realStateType) ? DBNull.Value : realStateType),
        new SqlParameter("@Category", string.IsNullOrEmpty(category) ? DBNull.Value : category),
        new SqlParameter("@Block", string.IsNullOrEmpty(block) ? DBNull.Value : block),
        new SqlParameter("@Type", string.IsNullOrEmpty(type) ? DBNull.Value : type),
        new SqlParameter("@Nature", string.IsNullOrEmpty(nature) ? DBNull.Value : nature),
    };

            string query = "EXEC [dbo].[sp_GetTransferSetSummary] " +
                           "@StartDate, @EndDate, @RefrenceNo, " +
                           "@PlotNo, @Sector, @DealerName, " +
                           "@Phase, @RealStateType, @Category, @Block, @Type, @Nature";

            var dataModel = _db.JsonOutPutModel.FromSqlRaw(query, parameters).ToList();

            if (dataModel != null && dataModel.Any())
            {
                var jsonValue = dataModel.FirstOrDefault()?.JsonStringValue;
                if (!string.IsNullOrWhiteSpace(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<KeyValuePairDto>>(jsonValue);
                }
            }

            return properties;
        }

        [HttpPost]
        [Route("TaxSummaryReport")]
        public IActionResult TaxSummaryReport()
        {
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "10");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            // Extract form values safely
            string startDate = Request.Form["startDate"].FirstOrDefault();
            string endDate = Request.Form["endDate"].FirstOrDefault();
            string refrenceNo = Request.Form["refrenceNo"].FirstOrDefault();
            string plotNo = Request.Form["plotNo"].FirstOrDefault();
            string sector = Request.Form["sector"].FirstOrDefault();
            string dealerName = Request.Form["dealerName"].FirstOrDefault();
            string phase = Request.Form["Phase"].FirstOrDefault();
            string realStateType = Request.Form["RealStateType"].FirstOrDefault();
            string category = Request.Form["Category"].FirstOrDefault();
            string block = Request.Form["Block"].FirstOrDefault();
            string type = Request.Form["Type"].FirstOrDefault();
            string nature = Request.Form["Nature"].FirstOrDefault();



            var data = GetTaxSummaryReportSP(pageNumber, pageSize, searchValue,
                startDate, endDate, refrenceNo, plotNo, sector, dealerName, phase, realStateType, category, block, type, nature);

            return Ok(data.ToList());
        }


        private List<KeyValuePairDto> GetTaxSummaryReportSP(
            int pageNumber = 1,
            int pageSize = 10,
            string? searchTerm = null,
            string? startDate = null,
            string? endDate = null,
            string? refrenceNo = null,
            string? plotNo = null,
            string? sector = null,
            string? dealerName = null,
            string? phase = null,
            string? realStateType = null,
            string? category = null,
            string? block = null,
            string? type = null,
            string? nature = null
        )
        {
            var properties = new List<KeyValuePairDto>();

            var parameters = new[]
            {
        new SqlParameter("@PageNumber", pageNumber),
        new SqlParameter("@PageSize", pageSize),
        new SqlParameter("@SearchTerm", string.IsNullOrEmpty(searchTerm) ? DBNull.Value : searchTerm),
        new SqlParameter("@StartDate", string.IsNullOrEmpty(startDate) ? DBNull.Value : DateTime.Parse(startDate)),
        new SqlParameter("@EndDate", string.IsNullOrEmpty(endDate) ? DBNull.Value : DateTime.Parse(endDate)),
        new SqlParameter("@RefrenceNo", string.IsNullOrEmpty(refrenceNo) ? DBNull.Value : refrenceNo),
        new SqlParameter("@PlotNo", string.IsNullOrEmpty(plotNo) ? DBNull.Value : plotNo),
        new SqlParameter("@Sector", string.IsNullOrEmpty(sector) ? DBNull.Value : sector),
       new SqlParameter("@DealerName", string.IsNullOrEmpty(dealerName) ? DBNull.Value : dealerName),
        new SqlParameter("@Phase", string.IsNullOrEmpty(phase) ? DBNull.Value : phase),
        new SqlParameter("@RealStateType", string.IsNullOrEmpty(realStateType) ? DBNull.Value : realStateType),
        new SqlParameter("@Category", string.IsNullOrEmpty(category) ? DBNull.Value : category),
        new SqlParameter("@Block", string.IsNullOrEmpty(block) ? DBNull.Value : block),
        new SqlParameter("@Type", string.IsNullOrEmpty(type) ? DBNull.Value : type),
        new SqlParameter("@Nature", string.IsNullOrEmpty(nature) ? DBNull.Value : nature),
    };

            string query = "EXEC [dbo].[sp_GetTaxSummary] " +
                           "@StartDate, @EndDate";

            var dataModel = _db.JsonOutPutModel.FromSqlRaw(query, parameters).ToList();

            if (dataModel != null && dataModel.Any())
            {
                var jsonValue = dataModel.FirstOrDefault()?.JsonStringValue;
                if (!string.IsNullOrWhiteSpace(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<KeyValuePairDto>>(jsonValue);
                }
            }

            return properties;
        }

        [HttpPost]
        [Route("GetAllNDCRequestFilters")]
        public IActionResult GetAllNDCRequestFilters()
        {
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "10");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            // Extract form values safely
            string startDate = Request.Form["startDate"].FirstOrDefault();
            string endDate = Request.Form["endDate"].FirstOrDefault();
            string refrenceNo = Request.Form["refrenceNo"].FirstOrDefault();
            string plotNo = Request.Form["plotNo"].FirstOrDefault();
            string sector = Request.Form["sector"].FirstOrDefault();
            string requestType = Request.Form["requestType"].FirstOrDefault();
            string transferTypeStr = Request.Form["transferType"].FirstOrDefault();
            string dealerName = Request.Form["dealerName"].FirstOrDefault();
            string phase = Request.Form["Phase"].FirstOrDefault();
            string realStateType = Request.Form["RealStateType"].FirstOrDefault();
            string category = Request.Form["Category"].FirstOrDefault();
            string block = Request.Form["Block"].FirstOrDefault();
            string type = Request.Form["Type"].FirstOrDefault();
            string nature = Request.Form["Nature"].FirstOrDefault();

            // Convert TransferType to int? since SP expects int
            int? transferType = null;
            if (int.TryParse(transferTypeStr, out int transferTypeVal))
            {
                transferType = transferTypeVal;
            }

            // Fetch filtered data from SP
            var data = GetAllNDCRequestFiltersSP(pageNumber, pageSize, searchValue,
                startDate, endDate, refrenceNo, plotNo, sector, requestType,
                transferType, dealerName, phase, realStateType, category, block, type, nature);

            // Get total record count using EF to support paging
            var totalRecord = _db.NDCRequestForMember
                .Include(x => x.StockCreation)
                .Where(x => !x.IsDeleted
                    && (string.IsNullOrEmpty(startDate) || x.CreatedOn.Date >= DateTime.Parse(startDate))
                    && (string.IsNullOrEmpty(endDate) || x.CreatedOn.Date <= DateTime.Parse(endDate))
                    && (string.IsNullOrEmpty(refrenceNo) || x.StockCreation.RegistrationNo == refrenceNo)
                    && (string.IsNullOrEmpty(plotNo) || x.StockCreation.PropertyNo == plotNo)
                    && (string.IsNullOrEmpty(sector) || x.StockCreation.PrefixProperty == sector)
                    && (string.IsNullOrEmpty(requestType) || x.NDCRequestType == requestType)
                    && (!transferType.HasValue || x.TransferTypeID == transferType)
                    && (string.IsNullOrEmpty(dealerName) || x.DealerName == dealerName)
                    && (string.IsNullOrEmpty(phase) || x.StockCreation.Phase == phase)
                    && (string.IsNullOrEmpty(realStateType) || x.StockCreation.RealStateType == realStateType)
                    && (string.IsNullOrEmpty(category) || x.StockCreation.Category == category)
                    && (string.IsNullOrEmpty(block) || x.StockCreation.Block == block)
                    && (string.IsNullOrEmpty(type) || x.StockCreation.Type == type)
                    && (string.IsNullOrEmpty(nature) || x.StockCreation.Nature == nature)
                )
                .Count();

            var returnObj = new
            {
                draw = draw,
                recordsTotal = totalRecord,
                recordsFiltered = totalRecord,
                data = data.ToList()
            };

            return Ok(returnObj);
        }


        private List<NDCStateReportDto> GetAllNDCRequestFiltersSP(
            int pageNumber = 1,
            int pageSize = 10,
            string? searchTerm = null,
            string? startDate = null,
            string? endDate = null,
            string? refrenceNo = null,
            string? plotNo = null,
            string? sector = null,
            string? requestType = null,
            int? transferType = null,
            string? dealerName = null,
            string? phase = null,
            string? realStateType = null,
            string? category = null,
            string? block = null,
            string? type = null,
            string? nature = null
        )
        {
            var properties = new List<NDCStateReportDto>();

            var parameters = new[]
            {
        new SqlParameter("@PageNumber", pageNumber),
        new SqlParameter("@PageSize", pageSize),
        new SqlParameter("@SearchTerm", string.IsNullOrEmpty(searchTerm) ? DBNull.Value : searchTerm),
        new SqlParameter("@StartDate", string.IsNullOrEmpty(startDate) ? DBNull.Value : DateTime.Parse(startDate)),
        new SqlParameter("@EndDate", string.IsNullOrEmpty(endDate) ? DBNull.Value : DateTime.Parse(endDate)),
        new SqlParameter("@RefrenceNo", string.IsNullOrEmpty(refrenceNo) ? DBNull.Value : refrenceNo),
        new SqlParameter("@PlotNo", string.IsNullOrEmpty(plotNo) ? DBNull.Value : plotNo),
        new SqlParameter("@Sector", string.IsNullOrEmpty(sector) ? DBNull.Value : sector),
        new SqlParameter("@RequestType", string.IsNullOrEmpty(requestType) ? DBNull.Value : requestType),
        new SqlParameter("@TransferType", transferType.HasValue ? (object)transferType.Value : DBNull.Value),
        new SqlParameter("@DealerName", string.IsNullOrEmpty(dealerName) ? DBNull.Value : dealerName),
        new SqlParameter("@Phase", string.IsNullOrEmpty(phase) ? DBNull.Value : phase),
        new SqlParameter("@RealStateType", string.IsNullOrEmpty(realStateType) ? DBNull.Value : realStateType),
        new SqlParameter("@Category", string.IsNullOrEmpty(category) ? DBNull.Value : category),
        new SqlParameter("@Block", string.IsNullOrEmpty(block) ? DBNull.Value : block),
        new SqlParameter("@Type", string.IsNullOrEmpty(type) ? DBNull.Value : type),
        new SqlParameter("@Nature", string.IsNullOrEmpty(nature) ? DBNull.Value : nature),
    };

            string query = "EXEC [dbo].[sp_GetAllNDCStateFilterReport] " +
                           "@PageNumber, @PageSize, @SearchTerm, @StartDate, @EndDate, @RefrenceNo, " +
                           "@PlotNo, @Sector, @RequestType, @TransferType, @DealerName, " +
                           "@Phase, @RealStateType, @Category, @Block, @Type, @Nature";

            var dataModel = _db.JsonOutPutModel.FromSqlRaw(query, parameters).ToList();

            if (dataModel != null && dataModel.Any())
            {
                var jsonValue = dataModel.FirstOrDefault()?.JsonStringValue;
                if (!string.IsNullOrWhiteSpace(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<NDCStateReportDto>>(jsonValue);
                }
            }

            return properties;
        }


        [HttpPost]
        [Route("GetPropertyStatusReport")]
        public IActionResult GetPropertyStatusReport()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            // Retrieve additional form values
            string startDate = Request.Form["startDate"].FirstOrDefault();
            string endDate = Request.Form["endDate"].FirstOrDefault();
            string refrenceNo = Request.Form["refrenceNo"].FirstOrDefault();
            string plotNo = Request.Form["plotNo"].FirstOrDefault();
            string sector = Request.Form["sector"].FirstOrDefault();
            string propertyStatus = Request.Form["propertyStatus"].FirstOrDefault();
            string plotSize = Request.Form["plotSize"].FirstOrDefault();
            string dealerName = Request.Form["dealerName"].FirstOrDefault();
            string phase = Request.Form["Phase"].FirstOrDefault();
            string realStateType = Request.Form["RealStateType"].FirstOrDefault();
            string category = Request.Form["Category"].FirstOrDefault();
            string block = Request.Form["Block"].FirstOrDefault();
            string type = Request.Form["Type"].FirstOrDefault();
            string nature = Request.Form["Nature"].FirstOrDefault();

            var data = GetPropertyStatusReportSP(pageNumber, pageSize, searchValue, startDate, endDate, refrenceNo, plotNo, sector, propertyStatus, plotSize, dealerName, phase, realStateType, category, block, type, nature);

            totalRecord = _db.StockCreations
                           .Include(x=>x.Dealer)
                           .Where(x => !x.is_deleted &&
                               (string.IsNullOrEmpty(startDate) || x.Created_at.Date >= DateTime.Parse(startDate)) &&
                               (string.IsNullOrEmpty(endDate) || x.Created_at.Date <= DateTime.Parse(endDate)) &&
                               (string.IsNullOrEmpty(refrenceNo) || x.RegistrationNo == refrenceNo) &&
                               (string.IsNullOrEmpty(plotNo) || x.PropertyNo == plotNo) &&
                               (string.IsNullOrEmpty(sector) || x.PrefixProperty == sector) &&
                               (string.IsNullOrEmpty(propertyStatus) || x.PropertyStatus == propertyStatus) 
                                 && (string.IsNullOrEmpty(phase) || x.Phase == phase)
                                 && (string.IsNullOrEmpty(realStateType) || x.RealStateType == realStateType)
                                 && (string.IsNullOrEmpty(category) || x.Category == category)
                                 && (string.IsNullOrEmpty(block) || x.Block == block)
                                 && (string.IsNullOrEmpty(type) || x.Type == type)
                                 && (string.IsNullOrEmpty(nature) || x.Nature == nature))
                           .Count();


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


        private List<PropertyInfoDTO> GetPropertyStatusReportSP(
          int pageNumber = 1,
          int pageSize = 10,
          string? searchTerm = "",
          string? startDate = null,
          string? endDate = null,
          string? refrenceNo = null,
          string? plotNo = null,
          string? sector = null,
          string? propertyStatus = null,
          string? plotSize = null,
          string? dealerName = null,
          string? phase = null,
          string? realStateType = null,
          string? category = null,
          string? block = null,
          string? type = null,
          string? nature = null)
        {
            var properties = new List<PropertyInfoDTO>();


        var parameters = new[]
           {
               new SqlParameter("@PageNumber", pageNumber),
               new SqlParameter("@PageSize", pageSize),
               new SqlParameter("@SearchTerm", string.IsNullOrEmpty(searchTerm) ? "" : searchTerm),
               new SqlParameter("@StartDate", string.IsNullOrEmpty(startDate) ? DBNull.Value : DateTime.Parse(startDate)),
               new SqlParameter("@EndDate", string.IsNullOrEmpty(endDate) ? DBNull.Value : DateTime.Parse(endDate)),
               new SqlParameter("@RefrenceNo", string.IsNullOrEmpty(refrenceNo) ? "" : refrenceNo),
               new SqlParameter("@PlotNo", string.IsNullOrEmpty(plotNo) ? "" : plotNo),
               new SqlParameter("@Sector", string.IsNullOrEmpty(sector) ? "" : sector),
               new SqlParameter("@PropertyStatus", string.IsNullOrEmpty(propertyStatus) ? "" : propertyStatus),
               new SqlParameter("@PlotSize", string.IsNullOrEmpty(plotSize) ? "" : plotSize),
               new SqlParameter("@DealerName", string.IsNullOrEmpty(dealerName) ? "" : dealerName),
               new SqlParameter("@Phase", string.IsNullOrEmpty(phase) ? DBNull.Value : phase),
               new SqlParameter("@RealStateType", string.IsNullOrEmpty(realStateType) ? DBNull.Value : realStateType),
               new SqlParameter("@Category", string.IsNullOrEmpty(category) ? DBNull.Value : category),
               new SqlParameter("@Block", string.IsNullOrEmpty(block) ? DBNull.Value : block),
               new SqlParameter("@Type", string.IsNullOrEmpty(type) ? DBNull.Value : type),
               new SqlParameter("@Nature", string.IsNullOrEmpty(nature) ? DBNull.Value : nature),
            };

            var query = "EXEC [dbo].[sp_GetPropertyStatusReportSP] " +
             "@PageNumber, @PageSize, @SearchTerm, @StartDate, @EndDate, " +
             "@Sector, @RefrenceNo, @PlotNo, @PropertyStatus, @PlotSize, @DealerName, " +
             "@Phase, @RealStateType, @Category, @Block, @Type, @Nature";


            var dataModel = _db.JsonOutPutModel.FromSqlRaw(query, parameters).ToList();

            if (dataModel != null && dataModel.Any())
            {
                var jsonValue = dataModel.FirstOrDefault()?.JsonStringValue;
                if (!String.IsNullOrEmpty(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<PropertyInfoDTO>>(jsonValue);
                }
            }

            return properties;
        }

        [HttpPost]
        [Route("PropertyStatusSummaryReport")]
        public IActionResult PropertyStatusSummaryReport()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
          

            // Retrieve additional form values
            string startDate = Request.Form["startDate"].FirstOrDefault();
            string endDate = Request.Form["endDate"].FirstOrDefault();
            string refrenceNo = Request.Form["refrenceNo"].FirstOrDefault();
            string plotNo = Request.Form["plotNo"].FirstOrDefault();
            string sector = Request.Form["sector"].FirstOrDefault();
            string propertyStatus = Request.Form["propertyStatus"].FirstOrDefault();
            string dealerName = Request.Form["dealerName"].FirstOrDefault();

            var data = GetPropertyStatusSummaryReportSP(searchValue, startDate, endDate, refrenceNo, plotNo, sector, propertyStatus, dealerName);

           
            return Ok(data.ToList());
        }


        private List<KeyValuePairDto> GetPropertyStatusSummaryReportSP(
    string? searchTerm = "",
    string? startDate = null,
    string? endDate = null,
    string? refrenceNo = null,
    string? plotNo = null,
    string? sector = null,
    string? propertyStatus = null,
    string? dealerName = null)
        {
            var properties = new List<KeyValuePairDto>();

            var parameters = new[]
            {
        new SqlParameter("@SearchTerm", searchTerm ?? ""),
        new SqlParameter("@StartDate", string.IsNullOrEmpty(startDate) ? DBNull.Value : DateTime.Parse(startDate)),
        new SqlParameter("@EndDate", string.IsNullOrEmpty(endDate) ? DBNull.Value : DateTime.Parse(endDate)),
        new SqlParameter("@Sector", sector ?? ""),
        new SqlParameter("@RefrenceNo", refrenceNo ?? ""),
        new SqlParameter("@PlotNo", plotNo ?? ""),
        new SqlParameter("@PropertyStatus", propertyStatus ?? ""),
        new SqlParameter("@DealerName", dealerName ?? "")
    };

            var query = @"EXEC [dbo].[sp_PropertyStatusSummaryReportSP] 
                  @SearchTerm, @StartDate, @EndDate, 
                  @Sector, @RefrenceNo, @PlotNo, @PropertyStatus, @DealerName";

            var dataModel = _db.JsonOutPutModel
                .FromSqlRaw(query, parameters)
                .ToList();

            if (dataModel != null && dataModel.Any())
            {
                var jsonValue = dataModel.FirstOrDefault()?.JsonStringValue;
                if (!string.IsNullOrEmpty(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<KeyValuePairDto>>(jsonValue);
                }
            }

            return properties;
        }


        [HttpPost]
        [Route("GetAllocationReport")]
        public IActionResult GetAllocationReport()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            // Retrieve additional form values
            string startDate = Request.Form["startDate"].FirstOrDefault();
            string endDate = Request.Form["endDate"].FirstOrDefault();
            string refrenceNo = Request.Form["refrenceNo"].FirstOrDefault();
            string plotNo = Request.Form["plotNo"].FirstOrDefault();
            string sector = Request.Form["sector"].FirstOrDefault();
            string propertyStatus = Request.Form["PropertyStatus"].FirstOrDefault();
            string dealerName = Request.Form["dealerName"].FirstOrDefault();

            var data = GetAllocationReportSP(pageNumber, pageSize, searchValue, startDate, endDate, refrenceNo, plotNo, sector, propertyStatus, dealerName);

            totalRecord = _db.PreSale
                           .Include(x => x.StockCreation)
                           .Include(x => x.Dealer)
                           .Where(x => !x.IsDeleted && x.StockCreation.Almt == "10" &&
                               (string.IsNullOrEmpty(startDate) || x.CreatedOn.Date >= DateTime.Parse(startDate)) &&
                               (string.IsNullOrEmpty(endDate) || x.CreatedOn.Date <= DateTime.Parse(endDate)) &&
                               (string.IsNullOrEmpty(refrenceNo) || x.StockCreation.RegistrationNo == refrenceNo) &&
                               (string.IsNullOrEmpty(plotNo) || x.StockCreation.PropertyNo == plotNo) &&
                               (string.IsNullOrEmpty(sector) || x.StockCreation.PrefixProperty == sector) &&
                               (string.IsNullOrEmpty(propertyStatus) || x.StockCreation.PropertyStatus == propertyStatus) &&
                               (string.IsNullOrEmpty(dealerName) || x.Dealer.PrincipalOwner == dealerName))
                           .Count();


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


        private List<PropertyInfoDTO> GetAllocationReportSP(
          int pageNumber = 1,
          int pageSize = 10,
          string? searchTerm = "",
          string? startDate = null,
          string? endDate = null,
          string? refrenceNo = null,
          string? plotNo = null,
          string? sector = null,
          string? propertyStatus = null,
          string? dealerName = null)
        {
            var properties = new List<PropertyInfoDTO>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            var startDateParam = new SqlParameter("@StartDate", startDate ?? (object)DBNull.Value);
            var endDateParam = new SqlParameter("@EndDate", endDate ?? (object)DBNull.Value);
            var refrenceNoParam = new SqlParameter("@RefrenceNo", refrenceNo ?? (object)DBNull.Value);
            var plotNoParam = new SqlParameter("@PlotNo", plotNo ?? (object)DBNull.Value);
            var sectorParam = new SqlParameter("@Sector", sector ?? (object)DBNull.Value);
            var propertyStatusParam = new SqlParameter("@PropertyStatus", propertyStatus ?? (object)DBNull.Value);
            var dealerNameParam = new SqlParameter("@DealerName", dealerName ?? (object)DBNull.Value);

            var query = $"EXEC [dbo].[sp_GetAllocationReportSP] " +
                        "@PageNumber = @PageNumber, @PageSize = @PageSize, @SearchTerm = @SearchTerm, " +
                        "@StartDate = @StartDate, @EndDate = @EndDate, @RefrenceNo = @RefrenceNo, " +
                        "@PlotNo = @PlotNo, @Sector = @Sector, @PropertyStatus = @PropertyStatus,@DealerName = @DealerName";

            var DataModel = _db.JsonOutPutModel.FromSqlRaw(query,
                pageNumberParam, pageSizeParam, searchTermParam, startDateParam, endDateParam, refrenceNoParam, plotNoParam, sectorParam, propertyStatusParam, dealerNameParam).ToList();

            if (DataModel != null && DataModel.Any())
            {
                var jsonValue = DataModel.FirstOrDefault()?.JsonStringValue;
                if (!String.IsNullOrEmpty(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<PropertyInfoDTO>>(jsonValue);
                }
            }

            return properties;
        }
        [HttpPost]
        [Route("GetMemberReport")]
        public IActionResult GetMemberReport()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            // Retrieve additional form values
            string refrenceNo = Request.Form["refrenceNo"].FirstOrDefault();
            string plotNo = Request.Form["plotNo"].FirstOrDefault();
            string propertyStatus = Request.Form["propertyStatus"].FirstOrDefault();
            string city = Request.Form["city"].FirstOrDefault();
            string mobile = Request.Form["Mobile"].FirstOrDefault();
            string address = Request.Form["Address"].FirstOrDefault();
            string phase = Request.Form["Phase"].FirstOrDefault();
            string realStateType = Request.Form["RealStateType"].FirstOrDefault();
            string category = Request.Form["Category"].FirstOrDefault();
            string block = Request.Form["Block"].FirstOrDefault();
            string type = Request.Form["Type"].FirstOrDefault();
            string nature = Request.Form["Nature"].FirstOrDefault();
            string dealerName = Request.Form["dealerName"].FirstOrDefault();
            string country = Request.Form["Country"].FirstOrDefault();
            string cnic = Request.Form["cnic"].FirstOrDefault();
            string almt = Request.Form["almt"].FirstOrDefault();

            var data = GetMemberReportSP(pageNumber, pageSize, searchValue,  refrenceNo, plotNo,  propertyStatus, city,mobile,address, phase, realStateType, category, block, type, nature, dealerName,country,cnic,almt);

            totalRecord = _db.StockCreations
                           .Include(x => x.MemberProfile)
                           .Include(x => x.Dealer)
                           .Where(x => !x.is_deleted &&
                               (string.IsNullOrEmpty(refrenceNo) || x.RegistrationNo == refrenceNo) &&
                               (string.IsNullOrEmpty(plotNo) || x.PropertyNo == plotNo) &&
                               (string.IsNullOrEmpty(propertyStatus) || x.PropertyStatus == propertyStatus) &&
                               (string.IsNullOrEmpty(city) || x.MemberProfile.CityOfResidence == city) &&
                               (string.IsNullOrEmpty(mobile) || x.MemberProfile.Mobile == mobile) &&
                               (string.IsNullOrEmpty(address) || x.MemberProfile.CurrentAddress == address)
                                 && (string.IsNullOrEmpty(phase) || x.Phase == phase)
                                 && (string.IsNullOrEmpty(realStateType) || x.RealStateType == realStateType)
                                 && (string.IsNullOrEmpty(category) || x.Category == category)
                                 && (string.IsNullOrEmpty(block) || x.Block == block)
                                 && (string.IsNullOrEmpty(type) || x.Type == type)
                                 && (string.IsNullOrEmpty(nature) || x.Nature == nature) 
                                 && (string.IsNullOrEmpty(country) || x.MemberProfile.CountryOfResidence == country) 
                                 && (string.IsNullOrEmpty(cnic) || x.MemberProfile.Cnic == cnic) 
                                 && (string.IsNullOrEmpty(almt) || x.Almt == almt) &&
                               (string.IsNullOrEmpty(dealerName) || x.Dealer.PrincipalOwner == dealerName))
                           .Count();


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


        private List<PropertyInfoDTO> GetMemberReportSP(
          int pageNumber = 1,
          int pageSize = 10,
          string? searchTerm = "",
          string? refrenceNo = null,
          string? plotNo = null,
          string? propertyStatus = null,
          string? city = null,
          string? mobile = null,
          string? address = null,
           string? phase = null,
            string? realStateType = null,
            string? category = null,
            string? block = null,
            string? type = null,
            string? nature = null,
          string? dealerName = null,
          string? country = null,
          string? cnic = null,
          string? almt = null
            )
        {
            var properties = new List<PropertyInfoDTO>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            //var startDateParam = new SqlParameter("@StartDate", startDate ?? (object)DBNull.Value);
            //var endDateParam = new SqlParameter("@EndDate", endDate ?? (object)DBNull.Value);
            var refrenceNoParam = new SqlParameter("@RefrenceNo", refrenceNo ?? (object)DBNull.Value);
            var plotNoParam = new SqlParameter("@PlotNo", plotNo ?? (object)DBNull.Value);
            var propertyStatusParam = new SqlParameter("@PropertyStatus", propertyStatus ?? (object)DBNull.Value);
            var cityParam = new SqlParameter("@City", string.IsNullOrEmpty(city) ? (object)DBNull.Value : city);
            var mobileParam = new SqlParameter("@Mobile", string.IsNullOrEmpty(mobile) ? (object)DBNull.Value : mobile);
            var addressParam = new SqlParameter("@Address", string.IsNullOrEmpty(address) ? (object)DBNull.Value : address);
            var phaseParam = new SqlParameter("@Phase", string.IsNullOrEmpty(phase) ? DBNull.Value : phase);
            var realStateTypeParam = new SqlParameter("@RealStateType", string.IsNullOrEmpty(realStateType) ? DBNull.Value : realStateType);
            var categoryParam = new SqlParameter("@Category", string.IsNullOrEmpty(category) ? DBNull.Value : category);
            var blockParam = new SqlParameter("@Block", string.IsNullOrEmpty(block) ? DBNull.Value : block);
            var typeParam = new SqlParameter("@Type", string.IsNullOrEmpty(type) ? DBNull.Value : type);
            var natureParam = new SqlParameter("@Nature", string.IsNullOrEmpty(nature) ? DBNull.Value : nature);
            var dealerNameParam = new SqlParameter("@DealerName", string.IsNullOrEmpty(dealerName) ? (object)DBNull.Value : dealerName);
            var countryParam = new SqlParameter("@Country", string.IsNullOrEmpty(country) ? (object)DBNull.Value : country);
            var cnicParam = new SqlParameter("@Cnic", string.IsNullOrEmpty(cnic) ? (object)DBNull.Value : cnic);
            var almtParam = new SqlParameter("@Almt", string.IsNullOrEmpty(almt) ? (object)DBNull.Value : almt);

            var query = @"EXEC [dbo].[sp_GetMemberReportSP] 
                        @PageNumber = @PageNumber, 
                        @PageSize = @PageSize, 
                        @SearchTerm = @SearchTerm, 
                        
                        @RefrenceNo = @RefrenceNo, 
                        @PlotNo = @PlotNo, 
                        @PropertyStatus = @PropertyStatus, 
                        @City = @City,
                         @Mobile = @Mobile,
                         @Address = @Address,
                        @DealerName = @DealerName,
                        @Phase = @Phase,
                        @RealStateType = @RealStateType,
                        @Category = @Category,
                        @Block = @Block,
                        @Type = @Type,
                        @Nature = @Nature,
                        @Country = @Country,
                        @Cnic = @Cnic,
                        @Almt = @Almt";

            var DataModel = _db.JsonOutPutModel.FromSqlRaw(query,
                pageNumberParam,
                pageSizeParam,
                searchTermParam,
                refrenceNoParam,
                plotNoParam,
                propertyStatusParam,
                cityParam,
                 mobileParam,
                  addressParam,
                dealerNameParam,
                phaseParam,
                realStateTypeParam,
                categoryParam,
                blockParam,
                typeParam,
                natureParam,
                countryParam,
                cnicParam,
                almtParam
            ).ToList();

            if (DataModel != null && DataModel.Any())
            {
                var jsonValue = DataModel.FirstOrDefault()?.JsonStringValue;
                if (!String.IsNullOrEmpty(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<PropertyInfoDTO>>(jsonValue);
                }
            }

            return properties;
        }

        [HttpPost]
        [Route("MemberSummaryReport")]
        public IActionResult MemberSummaryReport()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "1");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            // Retrieve additional form values
            string startDate = Request.Form["startDate"].FirstOrDefault();
            string endDate = Request.Form["endDate"].FirstOrDefault();
            string refrenceNo = Request.Form["refrenceNo"].FirstOrDefault();
            string plotNo = Request.Form["plotNo"].FirstOrDefault();
            string sector = Request.Form["sector"].FirstOrDefault();
            string propertyStatus = Request.Form["propertyStatus"].FirstOrDefault();
            string city = Request.Form["city"].FirstOrDefault();
            string phase = Request.Form["Phase"].FirstOrDefault();
            string realStateType = Request.Form["RealStateType"].FirstOrDefault();
            string category = Request.Form["Category"].FirstOrDefault();
            string block = Request.Form["Block"].FirstOrDefault();
            string type = Request.Form["Type"].FirstOrDefault();
            string nature = Request.Form["Nature"].FirstOrDefault();
            string dealerName = Request.Form["dealerName"].FirstOrDefault();
            string country = Request.Form["country"].FirstOrDefault();
            string cnic = Request.Form["cnic"].FirstOrDefault();
            string almt = Request.Form["almt"].FirstOrDefault();


            var data = GetMemberSummaryReportSP(pageNumber, pageSize, searchValue, startDate, endDate, refrenceNo, plotNo, sector, propertyStatus, city, phase, realStateType, category, block, type, nature, dealerName,country,cnic,almt);


            return Ok(data.ToList());
        }


        private List<KeyValuePairDto> GetMemberSummaryReportSP(
      int pageNumber = 1,
      int pageSize = 10,
      string? searchTerm = "",
      string? startDate = null,
      string? endDate = null,
      string? refrenceNo = null,
      string? plotNo = null,
      string? sector = null,
      string? propertyStatus = null,
      string? city = null,
       string? phase = null,
            string? realStateType = null,
            string? category = null,
            string? block = null,
            string? type = null,
            string? nature = null,
      string? dealerName = null,
       string? country = null,
          string? cnic = null,
          string? almt = null
            )
        {
            var properties = new List<KeyValuePairDto>();

            // Handle nulls and ensure DBNull is used where appropriate
            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", (object)(searchTerm ?? "") ?? DBNull.Value);
            var startDateParam = new SqlParameter("@StartDate", (object)(startDate ?? (object)DBNull.Value));
            var endDateParam = new SqlParameter("@EndDate", (object)(endDate ?? (object)DBNull.Value));
            var refrenceNoParam = new SqlParameter("@RefrenceNo", (object)(refrenceNo ?? "") ?? DBNull.Value);
            var plotNoParam = new SqlParameter("@PlotNo", (object)(plotNo ?? "") ?? DBNull.Value);
            var sectorParam = new SqlParameter("@Sector", (object)(sector ?? "") ?? DBNull.Value);
            var propertyStatusParam = new SqlParameter("@PropertyStatus", (object)(propertyStatus ?? "") ?? DBNull.Value);
            var cityParam = new SqlParameter("@City", (object)(city ?? "") ?? DBNull.Value);
            var phaseParam = new SqlParameter("@Phase", string.IsNullOrEmpty(phase) ? DBNull.Value : phase);
            var realStateTypeParam = new SqlParameter("@RealStateType", string.IsNullOrEmpty(realStateType) ? DBNull.Value : realStateType);
            var categoryParam = new SqlParameter("@Category", string.IsNullOrEmpty(category) ? DBNull.Value : category);
            var blockParam = new SqlParameter("@Block", string.IsNullOrEmpty(block) ? DBNull.Value : block);
            var typeParam = new SqlParameter("@Type", string.IsNullOrEmpty(type) ? DBNull.Value : type);
            var natureParam = new SqlParameter("@Nature", string.IsNullOrEmpty(nature) ? DBNull.Value : nature);
            var dealerNameParam = new SqlParameter("@DealerName", dealerName ?? (object)DBNull.Value);
            var countryParam = new SqlParameter("@Country", country ?? (object)DBNull.Value);
            var cnicParam = new SqlParameter("@Cnic", cnic ?? (object)DBNull.Value);
            var almtParam = new SqlParameter("@Almt", almt ?? (object)DBNull.Value);

            var query = @"EXEC [dbo].[sp_GetMemberSummary] 
                        @PageNumber = @PageNumber, 
                        @PageSize = @PageSize, 
                        @SearchTerm = @SearchTerm, 
                        @StartDate = @StartDate, 
                        @EndDate = @EndDate, 
                        @RefrenceNo = @RefrenceNo, 
                        @PlotNo = @PlotNo, 
                        @Sector = @Sector, 
                        @PropertyStatus = @PropertyStatus, 
                        @City = @City,
                        @DealerName = @DealerName,
                        @Phase = @Phase,
                        @RealStateType = @RealStateType,
                        @Category = @Category,
                        @Block = @Block,
                        @Type = @Type,
                        @Nature = @Nature,
                        @Country = @Country,
                        @Cnic = @Cnic,
                        @Almt = @Almt";

            var dataModel = _db.JsonOutPutModel.FromSqlRaw(query,
                pageNumberParam,
                pageSizeParam,
                searchTermParam,
                startDateParam,
                endDateParam,
                refrenceNoParam,
                plotNoParam,
                sectorParam,
                propertyStatusParam,
                cityParam,
                dealerNameParam,
                phaseParam,
                realStateTypeParam,
                categoryParam,
                blockParam,
                typeParam,
                natureParam,
                countryParam,
                cnicParam,
                almtParam
            ).ToList();

            if (dataModel != null && dataModel.Any())
            {
                var jsonValue = dataModel.FirstOrDefault()?.JsonStringValue;
                if (!string.IsNullOrWhiteSpace(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<KeyValuePairDto>>(jsonValue);
                }
            }

            return properties;
        }

        [HttpPost]
        [Route("GetDealerReport")]
        public IActionResult GetDealerReport()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            // Retrieve additional form values
            string startDate = Request.Form["startDate"].FirstOrDefault();
            string endDate = Request.Form["endDate"].FirstOrDefault();
            string city = Request.Form["city"].FirstOrDefault();
            string dealerName = Request.Form["dealerName"].FirstOrDefault();
            string DealerStatus = Request.Form["DealerStatus"].FirstOrDefault();

            var data = GetDealerReportSP(pageNumber, pageSize, searchValue, startDate, endDate,city, dealerName, DealerStatus);

            totalRecord = _db.Dealers
                           .Where(x => !x.IsDeleted &&
                               (string.IsNullOrEmpty(startDate) || x.CreatedOn.Date >= DateTime.Parse(startDate)) &&
                               (string.IsNullOrEmpty(endDate) || x.CreatedOn.Date <= DateTime.Parse(endDate)) &&
                               (string.IsNullOrEmpty(dealerName) || x.PrincipalOwner == dealerName) &&
                               (string.IsNullOrEmpty(DealerStatus) || x.DealerStatus == DealerStatus) &&
                               (string.IsNullOrEmpty(city) || x.City == city))
                           .Count();


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

        [HttpPost]
        [Route("DealerSummaryReport")]
        public IActionResult DealerSummaryReport()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "1");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            // Retrieve additional form values
            string startDate = Request.Form["startDate"].FirstOrDefault();
            string endDate = Request.Form["endDate"].FirstOrDefault();
            string city = Request.Form["city"].FirstOrDefault();
            string dealerName = Request.Form["dealerName"].FirstOrDefault();
            string DealerStatus = Request.Form["DealerStatus"].FirstOrDefault();

            var data = GetDealerSummaryReportSP(pageNumber, pageSize, searchValue, startDate, endDate,city, dealerName, DealerStatus);


            return Ok(data.ToList());
        }


        private List<KeyValuePairDto> GetDealerSummaryReportSP(
      int pageNumber = 1,
      int pageSize = 10,
      string? searchTerm = "",
      string? startDate = null,
      string? endDate = null,
      
      string? city = null,
      string? dealerName = null,
      string? DealerStatus = null
      )
        {
            var properties = new List<KeyValuePairDto>();

            // Handle nulls and ensure DBNull is used where appropriate
            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", (object)(searchTerm ?? "") ?? DBNull.Value);
            var startDateParam = new SqlParameter("@StartDate", (object)(startDate ?? (object)DBNull.Value));
            var endDateParam = new SqlParameter("@EndDate", (object)(endDate ?? (object)DBNull.Value));
            var cityParam = new SqlParameter("@City", (object)(city ?? "") ?? DBNull.Value);
            var dealerNameParam = new SqlParameter("@DealerName", (object)(dealerName ?? "") ?? DBNull.Value);
            var DealerStatusParam = new SqlParameter("@DealerStatus", (object)(DealerStatus ?? "") ?? DBNull.Value);

            var query = "EXEC [dbo].[sp_GetDealerSummary] " +
                        "@PageNumber, @PageSize, @SearchTerm, @StartDate, @EndDate, " +
                        "@City, @DealerName,@DealerStatus";

            var dataModel = _db.JsonOutPutModel.FromSqlRaw(query,
                pageNumberParam,
                pageSizeParam,
                searchTermParam,
                startDateParam,
                endDateParam,
                cityParam,
                dealerNameParam,
                DealerStatusParam
            ).ToList();

            if (dataModel != null && dataModel.Any())
            {
                var jsonValue = dataModel.FirstOrDefault()?.JsonStringValue;
                if (!string.IsNullOrWhiteSpace(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<KeyValuePairDto>>(jsonValue);
                }
            }

            return properties;
        }


        private List<PropertyInfoDTO> GetDealerReportSP(
          int pageNumber = 1,
          int pageSize = 10,
          string? searchTerm = "",
          string? startDate = null,
          string? endDate = null,
          string? city = null,
          string? dealerName = null,
          string? DealerStatus = null
          )
        {
            var properties = new List<PropertyInfoDTO>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            var startDateParam = new SqlParameter("@StartDate", startDate ?? (object)DBNull.Value);
            var endDateParam = new SqlParameter("@EndDate", endDate ?? (object)DBNull.Value);
             var cityParam = new SqlParameter("@City", city ?? (object)DBNull.Value);
            var dealerNameParam = new SqlParameter("@DealerName", dealerName ?? (object)DBNull.Value);
            var DealerStatusParam = new SqlParameter("@DealerStatus", DealerStatus ?? (object)DBNull.Value);

            var query = $"EXEC [dbo].[sp_GetDealerReportSP] " +
                        "@PageNumber = @PageNumber, @PageSize = @PageSize, @SearchTerm = @SearchTerm, " +
                        "@StartDate = @StartDate, @EndDate = @EndDate,@City = @City,@DealerName = @DealerName, @DealerStatus = @DealerStatus";

            var DataModel = _db.JsonOutPutModel.FromSqlRaw(query,
                pageNumberParam, pageSizeParam, searchTermParam, startDateParam, endDateParam, cityParam, dealerNameParam, DealerStatusParam).ToList();

            if (DataModel != null && DataModel.Any())
            {
                var jsonValue = DataModel.FirstOrDefault()?.JsonStringValue;
                if (!String.IsNullOrEmpty(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<PropertyInfoDTO>>(jsonValue);
                }
            }

            return properties;
        }


        [HttpPost]
        [Route("GetAllFindModeTransferSetFilters")]
        public IActionResult GetAllFindModeTransferSetFilters()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            string startDate = Request.Form["startDate"].FirstOrDefault();
            string endDate = Request.Form["endDate"].FirstOrDefault();
            string refrenceNo = Request.Form["refrenceNo"].FirstOrDefault();
            string plotNo = Request.Form["plotNo"].FirstOrDefault();
            string sector = Request.Form["sector"].FirstOrDefault();
            string plotSize = Request.Form["plotSize"].FirstOrDefault();
            string dealerName = Request.Form["dealerName"].FirstOrDefault();
            string phase = Request.Form["Phase"].FirstOrDefault();
            string realStateType = Request.Form["RealStateType"].FirstOrDefault();
            string category = Request.Form["Category"].FirstOrDefault();
            string block = Request.Form["Block"].FirstOrDefault();
            string type = Request.Form["Type"].FirstOrDefault();
            string nature = Request.Form["Nature"].FirstOrDefault();

            var data = GetAllFindModeTransferSetFiltersSP(pageNumber, pageSize, searchValue, startDate, endDate, refrenceNo, plotNo, sector, plotSize, dealerName, phase, realStateType, category, block, type, nature);

             totalRecord = _db.TransferSetReceivings
                              .Include(x => x.StockCreation)
                              .Where(x => !x.IsDeleted && x.IsActive
                                  && (string.IsNullOrEmpty(sector) || x.StockCreation.PrefixProperty == sector)
                                  && (string.IsNullOrEmpty(startDate) || x.CreatedOn.Date >= DateTime.Parse(startDate))
                                  && (string.IsNullOrEmpty(endDate) || x.CreatedOn.Date <= DateTime.Parse(endDate))
                                  && (string.IsNullOrEmpty(refrenceNo) || x.StockCreation.RegistrationNo == refrenceNo)
                                  && (string.IsNullOrEmpty(plotNo) || x.StockCreation.PropertyNo == plotNo)
                                  && (string.IsNullOrEmpty(plotSize) || x.StockCreation.ActualSize == plotSize)
                                  && (string.IsNullOrEmpty(dealerName) || x.DealerName == dealerName)
                                  && (string.IsNullOrEmpty(phase) || x.StockCreation.Phase == phase)
                                  && (string.IsNullOrEmpty(realStateType) || x.StockCreation.RealStateType == realStateType)
                                  && (string.IsNullOrEmpty(category) || x.StockCreation.Category == category)
                                  && (string.IsNullOrEmpty(block) || x.StockCreation.Block == block)
                                  && (string.IsNullOrEmpty(type) || x.StockCreation.Type == type)
                                  && (string.IsNullOrEmpty(nature) || x.StockCreation.Nature == nature)
                              )
                              .Count();

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


        private List<TransferSetReceivingDto> GetAllFindModeTransferSetFiltersSP(
          int pageNumber = 1,
          int pageSize = 10,
          string? searchTerm = "",
          string? startDate = null,
          string? endDate = null,
          string? refrenceNo = null,
          string? plotNo = null,
          string? sector = null,
          string? plotSize = null,
          string? dealerName = null,
          string? phase = null,
          string? realStateType = null,
          string? category = null,
          string? block = null,
          string? type = null,
          string? nature = null)

        {
            var properties = new List<TransferSetReceivingDto>();

            var parameters = new[]
            {
               new SqlParameter("@PageNumber", pageNumber),
               new SqlParameter("@PageSize", pageSize),
               new SqlParameter("@SearchTerm", string.IsNullOrEmpty(searchTerm) ? "" : searchTerm),
               new SqlParameter("@StartDate", string.IsNullOrEmpty(startDate) ? DBNull.Value : DateTime.Parse(startDate)),
               new SqlParameter("@EndDate", string.IsNullOrEmpty(endDate) ? DBNull.Value : DateTime.Parse(endDate)),
               new SqlParameter("@Sector", string.IsNullOrEmpty(sector) ? "" : sector),
               new SqlParameter("@RefrenceNo", string.IsNullOrEmpty(refrenceNo) ? "" : refrenceNo),
               new SqlParameter("@PlotNo", string.IsNullOrEmpty(plotNo) ? "" : plotNo),
               new SqlParameter("@PlotSize", string.IsNullOrEmpty(plotSize) ? "" : plotSize), // <--- ADD THIS
               new SqlParameter("@DealerName", string.IsNullOrEmpty(dealerName) ? "" : dealerName),
               new SqlParameter("@Phase", string.IsNullOrEmpty(phase) ? DBNull.Value : phase),
               new SqlParameter("@RealStateType", string.IsNullOrEmpty(realStateType) ? DBNull.Value : realStateType),
               new SqlParameter("@Category", string.IsNullOrEmpty(category) ? DBNull.Value : category),
               new SqlParameter("@Block", string.IsNullOrEmpty(block) ? DBNull.Value : block),
               new SqlParameter("@Type", string.IsNullOrEmpty(type) ? DBNull.Value : type),
               new SqlParameter("@Nature", string.IsNullOrEmpty(nature) ? DBNull.Value : nature),
           };

            string query = "EXEC [dbo].[sp_GetAllFindModeTransferSetFilters] " +
                           "@PageNumber, @PageSize, @SearchTerm, @StartDate, @EndDate, @Sector, " +
                           "@RefrenceNo, @PlotNo, @PlotSize, @DealerName, @Phase, @RealStateType, " +
                           "@Category, @Block, @Type, @Nature";

            var dataModel = _db.JsonOutPutModel.FromSqlRaw(query, parameters).ToList();


            if (dataModel != null && dataModel.Any())
            {
                var jsonValue = dataModel.FirstOrDefault()?.JsonStringValue;
                if (!String.IsNullOrEmpty(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<TransferSetReceivingDto>>(jsonValue);
                }
            }

            return properties;
        }


        #endregion

        #region Tax Report

        [HttpPost]
        [Route("GetTaxReport")]
        public IActionResult GetTaxReport()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            string startDate = Request.Form["startDate"].FirstOrDefault();
            string endDate = Request.Form["endDate"].FirstOrDefault();
            string refrenceNo = Request.Form["refrenceNo"].FirstOrDefault();
            string plotNo = Request.Form["plotNo"].FirstOrDefault();
            string sector = Request.Form["sector"].FirstOrDefault();
            string plotSize = Request.Form["plotSize"].FirstOrDefault();
            string dealerName = Request.Form["dealerName"].FirstOrDefault();
            string phase = Request.Form["Phase"].FirstOrDefault();
            string realStateType = Request.Form["RealStateType"].FirstOrDefault();
            string category = Request.Form["Category"].FirstOrDefault();
            string block = Request.Form["Block"].FirstOrDefault();
            string type = Request.Form["Type"].FirstOrDefault();
            string nature = Request.Form["Nature"].FirstOrDefault();

            var data = GetTaxReportSP(pageNumber, pageSize, searchValue, startDate, endDate, refrenceNo, plotNo, sector, plotSize, dealerName, phase, realStateType, category, block, type, nature);

            totalRecord = _db.TransferReceiptProcessing
                             .Include(x => x.StockCreation)
                             .Include(x => x.SellerTaxes) 
                             .Include(x => x.BuyerTaxes)
                             .Where(x =>
                                 x.IsActive &&
                                 !x.IsDeleted &&
                                 (string.IsNullOrEmpty(startDate) || x.CreatedOn.Date >= DateTime.Parse(startDate)) &&
                                 (string.IsNullOrEmpty(endDate) || x.CreatedOn.Date <= DateTime.Parse(endDate)) &&
                                 (string.IsNullOrEmpty(refrenceNo) || x.StockCreation.RegistrationNo == refrenceNo) &&
                                 (string.IsNullOrEmpty(plotNo) || x.StockCreation.PropertyNo == plotNo) &&
                                 (string.IsNullOrEmpty(sector) || x.StockCreation.PrefixProperty == sector) &&
                                 (string.IsNullOrEmpty(plotSize) || x.StockCreation.ActualSize == plotSize) &&
                                 (string.IsNullOrEmpty(dealerName) || x.DealerName == dealerName)
                                 && (string.IsNullOrEmpty(phase) || x.StockCreation.Phase == phase)
                                 && (string.IsNullOrEmpty(realStateType) || x.StockCreation.RealStateType == realStateType)
                                 && (string.IsNullOrEmpty(category) || x.StockCreation.Category == category)
                                 && (string.IsNullOrEmpty(block) || x.StockCreation.Block == block)
                                 && (string.IsNullOrEmpty(type) || x.StockCreation.Type == type)
                                 && (string.IsNullOrEmpty(nature) || x.StockCreation.Nature == nature)
                                 &&
                                 (
                                     x.SellerTaxes.Any(st =>
                                         st.Date != null ||
                                         st.Amount != null ||
                                         st.TaxTypeId != null ||
                                         st.ChallanNo != null
                                     ) ||
                                     x.BuyerTaxes.Any(bt =>
                                         bt.Date != null ||
                                         bt.Amount != null ||
                                         bt.TaxTypeId != null ||
                                         bt.ChallanNo != null
                                     )
                                 )
                             )
                             .Count();
                         
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


        private List<TaxReportDto> GetTaxReportSP(
          int pageNumber = 1,
          int pageSize = 10,
          string? searchTerm = "",
          string? startDate = null,
          string? endDate = null,
          string? refrenceNo = null,
          string? plotNo = null,
          string? sector = null,
          string? plotSize = null,
          string? dealerName = null,
          string? phase = null,
          string? realStateType = null,
          string? category = null,
          string? block = null,
          string? type = null,
          string? nature = null)

        {
            var properties = new List<TaxReportDto>();

            var parameters = new[]
            {
               new SqlParameter("@PageNumber", pageNumber),
               new SqlParameter("@PageSize", pageSize),
               new SqlParameter("@SearchTerm", string.IsNullOrEmpty(searchTerm) ? "" : searchTerm),
               new SqlParameter("@StartDate", string.IsNullOrEmpty(startDate) ? DBNull.Value : DateTime.Parse(startDate)),
               new SqlParameter("@EndDate", string.IsNullOrEmpty(endDate) ? DBNull.Value : DateTime.Parse(endDate)),
               new SqlParameter("@Sector", string.IsNullOrEmpty(sector) ? "" : sector),
               new SqlParameter("@RefrenceNo", string.IsNullOrEmpty(refrenceNo) ? "" : refrenceNo),
               new SqlParameter("@PlotNo", string.IsNullOrEmpty(plotNo) ? "" : plotNo),
               new SqlParameter("@PlotSize", string.IsNullOrEmpty(plotSize) ? "" : plotSize), // <--- ADD THIS
               new SqlParameter("@DealerName", string.IsNullOrEmpty(dealerName) ? "" : dealerName),
               new SqlParameter("@Phase", string.IsNullOrEmpty(phase) ? DBNull.Value : phase),
               new SqlParameter("@RealStateType", string.IsNullOrEmpty(realStateType) ? DBNull.Value : realStateType),
               new SqlParameter("@Category", string.IsNullOrEmpty(category) ? DBNull.Value : category),
               new SqlParameter("@Block", string.IsNullOrEmpty(block) ? DBNull.Value : block),
               new SqlParameter("@Type", string.IsNullOrEmpty(type) ? DBNull.Value : type),
               new SqlParameter("@Nature", string.IsNullOrEmpty(nature) ? DBNull.Value : nature),
           };

            string query = "EXEC [dbo].[sp_GetTaxReport] " +
                           "@PageNumber, @PageSize, @SearchTerm, @StartDate, @EndDate, @Sector, " +
                           "@RefrenceNo, @PlotNo, @PlotSize, @DealerName, @Phase, @RealStateType, " +
                           "@Category, @Block, @Type, @Nature";

            var dataModel = _db.JsonOutPutModel.FromSqlRaw(query, parameters).ToList();

            if (dataModel != null && dataModel.Any())
            {
                var jsonValue = dataModel.FirstOrDefault()?.JsonStringValue;
                if (!String.IsNullOrEmpty(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<TaxReportDto>>(jsonValue);
                }
            }

            return properties;
        }


        #endregion


        /*---USE----
        -----MemberNDC---
        -----RegistrationProfile---
        -----FileRequest-----
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

        [HttpGet]
        [Route("GetAllMapProperties")]
        public IActionResult GetAllMapProperties()
        {
            var data = GetAllMapPropertiesSP();
            return Ok(data);
        }

        private List<PropertBasicDetailsDto> GetAllMapPropertiesSP()
        {
            var properties = new List<PropertBasicDetailsDto>();



            var query = $"EXEC [dbo].[GetAllMapPropertiesSP]";
            var DataModel = _db.JsonOutPutModel.FromSqlRaw(query).ToList();

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
     -----Billing---
     */

        [HttpGet]
        [Route("GetCombineBillFromMonthlyBillGeneration")]
        public IActionResult GetCombineBillFromMonthlyBillGeneration(string readingFor, string month)
        {

            var data = GetAllGetCombineBillFromMonthlyBillGenerationSP(month);

            return Ok(new ApiResponse<object>
            {
                Code = ResponseCode.Success,
                Message = "Success",
                Data = data
            });
        }

        private List<BillDTO> GetAllGetCombineBillFromMonthlyBillGenerationSP(string month = null)
        {
            var properties = new List<BillDTO>();

            var Month = new SqlParameter("@Month", month);
            var query = $"EXEC [dbo].[GetCombineBillSp] @Month = @Month";

            var DataModel = _db.JsonOutPutModel.FromSqlRaw(query, Month).ToList();

            if (DataModel != null && DataModel.Any())
            {
                var jsonValue = DataModel.FirstOrDefault()?.JsonStringValue;
                if (!String.IsNullOrEmpty(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<BillDTO>>(jsonValue);
                }
            }
            return properties;
        }

        [HttpGet]
        [Route("GetDetailsFixedDuesFromMonthlyBillGeneration")]
        public IActionResult GetDetailsFixedDuesFromMonthlyBillGeneration(string readingFor, string month)
        {

            var data = GetAllFixedDuesFromMonthlyBillGenerationSP(readingFor, month);

            return Ok(new ApiResponse<object>
            {
                Code = ResponseCode.Success,
                Message = "Success",
                Data = data
            });
        }

        private List<BillDTO> GetAllFixedDuesFromMonthlyBillGenerationSP(string readingFor, string month = null)
        {
            var properties = new List<BillDTO>();

            var Month = new SqlParameter("@Month", month);
            var ReadingFor = new SqlParameter("@ReadingFor", readingFor);

            var query = $"EXEC [dbo].[GetFixedChargeBillDetail] @Month = @Month, @ReadingFor = @ReadingFor";

            var DataModel = _db.JsonOutPutModel.FromSqlRaw(query, Month, ReadingFor).ToList();

            if (DataModel != null && DataModel.Any())
            {
                var jsonValue = DataModel.FirstOrDefault()?.JsonStringValue;
                if (!String.IsNullOrEmpty(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<BillDTO>>(jsonValue);
                }
            }
            return properties;
        }

        [HttpGet]
        [Route("GetDetailsFromMonthlyBillGeneration")]
        public IActionResult GetDetailsFromMeterBillGeneration(string readingFor, string month)
        {

            var data = GetDetailsFromMeterBillGenerationSP(readingFor, month);

            return Ok(new ApiResponse<object>
            {
                Code = ResponseCode.Success,
                Message = "Success",
                Data = data
            });
        }

        private List<BillDTO> GetDetailsFromMeterBillGenerationSP(string readingFor, string month = null)
        {
            var properties = new List<BillDTO>();

            var Month = new SqlParameter("@Month", month);
            var ReadingFor = new SqlParameter("@ReadingFor", readingFor);

            var query = $"EXEC [dbo].[GetMeterBillGenerationDetailJson] @Month = @Month, @ReadingFor = @ReadingFor";

            var DataModel = _db.JsonOutPutModel.FromSqlRaw(query, Month, ReadingFor).ToList();

            if (DataModel != null && DataModel.Any())
            {
                var jsonValue = DataModel.FirstOrDefault()?.JsonStringValue;
                if (!String.IsNullOrEmpty(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<BillDTO>>(jsonValue);
                }
            }
            return properties;
        }

        /*---USE----
       -----MemberProfile---
       */

        [HttpPost]
        [Route("GetAllMemberProfiles")]
        public IActionResult GetAllMemberProfiles()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            var data = GetAllMemberProfilesSP(pageNumber, pageSize, searchValue);
            totalRecord = _db.MemberProfile.Count();

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

        private List<MemberBasicDetailsDto> GetAllMemberProfilesSP(int pageNumber = 1, int pageSize = 10, string? searchTerm = "")
        {
            var properties = new List<MemberBasicDetailsDto>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            var query = $"EXEC [dbo].[GetMemberProfileSpPagination] @PageNumber = @PageNumber, @PageSize = @PageSize, @SearchTerm = @SearchTerm";
            var DataModel = _db.JsonOutPutModel.FromSqlRaw(query, pageNumberParam, pageSizeParam, searchTermParam).ToList();

            if (DataModel != null && DataModel.Any())
            {
                var jsonValue = DataModel.FirstOrDefault()?.JsonStringValue;
                if (!String.IsNullOrEmpty(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<MemberBasicDetailsDto>>(jsonValue);
                }
            }

            return properties;
        }

        /*---USE----
        -----MemberNDC---
        */

        [HttpGet]
        [Route("GetNDCReceipt")]
        public IActionResult GetNDCReceipt(int id)
        {
            try
            {
                var receipt = new NDCReceiptDto();

                var DataModel = _db.JsonOutPutModel.FromSqlInterpolated($"Exec [dbo].[NDC_Receipt] @id={id}").ToList();

                receipt = !String.IsNullOrEmpty(DataModel.FirstOrDefault().JsonStringValue) ? JsonConvert.DeserializeObject<NDCReceiptDto>(DataModel.FirstOrDefault().JsonStringValue) : new NDCReceiptDto();

                return Ok(receipt);
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        /*---USE----
       -----Property Binding---
       */

        [HttpGet]
        [Route("GetFilteredPropertyBinding")]
        public IActionResult GetFilteredPropertyBinding(int key)
        {

            var data = GetSPFilteredPropertyBinding(key);

            return Ok(new ApiResponse<object>
            {
                Code = ResponseCode.Success,
                Message = "Success",
                Data = data
            });
        }

        private List<GetFilteredPropertyBindingDTO> GetSPFilteredPropertyBinding(int key)
        {
            var properties = new List<GetFilteredPropertyBindingDTO>();

            var Key = new SqlParameter("@Key", key);
            var query = $"EXEC [dbo].[GetFilteredPropertyBinding] @Key = @Key";

            var DataModel = _db.JsonOutPutModel.FromSqlRaw(query, Key).ToList();

            if (DataModel != null && DataModel.Any())
            {
                var jsonValue = DataModel.FirstOrDefault()?.JsonStringValue;
                if (!String.IsNullOrEmpty(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<GetFilteredPropertyBindingDTO>>(jsonValue);
                }
            }
            return properties;
        }

        [HttpGet]
        [Route("GetAllPropertyBinding")]
        public IActionResult GetAllPropertyBinding(int key)
        {

            var data = GetSPAllPropertyBinding(key);

            return Ok(new ApiResponse<object>
            {
                Code = ResponseCode.Success,
                Message = "Success",
                Data = data
            });
        }

        private List<GetAllPropertyBindingDTO> GetSPAllPropertyBinding(int key)
        {
            var properties = new List<GetAllPropertyBindingDTO>();

            var Key = new SqlParameter("@Key", key);
            var query = $"EXEC [dbo].[GetAllPropertyBinding] @Key = @Key";

            var DataModel = _db.JsonOutPutModel.FromSqlRaw(query, Key).ToList();

            if (DataModel != null && DataModel.Any())
            {
                var jsonValue = DataModel.FirstOrDefault()?.JsonStringValue;
                if (!String.IsNullOrEmpty(jsonValue))
                {
                    properties = JsonConvert.DeserializeObject<List<GetAllPropertyBindingDTO>>(jsonValue);
                }
            }
            return properties;
        }


        /*---USE----
        -----AllFormsPropertyDetail---
        */
        [HttpGet]
        [Route("GetSingleProperty")]
        public IActionResult GetSingleProperty(int id)
        {
            try
            {
                var property = new BasicPropertBasicDetailsDto();

                var DataModel = _db.JsonOutPutModel.FromSqlInterpolated($"Exec [dbo].[GetBasicPropertyDetailsSp] @id={id}").ToList();

                property = !String.IsNullOrEmpty(DataModel.FirstOrDefault().JsonStringValue) ? JsonConvert.DeserializeObject<BasicPropertBasicDetailsDto>(DataModel.FirstOrDefault().JsonStringValue) : new BasicPropertBasicDetailsDto();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = property
                });
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }


        /*---USE----
       -----RegistrationNoProfiles---
       */

        [HttpPost]
        [Route("GetAllFindModeRegistrationNoProfiles")]
        public IActionResult GetAllFindModeRegistrationNoProfiles()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            var data = GetAllFindModeRegistrationNoProfilesSP(pageNumber, pageSize, searchValue);
            totalRecord = _db.MemberProfile.Count();

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

        private List<PropertBasicDetailsDto> GetAllFindModeRegistrationNoProfilesSP(int pageNumber = 1, int pageSize = 10, string? searchTerm = "")
        {
            var properties = new List<PropertBasicDetailsDto>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            var query = $"EXEC [dbo].[GetRegistrationNoProfileSpPagination] @PageNumber = @PageNumber, @PageSize = @PageSize, @SearchTerm = @SearchTerm";
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
-----PossessionProperty---
*/

        [HttpPost]
        [Route("GetAllPossessionProperty")]
        public IActionResult GetAllPossessionProperty()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            var data = GetAllPossessionProperty(pageNumber, pageSize, searchValue);
            totalRecord = _db.StockCreations.Where(x => x.is_active == true
                                                    && x.PropertyNo != ""
                                                    && x.PropertyNo != null
                                                    && x.RegistrationNo != ""
                                                    && x.RegistrationNo != null
                                                    && x.PossessionEffectDate == null
                                                    && x.PossessionStatus != true
                                                    && x.Is_StockCreationApproved == true).Count();

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

        private List<PropertBasicDetailsDto> GetAllPossessionProperty(int pageNumber = 1, int pageSize = 10, string? searchTerm = "")
        {
            var properties = new List<PropertBasicDetailsDto>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            var query = $"EXEC [dbo].[GetPossessionSpPagination] @PageNumber = @PageNumber, @PageSize = @PageSize, @SearchTerm = @SearchTerm";
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
      -----GetAllDemarcationRequest---
      */

        [HttpPost]
        [Route("GetAllDemarcationRequest")]
        public IActionResult GetAllDemarcationRequest()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            var data = GetAllDemarcationRequest(pageNumber, pageSize, searchValue);
            totalRecord = _db.StockCreations
                             .Where(x => x.is_active == true
                                    && x.is_deleted == false
                                    && x.MemberProfileId != null
                                    && x.PossessionStatus == true
                                    && x.Is_DemarcationRequested == null)
                            .Count();

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

        private List<PropertBasicDetailsDto> GetAllDemarcationRequest(int pageNumber = 1, int pageSize = 10, string? searchTerm = "")
        {
            var properties = new List<PropertBasicDetailsDto>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            var query = $"EXEC [dbo].[GetDemarcationRequestSpPagination] @PageNumber = @PageNumber, @PageSize = @PageSize, @SearchTerm = @SearchTerm";
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

        [HttpPost]
        [Route("GetAllDemarcationRequestFindMode")]
        public IActionResult GetAllDemarcationRequestFindMode()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            var data = GetAllDemarcationRequestFindMode(pageNumber, pageSize, searchValue);
            var result = from dr in _db.NewDemarcationRequest
                         join sc in _db.StockCreations on dr.StockCreationId equals sc.ID
                         where sc.is_active == true &&
                               sc.is_deleted == false &&
                               sc.MemberProfileId != null &&
                               sc.PossessionStatus == true &&
                               sc.Is_DemarcationApproved == true
                         select new { NewDemarcationRequest = dr, StockCreations = sc };

            totalRecord = result.Count();

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

        private List<PropertBasicDetailsDto> GetAllDemarcationRequestFindMode(int pageNumber = 1, int pageSize = 10, string? searchTerm = "")
        {
            var properties = new List<PropertBasicDetailsDto>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            var query = $"EXEC [dbo].[GetAllDemarcationRequestFindMode] @PageNumber = @PageNumber, @PageSize = @PageSize, @SearchTerm = @SearchTerm";
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
     -----GetAllConstructionSecurity---
     */

        [HttpPost]
        [Route("GetAllConstructionSecurityFilterList")]
        public IActionResult GetAllConstructionSecurityFilterList()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            var data = GetAllConstructionSecurityFilterList(pageNumber, pageSize, searchValue);
            totalRecord = _db.StockCreations
                             .Where(x => x.is_active == true
                                    && x.is_deleted == false
                                    && x.MemberProfileId != null
                                    && x.Is_DemarcationFormApproved == true
                                    && x.Is_ConstructionSecurityRequested == null)
                            .Count();

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

        private List<PropertBasicDetailsDto> GetAllConstructionSecurityFilterList(int pageNumber = 1, int pageSize = 10, string? searchTerm = "")
        {
            var properties = new List<PropertBasicDetailsDto>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            var query = $"EXEC [dbo].[GetConstrcutionSecuritySpPagination] @PageNumber = @PageNumber, @PageSize = @PageSize, @SearchTerm = @SearchTerm";
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
-----Construction Mointring---
*/

        [HttpPost]
        [Route("GetAllConstructionMointring")]
        public IActionResult GetAllConstructionMointring()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            var data = GetAllConstructionMointringSP(pageNumber, pageSize, searchValue);
            totalRecord = _db.StockCreations.Where(x => !x.is_deleted
                                                   && x.MemberProfileId != null
                                                   && x.Is_ConstructionSecurityApproved == true
                                                   && x.Is_ConstructionMonitoringRequested != true
                                                     )
                                                 .Count();

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

        private List<PropertBasicDetailsDto> GetAllConstructionMointringSP(int pageNumber = 1, int pageSize = 10, string? searchTerm = "")
        {
            var properties = new List<PropertBasicDetailsDto>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            var query = $"EXEC [dbo].[sp_GetAllConstructionMointring] @PageNumber = @PageNumber, @PageSize = @PageSize, @SearchTerm = @SearchTerm";
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
         * 
         *---------DocumentSearchMod---------------
          -----GetAllConstructionMointringRequest---
        */

        [HttpPost]
        [Route("GetAllConstructionMointringRequest")]
        public IActionResult GetAllConstructionMointringRequest()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            var data = GetAllConstructionMointringRequest(pageNumber, pageSize, searchValue);
            totalRecord = _db.ConstructionMonitoring
                             .Where(x => x.IsActive == true
                                    && x.IsDeleted == false
                                    )
                            .Count();

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

        private List<PropertBasicDetailsDto> GetAllConstructionMointringRequest(int pageNumber = 1, int pageSize = 10, string? searchTerm = "")
        {
            var properties = new List<PropertBasicDetailsDto>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            var query = $"EXEC [dbo].[GetConstrcutionMointringSpPagination] @PageNumber = @PageNumber, @PageSize = @PageSize, @SearchTerm = @SearchTerm";
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
-----MemberNDCFindMode---
*/

        [HttpPost]
        [Route("GetAllFindModeNDCMember")]
        public IActionResult GetAllFindModeNDCMember()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            var data = GetAllFindModeNDCMemberSP(pageNumber, pageSize, searchValue);
            totalRecord = _db.NDCRequestForMember.Where(x => x.IsDeleted != true).Count();

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

        private List<PropertBasicDetailsDto> GetAllFindModeNDCMemberSP(int pageNumber = 1, int pageSize = 10, string? searchTerm = "")
        {
            var properties = new List<PropertBasicDetailsDto>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            var query = $"EXEC [dbo].[GetAllFindModeNDCMemberSpPagination] @PageNumber = @PageNumber, @PageSize = @PageSize, @SearchTerm = @SearchTerm";
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
-----NDC1FindMode---
*/

        [HttpPost]
        [Route("GetAllFindModeNDC1")]
        public IActionResult GetAllFindModeNDC1()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            var data = GetAllFindModeNDC1SP(pageNumber, pageSize, searchValue);
            totalRecord = _db.NDCRequestForMember.Where(x => x.IsDeleted != true &&
                                                             x.IsNDCRequestForMemberApproved == true &&
                                                             x.IsCanceled != true &&
                                                             x.IsActive != false)
                                                 .Count();

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

        private List<PropertBasicDetailsDto> GetAllFindModeNDC1SP(int pageNumber = 1, int pageSize = 10, string? searchTerm = "")
        {
            var properties = new List<PropertBasicDetailsDto>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            var query = $"EXEC [dbo].[GetAllFindModeNDC1SpPagination] @PageNumber = @PageNumber, @PageSize = @PageSize, @SearchTerm = @SearchTerm";
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

        [HttpPost]
        [Route("GetAllModeNDC1")]
        public IActionResult GetAllModeNDC1()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            var data = GetAllModeNDC1SP(pageNumber, pageSize, searchValue);
            totalRecord = _db.NDC1.Where(x => x.IsDeleted != true &&
                                                             x.IsActive != false)
                                                 .Count();

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

        private List<PropertBasicDetailsDto> GetAllModeNDC1SP(int pageNumber = 1, int pageSize = 10, string? searchTerm = "")
        {
            var properties = new List<PropertBasicDetailsDto>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            var query = $"EXEC [dbo].[GetAllModeNDC1SpPagination] @PageNumber = @PageNumber, @PageSize = @PageSize, @SearchTerm = @SearchTerm";
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
       -----GetAllTransferReceiptProcessingFindMode---
       */

        [HttpPost]
        [Route("GetAllTransferReceiptProcessingFindMode")]
        public IActionResult GetAllTransferReceiptProcessingFindMode()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            var data = GetAllFindModeTransferReceiptProcessingSP(pageNumber, pageSize, searchValue);
            totalRecord = _db.TransferReceiptProcessing.Where(x => x.IsDeleted != true &&
                                                                   x.SellerName != null)
                                                       .ToList()
                                                       .OrderByDescending(x => x.Id)
                                                       .DistinctBy(x => x.StockCreationId)
                                                       .Count();

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

        private List<PropertBasicDetailsDto> GetAllFindModeTransferReceiptProcessingSP(int pageNumber = 1, int pageSize = 10, string? searchTerm = "")
        {
            var properties = new List<PropertBasicDetailsDto>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            var query = $"EXEC [dbo].[GetAllTransferReciptFindModeSpPagination] @PageNumber = @PageNumber, @PageSize = @PageSize, @SearchTerm = @SearchTerm";
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
      -----GetAllTransferReceiptProcessing---
      */

        [HttpPost]
        [Route("GetAllTransferReceiptProcessing")]
        public IActionResult GetAllTransferReceiptProcessing()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            var data = GetAllTransferReceiptProcessingSP(pageNumber, pageSize, searchValue);
            totalRecord = _db.NDC1.Where(x => x.IsDeleted != true &&
                                              x.IsGovtTaxRequested == true &&
                                              (x.IsGovtTaxApproved == false || x.IsGovtTaxApproved == null) &&
                                              x.IsRequestClosed == true)
                                  .ToList()
                                  .OrderByDescending(x => x.Id)
                                  .DistinctBy(x => x.StockCreationId)
                                  .Count();

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

        private List<PropertBasicDetailsDto> GetAllTransferReceiptProcessingSP(int pageNumber = 1, int pageSize = 10, string? searchTerm = "")
        {
            var properties = new List<PropertBasicDetailsDto>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            var query = $"EXEC [dbo].[GetAllTransferReciptSpPagination] @PageNumber = @PageNumber, @PageSize = @PageSize, @SearchTerm = @SearchTerm";
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
      -----GetAllTransferHistory---
      */

        [HttpPost]
        [Route("GetAllTransferHistory")]
        public IActionResult GetAllTransferHistory()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            var data = GetAllTransferHistorySP(pageNumber, pageSize, searchValue);
            totalRecord = _db.TransferHistery.Where(x => !x.IsDeleted &&
                                                          x.IsGovtProcessingTaxApproved == true &&
                                                          x.IsRequestClosed != true)
                                             .ToList()
                                             .OrderByDescending(x => x.Id)
                                             .DistinctBy(x => x.StockCreationId)
                                             .Count();

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

        private List<PropertBasicDetailsDto> GetAllTransferHistorySP(int pageNumber = 1, int pageSize = 10, string? searchTerm = "")
        {
            var properties = new List<PropertBasicDetailsDto>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            var query = $"EXEC [dbo].[GetAllTransferHistorySpPagination] @PageNumber = @PageNumber, @PageSize = @PageSize, @SearchTerm = @SearchTerm";
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
     -----GetAllTransferHistoryFindMode---
     */

        [HttpPost]
        [Route("GetAllTransferHistoryFindMode")]
        public IActionResult GetAllTransferHistoryFindMode()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            var data = GetAllTransferHistoryFindModeSP(pageNumber, pageSize, searchValue);
            totalRecord = _db.TransferHistery.Where(x => !x.IsDeleted && x.IsActive &&
                                                          x.IsRequestClosed == true &&
                                                          x.SellerName != null
                                                          )
                                             .ToList()
                                             .OrderByDescending(x => x.Id)
                                             //.DistinctBy(x => x.StockCreationId)
                                             .Count();

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

        private List<PropertBasicDetailsDto> GetAllTransferHistoryFindModeSP(int pageNumber = 1, int pageSize = 10, string? searchTerm = "")
        {
            var properties = new List<PropertBasicDetailsDto>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            var query = $"EXEC [dbo].[GetAllTransferHistoryFindModeSpPagination] @PageNumber = @PageNumber, @PageSize = @PageSize, @SearchTerm = @SearchTerm";
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
        -----DealerNDCFindMode---
        */

        [HttpPost]
        [Route("GetAllFindModeNDCDealer")]
        public IActionResult GetAllFindModeNDCDealer()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            var data = GetAllFindModeNDCDealerSP(pageNumber, pageSize, searchValue);
            totalRecord = _db.NDCRequestForMember.Where(x => x.IsDeleted != true && x.DealerCode != null).Count();

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

        private List<PropertBasicDetailsDto> GetAllFindModeNDCDealerSP(int pageNumber = 1, int pageSize = 10, string? searchTerm = "")
        {
            var properties = new List<PropertBasicDetailsDto>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            var query = $"EXEC [dbo].[GetAllFindModeNDCDealerSpPagination] @PageNumber = @PageNumber, @PageSize = @PageSize, @SearchTerm = @SearchTerm";
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
-----PreSale---
*/

        [HttpPost]
        [Route("GetAllPreSale")]
        public IActionResult GetAllPreSale()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            var data = GetAllPreSaleSP(pageNumber, pageSize, searchValue);
            totalRecord = _db.StockCreations
                                 .Where(sc => sc.is_active == true
                                           && sc.is_deleted == false
                                           && sc.Is_StockCreationApproved == true
                                           && sc.RegistrationNo != null
                                           && sc.IsPreSaleRequested == null)
                                 .Count();


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

        private List<PropertBasicDetailsDto> GetAllPreSaleSP(int pageNumber = 1, int pageSize = 10, string? searchTerm = "")
        {
            var properties = new List<PropertBasicDetailsDto>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            var query = $"EXEC [dbo].[GetAllPreSaleSP] @PageNumber = @PageNumber, @PageSize = @PageSize, @SearchTerm = @SearchTerm";
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
-----Transfer Recieving Set---
*/

        [HttpPost]
        [Route("GetAllFindModeTransferSetReport1")]
        public IActionResult GetAllFindModeTransferSetReport1(string? refn, string? plot, string? fromdate, 
                                                              string? todate, string? sector, string? size,
                                                              string? dname, int? draw,int length,int start )
        {
            int totalRecord = 0;
            int filterRecord = 0;
            draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            length = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            start = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (start / length) + 1;

            // Use the received parameters in your logic
           // var data = GetAllFindModeTransferSetSPForReport(pageNumber, pageSize, request.FromDate, request.ToDate, request.Refn, request.Plot, request.Sector, request.Size, request.Dname);

            totalRecord = _db.NDC1.Where(x => !x.IsDeleted).Count();
            filterRecord = totalRecord; // Update this if you filter based on the input params

            var returnObj = new
            {
                draw = draw,
                recordsTotal = totalRecord,
                recordsFiltered = filterRecord,
            //    data = data.ToList()
            };

            return Ok(returnObj);
        }

        [HttpPost]
        [Route("GetAllFindModeTransferSet")]
        public IActionResult GetAllFindModeTransferSet()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            var data = GetAllFindModeTransferSetSP(pageNumber, pageSize, searchValue);
            totalRecord = _db.TransferSetReceivings.Where(x => !x.IsDeleted && x.IsActive).Count();

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

        private List<PropertBasicDetailsDto> GetAllFindModeTransferSetSP(int pageNumber = 1, int pageSize = 10, string? searchTerm = "")
        {
            var properties = new List<PropertBasicDetailsDto>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            var query = $"EXEC [dbo].[GetAllFindModeTransferSetSpPagination] @PageNumber = @PageNumber, @PageSize = @PageSize, @SearchTerm = @SearchTerm";
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

        [HttpPost]
        [Route("GetAllTransferSet")]
        public IActionResult GetAllTransferSet()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            int draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageNumber = (skip / pageSize) + 1;

            var data = GetAllTransferSetSP(pageNumber, pageSize, searchValue);
            totalRecord = _db.NDC1.Where(x => !x.IsDeleted && x.IsGovtTaxRequested == true && x.IsGovtTaxApproved != true && (x.IsRequestClosed == false || x.IsRequestClosed == null)).Count();

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

        private List<PropertBasicDetailsDto> GetAllTransferSetSP(int pageNumber = 1, int pageSize = 10, string? searchTerm = "")
        {
            var properties = new List<PropertBasicDetailsDto>();

            var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            var searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            var query = $"EXEC [dbo].[GetAllTransferSetRecievingSpPagination] @PageNumber = @PageNumber, @PageSize = @PageSize, @SearchTerm = @SearchTerm";
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

    }
}
