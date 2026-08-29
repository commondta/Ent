using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using HRMS_Web.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WithHoldingTaxController : ControllerBase
    {
        private readonly DataBase_Context _db;

        CommonBLL _commonBLL;
        ApprovalBLL _approvalBLL;
        public WithHoldingTaxController(DataBase_Context db)
        {
            _db = db;
            _approvalBLL = new ApprovalBLL(_db);
            _commonBLL = new CommonBLL(_db);
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _db.WithHoldingTax.Where(x => !x.IsDeleted)
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
        [Route("SaveWithHoldingTax")]
        public IActionResult SaveWithHoldingTax([FromBody] List<WithHoldingTax> dto)
        {
            try
            {   
                var withHoldingTaxs = _db.WithHoldingTax.ToList();

                if (withHoldingTaxs.Count() > 0)
                {
                    var idsToRemove = withHoldingTaxs.Where(x => !dto.Any(y => y.Id == x.Id)).Select(x => x.Id).ToList();

                    foreach (var id in idsToRemove)
                    {
                        var itemToRemove = withHoldingTaxs.First(x => x.Id == id);
                        _db.WithHoldingTax.Remove(itemToRemove);

                        var removedfromappliedCharges = _db.WithHoldingTaxPropertyWise.Where(x => x.MatchId == id).ToList();

                        if (removedfromappliedCharges.Count() > 0)
                        {
                            _db.WithHoldingTaxPropertyWise.RemoveRange(removedfromappliedCharges);
                        }
                    }

                    _db.SaveChanges();
                }

                if (dto.Count > 0)
                {
                    foreach (var item in dto)
                    {
                        var withHoldingTax = _db.WithHoldingTax.Where(x => x.Id == item.Id).FirstOrDefault();

                        if (withHoldingTax != null)
                        {
                            withHoldingTax.TaxCode = item.TaxCode;
                            withHoldingTax.Description = item.Description;
                            withHoldingTax.CreatedBy = item.CreatedBy;
                            withHoldingTax.ModifiedBy = item.ModifiedBy;
                            withHoldingTax.LastModifiedUserName = item.LastModifiedUserName;
                            withHoldingTax.Rate = item.Rate;
                            withHoldingTax.LastModified = DateTime.Now;

                            _db.SaveChanges();

                            var chargeonprop = _db.WithHoldingTaxPropertyWise.Where(x => x.MatchId == item.Id).ToList();

                            foreach (var charge in chargeonprop)
                            {
                                charge.MatchId = item.Id;
                                charge.Rate = (int?)item.Rate;
                                charge.TaxCode = item.TaxCode;
                                charge.Description = item.Description;
                                charge.LastModifiedUserName = item.LastModifiedUserName;
                                charge.CreatedBy = item.CreatedBy;
                                charge.ModifiedBy = item.ModifiedBy;
                            }

                            _db.SaveChanges();
                        }

                        else
                        {
                            item.CreatedOn = DateTime.Now;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                            item.CreatedBy = item.CreatedBy;
                            item.ModifiedBy = item.ModifiedBy;
                            item.LastModifiedUserName = item.LastModifiedUserName;

                            _db.WithHoldingTax.Add(item);
                            _db.SaveChanges();

                            List<WithHoldingTaxPropertyWise> chargesDto = new List<WithHoldingTaxPropertyWise>();

                            var chargeonprop = _db.WithHoldingTaxPropertyWise.ToList().DistinctBy(x => x.StockCreationId);

                            foreach (var fixedcharge in chargeonprop)
                            {
                                 WithHoldingTaxPropertyWise propchargedto = new WithHoldingTaxPropertyWise();

                                 propchargedto.MatchId = item.Id;
                                 propchargedto.RegistrationNo = fixedcharge.RegistrationNo;
                                 propchargedto.PropertyNo = fixedcharge.PropertyNo;
                                 propchargedto.StockCreationId = fixedcharge.StockCreationId;
                                 propchargedto.Rate = (int?)item.Rate;
                                 propchargedto.TaxCode = item.TaxCode;
                                 propchargedto.Description = item.Description;
                                 propchargedto.IsEnabled = false;

                                 chargesDto.Add(propchargedto);
                            }

                            _db.WithHoldingTaxPropertyWise.AddRange(chargesDto);
                            _db.SaveChanges();
                        }
                    }   
                }

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

        [HttpGet]
        [Route("Get")]
        public IActionResult Get(int id)
        {
            try
            {
                var result = _db.WithHoldingTax.Where(x => !x.IsDeleted && x.Id == id)
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
        [Route("AddNewWithHoldingTax")]
        public IActionResult AddNewWithHoldingTax(WithHoldingTax model)
        {
            try
            {
                model.IsActive = true;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = model.CreatedBy;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;

                _db.WithHoldingTax.Add(model);
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
        [Route("UpdateWithHoldingTax")]
        public IActionResult UpdateWithHoldingTax(WithHoldingTax model)
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

                var data = _db.WithHoldingTax.Find(model.Id);

                if (data != null)
                {
                    data.TaxCode = model.TaxCode;
                    data.Rate = model.Rate;
                    data.Description = model.Description;
                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;
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
        [Route("DeleteWithHoldingTax")]
        public IActionResult DeleteWithHoldingTax(int id)
        {
            try
            {
                var model = _db.WithHoldingTax.Find(id);

                if (model != null)
                {
                    model.ModifiedBy = model.ModifiedBy;
                    model.LastModified = DateTime.Now;
                    model.IsActive = false;
                    model.IsDeleted = true;

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
