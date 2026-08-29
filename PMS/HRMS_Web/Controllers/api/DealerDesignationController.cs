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
    public class DealerDesignationController : ControllerBase
    {
        private readonly DataBase_Context _db;
        public DealerDesignationController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("AddDealerDesignation")]
        public async Task<Response_Result> AddDealerDesignation(DealerDesignation dealerDesignation)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                var existingList = _db.DealerDesignation.Where(x => x.Description == dealerDesignation.Description && x.ID != dealerDesignation.ID && x.is_deleted != true).FirstOrDefault();

                if (existingList == null)
                {
                    if (dealerDesignation.ID == 0)
                    {
                        dealerDesignation.Created_at = DateTime.Now;
                        dealerDesignation.Updated_at = DateTime.Now;
                        dealerDesignation.is_active = true;
                        dealerDesignation.Updated_By = dealerDesignation.Updated_By;
                        dealerDesignation.Created_By = dealerDesignation.Created_By;
                        dealerDesignation.is_deleted = false;
                        _db.DealerDesignation.Add(dealerDesignation);
                        _db.SaveChanges();


                        response_Results.message = "Dealer Designation Succesfully Added";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                    else
                    {
                        DealerDesignation obj = _db.DealerDesignation.Where(i => i.ID == dealerDesignation.ID).FirstOrDefault();
                        obj.Description = dealerDesignation.Description;
                        obj.Code = dealerDesignation.Code;
                        obj.Updated_By = dealerDesignation.Updated_By;
                        obj.Updated_at = DateTime.Now;
                        _db.Update(obj);
                        _db.SaveChanges();
                        response_Results.message = "Dealer Designation Succesfully Updated";
                        response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.succcess);
                    }
                }
                else
                {
                    response_Results.message = "Dealer Designation Already Exist";
                    response_Results.code = Convert.ToInt32(Global_Utility.ResponseCode.conflict);
                }
            }
            catch (Exception ex)
            {

                response_Results.message = ex.Message;

            }
            return response_Results;

        }

        [HttpGet]
        [Route("GetAllDealerDesignation")]
        public async Task<Response_Result> GetAllDealerDesignation()
        {
            Response_Result response_Results = new Response_Result();

            try
            {
                List<DealerDesignation> blocks = _db.DealerDesignation.Where(i => i.is_active == true).ToList<DealerDesignation>();

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
        [Route("GetSingleDealerDesignation")]
        public async Task<Response_Result> GetSingleDealerDesignation(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {
                DealerDesignation obj = _db.DealerDesignation.Where(i => i.ID == id).FirstOrDefault();
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
        [Route("DeleteDealerDesignation")]
        public async Task<Response_Result> DeleteDealerDesignation(int? id)
        {

            Response_Result response_Results = new Response_Result();
            try
            {


                DealerDesignation obj = _db.DealerDesignation.Where(i => i.ID == id).FirstOrDefault();
                obj.is_deleted = true;
                obj.is_active = false;
                _db.Update(obj);
                _db.SaveChanges();
                response_Results.message = "Dealer Designation Deleted Successfully";
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
