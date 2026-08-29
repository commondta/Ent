using B_DB_Context;
using B_DB_Model;
using B_Utility.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransferTypeController : ControllerBase
    {
        private readonly DataBase_Context _db;
        public TransferTypeController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("AddNewTransferType")]
        public async Task<Response_Result> AddNewTransferType(TransferType model)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                if (model.ID == 0)
                {
                    model.Created_at = DateTime.Now;
                    model.Created_By = model.Created_By;
                    model.Updated_By = model.Updated_By;
                    model.Updated_at = DateTime.Now;
                    model.is_active = true;
                    model.is_deleted = false;
                    _db.TransferType.Add(model);
                    _db.SaveChanges();


                    response_Results.message = "Transfer Type Succesfully Added";
                    response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                }
                else
                {
                    TransferType obj = _db.TransferType.Where(i => i.ID == model.ID).FirstOrDefault();
                    obj.Description = model.Description;
                    obj.Updated_By = model.Updated_By;
                    obj.Code = model.Code;
                    obj.Updated_at = DateTime.Now;
                    obj.Updated_By = 1;
                    _db.Update(obj);
                    _db.SaveChanges();
                    response_Results.message = "Transfer Type Succesfully Updated";
                    response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
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
        [Route("GetAll")]
        public async Task<Response_Result> GetAll()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                List<TransferType> result = _db.TransferType.Where(i => i.is_active == true).ToList<TransferType>();

                response_Results.data = result;
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
        [Route("Get")]
        public async Task<Response_Result> Get(int id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                TransferType result = _db.TransferType.Where(i => i.ID == id).FirstOrDefault();
                response_Results.data = result;

                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);

            }
            catch (Exception ex)
            {

                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response_Results.message = ex.Message;

            }
            return response_Results;

        }
        [HttpDelete]
        [Route("DeleteTransferType")]
        public async Task<Response_Result> DeleteTransferType(int id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {


                TransferType obj = _db.TransferType.Where(i => i.ID == id).FirstOrDefault();
                obj.is_deleted = true;
                obj.is_active = false;
                _db.Update(obj);
                _db.SaveChanges();
                response_Results.message = "Transfer Type Deleted Successfully";
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
