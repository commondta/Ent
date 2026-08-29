using B_DB_Context;
using B_DB_Model;
using B_Utility.Common;
using HRMS_Web.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Results;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RenumberController : ControllerBase
    {
        private readonly DataBase_Context _db;

        public RenumberController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("GetAllRenumberRequest")]
        public IActionResult GetAllRenumberRequest()
        {
            try
            {
                var result = _db.RenumberHistories.Select(x => new
                {
                    x.Id,
                    x.CurrentPropertyRegistrationNo,
                    x.CurrentPropertyPropertyNo,
                    x.ProposedPropertyPropertyNo,
                    x.CreatedOn
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
        [Route("GetRenumberRequestForPrint")]
        public IActionResult GetRenumberRequestForPrint(int id)
        {
            try
            {
                var result = _db.RenumberHistories
                                .Where(x=>x.Id == id)
                                .Select(x => new
                                {
                                    x.Id,
                                    RegistrationNo =x.CurrentPropertyRegistrationNo,                                   
                                    MemberName = x.CurrentPropertyMemberName,
                                    MemberAddress = x.CurrentPropertyMemberAddress ?? "N/A",
                                    MemberMobile = x.CurrentPropertyMemberMobile ?? "N/A",
                                    OldPropertyNo = x.CurrentPropertyPropertyNo,
                                    OldBlock = x.CurrentPropertyBlock,
                                    NewPropertyNo = x.ProposedPropertyPropertyNo,
                                    NewBlock = x.ProposedPropertyBlock,
                                    DocDate = x.CreatedOn
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

        [HttpPost]
        [Route("/api/Renumber/SaveRenumber")]
        public IActionResult SaveRenumber(List<SaveRenumberDto> dto)
        {
            try
            {
                if(dto.Count() <= 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "Please add rows",
                        Data = null
                    });
                }

                var distinctRegIds = dto.Select(x => x.RegStockId).Distinct().ToList();
                var distinctPropIds = dto.Select(x => x.PropStockId).Distinct().ToList();

                if (distinctRegIds.Count != dto.Count || distinctPropIds.Count != dto.Count)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "Duplicates found in the values",
                        Data = null
                    });
                }
              
                var stock = _db.StockCreations.Where(x=> x.is_deleted != true).ToList();
                foreach (var item in dto)
                {
                    var existingReg = stock.FirstOrDefault(x => x.ID == item.RegStockId);
                    var newProp = stock.FirstOrDefault(x => x.ID == item.PropStockId);

                    RenumberHistery renumberHistory = new RenumberHistery();

                    var memberDetail = _db.MemberProfile
                                          .Where(x => x.Id == existingReg.MemberProfileId)
                                          .Select(x => new {
                                              x.MemberName,
                                              x.Cnic,
                                              x.PermanentAddress,
                                              x.Mobile
                                          })
                                          .FirstOrDefault();


                    renumberHistory.CurrentPropertyRegistrationNo = existingReg.RegistrationNo;
                    renumberHistory.CurrentPropertyPropertyNo = existingReg.PropertyNo;
                    renumberHistory.CurrentPropertyMemberName = memberDetail.MemberName;
                    renumberHistory.CurrentPropertyCNIC = memberDetail.Cnic;
                    renumberHistory.CurrentPropertyMemberAddress =memberDetail.PermanentAddress;
                    renumberHistory.CurrentPropertyMemberMobile = memberDetail.Mobile;
                    renumberHistory.CurrentPropertyBlock = _db.Blocks.Where(x => x.ID == Convert.ToInt32(existingReg.Block)).FirstOrDefault().Description ?? "N/A";
                    renumberHistory.CurrentPropertyCategory = _db.Categories.Where(x => x.ID == Convert.ToInt32(existingReg.Category)).FirstOrDefault().Description ?? "N/A";
                    renumberHistory.CurrentPropertySize = existingReg.coveredArea == null ? existingReg.ActualSize : existingReg.coveredArea.ToString();
                    renumberHistory.ProposedPropertyPropertyNo = newProp.PropertyNo;
                    renumberHistory.ProposedPropertyBlock = _db.Blocks.Where(x => x.ID == Convert.ToInt32(newProp.Block)).FirstOrDefault().Description ?? "N/A";
                    renumberHistory.ProposedPropertyCategory = _db.Categories.Where(x => x.ID == Convert.ToInt32(newProp.Category)).FirstOrDefault().Description ?? "N/A";
                    renumberHistory.ProposedPropertySize = newProp.coveredArea == null ? newProp.ActualSize : newProp.coveredArea.ToString();
                    renumberHistory.CreatedOn = (DateTime)item.RenmumberDate;

                    _db.RenumberHistories.Add(renumberHistory);

                    existingReg.PropertyNo = newProp.PropertyNo;
                    existingReg.PrefixProperty = newProp.PrefixProperty;
                    existingReg.numForProperty = newProp.numForProperty;
                    existingReg.postfixForProperty = newProp.postfixForProperty;
                    existingReg.Block = newProp.Block;
                    existingReg.Category = newProp.Category;
                    existingReg.RealStateType = newProp.RealStateType;
                    existingReg.ActualSize = newProp.ActualSize;
                    existingReg.ActualSizeUnit = newProp.ActualSizeUnit;
                    existingReg.Project = newProp.Project;
                    existingReg.Phase = newProp.Phase;
                    existingReg.Type = newProp.Type;
                    existingReg.Nature = newProp.Nature;
                    existingReg.Finishing = newProp.Finishing;
                    existingReg.Floor = newProp.Floor;
                    existingReg.PossessionStatus = newProp.PossessionStatus;
                    existingReg.ConstracutionStatus = newProp.ConstracutionStatus;
                    existingReg.coveredArea = newProp.coveredArea;
                    existingReg.LDAPlotNo = newProp.LDAPlotNo;

                    _db.StockCreations.Update(existingReg);
      

                    var newPropWTaxrecord = _db.WithHoldingTaxPropertyWise.Where(x => x.StockCreationId == item.PropStockId).ToList();
                    var newPropfixchargesrecord = _db.PropertyFixedChargesSetup.Where(x => x.StockCreationId == item.PropStockId).ToList();
                    if (newPropWTaxrecord.Count() > 0)
                    {
                        _db.WithHoldingTaxPropertyWise.RemoveRange(newPropWTaxrecord);

                    }
                    if (newPropfixchargesrecord.Count() > 0)
                    {
                        _db.PropertyFixedChargesSetup.RemoveRange(newPropfixchargesrecord);
     
                    }
                    
                    _db.StockCreations.Remove(newProp); 
                }
                _db.SaveChanges();

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
