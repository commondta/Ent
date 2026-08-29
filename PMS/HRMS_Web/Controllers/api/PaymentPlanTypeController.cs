using B_DB_Context;
using B_DB_Model;
using B_Utility.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentPlanTypeController : ControllerBase
    {

        private readonly DataBase_Context _db;
        public PaymentPlanTypeController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("/api/PaymentPlanType/AddPaymentPlanType")]
        public async Task<Response_Result> AddPaymentPlanType(PaymentPlanType model)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                var existingList = _db.PaymentPlanType.Where(x => x.Description == model.Description && x.ID != model.ID && x.is_deleted != true).FirstOrDefault();

                if (existingList == null)
                {
                    if (model.ID == 0)
                    {
                        model.Created_at = DateTime.Now;
                        model.Updated_at = DateTime.Now;
                        model.Created_By = model.Created_By;
                        model.Updated_By = model.Updated_By;
                        model.is_active = true;
                        model.is_deleted = false;
                        _db.PaymentPlanType.Add(model);
                        _db.SaveChanges();


                        response_Results.message = "PaymentPlan Type Def Succesfully Added";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        PaymentPlanType obj = _db.PaymentPlanType.Where(i => i.ID == model.ID).FirstOrDefault();
                        obj.Description = model.Description;
                        obj.Updated_By = model.Updated_By;
                         obj.Updated_at = DateTime.Now;
                        _db.Update(obj);
                        _db.SaveChanges();
                        response_Results.message = "PaymentPlan Type Def Succesfully Updated";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                }
                else
                {
                    response_Results.message = "PaymentPlan Type Already Exist";
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
        [Route("/api/PaymentPlanType/GetAllPaymentPlanTypes")]
        public async Task<Response_Result> GetAllPaymentPlanTypes()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                List<PaymentPlanType> blocks = _db.PaymentPlanType.Where(i => i.is_active == true).ToList<PaymentPlanType>();

                response_Results.data = blocks;
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
        [Route("/api/PaymentPlanType/GetSinglePaymentPlanType")]
        public async Task<Response_Result> GetSinglePaymentPlanType(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                PaymentPlanType obj = _db.PaymentPlanType.Where(i => i.ID == id).FirstOrDefault();
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
        [Route("/api/PaymentPlanType/DeletePaymentPlanType")]
        public async Task<Response_Result> DeletePaymentPlanType(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {


                PaymentPlanType obj = _db.PaymentPlanType.Where(i => i.ID == id).FirstOrDefault();
                obj.is_deleted = true;
                obj.is_active = false;
                _db.Update(obj);
                _db.SaveChanges();
                response_Results.message = "PaymentPlan Type Deleted Successfully";
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
