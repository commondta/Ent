using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using HRMS_Web.Models.DTOs;
using HRMS_Web.Services.SMSService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenralAdjustmentController : ControllerBase
    {
        private readonly DataBase_Context _db;
        private readonly ISMSService _sMSService;
        CommonBLL _commonBLL;
        public GenralAdjustmentController(DataBase_Context db, ISMSService sMSService)
        {
            _db = db;
            _sMSService = sMSService;
            _commonBLL = new CommonBLL(_db);
        }

        [HttpGet]
        [Route("GetAllStandaloneInvoices")]
        public IActionResult GetAllStandaloneInvoices(
            int draw,
            int start,
            int length,
            string? search = ""
        )
        {
            try
            {
                var query = _db.StandAlones
                    .Where(x => !x.IsDeleted && x.Type != null);

                // 🔍 SEARCH (optional)
                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(x =>
                        x.ChallanNo.Contains(search) ||
                        x.MemberProfile.MemberName.Contains(search) ||
                        x.StockCreation.RegistrationNo.Contains(search) ||
                        x.StandAloneCharges.Any(c => c.Remarks != null && c.Remarks.Contains(search))
                    );
                }

                var recordsTotal = query.Count();

                var data = query
                    .OrderByDescending(x => x.Id)
                    .Skip(start)
                    .Take(length)
                    .Select(x => new StandAloneListDTO
                    {
                        Id = x.Id,
                        ChallanNo = x.ChallanNo,
                        Type = x.Type,
                        Status = x.IsActive ? "Active" : $"Cancelled => {x.CancelRemarks}",
                        MemberName = x.MemberProfile != null
                            ? x.MemberProfile.MemberName
                            : string.Empty,

                        ReferenceNo = x.StockCreation != null
                            ? x.StockCreation.RegistrationNo
                            : string.Empty,

                        PropertyNo = x.StockCreation != null
                            ? x.StockCreation.PropertyNo
                            : string.Empty,

                        DocumentDate = x.DocumentDate.HasValue
                            ? x.DocumentDate.Value.ToString("dd-MM-yyyy")
                            : string.Empty,

                        DueDate = x.DueDate.HasValue
                            ? x.DueDate.Value.ToString("dd-MM-yyyy")
                            : string.Empty,

                        TotalAmount = x.StandAloneCharges.Sum(c => c.Amount),
                        Remarks = string.Join(", ",
                                      x.StandAloneCharges
                                          .Select(r => r.Remarks)
                                  ),
                    })
                    .ToList();

                return Ok(new
                {
                    draw = draw,
                    recordsTotal = recordsTotal,
                    recordsFiltered = recordsTotal,
                    data = data
                });
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }


        [HttpGet]
        [Route("GetStandaloneById")]
        public IActionResult GetStandaloneById(int id)
        {
            try
            {
                var sa = _db.StandAlones
                    .Include(x => x.StandAloneCharges)
                    .Include(x => x.StockCreation)
                    .Include(x => x.MemberProfile)
                    .Where(x => !x.IsDeleted && x.Id == id)
                    .FirstOrDefault();

                if (sa == null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Record not found"
                    });
                }

                var result = new StandAloneDetailDTO
                {
                    Id = sa.Id,
                    StockCreationId = sa.StockCreationId,
                    MemberProfileId = sa.MemberProfileId,

                    ChallanNo = sa.ChallanNo,
                    RegistrationNo = sa.RegistrationNo,
                    PropertyNo = sa.StockCreation?.PropertyNo,
                    MemberName = sa.MemberProfile?.MemberName,
                    Cnic = sa.MemberProfile?.Cnic,

                    TypeName = _commonBLL.GetTypeName(Convert.ToInt32(sa.StockCreation.Type)),
                    Block = sa.Block,
                    Category = sa.Category,
                    Size = sa.Size,
                    PossessionStatus = sa.PossessionStatus,
                    ConstrucationStatus = sa.ConstrucationStatus,

                    Type = sa.Type,
                    PaymentMode = sa.PaymentMode,
                    Remarks = sa.Remarks,
                    NameRecipt = sa.NameRecipt,
                    BankAccountDD = sa.BankAccountDD,

                    DocumentDate = sa.DocumentDate?.ToString("yyyy-MM-dd"),
                    DueDate = sa.DueDate?.ToString("yyyy-MM-dd"),

                    ShowOwnerDetails = sa.ShowOwnerDetails,

                    Charges = sa.StandAloneCharges != null
                        ? sa.StandAloneCharges.Select(c => new StandAloneChargeDTO
                        {
                            ChargeName = c.ChargeName,
                            Amount = c.Amount,
                            Remarks = c.Remarks,
                            SapAccount = c.SapAccount,
                            DueDate = null
                        }).ToList()
                        : new List<StandAloneChargeDTO>()
                };

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

        [HttpGet]
        [Route("CancellByChallanNo")]
        public IActionResult CancellByChallanNo(string challanNo, string remarks)
        {
            try
            {
                var sa = _db.StandAlones
                    .Where(x => !x.IsDeleted && x.ChallanNo == challanNo)
                    .FirstOrDefault();

                if (sa == null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Record not found"
                    });
                }

                if (sa.Type == "Challan")
                {
                    Response_Result response_Result = new SapIntegrationController(_db).CancelInvoicesByChallan(challanNo);

                    if (response_Result.code != 0)
                    {

                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.BadRequest,
                            Message = response_Result.message,
                            Data = null
                        });
                    }
                }

                sa.IsActive = false;
                sa.Remarks = remarks;

                _db.SaveChanges();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "All related invoices cancelled successfully",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }


        [HttpGet]
        [Route("GetFilterPropertyByRegistraionNo")]
        public IActionResult GetFilterPropertyByRegistraionNo(string reg)
        {
            try
            {
                var result = _db.StockCreations.Where(x => !x.is_deleted && x.is_active == true && x.RegistrationNo == reg)
                                               .Select(x => new NdcFilterDto
                                               {
                                                   ID = x.ID,
                                                   RegistrationNo = x.RegistrationNo,
                                                   PropertyNo = x.PropertyNo,
                                                   MemberName = x.MemberProfile.MemberName,
                                                   MemberProfileId = x.MemberProfileId,
                                                   Cnic = x.MemberProfile.Cnic,
                                                   CnicExpiryDate = (DateTime)x.MemberProfile.CnicExpiryDate,
                                                   RealStateType = x.RealStateType,
                                                   Phase = x.Phase,
                                                   Project = x.Project,
                                                   Category = x.Category,
                                                   Type = x.Type,
                                                   Nature = x.Nature,
                                                   Block = x.Block,
                                                   ConstructionStatus = x.ConstracutionStatus,
                                                   PossessionStatus = x.PossessionStatus,
                                                   ActualSize = x.ActualSize
                                               })
                                               .FirstOrDefault();
                if (result != null)
                {
                    result.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(result.Category));
                    result.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(result.Block));
                    result.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(result.Type));
                }
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
        [Route("/api/GenralAdjustment/SaveGenralAdjustment")]
        public IActionResult SaveGenralAdjustment(GenralAdjustment model)
        {
            try
            {

                //Response_Result response_Result = new SapIntegrationController(_db).PostingCreditNoteAndARInvoice(model);
                //if (response_Result.code == 0)
                //{
                    model.IsGenralAdjustmentClosed = false;
                    model.IsActive = true;
                    model.CreatedOn = DateTime.Now;
                    model.CreatedBy = model.CreatedBy;
                    model.LastModified = DateTime.Now;
                    model.ModifiedBy = model.ModifiedBy;
                    model.LastModifiedUserName = model.LastModifiedUserName;

                    _db.GenralAdjustments.Add(model);

                    _db.SaveChanges();

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Genral Adjustment Request added succesfully",
                        Data = null
                    });
                //}
                //else
                //{
                //    return Ok(new ApiResponse<object>
                //    {
                //        Code = ResponseCode.BadRequest,
                //        Message = response_Result.message,
                //        Data = null
                //    });
                //}
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        [Route("/api/GenralAdjustment/SaveStandAlone")]
        public async Task<IActionResult> SaveStandAloneAsync(StandAlone model)
        {
            try
            {
                if (model.Type == "Challan")
                {
                    Response_Result response_Result = new SapIntegrationController(_db).PostingStandAloneARInvoice(model);

                    if (response_Result.code != 0)
                    {

                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.BadRequest,
                            Message = response_Result.message,
                            Data = null
                        });
                    }
                }

                model.IsStandAloneClosed = false;
                model.IsActive = true;
                model.CreatedOn = (DateTime)model.DocumentDate;
                model.CreatedBy = model.CreatedBy;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;

                _db.StandAlones.Add(model);
                _db.SaveChanges();

                if (model.PaymentMode == "DD")
                {
                    var stock = _db.StockCreations.Where(x => x.ID == model.StockCreationId)
                        .Include(x => x.MemberProfile)
                        .Select(x => new
                        {
                            x.RegistrationNo,
                            x.PropertyNo,
                            x.MemberProfile.Mobile,
                            x.MemberProfile.MemberName
                        })
                        .FirstOrDefault();

                    if (stock != null && !string.IsNullOrEmpty(stock.Mobile))
                    {
                        string message =
                            $"Dear {stock.MemberName}, Thank you for your DD/PO received at DHAB. " +
                            $"Your website a/c will be updated after bank confirmation. " +
                            $"For help, please call UAN# +92 62 111 111 518.";

                        try
                        {
                            await _sMSService.SendSingleSmsAsync(message, stock.Mobile);
                        }
                        catch (Exception ex)
                        {
                            // log exception
                        }
                    }
                }


                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = model.Type == "Challan" ? "Stand Alone Request added successfully" : "Data is saved Only",
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
