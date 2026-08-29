using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace HRMS_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NDCRequestForDealerController : ControllerBase
    {
        private readonly DataBase_Context _db;
        ApprovalBLL _approvalBLL;
        public NDCRequestForDealerController(DataBase_Context db)
        {
            _db = db;
            _approvalBLL = new ApprovalBLL(_db);
        }

        // call this when user focus out from cnic field
        [HttpGet]
        [Route("/api/NDCRequestForDealer/GetNDCRequestForDealerByCnic")]
        public IActionResult GetNDCRequestForDealerByCnic(string cnic)
        {
            try
            {
                var result = _db.MemberProfile.Where(x => !x.IsDeleted
                                                   && x.Cnic == cnic
                                                   && x.CnicExpiryDate <= DateTime.Now
                                                     )
                                               .SingleOrDefault();
                if (result == null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Enter Valid Cnic",
                        Data = null
                    });

                }
                else
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result.Id
                    });
                }
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
                var result = _db.NDCRequestForDealer.Where(x => !x.IsDeleted)
                                                       .Include(x => x.NDCRequestForDealerCharges.Where(x => !x.IsDeleted))
                                                       .Include(x => x.NDCRequestForDealerAttachments.Where(x => !x.IsDeleted))
                                                       .Include(x => x.TransferType)
                                                       .Include(x => x.NDCRequestType)
                                                       .Include(x => x.StockCreation)
                                                       .Include(x=>x.Dealer)
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
                var result = _db.NDCRequestForDealer.Where(x => !x.IsDeleted)
                                                       .Include(x => x.NDCRequestForDealerCharges.Where(x => !x.IsDeleted))
                                                       .Include(x => x.NDCRequestForDealerAttachments.Where(x => !x.IsDeleted))
                                                       .Include(x => x.TransferType)
                                                       .Include(x => x.NDCRequestType)
                                                       .Include(x => x.Dealer)
                                                       .Include(x => x.StockCreation)
                                                       .Include(x => x.MemberProfile)
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
        [Route("AddNewNDCRequestForDealer")]
        public IActionResult AddNewNDCRequestForDealer(NDCRequestForDealer model)
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

                if(model.PropertyNo != null)
                {
                    StockCreation stockCreation = _db.StockCreations.Where(x => x.PropertyNo == model.PropertyNo && x.MemberProfileId == model.MemberProfileId).FirstOrDefault();
                    if (stockCreation == null)
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "No property exist! please check membercode and propertyno",
                            Data = null
                        });
                    }
                    else
                    {
                        model.StockCreationId = stockCreation.ID;
                        model.MemberProfileId = stockCreation.MemberProfileId;
                    }
                }

                //var approvalSetup = _db.ApprovalSetup.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.ConstructionSecurity).ToList();
                //if (approvalSetup.Count <= 0)
                //{
                //    return Ok(new ApiResponse<object>
                //    {
                //        Code = ResponseCode.NotFound,
                //        Message = "Approval setup not defined or In-active",
                //        Data = null
                //    });
                //}
                model.ValidityDate = DateTime.Now.AddDays(45);
                model.IsActive = true;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = model.CreatedBy;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;

                if (model.NDCRequestForDealerCharges?.Count > 0)
                {
                    foreach (var item in model.NDCRequestForDealerCharges)
                    {
                        item.ModifiedBy = 1;
                        item.CreatedBy = 1;
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                if (model.NDCRequestForDealerAttachments?.Count > 0)
                {
                    foreach (var item in model.NDCRequestForDealerAttachments)
                    {
                        item.ModifiedBy = item.ModifiedBy;
                        item.CreatedBy = item.CreatedBy;
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                _db.NDCRequestForDealer.Add(model);
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
        [Route("UpdateNDCRequestForDealer")]
        public IActionResult UpdateNDCRequestForDealer(NDCRequestForDealer model)
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

                var data = _db.NDCRequestForDealer.Find(model.Id);

                if (data != null)
                {
                    data.Outstation = model.Outstation;
                    data.SlotTime = model.SlotTime;
                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;
                    data.LastModified = DateTime.Now;

                    _db.Entry(data).State = EntityState.Modified;
                    _db.SaveChanges();

                    if (model.NDCRequestForDealerCharges?.Count > 0)
                    {
                        var result = _db.NDCRequestForDealerCharges.Where(x => x.NDCRequestForDealerId == model.Id).ToList();

                        _db.NDCRequestForDealerCharges.RemoveRange(result);
                        _db.SaveChanges();
                    }

                    if (model.NDCRequestForDealerCharges?.Count > 0)
                    {
                        foreach (var item in model.NDCRequestForDealerCharges)
                        {
                            item.NDCRequestForDealerId = data.Id;
                            item.ModifiedBy = item.ModifiedBy;
                            item.LastModifiedUserName = item.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.NDCRequestForDealerCharges.AddRange(model.NDCRequestForDealerCharges);
                        _db.SaveChanges();
                    }

                    if (model.NDCRequestForDealerAttachments?.Count > 0)
                    {
                        var result = _db.NDCRequestForDealerAttachments.Where(x => x.NDCRequestForDealerId == model.Id).ToList();

                        _db.NDCRequestForDealerAttachments.RemoveRange(result);
                        _db.SaveChanges();
                    }

                    if (model.NDCRequestForDealerAttachments?.Count > 0)
                    {
                        foreach (var item in model.NDCRequestForDealerAttachments)
                        {
                            item.NDCRequestForDealerId = data.Id;
                            item.ModifiedBy = item.ModifiedBy;
                            item.LastModifiedUserName = item.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.NDCRequestForDealerAttachments.AddRange(model.NDCRequestForDealerAttachments);
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
        [Route("DeleteNDCRequestForDealer")]
        public IActionResult DeleteNDCRequestForDealer(int id)
        {
            try
            {
                var model = _db.NDCRequestForDealer.Find(id);

                if (model != null)
                {
                    model.ModifiedBy = model.ModifiedBy;
                    model.LastModified = DateTime.Now;
                    model.IsActive = false;
                    model.IsDeleted = true;

                    _db.SaveChanges();

                    var nDCRequestForDealerCharges = _db.NDCRequestForDealerCharges.Where(x => x.NDCRequestForDealerId == model.Id).ToList();

                    if (nDCRequestForDealerCharges?.Count > 0)
                    {
                        foreach (var item in nDCRequestForDealerCharges)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;
                            _db.SaveChanges();
                        }
                    }

                    var nDCRequestForDealerAttachments = _db.NDCRequestForDealerAttachments.Where(x => x.NDCRequestForDealerId == model.Id).ToList();

                    if (nDCRequestForDealerAttachments?.Count > 0)
                    {
                        foreach (var item in nDCRequestForDealerAttachments)
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
