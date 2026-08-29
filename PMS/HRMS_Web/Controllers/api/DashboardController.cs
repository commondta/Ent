using B_DB_Context;
using B_DB_Model;
using B_Utility.Common;
using HRMS_Web.Extensions;
using HRMS_Web.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly DataBase_Context _db;

        public DashboardController(DataBase_Context db)
        {
            _db = db;
        }

        #region StockDashboard
        [HttpGet]
        [Route("/Home/api/Dashboard/GetCardsValues")]
        public IActionResult GetCardsValues(string project)
        {
            try
            {
                DashboardCardsValuesdto valuesdto = new DashboardCardsValuesdto();

                var overAllStock = project != "all" ? _db.StockCreations.Where(x => !x.is_deleted && x.is_active && x.Project == project).ToList()
                                                                  : _db.StockCreations.Where(x => !x.is_deleted && x.is_active).ToList();


                valuesdto.TypeWisePieChartDTO = GetStockTypeData(overAllStock);
                valuesdto.SizeWisePieChartDTO = GetStockSizeData(overAllStock);
                valuesdto.BlockWiseData = GetStockBlockData(overAllStock);
                valuesdto.TypeWiseStackedChartDTO = GetStockTypeWiseData(overAllStock);

                valuesdto.totalMembers = _db.MemberProfile.Count(x => !x.IsDeleted);
                valuesdto.totalDealers = _db.Dealers.Count(x => !x.IsDeleted);
                valuesdto.totalPMSUsers = _db.PMSUser.Count(x => !x.IsDeleted);
                valuesdto.totalStock = overAllStock.Count;
                valuesdto.soldStock = overAllStock.Count(x => !string.IsNullOrEmpty(x.RegistrationNo));
                valuesdto.availableStock = overAllStock.Count(x => string.IsNullOrEmpty(x.RegistrationNo) && !string.IsNullOrEmpty(x.PropertyNo));

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = valuesdto
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        private List<BlockWiseDataDTO> GetStockTypeWiseData(List<StockCreation> overAllStock)
        {
            var result = (from stock in overAllStock.Where(x => x.PropertyNo != null)
                          join type in _db.PropertyTypes on stock.Type equals type.ID.ToString()
                          group stock by new { type.ID, type.Description } into g
                          select new BlockWiseDataDTO
                          {
                              Name = g.Key.Description,
                              ResidentialCount = g.Count(stock => stock.Type == "2"),       // Residential  = 2
                              CommercialCount = g.Count(stock => stock.Type == "3"),        // Commercial  = 3
                              VillaCount = g.Count(stock => stock.Type == "4"),             // Villa  = 4
                              FarmHouseCount = g.Count(stock => stock.Type == "5"),         // FarmHouse  = 5
                              LargeComercialCount = g.Count(stock => stock.Type == "6"),    // Large Comercial  = 6
                          }).OrderBy(x => x.Name)
                          .ToList();

            return result;
        }

        private List<BlockWiseDataDTO> GetStockBlockData(List<StockCreation> overAllStock)
        {
            var result = (from stock in overAllStock
                          join block in _db.Blocks on stock.Block equals block.ID.ToString()
                          group stock by new { block.ID, block.Description } into g
                          select new BlockWiseDataDTO
                          {
                              Name = g.Key.Description,
                              ResidentialCount = g.Count(stock => stock.Type == "2"),       // Residential = 2
                              CommercialCount = g.Count(stock => stock.Type == "3"),        // Commercial = 3
                              VillaCount = g.Count(stock => stock.Type == "4"),             // Villa = 4
                              FarmHouseCount = g.Count(stock => stock.Type == "5"),         // FarmHouse = 5
                              LargeComercialCount = g.Count(stock => stock.Type == "6"),    // Large Comercial = 6
                          })
                          .OrderBy(x => x.Name) // Alphabetical order by Name
                          .ToList();

            return result;
        }


        private List<PieChartDTO> GetStockTypeData(List<StockCreation> overAllStock)
        {
            var result = (from stock in overAllStock.Where(x=>x.PropertyNo != null)
                          join type in _db.Categories on stock.Category equals type.ID.ToString()
                          group stock by new { type.ID, type.Description } into g
                          select new PieChartDTO
                          {
                              Name = g.Key.Description,
                              Count = g.Count()
                          }).OrderBy(x => x.Name)
                          .ToList();

            return result;
        }

        private List<PieChartDTO> GetStockSizeData(List<StockCreation> overAllStock)
        {
            var result = (from stock in overAllStock
                          join type in _db.Categories on stock.Category equals type.ID.ToString()
                          group stock by new { type.ID, type.Description } into g
                          select new PieChartDTO
                          {
                              Name = g.Key.Description,
                              Count = g.Count()
                          }).OrderBy(x => x.Name)
                          .ToList();

            return result;
        }

        #endregion

        #region AllotedInventory

        [HttpGet]
        [Route("/Home/api/Dashboard/GetAllotedInventory")]
        public IActionResult GetAllotedInventory(string project)
        {
            try
            {
                DashboardCardsValuesdto valuesdto = new DashboardCardsValuesdto();

                var overAllStock = project == "all" ? _db.StockCreations.Where(x => !x.is_deleted && x.is_active && !string.IsNullOrEmpty(x.RegistrationNo) &&  !string.IsNullOrEmpty(x.PropertyNo)).ToList()
                                                    : _db.StockCreations.Where(x => !x.is_deleted && x.is_active && x.Project == project && !string.IsNullOrEmpty(x.RegistrationNo) && !string.IsNullOrEmpty(x.PropertyNo)).ToList();

                valuesdto.TypeWisePieChartDTO = GetStockTypeDataAllotedInventory(overAllStock);
                valuesdto.SizeWisePieChartDTO = GetStockSizeDataAllotedInventory(overAllStock);
                valuesdto.BlockWiseData = GetStockBlockDataAllotedInventory(overAllStock);
                valuesdto.TypeWiseStackedChartDTO = GetStockTypeWiseDataAllotedInventory(overAllStock);
                valuesdto.totalStock = overAllStock.Count;

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = valuesdto
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        private List<BlockWiseDataDTO> GetStockTypeWiseDataAllotedInventory(List<StockCreation> overAllStock)
        {
            var result = (from stock in overAllStock.Where(x => x.PropertyNo != null)
                          join type in _db.PropertyTypes on stock.Type equals type.ID.ToString()
                          group stock by new { type.ID, type.Description } into g
                          select new BlockWiseDataDTO
                          {
                              Name = g.Key.Description,
                              ResidentialCount = g.Count(stock => stock.Type == "2"),       // Residential  = 2
                              CommercialCount = g.Count(stock => stock.Type == "3"),        // Commercial  = 3
                              VillaCount = g.Count(stock => stock.Type == "4"),             // Villa  = 4
                              FarmHouseCount = g.Count(stock => stock.Type == "5"),         // FarmHouse  = 5
                              LargeComercialCount = g.Count(stock => stock.Type == "6"),    // Large Comercial  = 6
                          }).OrderBy(x => x.Name)
                          .ToList();

            return result;
        }

        private List<BlockWiseDataDTO> GetStockBlockDataAllotedInventory(List<StockCreation> overAllStock)
        {
            var result = (from stock in overAllStock
                          join block in _db.Blocks on stock.Block equals block.ID.ToString()
                          group stock by new { block.ID, block.Description } into g
                          select new BlockWiseDataDTO
                          {
                              Name = g.Key.Description,
                              ResidentialCount = g.Count(stock => stock.Type == "2"),  // Residential  = 2
                              CommercialCount = g.Count(stock => stock.Type == "3"),   // Commercial  = 3
                              VillaCount = g.Count(stock => stock.Type == "4"),        // Villa  = 4
                              FarmHouseCount = g.Count(stock => stock.Type == "5"),    // FarmHouse  = 5
                              LargeComercialCount = g.Count(stock => stock.Type == "6"),// Large Comercial  = 6
                          })
                          .OrderBy(x=> x.Name)
                          .ToList();

            return result;
        }

        private List<PieChartDTO> GetStockTypeDataAllotedInventory(List<StockCreation> overAllStock)
        {
            var result = (from stock in overAllStock.Where(x => x.PropertyNo != null)
                          join type in _db.Categories on stock.Category equals type.ID.ToString()
                          group stock by new { type.ID, type.Description } into g
                          select new PieChartDTO
                          {
                              Name = g.Key.Description,
                              Count = g.Count()
                          }).OrderBy(x => x.Name)
                          .ToList();

            return result;
        }

        private List<PieChartDTO> GetStockSizeDataAllotedInventory(List<StockCreation> overAllStock)
        {
            var result = (from stock in overAllStock
                          join type in _db.Categories on stock.Category equals type.ID.ToString()
                          group stock by new { type.ID, type.Description } into g
                          select new PieChartDTO
                          {
                              Name = g.Key.Description,
                              Count = g.Count()
                          }).OrderBy(x => x.Name)
                          .ToList();

            return result;
        }
        #endregion

        #region AvailableInventory

        [HttpGet]
        [Route("/Home/api/Dashboard/GetAvailableInventory")]
        public IActionResult GetAvailableInventory(string project)
        {
            try
            {
                DashboardCardsValuesdto valuesdto = new DashboardCardsValuesdto();

                var overAllStock = project == "all" ? _db.StockCreations.Where(x => !x.is_deleted && x.is_active && string.IsNullOrEmpty(x.RegistrationNo) && !string.IsNullOrEmpty(x.PropertyNo)).ToList()
                                                    :_db.StockCreations.Where(x => !x.is_deleted && x.is_active && x.Project == project && string.IsNullOrEmpty(x.RegistrationNo) && !string.IsNullOrEmpty(x.PropertyNo)).ToList();

                valuesdto.TypeWisePieChartDTO = GetStockTypeDataAvailableInventory(overAllStock);
                valuesdto.SizeWisePieChartDTO = GetStockSizeDataAvailableInventory(overAllStock);
                valuesdto.BlockWiseData = GetStockBlockDataAvailableInventory(overAllStock);
                valuesdto.TypeWiseStackedChartDTO = GetStockTypeWiseDataAvailableInventory(overAllStock);
                valuesdto.totalStock = overAllStock.Count;

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = valuesdto
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        private List<BlockWiseDataDTO> GetStockTypeWiseDataAvailableInventory(List<StockCreation> overAllStock)
        {
            var result = (from stock in overAllStock.Where(x => x.PropertyNo != null)
                          join type in _db.PropertyTypes on stock.Type equals type.ID.ToString()
                          group stock by new { type.ID, type.Description } into g
                          select new BlockWiseDataDTO
                          {
                              Name = g.Key.Description,
                              ResidentialCount = g.Count(stock => stock.Type == "2"),       // Residential  = 2
                              CommercialCount = g.Count(stock => stock.Type == "3"),        // Commercial  = 3
                              VillaCount = g.Count(stock => stock.Type == "4"),             // Villa  = 4
                              FarmHouseCount = g.Count(stock => stock.Type == "5"),         // FarmHouse  = 5
                              LargeComercialCount = g.Count(stock => stock.Type == "6"),    // Large Comercial  = 6
                          }).OrderBy(x => x.Name)
                          .ToList();

            return result;
        }

        private List<BlockWiseDataDTO> GetStockBlockDataAvailableInventory(List<StockCreation> overAllStock)
        {
            var result = (from stock in overAllStock
                          join block in _db.Blocks on stock.Block equals block.ID.ToString()
                          group stock by new { block.ID, block.Description } into g
                          select new BlockWiseDataDTO
                          {
                              Name = g.Key.Description,
                              ResidentialCount = g.Count(stock => stock.Type == "2"),  // Residential  = 2
                              CommercialCount = g.Count(stock => stock.Type == "3"),   // Commercial  = 3
                              VillaCount = g.Count(stock => stock.Type == "4"),        // Villa  = 4
                              FarmHouseCount = g.Count(stock => stock.Type == "5"),    // FarmHouse  = 5
                              LargeComercialCount = g.Count(stock => stock.Type == "6"),// Large Comercial  = 6
                          })
                          .OrderBy(x => x.Name)
                          .ToList();

            return result;
        }

        private List<PieChartDTO> GetStockTypeDataAvailableInventory(List<StockCreation> overAllStock)
        {
            var result = (from stock in overAllStock.Where(x => x.PropertyNo != null)
                          join type in _db.Categories on stock.Category equals type.ID.ToString()
                          group stock by new { type.ID, type.Description } into g
                          select new PieChartDTO
                          {
                              Name = g.Key.Description,
                              Count = g.Count()
                          }).OrderBy(x => x.Name)
                          .ToList();

            return result;
        }

        private List<PieChartDTO> GetStockSizeDataAvailableInventory(List<StockCreation> overAllStock)
        {
            var result = (from stock in overAllStock
                          join type in _db.Categories on stock.Category equals type.ID.ToString()
                          group stock by new { type.ID, type.Description } into g
                          select new PieChartDTO
                          {
                              Name = g.Key.Description,
                              Count = g.Count()
                          }).OrderBy(x => x.Name)
                          .ToList();

            return result;
        }
        #endregion

        #region SalesDashboard

        [HttpGet]
        [Route("/Home/api/Dashboard/GetSalesDashboardValues")]
        public IActionResult GetSalesDashboardValues(DateTime fromDate, DateTime toDate, string? requestType)
        {
            try
            {
                DashboardSalesValuesdto valuesdto = new DashboardSalesValuesdto();

                var query = _db.Booking
                 .Where(x => !x.IsDeleted &&
                             x.CreatedOn.Date >= fromDate &&
                             x.CreatedOn.Date <= toDate);

                if (!string.IsNullOrEmpty(requestType))
                {
                    query = query.Where(x => _db.PreSale.Any(p => p.StockCreationId == x.StockCreationId
                                                               && p.TranscationType == requestType));
                }

                var overAllSales = query.ToList();

                valuesdto.BlockWiseData = GetSalesBlockData(overAllSales);
                valuesdto.TypeWisePieChartDTO = GetSalesTypeData(overAllSales);
                valuesdto.SizeWisePieChartDTO = GetSalesSizeData(overAllSales);
                valuesdto.BlockWiseData = GetSalesBlockData(overAllSales);
                valuesdto.TypeWiseStackedChartDTO = GetSalesTypeWiseData(overAllSales);
                valuesdto.MonthlySalesData = GetSalesMonthlyData(overAllSales);
                valuesdto.WeeklySalesData = GetSalesWeeklyData(overAllSales);
                valuesdto.DailySalesData = GetSalesDailyData(overAllSales);

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = valuesdto
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        private List<DailyDataDTO> GetSalesDailyData(List<Booking> overAllSales)
        {
            var result = (from sale in overAllSales
                          group sale by sale.CreatedOn.Date into g
                          select new { Date = g.Key, Count = g.Count() })
                          .OrderBy(x => x.Date)
                          .Select(x => new DailyDataDTO
                          {
                              Date = x.Date.ToString("dd-MMM"),
                              TransferCount = x.Count
                          }).ToList();

            return result;
        }

        private List<MonthlyDataDTO> GetSalesMonthlyData(List<Booking> overAllSales)
        {
            var result = (from transfer in overAllSales
                          group transfer by new
                          {
                              Year = transfer.CreatedOn.Year,
                              Month = transfer.CreatedOn.Month
                          } into g
                          select new MonthlyDataDTO
                          {
                              Month = g.Key.Month.ToString("00") + "-" + g.Key.Year,
                              TransferCount = g.Count()
                          }).OrderBy(x => x.Month).ToList();

            return result;
        }

        private List<WeeklyDataDTO> GetSalesWeeklyData(List<Booking> overAllSales)
        {
            var result = (from transfer in overAllSales
                          let weekStart = transfer.CreatedOn.StartOfWeek(DayOfWeek.Monday)
                          group transfer by weekStart into g
                          let weekEnd = g.Key.AddDays(6)
                          select new
                          {
                              WeekStartDate = $"{g.Key:dd-MMMM-yyyy} to {weekEnd:dd-MMMM-yyyy}",
                              TransferCount = g.Count(),
                              CreatedOnDate = g.OrderBy(t => t.CreatedOn).First().CreatedOn
                          })
                          .OrderBy(x => x.CreatedOnDate)
                          .Select(x => new WeeklyDataDTO
                          {
                              WeekStartDate = x.WeekStartDate,
                              TransferCount = x.TransferCount
                          })
                          .ToList();

            return result;
        }

        private List<BlockWiseDataDTO> GetSalesBlockData(List<Booking> overAllSales)
        {
            var result = (from transfer in overAllSales
                          join stock in _db.StockCreations on transfer.StockCreationId equals stock.ID
                          join block in _db.Blocks on stock.Block equals block.ID.ToString()
                          group stock by new { block.ID, block.Description } into g
                          select new BlockWiseDataDTO
                          {
                              Name = g.Key.Description,
                              ResidentialCount = g.Count(stock => stock.Type == "2"),  // Residential  = 2
                              CommercialCount = g.Count(stock => stock.Type == "3"),   // Commercial  = 3
                              VillaCount = g.Count(stock => stock.Type == "4"),        // Villa  = 4
                              FarmHouseCount = g.Count(stock => stock.Type == "5"),    // FarmHouse  = 5
                              LargeComercialCount = g.Count(stock => stock.Type == "6"),    // Large Comercial  = 6
                          }).OrderBy(x => x.Name)
                          .ToList();

            return result;
        }

        private List<BlockWiseDataDTO> GetSalesTypeWiseData(List<Booking> overAllSales)
        {
            var result = (from transfer in overAllSales
                          join stock in _db.StockCreations on transfer.StockCreationId equals stock.ID
                          join type in _db.PropertyTypes on stock.Type equals type.ID.ToString()
                          group stock by new { type.ID, type.Description } into g
                          select new BlockWiseDataDTO
                          {
                              Name = g.Key.Description,
                              ResidentialCount = g.Count(stock => stock.Type == "2"),  // Residential  = 2
                              CommercialCount = g.Count(stock => stock.Type == "3"),   // Commercial  = 3
                              VillaCount = g.Count(stock => stock.Type == "4"),        // Villa  = 4
                              FarmHouseCount = g.Count(stock => stock.Type == "5"),    // FarmHouse  = 5
                              LargeComercialCount = g.Count(stock => stock.Type == "6"),    // Large Comercial  = 6
                          }).OrderBy(x => x.Name)
                          .ToList();

            return result;
        }

        private List<PieChartDTO> GetSalesTypeData(List<Booking> overAllSales)
        {
            var result = (from transfer in overAllSales
                          join stock in _db.StockCreations on transfer.StockCreationId equals stock.ID
                          join sector in _db.Sectors on stock.PrefixProperty equals sector.ID.ToString()
                          group stock by new { sector.ID, sector.Description } into g
                          select new PieChartDTO
                          {
                              Name = g.Key.Description,
                              Count = g.Count()
                          }).ToList();

            return result;
        }

        private List<PieChartDTO> GetSalesSizeData(List<Booking> overAllSales)
        {
            var result = (from transfer in overAllSales
                          join stock in _db.StockCreations on transfer.StockCreationId equals stock.ID
                          join type in _db.Categories on stock.Category equals type.ID.ToString()
                          group stock by new { type.ID, type.Description } into g
                          select new PieChartDTO
                          {
                              Name = g.Key.Description,
                              Count = g.Count()
                          }).OrderBy(x => x.Name)
                          .ToList();

            return result;
        }

        #endregion

        #region TransferDashboard

        [HttpGet]
        [Route("/Home/api/Dashboard/GetTransferDashboardValues")]
        public IActionResult GetTransferDashboardValues(DateTime fromDate, DateTime toDate, string? requestType)
        {
            try
            {
                DashboardTransferValuesdto valuesdto = new DashboardTransferValuesdto();

                var overAllTransfer = string.IsNullOrEmpty(requestType) ? _db.TransferHistery.Where(x => !x.IsDeleted && x.IsRequestClosed == true &&
                                                                      x.SellerName != null &&
                                                                      x.CreatedOn.Date >= fromDate && x.CreatedOn.Date <= toDate).ToList()
                                                                      : _db.TransferHistery.Where(x => !x.IsDeleted && x.IsRequestClosed == true &&
                                                                      x.SellerName != null && x.ApplyStation == requestType &&
                                                                      x.CreatedOn.Date >= fromDate && x.CreatedOn.Date <= toDate).ToList();

                valuesdto.BlockWiseData = GetTransferBlockData(overAllTransfer);

                valuesdto.TypeWisePieChartDTO = GetTransferTypeData(overAllTransfer);
                valuesdto.SizeWisePieChartDTO = GetTransferSizeData(overAllTransfer);
                valuesdto.BlockWiseData = GetTransferBlockData(overAllTransfer);
                valuesdto.TypeWiseStackedChartDTO = GetTransferTypeWiseData(overAllTransfer);
                valuesdto.MonthlyTransferData = GetTransferMonthlyData(overAllTransfer);
                valuesdto.WeeklyTransferData = GetTransferWeeklyData(overAllTransfer);
                valuesdto.DailyTransferData = GetTransferDailyData(overAllTransfer);

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = valuesdto
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        private List<DailyDataDTO> GetTransferDailyData(List<TransferHistery> overAllTransfer)
        {
            var result = (from transfer in overAllTransfer
                          group transfer by transfer.CreatedOn.Date into g
                          select new { Date = g.Key, Count = g.Count() })
                          .OrderBy(x => x.Date)
                          .Select(x => new DailyDataDTO
                          {
                              Date = x.Date.ToString("dd-MMM"),
                              TransferCount = x.Count
                          }).ToList();

            return result;
        }

        private List<MonthlyDataDTO> GetTransferMonthlyData(List<TransferHistery> overAllTransfer)
        {
            var result = (from transfer in overAllTransfer
                          group transfer by new
                          {
                              Year = transfer.CreatedOn.Year,
                              Month = transfer.CreatedOn.Month
                          } into g
                          select new MonthlyDataDTO
                          {
                              Month = g.Key.Month.ToString("00") + "-" + g.Key.Year,
                              TransferCount = g.Count()
                          }).OrderBy(x => x.Month).ToList();

            return result;
        }

        private List<WeeklyDataDTO> GetTransferWeeklyData(List<TransferHistery> overAllTransfer)
        {
            var result = (from transfer in overAllTransfer
                          let weekStart = transfer.CreatedOn.StartOfWeek(DayOfWeek.Monday)
                          group transfer by weekStart into g
                          let weekEnd = g.Key.AddDays(6)
                          select new
                          {
                              WeekStartDate = $"{g.Key:dd-MMMM-yyyy} to {weekEnd:dd-MMMM-yyyy}",
                              TransferCount = g.Count(),
                              CreatedOnDate = g.OrderBy(t => t.CreatedOn).First().CreatedOn
                          })
                          .OrderBy(x => x.CreatedOnDate)
                          .Select(x => new WeeklyDataDTO
                          {
                              WeekStartDate = x.WeekStartDate,
                              TransferCount = x.TransferCount
                          })
                          .ToList();

            return result;
        }

        private List<BlockWiseDataDTO> GetTransferBlockData(List<TransferHistery> overAllTransfer)
        {
            var result = (from transfer in overAllTransfer
                          join stock in _db.StockCreations on transfer.StockCreationId equals stock.ID
                          join block in _db.Blocks on stock.Block equals block.ID.ToString()
                          group stock by new { block.ID, block.Description } into g
                          select new BlockWiseDataDTO
                          {
                              Name = g.Key.Description,
                              ResidentialCount = g.Count(stock => stock.Type == "2"),  // Residential  = 2
                              CommercialCount = g.Count(stock => stock.Type == "3"),   // Commercial  = 3
                              VillaCount = g.Count(stock => stock.Type == "4"),        // Villa  = 4
                              FarmHouseCount = g.Count(stock => stock.Type == "5"),    // FarmHouse  = 5
                              LargeComercialCount = g.Count(stock => stock.Type == "6"),    // Large Comercial  = 6
                          }).OrderBy(x => x.Name)
                          .ToList();

            return result;
        }

        private List<BlockWiseDataDTO> GetTransferTypeWiseData(List<TransferHistery> overAllTransfer)
        {
            var result = (from transfer in overAllTransfer
                          join stock in _db.StockCreations on transfer.StockCreationId equals stock.ID
                          join type in _db.PropertyTypes on stock.Type equals type.ID.ToString()
                          group stock by new { type.ID, type.Description } into g
                          select new BlockWiseDataDTO
                          {
                              Name = g.Key.Description,
                              ResidentialCount = g.Count(stock => stock.Type == "2"),  // Residential  = 2
                              CommercialCount = g.Count(stock => stock.Type == "3"),   // Commercial  = 3
                              VillaCount = g.Count(stock => stock.Type == "4"),        // Villa  = 4
                              FarmHouseCount = g.Count(stock => stock.Type == "5"),    // FarmHouse  = 5
                              LargeComercialCount = g.Count(stock => stock.Type == "6"),    // Large Comercial  = 6
                          }).OrderBy(x => x.Name)
                          .ToList();

            return result;
        }

        private List<PieChartDTO> GetTransferTypeData(List<TransferHistery> overAllTransfer)
        {
            var result = (from transfer in overAllTransfer
                          join stock in _db.StockCreations on transfer.StockCreationId equals stock.ID
                          join sector in _db.Sectors on stock.PrefixProperty equals sector.ID.ToString()
                          group stock by new { sector.ID, sector.Description } into g
                          select new PieChartDTO
                          {
                              Name = g.Key.Description,
                              Count = g.Count()
                          }).OrderBy(x => x.Name)
                          .ToList();

            return result;
        }

        private List<PieChartDTO> GetTransferSizeData(List<TransferHistery> overAllTransfer)
        {
            var result = (from transfer in overAllTransfer
                          join stock in _db.StockCreations on transfer.StockCreationId equals stock.ID
                          join type in _db.Categories on stock.Category equals type.ID.ToString()
                          group stock by new { type.ID, type.Description } into g
                          select new PieChartDTO
                          {
                              Name = g.Key.Description,
                              Count = g.Count()
                          }).OrderBy(x => x.Name)
                          .ToList();

            return result;
        }

        #endregion

        #region NDCDashboard

        [HttpGet]
        [Route("/Home/api/Dashboard/GetNDCDashboardValues")]
        public IActionResult GetNDCDashboardValues(DateTime fromDate, DateTime toDate, string requestType)
        {
            try
            {

                DashboardTransferValuesdto valuesdto = new DashboardTransferValuesdto();

                var query = _db.NDCRequestForMember
                  .Where(x => !x.IsDeleted &&
                              x.CreatedOn.Date >= fromDate &&
                              x.CreatedOn.Date <= toDate);

                if (requestType != "all")
                {
                    if (requestType == "1") // treat 1 as true
                    {
                        query = query.Where(x => x.IsNDCRequestForMemberApproved == true);
                    }
                    else
                    {
                        query = query.Where(x => x.IsNDCRequestForMemberApproved != true);
                    }               
                }

                var overAllNDC = query.ToList();

                valuesdto.BlockWiseData = GetNDCBlockData(overAllNDC);

                valuesdto.TypeWisePieChartDTO = GetNDCTypeData(overAllNDC);
                valuesdto.SizeWisePieChartDTO = GetNDCSizeData(overAllNDC);
                valuesdto.BlockWiseData = GetNDCBlockData(overAllNDC);
                valuesdto.TypeWiseStackedChartDTO = GetNDCTypeWiseData(overAllNDC);
                valuesdto.MonthlyTransferData = GetNDCMonthlyData(overAllNDC);
                valuesdto.WeeklyTransferData = GetNDCWeeklyData(overAllNDC);
                valuesdto.DailyTransferData = GetNDCDailyData(overAllNDC);

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = valuesdto
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        private List<DailyDataDTO> GetNDCDailyData(List<NDCRequestForMember> overAllNDC)
        {
            var result = (from NDC in overAllNDC
                          group NDC by NDC.CreatedOn.Date into g
                          select new { Date = g.Key, Count = g.Count() })
                          .OrderBy(x => x.Date)
                          .Select(x => new DailyDataDTO
                          {
                              Date = x.Date.ToString("dd-MMM"),
                              TransferCount = x.Count
                          }).ToList();

            return result;
        }

        private List<MonthlyDataDTO> GetNDCMonthlyData(List<NDCRequestForMember> overAllNDC)
        {
            var result = (from NDC in overAllNDC
                          group NDC by new
                          {
                              Year = NDC.CreatedOn.Year,
                              Month = NDC.CreatedOn.Month
                          } into g
                          select new MonthlyDataDTO
                          {
                              Month = g.Key.Month.ToString("00") + "-" + g.Key.Year,
                              TransferCount = g.Count()
                          }).OrderBy(x => x.Month).ToList();

            return result;
        }

        private List<WeeklyDataDTO> GetNDCWeeklyData(List<NDCRequestForMember> overAllNDC)
        {
            var result = (from NDC in overAllNDC
                          let weekStart = NDC.CreatedOn.StartOfWeek(DayOfWeek.Monday)
                          group NDC by weekStart into g
                          let weekEnd = g.Key.AddDays(6)
                          select new
                          {
                              WeekStartDate = $"{g.Key:dd-MMMM-yyyy} to {weekEnd:dd-MMMM-yyyy}",
                              NDCCount = g.Count(),
                              CreatedOnDate = g.OrderBy(t => t.CreatedOn).First().CreatedOn
                          })
                          .OrderBy(x => x.CreatedOnDate)
                          .Select(x => new WeeklyDataDTO
                          {
                              WeekStartDate = x.WeekStartDate,
                              TransferCount = x.NDCCount
                          })
                          .ToList();

            return result;
        }

        private List<BlockWiseDataDTO> GetNDCBlockData(List<NDCRequestForMember> overAllNDC)
        {
            var result = (from NDC in overAllNDC
                          join stock in _db.StockCreations on NDC.StockCreationId equals stock.ID
                          join block in _db.Blocks on stock.Block equals block.ID.ToString()
                          group stock by new { block.ID, block.Description } into g
                          select new BlockWiseDataDTO
                          {
                              Name = g.Key.Description,
                              ResidentialCount = g.Count(stock => stock.Type == "2"),  // Residential  = 2
                              CommercialCount = g.Count(stock => stock.Type == "3"),   // Commercial  = 3
                              VillaCount = g.Count(stock => stock.Type == "4"),        // Villa  = 4
                              FarmHouseCount = g.Count(stock => stock.Type == "5"),    // FarmHouse  = 5
                              LargeComercialCount = g.Count(stock => stock.Type == "6"),    // Large Comercial  = 6
                          }).OrderBy(x => x.Name)
                          .ToList();

            return result;
        }

        private List<BlockWiseDataDTO> GetNDCTypeWiseData(List<NDCRequestForMember> overAllNDC)
        {
            var result = (from NDC in overAllNDC
                          join stock in _db.StockCreations on NDC.StockCreationId equals stock.ID
                          join type in _db.PropertyTypes on stock.Type equals type.ID.ToString()
                          group stock by new { type.ID, type.Description } into g
                          select new BlockWiseDataDTO
                          {
                              Name = g.Key.Description,
                              ResidentialCount = g.Count(stock => stock.Type == "2"),  // Residential  = 2
                              CommercialCount = g.Count(stock => stock.Type == "3"),   // Commercial  = 3
                              VillaCount = g.Count(stock => stock.Type == "4"),        // Villa  = 4
                              FarmHouseCount = g.Count(stock => stock.Type == "5"),    // FarmHouse  = 5
                              LargeComercialCount = g.Count(stock => stock.Type == "6"),    // Large Comercial  = 6
                          }).OrderBy(x => x.Name)
                          .ToList();

            return result;
        }

        private List<PieChartDTO> GetNDCTypeData(List<NDCRequestForMember> overAllNDC)
        {
            var result = (from NDC in overAllNDC
                          join stock in _db.StockCreations on NDC.StockCreationId equals stock.ID
                          join sector in _db.Sectors on stock.PrefixProperty equals sector.ID.ToString()
                          group stock by new { sector.ID, sector.Description } into g
                          select new PieChartDTO
                          {
                              Name = g.Key.Description,
                              Count = g.Count()
                          }).OrderBy(x => x.Name)
                          .ToList();

            return result;
        }

        private List<PieChartDTO> GetNDCSizeData(List<NDCRequestForMember> overAllNDC)
        {
            var result = (from NDC in overAllNDC
                          join stock in _db.StockCreations on NDC.StockCreationId equals stock.ID
                          join type in _db.Categories on stock.Category equals type.ID.ToString()
                          group stock by new { type.ID, type.Description } into g
                          select new PieChartDTO
                          {
                              Name = g.Key.Description,
                              Count = g.Count()
                          }).OrderBy(x => x.Name)
                          .ToList();

            return result;
        }

        #endregion

        #region MemberDashboard

        [HttpGet]
        [Route("/Home/api/Dashboard/GetMemberDashboardValues")]
        public IActionResult GetMemberDashboardValues()
        {
            try
            {
                DashboardMemberValuesdto valuesdto = new DashboardMemberValuesdto();

                var overAllmembers = _db.MemberProfile.Where(x => !x.IsDeleted).ToList();


                valuesdto.MemberCityWiseData = GetMemberCityWiseData(overAllmembers);

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = valuesdto
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        private List<KeyValuePairDto> GetMemberCityWiseData(List<MemberProfile> overAllmembers)
        {
            var result = (from member in overAllmembers
                          group member by member.CountryOfResidence into g
                          select new KeyValuePairDto
                          {
                              Name = g.Key,
                              Count = g.Count()
                          }).ToList();

            return result;
        }
        #endregion
    }
}
