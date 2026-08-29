using B_DB_Context;
using B_Utility.BLL;
using B_Utility.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingReceiptProcessingController : ControllerBase
    {
        private readonly DataBase_Context _db;

        CommonBLL _commonBLL;
        ApprovalBLL _approvalBLL;
        public BookingReceiptProcessingController(DataBase_Context db)
        {
            _db = db;
            _approvalBLL = new ApprovalBLL(_db);
            _commonBLL = new CommonBLL(_db);
        }

        [HttpGet]
        [Route("/api/BookingReceiptProcessing/GetBookingApprovedFilterList")]
        public IActionResult GetBookingApprovedFilterList()
        {
            try
            {
                var result = _db.StockCreations.Where(x => !x.is_deleted
                                                   && x.IsBookingApproved == true
                                                     )
                                               .ToList();
                foreach (var item in result)
                {
                    item.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(item.Phase));
                    item.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(item.RealStateType));
                    item.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(item.Category));
                    item.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(item.Block));
                    item.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(item.Type));
                }
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }


        [HttpGet]
        [Route("Get")]
        public IActionResult Get(int id)
        {
            try
            {
                var result = _db.StockCreations.Where(x => !x.is_deleted && x.ID == id)
                                                       .Include(x => x.MemberProfile)
                                                       .FirstOrDefault();

                if (result != null)
                {
                    result.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(result.RealStateType));
                    result.ProjectName = _commonBLL.GetProjectName(Convert.ToInt32(result.Project));
                    result.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(result.Phase));
                    result.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(result.Category));
                    result.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(result.Block));
                    result.NatureName = _commonBLL.GetNatureName(Convert.ToInt32(result.Nature));
                    result.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(result.Type));
                }
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }
    }
}
