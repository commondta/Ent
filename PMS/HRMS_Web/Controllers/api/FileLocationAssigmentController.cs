using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileLocationAssigmentController : ControllerBase
    {
        private readonly DataBase_Context _db;

        CommonBLL _commonBLL;

        public FileLocationAssigmentController(DataBase_Context db)
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

                var result = _db.FileLocationAssigments.Where(x => !x.IsDeleted && x.Id == id)
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

                var result = _db.FileLocationAssigments.Where(x => !x.IsDeleted)
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
        [Route("AddFileLocationAssigment")]
        public IActionResult AddFileLocationAssigment([FromBody] FileLocationAssigment model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                    });
                }

                    model.CreatedOn = DateTime.Now;
                    model.CreatedBy = model.CreatedBy;
                    model.ModifiedBy = model.ModifiedBy;
                    model.LastModifiedUserName = model.LastModifiedUserName;
                    model.IsActive = true;
                    model.IsDeleted = false;

                    _db.FileLocationAssigments.Add(model);
                    _db.SaveChanges();

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = model
                    });
                
              
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        [Route("UpdateFileLocationAssigment")]
        public IActionResult UpdateFileLocationAssigment([FromBody] FileLocationAssigment model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                    });
                }

                var data = _db.FileLocationAssigments.Find(model.Id);
                data.Row = model.Row;
                data.Rack = model.Rack;
                data.LastModified = DateTime.Now;
                data.ModifiedBy = model.ModifiedBy;
                data.LastModifiedUserName = model.LastModifiedUserName;

                _db.SaveChanges();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Record Updated Successfully",
                    Data = data
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpDelete]
        [Route("DeleteFileLocationAssigment")]
        public IActionResult DeleteFileLocationAssigment(int id)
        {
            try
            {
                var data = _db.FileLocationAssigments.Find(id);
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
