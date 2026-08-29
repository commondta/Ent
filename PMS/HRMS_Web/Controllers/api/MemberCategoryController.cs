using B_DB_Context;
using B_DB_Model;
using B_Utility.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberCategoryController : ControllerBase
    {
        private readonly DataBase_Context _db;
        public MemberCategoryController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("AddMemberCategory")]
        public async Task<Response_Result> AddMemberCategory(MemberCategory membercategory)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                var existingList = _db.MemberCategorys.Where(x => x.Description == membercategory.Description && x.ID != membercategory.ID && x.is_deleted != true).FirstOrDefault();

                if (existingList == null)
                {
                    if (membercategory.ID == 0)
                    {
                        membercategory.Created_at = DateTime.Now;
                        membercategory.Updated_at = DateTime.Now;
                        membercategory.Created_By = membercategory.Created_By;
                        membercategory.Updated_By = membercategory.Updated_By;
                        membercategory.is_active = true;
                        membercategory.is_deleted = false;
                        _db.MemberCategorys.Add(membercategory);
                        _db.SaveChanges();


                        response_Results.message = "Member Category Succesfully Added";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        MemberCategory obj = _db.MemberCategorys.Where(i => i.ID == membercategory.ID).FirstOrDefault();
                        obj.Description = membercategory.Description;
                        obj.Code = membercategory.Code;
                        obj.Updated_By = membercategory.Updated_By;
                        obj.Updated_at = DateTime.Now;
                        _db.Update(obj);
                        _db.SaveChanges();
                        response_Results.message = "Member Category Succesfully Updated";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                }
                else
                {
                    response_Results.message = "Member Category Already Exist";
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
        [Route("GetAllMemberCategorys")]
        public async Task<Response_Result> GetAllMemberCategorys()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                List<MemberCategory> membercategorys = _db.MemberCategorys.Where(i => i.is_active == true).ToList<MemberCategory>();

                response_Results.data = membercategorys;
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
        [Route("GetSingleMemberCategory")]
        public async Task<Response_Result> GetSingleMemberCategory(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                MemberCategory obj = _db.MemberCategorys.Where(i => i.ID == id).FirstOrDefault();
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
        [Route("DeleteMemberCategory")]
        public async Task<Response_Result> DeleteMemberCategory(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {


                MemberCategory obj = _db.MemberCategorys.Where(i => i.ID == id).FirstOrDefault();
                obj.is_deleted = true;
                obj.is_active = false;
                _db.Update(obj);
                _db.SaveChanges();
                response_Results.message = "Member Category Deleted Successfully";
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
