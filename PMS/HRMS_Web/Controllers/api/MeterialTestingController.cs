using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common.Enums;
using B_Utility.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HRMS_Web.Extensions;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeterialTestingController : ControllerBase
    {
        private readonly DataBase_Context _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ApprovalBLL _approvalBLL;
        CommonBLL _commonBLL;
        public MeterialTestingController(DataBase_Context db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _approvalBLL = new ApprovalBLL(_db);
            _commonBLL = new CommonBLL(_db);
        }

        
        [HttpGet]
        [Route("Get")]
        public IActionResult Get(int id)
        {
            try
            {
                var result = _db.MeterialTesting.Where(x => !x.IsDeleted && x.StockCreationId == id)                                                                    
                                                                      .ToList();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

       

        [HttpPost]
        [Route("AddNewMeterialTesting")]
        public async Task<IActionResult> AddNewMeterialTesting(List<MeterialTesting> model)
        {
            try
            {
                var existing = _db.MeterialTesting.Where(x => x.StockCreationId == model.FirstOrDefault().StockCreationId).ToList();


                foreach (var attachment in existing)
                {
                    var existingFilePath = attachment.Attachment;

                    bool fileExistsInNewModel = existing.Any(x => x.Attachment == existingFilePath);

                    if (!fileExistsInNewModel && !string.IsNullOrEmpty(existingFilePath))
                    {
                        existingFilePath.DeleteFile();
                    }

                    _db.MeterialTesting.Remove(attachment);
                }

                _db.MeterialTesting.RemoveRange(existing);

                var path = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";

                if (model?.Count > 0)
                {
                    foreach (var item in model)
                    {
                         item.Attachment = string.IsNullOrEmpty(item.Attachment) ? "" : $"{path}{await item.Attachment.SaveBase64FileAsync()}";
                        item.CreatedOn = DateTime.Now;
                        item.LastModified = DateTime.Now;
                        item.ModifiedBy = item.ModifiedBy;
                        item.CreatedBy = item.CreatedBy;
                        item.LastModifiedUserName = item.LastModifiedUserName;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                _db.MeterialTesting.AddRange(model);
                _db.SaveChanges();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = null
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }
    }
}
