using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using B_DB_Model;

using B_Utility.Common;
using B_DB_Context;
using Microsoft.EntityFrameworkCore;
using B_Utility.BLL;
using Microsoft.AspNetCore.Authorization;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize]

    public class RealEstateController : ControllerBase
    {

        private readonly DataBase_Context _db;
        CommonBLL _commonBLL;
        public RealEstateController(DataBase_Context db)
        {
            _db = db;
            _commonBLL = new CommonBLL(_db);
        }

        [HttpGet]
        public async Task<Response_Result> GetRealEstatesByPhaseId(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                var obj = _db.Real_Estates.Where(i => i.PhaseId == id && i.is_active==true).ToList();
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

        [HttpPost]
        public async Task<Response_Result> AddRealEstate(Real_Estate block)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                var existingList = _db.Real_Estates.Where(x => x.Description == block.Description && x.PhaseId == block.PhaseId && x.ID != block.ID && x.is_deleted !=true).FirstOrDefault();

                if (existingList == null)
                {
                   if (block.ID == 0)
                    {
                        block.Created_at = DateTime.Now;
                        block.Updated_at = DateTime.Now;
                        block.Created_By = block.Created_By;
                        block.Updated_By = block.Updated_By;
                        block.is_active = true;
                        block.is_deleted = false;
                        _db.Real_Estates.Add(block);
                        _db.SaveChanges();

                        response_Results.message = "Real Estate Succesfully Added";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        Real_Estate obj = _db.Real_Estates.Where(i => i.ID == block.ID).FirstOrDefault();
                        obj.Description = block.Description;
                        obj.PhaseId = block.PhaseId;
                        obj.Updated_By = block.Updated_By;
                        obj.Updated_at = DateTime.Now;
                        _db.Update(obj);
                        _db.SaveChanges();
                        response_Results.message = "Real Estate Succesfully Updated";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                }
                else
                {
                    response_Results.message = "Real Estate Type Already Exist";
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
        [AllowAnonymous]
        public async Task<Response_Result> GetAllRealEstate()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                var blocks = _db.Real_Estates.Where(i => i.is_active == true).ToList();

                if (blocks?.Count > 0)
                {
                    foreach (var block in blocks)
                    {
                         block.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(block.PhaseId));
                    }
                }

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
        public async Task<Response_Result> GetSingleRealEstate(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                Real_Estate obj = _db.Real_Estates.Where(i => i.ID == id).FirstOrDefault();
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
        public async Task<Response_Result> DeleteRealEstate(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {


                Real_Estate obj = _db.Real_Estates.Where(i => i.ID == id).FirstOrDefault();
                obj.is_deleted = true;
                obj.is_active = false;
                _db.Update(obj);
                _db.SaveChanges();
                response_Results.message = "Real Estate Deleted Successfully";
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
