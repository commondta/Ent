using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using B_Utility.Common.Enums;
using HRMS_Web.Models.DTOs;
using HRMS_Web.Models.DTOs.SAPDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Linq;
using System.Runtime.InteropServices;
using static System.Reflection.Metadata.BlobBuilder;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DemarcationRequestController : ControllerBase
    {
        private readonly DataBase_Context _db;
        ApprovalBLL _approvalBLL;
        CommonBLL _commonBLL;
        public DemarcationRequestController(DataBase_Context db)
        {
            _db = db;
            _approvalBLL = new ApprovalBLL(_db);
            _commonBLL = new CommonBLL(_db);
        }

        [HttpGet]
        [Route("GetAllDemarcationFilterList")]
        public IActionResult GetAllDemarcationFilterList()
        {
            try
            {
                var newDemarcationfilterList = _db.StockCreations.Where(x => x.is_deleted != true
                                                                          && x.MemberProfileId != null
                                                                          && x.PropertyNo != ""
                                                                          && x.PropertyNo != null
                                                                          && x.RegistrationNo != ""
                                                                          && x.RegistrationNo != null
                                                                          && x.Is_DemarcationRequested != true
                                                                          && x.PossessionStatus == true)
                                                                  .ToList();
                if (newDemarcationfilterList?.Count > 0)
                {
                    foreach (var block in newDemarcationfilterList)
                    {
                        block.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(block.RealStateType));
                        block.ProjectName = _commonBLL.GetProjectName(Convert.ToInt32(block.Project));
                        block.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(block.Phase));
                        block.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(block.Category));
                        block.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(block.Block));
                        block.NatureName = _commonBLL.GetNatureName(Convert.ToInt32(block.Nature));
                        block.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(block.Type));
                    }
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = newDemarcationfilterList
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetRedesignAllDemarcationFilterList")]
        public IActionResult GetRedesignAllDemarcationFilterList()
        {
            try
            {
                var newDemarcationfilterList = _db.StockCreations.Where(x => x.is_deleted != true
                                                                  && x.Is_DemarcationRequested == true
                                                                  && x.PossessionStatus == true)
                                                                 .ToList();
                if (newDemarcationfilterList?.Count > 0)
                {
                    foreach (var block in newDemarcationfilterList)
                    {
                        block.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(block.RealStateType));
                        block.ProjectName = _commonBLL.GetProjectName(Convert.ToInt32(block.Project));
                        block.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(block.Phase));
                        block.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(block.Category));
                        block.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(block.Block));
                        block.NatureName = _commonBLL.GetNatureName(Convert.ToInt32(block.Nature));
                        block.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(block.Type));
                    }
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = newDemarcationfilterList
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var newDemarcationRequests = _db.NewDemarcationRequest.Where(x => !x.IsDeleted)
                                                       .Include(x => x.NewDemarcationRequestDetail)
                                                       .Include(x => x.StockCreation)
                                                       .ToList();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = newDemarcationRequests
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult Get(int id, int branch = 1)
        {
            try
            {
                //var matchedChargeTypeIds = _db.ChargeGroupType
                //                               .Where(x => x.Code == branch)
                //                               .Select(x => x.Id)
                //                               .ToList();

                var newDemarcationRequests = _db.NewDemarcationRequest
                    .Where(x => !x.IsDeleted && x.Id == id)
                    .Include(x => x.NewDemarcationRequestDetail)
                    .Include(x => x.StockCreation)
                    .Select(x => new
                    {
                        Id = x.Id,
                        ChallanNo = x.ChallanNo,
                        RegistrationNo = x.StockCreation.RegistrationNo,
                        PropertyNo = x.StockCreation.PropertyNo,
                        MemberName = x.StockCreation.MemberProfile.MemberName,
                        Address = x.StockCreation.MemberProfile.PermanentAddress ?? "N/A",
                        MemberCnic = x.StockCreation.MemberProfile.Cnic ?? "N/A",
                        Area = x.StockCreation.ActualSize,
                        UnitArea = x.StockCreation.ActualSizeUnit,
                        Block = _db.Blocks.Where(y => y.ID == Convert.ToInt32(x.StockCreation.Block)).Select(b => b.Description).FirstOrDefault() ?? "N/A",
                        Phase = _db.Phases.Where(y => y.ID == Convert.ToInt32(x.StockCreation.Phase)).Select(p => p.Description).FirstOrDefault() ?? "N/A",
                        Category = _db.Categories.Where(y => y.ID == Convert.ToInt32(x.StockCreation.Category)).Select(c => c.Description).FirstOrDefault() ?? "N/A",
                        Type = _db.PropertyTypes.Where(y => y.ID == Convert.ToInt32(x.StockCreation.Type)).Select(t => t.Description).FirstOrDefault() ?? "N/A",
                        Nature = _db.Natures.Where(y => y.ID == Convert.ToInt32(x.StockCreation.Nature)).Select(n => n.Description).FirstOrDefault() ?? "N/A",
                        DocDate = x.CreatedOn,
                        MemberCode = x.StockCreation.MemberProfile.Id,

                        // Step 3: Filter charges based on matchedChargeTypeIds
                        Charges = x.NewDemarcationRequestDetail
                                   //.Where(m => matchedChargeTypeIds.Contains((int)m.ChargeTypeId))
                                   .Select(m => new Charges
                                   {
                                       ChargeName = m.ChargeName,
                                       Amount = m.Rate
                                   })
                                   .ToList()
                    })
                    .FirstOrDefault();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = newDemarcationRequests
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }


        [HttpPost]
        [Route("AddNewDemarcationRequest")]
        public async Task<IActionResult> AddNewDemarcationRequestAsync(NewDemarcationRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                    });
                }

                //if (!model.NewDemarcationRequestDetail.Any())
                //{
                //    return Ok(new ApiResponse<object>
                //    {
                //        Code = ResponseCode.Error,
                //        Message = "charges are mandatory to complete the request",
                //        Data = null
                //    });
                //}

                var response = new SapIntegrationController(_db).PostingDemarcationARInvoice(model);

                if (response.Code != ResponseCode.Success)
                {
                    return Ok(response);
                }

                model.ChallanNo = await _commonBLL.GetNextChallanNumberAsync("BC");

                NewDemarcationRequest request = _db.NewDemarcationRequest.Where(x => x.StockCreationId == model.StockCreationId).FirstOrDefault();

                StockCreation stockCreation = (StockCreation)_db.StockCreations.Where(x => x.ID == model.StockCreationId)
                                                                               .FirstOrDefault();

                if (model.RedesignRequest != "Yes" && request is not null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Error,
                        Message = "request already exist. you can add only redesign request",
                        Data = null
                    });
                }

                if (model.RedesignRequest == "Yes" && request is not null)
                {
                    stockCreation.Is_DemarcationRequested = true;
                    stockCreation.Is_DemarcationApproved = true;
                    stockCreation.Is_ClearnceRequested = null;
                    stockCreation.Is_ClearnceApproved = null;
                    request.LastModified = DateTime.Now;
                    request.CreatedOn = model.CreatedOn;
                    request.IsCancelled = true;

                    _db.SaveChanges();

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Redesign Request Submited Successfully.",
                        Data = null
                    });
                }

                if (model.RedesignRequest == "Yes")
                {
                    model.IsCancelled = true;
                    stockCreation.Is_ClearnceRequested = null;
                    stockCreation.Is_ClearnceApproved = null;
                }

                model.Status = "Active";
                model.LastModified = DateTime.Now;
                model.CreatedOn = model.CreatedOn;
                model.CreatedBy = model.CreatedBy;
                model.IsActive = true;
                model.IsDeleted = false;

                if (model.NewDemarcationRequestDetail?.Count > 0)
                {
                    foreach (var item in model.NewDemarcationRequestDetail)
                    {
                        item.ModifiedBy = item.ModifiedBy;
                        item.CreatedBy = item.CreatedBy;
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }


                _db.NewDemarcationRequest.Add(model);

                if (stockCreation != null)
                {
                    stockCreation.Is_DemarcationRequested = true;
                    stockCreation.Is_DemarcationApproved = true;

                }
                _db.SaveChanges();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = model.Id
                });
            }

            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

    }
}
