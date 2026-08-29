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
    public class ApprovalUISetupController : ControllerBase
    {
        private readonly DataBase_Context _db;

        CommonBLL _commonBLL;

        public ApprovalUISetupController(DataBase_Context db)
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

                var result = _db.ApprovalUI.Where(x => x.Id == id)
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
                var result = _db.ApprovalUI.ToList();

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
        [Route("AddNewApprovalUISetup")]
        public IActionResult AddNewApprovalUISetup([FromBody] ApprovalUI model)
        {
            try
            {
                bool IsExsist = _db.ApprovalUI.Where(x => x.ModuleORSubModule.ToLower() == model.ModuleORSubModule.ToLower().Trim()).Any();

                if (IsExsist)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Conflict,
                        Message = "ModuleOrSubModule Already Exist",
                        Data = null
                    });

                }
                else
                {
                    _db.ApprovalUI.Add(model);
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
        [Route("UpdateApprovalUISetup")]
        public IActionResult UpdateApprovalUISetup([FromBody] ApprovalUI model)
        {
            try
            {
                bool IsExsist = _db.ApprovalUI.Where(x => x.ModuleORSubModule.ToLower() == model.ModuleORSubModule.ToLower().Trim() && x.Id != model.Id).Any();

                if (IsExsist)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Conflict,
                        Message = "ModuleOrSubModule Already Exist",
                        Data = null
                    });

                }

                var data = _db.ApprovalUI.Find(model.Id);
                data.ModuleORSubModule = model.ModuleORSubModule;
                data.SerialNo = model.SerialNo;
                data.Level = model.Level;
                data.ParentId = model.ParentId;
                data.Checked = model.Checked;
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
        [Route("DeleteApprovalUISetup")]
        public IActionResult DeleteApprovalUISetup(int id)
        {
            try
            {
                var data = _db.ApprovalUI.Find(id);
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
