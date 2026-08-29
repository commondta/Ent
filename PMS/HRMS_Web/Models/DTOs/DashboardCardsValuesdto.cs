namespace HRMS_Web.Models.DTOs
{
    public class DashboardCardsValuesdto
    {
        public int totalMembers { get; set; }
        public int totalDealers { get; set; }
        public int totalPMSUsers { get; set; }
        public int totalStock { get; set; }
        public double soldStock { get; set; }
        public double availableStock { get; set; }

        public List<PieChartDTO>? TypeWisePieChartDTO { get; set; }
        public List<PieChartDTO>? SizeWisePieChartDTO { get; set; }
        public List<BlockWiseDataDTO>? BlockWiseData { get; set; }
        public List<BlockWiseDataDTO>? TypeWiseStackedChartDTO { get; set; }
    }

    public class DashboardTransferValuesdto
    {
        public List<PieChartDTO>? TypeWisePieChartDTO { get; set; }
        public List<PieChartDTO>? SizeWisePieChartDTO { get; set; }
        public List<BlockWiseDataDTO>? BlockWiseData { get; set; }
        public List<BlockWiseDataDTO>? TypeWiseStackedChartDTO { get; set; }
        public List<MonthlyDataDTO>? MonthlyTransferData { get; set; }
        public List<WeeklyDataDTO>? WeeklyTransferData { get; set; }
        public List<DailyDataDTO>? DailyTransferData { get; set; }
    }

    public class DailyDataDTO
    {
        public string Date { get; set; }
        public int TransferCount { get; set; }
    }

    public class MonthlyDataDTO
    {
        public string Month { get; set; }
        public int TransferCount { get; set; }
    }

    public class WeeklyDataDTO
    {
        public string WeekStartDate { get; set; }
        public int TransferCount { get; set; }
    }

    public class PieChartDTO
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }

    public class BlockWiseDataDTO
    {
        public string Name { get; set; }
        public int ResidentialCount { get; set; }
        public int CommercialCount { get; set; }
        public int VillaCount { get; set; }
        public int FarmHouseCount { get; set; }
        public int LargeComercialCount { get; set; }

    }

    public class KeyValuePairDto
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }

    public class DashboardMemberValuesdto
    {
        public List<KeyValuePairDto>? MemberCityWiseData { get; set; }
    }

    public class DashboardSalesValuesdto
    {
        public List<PieChartDTO>? TypeWisePieChartDTO { get; set; }
        public List<PieChartDTO>? SizeWisePieChartDTO { get; set; }
        public List<BlockWiseDataDTO>? BlockWiseData { get; set; }
        public List<BlockWiseDataDTO>? TypeWiseStackedChartDTO { get; set; }
        public List<MonthlyDataDTO>? MonthlySalesData { get; set; }
        public List<WeeklyDataDTO>? WeeklySalesData { get; set; }
        public List<DailyDataDTO>? DailySalesData { get; set; }
    }

}
