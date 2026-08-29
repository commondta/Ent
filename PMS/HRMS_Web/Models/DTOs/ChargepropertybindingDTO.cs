using B_DB_Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS_Web.Models.DTOs
{
    public class ChargepropertybindingDTO
    {
        public int MatchCode { get; set; }
        public string? RegistrationNo { get; set; }
        public string? PropertyNo { get; set; }
        public string? ChargeType { get; set; }
        public int? Rate { get; set; }
        public int? Discount { get; set; }
        public string? ChargeDes { get; set; }
        public bool? IsEnabled { get; set; }
        public int? StockCreationId { get; set; }
    }
}
