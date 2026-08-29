using B_DB_Context;
using B_DB_Model;
using B_Utility.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize]
    public class GlobalSetupController : ControllerBase
    {
        private readonly DataBase_Context _db;
        public GlobalSetupController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<Response_Result> AddGroup(GlobalChargeGroup model)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                var existingList = _db.GlobalChargeGroup.Where(x => x.ChargeGroupName == model.ChargeGroupName && x.Id != model.Id && x.IsDeleted != true).FirstOrDefault();

                if (existingList == null)
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
                        _db.GlobalChargeGroup.Add(model);
                        _db.SaveChanges();


                        response_Results.message = "Global Charge Group Succesfully Added";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        GlobalChargeGroup obj = _db.GlobalChargeGroup.Where(i => i.Id == model.Id).FirstOrDefault();
                        obj.ChargeGroupName = model.ChargeGroupName;
                        obj.Remarks = model.Remarks;
                        obj.ModifiedBy = model.ModifiedBy;
                        obj.LastModifiedUserName = model.LastModifiedUserName;

                        obj.LastModified = DateTime.Now;
                        _db.Update(obj);
                        _db.SaveChanges();
                        response_Results.message = "Global Charge Group Succesfully Updated";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                }
                else
                {
                    response_Results.message = "Global Charge Group Already Exist";
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
        public async Task<Response_Result> GetGlobalChargeGroup()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                List<GlobalChargeGroup> list = _db.GlobalChargeGroup.Where(i => i.IsActive == true).ToList();

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
        public async Task<Response_Result> GetSingleGlobalChargeGroup(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                GlobalChargeGroup obj = _db.GlobalChargeGroup.Where(i => i.Id == id).FirstOrDefault();
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
        public async Task<Response_Result> DeleteGlobalChargeGroup(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {


                GlobalChargeGroup obj = _db.GlobalChargeGroup.Where(i => i.Id == id).FirstOrDefault();
                obj.IsDeleted = true;
                obj.IsActive = false;
                _db.Update(obj);
                _db.SaveChanges();
                response_Results.message = "Global Charge Group Deleted Successfully";
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
