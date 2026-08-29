using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using HRMS_Web.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly DataBase_Context _db;

        public NotificationController(DataBase_Context db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("GetNotificationCount")]
        public IActionResult GetNotificationCount(int userId)
        {
            try
            {
                List<Notification> notifications = _db.Notifications
                                                      .Where(n => n.Receivers.Any(r => r.Receiver == userId.ToString()) && n.IsViewed == false)
                                                      .OrderByDescending(x => x.CreatedOn)
                                                      .ToList();
                int count = notifications.Count();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = count
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll(int userId)
        {
            try
            {
                List<Notification> notifications = _db.Notifications
                                                      .Where(n => n.Receivers.Any(r => r.Receiver == userId.ToString()))
                                                      .OrderByDescending(x=>x.CreatedOn)
                                                      .ToList();
                if(notifications.Count() > 0)
                { 
                   foreach(var  notification in notifications.Where(x=>x.IsViewed == false))
                   {
                       notification.IsViewed = true;
                   }

                    _db.SaveChanges();
                }
                

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = notifications
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }
        [HttpPost]
        [Route("AddNotification")]
        public IActionResult AddNotification([FromBody] NotificationDTO model)
        {
            try
            {
                Notification notification = new Notification();

                notification.Narration = model.Narration;
                notification.Sender = model.Sender;
                notification.SenderName = model.SenderName;
                notification.Designation = model.Designation;
                notification.Type = model.Type;
                notification.IsViewed = false;
                notification.CreatedBy = model.Sender;
                notification.ModifiedBy = model.Sender;
                notification.CreatedOn = DateTime.Now;
                notification.IsActive = true;
                notification.IsDeleted = false;

                List<NotificationReceiver> notificationReceivers = new List<NotificationReceiver>();

                for (int i = 0; i < model.SelectedUsers.Length; i++)
                {
                    string userId = model.SelectedUsers[i];

                    if(!string.IsNullOrEmpty(userId))
                    { 
                        NotificationReceiver notificationReceiver = new NotificationReceiver()
                        {
                            Receiver = userId,
                        };
                        
                        notificationReceivers.Add(notificationReceiver);
                    }
                }

                notification.Receivers = notificationReceivers;

                _db.Notifications.Add(notification);
                _db.SaveChanges();

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
        [Route("AddFormAlert")]
        public IActionResult AddFormAlert([FromBody] FormAlert model)
        {
            try
            {
                var existingalert = _db.FormAlerts.Where(i => i.FormId == model.FormId).FirstOrDefault();
                if (existingalert == null)
                {
                    model.CreatedOn = DateTime.Now;
                    model.IsActive = true;
                    model.IsDeleted = false;
                    model.ModifiedBy = model.ModifiedBy;
                    model.CreatedBy = model.CreatedBy;
                    model.LastModifiedUserName = model.LastModifiedUserName;
                    _db.FormAlerts.Add(model);
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
                    var existingusers = _db.FormAlertUsers.Where(i => i.FormAlertId == existingalert.Id).ToList();
                    if (existingusers.Count > 0)
                    {
                        _db.RemoveRange(existingusers);
                        _db.SaveChanges();
                    }
                    if (model.FormAlertUsers?.Count > 0)
                    {

                        foreach (var user in model.FormAlertUsers)
                        {
                            user.IsDeleted = false;
                            user.IsActive = true;
                            user.ModifiedBy = model.ModifiedBy;
                            user.LastModifiedUserName = model.LastModifiedUserName;
                            user.CreatedBy = model.CreatedBy;
                            user.IsActive = true;
                            user.CreatedOn= DateTime.Now;
                            user.FormAlertId = existingalert.Id;
                            _db.FormAlertUsers.Add(user);
                            _db.SaveChanges();
                        }
                    }

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Updated Successfuly",
                        Data = model
                    });
                }
              

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }


        [HttpGet]
        [Route("GetAllFormAlerts")]
        public IActionResult GetAllFormAlerts()
        {
            try
            {
                List<FormAlert> FormAlerts = _db.FormAlerts
                                                      .Where(x=>x.IsActive==true)
                                                      .ToList();
                if (FormAlerts.Count() > 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = FormAlerts
                    });
                }
                else
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = null
                    });
                }


            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }


        [HttpGet]
        [Route("GetUsersAgainstForm")]
        public IActionResult GetUsersAgainstForm(int id)
        {
            try
            {
                var FormAlert = _db.FormAlerts.Where(x=>x.FormId==id).FirstOrDefault();
                if (FormAlert != null)
                {
                    var formalertusers=_db.FormAlertUsers.Where(x=>x.FormAlertId== FormAlert.Id).ToList();
                    if (formalertusers.Count() > 0)
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "Success",
                            Data = formalertusers
                        });
                    }
                    else
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "Success",
                            Data = null
                        });
                    }
                }
                
                else
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = null
                    });
                }


            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

    }
}
