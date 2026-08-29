using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using CloudinaryDotNet.Actions;
using HRMS_Web.Extensions;
using HRMS_Web.Services.PhotoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private readonly DataBase_Context _db;
        private readonly IPhotoService photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        CommonBLL _commonBLL;

        public BannerController(DataBase_Context db, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _commonBLL = new CommonBLL(_db);
            this.photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult Get(int id)
        {
            try
            {

                var result = _db.Banner.Where(x => !x.IsDeleted && x.Id == id)
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

                var result = _db.Banner
                                .Include(x => x.Block)
                                .Include(x => x.PropertyType)
                                .AsSplitQuery()
                                .Where(x => !x.IsDeleted)
                                .Select(x => new
                                {
                                    x.Id,
                                    x.Image,
                                    x.Thumbnail,
                                    x.CreatedOn,
                                    x.CreatedBy,
                                    x.LastModified,
                                    x.IsActive,
                                    Block = x.Block.Description ?? "N/A",
                                    Type = x.PropertyType.Description ?? "N/A",
                                    x.BannerType,
                                    x.Title,
                                    x.Description
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
        [Route("GetAllMobileBanners")]
        public IActionResult GetAllMobileBanners(string bannerType)
        {
            try
            {

                var result = _db.Banner
                                .Include(x => x.Block)
                                .Include(x => x.PropertyType)
                                .AsSplitQuery()
                                .Where(x => !x.IsDeleted && x.BannerType == bannerType)
                                .Select(x => new
                                {
                                    x.Id,
                                    x.Image,
                                    x.Thumbnail,
                                    x.CreatedOn,
                                    x.CreatedBy,
                                    x.LastModified,
                                    x.IsActive,
                                    Block = x.Block.Description ?? "N/A",
                                    Type = x.PropertyType.Description ?? "N/A",
                                    x.BannerType,
                                    x.Title,
                                    x.Description
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

        [HttpPost]
        [Route("AddNewBanner")]
        public async Task<IActionResult> AddNewBanner([FromBody] Banner model)
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
                    //var uploadResult = await photoService.UploadPhotoAsync(model.Image);

                    //if (uploadResult is ImageUploadResult imageResult)
                    //{
                    //    model.Image = imageResult.SecureUrl.AbsoluteUri;
                    //}
                    //else if (uploadResult is VideoUploadResult videoResult)
                    //{
                    //    model.Image = videoResult.SecureUrl.AbsoluteUri;
                    //}
                    //else if (uploadResult is RawUploadResult rawResult) 
                    //{
                    //    model.Image = rawResult.SecureUrl.AbsoluteUri;
                    //}
                    //else
                    //{
                    //    throw new InvalidOperationException("Unexpected upload result type.");
                    //}
                }


                if (!string.IsNullOrEmpty(model.Thumbnail))
                {
                    var path = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";
                    model.Thumbnail = string.IsNullOrEmpty(model.Image) ? "" : $"{path}{await model.Thumbnail.SaveBase64FileAsync()}";
                    //var uploadResult = await photoService.UploadPhotoAsync(model.Thumbnail);

                    //if (uploadResult is ImageUploadResult imageResult)
                    //{
                    //    model.Thumbnail = imageResult.SecureUrl.AbsoluteUri;
                    //}
                    //else if (uploadResult is VideoUploadResult videoResult)
                    //{
                    //    model.Thumbnail = videoResult.SecureUrl.AbsoluteUri;
                    //}
                    //else if (uploadResult is RawUploadResult rawResult)
                    //{
                    //    model.Image = rawResult.SecureUrl.AbsoluteUri;
                    //}
                    //else
                    //{
                    //    throw new InvalidOperationException("Unexpected upload result type.");
                    //}
                }

                model.CreatedOn = model.CreatedOn;
                model.LastModified = DateTime.Now;
                model.IsActive = model.IsActive;
                model.CreatedBy = model.CreatedBy;
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;
                model.IsDeleted = false;

                _db.Banner.Add(model);
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

        [HttpPut]
        [Route("UpdateBanner")]
        public async Task<IActionResult> UpdateBannerAsync([FromBody] Banner model)
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

                var data = _db.Banner.Find(model.Id);

                if (data != null)
                {
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

                    if (!string.IsNullOrEmpty(model.Thumbnail))
                    {
                        bool isUrl = UHelper.IsUrl(model.Thumbnail);
                        if (isUrl)
                        {
                            data.Thumbnail = model.Thumbnail;
                        }
                        else
                        {
                            data.Thumbnail.DeleteFile();

                            var request = _httpContextAccessor.HttpContext.Request;
                            var path = $"{request.Scheme}://{request.Host}{request.PathBase}";

                            data.Thumbnail = $"{path}{await model.Thumbnail.SaveBase64FileAsync()}";
                        }
                    }
                    else
                    {
                        data.Thumbnail = model.Thumbnail;
                    }

                    data.IsActive = model.IsActive;
                    data.BlockId = model.BlockId;
                    data.PropertyTypeId = model.PropertyTypeId;
                    data.BannerType = model.BannerType;
                    data.Title = model.Title;
                    data.Description = model.Description;
                    data.CreatedOn = model.CreatedOn;
                    data.LastModified = DateTime.Now;
                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;

                    _db.SaveChanges();

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = model
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
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpDelete]
        [Route("DeleteBanner")]
        public IActionResult DeleteBanner(int id)
        {
            try
            {
                var data = _db.Banner.Find(id);
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
            var banner = _db.Banner.Where(x => x.Id == id).FirstOrDefault();
            if (banner != null)
            {
                if (!string.IsNullOrEmpty(banner.Image))
                {

                    string publicId = UHelper.ExtractIdentifierFromUrl(banner.Image);
                    var deleteResult = photoService.DeletePhotoAsync(publicId)
                                                   .GetAwaiter()
                                                   .GetResult();
                }

                if (!string.IsNullOrEmpty(banner.Thumbnail))
                {

                    string publicId = UHelper.ExtractIdentifierFromUrl(banner.Thumbnail);
                    var deleteResult = photoService.DeletePhotoAsync(publicId)
                                                   .GetAwaiter()
                                                   .GetResult();
                }

                _db.Banner.Remove(banner);
                _db.SaveChanges();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Banner Deleted Successfully",
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
