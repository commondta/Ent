using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using HRMS_Web.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    [AllowAnonymous]
    public class PMSUserController : ControllerBase
    {
        private readonly DataBase_Context _db;

        CommonBLL _commonBLL;

        public PMSUserController(DataBase_Context db)
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

                var result = _db.PMSUser.Where(x => x.Id == id)
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
        [Route("GetPMSUserByDepartment")]
        public IActionResult GetPMSUserByDepartment(int id)
        {
            try
            {
                string depart = _db.Departments.SingleOrDefault(x=>x.ID == id).Description;

                var result = _db.PMSUser.Where(x => x.DEPARTMENT_DESC == depart)
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

        [HttpGet]
        [Route("GetAllPMSUsers")]
        [AllowAnonymous]
        public IActionResult GetAllPMSUsers()
        {
            try
            {
                var result = _db.PMSUser.Select(x=> new
                                         {
                                            x.Id,
                                            x.EMP_FULL_NAME,
                                            x.DESIG_DESC
                                         })
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

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _db.PMSUser.ToList();

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
        [Route("AddNewUser")]
        public IActionResult AddNewUser([FromBody] PMSUser model)
        {
            try
            {
                bool IsExsist = _db.PMSUser.Where(x => x.Username.ToLower() == model.Username.ToLower().Trim()).Any();

                if (IsExsist)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Conflict,
                        Message = "UserName Already Exist",
                        Data = null
                    });

                }
                else
                {
                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        byte[] passwordHashing, passwordKey;

                        using (var hmac = new HMACSHA512())
                        {
                            passwordKey = hmac.Key;
                            passwordHashing = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(model.Password));

                        }


                        model.PasswordHash = passwordHashing;
                        model.PasswordKey = passwordKey;
                        model.Password = null;
                    }
                    model.CreatedOn = DateTime.Now;
                    model.CreatedBy = model.CreatedBy;
                    model.LastModified = DateTime.Now;
                    model.ModifiedBy = model.ModifiedBy;
                    model.LastModifiedUserName = model.LastModifiedUserName;
                    _db.PMSUser.Add(model);
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

        [HttpPost]
        [Route("UpdateUser")]
        public IActionResult UpdateUser([FromBody] PMSUser model)
        {
            try
            {
                bool IsExsist = _db.PMSUser.Where(x => x.Username.ToLower() == model.Username.ToLower().Trim() && x.Id != model.Id).Any();

                if (IsExsist)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Conflict,
                        Message = "UserName Already Exist",
                        Data = null
                    });

                }

                var data = _db.PMSUser.Find(model.Id);
                if (!string.IsNullOrEmpty(model.Password))
                {
                    byte[] passwordHashing, passwordKey;

                    using (var hmac = new HMACSHA512())
                    {
                        passwordKey = hmac.Key;
                        passwordHashing = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(model.Password));

                    }


                    data.PasswordHash = passwordHashing;
                    data.PasswordKey = passwordKey;
                    data.Password = null;
                }
                data.EMP_CODE = model.EMP_CODE;
                data.Username = model.Username;
                data.NIC_NO = model.NIC_NO;
                data.EMP_FULL_NAME = model.EMP_FULL_NAME;
                data.EMP_FATHER_NAM = model.EMP_FATHER_NAM;
                data.DESIG_DESC = model.DESIG_DESC;
                data.DEPARTMENT_DESC = model.DEPARTMENT_DESC;
                data.SHIFT_DESC = model.SHIFT_DESC;
                data.JOINING_DATE = model.JOINING_DATE;
                data.EMP_BANK_ACC_NO = model.EMP_BANK_ACC_NO;
                data.PAY_ORG_DESC = model.PAY_ORG_DESC;
                data.PAY_CC_DESC = model.PAY_CC_DESC;
                data.LastModifiedUserName = model.LastModifiedUserName;
                data.ModifiedBy = model.ModifiedBy;
                data.IsActive = model.IsActive;
                data.LastModified = DateTime.Now;
                
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

        [HttpPost]
        [Route("UpdateUserPassword")]
        public IActionResult UpdateUserPassword([FromBody] PMSUser model)
        {
            try
            {
                bool IsExsist = _db.PMSUser.Where(x => x.Username.ToLower() == model.Username.ToLower().Trim() && x.Id != model.Id).Any();

                if (IsExsist)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Conflict,
                        Message = "UserName Already Exist",
                        Data = null
                    });

                }

                var data = _db.PMSUser.Find(model.Id);
                if (!string.IsNullOrEmpty(model.Password))
                {
                    byte[] passwordHashing, passwordKey;

                    using (var hmac = new HMACSHA512())
                    {
                        passwordKey = hmac.Key;
                        passwordHashing = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(model.Password));

                    }

                    data.PasswordHash = passwordHashing;
                    data.PasswordKey = passwordKey;
                    data.Password = null;
                }
                data.ModifiedBy = model.ModifiedBy;
                data.LastModifiedUserName = model.LastModifiedUserName;
                data.Username = model.Username;

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
        [Route("DeleteUser")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var data = _db.PMSUser.Find(id);
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
