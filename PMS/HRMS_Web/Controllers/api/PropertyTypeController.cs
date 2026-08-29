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
    public class PropertyTypeController : ControllerBase
    {


        private readonly DataBase_Context _db;
        public PropertyTypeController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<Response_Result> SavePropertyType(PropertyType finish)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                var existingList = _db.PropertyTypes.Where(x => x.Description == finish.Description && x.ID != finish.ID && x.is_deleted != true).FirstOrDefault();

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
                        _db.PropertyTypes.Add(finish);
                        _db.SaveChanges();


                        response_Results.message = "Property Type Succesfully Added";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        PropertyType obj = _db.PropertyTypes.Where(i => i.ID == finish.ID).FirstOrDefault();
                        obj.Code = finish.Code;
                        obj.Description = finish.Description;
                        obj.Updated_By = finish.Updated_By;


                        obj.Updated_at = DateTime.Now;
                        _db.Update(obj);
                        _db.SaveChanges();
                        response_Results.message = "Property Type Succesfully Updated";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                }
                else
                {
                    response_Results.message = "Proprty Type Already Exist";
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
        public async Task<Response_Result> GetAllPropertyTypes()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                List<PropertyType> blocks = _db.PropertyTypes.Where(i => i.is_active == true).ToList<PropertyType>();

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
        public async Task<Response_Result> DeletePropertyType(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {


                PropertyType obj = _db.PropertyTypes.Where(i => i.ID == id).FirstOrDefault();
                obj.is_deleted = true;
                obj.is_active = false;

                _db.Update(obj);
                _db.SaveChanges();
                response_Results.message = "Property Type Definition Deleted Successfully";
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
        public async Task<Response_Result> GetSinglePropertyType(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                PropertyType obj = _db.PropertyTypes.Where(i => i.ID == id).FirstOrDefault();
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
