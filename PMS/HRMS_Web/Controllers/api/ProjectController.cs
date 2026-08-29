using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using B_DB_Model;
using B_Utility.Common;
using B_DB_Context;
using B_Utility.BLL;
using Microsoft.AspNetCore.Authorization;
using CloudinaryDotNet.Actions;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {

        private readonly DataBase_Context _db;
        CommonBLL _commonBLL;
        public ProjectController(DataBase_Context db)
        {
            _db = db;
            _commonBLL = new CommonBLL(_db);
        }

        [HttpPost]
        public async Task<Response_Result> AddProject(Project project)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                var existingList = _db.Projects.Where(x => x.Description == project.Description && x.RealStateTypeId == project.RealStateTypeId && x.ID != project.ID && x.is_deleted != true).FirstOrDefault();

                if (existingList == null)
                {
                    if (project.ID == 0)
                    {
                        project.Created_at = DateTime.Now;
                        project.Updated_at = DateTime.Now;
                        project.Updated_By = project.Updated_By;
                        project.Created_By = project.Created_By;
                        project.is_active = true;
                        project.is_deleted = false;
                        _db.Projects.Add(project);
                        _db.SaveChanges();


                        response_Results.message = "Project Succesfully Added";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        Project obj = _db.Projects.Where(i => i.ID == project.ID).FirstOrDefault();
                        obj.RealStateTypeId = project.RealStateTypeId;
                        obj.Description = project.Description;
                        obj.Code = project.Code;
                        obj.Updated_By = project.Updated_By;
                        obj.Updated_at = DateTime.Now;
                        _db.Update(obj);
                        _db.SaveChanges();
                        response_Results.message = "Project Succesfully Updated";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                }
                else
                {
                    response_Results.message = "Project Already Exist";
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
        public async Task<Response_Result> GetAllProjects()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                var blocks = _db.Projects.Where(i => i.is_active == true).ToList();
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
        public async Task<Response_Result> GetSingleProject(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                Project obj = _db.Projects.Where(i => i.ID == id).FirstOrDefault();
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
        public async Task<Response_Result> DeleteProject(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {


                Project obj = _db.Projects.Where(i => i.ID == id).FirstOrDefault();
                obj.is_deleted = true;
                obj.is_active = false;
                _db.Update(obj);
                _db.SaveChanges();
                response_Results.message = "Project Deleted Successfully";
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
