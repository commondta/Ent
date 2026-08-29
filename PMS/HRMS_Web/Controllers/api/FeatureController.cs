using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using B_DB_Model;
using B_Utility.Common;
using B_DB_Context;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize]
    public class FeatureController : ControllerBase
    {

        private readonly DataBase_Context _db;
        public FeatureController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<Response_Result> AddFeature(Feature feature)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                var existingList = _db.Features.Where(x => x.Description == feature.Description && x.is_deleted != true).FirstOrDefault();

                if (existingList == null)
                {
                    if (feature.ID == 0)
                    {
                        feature.Created_at = DateTime.Now;
                        feature.Created_By = feature.Created_By;
                        feature.Updated_By = feature.Updated_By;
                        feature.Updated_at = DateTime.Now;
                        feature.is_active = true;
                        feature.is_deleted = false;
                        _db.Features.Add(feature);
                        _db.SaveChanges();


                        response_Results.message = "Feature Def Succesfully Added";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        Feature obj = _db.Features.Where(i => i.ID == feature.ID).FirstOrDefault();
                        obj.Description = feature.Description;
                        obj.Updated_By = feature.Updated_By;
                        obj.Code = feature.Code;
                        obj.Updated_at = DateTime.Now;
                        _db.Update(obj);
                        _db.SaveChanges();
                        response_Results.message = "Feature Def Succesfully Updated";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                }
                else
                {
                    response_Results.message = "Feature Already Exist";
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
        public async Task<Response_Result> GetAllFeatures()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                List<Feature> blocks = _db.Features.Where(i => i.is_active == true).ToList<Feature>();

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
        public async Task<Response_Result> GetSingleFeatures(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                Feature obj = _db.Features.Where(i => i.ID == id).FirstOrDefault();
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
        public async Task<Response_Result> DeleteFeature(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {


                Feature obj = _db.Features.Where(i => i.ID == id).FirstOrDefault();
                obj.is_deleted = true;
                obj.is_active = false;
                _db.Update(obj);
                _db.SaveChanges();
                response_Results.message = "Feature Def Deleted Successfully";
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
