using B_DB_Context;
using B_DB_Model;
using B_Utility.Common;
using HRMS_Web.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class LDAPlotNoController : ControllerBase
    {

        private readonly DataBase_Context _db;

        public LDAPlotNoController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("/api/LDAPlotNo/SaveLDAPlotNoDTO")]
        public IActionResult SaveLDAPlotNoDTO(LDAPlotNoDTO dto)
        {
            try
            {
                StockCreation stockCreation = _db.StockCreations.Find(dto.StockId);

                stockCreation.LDAPlotNo = dto.LDAPlotNo;
                stockCreation.LDAAreaSize = dto.LDAAreaSize;
                stockCreation.ID = (int)dto.StockId;
                _db.SaveChanges();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = dto
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetPropertiesForLDAPlotNo")]
        public IActionResult GetPropertiesForLDAPlotNo()
        {
            try
            {
                var result = _db.StockCreations.Where(x => !x.is_deleted &&
                                                x.Is_ClearnceApproved == true &&
                                                x.RegistrationNo != null &&
                                                x.PropertyNo != null)
                                               .Distinct()
                                               .Select(x => new
                                               {
                                                   x.ID,
                                                   x.RegistrationNo,
                                                   x.PropertyNo,
                                               })
                                               .ToList();

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
        [Route("GetFilterProperty")]
        public IActionResult GetFilterProperty(int id)
        {
            try
            {

                var result = _db.StockCreations.Where(x => x.ID == id &&
                                                      x.RegistrationNo != null 
                                                     )
                                           .Select(x => new
                                           {
                                               x.ID,
                                               RegistrationNo = x.RegistrationNo ?? "N/A",
                                               PropertyNo = x.PropertyNo ?? "N/A",
                                               x.Status,
                                               x.ActualSize,
                                               BlockName = _db.Blocks.Where(p => p.ID == (Convert.ToInt32(x.Block))).Select(x => x.Description).FirstOrDefault(),
                                               PhaseName = _db.Phases.Where(p => p.ID == (Convert.ToInt32(x.Phase))).Select(x => x.Description).FirstOrDefault(),
                                               CategoryName = _db.Categories.Where(p => p.ID == (Convert.ToInt32(x.Category))).Select(x => x.Description).FirstOrDefault(),
                                               x.LDAPlotNo,
                                               x.LDAAreaSize
                                           })
                                           .FirstOrDefault();

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
