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
    public class DealController : ControllerBase
    {
        private readonly DataBase_Context _db;

        CommonBLL _commonBLL;
        ApprovalBLL _approvalBLL;
        public DealController(DataBase_Context db)
        {
             _db = db;
            _approvalBLL = new ApprovalBLL(_db);
            _commonBLL = new CommonBLL(_db);
        }

        //[HttpGet]
        //[Route("/api/ConstructionSecurity/GetAllConstructionSecurityFilterList")]
        //public IActionResult GetAllConstructionSecurityFilterList()
        //{
        //    try
        //    {
        //        var result = _db.StockCreations.Where(x => !x.is_deleted
        //                                           && x.Is_MapApprovalApproved == true
        //                                           && x.Is_ConstructionSecurityRequested != true
        //                                             )
        //                                       .ToList();

        //        return Ok(new ApiResponse<object>
        //        {
        //            Code = ResponseCode.Success,
        //            Message = "Success",
        //            Data = result
        //        });
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
        //    }
        //}

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _db.Deal.Where(x => !x.IsDeleted)
                                                       .Include(x => x.Dealer)
                                                       .Include(x => x.DealProperty.Where(x => !x.IsDeleted))
                                                       .ThenInclude(x => x.DealPaymentPlan.Where(x => !x.IsDeleted))
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
                var result = _db.Deal.Where(x => !x.IsDeleted && x.Id == id)
                                                       .Include(x => x.Dealer)
                                                       .Include(x => x.DealProperty.Where(x => !x.IsDeleted))
                                                       .ThenInclude(x => x.DealPaymentPlan.Where(x => !x.IsDeleted))
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
        [Route("AddNewDeal")]
        public IActionResult AddNewDeal(Deal model)
        {
            try
            {
                //bool isApprovalActive = true;

                //var approvalStatus = _db.ApprovalUI.Find((int)ApprovalUIIds.BookingForm);
                //if (approvalStatus != null)
                //{
                //    if (approvalStatus.Checked != true)
                //    {
                //        isApprovalActive = false;
                //    }
                //}

                //var approvalSetup = _db.ApprovalSetup.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.BookingForm).ToList();
                //if (approvalSetup.Count <= 0 && isApprovalActive == true)
                //{
                //    return Ok(new ApiResponse<object>
                //    {
                //        Code = ResponseCode.Success,
                //        Message = "Approval setup not defined or In-active",
                //        Data = null
                //    });
                //}
                model.IsActive = true;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = model.CreatedBy;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;

                if (model.DealProperty?.Count > 0)
                {
                    foreach (var item in model.DealProperty)
                    {
                        item.ModifiedBy = model.ModifiedBy;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;

                        if(item.DealPaymentPlan.Count > 0)
                        {
                            foreach(var paymentPlan in item.DealPaymentPlan)
                            {
                                paymentPlan.ModifiedBy = model.ModifiedBy;
                                paymentPlan.CreatedBy = model.CreatedBy;
                                paymentPlan.LastModifiedUserName = model.LastModifiedUserName;
                                paymentPlan.LastModified = DateTime.Now;
                                paymentPlan.CreatedOn = DateTime.Now;
                                paymentPlan.IsActive = true;
                                paymentPlan.IsDeleted = false;
                            }
                        }
                    }
                }
                
                _db.Deal.Add(model);
                _db.SaveChanges();

                string message = string.Empty;

                foreach(var item in model.DealProperty)
                {
                    StockCreation stockCreation = (StockCreation)_db.StockCreations.Where(x => x.ID == item.StockId)
                                                                           .FirstOrDefault();
                    if (stockCreation != null)
                    {
                        stockCreation.DealerId = model.DealerId;
                    }
                }

                _db.SaveChanges();

                //Deal deal = (Deal)_db.Deal.Where(x => x.Id == model.Id)
                //                          .FirstOrDefault();
                //if (deal != null)
                //{
                //    deal.IsDealRequested = true;
                //    _db.SaveChanges();

                //    if (isApprovalActive == true)
                //    {
                //        // replace bookingform with dealform all approval places
                //        bool result = _approvalBLL.AddNewApprovalSetup(model.Id, (int)ApprovalUIIds.BookingForm);
                //        message = "Deal added succesfully and moved for approval";
                //        if (result)
                //        {
                //            return Ok(new ApiResponse<object>
                //            {
                //                Code = ResponseCode.Success,
                //                Message = message,
                //                Data = null
                //            });
                //        }
                //    }
                //    else
                //    {
                //        deal.IsDealApproved = true;
                //        _db.SaveChanges();

                //        message = "Deal added succesfully";

                //        return Ok(new ApiResponse<object>
                //        {
                //            Code = ResponseCode.Success,
                //            Message = message,
                //            Data = null
                //        });
                //    }
                //}

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
        [Route("UpdateDeal")]
        public IActionResult UpdateDeal(Deal model)
        {
            try
            { 
                var data = _db.Deal.Find(model.Id);

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


                    if (model.DealProperty?.Count > 0)
                    {
                        var result = _db.DealProperty.Where(x => x.DealId == model.Id).ToList();

                        if (result.Count > 0)
                        {
                            foreach (var item in result)
                            {
                                var paymentPlan = _db.DealPaymentPlan.Where(x=>x.DealPropertyId == item.Id).ToList();
                                if (paymentPlan.Count >0)
                                {
                                    _db.RemoveRange(paymentPlan);
                                    _db.SaveChanges();
                                }
                            }
                        }

                        _db.DealProperty.RemoveRange(result);
                        _db.SaveChanges();
                    }

                    if (model.DealProperty?.Count > 0)
                    {
                        foreach (var item in model.DealProperty)
                        {
                            item.DealId = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;

                            _db.DealProperty.Add(item);
                            _db.SaveChanges();

                            if (item.DealPaymentPlan?.Count > 0)
                            {
                                foreach (var paymentPlan in item.DealPaymentPlan)
                                {
                                    paymentPlan.ModifiedBy = model.ModifiedBy;
                                    paymentPlan.LastModifiedUserName = model.LastModifiedUserName;
                                    paymentPlan.LastModified = DateTime.Now;
                                    paymentPlan.CreatedOn = DateTime.Now;
                                    paymentPlan.IsActive = true;
                                    paymentPlan.IsDeleted = false;
                                }
                                 _db.SaveChanges();
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

        [HttpDelete]
        [Route("DeleteDeal")]
        public IActionResult DeleteDeal(int id)
        {
            try
            {
                var model = _db.Deal.Find(id);

                if (model != null)
                {
                    model.ModifiedBy = model.ModifiedBy;
                    model.LastModified = DateTime.Now;
                    model.IsActive = false;
                    model.IsDeleted = true;

                    _db.SaveChanges();

                    var dealProperties = _db.DealProperty.Where(x => x.Id == model.Id).ToList();

                    if (dealProperties?.Count > 0)
                    {
                        foreach (var item in dealProperties)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;

                            if (item.DealPaymentPlan?.Count > 0)
                            {
                                foreach (var paymentPlan in item.DealPaymentPlan)
                                {
                                    paymentPlan.LastModified = DateTime.Now;
                                    paymentPlan.IsActive = false;
                                    paymentPlan.IsDeleted = true;
                                }
                            }
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
