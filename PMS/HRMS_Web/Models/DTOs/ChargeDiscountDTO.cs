namespace HRMS_Web.Models.DTOs
{
    public class ChargeDiscountDTO
    {
       public bool IsEnabled { get; set; }
       public int ChargeDiscount { get; set; }
       public decimal Rate { get; set; }
       public int Unit { get; set; }
    }
}
