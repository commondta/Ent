using B_DB_Model;
using CsvHelper;
using CsvHelper.Configuration;
using DeleteStockUploader;
using System.Globalization;


class Program
{
    static void Main()
    {
        const string csvFilePath = @"C:\DataUploader\StockDataUploader.csv";


        SyncCsvWithDataBase<StockDTO>(csvFilePath);

        Console.WriteLine("Sync completed. Press any key to exit.");
        Console.ReadKey();
    }

    static void SyncCsvWithDataBase<T>(string filePath) where T : class, new()
    {
        try
        {
            Console.WriteLine("Sync Started. Please Wait..");


            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null
            }))
            {
                var records = csv.GetRecords<StockDTO>().ToList();

                using (var context = new AppDbContext())
                {
                    var dbSet = context.Set<StockCreation>();

                    int totalCount = records.Count;
                    int processedCount = 0;
                    int batchSize = 900;


                    Console.WriteLine($"\n\n Records Found: {totalCount} \n\n");


                    while (processedCount < totalCount)
                    {


                        var batch = records.Skip(processedCount).Take(batchSize).ToList();

                        foreach (var item in batch)
                        {
                            SetMissingValuesToNull(item);
                            StockCreation stock = dbSet.Find(item.ID);
                            
                            dbSet.Remove(stock);
                        }

                        context.SaveChanges();

                        processedCount += batch.Count;

                        Console.WriteLine($"Batch Execute, Dn't Kill until Sync completed");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static void SetMissingValuesToNull(object obj)
    {
        foreach (var property in obj.GetType().GetProperties())
        {
            if (property.PropertyType == typeof(string))
            {
                var value = (string)property.GetValue(obj);
                if (string.IsNullOrEmpty(value))
                {
                    property.SetValue(obj, null);
                }
            }
        }
    }

    public class StockDTO
    {
        public int ID { get; set; }
        public string? Created_at { get; set; }

        public string? RealStateType { get; set; }


        public string? Project { get; set; }

        public string? Phase { get; set; }


        public string? Block { get; set; }

        public string? Category { get; set; }


        public string? Type { get; set; }
        public int? DealerId { get; set; }
        public int? MemberProfileId { get; set; }

        public string? Nature { get; set; }
        public string? Finishing { get; set; }
        public string? Floor { get; set; }
        public string? ActualSize { get; set; }
        public string? ActualSizeUnit { get; set; }
        public string? Status { get; set; }
        public string? User { get; set; }
        public string? RegistrationNo { get; set; }
        public string? PropertyNo { get; set; }
        public string? PrefixRegistration { get; set; }
        public int? numForRegistration { get; set; }
        public string? postfixForRegistration { get; set; }
        public int? Quantity { get; set; }
        public decimal? coveredArea { get; set; }
        public string? ClearanceOn { get; set; }
        public string? MemberTaxStatus { get; set; }

        public string? PossessionEffectDate { get; set; }
        public bool? PossessionStatus { get; set; }

        public bool? UnderLitigation { get; set; }
        public string? ConstracutionStatus { get; set; }
        public string? GeneratorUnitType { get; set; }

        public bool? IsBillGenerationEnabled { get; set; }
        public bool? IsSaleTaxEnabled { get; set; }
        public bool? IsWithHoldingTaxEnabled { get; set; }
        public string? GrancePeriodForBillGenration { get; set; }

        public string? Location { get; set; }
        public string? Street { get; set; }
        public string? PrefixProperty { get; set; }
        public int? numForProperty { get; set; }
        public string? postfixForProperty { get; set; }
        public string? LDAPlotNo { get; set; }
        public string? LDAAreaSize { get; set; }

        public bool? Is_StockCreationRequested { get; set; }
        public bool? Is_StockCreationApproved { get; set; }
        public bool? Is_DemarcationRequested { get; set; }
        public bool? Is_ClearnceRequested { get; set; }
        public bool? Is_MapApprovalRequested { get; set; }
        public bool? Is_DemarcationApproved { get; set; }
        public bool? Is_ClearnceApproved { get; set; }
        public bool? Is_MapApprovalApproved { get; set; }
        public bool? Is_ConstructionSecurityRequested { get; set; }
        public bool? Is_ConstructionSecurityApproved { get; set; }
        public bool? Is_ConstructionMonitoringRequested { get; set; }
        public bool? Is_ConstructionMonitoringApproved { get; set; }
        public bool? Is_PossessionRequested { get; set; }
        public bool? Is_PossessionApproved { get; set; }
        public bool? Is_DemarcationFormRequested { get; set; }
        public bool? Is_DemarcationFormApproved { get; set; }

        public bool? IsPreSaleRequested { get; set; }
        public bool? IsPreSaleApproved { get; set; }

        public bool? IsBookingRequested { get; set; }
        public bool? IsBookingApproved { get; set; }

        public int? Created_By { get; set; }
        public string? Updated_at { get; set; }
        public int? Updated_By { get; set; }
        public bool? is_active { get; set; } = true;
        public bool? is_deleted { get; set; } = false;
        public string? Feature { get; set; }
        public string? InventoryStatus { get; set; }
        public string? Almt { get; set; }
        public decimal? DiscountPercent { get; set; }
    }
}
