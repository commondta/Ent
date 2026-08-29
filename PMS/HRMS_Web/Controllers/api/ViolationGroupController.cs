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
    public class ViolationGroupController : ControllerBase
    {
        private readonly DataBase_Context _db;
        public ViolationGroupController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("/api/ViolationGroup/AddGroup")]
        public async Task<Response_Result> AddGroup(ViolationGroup model)
        {

            Response_Result response_Results = new Response_Result();

            try
            {
                var exist = _db.ViolationGroup.Where(x => x.ViolationGroupName == model.ViolationGroupName).FirstOrDefault();

                if (exist == null)
                { 
                    if (model.Id == 0)
                    {
                        model.CreatedOn = DateTime.Now;
                        model.LastModified = DateTime.Now;
                        model.ModifiedBy = model.ModifiedBy;
                        model.CreatedBy = model.CreatedBy;
                        model.LastModifiedUserName = model.LastModifiedUserName;
                        model.IsActive = true;
                        model.IsDeleted = false;
                        _db.ViolationGroup.Add(model);
                        _db.SaveChanges();


                        response_Results.message = "Violation Group Succesfully Added";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        ViolationGroup obj = _db.ViolationGroup.Where(i => i.Id == model.Id).FirstOrDefault();
                        obj.ViolationGroupName = model.ViolationGroupName;
                        obj.Remarks = model.Remarks;
                        obj.ModifiedBy = model.ModifiedBy;
                        obj.LastModifiedUserName = model.LastModifiedUserName;

                        obj.LastModified = DateTime.Now;
                        _db.Update(obj);
                        _db.SaveChanges();
                        response_Results.message = "Violation Group Succesfully Updated";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                }
                else
                {
                    response_Results.message = "Violation Group Type Already Exist";
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
        [Route("/api/ViolationGroup/GetAllViolationGroup")]
        public async Task<Response_Result> GetAllViolationGroup()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                List<ViolationGroup> list = _db.ViolationGroup.Where(i => i.IsActive == true).ToList();

                response_Results.data = list;
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
        [Route("/api/ViolationGroup/GetSingleViolationGroup")]
        public async Task<Response_Result> GetSingleViolationGroup(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                ViolationGroup obj = _db.ViolationGroup.Where(i => i.Id == id).FirstOrDefault();
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
        [Route("/api/ViolationGroup/DeleteViolationGroup")]
        public async Task<Response_Result> DeleteViolationGroup(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                ViolationGroup obj = _db.ViolationGroup.Where(i => i.Id == id).FirstOrDefault();
                obj.IsDeleted = true;
                obj.IsActive = false;
                _db.Update(obj);
                _db.SaveChanges();
                response_Results.message = "Violation Group Deleted Successfully";
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
