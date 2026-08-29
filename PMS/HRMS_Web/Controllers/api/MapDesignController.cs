using B_DB_Context;
using B_DB_Model;
using B_Utility.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MapDesignController : ControllerBase
    {
        private readonly DataBase_Context _db;
        public MapDesignController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("AddMapDesign")]
        public async Task<Response_Result> AddMapDesign(MapDesign mapDesign)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                var existingList = _db.MapDesigns.Where(x => x.Description == mapDesign.Description && x.ID != mapDesign.ID && x.is_deleted != true).FirstOrDefault();

                if (existingList == null)
                {
                    if (mapDesign.ID == 0)
                    {
                        mapDesign.Created_at = DateTime.Now;
                        mapDesign.Updated_at = DateTime.Now;
                        mapDesign.is_active = true;
                        mapDesign.is_deleted = false;
                        _db.MapDesigns.Add(mapDesign);
                        _db.SaveChanges();


                        response_Results.message = "Map Design Succesfully Added";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        MapDesign obj = _db.MapDesigns.Where(i => i.ID == mapDesign.ID).FirstOrDefault();
                        obj.Description = mapDesign.Description;
                        obj.Code = mapDesign.Code;
                        obj.Updated_at = DateTime.Now;
                        _db.Update(obj);
                        _db.SaveChanges();
                        response_Results.message = "Map Design Succesfully Updated";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                }
                else
                {
                    response_Results.message = "Map Design Already Exist";
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
        [Route("GetAllMapDesigns")]
        public async Task<Response_Result> GetAllMapDesigns()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                List<MapDesign> blocks = _db.MapDesigns.Where(i => i.is_active == true).ToList<MapDesign>();

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
        [Route("GetSingleMapDesign")]

        public async Task<Response_Result> GetSingleMapDesign(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                MapDesign obj = _db.MapDesigns.Where(i => i.ID == id).FirstOrDefault();
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
        [Route("DeleteMapDesign")]

        public async Task<Response_Result> DeleteMapDesign(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {


                MapDesign obj = _db.MapDesigns.Where(i => i.ID == id).FirstOrDefault();
                obj.is_deleted = true;
                obj.is_active = false;
                _db.Update(obj);
                _db.SaveChanges();
                response_Results.message = "Map Design Deleted Successfully";
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
