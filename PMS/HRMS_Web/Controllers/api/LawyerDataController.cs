using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using HRMS_Web.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LawyerDataController : ControllerBase
    {
        private readonly DataBase_Context _db;

        CommonBLL _commonBLL;
        ApprovalBLL _approvalBLL;
        public LawyerDataController(DataBase_Context db)
        {
            _db = db;
            _approvalBLL = new ApprovalBLL(_db);
            _commonBLL = new CommonBLL(_db);
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _db.LawyerData.Where(x => !x.IsDeleted)
                                           .Include(x => x.CaseType)
                                           .Include(x => x.CaseCategory)
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
        [Route("SaveLawyerData")]
        public IActionResult SaveLawyerData([FromBody] List<LawyerData> dto)
        {
            try
            {
                if (dto.Count > 0)
                {
                    var data = _db.LawyerData.ToList();

                    _db.LawyerData.RemoveRange(data);
                    _db.SaveChanges();

                    foreach (var item in dto)
                    {
                        item.CreatedOn = DateTime.Now;
                        item.CreatedBy = item.CreatedBy;
                        item.ModifiedBy = item.ModifiedBy;
                        item.LastModifiedUserName = item.LastModifiedUserName;
                        item.IsActive = true;
                        item.IsDeleted = false;

                        _db.LawyerData.Add(item);
                        _db.SaveChanges();
                    }

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = null
                    });
                }

                else
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "Bad Request",
                        Data = null
                    });
                }
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }
    }
}
