using Microsoft.EntityFrameworkCore;

namespace HRMS_Web.Models.DTOs
{
    [Keyless]
    public class JsonOutPutModel
    {
        public String JsonStringValue { get; set; }
    }
}
