using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common.Enums;
using B_Utility.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HRMS_Web.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentPlanSetupController : ControllerBase
    {
        private readonly DataBase_Context _db;
        ApprovalBLL _approvalBLL;
        public PaymentPlanSetupController(DataBase_Context db)
        {
            _db = db;
            _approvalBLL = new ApprovalBLL(_db);
        }

        [HttpGet]
        [Route("GetGlobalChargeDetail")]
        public IActionResult GetGlobalChargeDetail(int formId)
        {
            try
            {
                var globalChargeGroupIds = _db.FormsChargeGroup.Where(x => x.FormId == formId && !x.IsDeleted).Select(x => x.ChargeGroupId).ToList();

                List<ChargeGroupType> chargeGroupTypes = new List<ChargeGroupType>();
                List<ChargeGroupType> chargeGroupsTypes = new List<ChargeGroupType>();
                
                if (globalChargeGroupIds.Count() > 0 || globalChargeGroupIds is not null)
                {
                    foreach (var item in globalChargeGroupIds)
                    {
                        chargeGroupTypes = _db.ChargeGroupType.Where(x => !x.IsDeleted
                                                               && x.GlobalChargeGroupId == item
                                                               )
                                                           .ToList();
                        if (chargeGroupTypes.Count() > 0)
                        {
                            chargeGroupsTypes.AddRange(chargeGroupTypes);
                        }
                    }

                }
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = chargeGroupsTypes
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
                var result = _db.PaymentPlanSetup.Where(x => !x.IsDeleted)
                                                 .Include(x => x.PlanInformation.Where(x=>!x.IsDeleted))
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
        [Route("Get")]
        public IActionResult Get(int id)
        {
            try
            {
                var result = _db.PaymentPlanSetup.Where(x => !x.IsDeleted && x.Id == id)
                                                                      .Include(x => x.PlanInformation.Where(x=>!x.IsDeleted))
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
        [Route("AddNewPaymentPlanSetup")]
        public IActionResult AddNewPaymentPlanSetup(PaymentPlanSetup model)
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

                //var approvalSetup = _db.ApprovalSetup.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.ConstructionSecurity).ToList();
                //if (approvalSetup.Count <= 0)
                //{
                //    return Ok(new ApiResponse<object>
                //    {
                //        Code = ResponseCode.NotFound,
                //        Message = "Not Found",
                //        Data = "Approval setup not defined or In-active"
                //    });
                //}
                model.IsActive = true;
                model.Status = "Pending";
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = model.CreatedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;

                if (model.PlanInformation?.Count > 0)
                {
                    foreach (var item in model.PlanInformation)
                    {
                        item.IsActive = true;
                        item.CreatedOn = DateTime.Now;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModified = DateTime.Now;
                        item.ModifiedBy = model.ModifiedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                    }
                }

                _db.PaymentPlanSetup.Add(model);
                _db.SaveChanges();

                //StockCreation stockCreation = (StockCreation)_db.StockCreations.Where(x => x.ID == model.StockCreationId)
                //                                                               .FirstOrDefault();
                //if (stockCreation != null)
                //{
                //    stockCreation.Is_ConstructionMonitoringRequested = true;
                //    _db.SaveChanges();

                //    bool result = _approvalBLL.AddNewApprovalSetup(stockCreation.ID, (int)ApprovalUIIds.ConstructionMonitoring);

                //    if (result)
                //    {
                //        return Ok(new ApiResponse<object>
                //        {
                //            Code = ResponseCode.Success,
                //            Message = "Success",
                //            Data = "Construction Security added succesfully and moved for approval"
                //        });
                //    }
                //}

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
        [Route("UpdatePaymentPlanSetup")]
        public IActionResult UpdatePaymentPlanSetup(PaymentPlanSetup model)
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

                var data = _db.PaymentPlanSetup.Find(model.Id);

                if (data != null)
                {
                    data.PlanType = model.PlanType;
                    data.Description = model.Description;
                    data.PhaseId = model.PhaseId;
                    data.RealEsateId = model.RealEsateId;   
                    data.RealEsateId = model.CategoryId;
                    data.ProjectId = model.ProjectId;
                    data.BlockId = model.BlockId;   
                    data.NatureId = model.NatureId;
                    data.NUmberOfInstallment = model.NUmberOfInstallment;
                    data.LandCost = model.LandCost;
                    data.InstallmentDays = model.InstallmentDays;
                    data.SurChargePerDay = model.SurChargePerDay;
                    data.GrancePeriodFine = model.GrancePeriodFine;
                    data.Total = model.Total;
                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;
                    data.LastModified = DateTime.Now;

                    _db.Entry(data).State = EntityState.Modified;
                    _db.SaveChanges();

                    if (model.PlanInformation?.Count > 0)
                    {
                        var result = _db.PlanInformation.Where(x => x.PaymentPlanSetupId == model.Id).ToList();

                        _db.PlanInformation.RemoveRange(result);
                        _db.SaveChanges();
                    }

                    if (model.PlanInformation?.Count > 0)
                    {
                        foreach (var item in model.PlanInformation)
                        {
                            item.PaymentPlanSetupId = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.CreatedOn = DateTime.Now;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.PlanInformation.AddRange(model.PlanInformation);
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
                    Data = data
                });

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpDelete]
        [Route("DeletePaymentPlanSetup")]
        public IActionResult DeletePaymentPlanSetup(int id)
        {
            try
            {
                var model = _db.PaymentPlanSetup.Find(id);

                if (model != null)
                {
                    model.LastModified = DateTime.Now;
                    model.IsActive = false;
                    model.IsDeleted = true;

                    _db.SaveChanges();

                    var result = _db.PlanInformation.Where(x => x.PaymentPlanSetupId == model.Id).ToList();

                    foreach (var item in result)
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
    }   
}
