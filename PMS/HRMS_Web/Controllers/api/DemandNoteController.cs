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
    public class DemandNoteController : ControllerBase
    {
        private readonly DataBase_Context _db;
        ApprovalBLL _approvalBLL;
        public DemandNoteController(DataBase_Context db)
        {
            _db = db;
            _approvalBLL = new ApprovalBLL(_db);
        }



        [HttpGet]
        [Route("GetAllWithRespectToHOD")]
        public IActionResult GetAllWithRespectToHOD(int managerId)
        {
            try
            {
                var result = _db.DemandNote.Where(x => !x.IsDeleted && x.ManagerId == managerId && x.ManagerAssigned == true && x.DNManagerStatus == null)
                                                       .Include(x => x.DemandNoteItems)
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
        [Route("GetAllWithRespectToCustodian")]
        public IActionResult GetAllWithRespectToCustodian(int id)
        {
            try
            {
                var result = _db.DemandNote.Where(x => !x.IsDeleted && x.CustodianId == id && x.DNManagerStatus == true && x.CustodianAssigned == true && x.SapPosting != true)
                                                       .Include(x => x.DemandNoteItems)
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
                var userLogedInIdString = HttpContext.Session.GetString("ID");
                int userIDInt = (int)(userLogedInIdString != null ? Convert.ToInt64(userLogedInIdString) : 0);
                var result = _db.DemandNote.Where(x => !x.IsDeleted && x.CreatedBy == userIDInt)
                                                       .Include(x => x.DemandNoteItems)
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
                var result = _db.DemandNote.Where(x => !x.IsDeleted && x.Id == id)
                                                                      .Include(x => x.DemandNoteItems)
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
        [Route("/api/DemandNote/AddNewDemandNote")]
        public IActionResult AddNewDemandNote(DemandNote model)
        {
            try
            {

                model.IsDemandNoteRequested = true;
                model.ManagerAssigned = true;
                model.IsActive = true;
                model.IsDeleted = false;
                model.CustodianAssigned = false;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = model.CreatedBy;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;

                if (model.DemandNoteItems.Count > 0)
                {
                    foreach (var item in model.DemandNoteItems)
                    {
                        item.ModifiedBy = item.ModifiedBy;
                        item.LastModifiedUserName = item.LastModifiedUserName;
                        item.CreatedBy = item.ModifiedBy;
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                _db.DemandNote.Add(model);
                _db.SaveChanges();


                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Demanad Note Added and Waiting for HOD Approval",
                    Data = null
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        [Route("UpdateDemandNote")]
        public IActionResult UpdateDemandNote(DemandNote model)
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

                var data = _db.DemandNote.Find(model.Id);

                if (data != null)
                {

                    data.LastModified = DateTime.Now;

                    _db.Entry(data).State = EntityState.Modified;
                    _db.SaveChanges();

                    if (model.DemandNoteItems?.Count() > 0)
                    {
                        var result = _db.DemandNoteItems.Where(x => x.DemandNoteId == model.Id).ToList();

                        _db.DemandNoteItems.RemoveRange(result);
                        _db.SaveChanges();
                    }
                    if (model.DemandNoteItems?.Count() > 0)
                    {
                        foreach (var item in model.DemandNoteItems)
                        {
                            item.DemandNoteId = model.Id;
                            item.ModifiedBy = item.ModifiedBy;
                            item.LastModifiedUserName = item.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;

                            _db.DemandNoteItems.Add(item);
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
                    Message = "Updated Successfully",
                    Data = data
                });

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }
        [HttpPost]
        [Route("UpdateDemandNoteByUser")]
        public IActionResult UpdateDemandNoteByUser(DemandNote model)
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

                var data = _db.DemandNote.Find(model.Id);

                if (data != null)
                {
                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;
                    data.LastModified = DateTime.Now;
                    data.ManagerId = model.ManagerId;
                    data.DNManagerStatus = null;
                    data.ItemGroupCode = model.ItemGroupCode;
                    data.Remarks = model.Remarks;
                    data.ValidUntill = model.ValidUntill;
                    data.RequiredDate = model.RequiredDate;
                    _db.Entry(data).State = EntityState.Modified;
                    _db.SaveChanges();

                    if (model.DemandNoteItems?.Count() > 0)
                    {
                        var result = _db.DemandNoteItems.Where(x => x.DemandNoteId == model.Id).ToList();

                        _db.DemandNoteItems.RemoveRange(result);
                        _db.SaveChanges();
                    }
                    if (model.DemandNoteItems?.Count() > 0)
                    {
                        foreach (var item in model.DemandNoteItems)
                        {
                            item.DemandNoteId = model.Id;
                            item.ModifiedBy = item.ModifiedBy;
                            item.LastModifiedUserName = item.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;

                            _db.DemandNoteItems.Add(item);
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
                    Message = "Updated Successfully",
                    Data = data
                });

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }



        [HttpDelete]
        [Route("DeleteDemandNote")]
        public IActionResult DeleteDemandNote(int id)
        {
            try
            {
                var model = _db.DemandNote.Find(id);

                if (model != null)
                {
                    model.LastModified = DateTime.Now;
                    model.IsActive = false;
                    model.IsDeleted = true;

                    _db.SaveChanges();

                    var demandNoteItems = _db.DemandNoteItems.Where(x => x.DemandNoteId == model.Id).ToList();

                    if (demandNoteItems.Count > 0)
                    {
                        foreach (var item in demandNoteItems)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;
                        }

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
        [HttpGet]
        [Route("ApproveOrRejectByManager")]
        public IActionResult ApproveOrRejectByManager(int id, bool status, string Comment)
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

                var data = _db.DemandNote.Find(id);

                if (data != null)
                {

                    data.ManagerApproved_At = DateTime.Now;
                    data.CustodianAssigned = true;
                    data.ManagerApprovedOrRejectRemarks = Comment;
                    data.DNManagerStatus = status;

                    data.LastModified = DateTime.Now;

                    _db.Entry(data).State = EntityState.Modified;
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

                if (status == true)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Demand Note Approved By HOD",
                        Data = data
                    });
                }
                else
                {
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "Demand Note Rejected By HOD",
                            Data = data
                        });
                    }
                }

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }


        [HttpGet]
        [Route("ApproveOrRejectByCustodian")]
        public IActionResult ApproveOrRejectByCustodian(int id, bool status, string Comment)
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
                var data = _db.DemandNote.Where(x => x.Id == id).Include(x => x.DemandNoteItems)
                                                       .FirstOrDefault();
                if (data != null)
                {
                    if (status == true)
                    {
                        data.CustodianApproved_At = DateTime.Now;
                        data.IsDemandNoteApproved = true;

                        data.CustodianApprovedOrRejectRemarks = Comment;
                        data.DNCustodianStatus = true;
                        data.LastModified = DateTime.Now;
                        //Response_Result integration = new SapIntegrationController(_db).PostPurchaseRequest(data);
                        //if (integration.code == 0)
                        //{
                        //    _db.Entry(data).State = EntityState.Modified;
                        //    _db.SaveChanges();
                        //    return Ok(new ApiResponse<object>
                        //    {
                        //        Code = ResponseCode.Success,
                        //        Message = "Demand Note Approved And Posted IN SAP",
                        //        Data = data
                        //    });
                        //}
                        //else
                        //{
                        //    return Ok(new ApiResponse<object>
                        //    {
                        //        Code = ResponseCode.Error,
                        //        Message = integration.message,
                        //        Data = data
                        //    });
                        //}
                    }
                    else
                    {
                        data.CustodianRejected_At = DateTime.Now;
                        data.CustodianAssigned = false;
                        data.ManagerApprovedOrRejectRemarks = Comment;
                        data.DNManagerStatus = null;
                        data.LastModified = DateTime.Now;
                    }

                    _db.Entry(data).State = EntityState.Modified;
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

                if (status == true)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Demand Note Approved And Posted IN SAP",
                        Data = data
                    });
                }
                else
                {
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "Demand Note Rejected By Custodian",
                            Data = data
                        });
                    }
                }

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        //[HttpGet]
        //[Route("ApproveOrRejectByCustodian")]
        //public IActionResult ApproveOrRejectByCustodian(int id, bool status, string Comment)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return Ok(new ApiResponse<object>
        //            {
        //                Code = ResponseCode.BadRequest,
        //                Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
        //            });
        //        }
        //        var data = _db.DemandNote.Where(x => x.Id == id).Include(x => x.DemandNoteItems)
        //                                               .FirstOrDefault();
        //        if (data != null)
        //        {
        //            if (status == true)
        //            {
        //                data.CustodianApproved_At = DateTime.Now;
        //                data.IsDemandNoteApproved = true;

        //                data.CustodianApprovedOrRejectRemarks = Comment;
        //                data.DNCustodianStatus = true;
        //                data.LastModified = DateTime.Now;
        //                Response_Result integration = new SapIntegrationController(_db).PostPurchaseRequest(data);
        //                if (integration.code == 0)
        //                {
        //                    _db.Entry(data).State = EntityState.Modified;
        //                    _db.SaveChanges();
        //                    return Ok(new ApiResponse<object>
        //                    {
        //                        Code = ResponseCode.Success,
        //                        Message = "Demand Note Approved And Posted IN SAP",
        //                        Data = data
        //                    });
        //                }
        //                else
        //                {
        //                    return Ok(new ApiResponse<object>
        //                    {
        //                        Code = ResponseCode.Error,
        //                        Message = integration.message,
        //                        Data = data
        //                    });
        //                }
        //            }
        //            else
        //            {
        //                data.CustodianRejected_At = DateTime.Now;
        //                data.CustodianAssigned = false;
        //                data.ManagerApprovedOrRejectRemarks = Comment;
        //                data.DNManagerStatus = null;
        //                data.LastModified = DateTime.Now;
        //            }

        //            _db.Entry(data).State = EntityState.Modified;
        //            _db.SaveChanges();

        //        }
        //        else
        //        {
        //            return Ok(new ApiResponse<object>
        //            {
        //                Code = ResponseCode.NotFound,
        //                Message = "Not Found",
        //                Data = null
        //            });
        //        }

        //        if (status == true)
        //        {
        //            return Ok(new ApiResponse<object>
        //            {
        //                Code = ResponseCode.Success,
        //                Message = "Demand Note Approved And Posted IN SAP",
        //                Data = data
        //            });
        //        }
        //        else
        //        {
        //            {
        //                return Ok(new ApiResponse<object>
        //                {
        //                    Code = ResponseCode.Success,
        //                    Message = "Demand Note Rejected By Custodian",
        //                    Data = data
        //                });
        //            }
        //        }

        //    }
        //    catch (System.Exception ex)
        //    {
        //        return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
        //    }
        //}
    }
}
