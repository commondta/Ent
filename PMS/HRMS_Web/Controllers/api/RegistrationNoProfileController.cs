using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using HRMS_Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegistrationNoProfileController : ControllerBase
    {
        private readonly DataBase_Context _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ApprovalBLL _approvalBLL;
        CommonBLL _commonBLL;
        public RegistrationNoProfileController(DataBase_Context db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _approvalBLL = new ApprovalBLL(_db);
            _commonBLL = new CommonBLL(_db);
        }


        [HttpGet]
        [Route("GetFilterListForPropertyBillSetup")]
        public IActionResult GetFilterListForPropertyBillSetup()
        {
            try
            {
                var result = _db.StockCreations.Where(x => !x.is_deleted
                                                      && x.Is_StockCreationApproved == true
                                                      && x.MemberProfileId != null)
                                               .Select(x => new
                                               {
                                                   x.ID,
                                                   x.RegistrationNo,
                                                   x.PropertyNo,
                                                   x.MemberProfileId,
                                                   x.MemberProfile.MemberName,
                                                   x.MemberProfile.Cnic,
                                                   x.Status
                                               })
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
                var result = _db.RegistrationNoProfile.Where(x => !x.IsDeleted)
                                                       .Include(x => x.SoftLock.Where(x => !x.IsDeleted))
                                                       .Include(x => x.Alerts.Where(x => !x.IsDeleted))
                                                       .Include(x => x.RegNoProfileAttachments.Where(x => !x.IsDeleted))
                                                       .Include(x => x.StockCreation)
                                                       .Include(x => x.MemberProfile)
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
                var result = _db.RegistrationNoProfile.Where(x => !x.IsDeleted && x.Id == id)
                                                       .Include(x => x.SoftLock.Where(x => !x.IsDeleted))
                                                       .Include(x => x.Alerts.Where(x => !x.IsDeleted))
                                                       .Include(x => x.RegNoProfileAttachments.Where(x => !x.IsDeleted))
                                                       .Include(x => x.StockCreation)
                                                       .Include(x => x.PropertyStatus)
                                                       .Include(x => x.MemberProfile)
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
                    //result.StockCreation.ConstracutionStatus = _commonBLL.GetConstrcutionStatus(Convert.ToInt32(result.StockCreation.ID));
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
        [Route("AddNewRegistrationNoProfile")]
        public async Task<IActionResult> AddNewRegistrationNoProfileAsync(RegistrationNoProfile model)
        {
            try
            {
                var path = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";


                bool isExist = _db.RegistrationNoProfile.Any(x => x.StockCreationId == model.StockCreationId);
                if (isExist)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Already exist please use find mode to update it",
                        Data = null
                    });
                }
                model.IsActive = true;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = model.CreatedBy;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;

                if (model.SoftLock?.Count > 0)
                {
                    foreach (var item in model.SoftLock)
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

                if (model.Alerts?.Count > 0)
                {
                    foreach (var item in model.Alerts)
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
                if (model.RegNoProfileAttachments?.Count > 0)
                {
                    foreach (var item in model.RegNoProfileAttachments)
                    {
                        item.Attachment = string.IsNullOrEmpty(item.Attachment) ? "" : $"{path}{await item.Attachment.SaveBase64FileAsync()}";
                        item.ModifiedBy = model.ModifiedBy;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                _db.RegistrationNoProfile.Add(model);
                _db.SaveChanges();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Registration Profile added Successfully",
                    Data = null
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPut]
        [Route("UpdateRegistrationNoProfile")]
        public async Task<IActionResult> UpdateRegistrationNoProfile(RegistrationNoProfile model)
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

                var data = _db.RegistrationNoProfile.Find(model.Id);

                if (data != null)
                {
                    data.Remarks = model.Remarks;
                    data.CorrespondenceAddress = model.CorrespondenceAddress;
                    data.CorrespondenceEmail = model.CorrespondenceEmail;
                    data.CorrespondenceMobileNo = model.CorrespondenceMobileNo;
                    data.CorrespondenceWhatsappNo = model.CorrespondenceWhatsappNo;
                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;
                    data.LastModified = DateTime.Now;

                    _db.Entry(data).State = EntityState.Modified;


                    if (model.SoftLock?.Count > 0)
                    {
                        var result = _db.SoftLock.Where(x => x.RegistrationNoProfileId == model.Id).ToList();

                        _db.SoftLock.RemoveRange(result);

                    }

                    if (model.SoftLock?.Count > 0)
                    {
                        foreach (var item in model.SoftLock)
                        {
                            item.RegistrationNoProfileId = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.SoftLock.AddRange(model.SoftLock);

                    }

                    if (model.PropertyStatus?.Count > 0)
                    {
                        var result = _db.PropertyStatus.Where(x => x.RegistrationNoProfileId == model.Id).ToList();

                        _db.PropertyStatus.RemoveRange(result);

                    }

                    if (model.PropertyStatus?.Count > 0)
                    {
                        foreach (var item in model.PropertyStatus)
                        {
                            item.RegistrationNoProfileId = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.PropertyStatus.AddRange(model.PropertyStatus);

                    }

                    if (model.Alerts?.Count > 0)
                    {
                        var result = _db.Alerts.Where(x => x.RegistrationNoProfileId == model.Id).ToList();

                        _db.Alerts.RemoveRange(result);

                    }

                    if (model.Alerts?.Count > 0)
                    {
                        foreach (var item in model.Alerts)
                        {
                            item.RegistrationNoProfileId = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.Alerts.AddRange(model.Alerts);

                    }

                    var attachmentresult = _db.RegNoProfileAttachments.Where(x => x.RegistrationNoProfileId == model.Id).ToList();

                    foreach (var attachment in attachmentresult)
                    {
                        var existingFilePath = attachment.Attachment;

                        bool fileExistsInNewModel = model.RegNoProfileAttachments.Any(x => x.Attachment == existingFilePath);

                        if (!fileExistsInNewModel && !string.IsNullOrEmpty(existingFilePath))
                        {
                            existingFilePath.DeleteFile();
                        }

                        _db.RegNoProfileAttachments.Remove(attachment);
                    }

                    if (model.RegNoProfileAttachments?.Count > 0)
                    {
                        var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";

                        foreach (var item in model.RegNoProfileAttachments)
                        {
                            if (!string.IsNullOrEmpty(item.Attachment))
                            {
                                var savedPath = await item.Attachment.SaveBase64FileAsync();

                                if (!savedPath.StartsWith(baseUrl, StringComparison.OrdinalIgnoreCase))
                                {
                                    item.Attachment = $"{baseUrl}{savedPath}";
                                }
                                else
                                {
                                    item.Attachment = savedPath; 
                                }
                            }
                            else
                            {
                                item.Attachment = "";
                            }

                            item.RegistrationNoProfileId = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.RegNoProfileAttachments.AddRange(model.RegNoProfileAttachments);
                    }


                    StockCreation stock = _db.StockCreations.Find(model.StockCreationId);
                    if (model.PropertyStatus.Count() > 0)
                    {
                        stock.PropertyStatus = model.PropertyStatus.LastOrDefault().Status;
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
        [Route("DeleteRegistrationNoProfile")]
        public IActionResult DeleteRegistrationNoProfile(int id)
        {
            try
            {
                var model = _db.RegistrationNoProfile.Find(id);

                if (model != null)
                {
                    model.ModifiedBy = model.ModifiedBy;
                    model.LastModified = DateTime.Now;
                    model.IsActive = false;
                    model.IsDeleted = true;

                    _db.SaveChanges();

                    var softLocks = _db.SoftLock.Where(x => x.RegistrationNoProfileId == model.Id).ToList();

                    if (softLocks?.Count > 0)
                    {
                        foreach (var item in softLocks)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;
                            _db.SaveChanges();
                        }
                    }

                    var alerts = _db.Alerts.Where(x => x.RegistrationNoProfileId == model.Id).ToList();

                    if (alerts?.Count > 0)
                    {
                        foreach (var item in alerts)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;
                            _db.SaveChanges();
                        }
                    }

                    var regNoProfileAttachments = _db.RegNoProfileAttachments.Where(x => x.RegistrationNoProfileId == model.Id).ToList();

                    if (regNoProfileAttachments?.Count > 0)
                    {
                        foreach (var item in regNoProfileAttachments)
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

        [HttpPost]
        [Route("SaveFixedCharges")]
        public IActionResult SaveFixedCharges(List<PropertyFixedChargesSetup> model)
        {
            try
            {

                if (model.Count > 0)
                {
                    foreach (var item in model)
                    {
                        var idsToRemove = _db.PropertyFixedChargesSetup
                                            .Where(x => x.StockCreationId == item.StockCreationId &&
                                                   x.MatchId != item.MatchId)
                                            .Select(x => x.Id)
                                            .ToList();

                        foreach (var id in idsToRemove)
                        {
                            var itemToRemove = _db.PropertyFixedChargesSetup.Find(id);
                            if (itemToRemove != null)
                            {
                                _db.PropertyFixedChargesSetup.Remove(itemToRemove);
                            }
                        }

                        _db.SaveChanges();
                    }

                }

                if (model.Count > 0)
                {
                    foreach (var item in model)
                    {
                        var result = _db.PropertyFixedChargesSetup.Where(x => x.StockCreationId == item.StockCreationId && x.MatchId == item.MatchId).FirstOrDefault();
                        if (result != null)
                        {
                            result.ChargeSetupRate = item.ChargeSetupRate;
                            result.Rate = item.Rate;
                            result.Discount = item.Discount;
                            result.CreatedBy = item.CreatedBy;
                            result.ModifiedBy = item.ModifiedBy;
                            result.LastModifiedUserName = item.LastModifiedUserName;
                            result.IsEnabled = item.IsEnabled;
                            result.LastModified = DateTime.Now;
                            _db.SaveChanges();
                        }
                        else
                        {
                            item.ModifiedBy = item.ModifiedBy;
                            item.LastModifiedUserName = item.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;

                            _db.PropertyFixedChargesSetup.Add(item);
                            _db.SaveChanges();
                        }
                    }
                }
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Saved",
                    Data = null
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }


        [HttpPost]
        [Route("SaveTanant")]
        public IActionResult SaveTanant(List<TanantDetail> model)
        {
            try
            {

                if (model.Count > 0)
                {
                    var result = _db.TanantDetail.Where(x => x.StockCreationID == model.FirstOrDefault().StockCreationID).ToList();
                    if (result.Count > 0)
                    {
                        _db.TanantDetail.RemoveRange(result);
                        _db.SaveChanges();
                    }
                }

                if (model.Count > 0)
                {
                    foreach (var item in model)
                    {
                        item.ModifiedBy = item.ModifiedBy;
                        item.LastModified = DateTime.Now;
                        item.IsActive = item.IsActive;
                        item.IsDeleted = false;
                    }

                    _db.TanantDetail.AddRange(model);
                    _db.SaveChanges();
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Saved",
                    Data = null
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        [Route("SaveWHTax")]
        public IActionResult SaveWHTax(List<WithHoldingTaxPropertyWise> model)
        {
            try
            {

                if (model.Count > 0)
                {
                    var result = _db.WithHoldingTaxPropertyWise.Where(x => x.StockCreationId == model.FirstOrDefault().StockCreationId).ToList();
                    if (result.Count > 0)
                    {
                        _db.WithHoldingTaxPropertyWise.RemoveRange(result);
                        _db.SaveChanges();
                    }
                }

                if (model.Count > 0)
                {
                    foreach (var item in model)
                    {
                        item.ModifiedBy = item.ModifiedBy;
                        item.LastModified = DateTime.Now;
                        item.IsActive = item.IsActive;
                        item.IsDeleted = false;
                    }

                    _db.WithHoldingTaxPropertyWise.AddRange(model);
                    _db.SaveChanges();
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Saved",
                    Data = null
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        [Route("BillingActivationSetup")]
        public IActionResult BillingActivationSetup(BillActivationSetupDto model)
        {
            try
            {

                StockCreation stockCreation = _db.StockCreations.SingleOrDefault(x => x.ID == model.StockCreationId);

                if (stockCreation != null)
                {
                    stockCreation.GeneratorUnitType = model.GeneratorUnitType;
                    stockCreation.IsBillGenerationEnabled = model.IsBillGenerationEnabled;
                    stockCreation.IsSaleTaxEnabled = model.IsSaleTaxEnabled;
                    stockCreation.IsWithHoldingTaxEnabled = model.IsWithHoldingTaxEnabled;
                    stockCreation.MaintenceAdvanceBillPaid = model.MaintenceAdvanceBillPaid;
                    stockCreation.BillPrintRegistrationNo = model.BillPrintRegistrationNo;
                    stockCreation.BillPrintPropertyNo = model.BillPrintPropertyNo;
                    stockCreation.BillPrintName = model.BillPrintName;
                    stockCreation.BillPrintAddress = model.BillPrintAddress;
                    stockCreation.MemberProfileId = model.MemberProfileId;

                    _db.Entry(stockCreation).State = EntityState.Modified;
                    _db.SaveChanges();
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Saved",
                    Data = null
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        private bool IsBase64String(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            s = s.Trim();
            return (s.Length % 4 == 0) &&
                   Regex.IsMatch(s, @"^[A-Za-z0-9\+/]*={0,2}$");
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ChangeAttachmentToBase64Files")]
        public async Task<IActionResult> ChangeAttachmentToBase64Files()
        {
            try
            {
                const int batchSize = 99; // process 99 IDs at a time
                var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://"
                            + $"{_httpContextAccessor.HttpContext.Request.Host}"
                            + $"{_httpContextAccessor.HttpContext.Request.PathBase}";

                // Find the maximum Id so we know where to stop
                int maxId = await _db.RegNoProfileAttachments.MaxAsync(a => a.Id);

                for (int startId = 1; startId <= maxId; startId += batchSize)
                {
                    int endId = startId + batchSize - 1;

                    var batch = await _db.RegNoProfileAttachments
                        .Where(a => a.Attachment.StartsWith("data:") &&
                                    a.Id >= startId && a.Id <= endId)
                        .OrderBy(a => a.Id)
                        .ToListAsync();

                    if (!batch.Any())
                        continue; // skip empty ranges

                    foreach (var item in batch)
                    {
                        var relativePath = await item.Attachment.SaveBase64FileAsync();
                        item.Attachment = $"{baseUrl}{relativePath}";
                    }

                    await _db.SaveChangesAsync();
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Base64 attachments saved as files in ID ranges of 999.",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

    }
}
