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
    public class PhaseController : ControllerBase
    {

        private readonly DataBase_Context _db;
        public PhaseController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<Response_Result> AddPhase(Phase phase)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                var existingList = _db.Phases.Where(x => x.Description == phase.Description && x.ID != phase.ID && x.is_deleted != true).FirstOrDefault();

                if (existingList == null)
                {
                    if (phase.ID == 0)
                    {
                        phase.Created_at = DateTime.Now;
                        phase.Updated_at = DateTime.Now;
                        phase.Updated_By = phase.Updated_By;
                        phase.Created_By = phase.Created_By;
                        phase.is_active = true;
                        phase.is_deleted = false;
                        _db.Phases.Add(phase);
                        _db.SaveChanges();


                        response_Results.message = "Phase Def Succesfully Added";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        Phase obj = _db.Phases.Where(i => i.ID == phase.ID).FirstOrDefault();
                        obj.Description = phase.Description;
                        obj.Updated_By = phase.Updated_By;
                        obj.Code = phase.Code;
                        obj.Updated_at = DateTime.Now;
                        _db.Update(obj);
                        _db.SaveChanges();
                        response_Results.message = "Phase Def Succesfully Updated";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                }
                else
                {
                    response_Results.message = "Phase Already Exist";
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
        public async Task<Response_Result> GetAllPhases()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                List<Phase> blocks = _db.Phases.Where(i => i.is_active == true).ToList<Phase>();

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
        public async Task<Response_Result> GetSinglePhase(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                Phase obj = _db.Phases.Where(i => i.ID == id).FirstOrDefault();
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
        public async Task<Response_Result> DeletePhase(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {


                Phase obj = _db.Phases.Where(i => i.ID == id).FirstOrDefault();
                obj.is_deleted = true;
                obj.is_active = false;
                _db.Update(obj);
                _db.SaveChanges();
                response_Results.message = "Phase Deleted Successfully";
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
