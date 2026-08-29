using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using B_Utility.Common.Enums;
using HRMS_Web.Services.BusinessServicesInterFace;
using HRMS_Web.Services.PhotoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DealerController : ControllerBase
    {
        private readonly DataBase_Context _db;
        private readonly IPhotoService photoService;
        ApprovalBLL _approvalBLL;

        public DealerController(DataBase_Context db, IPhotoService photoService)
        {
            _db = db;
            this.photoService = photoService;
            _approvalBLL = new ApprovalBLL(_db);
        }

        [HttpGet]
        [Route("/api/Dealer/GetFilterListForPreSale")]
        public IActionResult GetFilterListForPreSale()
        {
            try
            {
                var result = _db.Dealers.Where(x => !x.IsDeleted
                                                && x.IsDealerProfileApproved == true
                                                )
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
        [Route("/api/Dealer/GetAllCities")]
        public IActionResult GetAllCities()
        {
            try
            {
                var result = _db.Dealers
                                        .Select(x => new
                                        {
                                            Name = x.City
                                        })
                                        .ToList()
                                        .DistinctBy(x => x.Name);

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
        [Route("/api/Dealer/GetFilterListForNDC")]
        public IActionResult GetFilterListForNDC()
        {
            try
            {
                var result = _db.Dealers.Where(x => !x.IsDeleted
                                                && x.IsDealerProfileApproved == true
                                                )
                                        .Select(x=> new
                                        {
                                            x.Id,
                                            x.PrincipalOwner,
                                            x.EstateName,
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
        [Route("/api/Dealer/GetFilterListForApprovalHistory")]
        public IActionResult GetFilterListForApprovalHistory()
        {
            try
            {
                var result = _db.Dealers.Where(x => !x.IsDeleted
                                                )
                                        .Select(x => new
                                        {
                                            x.Id,
                                            x.PrincipalOwner,
                                            x.EstateName,
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
        [Route("GetDealerNDC")]
        public IActionResult GetDealerNDC(int id)
        {
            try
            {
                var result = _db.Dealers.Where(x => !x.IsDeleted && x.Id == id)
                                        .Select(x => new
                                        {
                                            x.Id,
                                            x.EstateName
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
        [Route("GetAllEstateMembers")]
        public IActionResult GetAllEstateMembers(int id)
        {
            try
            {
                var result = _db.dealerEstateDeatails.Where(x => !x.IsDeleted && x.DealerId == id)
                                                     .Include(x=>x.DealerDesignation)
                                        .Select(x => new
                                        {
                                            x.Id,
                                            x.Name,
                                            x.DealerDesignation.Description,
                                            x.IsPrimary
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
        [Route("Get")]
        public IActionResult Get(int id)
        {
            try
            {
                var result = _db.Dealers.Where(x => !x.IsDeleted && x.Id == id)
                                   .Include(x => x.DealerEstateDeatail.Where(x => !x.IsDeleted))
                                   .Include(x => x.DealerAttachments.Where(x => !x.IsDeleted))
                                   .Include(x => x.DealerRelationshipHistory.Where(x => !x.IsDeleted))
                                   .Include(x => x.DealerWitness.Where(x => !x.IsDeleted))
                                   .Include(x => x.DealerCategory)
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
                var result = _db.Dealers.Include(x => x.DealerEstateDeatail.Where(x => !x.IsDeleted))
                                   .Include(x => x.DealerAttachments.Where(x => !x.IsDeleted))
                                   .Include(x => x.DealerCategory)
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
        [Route("/api/Dealer/AddDealer")]
        public IActionResult AddDealer(Dealer dto)
        {
            try
            {
                bool isApprovalActive = true;

                var approvalStatus = _db.ApprovalUI.Find((int)ApprovalUIIds.DealerRegistrationForm);
                if (approvalStatus != null)
                {
                    if (approvalStatus.Checked != true)
                    {
                        isApprovalActive = false;
                    }
                }

                var approvalSetup = _db.ApprovalSetup.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.DealerRegistrationForm).ToList();
                if (approvalSetup.Count <= 0 && isApprovalActive == true)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Approval setup not defined or In-active",
                        Data = null
                    });
                }

                if (_db.Dealers.Any(x => x.DelaerRegisrationCode == dto.DelaerRegisrationCode))
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Conflict,
                        Message = "Dealer with Regisration Code Already Exist",
                        Data = null
                    });
                }

                dto.CreatedBy = dto.CreatedBy;
                dto.ModifiedBy = dto.ModifiedBy;
                dto.LastModifiedUserName = dto.LastModifiedUserName;
                dto.CreatedOn = dto.CreatedOn;
                dto.IsDeleted = false;
                dto.IsActive = true;

                if (dto.DealerEstateDeatail?.Count > 0)
                {
                    foreach (var item in dto.DealerEstateDeatail)
                    {
                        item.ModifiedBy = dto.ModifiedBy;
                        item.CreatedBy = dto.CreatedBy;
                        item.LastModifiedUserName = dto.LastModifiedUserName;
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                if (dto.DealerRelationshipHistory?.Count > 0)
                {
                    foreach (var item in dto.DealerRelationshipHistory)
                    {
                        item.ModifiedBy = dto.ModifiedBy;
                        item.CreatedBy = dto.CreatedBy;
                        item.LastModifiedUserName = dto.LastModifiedUserName;
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                if (dto.DealerAttachments?.Count > 0)
                {
                    foreach (var item in dto.DealerAttachments)
                    {
                        item.ModifiedBy = dto.ModifiedBy;
                        item.CreatedBy = dto.CreatedBy;
                        item.LastModifiedUserName = dto.LastModifiedUserName;
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }
                if (dto.DealerWitness?.Count > 0)
                {
                    foreach (var item in dto.DealerWitness)
                    {
                        item.ModifiedBy = dto.ModifiedBy;
                        item.CreatedBy = dto.CreatedBy;
                        item.LastModifiedUserName = dto.LastModifiedUserName;
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                if(!string.IsNullOrEmpty(dto.PictureBase64))
                { 
                   var uploadResult = photoService.UploadPhotoAsync(dto.PictureBase64)
                                                  .GetAwaiter()
                                                  .GetResult();
                   dto.PictureBase64 = uploadResult.SecureUrl.AbsoluteUri;
                }

                _db.Dealers.Add(dto);
                _db.SaveChanges();

                string message = string.Empty;

                Dealer memberProfile = (Dealer)_db.Dealers.Where(x => x.Id == dto.Id).FirstOrDefault();
                if (memberProfile != null)
                {
                    memberProfile.IsDealerProfileRequested = true;
                    _db.SaveChanges();

                    if (isApprovalActive == true)
                    {
                        bool result = _approvalBLL.AddNewApprovalSetup(dto.Id, (int)ApprovalUIIds.DealerRegistrationForm);
                        message = "Dealer Profile added succesfully and moved for approval";
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
                        memberProfile.IsDealerProfileApproved = true;
                        _db.SaveChanges();

                        message = "Dealer Profile added succesfully";

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

        [HttpPut]
        [Route("UpdateDealer")]
        public IActionResult UpdateDealer(Dealer dto)
        {
            try
            {
                var existing = _db.Dealers.Where(x => x.DelaerRegisrationCode == dto.DelaerRegisrationCode && x.Id != dto.Id).FirstOrDefault();

                if (existing != null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Conflict,
                        Message = "Dealer with Regisration Code Already Exist",
                        Data = null
                    });
                }
                var model = _db.Dealers.Find(dto.Id);
                if (model != null)
                {
                    if(!string.IsNullOrEmpty(dto.PictureBase64))
                    {
                        bool isUrl = UHelper.IsUrl(dto.PictureBase64);
                        if (isUrl) { model.PictureBase64 = dto.PictureBase64; }
                        else
                        {
                            string publicId = UHelper.ExtractIdentifierFromUrl(model.PictureBase64);
                            var deleteResult = photoService.DeletePhotoAsync(publicId)
                                                           .GetAwaiter()
                                                           .GetResult();

                            var uploadResult = photoService.UploadPhotoAsync(dto.PictureBase64)
                                                           .GetAwaiter()
                                                           .GetResult();
                            model.PictureBase64 = uploadResult.SecureUrl.AbsoluteUri;
                        }
                    }
                    else { model.PictureBase64 = dto.PictureBase64; }
                    model.DealerStatus = dto.DealerStatus;
                    model.PrincipalOwner = dto.PrincipalOwner;
                    model.RegistrationFee = dto.RegistrationFee;
                    model.EstateAddress = dto.EstateAddress;
                    model.CNIC = dto.CNIC;
                    model.ResidentialAddress = dto.ResidentialAddress;
                    model.EstateName = dto.EstateName;
                    model.Email = dto.Email;
                    model.ContactNo = dto.ContactNo;
                    model.RenewalDate = dto.RenewalDate;
                    model.Nationality = dto.Nationality;
                    model.Country = dto.Country;
                    model.City = dto.City;
                    model.OutstandingBalance = dto.OutstandingBalance;
                    model.OutstandingAdvance = dto.OutstandingAdvance;
                    model.DelaerRegisrationCode = dto.DelaerRegisrationCode;
                    model.UserName = dto.UserName;
                    model.Password = dto.Password;
                    model.ModifiedBy = dto.ModifiedBy;
                    model.LastModifiedUserName = dto.LastModifiedUserName;
                    model.LastModified = DateTime.Now;
                    model.CreatedOn = dto.CreatedOn;
                    _db.Entry(model).State = EntityState.Modified;

                    _db.SaveChanges();

                    if (dto.DealerEstateDeatail?.Count >= 0)
                    {
                        var estateDetailList = _db.dealerEstateDeatails.Where(x => x.DealerId == model.Id).ToList();

                        _db.dealerEstateDeatails.RemoveRange(estateDetailList);
                        _db.SaveChanges();
                    }
                    if (dto.DealerEstateDeatail?.Count > 0)
                    {
                        foreach (var dealer in dto.DealerEstateDeatail)
                        {
                            dealer.DealerId = model.Id;
                            dealer.ModifiedBy = dto.ModifiedBy;
                            dealer.LastModifiedUserName = dto.LastModifiedUserName;
                            dealer.CreatedOn = DateTime.Now;
                            dealer.IsDeleted = false;
                            dealer.IsActive = true;
                        }

                        _db.dealerEstateDeatails.AddRange(dto.DealerEstateDeatail);
                        _db.SaveChanges();
                    }

                    if (dto.DealerRelationshipHistory?.Count >= 0)
                    {
                        var dealerRelationshipHistories = _db.DealerRelationshipHistery.Where(x => x.DealerId == model.Id).ToList();

                        _db.DealerRelationshipHistery.RemoveRange(dealerRelationshipHistories);
                        _db.SaveChanges();
                    }
                    if (dto.DealerRelationshipHistory?.Count > 0)
                    {
                        foreach (var item in dto.DealerRelationshipHistory)
                        {
                            item.DealerId = model.Id;
                            item.ModifiedBy = dto.ModifiedBy;
                            item.LastModifiedUserName = dto.LastModifiedUserName;
                            item.CreatedOn = DateTime.Now;
                            item.IsDeleted = false;
                            item.IsActive = true;
                        }

                        _db.DealerRelationshipHistery.AddRange(dto.DealerRelationshipHistory);
                        _db.SaveChanges();

                    }

                    if (dto.DealerAttachments?.Count >= 0)
                    {
                        var attachmentsList = _db.DealerAttachments.Where(x => x.DealerId == model.Id).ToList();
                        _db.DealerAttachments.RemoveRange(attachmentsList);
                        _db.SaveChanges();
                    }
                    if (dto.DealerAttachments?.Count > 0)
                    {
                        foreach (var attachment in dto.DealerAttachments)
                        {
                            attachment.DealerId = model.Id;
                            attachment.ModifiedBy = dto.ModifiedBy;
                            attachment.LastModifiedUserName = dto.LastModifiedUserName;
                            attachment.CreatedOn = DateTime.Now;
                            attachment.IsDeleted = false;
                            attachment.IsActive = true;
                        }

                        _db.DealerAttachments.AddRange(dto.DealerAttachments);
                        _db.SaveChanges();
                    }
                    if (dto.DealerWitness?.Count >= 0)
                    {
                        var attachmentsList = _db.DealerWitness.Where(x => x.DealerId == model.Id).ToList();
                        _db.DealerWitness.RemoveRange(attachmentsList);
                        _db.SaveChanges();
                    }
                    if (dto.DealerWitness?.Count > 0)
                    {
                        foreach (var attachment in dto.DealerWitness)
                        {
                            attachment.DealerId = model.Id;
                            attachment.ModifiedBy = dto.ModifiedBy;
                            attachment.LastModifiedUserName = dto.LastModifiedUserName;
                            attachment.CreatedOn = DateTime.Now;
                            attachment.IsDeleted = false;
                            attachment.IsActive = true;
                        }

                        _db.DealerWitness.AddRange(dto.DealerWitness);
                        _db.SaveChanges();
                    }

                    bool result = _approvalBLL.UpdateRequestApprovalSetup(dto.Id, (int)ApprovalUIIds.DealerRegistrationForm);

                    if (result)
                    {
                        model.IsDealerProfileRequested = true;
                        model.IsDealerProfileApproved = false;

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
                    Data = dto
                });

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpDelete]
        [Route("DeleteDealer")]
        public IActionResult DeleteDealer(int id)
        {
            try
            {
                var model = _db.Dealers.Find(id);

                if (model != null)
                {
                    model.IsActive = false;
                    model.LastModified = DateTime.Now;
                    model.IsDeleted = true;

                    _db.SaveChanges();

                    var estateDetailList = _db.dealerEstateDeatails.Where(x => x.DealerId == model.Id).ToList();

                    if (estateDetailList?.Count > 0)
                    {
                        foreach (var item in estateDetailList)
                        {
                            item.ModifiedBy = 1;
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;

                            _db.SaveChanges();
                        }
                    }

                    var dealerRelationshipHistories = _db.DealerRelationshipHistery.Where(x => x.DealerId == model.Id).ToList();

                    if (dealerRelationshipHistories?.Count > 0)
                    {
                        foreach (var item in dealerRelationshipHistories)
                        {
                            item.ModifiedBy = 1;
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;

                            _db.SaveChanges();
                        }
                    }

                    var dealerAttachmentsList = _db.DealerAttachments.Where(x => x.DealerId == model.Id).ToList();

                    if (dealerAttachmentsList?.Count > 0)
                    {
                        foreach (var item in dealerAttachmentsList)
                        {
                            item.ModifiedBy = 1;
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

        [HttpGet]
        [Route("GetAllDealerEstates")]
        public IActionResult GetAllDealerEstates()
        {
            try
            {
                var result = _db.dealerEstateDeatails.Where(x => !x.IsDeleted && x.IsPrimary==true)
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
        [Route("SelectDealerEstateById")]
        public IActionResult SelectDealerEstateById(int id)
        {
            try
            {
                var result = _db.dealerEstateDeatails.Where(x => !x.IsDeleted && x.Id == id)
                                                     .Include(x=>x.DealerDesignation)
                                                     .Select(x=> new
                                                     {
                                                         x.DealerDesignation.Description,
                                                         x.Name,
                                                         x.CNIC,
                                                         x.MobileNo,
                                                         x.EmailAddress
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




    }
}