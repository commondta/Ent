using B_DB_Model;
using CsvHelper;
using CsvHelper.Configuration;
using MemberUploader;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        const string csvFilePath = @"C:\DataUploader\MemberDataUploader.csv";


        SyncCsvWithDataBase<MemberDTO>(csvFilePath);

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
                var records = csv.GetRecords<MemberDTO>().ToList();

                using (var context = new AppDbContext())
                {
                    var dbSet = context.Set<MemberProfile>();

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
                            MemberProfile mem = new MemberProfile();
                            mem.ImageURL = item.ImageURL;
                            mem.HonorificsName = item.HonorificsName;
                            mem.MemberName = item.MemberName;
                            mem.Relationship = item.Relationship;
                            mem.RelationshipWith = item.RelationshipWith;
                            mem.MemberStatus = item.MemberStatus;
                            mem.DOB = DateTime.ParseExact(item.DOB, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            mem.Gender = item.Gender;
                            mem.Cnic = item.Cnic;
                            mem.CnicExpiryDate = DateTime.ParseExact(item.CnicExpiryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            mem.PassportNo = item.PassportNo;
                            mem.PassportExpiryDate = DateTime.ParseExact(item.PassportExpiryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            mem.Nationality = item.Nationality;
                            mem.OverSeas = item.OverSeas;
                            mem.CountryOfResidence = item.CountryOfResidence;
                            mem.CityOfResidence = item.CityOfResidence;
                            mem.SourceOfInfo = item.SourceOfInfo;
                            mem.OutstandingBalance = (decimal)item.OutstandingBalance;
                            mem.NICOPNo = item.NICOPNo;
                            mem.POCNO = item.POCNO;
                            mem.BioMetircInfo = item.BioMetircInfo;
                            mem.CurrentAddress = item.CurrentAddress;
                            mem.ResidenenceStatus = item.ResidenenceStatus;
                            mem.PermanentAddress = item.PermanentAddress;
                            mem.Vehicle = item.Vehicle;
                            mem.Mobile = item.Mobile;
                            mem.Phone = item.Phone;
                            mem.MothersMaidenName = item.MothersMaidenName;
                            mem.HomeNo = item.HomeNo;
                            mem.WhatsAppNo = item.WhatsAppNo;
                            mem.OfficeNo = item.OfficeNo;
                            mem.ImoNo = item.ImoNo;
                            mem.EmailId = item.EmailId;
                            mem.InstagramId = item.InstagramId;
                            mem.LinkedInId = item.LinkedInId;
                            mem.FacebookId = item.FacebookId;
                            mem.TwitterId = item.TwitterId;
                            mem.Profession = item.Profession;
                            mem.BussinessAddress = item.BussinessAddress;
                            mem.BussinessTenoure = item.BussinessTenoure;
                            mem.Salary = (decimal)item.Salary;
                            mem.TaxStatus = item.TaxStatus;
                            mem.NoOfDepartments = item.NoOfDepartments;
                            mem.RelationshipManager = item.RelationshipManager;
                            mem.NTNNo = item.NTNNo;
                            mem.UserName = item.UserName;
                            mem.Password = item.Password;
                            mem.PasswordHash = item.PasswordHash;
                            mem.PasswordKey = item.PasswordKey;
                            mem.DocNum = item.DocNum;
                            mem.SapPosting = item.SapPosting;
                            mem.DocEntry = item.DocEntry;
                            mem.IsActive = item.IsActive;
                            mem.IsDeleted = item.IsDeleted;
                            mem.CreatedBy = item.CreatedBy;
                            mem.ModifiedBy = item.ModifiedBy;
                            mem.LastModifiedUserName = item.LastModifiedUserName;
                            mem.MemberCategory = item.MemberCategory;
                            mem.Quota = item.Quota;
                            mem.Rank = item.Rank;
                            mem.PANO = item.PANO;
                            mem.Force = item.Force;
                            mem.Force = item.Shaheed;
                            mem.CNICBack = item.CNICBack;
                            mem.CNICFront = item.CNICFront;
                            mem.LastModified = DateTime.ParseExact(item.LastModified, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            mem.CreatedOn = DateTime.ParseExact(item.CreatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            dbSet.Add(mem);
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

    public class MemberDTO
    {

        public string? ImageURL { get; set; }
        public string? HonorificsName { get; set; }
        public string? MemberName { get; set; }
        public string? Relationship { get; set; }
        public string? RelationshipWith { get; set; }
        public string? MemberStatus { get; set; }
        public string? DOB { get; set; }
        public string? Gender { get; set; }
        public string? Cnic { get; set; }
        public string? CnicExpiryDate { get; set; }
        public string? PassportNo { get; set; }
        public string? PassportExpiryDate { get; set; }
        public string? Nationality { get; set; }
        public bool? OverSeas { get; set; }
        public string? CountryOfResidence { get; set; }
        public string? CityOfResidence { get; set; }
        public string? SourceOfInfo { get; set; }
        public decimal? OutstandingBalance { get; set; }
        public string? NICOPNo { get; set; }
        public string? POCNO { get; set; }
        public string? BioMetircInfo { get; set; }
        public string? CurrentAddress { get; set; }
        public string? ResidenenceStatus { get; set; }
        public string? PermanentAddress { get; set; }
        public string? Vehicle { get; set; }
        public string? Mobile { get; set; }
        public string? Phone { get; set; }
        public string? MothersMaidenName { get; set; }
        public string? HomeNo { get; set; }
        public string? WhatsAppNo { get; set; }
        public string? OfficeNo { get; set; }
        public string? ImoNo { get; set; }
        public string? EmailId { get; set; }
        public string? InstagramId { get; set; }
        public string? LinkedInId { get; set; }
        public string? FacebookId { get; set; }
        public string? TwitterId { get; set; }
        public string? Profession { get; set; }
        public string? BussinessAddress { get; set; }
        public string? BussinessTenoure { get; set; }
        public decimal? Salary { get; set; }
        public string? TaxStatus { get; set; }
        public string? NoOfDepartments { get; set; }
        public string? RelationshipManager { get; set; }
        public string? NTNNo { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public Byte[]? PasswordHash { get; set; }
        public Byte[]? PasswordKey { get; set; }
        public string? DocNum { get; set; }
        public bool? SapPosting { get; set; }
        public string? DocEntry { get; set; }

        public bool? IsMemberProfileRequested { get; set; }
        public bool? IsMemberProfileApproved { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public string? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public string? LastModified { get; set; }

        public int? ModifiedBy { get; set; }

        public string? LastModifiedUserName { get; set; }

        public string? MEMBERSHIPNO { get; set; }

        public string? MemberCategory { get; set; }
        public string? Rank { get; set; }
        public string? Force { get; set; }
        public string? PANO { get; set; }
        public string? Shaheed { get; set; }
        public string? Quota { get; set; }

        public string? CNICFront { get; set; }
        public string? CNICBack { get; set; }
    }
}
