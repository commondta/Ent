using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize]
    public class RolesPermissionsController : ControllerBase
    {
        private readonly DataBase_Context _db;

        public RolesPermissionsController( DataBase_Context db )
        {
            _db = db;
        }


        // GET: api/<RolesPermissionsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<RolesPermissionsController>/5
        [HttpGet("{roleName}")]
        public async Task<Response_Result> GetPermissions( string roleName )
        {
            Response_Result response_Results = new Response_Result();

            var response=_db.RolesPermissions.FirstOrDefault(x => x.Designation== roleName);
            var permissions=_db.Permissions.Where(x=>x.RolesPermissionsId== response.Id).ToList();
            response.Permissions=permissions;
             response_Results.data = response;
            return response_Results;
        }

        // POST api/<RolesPermissionsController>
        [HttpPost]
        public async Task<Response_Result> Create( [FromBody] RolesPermissions requestBody )
        {
            Response_Result response_Results = new Response_Result();
            

            try
            {
                var alreadyExistingRole=_db.RolesPermissions.FirstOrDefault(x=>x.Designation==requestBody.Designation && !x.IsDeleted);
                if (alreadyExistingRole!=null)
                {
                    response_Results.message = "Roles with same designation already exists";
                    response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.conflict);
                    return response_Results;
                }
                requestBody.CreatedOn = DateTime.Now;
                requestBody.LastModified = DateTime.Now;
                requestBody.IsActive = true;
                requestBody.IsDeleted = false;
                _db.RolesPermissions.Add(requestBody);
                _db.SaveChanges();


                response_Results.message = "Roles permission added successfully";
                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
            }
            catch ( Exception ex )
            {
                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response_Results.message = ex.Message;
            }

            return response_Results;
        }

        // PUT api/<RolesPermissionsController>/5
        [HttpPut("{id}")]
        public async Task<Response_Result> Update( int id, [FromBody] RolesPermissions requestBody )
        {

            Response_Result response_Results = new Response_Result();

            try
            {
                requestBody.Id=id;
                requestBody.LastModified = DateTime.Now;
                requestBody.IsActive = true;
                requestBody.IsDeleted = false;

                    var removePermissions = _db.Permissions.Where(x => x.RolesPermissionsId==requestBody.Id).ToList();
                    _db.Permissions.RemoveRange(removePermissions);
                    _db.SaveChanges();
                _db.RolesPermissions.Update(requestBody);
                _db.SaveChanges();


                response_Results.message = "Roles permission added successfully";
                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
            }
            catch ( Exception ex )
            {
                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response_Results.message = ex.Message;
            }

            return response_Results;
        }

        // DELETE api/<RolesPermissionsController>/5
        [HttpDelete("{id}")]
        public async Task<Response_Result> DeletePermission( int id )
        {
            Response_Result response_Results = new Response_Result();
            try
            {
                var removePermissions = _db.Permissions.Where(x => x.Id == id).ToList();
                _db.Permissions.RemoveRange(removePermissions);
                _db.SaveChanges();

                response_Results.message = "Roles permissions deleted successfully";
                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
            }
            catch(Exception ex )
            {
                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response_Results.message = ex.Message;
            }
            return response_Results;
        }

        [HttpDelete("{id}")]
        public async Task<Response_Result> DeleteRole( int id )
        {
            Response_Result response_Results = new Response_Result();
            try
            {
                var removePermissions = _db.Permissions.Where(x => x.RolesPermissionsId == id && !x.IsDeleted).ToList();
                _db.Permissions.RemoveRange(removePermissions);
                _db.SaveChanges();

                var removeRole=_db.RolesPermissions.Where(x => x.Id==id).ToList();
                _db.RolesPermissions.RemoveRange(removeRole);
                _db.SaveChanges();

                response_Results.message = "Role deleted successfully";
                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
            }
            catch ( Exception ex )
            {
                response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.exception);
                response_Results.message = ex.Message;
            }
            return response_Results;
        }

        [HttpGet]
        public async Task<Response_Result> GetDesignations()
        {
            Response_Result response_Results = new Response_Result();
            try 
            {
                var response = _db.PMSUser.ToList().Select(x=> new { designation = x.DESIG_DESC}).Distinct();
                response_Results.data= response;
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
