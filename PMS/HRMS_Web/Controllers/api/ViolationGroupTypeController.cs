using B_DB_Context;
using B_DB_Model;
using B_Utility.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ViolationGroupTypeController : ControllerBase
    {
        private readonly DataBase_Context _db;
        public ViolationGroupTypeController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("/api/ViolationGroupType/GetAllViolationGroupTypesByViolationGroup")]
        public IActionResult GetAllViolationGroupTypesByViolationGroup()
        {
            try
            {
                var result = _db.ViolationGroupType.Where(x => !x.IsDeleted)
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
        [Route("/api/ViolationGroupType/AddViolationGroupType")]
        public async Task<Response_Result> AddViolationGroupType(ViolationGroupType model)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                var exist = _db.ViolationGroupType.Where(x => x.ViolationTypeName == model.ViolationTypeName).FirstOrDefault();

                if (exist == null)
                {
                    if (model.Id == 0)
                    {
                        model.CreatedOn = DateTime.Now;
                        model.LastModified = DateTime.Now;
                        model.CreatedBy = model.CreatedBy;
                        model.ModifiedBy = model.ModifiedBy;
                        model.LastModifiedUserName = model.LastModifiedUserName;
                        model.IsActive = true;
                        model.IsDeleted = false;
                        _db.ViolationGroupType.Add(model);
                        _db.SaveChanges();


                        response_Results.message = "Violation Group Type Succesfully Added";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        ViolationGroupType obj = _db.ViolationGroupType.Where(i => i.Id == model.Id).FirstOrDefault();
                        obj.ViolationGroupId = model.ViolationGroupId;
                        obj.ViolationTypeName = model.ViolationTypeName;
                        obj.ModifiedBy = model.ModifiedBy;
                        obj.LastModifiedUserName = model.LastModifiedUserName;
                        obj.Code = model.Code;
                        obj.LastModified = DateTime.Now;
                        _db.Update(obj);
                        _db.SaveChanges();
                        response_Results.message = "Violation Group Type Succesfully Updated";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                }
                else
                {
                    response_Results.message = "Violation Group Type Already Exist";
                    response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.conflict);
                }
            }

            catch (Exception ex)
            {

                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response_Results.message = ex.Message;

            }
            return response_Results;

        }

        [HttpGet]
        [Route("/api/ViolationGroupType/GetAllViolationGroupType")]
        public async Task<Response_Result> GetAllViolationGroupType()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                var list = _db.ViolationGroupType.Where(i => i.IsActive == true)
                                                 .Include(x=>x.ViolationGroup)
                                                 .ToList();

                response_Results.data = list;
                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);

            }
            catch (Exception ex)
            {

                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response_Results.message = ex.Message;

            }
            return response_Results;
        }


        [HttpGet]
        [Route("/api/ViolationGroupType/GetSingleChargeGroupType")]
        public async Task<Response_Result> GetSingleViolationGroupType(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                var obj = _db.ViolationGroupType.Where(i => i.Id == id)
                                                               .Include(x => x.ViolationGroup)
                                                               .FirstOrDefault();
                response_Results.data = obj;

                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);

            }
            catch (Exception ex)
            {

                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response_Results.message = ex.Message;

            }
            return response_Results;
        }

        [HttpGet]
        [Route("/api/ViolationGroupType/DeleteViolationGroupType")]
        public async Task<Response_Result> DeleteViolationGroupType(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {

                ViolationGroupType obj = _db.ViolationGroupType.Where(i => i.Id == id).FirstOrDefault();
                obj.IsDeleted = true;
                obj.IsActive = false;
                _db.Update(obj);
                _db.SaveChanges();
                response_Results.message = "Violation Group Type Deleted Successfully";
                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);

            }
            catch (Exception ex)
            {

                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response_Results.message = ex.Message;

            }
            return response_Results;
        }
    }
}
