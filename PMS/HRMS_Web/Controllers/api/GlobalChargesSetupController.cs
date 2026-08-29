using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using HRMS_Web.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GlobalChargesSetupController : ControllerBase
    {
        private DataBase_Context _db;
        CommonBLL _commonBLL;
        public GlobalChargesSetupController(DataBase_Context context)
        {
            _db = context;
            _commonBLL = new CommonBLL(_db);
        }

        //[HttpPost]
        //[Route("GetGlobalChargeDetail")]
        //public IActionResult GetGlobalChargeDetail( GlobalChargeSetupDetailFilterDTO dto )
        //{
        //    try
        //    {
        //        if ( !ModelState.IsValid )
        //        {
        //            return Ok(new ApiResponse<object>
        //            {
        //                Code = ResponseCode.BadRequest,
        //                Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
        //            });
        //        }

        //        int globalChargeGroupId = _db.GlobalChargeGroup.Where(x => x.ChargeGroupName == "Demarcation").Select(x => x.Id).FirstOrDefault();

        //        var globalChargeSetupsDetail = _db.GlobalChargeSetup.Where(x => !x.IsDeleted
        //                                                            && x.GlobalChargeGroupId == globalChargeGroupId
        //                                                            && x.RealStateTypeId == dto.RealStateTypeId
        //                                                            && x.ProjectId == dto.ProjectId
        //                                                            && x.PhaseId == dto.PhaseId
        //                                                            && x.BlockId == dto.BlockId
        //                                                            && x.CategoryId == dto.CategoryId
        //                                                            && x.PropertyTypeId == dto.PropertyTypeId
        //                                                            && x.NatureId == dto.NatureId
        //                                                            )
        //                                                       .Include(x => x.GlobalChargeDetail.Where(x => !x.IsDeleted))
        //                                                       .FirstOrDefault();

        //        return Ok(new ApiResponse<object>
        //        {
        //            Code = ResponseCode.Success,
        //            Message = "Success",
        //            Data = globalChargeSetupsDetail
        //        });
        //    }
        //    catch ( System.Exception ex )
        //    {
        //        return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
        //    }
        //}


        [HttpPost]
        [Route("GetGlobalChargeDetail")]
        public IActionResult GetGlobalChargeDetail(GlobalChargeSetupDetailFilterDTO dto, int formId)
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

                var globalChargeGroupIds = _db.FormsChargeGroup.Where(x => x.FormId == formId && !x.IsDeleted).Select(x => x.ChargeGroupId).ToList();

                var globalChargeSetupDetails = new List<GlobalChargeDetail>();
                var globalChargeSetupsDetail = new List<GlobalChargeSetup>();


                if (globalChargeGroupIds.Count() > 0 || globalChargeGroupIds is not null)
                {
                    foreach (var item in globalChargeGroupIds)
                    {
                        globalChargeSetupsDetail = _db.GlobalChargeSetup.Where(x => !x.IsDeleted
                                                               && x.GlobalChargeGroupId == item
                                                               && x.RealStateTypeId == dto.RealStateTypeId
                                                               && x.ProjectId == dto.ProjectId
                                                               && x.PhaseId == dto.PhaseId
                                                               && x.BlockId == dto.BlockId
                                                               && x.CategoryId == dto.CategoryId
                                                               && x.PropertyTypeId == dto.PropertyTypeId
                                                               && x.NatureId == dto.NatureId
                                                               )
                                                           .Include(x => x.GlobalChargeDetail.Where(x => !x.IsDeleted))
                                                           .ToList();

                        if (globalChargeSetupsDetail != null)
                        {
                            foreach (var item2 in globalChargeSetupsDetail)
                            {
                                foreach (var detail in item2.GlobalChargeDetail)
                                {
                                    detail.GlobalChargeSetup = null;
                                    globalChargeSetupDetails.Add(detail);
                                }

                            }
                        }

                    }
                }
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = new { globalChargeDetail = globalChargeSetupDetails.Distinct() }
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
                var globalChargeSetups = _db.GlobalChargeSetup.Where(x => !x.IsDeleted)
                                                       .Include(x => x.GlobalChargeDetail.Where(x => !x.IsDeleted))
                                                       .ToList();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = globalChargeSetups
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
                var globalChargeSetups = _db.GlobalChargeSetup.Where(x => !x.IsDeleted && x.Id == id)
                                                       .Include(x => x.GlobalChargeDetail.Where(x => !x.IsDeleted))
                                                       .FirstOrDefault();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = globalChargeSetups
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        [Route("AddGlobalChargeSetup")]
        public IActionResult AddGlobalChargeSetup(GlobalChargeSetup model)
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
                model.LastModified = DateTime.Now;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = model.CreatedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;
                model.ModifiedBy = model.ModifiedBy;
                model.IsActive = true;
                model.IsDeleted = false;

                if(model.GlobalChargeDetail.Any(x=>x.SapAccount == ""))
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "Sap Account missing of them charge type.",
                        Data = null
                    });
                }

                if (model.GlobalChargeDetail?.Count > 0)
                {
                    foreach (var item in model.GlobalChargeDetail)
                    {
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.ModifiedBy = model.ModifiedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.CreatedBy = model.CreatedBy;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                _db.GlobalChargeSetup.Add(model);
                _db.SaveChanges();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = model
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        [Route("UpdateGlobalChargeSetup")]
        public IActionResult UpdateGlobalChargeSetup(GlobalChargeSetup model)
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

                if (model.GlobalChargeDetail.Any(x => x.SapAccount == ""))
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "Sap Account missing of them charge type.",
                        Data = null
                    });
                }

                var data = _db.GlobalChargeSetup.Find(model.Id);

                if (data != null)
                {
                    data.Description = model.Description;
                    data.ChargeStatus = model.ChargeStatus;
                    data.ConstructionStatus = model.ConstructionStatus;
                    data.GeneratorUnitType = model.GeneratorUnitType;
                    data.PossessionStatus = model.PossessionStatus;
                    data.EffectiveDate = model.EffectiveDate;
                    data.ToDate = model.ToDate;
                    data.RealStateTypeId = model.RealStateTypeId;
                    data.GlobalChargeGroupId = model.GlobalChargeGroupId;
                    data.ProjectId = model.ProjectId;
                    data.PhaseId = model.PhaseId;
                    data.BlockId = model.BlockId;
                    data.CategoryId = model.CategoryId;
                    data.PropertyTypeId = model.PropertyTypeId;
                    data.NatureId = model.NatureId;
                    data.GracePeriod = model.GracePeriod;
                    data.NDCProcessing = model.NDCProcessing;
                    data.NDCRequestType = model.NDCRequestType;
                    data.NDCTransferType = model.NDCTransferType;
                    data.RegistryVerification = model.RegistryVerification;
                    data.FBR236C = model.FBR236C;
                    data.TaxStatus = model.TaxStatus;
                    data.FileRequestType = model.FileRequestType;
                    data.Sector = model.Sector;
                    data.Redesign = model.Redesign;
                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;
                    data.LastModified = DateTime.Now;

                    _db.Entry(data).State = EntityState.Modified;
                    _db.SaveChanges();


                    var globalChargeDetails = _db.GlobalChargeDetail.Where(x => x.GlobalChargeSetupId == model.Id).ToList();

                    if (globalChargeDetails.Count() > 0)
                    {
                        var idsToRemove = globalChargeDetails.Where(x => !model.GlobalChargeDetail.Any(y => y.Id == x.Id)).Select(x => x.Id).ToList();

                        foreach (var id in idsToRemove)
                        {
                            var itemToRemove = globalChargeDetails.First(x => x.Id == id);
                            _db.GlobalChargeDetail.Remove(itemToRemove);

                            var removedfromappliedCharges = _db.PropertyFixedChargesSetup.Where(x => x.MatchId == id).ToList();

                            if (removedfromappliedCharges.Count() > 0)
                            {
                                _db.PropertyFixedChargesSetup.RemoveRange(removedfromappliedCharges);
                            }
                        }

                        _db.SaveChanges();
                    }

                    if (model.GlobalChargeDetail?.Count > 0)
                    {
                        int Id = _db.FormsChargeGroup.SingleOrDefault(x => x.FormId == 5).ChargeGroupId;
                        int groupId = (int)_db.GlobalChargeSetup.Where(x => x.Id == data.Id).FirstOrDefault().GlobalChargeGroupId;

                        foreach (var item in model.GlobalChargeDetail)
                        {
                            var globalChargeDetail = _db.GlobalChargeDetail.Where(x => x.GlobalChargeSetupId == model.Id && x.Id == item.Id).FirstOrDefault();

                            item.Description = _db.ChargeGroupType.Where(x=>x.Id == Convert.ToInt16(item.ChargeType)).FirstOrDefault()?.ChargeTypeName;

                            if (globalChargeDetail != null)
                            {
                                globalChargeDetail.GlobalChargeSetupId = data.Id;
                                globalChargeDetail.ChargeType = item.ChargeType;
                                globalChargeDetail.Description = item.Description;
                                globalChargeDetail.Rate = item.Rate;
                                globalChargeDetail.Percentage = item.Percentage;
                                globalChargeDetail.EffectiveFrom = item.EffectiveFrom;
                                globalChargeDetail.EffectiveTo = item.EffectiveTo;
                                globalChargeDetail.Status = item.Status;
                                globalChargeDetail.WHStatus = item.WHStatus;
                                globalChargeDetail.MultiplyBySize = item.MultiplyBySize;

                                globalChargeDetail.ModifiedBy = item.ModifiedBy;
                                globalChargeDetail.LastModifiedUserName = item.LastModifiedUserName;

                                globalChargeDetail.Yearly = item.Yearly;
                                //globalChargeDetail.IsPropertyTax = item.IsPropertyTax;

                                globalChargeDetail.LastModified = DateTime.Now;

                                _db.SaveChanges();

                                if (Id != 0 && Id == groupId)
                                {
                                    var chargeonprop = _db.PropertyFixedChargesSetup.Where(x => x.MatchId == item.Id).ToList();

                                    foreach (var charge in chargeonprop)
                                    {
                                        charge.MatchId = item.Id;
                                        charge.GlobalChargeSetupId = data.Id;
                                        charge.Unit = charge.Unit;
                                        charge.ChargeSetupRate = Convert.ToDecimal(item.Rate);
                                        charge.Rate = Convert.ToDecimal(item.Rate);//charge.ChargeSetupRate == charge.Rate ? Convert.ToDecimal(item.Rate) : charge.Rate;                                                  
                                        charge.ChargeDes = model.Description;
                                        charge.ChargeType = item.Description;
                                        charge.IsEnabled = true;
                                        charge.IsActive = true;
                                        charge.LastModified = DateTime.Now;
                                        charge.ModifiedBy = model.ModifiedBy;
                                        charge.LastModifiedUserName = item.LastModifiedUserName;
                                    }

                                    _db.SaveChanges();
                                }
                            }
                            else
                            {
                                item.GlobalChargeSetupId = data.Id;
                                item.CreatedOn = DateTime.Now;
                                item.LastModified = DateTime.Now;
                                item.ModifiedBy = model.ModifiedBy;
                                item.LastModifiedUserName = model.LastModifiedUserName;
                                item.IsActive = true;
                                item.IsDeleted = false;

                                _db.GlobalChargeDetail.Add(item);
                                _db.SaveChanges();

                                if (Id != 0 && Id == groupId)
                                {

                                    List<PropertyFixedChargesSetup> chargesDto = new List<PropertyFixedChargesSetup>();

                                    var chargeonprop = _db.PropertyFixedChargesSetup.ToList().DistinctBy(x => x.StockCreationId);

                                    foreach (var fixedcharge in chargeonprop)
                                    {

                                        var property = _db.StockCreations.Find(fixedcharge.StockCreationId);

                                        GlobalChargeSetupDetailFixedChargFilterDTO dto = new GlobalChargeSetupDetailFixedChargFilterDTO()
                                        {
                                            FormId = 5,
                                            RealStateTypeId = Convert.ToInt32(property.RealStateType),
                                            ProjectId = Convert.ToInt32(property.Project),
                                            PhaseId = Convert.ToInt32(property.Phase),
                                            BlockId = Convert.ToInt32(property.Block),
                                            CategoryId = Convert.ToInt32(property.Category),
                                            PropertyTypeId = Convert.ToInt32(property.Type),
                                            NatureId = Convert.ToInt32(property.Nature),
                                            PossessionStatus = property.PossessionStatus,
                                            ConstructionStatus = property.ConstracutionStatus,
                                            //GracePeriod = item.GrancePeriodForBillGenration < DateTime.Now.Date ? false : true,
                                        };

                                        if (dto.RealStateTypeId == model.RealStateTypeId &&
                                                         dto.ProjectId == model.ProjectId &&
                                                         dto.PhaseId == model.PhaseId &&
                                                         //dto.BlockId == model.BlockId &&
                                                         dto.CategoryId == model.CategoryId &&
                                                         dto.PropertyTypeId == model.PropertyTypeId &&
                                                         dto.NatureId == model.NatureId &&
                                                         dto.PossessionStatus == model.PossessionStatus &&
                                                         dto.ConstructionStatus == model.ConstructionStatus)
                                        {
                                            PropertyFixedChargesSetup propchargedto = new PropertyFixedChargesSetup();

                                            propchargedto.MatchId = item.Id;
                                            propchargedto.RegistrationNo = fixedcharge.RegistrationNo;
                                            propchargedto.GlobalChargeSetupId = data.Id;
                                            propchargedto.PropertyNo = fixedcharge.PropertyNo;
                                            propchargedto.StockCreationId = fixedcharge.StockCreationId;
                                            propchargedto.ChargeSetupRate = Convert.ToDecimal(item.Rate);
                                            propchargedto.Rate = Convert.ToDecimal(item.Rate);
                                            propchargedto.Discount = 0;
                                            propchargedto.Unit = 1;
                                            propchargedto.ChargeDes = model.Description;
                                            propchargedto.ChargeType = item.Description;
                                            propchargedto.IsEnabled = true;
                                            propchargedto.IsActive = true;
                                            propchargedto.LastModified = DateTime.Now;
                                            propchargedto.ModifiedBy = model.ModifiedBy;
                                            propchargedto.LastModifiedUserName = item.LastModifiedUserName;

                                            chargesDto.Add(propchargedto);

                                        }
                                    }

                                    _db.PropertyFixedChargesSetup.AddRange(chargesDto);
                                    _db.SaveChanges();
                                }
                            }
                        }
                    }
                }
                else
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Not Found",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = data
                });

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        [Route("GetGlobalChargeDetailForDemarcation")]
        public IActionResult GetGlobalChargeDetailForDemarcation(GlobalChargeSetupDetailFilterDTO dto, int formId)
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

                var globalChargeGroupIds = _db.FormsChargeGroup.Where(x => x.FormId == formId && !x.IsDeleted).Select(x => x.ChargeGroupId).ToList();

                var globalChargeSetupDetails = new List<GlobalChargeDetail>();
                var globalChargeSetupsDetail = new List<GlobalChargeSetup>();


                if (globalChargeGroupIds.Count() > 0 || globalChargeGroupIds is not null)
                {
                    foreach (var item in globalChargeGroupIds)
                    {
                        globalChargeSetupsDetail = _db.GlobalChargeSetup.Where(x => !x.IsDeleted
                                                               && x.GlobalChargeGroupId == item
                                                               //&& x.RealStateTypeId == dto.RealStateTypeId
                                                               //&& x.ProjectId == dto.ProjectId
                                                               //&& x.PhaseId == dto.PhaseId
                                                               //&& (x.BlockId == dto.BlockId || x.BlockId == null)
                                                               //&& x.CategoryId == dto.CategoryId
                                                               && x.PropertyTypeId == dto.PropertyTypeId
                                                               //&& x.NatureId == dto.NatureId
                                                               && x.Redesign == dto.Redesign
                                                               )
                                                           .Include(x => x.GlobalChargeDetail.Where(x => !x.IsDeleted))
                                                           .ToList();

                        if (globalChargeSetupsDetail != null)
                        {
                            foreach (var item2 in globalChargeSetupsDetail)
                            {
                                foreach (var detail in item2.GlobalChargeDetail)
                                {
                                    detail.GlobalChargeSetup = null;
                                    detail.Rate = (bool)detail.MultiplyBySize ? (dto.Size * detail.Rate) : detail.Rate;
                                    globalChargeSetupDetails.Add(detail);
                                }

                            }
                        }

                    }
                }
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = new { globalChargeDetail = globalChargeSetupDetails.Distinct() }
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpDelete]
        [Route("DeleteGlobalChargeSetup")]
        public IActionResult DeleteGlobalChargeSetup(int id)
        {
            try
            {
                var model = _db.GlobalChargeSetup.Find(id);

                if (model != null)
                {
                    model.LastModified = DateTime.Now;
                    model.IsActive = false;
                    model.IsDeleted = true;

                    _db.SaveChanges();

                    var globalChargeDetail = _db.GlobalChargeDetail.Where(x => x.GlobalChargeSetupId == model.Id).ToList();

                    foreach (var item in globalChargeDetail)
                    {
                        item.LastModified = DateTime.Now;
                        item.IsActive = false;
                        item.IsDeleted = true;
                        _db.SaveChanges();
                    }
                }
                else
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Not Found",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = model
                });

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        [Route("GetGlobalChargeDetailForWavieOff")]
        public IActionResult GetGlobalChargeDetailForWavieOff(GlobalChargeSetupWavieOffDTO dto, int formId)
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

                var globalChargeGroupIds = _db.FormsChargeGroup.Where(x => x.FormId == formId && !x.IsDeleted).Select(x => x.ChargeGroupId).ToList();

                var globalChargeSetupDetails = new List<GlobalChargeDetail>();
                var globalChargeSetupsDetail = new List<GlobalChargeSetup>();


                if (globalChargeGroupIds.Count() > 0 || globalChargeGroupIds is not null)
                {
                    foreach (var item in globalChargeGroupIds)
                    {
                        globalChargeSetupsDetail = _db.GlobalChargeSetup.Where(x => !x.IsDeleted
                                                               && x.GlobalChargeGroupId == item
                                                               && x.CategoryId == dto.Category
                                                               && x.ConstructionStatus == dto.ConstracutionStatus
                                                               && x.BlockId == dto.Block
                                                               && x.Sector == dto.Sector
                                                               && x.EffectiveDate <= DateTime.Now && x.ToDate >= DateTime.Now
                                                               )
                                                           .Include(x => x.GlobalChargeDetail.Where(x => !x.IsDeleted))
                                                           .ToList();

                        if (globalChargeSetupsDetail != null)
                        {
                            foreach (var item2 in globalChargeSetupsDetail)
                            {
                                foreach (var detail in item2.GlobalChargeDetail)
                                {
                                    detail.SapAccount = _commonBLL.GetSapAccountByChargeTypeId(Convert.ToInt32(detail.ChargeType));
                                    detail.GlobalChargeSetup = null;
                                    globalChargeSetupDetails.Add(detail);
                                }

                            }
                        }

                    }
                }
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = new { globalChargeDetail = globalChargeSetupDetails }
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        [Route("GetGlobalChargeDetailForNDCMemberRequest")]
        public IActionResult GetGlobalChargeDetailForNDCMemberRequest(GlobalChargeSetupNDCMemberFilterDTO dto, int formId)
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

                var globalChargeGroupIds = _db.FormsChargeGroup.Where(x => x.FormId == formId && !x.IsDeleted).Select(x => x.ChargeGroupId).ToList();

                var globalChargeSetupDetails = new List<GlobalChargeDetail>();
                var globalChargeSetupsDetail = new List<GlobalChargeSetup>();


                if (globalChargeGroupIds.Count() > 0 || globalChargeGroupIds is not null)
                {
                    foreach (var item in globalChargeGroupIds)
                    {
                        globalChargeSetupsDetail = _db.GlobalChargeSetup.Where(x => !x.IsDeleted
                                                               && x.GlobalChargeGroupId == item
                                                               && x.CategoryId == dto.Category
                                                               //&& x.ConstructionStatus == dto.ConstracutionStatus
                                                               && x.NDCRequestType == dto.RequestType
                                                               && x.NDCTransferType == dto.TransferType
                                                               )
                                                           .Include(x => x.GlobalChargeDetail.Where(x => !x.IsDeleted))
                                                           .ToList();

                        if (globalChargeSetupsDetail != null)
                        {
                            foreach (var item2 in globalChargeSetupsDetail)
                            {
                                foreach (var detail in item2.GlobalChargeDetail)
                                {
                                    detail.SapAccount = _commonBLL.GetSapAccountByChargeTypeId(Convert.ToInt32(detail.ChargeType));
                                    detail.GlobalChargeSetup = null;
                                    globalChargeSetupDetails.Add(detail);
                                }

                            }
                        }

                    }
                }
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = new { globalChargeDetail = globalChargeSetupDetails.Distinct() }
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        [Route("GetGlobalChargeDetailForFileVerificationRequest")]
        public IActionResult GetGlobalChargeDetailForFileVerificationRequest(GlobalChargeSetupFileVerificationFilterDTO dto, int formId)
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

                var globalChargeGroupIds = _db.FormsChargeGroup.Where(x => x.FormId == formId && !x.IsDeleted).Select(x => x.ChargeGroupId).ToList();

                var globalChargeSetupDetails = new List<GlobalChargeDetail>();
                var globalChargeSetupsDetail = new List<GlobalChargeSetup>();


                if (globalChargeGroupIds.Count() > 0 || globalChargeGroupIds is not null)
                {
                    foreach (var item in globalChargeGroupIds)
                    {
                        globalChargeSetupsDetail = _db.GlobalChargeSetup.Where(x => !x.IsDeleted
                                                               && x.GlobalChargeGroupId == item
                                                               && x.ConstructionStatus == dto.ConstracutionStatus
                                                               )
                                                           .Include(x => x.GlobalChargeDetail.Where(x => !x.IsDeleted))
                                                           .ToList();

                        if (globalChargeSetupsDetail != null)
                        {
                            foreach (var item2 in globalChargeSetupsDetail)
                            {
                                foreach (var detail in item2.GlobalChargeDetail)
                                {
                                    detail.SapAccount = _commonBLL.GetSapAccountByChargeTypeId(Convert.ToInt32(detail.ChargeType));
                                    detail.GlobalChargeSetup = null;
                                    globalChargeSetupDetails.Add(detail);
                                }

                            }
                        }

                    }
                }
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = new { globalChargeDetail = globalChargeSetupDetails.Distinct() }
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        [Route("GetGlobalChargeDetailForFileRequest")]
        public IActionResult GetGlobalChargeDetailForFileRequest(GlobalChargeSetupFileRequestFilterDTO dto, int formId)
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

                var globalChargeGroupIds = _db.FormsChargeGroup.Where(x => x.FormId == formId && !x.IsDeleted).Select(x => x.ChargeGroupId).ToList();

                var globalChargeSetupDetails = new List<GlobalChargeDetail>();
                var globalChargeSetupsDetail = new List<GlobalChargeSetup>();


                if (globalChargeGroupIds.Count() > 0 || globalChargeGroupIds is not null)
                {
                    foreach (var item in globalChargeGroupIds)
                    {
                        globalChargeSetupsDetail = _db.GlobalChargeSetup.Where(x => !x.IsDeleted
                                                               && x.GlobalChargeGroupId == item
                                                               && x.FileRequestType == dto.RequestType
                                                               )
                                                           .Include(x => x.GlobalChargeDetail.Where(x => !x.IsDeleted))
                                                           .ToList();

                        if (globalChargeSetupsDetail != null)
                        {
                            foreach (var item2 in globalChargeSetupsDetail)
                            {
                                foreach (var detail in item2.GlobalChargeDetail)
                                {
                                    detail.SapAccount = _commonBLL.GetSapAccountByChargeTypeId(Convert.ToInt32(detail.ChargeType));
                                    detail.GlobalChargeSetup = null;
                                    globalChargeSetupDetails.Add(detail);
                                }

                            }
                        }

                    }
                }
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = new { globalChargeDetail = globalChargeSetupDetails.Distinct() }
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        [Route("GetGlobalChargeDetailForNDCDealerRequest")]
        public IActionResult GetGlobalChargeDetailForNDCDealerRequest(GlobalChargeSetupNDCDealerFilterDTO dto, int formId)
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

                var globalChargeGroupIds = _db.FormsChargeGroup.Where(x => x.FormId == formId && !x.IsDeleted).Select(x => x.ChargeGroupId).ToList();

                var globalChargeSetupDetails = new List<GlobalChargeDetail>();
                var globalChargeSetupsDetail = new List<GlobalChargeSetup>();


                if (globalChargeGroupIds.Count() > 0 || globalChargeGroupIds is not null)
                {
                    foreach (var item in globalChargeGroupIds)
                    {
                        globalChargeSetupsDetail = _db.GlobalChargeSetup.Where(x => !x.IsDeleted
                                                               && x.GlobalChargeGroupId == item
                                                               && x.CategoryId == dto.Category
                                                               && x.ConstructionStatus == dto.ConstracutionStatus
                                                               && x.NDCRequestType == dto.RequestType
                                                               && x.NDCTransferType == dto.TransferType
                                                               && x.NDCProcessing == dto.Processing
                                                               )
                                                           .Include(x => x.GlobalChargeDetail.Where(x => !x.IsDeleted))
                                                           .ToList();

                        if (globalChargeSetupsDetail != null)
                        {
                            foreach (var item2 in globalChargeSetupsDetail)
                            {
                                foreach (var detail in item2.GlobalChargeDetail)
                                {
                                    detail.SapAccount = _commonBLL.GetSapAccountByChargeTypeId(Convert.ToInt32(detail.ChargeType));
                                    detail.GlobalChargeSetup = null;
                                    globalChargeSetupDetails.Add(detail);
                                }

                            }
                        }

                    }
                }
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = new { globalChargeDetail = globalChargeSetupDetails.Distinct() }
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        [Route("GetGlobalChargeDetailForSellerGovtTaxes")]
        public IActionResult GetGlobalChargeDetailForSellerGovtTaxes(GlobalChargesSellerGovtTaxFilterDTO dto, int formId)
        {
            try
            {

                var globalChargeGroupIds = _db.FormsChargeGroup.Where(x => x.FormId == formId && !x.IsDeleted).Select(x => x.ChargeGroupId).ToList();

                var globalChargeSetupDetails = new List<GlobalChargeDetail>();
                var globalChargeSetupsDetail = new List<GlobalChargeSetup>();


                if (globalChargeGroupIds.Count() > 0 || globalChargeGroupIds is not null)
                {
                    foreach (var item in globalChargeGroupIds)
                    {
                        globalChargeSetupsDetail = _db.GlobalChargeSetup.Where(x => !x.IsDeleted
                                                               && x.GlobalChargeGroupId == item
                                                               && x.CategoryId == dto.CategoryId
                                                               && x.PropertyTypeId == dto.PropertyTypeId
                                                               && x.TaxStatus == dto.Filer
                                                               && x.ConstructionStatus == dto.ConstracutionStatus
                                                               && x.FBR236C == dto.FBRTAX236C
                                                               )
                                                           .Include(x => x.GlobalChargeDetail.Where(x => !x.IsDeleted))
                                                           .ToList();

                        if (globalChargeSetupsDetail != null)
                        {
                            decimal area = GetArea(dto.StockId);

                            foreach (var item2 in globalChargeSetupsDetail)
                            {
                                foreach (var detail in item2.GlobalChargeDetail)
                                {
                                    if(detail.Yearly == true)
                                    {
                                        if(dto.PropertyTaxYears == 1)
                                        {
                                            detail.Rate = (detail.Rate * dto.PropertyTaxYears) + ((detail.Rate * dto.PropertyTaxYears) * 25 / 100);
                                        }
                                        else
                                        {
                                            detail.Rate = (detail.Rate * dto.PropertyTaxYears) + ((detail.Rate * dto.PropertyTaxYears) * 30 / 100);
                                        }
                                    }

                                    if (detail.Percentage <= 0 && detail.MultiplyBySize == true)
                                    {
                                        detail.Rate = (detail.Rate * area);
                                    }
                                    else if (detail.Percentage > 0 && detail.MultiplyBySize == true)
                                    {
                                        detail.Rate = (detail.Rate * area) * (detail.Percentage / 100);
                                    }
                                    else
                                    {
                                        detail.Rate = detail.Rate;
                                    }
                                    detail.SapAccount = _commonBLL.GetSapAccountByChargeTypeId(Convert.ToInt32(detail.ChargeType));
                                    detail.GlobalChargeSetup = null;
                                    globalChargeSetupDetails.Add(detail);
                                }

                            }
                        }

                    }
                }
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = new { globalChargeDetail = globalChargeSetupDetails.Distinct() }
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        // Get Area
        private decimal GetArea(int stockId)
        {
            var stockCreation = _db.StockCreations.FirstOrDefault(x => x.ID == stockId);
            decimal area = stockCreation.coveredArea != null ? (decimal)stockCreation.coveredArea : Convert.ToDecimal(stockCreation.ActualSize);
            return area;
        }


        [HttpPost]
        [Route("GetGlobalChargeDetailForBuyerGovtTaxes")]
        public IActionResult GetGlobalChargeDetailForBuyerGovtTaxes(GlobalChargesBuyerGovtTaxFilterDTO dto, int formId)
        {
            try
            {

                var globalChargeGroupIds = _db.FormsChargeGroup.Where(x => x.FormId == formId && !x.IsDeleted).Select(x => x.ChargeGroupId).ToList();

                var globalChargeSetupDetails = new List<GlobalChargeDetail>();
                var globalChargeSetupsDetail = new List<GlobalChargeSetup>();


                if (globalChargeGroupIds.Count() > 0 || globalChargeGroupIds is not null)
                {
                    foreach (var item in globalChargeGroupIds)
                    {
                        globalChargeSetupsDetail = _db.GlobalChargeSetup.Where(x => !x.IsDeleted
                                                               && x.GlobalChargeGroupId == item
                                                               && x.CategoryId == dto.CategoryId
                                                               && x.PropertyTypeId == dto.PropertyTypeId
                                                               && x.TaxStatus == dto.BFiler
                                                               && x.ConstructionStatus == dto.ConstracutionStatus
                                                               && x.RegistryVerification == dto.RegistryVerification
                                                               )
                                                           .Include(x => x.GlobalChargeDetail.Where(x => !x.IsDeleted))
                                                           .ToList();

                        if (globalChargeSetupsDetail != null)
                        {
                            decimal area = GetArea(dto.StockId);
                            foreach (var item2 in globalChargeSetupsDetail)
                            {
                                foreach (var detail in item2.GlobalChargeDetail)
                                {
                                    if (detail.Percentage <= 0 && detail.MultiplyBySize == true)
                                    {
                                        detail.Rate = (detail.Rate * area);
                                    }
                                    else if (detail.Percentage > 0 && detail.MultiplyBySize == true)
                                    {
                                        detail.Rate = (detail.Rate * area) * (detail.Percentage / 100);
                                    }
                                    else
                                    {
                                        detail.Rate = detail.Rate;
                                    }
                                    detail.SapAccount = _commonBLL.GetSapAccountByChargeTypeId(Convert.ToInt32(detail.ChargeType));
                                    detail.GlobalChargeSetup = null;
                                    globalChargeSetupDetails.Add(detail);
                                }

                            }
                        }

                    }
                }
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = new { globalChargeDetail = globalChargeSetupDetails.Distinct() }
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }
    }
}
