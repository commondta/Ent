using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B_DB_Model
{
    // Form / navigation registry (AI file.xlsx Instructions §5): one row per module,
    // group or form. DisplayName is presentation only — PermissionKey is the stable
    // authorization identity and must never change when a form is renamed.
    [Table("NavigationNodes")]
    public class NavigationNode
    {
        [Key]
        public int Id { get; set; }

        public int? ParentId { get; set; }

        [MaxLength(20)]
        public string NodeType { get; set; } = "Form";   // Module | Group | Form

        [MaxLength(200)]
        public string DisplayName { get; set; } = "";

        [MaxLength(200)]
        public string? LegacyName { get; set; }

        [MaxLength(200)]
        public string? PermissionKey { get; set; }

        [MaxLength(200)]
        public string? FormKey { get; set; }

        [MaxLength(300)]
        public string? Route { get; set; }

        public int SequenceNo { get; set; }

        public int Depth { get; set; }

        public bool IsVisible { get; set; } = true;

        public bool IsActive { get; set; } = true;
    }
}
