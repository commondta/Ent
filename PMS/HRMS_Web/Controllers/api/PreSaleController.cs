using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using B_Utility.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PreSaleController : ControllerBase
    {
        private readonly DataBase_Context _db;
      
        ApprovalBLL _approvalBLL;
        CommonBLL _commonBLL;
        public PreSaleController(DataBase_Context db)
        {   
            _db = db;
            _approvalBLL = new ApprovalBLL(_db);
            _commonBLL = new CommonBLL(_db);
        }

        [HttpGet]
        [Route("GetFilterList")]
        public IActionResult GetFilterList()
        {
            try
            {
                var result =  _db.StockCreations.Where(x => x.Is_StockCreationApproved == true
                                                           && x.RegistrationNo != ""
                                                           && x.RegistrationNo != null
                                                           && x.IsPreSaleRequested != true
                                                       )
                                                  .Select(x=> new StockCreation
                                                  {
                                                      ID = x.ID,
                                                      PropertyNo = x.PropertyNo ?? "N/A",
                                                      RegistrationNo = x.RegistrationNo,
                                                      RealStateType = x.RealStateType,
                                                      Type = x.Type,
                                                      Status = x.Status
                                                  }).ToList().OrderByDescending(x => x.ID);

                if (result?.Count() > 0)
                {
                    foreach (var item in result)
                    {
                        item.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(item.RealStateType));
                        item.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(item.Type));
                    }
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
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _db.PreSale.Where(x => !x.IsDeleted)
                                                       .Include(x => x.TermsConditions)
                                                       .Include(x => x.PaymentPlan.Where(x => !x.IsDeleted))
                                                       .Include(x=>x.StockCreation)
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
                var result = _db.PreSale.Where(x => !x.IsDeleted && x.Id == id)
                                                                      .Include(x => x.TermsConditions)
                                                                      .Include(x => x.PaymentPlan.Where(x => !x.IsDeleted))
                                                                      .Include(x => x.StockCreation)
                                                                      .Include(x=> x.MemberProfile)
                                                                      .Include(x=> x.Dealer)
                                                                      .FirstOrDefault();
                if (result != null)
                {
                    result.StockCreation.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(result.StockCreation.RealStateType));
                    result.StockCreation.ProjectName = _commonBLL.GetProjectName(Convert.ToInt32(result.StockCreation.Project));
                    result.StockCreation.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(result.StockCreation.Phase));
                    result.StockCreation.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(result.StockCreation.Category));
                    result.StockCreation.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(result.StockCreation.Block));
                    result.StockCreation.NatureName = _commonBLL.GetNatureName(Convert.ToInt32(result.StockCreation.Nature));
                    result.StockCreation.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(result.StockCreation.Type));
                    result.StockCreation.ConstracutionStatus = _commonBLL.GetConstrcutionStatus(Convert.ToInt32(result.StockCreation.ID));
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

        [HttpPost]
        [Route("/api/PreSale/AddNewPreSale")]
        public IActionResult AddNewPreSale(PreSale model)
        {
            try
            {
                bool isApprovalActive = true;

                var approvalStatus = _db.ApprovalUI.Find((int)ApprovalUIIds.PreSale);
                if (approvalStatus != null)
                {
                    if (approvalStatus.Checked != true)
                    {
                        isApprovalActive = false;
                    }
                }

                var approvalSetup = _db.ApprovalSetup.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.PreSale).ToList();
                if (approvalSetup.Count <= 0 && isApprovalActive == true)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Approval setup not defined or In-active",
                        Data = null
                    });
                }
                model.IsActive = true;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = model.CreatedBy;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;

                if (model.PaymentPlan?.Count > 0)
                {
                    foreach (var item in model.PaymentPlan)
                    {
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.ModifiedBy = model.ModifiedBy;
                    }
                }

                _db.PreSale.Add(model);
                _db.SaveChanges();

                string message = string.Empty;

                StockCreation stock = (StockCreation)_db.StockCreations.Where(x => x.ID == model.StockCreationId)
                                                      .FirstOrDefault();
                if (stock != null)
                {
                    stock.IsPreSaleRequested = true;
                    _db.SaveChanges();

                    if(isApprovalActive == true)
                    { 
                        bool result = _approvalBLL.AddNewApprovalSetup(model.Id, (int)ApprovalUIIds.PreSale);
                        message = "Pre Sale added succesfully and moved for approval";
                        if (result)
                        {
                            return Ok(new ApiResponse<object>
                            {
                                Code = ResponseCode.Success,
                                Message = message,
                                Data = null
                            });
                        }
                    }
                    else
                    {
                        stock.IsPreSaleApproved = true;
                        _db.SaveChanges();

                        message = "Pre Sale added succesfully";

                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = message,
                            Data = null
                        });
                    }
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "Not Found",
                    Data = null
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        [Route("UpdatePreSale")]
        public IActionResult UpdatePreSale(PreSale model)
        {
            try
            {
                var data = _db.PreSale.Find(model.Id);

                if (data != null)
                {
                    data.Status = model.Status;
                    data.MemberName = model.MemberName;
                    data.Cnic = model.Cnic;
                    data.Address = model.Address;
                    data.Email = model.Email;
                    data.MobileNo = model.MobileNo;
                    data.ByCareOf = model.ByCareOf;
                    data.ReferedBy = model.ReferedBy;
                    data.DealerCode = model.DealerCode;
                    data.DealerName = model.DealerName;
                    data.SaleBy = model.SaleBy;
                    data.TranscationType = model.TranscationType;
                    data.Remarks = model.Remarks;
                    data.OneTimePayment = model.OneTimePayment;
                    data.Installments = model.Installments;
                    data.PlanCode = model.PlanCode;
                    data.TotalCost = model.TotalCost;
                    data.TotalRebate = model.TotalRebate;
                    data.NetCost = model.NetCost;
                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;
                    data.LastModified = DateTime.Now;

                    _db.Entry(data).State = EntityState.Modified;
                    _db.SaveChanges();

                    if (model.TermsConditions?.Count > 0)
                    {
                        var termsConditions = _db.TermsConditions.Where(x => x.PreSaleId == model.Id).ToList();

                        _db.TermsConditions.RemoveRange(termsConditions);
                        _db.SaveChanges();
                    }

                    if (model.TermsConditions?.Count > 0)
                    {
                        foreach (var item in model.TermsConditions)
                        {
                            item.PreSaleId = data.Id;
                        }

                        _db.TermsConditions.AddRange(model.TermsConditions);
                        _db.SaveChanges();
                    }

                    if (model.PaymentPlan?.Count > 0)
                    {
                        var paymentPlans = _db.PaymentPlan.Where(x => x.PreSaleId == model.Id).ToList();

                        _db.PaymentPlan.RemoveRange(paymentPlans);
                        _db.SaveChanges();
                    }

                    if (model.PaymentPlan?.Count > 0)
                    {
                        foreach (var item in model.PaymentPlan)
                        {
                            item.PreSaleId = data.Id;
                            item.CreatedOn = DateTime.Now;
                            item.LastModified = DateTime.Now;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.PaymentPlan.AddRange(model.PaymentPlan);
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
        [Route("DeletePreSale")]
        public IActionResult DeletePreSale(int id)
        {
            try
            {
                var model = _db.PreSale.Find(id);

                if (model != null)
                {
                    model.LastModified = DateTime.Now;
                    model.IsActive = false;
                    model.IsDeleted = true;

                    _db.SaveChanges();

                    var termsConditions = _db.TermsConditions.Where(x => x.PreSaleId == model.Id).ToList();
                    
                    if(termsConditions?.Count > 0)
                    { 
                        _db.TermsConditions.RemoveRange(termsConditions);
                        _db.SaveChanges();
                    }

                    var paymentPlans = _db.PaymentPlan.Where(x => x.PreSaleId == model.Id).ToList();

                    if (paymentPlans?.Count > 0)
                    {
                        foreach (var item in paymentPlans)
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
