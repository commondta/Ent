using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SAPOperationsController : ControllerBase
    {
        private readonly DataBase_Context _db;

        CommonBLL _commonBLL;
        ApprovalBLL _approvalBLL;
        public SAPOperationsController(DataBase_Context db)
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
                var result = _db.SAPOperations.Where(x => !x.IsDeleted)
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
        public IActionResult Get()
        {
            try
            {
                var result = _db.SAPOperations.Where(x => !x.IsDeleted)
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
        [Route("AddNewSAPOperations")]
        public IActionResult AddNewSAPOperations(SAPOperations model)
        {
            try
            {
                model.IsActive = true;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = model.CreatedBy;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;

                _db.SAPOperations.Add(model);
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

        [HttpPost]
        [Route("UpdateSAPOperations")]
        public IActionResult UpdateSAPOperations(SAPOperations model)
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

                var data = _db.SAPOperations.Find(model.Id);

                if (data != null)
                {
                    data.Server = model.Server;
                    data.DBName = model.DBName;
                    data.DBUserName = model.DBUserName;
                    data.DBPassword = model.DBPassword;
                    data.SAPUser = model.SAPUser;
                    data.SAPPassword = model.SAPPassword;
                    data.CustomerSeries = model.CustomerSeries;
                    data.MemberAccountCode = model.MemberAccountCode;
                    data.DealerAccountCode = model.DealerAccountCode;
                    data.BookingAccount = model.BookingAccount;
                    data.DBType = model.DBType;
                    data.TownPlanningClearanceCommaSepratedGLs = model.TownPlanningClearanceCommaSepratedGLs;
                    data.BillDiscountPercentage = model.BillDiscountPercentage;
                    data.SignatoryRank = model.SignatoryRank;
                    data.FingerPrintThreshhold = model.FingerPrintThreshhold;
                    data.SignatoryDesignation = model.SignatoryDesignation;
                    data.SignatoryName = model.SignatoryName;
                    data.AllocationSignatoryRank = model.AllocationSignatoryRank;
                    data.AllocationSignatoryName = model.AllocationSignatoryName;
                    data.AllocationSignatoryDesignation = model.AllocationSignatoryDesignation;
                    data.TransferCertificateTimeLineStatement = model.TransferCertificateTimeLineStatement;
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
        [Route("DeleteSAPOperations")]
        public IActionResult DeleteSAPOperations(int id)
        {
            try
            {
                var model = _db.SAPOperations.Find(id);

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
