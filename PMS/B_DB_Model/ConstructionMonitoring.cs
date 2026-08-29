using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class ConstructionMonitoring : BaseModel
    {
        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public StockCreation? StockCreation { get; set; }

        public int? UserId { get; set; }
      
        public string? ConstructionStatus { get; set; }
        public string? ConstructedStatus { get; set; }
        public string? EWSConnectionStatus { get; set; }
        public DateTime? ConstructionStartDate { get; set; }
        public DateTime? ConstructionEndDate { get; set; }

        public virtual ICollection<SiteServicesCM>? SiteServicesCM { get; set; }
        public virtual ICollection<YardStickCM>? YardStickCM { get; set; }
        public virtual ICollection<StackingCM>? StackingCM { get; set; }
        public virtual ICollection<ViolationCM>? ViolationCM { get; set; }
        public virtual ICollection<ConstructionMonitoringStageDetail>? ConstructionMonitoringStageDetail { get; set; }

    }

    public class YardStickCM : BaseModel
    {
        public DateTime Date { get; set; }

        [MaxLength(100)]
        public string Stage { get; set; }
        public int Progress { get; set; }
        public bool Status { get; set; } = true;
        [MaxLength(5000)]
        public string? Remarks { get; set; }

        [ForeignKey("ConstructionMonitoringId")]
        public int? ConstructionMonitoringId { get; set; }
        public ConstructionMonitoring? ConstructionMonitoring { get; set; }
    }

    public class SiteServicesCM : BaseModel
    {
        public DateTime Date { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Status { get; set; }
        [MaxLength(5000)]
        public string? Remarks { get; set; }

        [ForeignKey("ConstructionMonitoringId")]
        public int? ConstructionMonitoringId { get; set; }
        public ConstructionMonitoring? ConstructionMonitoring { get; set; }
    }

    public class StackingCM : BaseModel
    {
        [MaxLength(100)]
        public string Designation { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(20)]
        public string? Remarks { get; set; }

        [ForeignKey("ConstructionMonitoringId")]
        public int? ConstructionMonitoringId { get; set; }
        public ConstructionMonitoring? ConstructionMonitoring { get; set; }
    }
    public class ViolationCM : BaseModel
    {
        [MaxLength(100)]
        public string? InspectionBy { get; set; }
        public DateTime Date { get; set; }
        [MaxLength(200)]
        public string Violation { get; set; }
        public int Amount { get; set; }
        [MaxLength(5000)]
        public string? Remarks { get; set; }

        [ForeignKey("ConstructionMonitoringId")]
        public int? ConstructionMonitoringId { get; set; }
        public ConstructionMonitoring? ConstructionMonitoring { get; set; }
    }

    public class ConstructionMonitoringStageDetail : BaseModel
    {
         public int? StageCode { get; set; }
        [MaxLength(200)]
        public string? StageName { get; set; }
        [MaxLength(200)]
        public string? InspectionBy { get; set; }
        public DateTime? Date { get; set; }
        [MaxLength(200)]
        public string? Violation { get; set; }
        [MaxLength(5000)]
        public string? Remarks { get; set; }
        public string? Picture { get; set; }

        [ForeignKey("ConstructionMonitoringId")]
        public int? ConstructionMonitoringId { get; set; }
        public ConstructionMonitoring? ConstructionMonitoring { get; set; }
    }


}
