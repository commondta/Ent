using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize]
    public class NatureController : ControllerBase
    {
        private readonly DataBase_Context _db;
        CommonBLL _commonBLL;
        public NatureController(DataBase_Context db)
        {
            _db = db;
            _commonBLL = new CommonBLL(_db);
        }

        [HttpPost]
        public async Task<Response_Result> AddNature(Nature nature)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                var existingList = _db.Natures.Where(x => x.Description == nature.Description && x.RealStateTypeId == nature.RealStateTypeId && x.ID != nature.ID && x.is_deleted != true).FirstOrDefault();

                if (existingList == null)
                {
                    if (nature.ID == 0)
                    {
                        nature.Created_at = DateTime.Now;
                        nature.Updated_at = DateTime.Now;
                        nature.Created_By = nature.Created_By;
                        nature.Updated_By = nature.Updated_By;
                        nature.is_active = true;
                        nature.is_deleted = false;
                        _db.Natures.Add(nature);
                        _db.SaveChanges();


                        response_Results.message = "Nature Succesfully Added";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        Nature obj = _db.Natures.Where(i => i.ID == nature.ID).FirstOrDefault();
                        obj.RealStateTypeId = nature.RealStateTypeId;
                        obj.Description = nature.Description;
                        obj.Code = nature.Code;
                        obj.Updated_By = nature.Updated_By;
                        obj.Updated_at = DateTime.Now;
                        _db.Update(obj);
                        _db.SaveChanges();
                        response_Results.message = "Nature Succesfully Updated";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                }
                else
                {
                    response_Results.message = "Property Nature Already Exist";
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
        public async Task<Response_Result> GetAllNatures()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                var blocks = _db.Natures.Where(i => i.is_active == true).ToList();
                if (blocks?.Count > 0)
                {
                    foreach (var block in blocks)
                    {
                        block.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(block.RealStateTypeId));
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
        public async Task<Response_Result> GetSingleNature(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                Nature obj = _db.Natures.Where(i => i.ID == id).FirstOrDefault();
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
        public async Task<Response_Result> DeleteNature(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {


                Nature obj = _db.Natures.Where(i => i.ID == id).FirstOrDefault();
                obj.is_deleted = true;
                obj.is_active = false;
                _db.Update(obj);
                _db.SaveChanges();
                response_Results.message = "Nature Deleted Successfully";
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
