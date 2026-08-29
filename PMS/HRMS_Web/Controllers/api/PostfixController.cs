using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using B_DB_Model;

using B_Utility.Common;
using B_DB_Context;
using Microsoft.AspNetCore.Authorization;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize]
    public class PostfixController : ControllerBase
    {

        private readonly DataBase_Context _db;
        public PostfixController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<Response_Result> AddPrefix(Postfix block)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                var existingList = _db.Postfixes.Where(x => x.Description == block.Description && x.ID != block.ID && x.is_deleted != true).FirstOrDefault();

                if (existingList == null)
                {
                    if (block.ID == 0)
                    {
                        block.Created_at = DateTime.Now;
                        block.Updated_at = DateTime.Now;
                        block.Updated_By = block.Updated_By;
                        block.Created_By = block.Created_By;
                        block.is_active = true;
                        block.is_deleted = false;
                        _db.Postfixes.Add(block);
                        _db.SaveChanges();


                        response_Results.message = "Postfix Succesfully Added";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        Postfix obj = _db.Postfixes.Where(i => i.ID == block.ID).FirstOrDefault();
                        obj.Description = block.Description;
                        obj.Code = block.Code;
                        obj.Remarks = block.Remarks;
                        obj.Updated_By = block.Updated_By;
                        obj.Updated_at = DateTime.Now;
                        _db.Update(obj);
                        _db.SaveChanges();
                        response_Results.message = "Postfix Succesfully Updated";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                }
                else
                {
                    response_Results.message = "Postfix Already Exist";
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
        public async Task<Response_Result> GetAllPrefixes()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                List<Postfix> blocks = _db.Postfixes.Where(i => i.is_active == true).ToList<Postfix>();

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
        public async Task<Response_Result> GetSinglePrefix(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                Postfix obj = _db.Postfixes.Where(i => i.ID == id).FirstOrDefault();
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
        public async Task<Response_Result> DeletePrefix(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {


                Postfix obj = _db.Postfixes.Where(i => i.ID == id).FirstOrDefault();
                obj.is_deleted = true;
                obj.is_active = false;
                _db.Update(obj);
                _db.SaveChanges();
                response_Results.message = "Postfix Deleted Successfully";
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
