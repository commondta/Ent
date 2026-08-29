namespace HRMS_Web.Models.DTOs
{
    public class UpdateFixedChargeDTO
    {
        public int Id { get; set; }
        public string ChargeType { get; set; }
        public string ChargeDes { get; set; }
        public decimal Rate { get; set; }
        public int GlobalChargeSetupId { get; set; }
    }
}
