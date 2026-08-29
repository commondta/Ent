using B_DB_Context;
using B_DB_Model;
using B_Utility.Common;
using HRMS_Web.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class COPController : ControllerBase
    {
        private readonly DataBase_Context _db;

        public COPController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("GetAllCOPRequest")]
        public IActionResult GetAllCOPRequest()
        {
            try
            {
                var result = _db.COPHistories.Select(x => new
                {
                    x.Id,
                    x.CurrentPropertyRegistrationNo,
                    x.CurrentPropertyPropertyNo,
                    x.ProposedPropertyRegistrationNo,
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
        [Route("GeFileCOPRequest")]
        public IActionResult GeFileCOPRequest(int id)
        {
            try
            {
                var result = _db.COPHistories.Where(x => x.Id == id)
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

        [HttpGet]
        [Route("GetCOPRequestForPrint")]
        public IActionResult GetRenumberRequestForPrint(int id)
        {
            try
            {
                var result = _db.COPHistories
                                .Where(x => x.Id == id)
                                .Select(x => new
                                {
                                    x.Id,
                                    ARegistrationNo = x.CurrentPropertyRegistrationNo,
                                    AMemberName = x.CurrentPropertyMemberName,
                                    AMemberAddress = x.CurrentPropertyMemberAddress ?? "N/A",
                                    AMemberMobile = x.CurrentPropertyMemberMobile ?? "N/A",
                                    AOldPropertyNo = x.CurrentPropertyPropertyNo,
                                    AOldBlock = x.CurrentPropertyBlock,
                                    ANewPropertyNo = x.ProposedPropertyPropertyNo,
                                    ANewBlock = x.ProposedPropertyBlock,
                                    BRegistrationNo = x.ProposedPropertyRegistrationNo,
                                    BMemberName = x.ProposedPropertyMemberName,
                                    BMemberAddress = x.ProposedPropertyMemberAddress ?? "N/A",
                                    BMemberMobile = x.ProposedPropertyMemberMobile ?? "N/A",
                                    BOldPropertyNo = x.ProposedPropertyPropertyNo,
                                    BOldBlock = x.ProposedPropertyBlock,
                                    BNewPropertyNo = x.CurrentPropertyPropertyNo,
                                    BNewBlock = x.CurrentPropertyBlock,
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
        [Route("/api/COP/SaveCOP")]
        public IActionResult SaveCOP(COPDto dto)
        {
            try
            {
                if (dto.StockIdA == 0 || dto.StockIdB == 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Please select both properties",
                        Data = null
                    });
                }
                else
                {
                    var a = _db.StockCreations.Where(x => x.ID == dto.StockIdA).FirstOrDefault();
                    var b = _db.StockCreations.Where(x => x.ID == dto.StockIdB).FirstOrDefault();

                    string aPropertyNo = b.PropertyNo;
                    string aPrefixProperty = b.PrefixProperty;
                    int? anumForProperty = b.numForProperty;
                    string apostfixForProperty = b.postfixForProperty;
                    string aBlock = b.Block;
                    string aCategory = b.Category;
                    string aRealStateType = b.RealStateType;
                    string aActualSize = b.ActualSize;
                    string aActualSizeUnit = b.ActualSizeUnit;
                    string aProject = b.Project;
                    string aPhase = b.Phase;
                    string aType = b.Type;
                    string aNature = b.Nature;
                    string aFinishing = b.Finishing;
                    string aFloor = b.Floor;
                    bool? aPossessionStatus = b.PossessionStatus;
                    string aConstracutionStatus = b.ConstracutionStatus;
                    decimal? acoveredArea = b.coveredArea;
                    string? aLdaPlotNo = b.LDAPlotNo;

                    string bPropertyNo = a.PropertyNo;
                    string bPrefixProperty = a.PrefixProperty;
                    int? bnumForProperty = a.numForProperty;
                    string bpostfixForProperty = a.postfixForProperty;
                    string bBlock = a.Block;
                    string bCategory = a.Category;
                    string bRealStateType = a.RealStateType;
                    string bActualSize = a.ActualSize;
                    string bActualSizeUnit = a.ActualSizeUnit;
                    string bProject = a.Project;
                    string bPhase = a.Phase;
                    string bType = a.Type;
                    string bNature = a.Nature;
                    string bFinishing = a.Finishing;
                    string bFloor = a.Floor;
                    bool? bPossessionStatus = a.PossessionStatus;
                    string bConstracutionStatus = a.ConstracutionStatus;
                    decimal? bcoveredArea = a.coveredArea;
                    string? bLdaPlotNo = a.LDAPlotNo;

                    a.PropertyNo = aPropertyNo;
                    a.PrefixProperty = aPrefixProperty;
                    a.numForProperty = anumForProperty;
                    a.postfixForProperty = apostfixForProperty;
                    a.Block = aBlock;
                    a.Category = aCategory;
                    a.RealStateType = aRealStateType;
                    a.ActualSize = aActualSize;
                    a.ActualSizeUnit = aActualSizeUnit;
                    a.Project = aProject;
                    a.Phase = aPhase;
                    a.Type = aType;
                    a.Nature = aNature;
                    a.Finishing = aFinishing;
                    a.Floor = aFloor;
                    a.PossessionStatus = aPossessionStatus;
                    a.ConstracutionStatus = aConstracutionStatus;
                    a.coveredArea = acoveredArea;
                    a.LDAPlotNo = aLdaPlotNo;

                    _db.StockCreations.Update(a);
                    // _db.SaveChanges();

                    b.PropertyNo = bPropertyNo;
                    b.PrefixProperty = bPrefixProperty;
                    b.numForProperty = bnumForProperty;
                    b.postfixForProperty = bpostfixForProperty;
                    b.Block = bBlock;
                    b.Category = bCategory;
                    b.RealStateType = bRealStateType;
                    b.ActualSize = bActualSize;
                    b.ActualSizeUnit = bActualSizeUnit;
                    b.Project = bProject;
                    b.Phase = bPhase;
                    b.Type = bType;
                    b.Nature = bNature;
                    b.Finishing = bFinishing;
                    b.Floor = bFloor;
                    b.PossessionStatus = bPossessionStatus;
                    b.ConstracutionStatus = bConstracutionStatus;
                    b.coveredArea = bcoveredArea;
                    b.LDAPlotNo = aLdaPlotNo;

                    _db.StockCreations.Update(b);
                    //_db.SaveChanges();

                    COPHistery cOPHistory = new COPHistery();

                    var memberDetailA = _db.MemberProfile
                                         .Where(x => x.Id == a.MemberProfileId)
                                         .Select(x => new
                                         {
                                             x.MemberName,
                                             x.Cnic,
                                             x.PermanentAddress,
                                             x.Mobile
                                         })
                                         .FirstOrDefault();
                    var memberDetailB = _db.MemberProfile
                                         .Where(x => x.Id == b.MemberProfileId)
                                         .Select(x => new
                                         {
                                             x.MemberName,
                                             x.Cnic,
                                             x.PermanentAddress,
                                             x.Mobile
                                         })
                                         .FirstOrDefault();

                    cOPHistory.CurrentPropertyRegistrationNo = a.RegistrationNo;
                    cOPHistory.CurrentPropertyPropertyNo = a.PropertyNo;
                    cOPHistory.CurrentPropertyMemberCode = a.MemberProfileId.ToString();
                    cOPHistory.CurrentPropertyMemberName = memberDetailA.MemberName;
                    cOPHistory.CurrentPropertyMemberAddress = memberDetailA.PermanentAddress;
                    cOPHistory.CurrentPropertyMemberMobile = memberDetailA.Mobile;
                    cOPHistory.CurrentPropertyMemberCnic = memberDetailA.Cnic;
                    cOPHistory.CurrentPropertyBlock = _db.Blocks.Where(x => x.ID == Convert.ToInt32(a.Block)).FirstOrDefault().Description ?? "N/A";
                    cOPHistory.CurrentPropertyCategory = _db.Categories.Where(x => x.ID == Convert.ToInt32(a.Category)).FirstOrDefault().Description ?? "N/A";
                    cOPHistory.CurrentPropertySize = a.coveredArea == null ? a.ActualSize : a.coveredArea.ToString();
                    cOPHistory.CurrentPropertyConstructionStatus = a.ConstracutionStatus;
                    cOPHistory.CurrentPropertyPossessionStatus = a.PossessionStatus == true ? "Yes" : "No";
                    cOPHistory.ProposedPropertyRegistrationNo = b.RegistrationNo;
                    cOPHistory.ProposedPropertyPropertyNo = b.PropertyNo;
                    cOPHistory.ProposedPropertyMemberCode = b.MemberProfileId.ToString();
                    cOPHistory.ProposedPropertyMemberName = memberDetailB.MemberName;
                    cOPHistory.ProposedPropertyMemberAddress = memberDetailB.PermanentAddress;
                    cOPHistory.ProposedPropertyMemberMobile = memberDetailB.Mobile;
                    cOPHistory.ProposedPropertyMemberCnic = memberDetailB.Cnic;
                    cOPHistory.ProposedPropertyBlock = _db.Blocks.Where(x => x.ID == Convert.ToInt32(b.Block)).FirstOrDefault().Description ?? "N/A";
                    cOPHistory.ProposedPropertyCategory = _db.Categories.Where(x => x.ID == Convert.ToInt32(b.Category)).FirstOrDefault().Description ?? "N/A";
                    cOPHistory.ProposedPropertySize = b.coveredArea == null ? b.ActualSize : b.coveredArea.ToString();
                    cOPHistory.ProposedPropertyConstructionStatus = b.ConstracutionStatus;
                    cOPHistory.ProposedPropertyPossessionStatus = b.PossessionStatus == true ? "Yes" : "No";
                    cOPHistory.CurrentPropertyMarketValue = dto.CurrentPropertyMarketValue;
                    cOPHistory.ProposedPropertyMarketValue = dto.ProposedPropertyMarketValue;
                    cOPHistory.ProposedPropertyPossessionStatus = b.PossessionStatus == true ? "Yes" : "No";
                    cOPHistory.Remarks = dto.Remarks;
                    cOPHistory.CreatedOn = (DateTime)dto.COPDate;

                    _db.COPHistories.Add(cOPHistory);
                    _db.SaveChanges();
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "COP Success",
                    Data = null
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetPropertiesForCOP")]
        public IActionResult GetPropertiesForCOP()
        {
            try
            {
                var result = _db.StockCreations.Where(x => !x.is_deleted &&
                                                x.MemberProfileId != null &&
                                                x.RegistrationNo != null &&
                                                x.PropertyNo != null)
                                               .Include(x => x.MemberProfile)
                                               .Distinct()
                                              .Select(x => new SelectCOPDto
                                              {
                                                  ID = x.ID,
                                                  RegistrationNo = x.RegistrationNo,
                                                  PropertyNo = x.PropertyNo,
                                                  Type = x.Type,
                                                  BlockName = x.Block,
                                                  CategoryName = x.Category,
                                                  MemberName = x.MemberProfile.MemberName,
                                                  Cnic = x.MemberProfile.Cnic
                                              })
                                           .ToList();

                var Blocks = _db.Blocks.ToList();
                var Categories = _db.Categories.ToList();
                var Types = _db.PropertyTypes.ToList();

                foreach (var item in result)
                {
                    item.BlockName = Blocks.Where(p => p.ID == (Convert.ToInt32(item.BlockName))).Select(x => x.Description).FirstOrDefault();
                    item.CategoryName = Categories.Where(p => p.ID == (Convert.ToInt32(item.CategoryName))).Select(x => x.Description).FirstOrDefault();
                    item.Type = Types.Where(p => p.ID == (Convert.ToInt32(item.Type))).Select(x => x.Description).FirstOrDefault();
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
        [Route("GetFilterProperty")]
        public IActionResult GetFilterProperty(int id)
        {
            try
            {

                var result = _db.StockCreations.Where(x => !x.is_deleted && x.ID == id &&
                                                      x.MemberProfileId != null &&
                                                      x.RegistrationNo != null &&
                                                      x.PropertyNo != null)
                                           .Include(x => x.MemberProfile)
                                           .Include(x => x.Dealer)
                                           .Select(x => new
                                           {
                                               x.ID,
                                               x.RegistrationNo,
                                               x.PropertyNo,
                                               MemberName = x.MemberProfile.MemberName ?? "N/A",
                                               MemberCode = x.MemberProfile.Id,
                                               EstateName = x.Dealer.EstateName ?? "N/A",
                                               x.Status,
                                               x.ActualSize,
                                               x.PossessionStatus,
                                               x.ConstracutionStatus,
                                               BlockName = _db.Blocks.Where(p => p.ID == (Convert.ToInt32(x.Block))).Select(x => x.Description).FirstOrDefault(),
                                               BookingDate = _db.Booking.Where(p => p.StockCreationId == (Convert.ToInt32(x.ID))).Select(x => x.CreatedOn).FirstOrDefault(),
                                               CategoryName = _db.Categories.Where(p => p.ID == (Convert.ToInt32(x.Category))).Select(x => x.Description).FirstOrDefault()
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

        [HttpDelete]
        [Route("/api/COP/Cancel")]
        public IActionResult Cancel(int id)
        {
            try
            {

                var regIds = _db.COPHistories
                                     .Where(x => x.Id == id)
                                     .Select(x => new
                                     {
                                         x.CurrentPropertyRegistrationNo,
                                         x.ProposedPropertyRegistrationNo,
                                         x.CurrentPropertyPropertyNo,
                                         x.ProposedPropertyPropertyNo
                                     })
                                     .FirstOrDefault();

                var a = _db.StockCreations.Where(x => x.RegistrationNo == regIds.CurrentPropertyRegistrationNo && x.PropertyNo == regIds.CurrentPropertyPropertyNo).FirstOrDefault();
                var b = _db.StockCreations.Where(x => x.RegistrationNo == regIds.ProposedPropertyRegistrationNo && x.PropertyNo == regIds.ProposedPropertyPropertyNo).FirstOrDefault();

                if(a == null && b == null) {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "Property again passed the process of COP",
                        Data = null
                    });
                }


                    string aPropertyNo = b.PropertyNo;
                    string aPrefixProperty = b.PrefixProperty;
                    int? anumForProperty = b.numForProperty;
                    string apostfixForProperty = b.postfixForProperty;
                    string aBlock = b.Block;
                    string aCategory = b.Category;
                    string aRealStateType = b.RealStateType;
                    string aActualSize = b.ActualSize;
                    string aActualSizeUnit = b.ActualSizeUnit;
                    string aProject = b.Project;
                    string aPhase = b.Phase;
                    string aType = b.Type;
                    string aNature = b.Nature;
                    string aFinishing = b.Finishing;
                    string aFloor = b.Floor;
                    bool? aPossessionStatus = b.PossessionStatus;
                    string aConstracutionStatus = b.ConstracutionStatus;
                    decimal? acoveredArea = b.coveredArea;
                    string? aLdaPlotNo = b.LDAPlotNo;

                    string bPropertyNo = a.PropertyNo;
                    string bPrefixProperty = a.PrefixProperty;
                    int? bnumForProperty = a.numForProperty;
                    string bpostfixForProperty = a.postfixForProperty;
                    string bBlock = a.Block;
                    string bCategory = a.Category;
                    string bRealStateType = a.RealStateType;
                    string bActualSize = a.ActualSize;
                    string bActualSizeUnit = a.ActualSizeUnit;
                    string bProject = a.Project;
                    string bPhase = a.Phase;
                    string bType = a.Type;
                    string bNature = a.Nature;
                    string bFinishing = a.Finishing;
                    string bFloor = a.Floor;
                    bool? bPossessionStatus = a.PossessionStatus;
                    string bConstracutionStatus = a.ConstracutionStatus;
                    decimal? bcoveredArea = a.coveredArea;
                    string? bLdaPlotNo = a.LDAPlotNo;

                    a.PropertyNo = aPropertyNo;
                    a.PrefixProperty = aPrefixProperty;
                    a.numForProperty = anumForProperty;
                    a.postfixForProperty = apostfixForProperty;
                    a.Block = aBlock;
                    a.Category = aCategory;
                    a.RealStateType = aRealStateType;
                    a.ActualSize = aActualSize;
                    a.ActualSizeUnit = aActualSizeUnit;
                    a.Project = aProject;
                    a.Phase = aPhase;
                    a.Type = aType;
                    a.Nature = aNature;
                    a.Finishing = aFinishing;
                    a.Floor = aFloor;
                    a.PossessionStatus = aPossessionStatus;
                    a.ConstracutionStatus = aConstracutionStatus;
                    a.coveredArea = acoveredArea;
                    a.LDAPlotNo = aLdaPlotNo;

                    _db.StockCreations.Update(a);

                    b.PropertyNo = bPropertyNo;
                    b.PrefixProperty = bPrefixProperty;
                    b.numForProperty = bnumForProperty;
                    b.postfixForProperty = bpostfixForProperty;
                    b.Block = bBlock;
                    b.Category = bCategory;
                    b.RealStateType = bRealStateType;
                    b.ActualSize = bActualSize;
                    b.ActualSizeUnit = bActualSizeUnit;
                    b.Project = bProject;
                    b.Phase = bPhase;
                    b.Type = bType;
                    b.Nature = bNature;
                    b.Finishing = bFinishing;
                    b.Floor = bFloor;
                    b.PossessionStatus = bPossessionStatus;
                    b.ConstracutionStatus = bConstracutionStatus;
                    b.coveredArea = bcoveredArea;
                    b.LDAPlotNo = aLdaPlotNo;

                    _db.StockCreations.Update(b);

                COPHistery cOPHistory = new COPHistery();

                var memberDetailA = _db.MemberProfile
                                     .Where(x => x.Id == a.MemberProfileId)
                                     .Select(x => new
                                     {
                                         x.MemberName,
                                         x.Cnic,
                                         x.PermanentAddress,
                                         x.Mobile
                                     })
                                     .FirstOrDefault();
                var memberDetailB = _db.MemberProfile
                                     .Where(x => x.Id == b.MemberProfileId)
                                     .Select(x => new
                                     {
                                         x.MemberName,
                                         x.Cnic,
                                         x.PermanentAddress,
                                         x.Mobile
                                     })
                                     .FirstOrDefault();

                cOPHistory.CurrentPropertyRegistrationNo = a.RegistrationNo;
                cOPHistory.CurrentPropertyPropertyNo = a.PropertyNo;
                cOPHistory.CurrentPropertyMemberCode = a.MemberProfileId.ToString();
                cOPHistory.CurrentPropertyMemberName = memberDetailA.MemberName;
                cOPHistory.CurrentPropertyMemberAddress = memberDetailA.PermanentAddress;
                cOPHistory.CurrentPropertyMemberMobile = memberDetailA.Mobile;
                cOPHistory.CurrentPropertyMemberCnic = memberDetailA.Cnic;
                cOPHistory.CurrentPropertyBlock = _db.Blocks.Where(x => x.ID == Convert.ToInt32(a.Block)).FirstOrDefault().Description ?? "N/A";
                cOPHistory.CurrentPropertyCategory = _db.Categories.Where(x => x.ID == Convert.ToInt32(a.Category)).FirstOrDefault().Description ?? "N/A";
                cOPHistory.CurrentPropertySize = a.coveredArea == null ? a.ActualSize : a.coveredArea.ToString();
                cOPHistory.CurrentPropertyConstructionStatus = a.ConstracutionStatus;
                cOPHistory.CurrentPropertyPossessionStatus = a.PossessionStatus == true ? "Yes" : "No";
                cOPHistory.ProposedPropertyRegistrationNo = b.RegistrationNo;
                cOPHistory.ProposedPropertyPropertyNo = b.PropertyNo;
                cOPHistory.ProposedPropertyMemberCode = b.MemberProfileId.ToString();
                cOPHistory.ProposedPropertyMemberName = memberDetailB.MemberName;
                cOPHistory.ProposedPropertyMemberAddress = memberDetailB.PermanentAddress;
                cOPHistory.ProposedPropertyMemberMobile = memberDetailB.Mobile;
                cOPHistory.ProposedPropertyMemberCnic = memberDetailB.Cnic;
                cOPHistory.ProposedPropertyBlock = _db.Blocks.Where(x => x.ID == Convert.ToInt32(b.Block)).FirstOrDefault().Description ?? "N/A";
                cOPHistory.ProposedPropertyCategory = _db.Categories.Where(x => x.ID == Convert.ToInt32(b.Category)).FirstOrDefault().Description ?? "N/A";
                cOPHistory.ProposedPropertySize = b.coveredArea == null ? b.ActualSize : b.coveredArea.ToString();
                cOPHistory.ProposedPropertyConstructionStatus = b.ConstracutionStatus;
                cOPHistory.ProposedPropertyPossessionStatus = b.PossessionStatus == true ? "Yes" : "No";
                cOPHistory.CurrentPropertyMarketValue = "";
                cOPHistory.ProposedPropertyMarketValue = "";
                cOPHistory.ProposedPropertyPossessionStatus = b.PossessionStatus == true ? "Yes" : "No";
                cOPHistory.Remarks = "Reversal";
                cOPHistory.CreatedOn = DateTime.Now;

                _db.COPHistories.Add(cOPHistory);
                _db.SaveChanges();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Reversal Successfully",
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
