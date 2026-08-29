using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CalendarController : ControllerBase
    {
        private readonly DataBase_Context _db;
        public CalendarController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult Get(string dayOfWeek)
        {
            try
            {
                var result = _db.WeekSchedules.Where(x => !x.IsDeleted && x.DayOfWeek == dayOfWeek)
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
        [Route("GetAvailableHours")]
        public IActionResult GetAvailableHours(string dayOfWeek, DateTime datePicker)
        {
            try
            {
                var availableHours = _db.WeekSchedules
                                             .Where(ws => !ws.IsDeleted && ws.DayOfWeek == dayOfWeek)
                                             .Select(ws => ws.Hour)
                                             .Distinct()
                                             //.Except(_db.NDCRequestForMember
                                             //    .Where(ndc => !ndc.IsDeleted && ndc.SlotDate == datePicker)
                                             //    .Select(ndc => ndc.SlotHour))
                                             .ToList();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = availableHours
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetAvailableMintues")]
        public IActionResult GetAvailableMintues(string dayOfWeek, DateTime datePicker, string hour)
        {
            try
            {
                var availableMintues = _db.WeekSchedules
                                             .Where(ws => !ws.IsDeleted && ws.DayOfWeek == dayOfWeek && ws.Hour == hour)
                                             .Select(ws => ws.Mintues)
                                             .Except(_db.NDCRequestForMember
                                                 .Where(ndc => !ndc.IsDeleted && ndc.SlotDate == datePicker && ndc.SlotHour == hour)
                                                 .Select(ndc => ndc.SlotMintues))
                                             .ToList();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = availableMintues
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        [Route("SaveWeekSchedule")]
        public IActionResult SaveWeekSchedule(List<WeekSchedule> model)
        {
            try
            {

                if (model.Count > 0)
                {
                    var result = _db.WeekSchedules.Where(x => x.DayOfWeek == model.FirstOrDefault().DayOfWeek).ToList();
                    if (result.Count > 0)
                    {
                        _db.WeekSchedules.RemoveRange(result);
                        _db.SaveChanges();
                    }
                }

                if (model.Count > 0)
                {
                    foreach (var item in model)
                    {
                        item.ModifiedBy = item.ModifiedBy;
                        item.LastModifiedUserName = item.LastModifiedUserName;
                        item.CreatedBy = item.CreatedBy;
                        item.LastModified = DateTime.Now;
                        item.IsActive = item.IsActive;
                        item.IsDeleted = false;
                    }

                    _db.WeekSchedules.AddRange(model);
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


        // Executive Setup

        [HttpGet]
        [Route("GetExecutive")]
        public IActionResult GetExecutive(string dayOfWeek)
        {
            try
            {
                var result = _db.WeekScheduleExective.Where(x => !x.IsDeleted && x.DayOfWeek == dayOfWeek)
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
        [Route("GetAvailableHoursExecutive")]
        public IActionResult GetAvailableHoursExecutive(string dayOfWeek, DateTime datePicker)
        {
            try
            {
                var availableHours = _db.WeekScheduleExective
                                             .Where(ws => !ws.IsDeleted && ws.DayOfWeek == dayOfWeek)
                                             .Select(ws => ws.Hour)
                                             .Distinct()
                                             //.Except(_db.NDCRequestForMember
                                             //    .Where(ndc => !ndc.IsDeleted && ndc.SlotDate == datePicker)
                                             //    .Select(ndc => ndc.SlotHour))
                                             .ToList();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = availableHours
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetAvailableMintuesExecutive")]
        public IActionResult GetAvailableMintuesExecutive(string dayOfWeek, DateTime datePicker, string hour)
        {
            try
            {
                var availableMintues = _db.WeekScheduleExective
                                             .Where(ws => !ws.IsDeleted && ws.DayOfWeek == dayOfWeek && ws.Hour == hour)
                                             .Select(ws => ws.Mintues)
                                             .Except(_db.NDCRequestForMember
                                                 .Where(ndc => !ndc.IsDeleted && ndc.SlotDate == datePicker && ndc.SlotHour == hour)
                                                 .Select(ndc => ndc.SlotMintues))
                                             .ToList();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = availableMintues
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }


        [HttpPost]
        [Route("SaveWeekScheduleExective")]
        public IActionResult SaveWeekScheduleExective(List<WeekScheduleExective> model)
        {
            try
            {

                if (model.Count > 0)
                {
                    var result = _db.WeekScheduleExective.Where(x => x.DayOfWeek == model.FirstOrDefault().DayOfWeek).ToList();
                    if (result.Count > 0)
                    {
                        _db.WeekScheduleExective.RemoveRange(result);
                        _db.SaveChanges();
                    } 
                }

                if (model.Count > 0)
                {
                    foreach (var item in model)
                    {
                        item.CreatedBy = item.CreatedBy;
                        item.ModifiedBy = item.ModifiedBy;
                        item.LastModifiedUserName = item.LastModifiedUserName;
                        item.LastModified = DateTime.Now;
                        item.IsActive = item.IsActive;
                        item.IsDeleted = false;
                    }

                    _db.WeekScheduleExective.AddRange(model);
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
    }
}
