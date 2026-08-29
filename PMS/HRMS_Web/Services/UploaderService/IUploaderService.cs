namespace HRMS_Web.Services.UploaderService
{
    public interface IUploaderService
    {
        Task ProcessCsvAsync(IFormFile file, string tableName);
    }
}
