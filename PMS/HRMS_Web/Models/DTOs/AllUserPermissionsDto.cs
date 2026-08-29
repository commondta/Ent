namespace HRMS_Web.Models.DTOs
{
    public class AllUserPermissionsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
    }
}
