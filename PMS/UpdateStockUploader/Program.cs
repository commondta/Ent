using B_DB_Model;
using CsvHelper;
using CsvHelper.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using UpdateStockUploader;

class Program
{
    static void Main()
    {
        const string csvFilePath = @"C:\Uploader\UploadingTemplate.csv";


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
                            //stock.Created_at = DateTime.ParseExact(item.Created_at, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            stock.RealStateType = item.RealStateType;
                            stock.Project = item.Project;
                            stock.Phase = item.Phase;
                            stock.Block = item.Block;
                            stock.Category = item.Category;
                            stock.Type = item.Type;
                            //stock.DealerId = item.DealerId;
                            //stock.MemberProfileId = item.MemberProfileId;
                            stock.Nature = item.Nature;
                            //stock.Finishing = item.Finishing;
                            stock.Floor = item.Floor;
                            stock.ActualSize = item.ActualSize;
                            stock.ActualSizeUnit = item.ActualSizeUnit;
                            //stock.Status = item.Status;
                            //stock.User = item.User;
                            stock.RegistrationNo = item.RegistrationNo;
                            stock.PropertyNo = item.PropertyNo;
                            //stock.PrefixRegistration = item.PrefixRegistration;
                            //stock.numForRegistration = item.numForRegistration;
                            //stock.postfixForRegistration = item.postfixForRegistration;
                            //stock.Quantity = item.Quantity;
                            //stock.coveredArea = item.coveredArea;
                            //stock.ClearanceOn = DateTime.ParseExact(item.ClearanceOn, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            //stock.MemberTaxStatus = item.MemberTaxStatus;
                            //stock.PossessionEffectDate = DateTime.ParseExact(item.PossessionEffectDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            //stock.PossessionStatus = item.PossessionStatus;
                            //stock.UnderLitigation = item.UnderLitigation;
                            stock.ConstracutionStatus = item.ConstracutionStatus;
                            //stock.GeneratorUnitType = item.GeneratorUnitType;
                            //stock.IsBillGenerationEnabled = item.IsBillGenerationEnabled;
                            //stock.IsSaleTaxEnabled = item.IsSaleTaxEnabled;
                            //stock.IsWithHoldingTaxEnabled = item.IsWithHoldingTaxEnabled;
                            //stock.GrancePeriodForBillGenration = DateTime.ParseExact(item.GrancePeriodForBillGenration, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            //stock.Location = item.Location;
                            stock.Street = item.Street;
                            stock.PrefixProperty = item.PrefixProperty;
                            //stock.numForProperty = item.numForProperty;
                            stock.postfixForProperty = item.postfixForProperty;
                            //stock.LDAPlotNo = item.LDAPlotNo;
                            //stock.LDAAreaSize = item.LDAAreaSize;
                            stock.Feature = item.Feature;
                            //stock.InventoryStatus = item.InventoryStatus;
                            stock.Almt = item.Almt;
                            stock.Latitude = item.Latitude;
                            stock.Longitude = item.Longitude;
                            stock.Mouza = item.Mouza;
                            stock.SaleDeedDate = (DateTime)item.SaleDeedDate;
                            stock.SaleDeedNo = item.SaleDeedNo;
                            stock.AllocationNo = item.AllocationNo;
                            stock.AffidavitCode = item.AffidavitCode;
                            stock.PropertyStatus = item.PropertyStatus;
                            //stock.DiscountPercent = item.DiscountPercent;
                            //stock.Is_StockCreationRequested = item.Is_StockCreationRequested;
                            //stock.Is_StockCreationApproved = item.Is_StockCreationApproved;
                            //stock.Is_DemarcationRequested = item.Is_DemarcationRequested;
                            //stock.Is_ClearnceRequested = item.Is_ClearnceRequested;
                            //stock.Is_MapApprovalRequested = item.Is_MapApprovalRequested;
                            //stock.Is_DemarcationApproved = item.Is_DemarcationApproved;
                            //stock.Is_ClearnceApproved = item.Is_ClearnceApproved;
                            //stock.Is_MapApprovalApproved = item.Is_MapApprovalApproved;
                            //stock.Is_ConstructionSecurityRequested = item.Is_ConstructionSecurityRequested;
                            //stock.Is_ConstructionSecurityApproved = item.Is_ConstructionSecurityApproved;
                            //stock.Is_ConstructionMonitoringRequested = item.Is_ConstructionMonitoringRequested;
                            //stock.Is_ConstructionMonitoringApproved = item.Is_ConstructionMonitoringApproved;
                            //stock.Is_PossessionRequested = item.Is_PossessionRequested;
                            //stock.Is_PossessionApproved = item.Is_PossessionApproved;
                            //stock.Is_DemarcationFormRequested = item.Is_DemarcationFormRequested;
                            //stock.Is_DemarcationFormApproved = item.Is_DemarcationFormApproved;
                            //stock.IsPreSaleRequested = item.IsPreSaleRequested;
                            //stock.IsPreSaleApproved = item.IsPreSaleApproved;
                            //stock.IsBookingRequested = item.IsBookingRequested;
                            //stock.IsBookingApproved = item.IsBookingApproved;
                            //stock.Created_By = (int)item.Created_By;
                            //stock.Updated_at = DateTime.ParseExact(item.Updated_at, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            //stock.Updated_By = (int)item.Updated_By;
                            //stock.is_active = (bool)item.is_active;
                            //stock.is_deleted = (bool)item.is_deleted;

                            //dbSet.Add(stock);
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
        //public string? Created_at { get; set; }

        public string? RealStateType { get; set; }


        public string? Project { get; set; }

        public string? Phase { get; set; }


        public string? Block { get; set; }

        public string? Category { get; set; }


        public string? Type { get; set; }
        //public int? DealerId { get; set; }
        //public int? MemberProfileId { get; set; }

        public string? Nature { get; set; }
        //public string? Finishing { get; set; }
        public string? Floor { get; set; }
        public string? ActualSize { get; set; }
        public string? ActualSizeUnit { get; set; }
        //public string? Status { get; set; }
        //public string? User { get; set; }
        public string? RegistrationNo { get; set; }
        public string? PropertyNo { get; set; }
        //public string? PrefixRegistration { get; set; }
        //public int? numForRegistration { get; set; }
        //public string? postfixForRegistration { get; set; }
        //public int? Quantity { get; set; }
        //public decimal? coveredArea { get; set; }
        //public string? ClearanceOn { get; set; }
        //public string? MemberTaxStatus { get; set; }

        //public string? PossessionEffectDate { get; set; }
        //public bool? PossessionStatus { get; set; }

        //public bool? UnderLitigation { get; set; }
        public string? ConstracutionStatus { get; set; }
        //public string? GeneratorUnitType { get; set; }

        //public bool? IsBillGenerationEnabled { get; set; }
        //public bool? IsSaleTaxEnabled { get; set; }
        //public bool? IsWithHoldingTaxEnabled { get; set; }
        //public string? GrancePeriodForBillGenration { get; set; }

        //public string? Location { get; set; }
        public string? Street { get; set; }
        public string? PrefixProperty { get; set; }
        //public int? numForProperty { get; set; }
        public string? postfixForProperty { get; set; }
        //public string? LDAPlotNo { get; set; }
        //public string? LDAAreaSize { get; set; }

        //public bool? Is_StockCreationRequested { get; set; }
        //public bool? Is_StockCreationApproved { get; set; }
        //public bool? Is_DemarcationRequested { get; set; }
        //public bool? Is_ClearnceRequested { get; set; }
        //public bool? Is_MapApprovalRequested { get; set; }
        //public bool? Is_DemarcationApproved { get; set; }
        //public bool? Is_ClearnceApproved { get; set; }
        //public bool? Is_MapApprovalApproved { get; set; }
        //public bool? Is_ConstructionSecurityRequested { get; set; }
        //public bool? Is_ConstructionSecurityApproved { get; set; }
        //public bool? Is_ConstructionMonitoringRequested { get; set; }
        //public bool? Is_ConstructionMonitoringApproved { get; set; }
        //public bool? Is_PossessionRequested { get; set; }
        //public bool? Is_PossessionApproved { get; set; }
        //public bool? Is_DemarcationFormRequested { get; set; }
        //public bool? Is_DemarcationFormApproved { get; set; }

        //public bool? IsPreSaleRequested { get; set; }
        //public bool? IsPreSaleApproved { get; set; }

        //public bool? IsBookingRequested { get; set; }
        //public bool? IsBookingApproved { get; set; }

        //public int? Created_By { get; set; }
        //public string? Updated_at { get; set; }
        //public int? Updated_By { get; set; }
        //public bool? is_active { get; set; } = true;
        //public bool? is_deleted { get; set; } = false;
        public string? Feature { get; set; }
        //public string? InventoryStatus { get; set; }
        public string? Almt { get; set; }
        //public decimal? DiscountPercent { get; set; }

        public string? Latitude { get; set; }
        public string? Longitude { get; set; }

        public string? PropertyStatus { get; set; }

        //[MaxLength(100)]
        //public string? CaseCode { get; set; }
        [MaxLength(100)]
        public string? AffidavitCode { get; set; }
        [MaxLength(100)]
        public string? SaleDeedNo { get; set; }
        public DateTime? SaleDeedDate { get; set; }
        [MaxLength(100)]
        public string? Mouza { get; set; }
        [MaxLength(100)]
        public string? AllocationNo { get; set; }

    }
}
