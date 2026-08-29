using B_DB_Context;
using B_DB_Model;
using B_Utility.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChargeGroupTypeController : ControllerBase
    {
        private readonly DataBase_Context _db;
        public ChargeGroupTypeController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("GetAllChargeGroupTypesByChargeSetup")]
        public IActionResult GetAllChargeGroupTypesByChargeSetup(int id)
        {
            try
            {
                var chargeGroupTypes = _db.ChargeGroupType
                                                          .Where(x => !x.IsDeleted && x.GlobalChargeGroupId == id)
                                                          .ToList();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = chargeGroupTypes
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        [Route("AddChargeGroupType")]
        public async Task<Response_Result> AddChargeGroupType(ChargeGroupType model)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                var existingList = _db.ChargeGroupType.Where(x => x.ChargeTypeName == model.ChargeTypeName && x.Id != model.Id && x.GlobalChargeGroupId == model.GlobalChargeGroupId && x.IsDeleted != true).FirstOrDefault();

                if(existingList == null)
                {
                   
                    if (model.Id == 0)
                    {
                    
                        model.CreatedOn = DateTime.Now;
                        model.LastModified = DateTime.Now;
                        model.IsActive = true;
                        model.IsDeleted = false;
                        model.CreatedBy = model.CreatedBy;
                        model.ModifiedBy = model.ModifiedBy;
                        model.LastModifiedUserName = model.LastModifiedUserName;
                        _db.ChargeGroupType.Add(model);
                        _db.SaveChanges();


                        response_Results.message = "Charge Group Type Succesfully Added";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        ChargeGroupType obj = _db.ChargeGroupType.Where(i => i.Id == model.Id).FirstOrDefault();
                        obj.GlobalChargeGroupId = model.GlobalChargeGroupId;
                        obj.ChargeTypeName = model.ChargeTypeName;
                        obj.SapAccount = model.SapAccount;
                        obj.Remarks = model.Remarks;
                        obj.Code = model.Code;
                        obj.LastModified = DateTime.Now;
                        obj.ModifiedBy = model.ModifiedBy;
                        obj.LastModifiedUserName = model.LastModifiedUserName;
                        _db.Update(obj);
                        _db.SaveChanges();
                        response_Results.message = "Charge Group Type Succesfully Updated";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                }
                else
                {
                    response_Results.message = "Charge Group Type Already Exist Against Group";
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
        [Route("GetAllChargeGroupType")]
        public async Task<Response_Result> GetAllChargeGroupType()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                List<ChargeGroupType> list = _db.ChargeGroupType.Where(i => i.IsActive == true)
                                                                .Include(x=>x.GlobalChargeGroup)
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
        [Route("GetSingleChargeGroupType")]
        public async Task<Response_Result> GetSingleChargeGroupType(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                ChargeGroupType obj = _db.ChargeGroupType.Where(i => i.Id == id)
                                                         .Include(x => x.GlobalChargeGroup)
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
        [Route("DeleteChargeGroupType")]
        public async Task<Response_Result> DeleteChargeGroupType(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {


                ChargeGroupType obj = _db.ChargeGroupType.Where(i => i.Id == id).FirstOrDefault();
                obj.IsDeleted = true;
                obj.IsActive = false;
                _db.Update(obj);
                _db.SaveChanges();
                response_Results.message = "Charge Group Type Deleted Successfully";
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
