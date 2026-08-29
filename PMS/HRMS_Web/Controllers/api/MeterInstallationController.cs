using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common.Enums;
using B_Utility.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Authorization;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MeterInstallationController : ControllerBase
    {
        private readonly DataBase_Context _db;

        CommonBLL _commonBLL;
        ApprovalBLL _approvalBLL;
        public MeterInstallationController(DataBase_Context db)
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
                var result = _db.MeterInstallation.Where(x => !x.IsDeleted)
                                                       .Include(x => x.MeterDetail.Where(x => !x.IsDeleted))
                                                       .ThenInclude(x => x.MeterType)
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
                var result = _db.MeterInstallation.Where(x => !x.IsDeleted && x.Id == id)
                                                       .Include(x => x.MeterDetail.Where(x => !x.IsDeleted))
                                                       .ThenInclude(x => x.MeterType)
                                                       .Include(x => x.MeterDetail.Where(x => !x.IsDeleted))
                                                       .ThenInclude(x => x.MeterPhase)
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
        [Route("AddNewMeterInstallation")]
        public IActionResult AddNewMeterInstallation(MeterInstallation model)
        {
            try
            {
                var isExist = _db.MeterInstallation.Where(x => x.StockCreationId == model.StockCreationId).FirstOrDefault();
                if(isExist != null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "Detail Against Property Already Exist You Can Update only",
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

                if (model.MeterDetail?.Count > 0)
                {
                    foreach (var item in model.MeterDetail)
                    {
                        bool existing = _db.MeterDetail.Any(x => x.MeterNumber == item.MeterNumber && x.Status == "Active");

                        if(existing)
                        {
                            return Ok(new ApiResponse<object>
                            {
                                Code = ResponseCode.Conflict,
                                Message = item.MeterNumber + "Meter Number already active on another property",
                                Data = item.MeterNumber
                            });
                        }
                        else
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
                }
               

                _db.MeterInstallation.Add(model);
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
        [Route("UpdateMeterInstallation")]
        public IActionResult UpdateMeterInstallation(MeterInstallation model)
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

                var data = _db.MeterInstallation.Find(model.Id);

                if (data != null)
                {
                    data.Remarks = model.Remarks;
                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;
                    data.LastModified = DateTime.Now;

                    _db.Entry(data).State = EntityState.Modified;
                    

                    if (model.MeterDetail?.Count > 0)
                    {
                        foreach (var item in model.MeterDetail)
                        {
                            bool existing = _db.MeterDetail.Any(x => x.MeterNumber == item.MeterNumber &&
                                                                     x.Status == "Active" &&
                                                                     x.Id != item.Id);

                            if (existing)
                            {
                                return Ok(new ApiResponse<object>
                                {
                                    Code = ResponseCode.Conflict,
                                    Message = item.MeterNumber + "  Meter Number already active on another property",
                                    Data = item.MeterNumber
                                });
                            }
                        }
                    }

                    if (model.MeterDetail?.Count > 0)
                    {
                        var result = _db.MeterDetail.Where(x => x.MeterInstallationId == model.Id).ToList();

                        _db.MeterDetail.RemoveRange(result);
                        
                    }

                    if (model.MeterDetail?.Count > 0)
                    {
                        foreach (var item in model.MeterDetail)
                        {
                            item.Id = 0;
                            item.MeterInstallationId = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.CreatedOn = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.MeterDetail.AddRange(model.MeterDetail);
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
        [Route("DeleteMeterInstallation")]
        public IActionResult DeleteMeterInstallation(int id)
        {
            try
            {
                var model = _db.MeterInstallation.Find(id);

                if (model != null)
                {
                    model.ModifiedBy = model.ModifiedBy;
                    model.LastModified = DateTime.Now;
                    model.IsActive = false;
                    model.IsDeleted = true;

                    _db.SaveChanges();

                    var meterDetails = _db.MeterDetail.Where(x => x.MeterInstallationId == model.Id).ToList();

                    if (meterDetails?.Count > 0)
                    {
                        foreach (var item in meterDetails)
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
