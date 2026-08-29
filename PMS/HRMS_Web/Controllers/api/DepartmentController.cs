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
    //[Authorize]
    [AllowAnonymous]
    public class DepartmentController : ControllerBase
    {
        private readonly DataBase_Context _db;
        public DepartmentController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("AddNewDepartment")]
        public async Task<Response_Result> AddNewDepartment(Department model)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                var existingList = _db.Departments.Where(x => x.Description.ToLower() == model.Description.ToLower().Trim() && x.ID != model.ID && x.is_deleted != true).FirstOrDefault();

                if (existingList == null)
                {
                    if (model.ID == 0)
                    {
                        model.Created_at = DateTime.Now;
                        model.Updated_at = DateTime.Now;
                        model.is_active = true;
                        model.Updated_By = model.Updated_By;
                        model.Created_By = model.Created_By;
                        model.is_deleted = false;
                        _db.Departments.Add(model);
                        _db.SaveChanges();


                        response_Results.message = "Department Succesfully Added";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        Department obj = _db.Departments.Where(i => i.ID == model.ID).FirstOrDefault();
                        obj.Description = model.Description;
                        obj.Updated_By = model.Updated_By;
                        obj.Code = model.Code;
                        obj.Updated_at = DateTime.Now;
                        _db.Update(obj);
                        _db.SaveChanges();
                        response_Results.message = "Department Succesfully Updated";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                }
                else
                {
                    response_Results.message = "Department Already Exist";
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
        [Route("GetAll")]
        public async Task<Response_Result> GetAll()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                List<Department> result = _db.Departments.Where(i => i.is_active == true).ToList<Department>();

                response_Results.data = result;
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
        [Route("Get")]
        public async Task<Response_Result> Get(int id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                Department result = _db.Departments.Where(i => i.ID == id).FirstOrDefault();
                response_Results.data = result;

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
        [Route("DeleteDepartment")]
        public async Task<Response_Result> DeleteDepartment(int id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {


                Department obj = _db.Departments.Where(i => i.ID == id).FirstOrDefault();
                obj.is_deleted = true;
                obj.is_active = false;
                _db.Update(obj);
                _db.SaveChanges();
                response_Results.message = "Department Deleted Successfully";
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
