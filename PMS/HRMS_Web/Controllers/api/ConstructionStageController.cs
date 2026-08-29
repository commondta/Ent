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
    public class ConstructionStageController : ControllerBase
    {


        private readonly DataBase_Context _db;
        public ConstructionStageController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<Response_Result> SaveConstruction(ConstructionStage finish)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                var existingList = _db.ConstructionStages.Where(x => x.Name == finish.Name && x.ID != finish.ID && x.is_deleted != true).FirstOrDefault();

                if (existingList == null)
                {
                        if (finish.ID == 0)
                    {
                        finish.Created_at = DateTime.Now;
                        finish.Updated_at = DateTime.Now;
                        finish.Created_By = finish.Created_By;
                        finish.Updated_By = finish.Updated_By;
                        finish.is_active = true;
                        finish.is_deleted = false;
                        _db.ConstructionStages.Add(finish);
                        _db.SaveChanges();


                        response_Results.message = "Construction Stage Succesfully Added";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        ConstructionStage obj = _db.ConstructionStages.Where(i => i.ID == finish.ID).FirstOrDefault();
                        obj.Updated_By = finish.Updated_By;
                        obj.Code = finish.Code;
                        obj.Name = finish.Name;
                        obj.Remarks = finish.Remarks;
                 
                        obj.Updated_at = DateTime.Now;
                        _db.Update(obj);
                        _db.SaveChanges();
                        response_Results.message = "Construction Stage Succesfully Updated";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                }
                else
                {
                    response_Results.message = "Construction Stage Already Exist";
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
        public async Task<Response_Result> GetAllConstructions()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                List<ConstructionStage> blocks = _db.ConstructionStages.Where(i => i.is_active == true).ToList<ConstructionStage>();

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
        public async Task<Response_Result> DeleteConstruction(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {


                ConstructionStage obj = _db.ConstructionStages.Where(i => i.ID == id).FirstOrDefault();
                obj.is_deleted = true;
                obj.is_active = false;

                _db.Update(obj);
                _db.SaveChanges();
                response_Results.message = "Construction Stage Definition Deleted Successfully";
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
        public async Task<Response_Result> GetSingleConstruction(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                ConstructionStage obj = _db.ConstructionStages.Where(i => i.ID == id).FirstOrDefault();
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
    }
}
