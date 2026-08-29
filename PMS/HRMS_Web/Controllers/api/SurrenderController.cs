using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using B_Utility.Common.Enums;
using HRMS_Web.Services.AlertService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SurrenderController : ControllerBase
    {
        private readonly DataBase_Context _db;
        private readonly IAlertService alertService;
        CommonBLL _commonBLL;
        ApprovalBLL _approvalBLL;

        public SurrenderController(DataBase_Context db, IAlertService alertService)
        {
            _db = db;
            this.alertService = alertService;
            _commonBLL = new CommonBLL(_db);
            _approvalBLL = new ApprovalBLL(_db);
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult Get(int id)
        {
            try
            {

                var result = _db.Surrender.Where(x => !x.IsDeleted && x.Id == id)
                                           .Include(x => x.StockCreation)
                                           .Include(x => x.Dealer)
                                           .Select(x => new
                                           {
                                               x.Id,
                                               x.StockCreation.RegistrationNo,
                                               x.StockCreation.PropertyNo,
                                               x.StockCreation.MemberProfile.MemberName,
                                               MemberCode = x.StockCreation.MemberProfile.Id,
                                               StockCreationId = x.StockCreationId,
                                               DealerId = x.Dealer.Id,
                                               x.DealerName,
                                               x.EstateName,
                                               x.ExpiryDays,
                                               x.ResurrenderDate,
                                               x.Remarks,
                                               x.Status,
                                               x.StockCreation.ActualSize,
                                               x.StockCreation.PossessionStatus,
                                               x.StockCreation.ConstracutionStatus,
                                               BlockName = _db.Blocks.Where(p => p.ID == (Convert.ToInt32(x.StockCreation.Block))).Select(x => x.Description).FirstOrDefault(),
                                               CategoryName = _db.Categories.Where(p => p.ID == (Convert.ToInt32(x.StockCreation.Category))).Select(x => x.Description).FirstOrDefault()

                                           })
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

        [HttpGet]
        [Route("GetFilterProperty")]
        public IActionResult GetFilterProperty(int id)
        {
            try
            {

                var result = _db.StockCreations.Where(x => x.ID == id && x.MemberProfileId != null)
                                           .Include(x => x.MemberProfile)
                                           .Include(x => x.Dealer)
                                           .Select(x => new
                                           {
                                               x.ID,
                                               x.RegistrationNo,
                                               x.PropertyNo,
                                               MemberName = x.MemberProfile.MemberName ?? "N/A",
                                               MemberCode = x.MemberProfile.Id,
                                               EstateName = x.Dealer.EstateName ?? "N/A",
                                               x.Status,
                                               x.ActualSize,
                                               x.PossessionStatus,
                                               x.ConstracutionStatus,
                                               BlockName = _db.Blocks.Where(p => p.ID == (Convert.ToInt32(x.Block))).Select(x => x.Description).FirstOrDefault(),
                                               CategoryName = _db.Categories.Where(p => p.ID == (Convert.ToInt32(x.Category))).Select(x => x.Description).FirstOrDefault(),
                                               BookingDate = _db.Booking.Where(p => p.StockCreationId == (Convert.ToInt32(x.ID))).Select(x => x.CreatedOn).FirstOrDefault()
                                           })
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

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {

                var result = _db.Surrender.Where(x => !x.IsDeleted)
                                          .Include(x => x.StockCreation)
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
        [HttpPost]
        [Route("AddNewSurrender")]
        public IActionResult AddNewSurrender([FromBody] Surrender model)
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

                model.CreatedOn = model.CreatedOn;
                model.CreatedBy = model.CreatedBy;
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;
                model.IsActive = true;
                model.IsDeleted = false;

                _db.Surrender.Add(model);
                _db.SaveChanges();

                SurrenderHistery dto = new SurrenderHistery();

                dto.StockCreationId = model.StockCreationId;
                dto.DealerId = model.DealerId;
                dto.ResurrenderDate = model.ResurrenderDate;
                dto.Status = model.Status;
                dto.ExpiryDays = model.ExpiryDays;
                dto.EstateName = model.EstateName;
                dto.DealerName = model.DealerName;
                dto.CreatedOn = DateTime.Now;
                dto.CreatedBy = model.CreatedBy;
                dto.ModifiedBy = model.ModifiedBy;
                dto.LastModifiedUserName = model.LastModifiedUserName;
                dto.IsActive = true;
                dto.IsDeleted = false;

                _db.SurrenderHistery.Add(dto);
                _db.SaveChanges();

                var request = _db.NDCRequestForMember.Where(x => x.StockCreationId == model.StockCreationId).ToList();
                if (request.Count() > 0)
                {
                    foreach (var item in request)
                    {
                        item.IsSurrenderRequested = true;
                        _db.SaveChanges();
                    }
                }

                //string message = string.Empty;

                //Surrender surrender = (Surrender)_db.Surrender.Where(x => x.Id == model.Id)
                //                                      .FirstOrDefault();
                //if (surrender != null)
                //{
                //    surrender.IsSurrenderRequest = true;
                //    _db.SaveChanges();

                //    if (isApprovalActive == true)
                //    {
                //        bool result = _approvalBLL.AddNewApprovalSetup(model.Id, (int)ApprovalUIIds.Transfer);
                //        message = "Surrender added succesfully and moved for approval";
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
                //        surrender.IsSurrenderApproved = true;
                //        _db.SaveChanges();

                //        message = "Surrender added succesfully";

                //        return Ok(new ApiResponse<object>
                //        {
                //            Code = ResponseCode.Success,
                //            Message = message,
                //            Data = null
                //        });
                //    }
                //}


                var property = _db.StockCreations.Where(x => x.ID == model.StockCreationId).FirstOrDefault();
                string narration = $"The Property No. {property.PropertyNo} is Surrendered to Estate Name: {model.EstateName} and Dealer Name: {model.DealerName}";
                alertService.PushAlert(2, narration);


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

        [HttpPut]
        [Route("UpdateSurrender")]
        public IActionResult UpdateSurrender([FromBody] Surrender model)
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

                var data = _db.Surrender.Find(model.Id);

                data.DealerId = model.DealerId;
                data.StockCreationId = model.StockCreationId;
                data.ResurrenderDate = model.ResurrenderDate;
                data.ExpiryDays = model.ExpiryDays;
                data.Remarks = model.Remarks;
                data.EstateName = model.EstateName;
                data.DealerName = model.DealerName;
                data.LastModified = DateTime.Now;
                data.ModifiedBy = model.ModifiedBy;
                data.LastModifiedUserName = model.LastModifiedUserName;

                _db.SaveChanges();

                SurrenderHistery dto = new SurrenderHistery();

                dto.StockCreationId = model.StockCreationId;
                dto.DealerId = model.DealerId;
                dto.ResurrenderDate = model.ResurrenderDate;
                dto.Status = model.Status;
                dto.ExpiryDays = model.ExpiryDays;
                dto.EstateName = model.EstateName;
                dto.DealerName = model.DealerName;
                dto.CreatedOn = DateTime.Now;
                dto.ModifiedBy = model.ModifiedBy;
                dto.LastModifiedUserName = model.LastModifiedUserName;
                dto.IsActive = true;
                dto.IsDeleted = false;

                _db.SurrenderHistery.Add(dto);
                _db.SaveChanges();


                var property = _db.StockCreations.Where(x => x.ID == model.StockCreationId).FirstOrDefault();
                string narration = $"The Property No. {property.PropertyNo} is Re-Surrendered to Estate Name: {model.EstateName} and Dealer Name: {model.DealerName}";
                alertService.PushAlert(3, narration);


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

        [HttpDelete]
        [Route("DeleteSurrender")]
        public IActionResult DeleteSurrender(int id)
        {
            try
            {
                var data = _db.Surrender.Find(id);
                data.LastModified = DateTime.Now;
                data.IsDeleted = true;
                data.IsActive = false;

                _db.SaveChanges();

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
    }
}
