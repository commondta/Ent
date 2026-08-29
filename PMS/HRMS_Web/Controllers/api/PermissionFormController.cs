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
    public class PermissionFormController : ControllerBase
    {
        private readonly DataBase_Context _db;

        CommonBLL _commonBLL;

        public PermissionFormController(DataBase_Context db)
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

                var result = _db.PermissionForms.Where(x => x.Id == id)
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
                var result = _db.PermissionForms.ToList().OrderBy(x=>x.SerialNo);

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
        [Route("AddNewPermission")]
        public IActionResult AddNewPermission([FromBody] PermissionForms model)
        {
            try
            {
                bool IsExsist = _db.PermissionForms.Where(x => x.Name.ToLower() == model.Name.ToLower().Trim()).Any();

                if (IsExsist)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Conflict,
                        Message = "Name Already Exist",
                        Data = null
                    });

                }
                else
                {
                    _db.PermissionForms.Add(model);
                    _db.SaveChanges();

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = null
                    });
                }

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPut]
        [Route("UpdatePermissionForm")]
        public IActionResult UpdatePermissionForm([FromBody] PermissionForms model)
        {
            try
            {
                bool IsExsist = _db.PermissionForms.Where(x => x.Name.ToLower() == model.Name.ToLower().Trim() && x.Id != model.Id).Any();

                if (IsExsist)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Conflict,
                        Message = "Name Already Exist",
                        Data = null
                    });

                }

                var data = _db.PermissionForms.Find(model.Id);
                data.Name = model.Name;
                data.Title = model.Title;
                data.IsActive = model.IsActive;
                data.SerialNo = model.SerialNo;
                data.ModifiedBy = model.ModifiedBy;
                data.LastModifiedUserName = model.LastModifiedUserName;

                _db.Update(data);
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


        [HttpDelete]
        [Route("DeletePermissionForm")]
        public IActionResult DeletePermissionForm(int id)
        {
            try
            {
                var data = _db.PermissionForms.Find(id);
                _db.Remove(data);
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
