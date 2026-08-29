namespace HRMS_Web.Models.DTOs
{
    public class StandAloneListDTO
    {
        public int Id { get; set; }
        public string? ChallanNo { get; set; }
        public string? Type { get; set; }
        public string? MemberName { get; set; }
        public string? ReferenceNo { get; set; }
        public string? PropertyNo { get; set; }
        public string? DocumentDate { get; set; }
        public string? DueDate { get; set; }
        public string? Status { get; set; }
        public string? Remarks { get; set; }
        public string? CancelRemarks { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class StandAloneDetailDTO
    {
        public int Id { get; set; }

        public int? StockCreationId { get; set; }
        public int? MemberProfileId { get; set; }

        public string? ChallanNo { get; set; }
        public string? RegistrationNo { get; set; }
        public string? PropertyNo { get; set; }
        public string? MemberName { get; set; }
        public string? Cnic { get; set; }

        public string? TypeName { get; set; }
        public string? Block { get; set; }
        public string? Category { get; set; }
        public string? Size { get; set; }
        public string? PossessionStatus { get; set; }
        public string? ConstrucationStatus { get; set; }

        public string? Type { get; set; }
        public string? PaymentMode { get; set; }
        public string? Remarks { get; set; }
        public string? NameRecipt { get; set; }
        public string? BankAccountDD { get; set; }

        public string? DocumentDate { get; set; }
        public string? DueDate { get; set; }

        public bool? ShowOwnerDetails { get; set; }

        public List<StandAloneChargeDTO> Charges { get; set; } = new();
    }

    public class StandAloneChargeDTO
    {
        public string ChargeName { get; set; }
        public decimal Amount { get; set; }
        public string? Remarks { get; set; }
        public string? SapAccount { get; set; }

        // Frontend only
        public DateTime? DueDate { get; set; }
    }

}
