using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class TransferHistoricalData
    {
        [Key]
        public int Id { get; set; }
        public string? RegistrationNo { get; set; }
        public string? PropertyNo { get; set; }
        public DateTime? TransferDate { get; set; }
        public string? SellerName { get; set; }
        public string? SellerCNIC { get; set; }
        public string? BuyerName { get; set; }
        public string? BuyerCNIC { get; set; }
    }
}
