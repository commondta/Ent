using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


using static System.Reflection.Metadata.BlobBuilder;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly DataBase_Context _db;

        CommonBLL _commonBLL;
        public CategoryController(DataBase_Context db)
        {
            _db = db;
            _commonBLL = new CommonBLL(_db);
        }

        [HttpPost]
        public async Task<Response_Result> AddCategory(Category category)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                // var existingList = _db.Categories.Where(x => x.Description == category.Description && x.ID != category.ID && x.is_deleted != true).FirstOrDefault();
                var existingList = _db.Categories.Where(x => x.Description == category.Description && x.RealStateTypeId == category.RealStateTypeId && x.ID != category.ID && x.is_deleted != true).FirstOrDefault();
                if (existingList == null)
                {
                    if (category.ID == 0)
                    {
                        category.Created_at = DateTime.Now;
                        category.Updated_at = DateTime.Now;
                        category.Created_By = category.Created_By;
                        category.Updated_By = category.Updated_By;
                        category.is_active = true;
                        category.is_deleted = false;
                        _db.Categories.Add(category);
                        _db.SaveChanges();


                        response_Results.message = "Category Succesfully Added";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        Category obj = _db.Categories.Where(i => i.ID == category.ID).FirstOrDefault();
                        obj.RealStateTypeId = category.RealStateTypeId;
                        obj.Description = category.Description;
                        obj.Code = category.Code;
                        obj.Updated_By = category.Updated_By;
                        obj.UOM = category.UOM;
                        obj.ConstructionGracePeriod = category.ConstructionGracePeriod;
                        obj.Updated_at = DateTime.Now;
                        _db.Update(obj);
                        _db.SaveChanges();
                        response_Results.message = "Category Succesfully Updated";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                }
                else
                {
                    response_Results.message = "Category Already Exist";
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
        public async Task<Response_Result> GetAllCategories()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                List<Category> categories = _db.Categories.Where(i => i.is_active == true).ToList<Category>();
                if (categories?.Count > 0)
                {
                    foreach (var block in categories)
                    {
                        block.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(block.RealStateTypeId));
                    }
                }
                response_Results.data = categories;
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
        public async Task<Response_Result> GetSingleCategory(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                Category obj = _db.Categories.Where(i => i.ID == id).FirstOrDefault();
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
        public async Task<Response_Result> DeleteCategory(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {


                Category obj = _db.Categories.Where(i => i.ID == id).FirstOrDefault();
                obj.is_deleted = true;
                obj.is_active = false;
                _db.Update(obj);
                _db.SaveChanges();
                response_Results.message = "Category Deleted Successfully";
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
