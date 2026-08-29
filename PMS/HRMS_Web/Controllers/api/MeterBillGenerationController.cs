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
    public class MeterBillGenerationController : ControllerBase
    {
        private readonly DataBase_Context _db;

        CommonBLL _commonBLL;
        ApprovalBLL _approvalBLL;
        public MeterBillGenerationController(DataBase_Context db)
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
                var result = _db.MeterBillGeneration.Where(x => !x.IsDeleted)
                                                       .Include(x => x.MeterBillGenerationDetail.Where(x => !x.IsDeleted))
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
                var result = _db.MeterBillGeneration.Where(x => !x.IsDeleted && x.Id == id)
                                                       .Include(x => x.MeterBillGenerationDetail.Where(x => !x.IsDeleted))
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
        [Route("AddNewMeterBillGeneration")]
        public IActionResult AddNewMeterBillGeneration(MeterBillGeneration model)
        {
            try
            {
                var isExist = _db.MeterBillGeneration.Where(x => x.Month == model.Month &&
                                                            x.BillFor == model.BillFor)
                                                     .FirstOrDefault();
                if(isExist != null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "Detail Already Exist! You can only view it",
                        Data = null
                    });
                }

                //bool isApprovalActive = true;

                //var approvalStatus = _db.ApprovalUI.Find((int)ApprovalUIIds.Transfer);
                //if (approvalStatus != null)
                //{
                //    if (approvalStatus.Checked != true)
                //    {
                //        isApprovalActive = false;
                //    }
                //}

                //var approvalSetup = _db.ApprovalSetup.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.Transfer).ToList();
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

                if (model.MeterBillGenerationDetail?.Count > 0)
                {
                    foreach (var item in model.MeterBillGenerationDetail)
                    {
                        var meterDetail = _db.MeterDetail.Where(x => x.MeterNumber == item.MeterNo).FirstOrDefault();

                        if (meterDetail != null)
                        {
                            meterDetail.UnitsAtInstallation = decimal.Parse(item.CurrentReading);
                            _db.Entry(meterDetail).State = EntityState.Modified;
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


                _db.MeterBillGeneration.Add(model);
                _db.SaveChanges();

                //string message = string.Empty;

                //TransferHistory transferHistory = (TransferHistory)_db.TransferHistory.Where(x => x.Id == model.Id)
                //                                      .FirstOrDefault();
                //if (transferHistory != null)
                //{
                //    transferHistory.IsTransferRequested = true;
                //    _db.SaveChanges();

                //    if (isApprovalActive == true)
                //    {
                //        bool result = _approvalBLL.AddNewApprovalSetup(model.Id, (int)ApprovalUIIds.Transfer);
                //        message = "Transfer added succesfully and moved for approval";
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
                //        transferHistory.IsTransferApproved = true;
                //        _db.SaveChanges();

                //        message = "Transfer added succesfully";

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
        [Route("UpdateMeterBillGeneration")]
        public IActionResult UpdateMeterBillGeneration(MeterBillGeneration model)
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

                var data = _db.MeterBillGeneration.Find(model.Id);

                if (data != null)
                {
                    data.Month = model.Month;
                    data.BillFor = model.BillFor;
                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;
                    data.LastModified = DateTime.Now;

                    _db.Entry(data).State = EntityState.Modified;
                    _db.SaveChanges();


                    if (model.MeterBillGenerationDetail?.Count > 0)
                    {
                        var result = _db.MeterBillGenerationDetail.Where(x => x.MeterBillGenerationId == model.Id).ToList();

                        _db.MeterBillGenerationDetail.RemoveRange(result);
                        _db.SaveChanges();
                    }

                    if (model.MeterBillGenerationDetail?.Count > 0)
                    {
                        foreach (var item in model.MeterBillGenerationDetail)
                        {
                            item.MeterBillGenerationId = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.MeterBillGenerationDetail.AddRange(model.MeterBillGenerationDetail);
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
        [Route("DeleteMeterBillGenerationDetail")]
        public IActionResult DeleteMeterBillGenerationDetail(int id)
        {
            try
            {
                var model = _db.MeterBillGenerationDetail.Find(id);

                if (model != null)
                {
                    model.ModifiedBy = model.ModifiedBy;
                    model.LastModified = DateTime.Now;
                    model.IsActive = false;
                    model.IsDeleted = true;

                    _db.SaveChanges();

                    var meterBillGenerationDetails = _db.MeterBillGenerationDetail.Where(x => x.MeterBillGenerationId == model.Id).ToList();

                    if (meterBillGenerationDetails?.Count > 0)
                    {
                        foreach (var item in meterBillGenerationDetails)
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

        // Discard
        [HttpDelete]
        [Route("Discard")]
        public IActionResult Discard(string month)
        {
            var sapPost = _db.SAPBillPostingCheck.Where(x => x.Month == month).FirstOrDefault();
            if (sapPost == null)
            {
                var bill = _db.MeterBillGeneration.Where(x => x.Month == month).FirstOrDefault();

                if (bill != null)
                {

                    var meterBillGenerations = _db.MeterBillGeneration.Where(x => x.Month == month)
                                                             .Include(x => x.MeterBillGenerationDetail)
                                                             .ToList();
                    if (meterBillGenerations.Count() > 0)
                    {
                        foreach (var charge in meterBillGenerations)
                        {
                            _db.MeterBillGenerationDetail.RemoveRange(charge.MeterBillGenerationDetail);
                            _db.MeterBillGeneration.Remove(charge);
                            _db.SaveChanges();

                        }
                    }
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Meter Bills Record Discard For This Month Successfully",
                        Data = null
                    });
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
            }
            return Ok(new ApiResponse<object>
            {
                Code = ResponseCode.Conflict,
                Message = "You can't discard beacuse its already posted in SAP",
                Data = null
            });
        }
    }
}
