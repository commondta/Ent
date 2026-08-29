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
    public class SectorController : ControllerBase
    {
        private readonly DataBase_Context _db;
        public SectorController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("AddSector")]
        public async Task<Response_Result> AddSector(Sector sector)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                var existingList = _db.Sectors.Where(x => x.Description == sector.Description && x.ID != sector.ID && x.is_deleted != true).FirstOrDefault();

                if (existingList == null)
                {
                    if (sector.ID == 0)
                    {
                        sector.Created_at = DateTime.Now;
                        sector.Updated_at = DateTime.Now;
                        sector.Created_By = sector.Created_By;
                        sector.Updated_By = sector.Updated_By;
                        sector.is_active = true;
                        sector.is_deleted = false;
                        _db.Sectors.Add(sector);
                        _db.SaveChanges();


                        response_Results.message = "Sector Succesfully Added";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        Sector obj = _db.Sectors.Where(i => i.ID == sector.ID).FirstOrDefault();
                        obj.Description = sector.Description;
                        obj.Code = sector.Code;
                        obj.Updated_By = sector.Updated_By;
                        obj.Updated_at = DateTime.Now;
                        _db.Update(obj);
                        _db.SaveChanges();
                        response_Results.message = "Sector Succesfully Updated";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                }
                else
                {
                    response_Results.message = "Sector Already Exist";
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
        [Route("GetAllSectors")]
        public async Task<Response_Result> GetAllSectors()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                List<Sector> sectors = _db.Sectors.Where(i => i.is_active == true).ToList<Sector>();

                response_Results.data = sectors;
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
        [Route("GetSingleSector")]
        public async Task<Response_Result> GetSingleSector(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                Sector obj = _db.Sectors.Where(i => i.ID == id).FirstOrDefault();
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
        [Route("DeleteSector")]
        public async Task<Response_Result> DeleteSector(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {


                Sector obj = _db.Sectors.Where(i => i.ID == id).FirstOrDefault();
                obj.is_deleted = true;
                obj.is_active = false;
                _db.Update(obj);
                _db.SaveChanges();
                response_Results.message = "Sector Deleted Successfully";
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
