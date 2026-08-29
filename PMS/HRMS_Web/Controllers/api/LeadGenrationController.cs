using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common.Enums;
using B_Utility.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LeadGenrationController : ControllerBase
    {
        private readonly DataBase_Context _db;
  
        ApprovalBLL _approvalBLL;
        public LeadGenrationController(DataBase_Context db)
        {
            _db = db;
            _approvalBLL = new ApprovalBLL(_db);
        }

        //[HttpGet]
        //[Route("/api/ConstructionSecurity/GetAllConstructionSecurityFilterList")]
        //public IActionResult GetAllConstructionSecurityFilterList()
        //{
        //    try
        //    {
        //        var result = _db.StockCreations.Where(x => !x.is_deleted
        //                                           && x.Is_MapApprovalApproved == true
        //                                           && x.Is_ConstructionSecurityRequested != true
        //                                             )
        //                                       .ToList();

        //        return Ok(new ApiResponse<object>
        //        {
        //            Code = ResponseCode.Success,
        //            Message = "Success",
        //            Data = result
        //        });
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
        //    }
        //}

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _db.LeadGenration.Where(x => !x.IsDeleted)
                                                       .Include(x => x.LGSocialStatus)
                                                       .Include(x => x.LGActivities)
                                                       .Include(x => x.LGInterests)
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
        [Route("GetAllLead")]
        public IActionResult GetAllLead()
        {
            try
            {
                var result = _db.LeadGenration.Where(x => !x.IsDeleted)                                            
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
        [Route("GetAllByUserId")]
        public IActionResult GetAllByUserId(int userId)
        {
            try
            {
                var result = _db.LeadGenration.Where(x => !x.IsDeleted && x.CreatedBy == userId)                                                     
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
                var result = _db.LeadGenration.Where(x => !x.IsDeleted && x.Id == id)
                                                                      .Include(x => x.LGSocialStatus)
                                                                      .Include(x => x.LGActivities)
                                                                      .Include(x => x.LGInterests)
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

        [HttpPost]
        [Route("/api/LeadGenration/AddNewLeadGenration")]
        public IActionResult AddNewLeadGenration(LeadGenration model)
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

                //var approvalSetup = _db.ApprovalSetup.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.ConstructionSecurity).ToList();
                //if (approvalSetup.Count <= 0)
                //{
                //    return Ok(new ApiResponse<object>
                //    {
                //        Code = ResponseCode.NotFound,
                //        Message = "Not Found",
                //        Data = "Approval setup not defined or In-active"
                //    });
                //}
                model.IsActive = true;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = model.CreatedBy;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;

                _db.LeadGenration.Add(model);
                _db.SaveChanges();

                //StockCreation stockCreation = (StockCreation)_db.StockCreations.Where(x => x.ID == model.StockCreationId)
                //                                                               .FirstOrDefault();
                //if (stockCreation != null)
                //{
                //    stockCreation.Is_ConstructionSecurityRequested = true;
                //    _db.SaveChanges();

                //    bool result = _approvalBLL.AddNewApprovalSetup(stockCreation.ID, (int)ApprovalUIIds.ConstructionSecurity);

                //    if (result)
                //    {
                //        return Ok(new ApiResponse<object>
                //        {
                //            Code = ResponseCode.Success,
                //            Message = "Success",
                //            Data = "Construction Security added succesfully and moved for approval"
                //        });
                //    }
                //}

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
        [Route("UpdateLeadGenration")]
        public IActionResult UpdateLeadGenration(LeadGenration model)
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

                var data = _db.LeadGenration.Find(model.Id);

                if (data != null)
                {
                    data.HonorificsName = model.HonorificsName;
                    data.Name = model.Name;
                    data.HonorificsContactPersoon = model.HonorificsContactPersoon;
                    data.ContactPerson = model.ContactPerson;
                    data.ContactPersonNumber = model.ContactPersonNumber;
                    data.Relationship = model.Relationship;
                    data.RelationshipWith = model.RelationshipWith;
                    data.Cnic = model.Cnic;
                    data.CnicExpirtyDate=model.CnicExpirtyDate;
                    data.MobileNo = model.MobileNo;
                    data.WhatsAppNo = model.WhatsAppNo;
                    data.Nationality = model.Nationality;
                    data.CountryOfResidence = model.CountryOfResidence;
                    data.CityOfResidence = model.CityOfResidence;
                    data.EmailId = model.EmailId;
                    data.Interst = model.Interst;
                    data.Gender = model.Gender;
                    data.ModeOfContact = model.ModeOfContact;
                    data.SourceOfInfo = model.SourceOfInfo;
                    data.Remarks = model.Remarks;
                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;
                    data.LastModified = DateTime.Now;

                    _db.Entry(data).State = EntityState.Modified;
                    _db.SaveChanges();

                    if (model.LGSocialStatus?.Count > 0)
                    {
                        var result = _db.LGSocialStatus.Where(x => x.LeadGenrationId == model.Id).ToList();

                        _db.LGSocialStatus.RemoveRange(result);
                        _db.SaveChanges();
                    }

                    if (model.LGSocialStatus?.Count > 0)
                    {
                        foreach (var item in model.LGSocialStatus)
                        {
                            item.LeadGenrationId = data.Id;
                        }

                        _db.LGSocialStatus.AddRange(model.LGSocialStatus);
                        _db.SaveChanges();
                    }

                    if (model.LGActivities?.Count > 0)
                    {
                        var result = _db.LGActivities.Where(x => x.LeadGenrationId == model.Id).ToList();

                        _db.LGActivities.RemoveRange(result);
                        _db.SaveChanges();
                    }

                    if (model.LGActivities?.Count > 0)
                    {
                        foreach (var item in model.LGActivities)
                        {
                            item.LeadGenrationId = data.Id;
                        }

                        _db.LGActivities.AddRange(model.LGActivities);
                        _db.SaveChanges();
                    }

                    if (model.LGInterests?.Count > 0)
                    {
                        var result = _db.LGInterests.Where(x => x.LeadGenrationId == model.Id).ToList();

                        _db.LGInterests.RemoveRange(result);
                        _db.SaveChanges();
                    }

                    if (model.LGInterests?.Count > 0)
                    {
                        foreach (var item in model.LGInterests)
                        {
                            item.LeadGenrationId = data.Id;
                        }

                        _db.LGInterests.AddRange(model.LGInterests);
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
        [Route("DeleteLeadGenration")]
        public IActionResult DeleteLeadGenration(int id)
        {
            try
            {
                var model = _db.LeadGenration.Find(id);

                if (model != null)
                {
                   _db.LeadGenration.Remove(model);
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
