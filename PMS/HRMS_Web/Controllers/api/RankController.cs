
using Microsoft.AspNetCore.Mvc;
using B_DB_Model;
using B_Utility.Common;
using B_DB_Context;
using Microsoft.AspNetCore.Authorization;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RankController : ControllerBase
    {
        private readonly DataBase_Context _db;
        public RankController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("AddRank")]
        public async Task<Response_Result> AddRank(Rank rank)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                var existingList = _db.Ranks.Where(x => x.Description == rank.Description && x.ID != rank.ID && x.is_deleted != true).FirstOrDefault();

                if (existingList == null)
                {
                    if (rank.ID == 0)
                    {
                        rank.Created_at = DateTime.Now;
                        rank.Updated_at = DateTime.Now;
                        rank.Created_By = rank.Created_By;
                        rank.Updated_By = rank.Updated_By;
                        rank.is_active = true;
                        rank.is_deleted = false;
                        _db.Ranks.Add(rank);
                        _db.SaveChanges();


                        response_Results.message = "Rank Succesfully Added";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        Rank obj = _db.Ranks.Where(i => i.ID == rank.ID).FirstOrDefault();
                        obj.Description = rank.Description;
                        obj.Code = rank.Code;
                        obj.Updated_at = DateTime.Now;
                        _db.Update(obj);
                        _db.SaveChanges();
                        response_Results.message = "Rank Succesfully Updated";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                }
                else
                {
                    response_Results.message = "Rank Already Exist";
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
        [Route("GetAllRanks")]
        public async Task<Response_Result> GetAllRanks()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                List<Rank> ranks = _db.Ranks.Where(i => i.is_active == true).ToList<Rank>();

                response_Results.data = ranks;
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
        [Route("GetSingleRank")]
        public async Task<Response_Result> GetSingleRank(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                Rank obj = _db.Ranks.Where(i => i.ID == id).FirstOrDefault();
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
        [Route("DeleteRank")]
        public async Task<Response_Result> DeleteRank(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {


                Rank obj = _db.Ranks.Where(i => i.ID == id).FirstOrDefault();
                obj.is_deleted = true;
                obj.is_active = false;
                _db.Update(obj);
                _db.SaveChanges();
                response_Results.message = "Rank Deleted Successfully";
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
