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
    public class UOMController : ControllerBase
    {
        private readonly DataBase_Context _db;
        CommonBLL _commonBLL;
        public UOMController(DataBase_Context db)
        {
            _db = db;
            _commonBLL = new CommonBLL(_db);
        }

        [HttpPost]
        public async Task<Response_Result> AddSize(UOM size)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                var existingList = _db.UOM.Where(x => x.Description == size.Description && x.ID != size.ID && x.is_deleted != true).FirstOrDefault();

                if (existingList == null)
                {
                    if (size.ID == 0)
                    {
                        size.Created_at = DateTime.Now;
                        size.Updated_at = DateTime.Now;
                        size.Updated_By = size.Updated_By;
                        size.Created_By = size.Created_By;
                        size.is_active = true;
                        size.is_deleted = false;
                        _db.UOM.Add(size);
                        _db.SaveChanges();


                        response_Results.message = "UOM definition Succesfully Added";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        UOM obj = _db.UOM.Where(i => i.ID == size.ID).FirstOrDefault();
                        obj.Description = size.Description;
                        obj.Updated_By = size.Updated_By;
                        obj.Updated_at = DateTime.Now;
                        _db.Update(obj);
                        _db.SaveChanges();
                        response_Results.message = "UOM Definition Succesfully Updated";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                }
                else
                {
                    response_Results.message = "UOM Already Exist";
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
        public async Task<Response_Result> GetAllSizes()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                var blocks = _db.UOM.Where(i => i.is_active == true).ToList();

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
        public async Task<Response_Result> GetSingleSize(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                UOM obj = _db.UOM.Where(i => i.ID == id).FirstOrDefault();
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
        public async Task<Response_Result> DeleteSize(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {


                UOM obj = _db.UOM.Where(i => i.ID == id).FirstOrDefault();
                obj.is_deleted = true;
                obj.is_active = false;
                _db.Update(obj);
                _db.SaveChanges();
                response_Results.message = "Size def Deleted Successfully";
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
