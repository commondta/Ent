using B_DB_Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS_Web.Models.DTOs
{
    public class UserPermissionMappingDTO
    {
        public int EMP_CODE { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }

        // Navigation

        public int? PermissionFormsId { get; set; }
        public string? PermissionForm { get; set; }
    }
}
