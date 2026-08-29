using B_DB_Context;
using B_DB_Model;
using B_Utility.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerificationTypeController : ControllerBase
    {
        private readonly DataBase_Context _db;
        public VerificationTypeController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("AddVerificationType")]
        public async Task<Response_Result> AddVerificationType(VerificationType verificationType)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                var existingList = _db.VerificationTypes.Where(x => x.Description == verificationType.Description && x.ID != verificationType.ID && x.is_deleted != true).FirstOrDefault();

                if (existingList == null)
                {
                    if (verificationType.ID == 0)
                    {
                        verificationType.Created_at = DateTime.Now;
                        verificationType.Updated_at = DateTime.Now;
                        verificationType.Created_By = verificationType.Created_By;
                        verificationType.Updated_By = verificationType.Updated_By;
                        verificationType.is_active = true;
                        verificationType.is_deleted = false;
                        _db.VerificationTypes.Add(verificationType);
                        _db.SaveChanges();


                        response_Results.message = "Verification Type Succesfully Added";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        VerificationType obj = _db.VerificationTypes.Where(i => i.ID == verificationType.ID).FirstOrDefault();
                        obj.Description = verificationType.Description;
                        obj.Code = verificationType.Code;
                        obj.Updated_By = verificationType.Updated_By;
                        obj.Updated_at = DateTime.Now;
                        _db.Update(obj);
                        _db.SaveChanges();
                        response_Results.message = "Verification Type Succesfully Updated";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                }
                else
                {
                    response_Results.message = "Verification Type Already Exist";
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
        [Route("GetAllVerificationTypes")]
        public async Task<Response_Result> GetAllVerificationTypes()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                List<VerificationType> verificationTypes = _db.VerificationTypes.Where(i => i.is_active == true).ToList<VerificationType>();

                response_Results.data = verificationTypes;
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
        [Route("GetSingleVerificationType")]
        public async Task<Response_Result> GetSingleVerificationType(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                VerificationType obj = _db.VerificationTypes.Where(i => i.ID == id).FirstOrDefault();
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
        [Route("DeleteVerificationType")]
        public async Task<Response_Result> DeleteVerificationType(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {


                VerificationType obj = _db.VerificationTypes.Where(i => i.ID == id).FirstOrDefault();
                obj.is_deleted = true;
                obj.is_active = false;
                _db.Update(obj);
                _db.SaveChanges();
                response_Results.message = "Verification Type Deleted Successfully";
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
