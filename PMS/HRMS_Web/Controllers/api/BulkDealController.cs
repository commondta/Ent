using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using B_Utility.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BulkDealController : ControllerBase
    {
        private readonly DataBase_Context _db;

        CommonBLL _commonBLL;
        ApprovalBLL _approvalBLL;
        public BulkDealController(DataBase_Context db)
        {
            _db = db;
            _approvalBLL = new ApprovalBLL(_db);
            _commonBLL = new CommonBLL(_db);
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _db.BulkDeal.Where(x => !x.IsDeleted)
                                                       .Include(x => x.Dealer)
                                                       .Include(x => x.BulkDealProperty.Where(x => !x.IsDeleted))
                                                       .Include(x => x.BulkDealProposePlan.Where(x => !x.IsDeleted))
                                                       .Include(x => x.BulkPaymentSchedule.Where(x => !x.IsDeleted))
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
                var result = _db.BulkDeal.Where(x => !x.IsDeleted && x.Id == id)
                                                       .Include(x => x.Dealer)
                                                       .Include(x => x.BulkDealProperty.Where(x => !x.IsDeleted))
                                                       .Include(x => x.BulkDealProposePlan.Where(x => !x.IsDeleted))
                                                       .Include(x => x.BulkPaymentSchedule.Where(x => !x.IsDeleted))
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
        [Route("AddNewBulkDeal")]
        public IActionResult AddNewBulkDeal(BulkDeal model)
        {
            try
            {
                
                model.IsActive = true;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = model.CreatedBy;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;

                if (model.BulkDealProperty?.Count > 0)
                {
                    foreach (var item in model.BulkDealProperty)
                    {
                        item.ModifiedBy = model.ModifiedBy;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;

                    }
                }
                if (model.BulkDealProposePlan?.Count > 0)
                {
                    foreach (var item in model.BulkDealProposePlan)
                    {
                        item.ModifiedBy = model.ModifiedBy;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;

                    }
                }
                if (model.BulkPaymentSchedule?.Count > 0)
                {
                    foreach (var item in model.BulkPaymentSchedule)
                    {
                        item.ModifiedBy = model.ModifiedBy;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;

                    }
                }

                _db.BulkDeal.Add(model);
                _db.SaveChanges();

                string message = string.Empty;

                //foreach (var item in model.BulkDealProperty)
                //{
                //    StockCreation stockCreation = (StockCreation)_db.StockCreations.Where(x => x.ID == item.StockId)
                //                                                           .FirstOrDefault();
                //    if (stockCreation != null)
                //    {
                //        stockCreation.DealerId = model.DealerId;
                //    }
                //}

                //_db.SaveChanges();

                

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

        [HttpPut]
        [Route("UpdateBulkDeal")]
        public IActionResult UpdateBulkDeal(BulkDeal model)
        {
            try
            {
                var data = _db.BulkDeal.Find(model.Id);

                if (data != null)
                {
                    data.DealNature = model.DealNature;
                    data.DealName = model.DealName;
                    data.DealType = model.DealType;
                    data.QtyProperty = model.QtyProperty;
                    data.CommissionType = model.CommissionType;
                    data.Commission = model.Commission;
                    data.RebateType = model.RebateType;
                    data.Rebate = model.Rebate;
                    data.DealExpDate = model.DealExpDate;
                    data.TotalValue = model.TotalValue;
                    data.OutstandingBalance = model.OutstandingBalance;
                    data.NetReceivable = model.NetReceivable;
                    data.TotalReceied = model.TotalReceied;
                    data.SurchargePerDay = model.SurchargePerDay;
                    data.GracePeriod = model.GracePeriod;
                    data.OneTimePayment = model.OneTimePayment;
                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;
                    data.LastModified = DateTime.Now;
                    data.IsActive = true;
                    data.IsDeleted = false;

                    _db.Entry(data).State = EntityState.Modified;
                    _db.SaveChanges();
                    //bulkDealProperty

                    if (model.BulkDealProperty?.Count > 0)
                    {
                        var result = _db.BulkDealProperty.Where(x => x.BulkDealId == model.Id).ToList();


                        _db.BulkDealProperty.RemoveRange(result);
                        _db.SaveChanges();
                    }

                    if (model.BulkDealProperty?.Count > 0)
                    {
                        foreach (var item in model.BulkDealProperty)
                        {
                            item.BulkDealId = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;

                            _db.BulkDealProperty.Add(item);
                            _db.SaveChanges();
                        }
                    }
                    //bulkDealProperty
                    //BulkDealProposePlan
                    if (model.BulkDealProposePlan?.Count > 0)
                    {
                        var result = _db.BulkDealProposePlan.Where(x => x.BulkDealId == model.Id).ToList();


                        _db.BulkDealProposePlan.RemoveRange(result);
                        _db.SaveChanges();
                    }

                    if (model.BulkDealProposePlan?.Count > 0)
                    {
                        foreach (var item in model.BulkDealProposePlan)
                        {
                            item.BulkDealId = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;

                            _db.BulkDealProposePlan.Add(item);
                            _db.SaveChanges();
                        }
                    }
                    //    BulkDealProposePlan 
                    //BulkPaymentSchedule
                    if (model.BulkPaymentSchedule?.Count > 0)
                    {
                        var result = _db.BulkPaymentSchedule.Where(x => x.BulkDealId == model.Id).ToList();


                        _db.BulkPaymentSchedule.RemoveRange(result);
                        _db.SaveChanges();
                    }

                    if (model.BulkPaymentSchedule?.Count > 0)
                    {
                        foreach (var item in model.BulkPaymentSchedule)
                        {
                            item.BulkDealId = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;

                            _db.BulkPaymentSchedule.Add(item);
                            _db.SaveChanges();
                        }
                    }
                //    BulkDealProposePlan
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
        [Route("DeleteBulkDeal")]
        public IActionResult DeleteBulkDeal(int id)
        {
            try
            {
                var model = _db.BulkDeal.Find(id);

                if (model != null)
                {
                    model.ModifiedBy = model.ModifiedBy;
                    model.LastModified = DateTime.Now;
                    model.IsActive = false;
                    model.IsDeleted = true;

                    _db.SaveChanges();

                    var dealProperties = _db.BulkDealProperty.Where(x => x.Id == model.Id).ToList();

                    if (dealProperties?.Count > 0)
                    {
                        foreach (var item in dealProperties)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;
                            _db.SaveChanges();
                        }
                    }
                    var dealPropPlan = _db.BulkDealProposePlan.Where(x => x.Id == model.Id).ToList();

                    if (dealPropPlan?.Count > 0)
                    {
                        foreach (var item in dealPropPlan)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;
                            _db.SaveChanges();
                        }
                    }
                    var dealpaymentSchedule = _db.BulkPaymentSchedule.Where(x => x.Id == model.Id).ToList();

                    if (dealpaymentSchedule?.Count > 0)
                    {
                        foreach (var item in dealpaymentSchedule)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;
                            _db.SaveChanges();
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
