using B_DB_Model;

namespace HRMS_Web.Models.DTOs
{
    public class TransferReciptDTO
    {
        public int Id { get; set; }
        public string BlockName { get; set; }
        public string RegistrationNo { get; set; }
        public string PropertyNo { get; set; }
        public string CategoryName { get; set; }
        public string PlotSize { get; set; }
        public string ConstructionStatus { get; set; }
        public string Filer { get; set; }
        public string BuyerName { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public string? ApplyStation { get; set; }
        public string CNIC { get; set; }
        public DateTime TransferDate { get; set; }
        public int MemberProfileId { get; set; }
        public int StockCreationId { get; set; }
        public string CoveredArea { get; set; }
        public string SellerName { get; set; }
        public string SellerFilerStatus { get; set; }
        public DateTime? ConstructedDateTime { get; set; }
        public int TimeAgo { get; set; }
        public int PropertyTaxYear { get; set; }
        public string? CategoryId { get; set; }
        public string? NatureId { get; set; }
        public string? PropertyTypeId { get; set; }
        public DateTime? EffectiveDateTime { get; set; }
        public string? NDCRequestType { get; set; }
        public string? TransferType { get; set; }
        public string? EstateName { get; set; }
        public string? DealerName { get; set; }
        public string? DealerCode { get; set; }
        public DateTime? SlotDate { get; set; }
        public DateTime? ValidateDate { get; set; }
        public string? SlotHour { get; set; }
        public string? SlotMintues { get; set; }
        public string? Day { get; set; }
        public string? PossessionStatus { get; set; }

        public List<TransferSetReceivingAttachments>? TransferSetReceivingAttachments { get; set; }
    }
}
