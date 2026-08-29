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
using System.Net.Http.Headers;
using static iTextSharp.text.pdf.AcroFields;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly DataBase_Context _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ApprovalBLL _approvalBLL;
        CommonBLL _commonBLL;
        public BookingController(DataBase_Context db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _approvalBLL = new ApprovalBLL(_db);
            _commonBLL = new CommonBLL(_db);
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _db.Booking.Where(x => !x.IsDeleted)
                                        .Select(x => new
                                        {
                                            x.Id,
                                            x.MemberProfile.MemberName,
                                            x.MemberProfile.MEMBERSHIPNO,
                                            x.MemberProfile.Mobile,
                                            x.MemberProfile.PermanentAddress,
                                            x.StockCreation.RegistrationNo,
                                            x.StockCreation.PropertyNo
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
        [Route("GetAllBookingDealerAvailable")]
        public IActionResult GetAllBookingDealerAvailable()
        {
            try
            {
                var result = _db.Booking.Where(x => !x.IsDeleted && x.DealerId >0)
                                                       .Include(x => x.BookingProcessingCharges.Where(x => !x.IsDeleted))
                                                       .Include(x => x.BookingSchedulePaymentPlanDetail.Where(x => !x.IsDeleted))
                                                       .Include(x => x.BookingJointMember.Where(x => !x.IsDeleted))
                                                       .ThenInclude(x => x.MemberProfile)
                                                       .Include(x => x.BookingNominee.Where(x => !x.IsDeleted))
                                                       .Include(x => x.StockCreation)
                                                       .Include(x => x.MemberProfile)
                                                       .Include(x => x.Dealer)
                                                       .ThenInclude(x=>x.DealerEstateDeatail)
                                                       .AsNoTracking()
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
                var result = _db.Booking.Where(x => !x.IsDeleted && x.Id == id)
                                                       .Include(x => x.BookingProcessingCharges.Where(x => !x.IsDeleted))
                                                       .Include(x => x.BookingSchedulePaymentPlanDetail.Where(x => !x.IsDeleted))
                                                       .Include(x => x.BookingJointMember.Where(x => !x.IsDeleted))
                                                       .ThenInclude(x => x.MemberProfile)
                                                       .Include(x => x.BookingNominee.Where(x => !x.IsDeleted))
                                                       .Include(x => x.StockCreation)
                                                       .Include(x => x.MemberProfile)
                                                       .Include(x => x.Dealer)
                                                       .AsNoTracking()
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


        [HttpGet]
        [Route("PrintCertificate")]
        public IActionResult PrintCertificate(int id)
        {
            try
            {
                var operation = _db.SAPOperations.FirstOrDefault();
                var result = _db.Booking.Where(x => !x.IsDeleted && x.Id == id)
                                                       .Include(x => x.MemberProfile)
                                                       .Include(x => x.StockCreation)
                                                       .AsNoTracking()
                                                       .FirstOrDefault();
                if (result != null)
                {
                    result.AllocationSignatoryDesignation = operation?.AllocationSignatoryDesignation;
                    result.AllocationSignatoryName = operation?.AllocationSignatoryName;
                    result.AllocationSignatoryRank = operation?.AllocationSignatoryRank;
                    result.ImageURL = string.IsNullOrEmpty(result.ImageURL) ? result.MemberProfile.ImageURL : result.ImageURL;
                    result.SalePerson = GetSalePerson((int)result.StockCreationId);
                    result.StockCreation.MemberNames = GetAllMemberNames(Convert.ToInt32(result.Id));
                    result.StockCreation.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(result.StockCreation.RealStateType));
                    result.StockCreation.ProjectName = _commonBLL.GetProjectName(Convert.ToInt32(result.StockCreation.Project));
                    result.StockCreation.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(result.StockCreation.Phase));
                    result.StockCreation.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(result.StockCreation.Category));
                    result.StockCreation.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(result.StockCreation.Block));
                    result.StockCreation.NatureName = _commonBLL.GetNatureName(Convert.ToInt32(result.StockCreation.Nature));
                    result.StockCreation.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(result.StockCreation.Type));
                    result.StockCreation.PrefixProperty = _commonBLL.GetSectoreName(Convert.ToInt32(result.StockCreation.PrefixProperty));
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

        private string GetSalePerson(int stockId)
        {
            return _db.PreSale.Where(x=>x.StockCreationId == stockId).FirstOrDefault().SaleBy ?? "";
        }
        private List<B_DB_Model.MemberName> GetAllMemberNames(int id)
        {
            var MemberLists = new List<B_DB_Model.MemberName>();

            var principalMemberImage = _db.Booking
                .Where(th => th.Id == id)
                .Select(x => new B_DB_Model.MemberName
                {
                    MemeberName = $"{x.MemberProfile.HonorificsName}. {x.MemberProfile.MemberName}",
                    Relationhipwith = x.MemberProfile.RelationshipWith,
                    RelationName = x.MemberProfile.Relationship,
                    Cnic = $"({x.MemberProfile.Cnic})"
                }).FirstOrDefault();

            MemberLists.Add(principalMemberImage);


            var jointMemberCnics = _db.BookingJointMember
                .Where(jm => jm.BookingId == id)
                .Select(jm => jm.CNIC)
                .Distinct()
                .ToList(); // Materialize result first

            var jointMembersImageUrls = _db.MemberProfile
                .Where(mp => jointMemberCnics.Contains(mp.Cnic))
                .ToList() // Materialize so GroupBy happens client-side
                .GroupBy(mp => mp.Cnic)
                .Select(g => g.First())
                .Select(x => new B_DB_Model.MemberName
                {
                    MemeberName = x.MemberName,
                    Relationhipwith = x.RelationshipWith,
                    RelationName = x.Relationship,
                    Cnic = $"({x.Cnic})"
                })
                .ToList();



            MemberLists.AddRange(jointMembersImageUrls);
            return MemberLists;
        }

        [HttpPost]
        [Route("AddNewBooking")]
        public async Task<IActionResult> AddNewBookingAsync(Booking model)
        {
            try
            {
                bool isApprovalActive = true;

                var approvalStatus = _db.ApprovalUI.Find((int)ApprovalUIIds.BookingForm);
                if (approvalStatus != null)
                {
                    if (approvalStatus.Checked != true)
                    {
                        isApprovalActive = false;
                    }
                }

                var approvalSetup = _db.ApprovalSetup.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.BookingForm).ToList();
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

                if (model.BookingProcessingCharges?.Count > 0)
                {
                    foreach (var item in model.BookingProcessingCharges)
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

                if (model.BookingSchedulePaymentPlanDetail?.Count > 0)
                {
                    foreach (var item in model.BookingSchedulePaymentPlanDetail)
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
                if (model.BookingJointMember?.Count > 0)
                {
                    foreach (var item in model.BookingJointMember)
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
                if (model.BookingNominee?.Count > 0)
                {
                    foreach (var item in model.BookingNominee)
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

                var path = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";
                model.ImageURL = string.IsNullOrEmpty(model.ImageURL) ? "" : $"{path}{await model.ImageURL.SaveBase64FileAsync()}";
                
                _db.Booking.Add(model);
                string message = string.Empty;
                StockCreation stockCreation = (StockCreation)_db.StockCreations.Where(x => x.ID == model.StockCreationId)
                                                                               .FirstOrDefault();
                string buyerName = _db.MemberProfile.SingleOrDefault(x => x.Id == model.MemberProfileId).MemberName;

                FileReceivingRegister register = new FileReceivingRegister();
                register.RegisterNo = (int)Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd"));
                register.Registration = stockCreation.RegistrationNo;
                register.Plot = stockCreation.PropertyNo;
                register.Block = _db.Blocks.SingleOrDefault(x=>x.ID == Convert.ToInt32(stockCreation.Block)).Description;
                register.Area = stockCreation.coveredArea == null ? stockCreation.ActualSize : stockCreation.coveredArea.ToString();
                register.SellerName = "UD Asset";
                register.BuyerName = buyerName;
                register.InternalNo = UHelper.GenerateUniqueNumber();
                register.Remarks = "";
                register.CreatedOn = DateTime.Now;

                //_db.FileReceivingRegisters.Add(register);

                _db.SaveChanges();

                if (stockCreation != null)
                {
                    stockCreation.MemberProfileId = model.MemberProfileId;
                    stockCreation.DealerId = model.DealerId;
                    stockCreation.MemberTaxStatus = _db.MemberProfile.SingleOrDefault(x => x.Id == model.MemberProfileId).TaxStatus;
                    stockCreation.IsBookingRequested = true;
                    _db.SaveChanges();

                    if(isApprovalActive == true)
                    { 
                       bool result = _approvalBLL.AddNewApprovalSetup(model.Id, (int)ApprovalUIIds.BookingForm);
                       message = "Booking added succesfully and moved for approval";    
                    }
                    else
                    {
                        stockCreation.IsBookingApproved = true;
                        _db.SaveChanges();
                
                        Response_Result responseForContactPersonAddition = new SapIntegrationController(_db).UpdateMemberProfileToAddContactPerson((int)model.StockCreationId,(int)model.MemberProfileId);
                        if (responseForContactPersonAddition != null)
                        {

                        }
                        if (model.BookingSchedulePaymentPlanDetail.Count > 0)
                        {
                            Response_Result response_ResultBookingSchedule = new SapIntegrationController(_db).AddServiceTypeInvoiceBookingSchedule(model, false);
                        }
                        if (model.BookingProcessingCharges.Count > 0)
                        {
                            Response_Result response_Result = new SapIntegrationController(_db).AddServiceTypeInvoiceProcessingCharges(model, false);

                        }
                        message = "Booking added succesfully";
                    }
                }
    
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = message,
                    Data = null
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        [Route("UpdateBooking")]
        public async Task<IActionResult> UpdateBookingAsync(Booking model)
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

                var data = _db.Booking.Find(model.Id);

                if (data != null)
                {
                    if (model.ImageURL != data.ImageURL)
                    {
                        if (!string.IsNullOrEmpty(data.ImageURL))
                        {
                            data.   ImageURL.DeleteFile();
                        }

                        if (!string.IsNullOrEmpty(model.ImageURL))
                        {
                            var path = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";
                            data.ImageURL = $"{path}{await model.ImageURL.SaveBase64FileAsync()}";
                        }
                        else
                        {
                            data.ImageURL = "";
                        }
                    }
                    data.JointOwners = model.JointOwners;
                    data.Remarks = model.Remarks;
                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;
                    data.LastModified = DateTime.Now;

                    _db.Entry(data).State = EntityState.Modified;
                    _db.SaveChanges();

                    if (model.BookingSchedulePaymentPlanDetail?.Count > 0)
                    {
                        var result = _db.BookingSchedulePaymentPlanDetail.Where(x => x.BookingId == model.Id).ToList();

                        _db.BookingSchedulePaymentPlanDetail.RemoveRange(result);
                        _db.SaveChanges();
                    }

                    if (model.BookingSchedulePaymentPlanDetail?.Count > 0)
                    {
                        foreach (var item in model.BookingSchedulePaymentPlanDetail)
                        {
                            item.BookingId = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.BookingSchedulePaymentPlanDetail.AddRange(model.BookingSchedulePaymentPlanDetail);
                        _db.SaveChanges();
                    }

                    if (model.BookingJointMember?.Count > 0)
                    {
                        var result = _db.BookingJointMember.Where(x => x.BookingId == model.Id).ToList();

                        _db.BookingJointMember.RemoveRange(result);
                        _db.SaveChanges();
                    }

                    if (model.BookingJointMember?.Count > 0)
                    {
                        foreach (var item in model.BookingJointMember)
                        {
                            item.BookingId = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.BookingJointMember.AddRange(model.BookingJointMember);
                        _db.SaveChanges();
                    }

                    if (model.BookingNominee?.Count > 0)
                    {
                        var result = _db.BookingNominee.Where(x => x.BookingId == model.Id).ToList();

                        _db.BookingNominee.RemoveRange(result);
                        _db.SaveChanges();
                    }

                    if (model.BookingNominee?.Count > 0)
                    {
                        foreach (var item in model.BookingNominee)
                        {
                            item.BookingId = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.BookingNominee.AddRange(model.BookingNominee);
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
        [Route("DeleteBooking")]
        public IActionResult DeleteBooking(int id)
        {
            try
            {
                var model = _db.Booking.Find(id);

                if (model != null)
                {
                    model.ModifiedBy = model.ModifiedBy;
                    model.LastModified = DateTime.Now;
                    model.IsActive = false;
                    model.IsDeleted = true;

                    _db.SaveChanges();

                    var bookingProcessingCharges = _db.BookingProcessingCharges.Where(x => x.BookingId == model.Id).ToList();

                    if (bookingProcessingCharges?.Count > 0)
                    {
                        foreach (var item in bookingProcessingCharges)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;
                            _db.SaveChanges();
                        }
                    }

                    var bookingSchedulePaymentPlanDetails = _db.BookingSchedulePaymentPlanDetail.Where(x => x.BookingId == model.Id).ToList();

                    if (bookingSchedulePaymentPlanDetails?.Count > 0)
                    {
                        foreach (var item in bookingSchedulePaymentPlanDetails)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;
                            _db.SaveChanges();
                        }
                    }

                    var bookingJointMembers = _db.BookingJointMember.Where(x => x.BookingId == model.Id).ToList();

                    if (bookingJointMembers?.Count > 0)
                    {
                        foreach (var item in bookingJointMembers)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;
                            _db.SaveChanges();
                        }
                    }

                    var bookingNominees = _db.BookingNominee.Where(x => x.BookingId == model.Id).ToList();

                    if (bookingNominees?.Count > 0)
                    {
                        foreach (var item in bookingNominees)
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
