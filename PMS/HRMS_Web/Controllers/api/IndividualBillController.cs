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
    public class IndividualBillController : ControllerBase
    {
        private readonly DataBase_Context _db;

        CommonBLL _commonBLL;
        ApprovalBLL _approvalBLL;
        public IndividualBillController(DataBase_Context db)
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
                var result = _db.IndividualBill.Where(x => !x.IsDeleted)
                                                       .Include(x => x.IndividualBillDetail.Where(x => !x.IsDeleted))
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
                var result = _db.IndividualBill.Where(x => !x.IsDeleted && x.Id == id)
                                                       .Include(x => x.IndividualBillDetail.Where(x => !x.IsDeleted))
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
        [Route("AddNewIndividualBill")]
        public IActionResult AddNewIndividualBill(IndividualBill model)
        {
            try
            {
   
                if (model.BillFor != "Fixed Dues")
                {
                    foreach (var item in model.IndividualBillDetail)
                    {
                        var data = _db.MeterBillGenerationDetail.FirstOrDefault(x => x.Id == item.Id);

                        if (data != null)
                        {
                            data.Surcharge = item.Surcharge;
                            data.OtherDuesDescription = item.OtherDuesDescription;
                            data.OtherDuesAmount = item.OtherDuesAmount;
                            data.GrossAmount = item.GrossAmount;
                            data.Discount = item.Discount;
                            data.NetAmount = item.NetAmount;

                            _db.Entry(data).State = EntityState.Modified;
                            _db.SaveChanges();
                        }
                    }
                }

                else
                {
                   foreach(var item in model.IndividualBillDetail)
                    {
                        var data = _db.FixedChargeBillDetail.FirstOrDefault(x => x.Id == item.Id);

                        if (data != null)
                        {
                            data.Surcharge = item.Surcharge;
                            data.OtherDuesDescription = item.OtherDuesDescription;
                            data.OtherDuesAmount = item.OtherDuesAmount;
                            data.GrossAmount = item.GrossAmount;
                            data.Discount = item.Discount;
                            data.NetAmount = item.NetAmount;
                            data.ModifiedBy = model.ModifiedBy;
                            data.CreatedBy = model.CreatedBy;
                            data.LastModifiedUserName = model.LastModifiedUserName;

                            _db.Entry(data).State = EntityState.Modified;
                            _db.SaveChanges();
                        }
                    }
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

        [HttpPut]
        [Route("UpdateIndividualBill")]
        public IActionResult UpdateIndividualBill(IndividualBill model)
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

                var data = _db.IndividualBill.Find(model.Id);

                if (data != null)
                {
                    data.Month = model.Month;
                    data.BillFor = model.BillFor;
                    data.StockCreationID = model.StockCreationID;
                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;
                    data.LastModified = DateTime.Now;

                    _db.Entry(data).State = EntityState.Modified;
                    _db.SaveChanges();


                    if (model.IndividualBillDetail?.Count > 0)
                    {
                        var result = _db.IndividualBillDetail.Where(x => x.IndividualBillId == model.Id).ToList();

                        _db.IndividualBillDetail.RemoveRange(result);
                        _db.SaveChanges();
                    }

                    if (model.IndividualBillDetail?.Count > 0)
                    {
                        foreach (var item in model.IndividualBillDetail)
                        {
                            item.IndividualBillId = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.IndividualBillDetail.AddRange(model.IndividualBillDetail);
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
        [Route("DeleteIndividualBill")]
        public IActionResult DeleteIndividualBill(int id)
        {
            try
            {
                var model = _db.IndividualBillDetail.Find(id);

                if (model != null)
                {
                    model.ModifiedBy = model.ModifiedBy;
                    model.LastModified = DateTime.Now;
                    model.IsActive = false;
                    model.IsDeleted = true;

                    _db.SaveChanges();

                    var individualBillDetails = _db.IndividualBillDetail.Where(x => x.IndividualBillId == model.Id).ToList();

                    if (individualBillDetails?.Count > 0)
                    {
                        foreach (var item in individualBillDetails)
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
