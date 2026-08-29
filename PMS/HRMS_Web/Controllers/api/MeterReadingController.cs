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
    public class MeterReadingController : ControllerBase
    {
        private readonly DataBase_Context _db;

        CommonBLL _commonBLL;
        ApprovalBLL _approvalBLL;
        public MeterReadingController(DataBase_Context db)
        {
            _db = db;
            _approvalBLL = new ApprovalBLL(_db);
            _commonBLL = new CommonBLL(_db);
        }

        [HttpGet]
        [Route("FilterByMeterNo")]
        public IActionResult FilterByMeterNo(string month, string meterNo)
        {
            try
            {
                var result = _db.ReadingDetail.Where(x => !x.IsDeleted && x.MeterNo == meterNo && x.MeterReading.Month == month)
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
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _db.MeterReading.Where(x => !x.IsDeleted)
                                                       .Include(x => x.ReadingDetail.Where(x => !x.IsDeleted))
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
                var result = _db.MeterReading.Where(x => !x.IsDeleted && x.Id == id)
                                                       .Include(x => x.ReadingDetail.Where(x => !x.IsDeleted))
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
        [Route("AddNewMeterReading")]
        public IActionResult AddNewMeterReading(MeterReading model)
        {
            try
            {

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

                if (model.ReadingDetail?.Count > 0)
                {
                    foreach (var item in model.ReadingDetail)
                    {
                        var meterReading = _db.ReadingDetail.Where(x => x.MeterReading.Month == model.Month && x.MeterNo == item.MeterNo).FirstOrDefault();

                        if (meterReading == null)
                        {
                            //var meterDetail = _db.MeterDetail.Where(x => x.MeterNumber == item.MeterNo).FirstOrDefault();

                            //if (meterDetail != null)
                            //{
                            //    meterDetail.UnitsAtInstallation = item.CurrentReading;
                            //    _db.Entry(meterDetail).State = EntityState.Modified;
                            //    _db.SaveChanges();
                            //}

                            item.ModifiedBy = model.ModifiedBy;
                            item.CreatedBy = model.CreatedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.CreatedOn = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }
                    }
                }

                _db.MeterReading.Add(model);
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
                    Message = "Reading Saved",
                    Data = null
                });

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPut]
        [Route("UpdateMeterReading")]
        public IActionResult UpdateMeterReading(MeterReading model)
        {
            try
            {
                var data = _db.MeterReading.Find(model.Id);

                if (data != null)
                {
                    data.Month = model.Month;
                    data.MeterReadingOfficer = model.MeterReadingOfficer;
                    data.ReadingFor = model.ReadingFor;
                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;
                    data.LastModified = DateTime.Now;

                    _db.Entry(data).State = EntityState.Modified;
                    _db.SaveChanges();

                    if (model.ReadingDetail?.Count > 0)
                    {
                        foreach (var item in model.ReadingDetail)
                        {
                            var result = _db.ReadingDetail.Where(x => x.MeterNo == item.MeterNo && x.MeterReadingId == data.Id).FirstOrDefault();
                            if (result != null)
                            {
                                result.PropertyNo = item.PropertyNo;
                                result.UnitsConsumed = item.UnitsConsumed;
                                result.CurrentReading = item.CurrentReading;
                                result.LastReading = item.LastReading;
                                result.ReadingOfficerId = item.ReadingOfficerId;
                                result.Picture = item.Picture;
                                result.LastModified = DateTime.Now;

                                _db.Entry(result).State = EntityState.Modified;
                                _db.SaveChanges();

                                var meterDetail = _db.MeterDetail.Where(x => x.MeterNumber == item.MeterNo).FirstOrDefault();

                                if (meterDetail != null)
                                {
                                    meterDetail.UnitsAtInstallation = item.CurrentReading;
                                    _db.Entry(meterDetail).State = EntityState.Modified;
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

        [HttpDelete]
        [Route("DeleteMeterReading")]
        public IActionResult DeleteMeterReading(int id)
        {
            try
            {
                var model = _db.MeterReading.Find(id);

                if (model != null)
                {
                    model.ModifiedBy = model.ModifiedBy;
                    model.LastModified = DateTime.Now;
                    model.IsActive = false;
                    model.IsDeleted = true;

                    _db.SaveChanges();

                    var readingDetails = _db.ReadingDetail.Where(x => x.MeterReadingId == model.Id).ToList();

                    if (readingDetails?.Count > 0)
                    {
                        foreach (var item in readingDetails)
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
