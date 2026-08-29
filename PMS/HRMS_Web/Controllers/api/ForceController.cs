using Microsoft.AspNetCore.Mvc;
using B_DB_Model;
using B_Utility.Common;
using B_DB_Context;
using Microsoft.AspNetCore.Authorization;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForceController : ControllerBase
    {
        private readonly DataBase_Context _db;
        public ForceController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("AddForce")]
        public async Task<Response_Result> AddForce(Force force)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                var existingList = _db.Forces.Where(x => x.Description == force.Description && x.ID != force.ID && x.is_deleted != true).FirstOrDefault();

                if (existingList == null)
                {
                    if (force.ID == 0)
                    {
                        force.Created_at = DateTime.Now;
                        force.Updated_at = DateTime.Now;
                        force.Updated_By = force.Updated_By;
                        force.Created_By = force.Created_By;
                        force.is_active = true;
                        force.is_deleted = false;
                        _db.Forces.Add(force);
                        _db.SaveChanges();


                        response_Results.message = "Force Succesfully Added";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        Force obj = _db.Forces.Where(i => i.ID == force.ID).FirstOrDefault();
                        obj.Description = force.Description;
                        obj.Code = force.Code;
                        obj.Updated_By = force.Updated_By;
                        obj.Updated_at = DateTime.Now;
                        _db.Update(obj);
                        _db.SaveChanges();
                        response_Results.message = "Force Succesfully Updated";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                }
                else
                {
                    response_Results.message = "Force Already Exist";
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
        [Route("GetAllForces")]
        public async Task<Response_Result> GetAllForces()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                List<Force> forces = _db.Forces.Where(i => i.is_active == true).ToList<Force>();

                response_Results.data = forces;
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
        [Route("GetSingleForce")]
        public async Task<Response_Result> GetSingleForce(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                Force obj = _db.Forces.Where(i => i.ID == id).FirstOrDefault();
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
        [Route("DeleteForce")]
        public async Task<Response_Result> DeleteForce(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {


                Force obj = _db.Forces.Where(i => i.ID == id).FirstOrDefault();
                obj.is_deleted = true;
                obj.is_active = false;
                _db.Update(obj);
                _db.SaveChanges();
                response_Results.message = "Force Deleted Successfully";
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
