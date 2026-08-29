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
    public class SocialStatusController : ControllerBase
    {
        private readonly DataBase_Context _db;
        public SocialStatusController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("AddNewSocialStatus")]
        public async Task<Response_Result> AddNewSocialStatus(SocialStatus model)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                var existingList = _db.SocialStatus.Where(x => x.Description == model.Description && x.ID != model.ID && x.is_deleted != true).FirstOrDefault();

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
                        _db.SocialStatus.Add(model);
                        _db.SaveChanges();


                        response_Results.message = "Social Status Succesfully Added";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        SocialStatus obj = _db.SocialStatus.Where(i => i.ID == model.ID).FirstOrDefault();
                        obj.Description = model.Description;
                        obj.Code = model.Code;
                        obj.Updated_By = model.Updated_By;
                        obj.Updated_at = DateTime.Now;
                        _db.Update(obj);
                        _db.SaveChanges();
                        response_Results.message = "Social Status Succesfully Updated";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                }
                else
                {
                    response_Results.message = "Social Status Already Exist";
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
        [Route("GetAll")]
        public async Task<Response_Result> GetAll()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                List<SocialStatus> result = _db.SocialStatus.Where(i => i.is_active == true).ToList<SocialStatus>();

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
                SocialStatus result = _db.SocialStatus.Where(i => i.ID == id).FirstOrDefault();
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
        [Route("DeleteSocialStatus")]
        public async Task<Response_Result> DeleteSocialStatus(int id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                SocialStatus obj = _db.SocialStatus.Where(i => i.ID == id).FirstOrDefault();
                if (obj != null)
                {
                    obj.is_deleted = true;
                    obj.is_active = false;
                    _db.Update(obj);
                    _db.SaveChanges();
                    response_Results.message = "Social Status Deleted Successfully";
                    response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                }
                else
                {
                    response_Results.message = "Social Status Not Found!";
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
    }
}


