using HRMS_Web.Services.UploaderService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploaderController : ControllerBase
    {
        private readonly IUploaderService _uploaderService;

        public UploaderController(IUploaderService uploaderService)
        {
            _uploaderService = uploaderService;
        }

        [HttpPost]
        [Route("/api/Uploader/UploadCsv")]
        public async Task<IActionResult> UploadCsv([FromForm] IFormFile file, [FromForm] string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return BadRequest("Please select a valid table.");
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest("Please upload a valid CSV file.");
            }

            try
            {
                await _uploaderService.ProcessCsvAsync(file, tableName);
                return Ok(new { message = "File uploaded and processed successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
