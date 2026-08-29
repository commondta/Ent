using B_DB_Context;
using B_DB_Model;
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
    public class AdvanceApplicationController : ControllerBase
    {
        private readonly DataBase_Context _db;

        CommonBLL _commonBLL;
        ApprovalBLL _approvalBLL;
        public AdvanceApplicationController(DataBase_Context db)
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
                var result = _db.AdvanceApplication.Where(x => !x.IsDeleted)
                                                       .Include(x => x.DealAdvanceApplicationHistory.Where(x => !x.IsDeleted))
                                                       .Include(x => x.DealAdvanceApplicationRecipt.Where(x => !x.IsDeleted))
                                                       .Include(x => x.Deal)
                                                       .Include(x => x.Dealer)
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
                var result = _db.AdvanceApplication.Where(x => !x.IsDeleted && x.Id == id)
                                                       .Include(x => x.DealAdvanceApplicationHistory.Where(x => !x.IsDeleted))
                                                       .Include(x => x.DealAdvanceApplicationRecipt.Where(x => !x.IsDeleted))
                                                       .Include(x => x.Deal)
                                                       .Include(x => x.Dealer)
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
        [Route("AddNewAdvanceApplication")]
        public IActionResult AddNewAdvanceApplication(AdvanceApplication model)
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

                var deal = _db.Deal.Where(x => !x.IsDeleted && x.Id == model.DealId)
                                                       .Include(x => x.DealProperty)
                                                       .FirstOrDefault();

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
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = model.CreatedBy;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;

                if (model.DealAdvanceApplicationHistory?.Count > 0)
                {
                    foreach (var item in model.DealAdvanceApplicationHistory)
                    {
                        var paymentPlan = deal.DealProperty.Where(x => x.RegistrationNo == item.RegistrationNo).FirstOrDefault();

                        if (paymentPlan != null)
                        {
                            paymentPlan.ReceiedAmount = paymentPlan.ReceiedAmount + item.AmountApplied;
                            paymentPlan.OutstandingBalance = paymentPlan.OutstandingBalance - item.AmountApplied;
                            deal.TotalReceied = deal.TotalReceied + item.AmountApplied;
                            deal.OutstandingBalance = deal.OutstandingBalance - item.AmountApplied;

                            _db.SaveChanges();
                        }

                        item.ModifiedBy = model.ModifiedBy;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                if (model.DealAdvanceApplicationRecipt?.Count > 0)
                {
                    foreach (var item in model.DealAdvanceApplicationRecipt)
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

                _db.AdvanceApplication.Add(model);
                _db.SaveChanges();

                //StockCreation stockCreation = (StockCreation)_db.StockCreations.Where(x => x.ID == model.StockCreationId)
                //                                                               .FirstOrDefault();
                //if (stockCreation != null)
                //{
                //    stockCreation.MemberProfileId = model.MemberProfileId;
                //    _db.SaveChanges();
                //}

                //StockCreation stockCreation = (StockCreation)_db.StockCreations.Where(x => x.ID == model.StockCreationId)
                //                                                               .FirstOrDefault();
                //if (stockCreation != null)
                //{
                //    stockCreation.Is_ConstructionSecurityRequested = true;
                //    _db.SaveChanges();

                //    bool result = _approvalBLL.AddNewApprovalSetup(stockCreation.ID, (int)ApprovalUIIds.ConstructionSecurity);

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
                    Data = null
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPut]
        [Route("UpdateAdvanceApplication")]
        public IActionResult UpdateAdvanceApplication(AdvanceApplication model)
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

                var data = _db.AdvanceApplication.Find(model.Id);

                if (data != null)
                {

                    var deal = _db.Deal.Where(x => !x.IsDeleted && x.Id == model.DealId)
                                       .Include(x => x.DealProperty)
                                       .FirstOrDefault();

                    if (data.DealAdvanceApplicationHistory?.Count > 0)
                    {
                        foreach (var item in data.DealAdvanceApplicationHistory)
                        {
                            var dealProperty = deal.DealProperty.Where(x => x.RegistrationNo == item.RegistrationNo).FirstOrDefault();

                            if (dealProperty != null)
                            {
                                dealProperty.ReceiedAmount = dealProperty.ReceiedAmount - item.AmountApplied;
                                dealProperty.OutstandingBalance = dealProperty.OutstandingBalance + item.AmountApplied;

                                _db.SaveChanges();
                            }
                        }
                    }

                    if (model.DealAdvanceApplicationHistory?.Count > 0)
                    {
                        foreach (var item in model.DealAdvanceApplicationHistory)
                        {
                            var dealProperty = deal.DealProperty.Where(x => x.RegistrationNo == item.RegistrationNo).FirstOrDefault();

                            if (dealProperty != null)
                            {
                                dealProperty.ReceiedAmount = dealProperty.ReceiedAmount + item.AmountApplied;
                                dealProperty.OutstandingBalance = dealProperty.OutstandingBalance - item.AmountApplied;

                                _db.SaveChanges();
                            }
                        }
                    }

                    if (model.DealAdvanceApplicationRecipt?.Count > 0)
                    {
                        var result = _db.DealAdvanceApplicationRecipt.Where(x => x.AdvanceApplicationId == model.Id).ToList();

                        _db.DealAdvanceApplicationRecipt.RemoveRange(result);
                        _db.SaveChanges();
                    }

                    if (model.DealAdvanceApplicationRecipt?.Count > 0)
                    {
                        foreach (var item in model.DealAdvanceApplicationRecipt)
                        {
                            item.AdvanceApplicationId = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.DealAdvanceApplicationRecipt.AddRange(model.DealAdvanceApplicationRecipt);
                        _db.SaveChanges();
                    }


                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;
                    data.LastModified = DateTime.Now;

                    _db.Entry(data).State = EntityState.Modified;
                    _db.SaveChanges();
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
        [Route("DeleteAdvanceApplication")]
        public IActionResult DeleteAdvanceApplication(int id)
        {
            try
            {
                var model = _db.AdvanceApplication.Find(id);

                if (model != null)
                {
                    model.ModifiedBy = model.ModifiedBy;
                    model.LastModified = DateTime.Now;
                    model.IsActive = false;
                    model.IsDeleted = true;

                    _db.SaveChanges();

                    var dealAdvanceApplicationHistories = _db.DealAdvanceApplicationHistery.Where(x => x.AdvanceApplicationId == model.Id).ToList();

                    if (dealAdvanceApplicationHistories?.Count > 0)
                    {
                        foreach (var item in dealAdvanceApplicationHistories)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;
                            _db.SaveChanges();
                        }
                    }

                    var dealAdvanceApplicationRecipts = _db.DealAdvanceApplicationRecipt.Where(x => x.AdvanceApplicationId == model.Id).ToList();

                    if (dealAdvanceApplicationRecipts?.Count > 0)
                    {
                        foreach (var item in dealAdvanceApplicationRecipts)
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
