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
    public class NDCRequestTypeController : ControllerBase
    {
        private readonly DataBase_Context _db;
        public NDCRequestTypeController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("AddNewNDCRequestType")]
        public async Task<Response_Result> AddNewNDCRequestType(NDCRequestType model)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                if (model.ID == 0)
                {
                    model.Created_at = DateTime.Now;
                    model.Updated_at = DateTime.Now;
                    model.is_active = true;
                    model.is_deleted = false;
                    _db.NDCRequestType.Add(model);
                    _db.SaveChanges();


                    response_Results.message = "NDC Request Type Succesfully Added";
                    response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                }
                else
                {
                    NDCRequestType obj = _db.NDCRequestType.Where(i => i.ID == model.ID).FirstOrDefault();
                    obj.Description = model.Description;
                    obj.Code = model.Code;
                    obj.Updated_at = DateTime.Now;
                    obj.Updated_By = 1;
                    _db.Update(obj);
                    _db.SaveChanges();
                    response_Results.message = "NDC Request Type Succesfully Updated";
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
                List<NDCRequestType> result = _db.NDCRequestType.Where(i => i.is_active == true).ToList<NDCRequestType>();

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
                NDCRequestType result = _db.NDCRequestType.Where(i => i.ID == id).FirstOrDefault();
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
        [Route("DeleteNDCRequestType")]
        public async Task<Response_Result> DeleteNDCRequestType(int id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {


                NDCRequestType obj = _db.NDCRequestType.Where(i => i.ID == id).FirstOrDefault();
                obj.is_deleted = true;
                obj.is_active = false;
                _db.Update(obj);
                _db.SaveChanges();
                response_Results.message = "NDC Request Type Deleted Successfully";
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
