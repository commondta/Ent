using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using CloudinaryDotNet.Actions;
using HRMS_Web.Extensions;
using HRMS_Web.Services.NotificationService;
using HRMS_Web.Services.PhotoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly DataBase_Context _db;
        private readonly IPhotoService photoService;
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        CommonBLL _commonBLL;

        public PromotionController(DataBase_Context db, IPhotoService photoService, INotificationService notificationService, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _commonBLL = new CommonBLL(_db);
            this.photoService = photoService;
            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult Get(int id)
        {
            try
            {

                var result = _db.Promotion.Where(x => !x.IsDeleted && x.Id == id)
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
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = _db.Promotion.Where(x => !x.IsDeleted)
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
        [Route("GetAllFilteredPromotions")]
        public IActionResult GetAllFilteredPromotions(string promtionType)
        {
            try
            {
                var result = _db.Promotion
                                .Where(x => x.IsActive
                                         && x.PromotionType == promtionType
                                         && x.FromDate <= DateTime.Now.Date
                                         && x.ToDate >= DateTime.Now.Date)
                                .ToList();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }


        [HttpPost]
        [Route("AddNewPromotion")]
        public async Task<IActionResult> AddNewPromotionAsync([FromBody] Promotion model)
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
                if (!string.IsNullOrEmpty(model.Image))
                {
                    var path = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";
                    model.Image = string.IsNullOrEmpty(model.Image) ? "" : $"{path}{await model.Image.SaveBase64FileAsync()}";
                    //var uploadResult = photoService.UploadPhotoAsync(model.Image)
                    //                                .GetAwaiter()
                    //                                .GetResult();

                    //if (uploadResult is ImageUploadResult imageResult)
                    //{
                    //    model.Image = imageResult.SecureUrl.AbsoluteUri;
                    //}
                    //else if (uploadResult is VideoUploadResult videoResult)
                    //{
                    //    model.Image = videoResult.SecureUrl.AbsoluteUri;
                    //}
                    //else
                    //{
                    //    throw new InvalidOperationException("Unexpected upload result type.");
                    //}

                }

                model.CreatedOn = DateTime.Now;
                model.LastModified = DateTime.Now;
                model.CreatedBy = model.CreatedBy;
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;
                model.IsDeleted = false;
                model.IsActive = model.IsActive;

                _db.Promotion.Add(model);
                _db.SaveChanges();

                //await _notificationService.SendOfferNotificationAsync(
                //      $"New {model.PromotionType} Added",
                //      $"{model.Title}");

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
        [Route("UpdatePromotion")]
        public async Task<IActionResult> UpdatePromotionAsync([FromBody] Promotion model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorMessages = string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = errorMessages
                    });
                }

                var data = _db.Promotion.Find(model.Id);
                if (!string.IsNullOrEmpty(model.Image))
                {
                    bool isUrl = UHelper.IsUrl(model.Image);
                    if (isUrl)
                    {
                        data.Image = model.Image;
                    }
                    else
                    {
                        data.Image.DeleteFile();

                        var request = _httpContextAccessor.HttpContext.Request;
                        var path = $"{request.Scheme}://{request.Host}{request.PathBase}";

                        data.Image = $"{path}{await model.Image.SaveBase64FileAsync()}";
                    }
                }
                else
                {
                    data.Image = model.Image;
                }

                data.Title = model.Title;
                data.FromDate = model.FromDate;
                data.ToDate = model.ToDate;
                data.PromotionType = model.PromotionType;
                data.IsActive = model.IsActive;
                data.LastModified = DateTime.Now;
                data.ModifiedBy = model.ModifiedBy;
                data.LastModifiedUserName = model.LastModifiedUserName;

                _db.SaveChanges();

                //await _notificationService.SendOfferNotificationAsync(
                //     $"New {model.PromotionType} Added",
                //     $"{model.Title}");

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = model
                });
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }


        [HttpDelete]
        [Route("DeletePromotion")]
        public IActionResult DeletePromotion(int id)
        {
            try
            {
                var data = _db.Promotion.Find(id);
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

        [HttpDelete]
        [Route("Discard")]
        public IActionResult Discard(int id)
        {
            var promotion = _db.Promotion.Where(x => x.Id == id).FirstOrDefault();
            if (promotion != null)
            {
                if (!string.IsNullOrEmpty(promotion.Image))
                {

                    string publicId = UHelper.ExtractIdentifierFromUrl(promotion.Image);
                    var deleteResult = photoService.DeletePhotoAsync(publicId)
                                                   .GetAwaiter()
                                                   .GetResult();
                }

                _db.Promotion.Remove(promotion);
                _db.SaveChanges();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Promotion Deleted Successfully",
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

            return Ok(new ApiResponse<object>
            {
                Code = ResponseCode.BadRequest,
                Message = "something went wrong",
                Data = null
            });
        }
    }
}
