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
    public class DeAllocationController : ControllerBase
    {
        private readonly DataBase_Context _db;

        public DeAllocationController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("/api/DeAllocation/SaveDeAllocation")]
        public IActionResult SaveDeAllocation(List<SaveDeAllocationDto> dto)
        {
            try
            {
                if (dto.Count() <= 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "Please add rows",
                        Data = null
                    });
                }

                var distinctRegIds = dto.Select(x => x.RegStockId).Distinct().ToList();
               
                if (distinctRegIds.Count != dto.Count)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "Duplicates found in the values",
                        Data = null
                    });
                }

                var stock = _db.StockCreations.Where(x => x.is_deleted != true).ToList();
                foreach (var item in dto)
                {
                    var existingReg = stock.FirstOrDefault(x => x.ID == item.RegStockId);

                    string PropertyNo = existingReg.PropertyNo;
                    string PrefixProperty = existingReg.PrefixProperty;
                    int? numForProperty = existingReg.numForProperty;
                    string postfixForProperty = existingReg.postfixForProperty;
                    string Block = existingReg.Block;
                    string Category = existingReg.Category;
                    string RealStateType = existingReg.RealStateType;
                    string ActualSize = existingReg.ActualSize;
                    string ActualSizeUnit = existingReg.ActualSizeUnit;
                    string Project = existingReg.Project;
                    string Phase = existingReg.Phase;
                    string Type = existingReg.Type;
                    string Nature = existingReg.Nature;
                    string Finishing = existingReg.Finishing;
                    string Floor = existingReg.Floor;
                    bool? PossessionStatus = existingReg.PossessionStatus;
                    string ConstracutionStatus = existingReg.ConstracutionStatus;
                    decimal? coveredArea = existingReg.coveredArea;
                    string? LDAPlotNo = existingReg.LDAPlotNo;
                    string? LDAAreaSize = existingReg.LDAAreaSize;

                    existingReg.PropertyNo = null;
                    _db.StockCreations.Update(existingReg);
                    _db.SaveChanges();
                    

                    existingReg.ID = 0;
                    existingReg.RegistrationNo= null;
                    existingReg.PropertyNo = PropertyNo;
                    existingReg.PrefixProperty = PrefixProperty;
                    existingReg.numForProperty = numForProperty;
                    existingReg.postfixForProperty = postfixForProperty;
                    existingReg.Block = Block;
                    existingReg.Category = Category;
                    existingReg.RealStateType = RealStateType;
                    existingReg.ActualSize = ActualSize;
                    existingReg.ActualSizeUnit = ActualSizeUnit;
                    existingReg.Project = Project;
                    existingReg.Phase = Phase;
                    existingReg.Type = Type;
                    existingReg.Nature = Nature;
                    existingReg.Finishing = Finishing;
                    existingReg.Floor = Floor;
                    existingReg.PossessionStatus = PossessionStatus;
                    existingReg.ConstracutionStatus = ConstracutionStatus;
                    existingReg.coveredArea = coveredArea;
                    existingReg.LDAPlotNo = LDAPlotNo;
                    existingReg.LDAAreaSize = LDAPlotNo;
                    _db.StockCreations.Add(existingReg);
                    _db.SaveChanges();
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = null
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }
    }
}
