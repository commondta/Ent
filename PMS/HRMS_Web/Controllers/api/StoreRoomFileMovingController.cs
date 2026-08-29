using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using B_Utility.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StoreRoomFileMovingController : ControllerBase
    {

        private readonly DataBase_Context _db;

        CommonBLL _commonBLL;

        public StoreRoomFileMovingController(DataBase_Context db)
        {
            _db = db;
            _commonBLL = new CommonBLL(_db);
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult Get(int id)
        {
            try
            {

                var result = _db.StoreRoomFileMoving.Where(x => x.Id == id)
                                           .Include(x=>x.PMSUser)
                                           .Include(x=>x.StockCreation)
                                           .Include(x=>x.Department)
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
        public IActionResult GetAll(int id)
        {
            try
            {
                var result = _db.StoreRoomFileMoving.Where(x=>x.PMSUserId == id && x.IsFileClosed != true)
                                                    .Include(x=>x.StockCreation)
                                                    .Select(x=> new{
                                                        x.Id,
                                                        x.StockCreationId,
                                                        x.StockCreation.RegistrationNo,
                                                        x.StockCreation.PropertyNo,
                                                        x.Remarks,
                                                        x.CreatedOn,
                                                        x.ExpectedReceivingDate,
                                                        x.PageOutIn,
                                                        Block = _db.Blocks.Where(b=>b.ID == Convert.ToInt32(x.StockCreation.Block)).FirstOrDefault().Description
                                                    })
                                                    .ToList()
                                                    .OrderByDescending(x=>x.Id);

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
        [Route("GetTodayAllFiles")]
        public IActionResult GetTodayAllFiles()
        {
            try
            {
                var todayStart = DateTime.Today;
                var todayEnd = todayStart.AddDays(1);

                var result = _db.StoreRoomFileMoving.Where(x => x.CreatedOn >= todayStart && x.CreatedOn < todayEnd) 
                                                    .Include(x => x.StockCreation)
                                                    .Include(x => x.PMSUser)
                                                    .Include(x => x.Department)
                                                    .Select(x => new {
                                                        x.Id,
                                                        x.StockCreationId,
                                                        x.StockCreation.RegistrationNo,
                                                        x.StockCreation.PropertyNo,
                                                        x.PMSUser.Username,
                                                        x.Department.Description,
                                                        x.IsFileClosed,
                                                        x.Remarks,
                                                        x.CreatedOn,
                                                        Block = _db.Blocks.Where(b => b.ID == Convert.ToInt32(x.StockCreation.Block)).FirstOrDefault().Description
                                                    })
                                                    .ToList()
                                                    .OrderByDescending(x => x.Id)
                                                    .DistinctBy(x => x.StockCreationId);


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
        [Route("GetAllOpenedFiles")]
        public IActionResult GetAllOpenedFiles()
        {
            try
            {
                var result = _db.StoreRoomFileMoving.Where(x => !x.IsFileClosed)
                                                    .Include(x => x.StockCreation)
                                                    .Include(x => x.PMSUser)
                                                    .Include(x => x.Department)
                                                    .Select(x => new {
                                                        x.Id,
                                                        x.StockCreationId,
                                                        x.StockCreation.RegistrationNo,
                                                        x.StockCreation.PropertyNo,
                                                        x.PMSUser.Username,
                                                        x.Department.Description,
                                                        x.IsFileClosed,
                                                        x.Remarks,
                                                        x.CreatedOn,
                                                        Block = _db.Blocks.Where(b => b.ID == Convert.ToInt32(x.StockCreation.Block)).FirstOrDefault().Description
                                                    })
                                                    .ToList()
                                                    .OrderByDescending(x => x.Id)
                                                    .DistinctBy(x => x.StockCreationId);


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
        [Route("GetAllIssuedFiles")]
        public IActionResult GetAllIssuedFiles()
        {
            try
            {
                var result = _db.StoreRoomFileMoving.Include(x => x.StockCreation)
                                                    .Include(x => x.PMSUser)
                                                    .Include(x => x.Department)
                                                    .Select(x => new {
                                                        x.Id,
                                                        x.StockCreationId,
                                                        x.StockCreation.RegistrationNo,
                                                        x.StockCreation.PropertyNo,
                                                        x.PMSUser.Username,
                                                        x.Department.Description,
                                                        x.IsFileClosed,
                                                        x.Remarks,
                                                        x.CreatedOn,
                                                        Block = _db.Blocks.Where(b => b.ID == Convert.ToInt32(x.StockCreation.Block)).FirstOrDefault().Description
                                                    })
                                                    .ToList()
                                                    .OrderByDescending(x => x.Id)
                                                    .DistinctBy(x => x.StockCreationId);


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
        [Route("AddNewFile")]
        public IActionResult AddNewFile(StoreRoomFileMoving model)
        {
            try
            {
                if(model.IsRecordRoom)
                { 
                   var isSoftLockActive = _commonBLL.IsSoftLockActive((int)model.StockCreationId, (int)SoftLocks.No_Transfer);

                    if (isSoftLockActive.IsFound)
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.NotFound,
                            Message = isSoftLockActive.message,
                            Data = null
                        });
                    }
                }

                model.IsActive = true;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = model.CreatedBy;
                model.LastModified = DateTime.Now;
                model.LastModifiedUserName = model.LastModifiedUserName;
                model.ModifiedBy = model.ModifiedBy;
                model.PageOutIn = model.PageOut + "/" + model.PageOutIn;
                

                var closeItems = _db.StoreRoomFileMoving.Where(x => x.StockCreationId == model.StockCreationId &&
                                                                    x.IsFileClosed != true)
                                                        .ToList();
                if(closeItems.Count()  > 0 && model.IsRecordRoom == true)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "File Already Issued.",
                        Data = null
                    });
                }
                foreach(var item in closeItems)
                {
                    item.IsFileClosed = true;
                }

                model.IsFileClosed = false;
                _db.StoreRoomFileMoving.Add(model);
                _db.SaveChanges();


                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "File Moved Successfully.",
                    Data = null
                });

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }


        [HttpPost]
        [Route("CloseFile")]
        public IActionResult CloseFile(int id)
        {
            try
            {
                var data = _db.StoreRoomFileMoving.Find(id);
                data.IsFileClosed = true;
                data.LastModified = DateTime.Now;
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
