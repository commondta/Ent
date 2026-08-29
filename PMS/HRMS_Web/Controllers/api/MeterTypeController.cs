using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MeterTypeController : ControllerBase
    {
        private readonly DataBase_Context _db;

        CommonBLL _commonBLL;

        public MeterTypeController(DataBase_Context db)
        {
            _db = db;
            _commonBLL = new CommonBLL(_db);
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult Get(int id)
        {
            try
            {

                var result = _db.MeterType.Where(x => !x.IsDeleted && x.Id == id)
                                           .FirstOrDefault();

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

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {

                var result = _db.MeterType.Where(x => !x.IsDeleted)
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
        [Route("AddMeterType")]
        public IActionResult AddMeterType([FromBody] MeterType model)
        {
            try
            {
                var existingList = _db.MeterType.Where(x => x.Description == model.Description && x.IsDeleted != true).FirstOrDefault();

                if (existingList == null)
                {
                    model.CreatedOn = DateTime.Now;
                    model.CreatedBy = model.CreatedBy;
                    model.ModifiedBy = model.ModifiedBy;
                    model.LastModifiedUserName = model.LastModifiedUserName;
                    model.IsActive = true;
                    model.IsDeleted = false;

                    _db.MeterType.Add(model);
                    _db.SaveChanges();

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = model
                    });
                }

               else
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "Meter Type Already Exist",
                        Data = model
                    });
                }
            }

            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPut]
        [Route("UpdateMeterType")]
        public IActionResult UpdateMeterType([FromBody] MeterType model)
        {
            try
            {
                var existingList = _db.MeterType.Where(x => x.Description == model.Description && x.Id != model.Id && x.IsDeleted != true).FirstOrDefault();

                if (existingList == null)
                {

                    var data = _db.MeterType.Find(model.Id);
                    data.Description = model.Description;
                    data.LastModified = DateTime.Now;
                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;

                    _db.SaveChanges();

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = model
                    });
                }
                else
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "Meter Type Already Exist",
                        Data = model
                    });
                }
             }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpDelete]
        [Route("DeleteMeterType")]
        public IActionResult DeleteMeterType(int id)
        {
            try
            {
                var data = _db.MeterType.Find(id);
                data.LastModified = DateTime.Now;
                data.IsDeleted = true;
                data.IsActive = false;

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
