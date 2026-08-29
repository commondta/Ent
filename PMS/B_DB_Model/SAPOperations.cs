using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class SAPOperations : BaseModel
    {
        public string Server { get; set; } = String.Empty;
        public string DBName { get; set; } = String.Empty;
        public string DBUserName { get; set; } = String.Empty;
        public string DBPassword { get; set; } = String.Empty;
        public string SAPUser { get; set; } = String.Empty;
        public string SAPPassword { get; set; } = String.Empty;

        public string DBType { get; set; } = String.Empty;
        public string CustomerSeries { get; set; } = String.Empty;
        public string DealerAccountCode { get; set; } = String.Empty;
        public string MemberAccountCode { get; set; } = String.Empty;
        public string BookingAccount { get; set; } = String.Empty;
        public decimal FingerPrintThreshhold { get; set; }

        public string RepurchaseDeductionAccount { get; set; } = String.Empty;
        [MaxLength(200)]
        public string? SignatoryRank { get; set; } = String.Empty;
        [MaxLength(200)]
        public string? SignatoryDesignation { get; set; } = String.Empty;
        [MaxLength(200)]
        public string? SignatoryName { get; set; } = String.Empty;

        public string? AllocationSignatoryRank { get; set; } = String.Empty;
        [MaxLength(200)]
        public string? AllocationSignatoryDesignation { get; set; } = String.Empty;
        [MaxLength(200)]
        public string? AllocationSignatoryName { get; set; } = String.Empty;
        public int BillDiscountPercentage { get; set; } = 0;
        public string TownPlanningClearanceCommaSepratedGLs { get; set; } = String.Empty;
        public string TransferCertificateTimeLineStatement { get; set; } = String.Empty;
    }
}
