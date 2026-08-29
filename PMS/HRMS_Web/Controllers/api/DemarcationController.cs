using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using B_Utility.Common.Enums;
using HRMS_Web.Extensions;
using HRMS_Web.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]/Action")]
    [ApiController]
    [Authorize]
    public class DemarcationController : ControllerBase
    {
        private readonly DataBase_Context _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ApprovalBLL _approvalBLL;
        public DemarcationController(DataBase_Context db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _approvalBLL = new ApprovalBLL(_db);
        }

        [HttpGet]
        [Route("/api/Demarcation/GetAllDemarcationFormFilterList")]
        public IActionResult GetAllDemarcationFormFilterList()
        {
            try
            {

                var result = (from stock in _db.StockCreations
                                                     .Include(x => x.MemberProfile)
                                                     .Where(x => x.is_active == true
                                                      && x.MemberProfileId != null
                                                      && x.Is_MapApprovalApproved == true
                                                      && x.Is_DemarcationFormRequested != true
                                                      )
                              join ma in _db.MapApprovalHistery.Where(x => x.Is_Checked == true).OrderByDescending(x => x.Id)

                              on stock.ID equals ma.StockCreationID
                              select new DemarcationFormFilterDTO
                              {
                                  ID = stock.ID,
                                  RegistrationNo = stock.RegistrationNo,
                                  PropertyNo = stock.PropertyNo,
                                  MemberCode = stock.MemberProfile.Id,
                                  MemberName = stock.MemberProfile.MemberName,
                                  CNIC = stock.MemberProfile.Cnic,
                                  Date = ma.DateofFeedback.Value.ToString("dd-MM-yyyy"),
                                  ClientDemarmationDate = stock.DemarcationFileSubmitedDate,
                                  GracePeriodTime =
                                                   string.IsNullOrWhiteSpace(
                                                       _db.Categories
                                                       .Where(x => x.RealStateTypeId == Convert.ToInt32(stock.RealStateType))
                                                       .Select(x => x.ConstructionGracePeriod)
                                                       .FirstOrDefault()
                                                   )
                                                   ? "0"
                                                   : _db.Categories
                                                       .Where(x => x.RealStateTypeId == Convert.ToInt32(stock.RealStateType))
                                                       .Select(x => x.ConstructionGracePeriod)
                                                       .FirstOrDefault()

                              })
                              .ToList() ?? new List<DemarcationFormFilterDTO>();

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
        [Route("/api/Demarcation/GetAllDemarcationFormFilterListFindMode")]
        public IActionResult GetAllDemarcationFormFilterListFindMode()
        {
            try
            {

                var result = (from stock in _db.StockCreations
                                                     .Include(x => x.MemberProfile)
                                                     .Where(x => x.is_active == true
                                                      && x.MemberProfileId != null
                                                      && x.Is_MapApprovalApproved == true
                                                      && x.Is_DemarcationFormRequested == true
                                                      && x.Is_DemarcationFormApproved == true
                                                      && x.DemarcationFileSubmitedDate != null
                                                      )
                              join ma in _db.MapApprovalHistery.Where(x => x.Is_Checked == true).OrderByDescending(x => x.Id)

                              on stock.ID equals ma.StockCreationID
                              select new DemarcationFormFilterDTO
                              {
                                  ID = stock.ID,
                                  RegistrationNo = stock.RegistrationNo,
                                  PropertyNo = stock.PropertyNo,
                                  MemberCode = stock.MemberProfile.Id,
                                  MemberName = stock.MemberProfile.MemberName,
                                  CNIC = stock.MemberProfile.Cnic,
                                  Date = ma.DateofFeedback.Value.ToString("dd-MM-yyyy"),
                                  ClientDemarmationDate = stock.DemarcationFileSubmitedDate,
                                  GracePeriodTime = _db.Categories.Where(x => x.RealStateTypeId == Convert.ToInt32(stock.RealStateType))
                                                                  .Select(x => x.ConstructionGracePeriod)
                                                                  .FirstOrDefault()
                              })
                              .ToList() ?? new List<DemarcationFormFilterDTO>();

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
                var result = _db.Demarcation.Where(x => !x.IsDeleted)
                                                       .Include(x => x.DemarcationFormAttachments)
                                                       .Include(x => x.StockCreation)
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
                var result = _db.Demarcation.Where(x => !x.IsDeleted && x.Id == id)
                                                                      .Include(x => x.DemarcationFormAttachments)
                                                                      .Include(x => x.StockCreation)
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
        [Route("/api/Demarcation/GetByStocId")]
        public IActionResult GetByStocId(int id)
        {
            try
            {
                var result = _db.Demarcation.Where(x => !x.IsDeleted && x.StockCreationId == id)
                                                                      .Include(x => x.DemarcationFormAttachments)
                                                                      .Include(x => x.StockCreation)
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
        [Route("/api/Demarcation/GetId")]
        public IActionResult GetId(int id)
        {
            try
            {
                DateTime DateofFeedback = (DateTime)_db.MapApprovalHistery.Where(x => x.Is_Checked == true && x.StockCreationID == id).OrderByDescending(x => x.Id).FirstOrDefault().DateofFeedback;
                int ids = _db.Demarcation.Where(x => x.StockCreationId == id).FirstOrDefault().Id;
                var result = _db.Demarcation.Where(x => !x.IsDeleted && x.Id == ids)
                                                                      .Include(x => x.StockCreation)
                                                                      .Select(x => new DemarcationFormFilterDTO
                                                                      {
                                                                          ID = x.StockCreation.ID,
                                                                          RegistrationNo = x.StockCreation.RegistrationNo,
                                                                          PropertyNo = x.StockCreation.PropertyNo,
                                                                          MemberCode = x.StockCreation.MemberProfile.Id,
                                                                          MemberName = x.StockCreation.MemberProfile.MemberName,
                                                                          CNIC = x.StockCreation.MemberProfile.Cnic,
                                                                          Date = DateofFeedback.ToString("dd-MM-yyyy"),
                                                                          CreatedOn = x.CreatedOn,
                                                                          ClientDemarmationDate = x.StockCreation.DemarcationFileSubmitedDate,
                                                                          GracePeriodTime = _db.Categories.Where(y => y.RealStateTypeId == Convert.ToInt32(x.StockCreation.RealStateType))
                                                                                                          .Select(y => y.ConstructionGracePeriod)
                                                                                                          .FirstOrDefault()
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
        [Route("/api/Demarcation/UpdateDemarcationDate")]
        public IActionResult UpdateDemarcationDate(DateTime date, int stockId)
        {
            try
            {
                var result = _db.StockCreations
                                .Where(x => !x.is_deleted && x.ID == stockId)
                                .FirstOrDefault();

                result.DemarcationFileSubmitedDate = date;
                result.GrancePeriodForBillGenration = date.AddMonths(12);
                result.DemarcationExpireOn = date.AddMonths(12);

                _db.SaveChanges();

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
        [Route("/api/Demarcation/AddNewDemarcation")]
        public async Task<IActionResult> AddNewDemarcationAsync(Demarcation model)
        {
            try
            {
                bool isApprovalActive = true;

                var approvalStatus = _db.ApprovalUI.Find((int)ApprovalUIIds.DemarcationForm);
                if (approvalStatus != null)
                {
                    if (approvalStatus.Checked != true)
                    {
                        isApprovalActive = false;
                    }
                }

                var approvalSetup = _db.ApprovalSetup.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.DemarcationForm).ToList();
                if (approvalSetup.Count <= 0 && isApprovalActive == true)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Approval setup not defined or In-active",
                        Data = null
                    });
                }

                model.CreatedOn = DateTime.Now;
                model.CreatedBy = model.CreatedBy;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;
                model.IsActive = true;
                model.IsDeleted = false;

                foreach (var item in model.DemarcationFormAttachments)
                {
                    var path = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";
                    item.Piture = string.IsNullOrEmpty(item.Piture) ? "" : $"{path}{await item.Piture.SaveBase64FileAsync()}";
                }

                _db.Demarcation.Add(model);
                _db.SaveChanges();

                string message = string.Empty;

                StockCreation stockCreation = (StockCreation)_db.StockCreations.Where(x => x.ID == model.StockCreationId)
                                                                               .FirstOrDefault();
                if (stockCreation != null)
                {
                    stockCreation.DemarcationFileSubmitedDate = DateTime.Now;
                    stockCreation.Is_DemarcationFormRequested = true;
                    _db.SaveChanges();

                    if (isApprovalActive == true)
                    {
                        bool result = _approvalBLL.AddNewApprovalSetup(stockCreation.ID, (int)ApprovalUIIds.DemarcationForm);
                        message = "Demarcation Form added succesfully and moved for approval";
                        if (result)
                        {
                            return Ok(new ApiResponse<object>
                            {
                                Code = ResponseCode.Success,
                                Message = message,
                            });
                        }
                    }
                    else
                    {
                        stockCreation.GrancePeriodForBillGenration = DateTime.Now.AddMonths((int)model.GraceMonth);
                        stockCreation.Is_DemarcationFormApproved = true;
                        _db.SaveChanges();

                        message = "Demarcation Form added succesfully";

                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = message,
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

        [HttpPut]
        [Route("UpdateDemarcation")]
        public async Task<IActionResult> UpdateDemarcationAsync(Demarcation model)
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

                var data = _db.Demarcation.Find(model.Id);

                if (data != null)
                {
                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;
                    data.IsActive = true;
                    data.IsDeleted = false;
                    data.LastModified = DateTime.Now;

                    _db.Entry(data).State = EntityState.Modified;

                    var attachmentresult = _db.DemarcationFormAttachments.Where(x => x.DemarcationId == model.Id).ToList();

                    foreach (var attachment in attachmentresult)
                    {
                        var existingFilePath = attachment.Piture;

                        bool fileExistsInNewModel = model.DemarcationFormAttachments.Any(x => x.Piture == existingFilePath);

                        if (!fileExistsInNewModel && !string.IsNullOrEmpty(existingFilePath))
                        {
                            existingFilePath.DeleteFile();
                        }

                        _db.DemarcationFormAttachments.Remove(attachment);
                    }

                    if (model.DemarcationFormAttachments?.Count() > 0)
                    {
                        var path = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";

                        foreach (var item in model.DemarcationFormAttachments)
                        {
                            if (!string.IsNullOrEmpty(item.Piture) && !item.Piture.StartsWith("http"))
                            {
                                item.Piture = $"{path}{await item.Piture.SaveBase64FileAsync()}";
                            }
                        }

                        _db.DemarcationFormAttachments.AddRange(model.DemarcationFormAttachments);
                    }

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
        [Route("DeleteDemarcation")]
        public IActionResult DeleteDemarcation(int id)
        {
            try
            {
                var model = _db.Demarcation.Find(id);

                if (model != null)
                {
                    model.LastModified = DateTime.Now;
                    model.IsActive = false;
                    model.IsDeleted = true;

                    _db.SaveChanges();

                    var demarcationFormAttachments = _db.DemarcationFormAttachments.Where(x => x.DemarcationId == model.Id).ToList();

                    if (demarcationFormAttachments.Count > 0)
                    {
                        _db.DemarcationFormAttachments.RemoveRange(demarcationFormAttachments);
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
