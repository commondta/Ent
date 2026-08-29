namespace HRMS_Web.Models.DTOs
{
    public class QueryRequest
    {
        public int QueryId { get; set; } // This is the dropdown selection
        public Dictionary<string, string> Parameters { get; set; }
    }
}
