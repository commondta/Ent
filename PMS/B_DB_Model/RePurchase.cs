using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class RePurchase : BaseModel
    {
        public string? Dealer { get; set; }
        public string? BookingDate { get; set; }
        public string? Type { get; set; }
        public string? MarketValue { get; set; }
        public string? PurchaseRefundValue { get; set; }
        public string? NetProfitLoss { get; set; }
        public string? TotalRecieved { get; set; }
        public string? DeductionAmount { get; set; }
        public string? Balance { get; set; }
        public string? Remarks { get; set; }

        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public StockCreation? StockCreation { get; set; }

        public virtual ICollection<RePurchaseFinanceDetail>? RePurchaseFinanceDetail { get; set; }
        public virtual ICollection<RePurchasePropertyDivision>? RePurchasePropertyDivision { get; set; }
    }

    public class RePurchaseFinanceDetail 
    {
        [Key]
        public int Id { get; set; }
        public string? ChargeType { get; set; }
        public string? SapAccount { get; set; }
        public decimal AmountDue { get; set; }
        public decimal AmountRecieved { get; set; }
        public decimal DocTotal { get; set; }
        [NotMapped]
        public int? LineNum { get; set; }
        [NotMapped]
        public int? DocEntry { get; set; }
        [NotMapped]
        public int? DeductionPercentage { get; set; }

        [ForeignKey("RePurchaseId")]
        public int? RePurchaseId { get; set; }
        public RePurchase? RePurchase { get; set; }
    }

    public class RePurchasePropertyDivision 
    {
        [Key]
        public int Id { get; set; }
        public string? RegPrefix { get; set; }
        public int? RegNumber { get; set; }
        public string? RegPostfix { get; set; }
        public string? Size  { get; set; }
        public string? PropPrefix { get; set; }
        public int? PropNumber { get; set; }
        public string? PropPostfix { get; set; }
        public string? Category { get; set; }

        [ForeignKey("RePurchaseId")]
        public int? RePurchaseId { get; set; }
        public RePurchase? RePurchase { get; set; }
    }
}
