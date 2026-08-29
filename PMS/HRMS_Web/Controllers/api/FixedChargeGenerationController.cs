using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using HRMS_Web.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System.Web.Http;
using HttpDeleteAttribute = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [System.Web.Http.Authorize]
    public class FixedChargeGenerationController : ControllerBase
    {
        private readonly DataBase_Context _db;

        CommonBLL _commonBLL;
        ApprovalBLL _approvalBLL;
        public FixedChargeGenerationController(DataBase_Context db)
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
                var result = _db.MeterBillGeneration.Where(x => !x.IsDeleted)
                                                       .Include(x => x.MeterBillGenerationDetail.Where(x => !x.IsDeleted))
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
                var result = _db.MeterBillGeneration.Where(x => !x.IsDeleted && x.Id == id)
                                                    .Include(x => x.MeterBillGenerationDetail.Where(x => !x.IsDeleted))
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
        [Route("GetHeads")]
        public IActionResult GetHeads(string month)
        {
            try
            {
                var result = from a in _db.FixedChargeBillDetail
                             join b in _db.FixedChargeBill on a.FixedChargeBillId equals b.Id
                             where b.Month == month
                             group a by a.Description into grouped
                             select new
                             {
                                 Description = grouped.Key,
                                 TotalAmount = grouped.Sum(a => a.Amount)
                             };


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

        //Todo : RunConstructed

        [HttpGet]
        [Route("AddNewMonthlyFixedChargesRunConstructed")]
        public IActionResult AddNewMonthlyFixedChargesRunConstructed(string month, string gracePeriodDate)
        {
            try
            {
                var isExist = _db.FixedChargeBill.Where(x => x.Month == month &&
                                                            x.BillFor == "Fixed Dues")
                                                     .FirstOrDefault();

                DateTime gracedate = Convert.ToDateTime(gracePeriodDate);

                if (isExist != null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "Detail Already Exist.",
                        Data = null
                    });
                }

                var properties = _db.StockCreations.Where(x => x.MemberProfileId != null &&
                                                          x.IsBillGenerationEnabled == true &&
                                                          x.PossessionStatus == true &&
                                                          //x.PropertyNo != null &&
                                                          x.PropertyNo != "" &&
                                                          x.RegistrationNo != null &&
                                                          x.RegistrationNo != "" &&
                                                          x.ConstracutionStatus == "Constructed"
                                                          //&& x.DemarcationExpireOn < gracedate.Date
                                                          )
                                                   .ToList();
                foreach (var item in properties)
                {


                    GlobalChargeSetupDetailFixedChargFilterDTO model = new GlobalChargeSetupDetailFixedChargFilterDTO()
                    {
                        FormId = 5,
                        RealStateTypeId = Convert.ToInt32(item.RealStateType),
                        ProjectId = Convert.ToInt32(item.Project),
                        PhaseId = Convert.ToInt32(item.Phase),
                        BlockId = Convert.ToInt32(item.Block),
                        CategoryId = Convert.ToInt32(item.Category),
                        PropertyTypeId = Convert.ToInt32(item.Type),
                        NatureId = Convert.ToInt32(item.Nature),
                        PossessionStatus = item.PossessionStatus,
                        ConstructionStatus = item.ConstracutionStatus,
                        GracePeriod = item.GrancePeriodForBillGenration < gracedate.Date ? false : true,
                    };

                    List<GlobalChargeDetail> chargeDetails = GetGlobalChargeDetail(model);

                    if (chargeDetails.Count() > 0)
                    {
                        int wTaxAmount = 0;

                        FixedChargeBill individualBill = new FixedChargeBill();

                        individualBill.BillFor = "Constructed";
                        individualBill.Month = month;
                        individualBill.RegistrationNo = item.RegistrationNo;
                        individualBill.StockCreationID = item.ID;
                        individualBill.CreatedOn = DateTime.Now;
                        individualBill.LastModified = DateTime.Now;
                        individualBill.IsActive = true;
                        individualBill.IsDeleted = false;

                        List<FixedChargeBillDetail> individualBillDetails = new List<FixedChargeBillDetail>();
                        List<FixedChargeBillWHApplied> fixedChargeBillWHApplieds = new List<FixedChargeBillWHApplied>();
                        if (chargeDetails.Count() > 0)
                        {
                            foreach (var charge in chargeDetails)
                            {
                                ChargeDiscountDTO chargeSetup = ChargeEnable(charge.Id, item.ID);

                                if (chargeSetup.IsEnabled == true)
                                {
                                    decimal chargeAmount = 0;
                                    decimal salaTaxAmount = 0;
                                    decimal discount = 0;

                                    double actualSize = double.Parse(item.ActualSize);
                                    chargeAmount = charge.MultiplyBySize == true ? (decimal)(Convert.ToDecimal(chargeSetup.Rate) * Convert.ToDecimal(actualSize)) : Convert.ToDecimal(chargeSetup.Rate) * (int)chargeSetup.Unit;

                                    if (item.IsSaleTaxEnabled == true && charge.Status == true)
                                    {
                                        if (charge.MultiplyBySize == true)
                                        {
                                            salaTaxAmount = (chargeAmount * GetSaleTax(item.ConstracutionStatus) / 100);
                                        }
                                        else
                                        {
                                            salaTaxAmount = (chargeAmount * GetSaleTax(item.ConstracutionStatus) / 100);
                                        }
                                    }

                                    decimal chargeNetAmount = chargeAmount + salaTaxAmount;
                                    decimal chargeNetAmountDiscounted = chargeNetAmount - (int)chargeSetup.ChargeDiscount;

                                    decimal wTaxAmountLine = 0;

                                    if (item.IsWithHoldingTaxEnabled == true && charge.WHStatus == true)
                                    {
                                        wTaxAmountLine = (chargeNetAmountDiscounted * GetWTax(item.ID) / 100);

                                        List<WHTaxAplliedDTO> charges = GetEnabledWHTax(item.ID);

                                        if (charges.Count() > 0)
                                        {
                                            foreach (var itemcharge in charges)
                                            {
                                                FixedChargeBillWHApplied dtos = new FixedChargeBillWHApplied();

                                                dtos.RegistrationNo = item.RegistrationNo;
                                                dtos.Month = month;
                                                dtos.NetAmount = chargeNetAmountDiscounted;
                                                dtos.WHPercentage = (int)itemcharge.Rate;
                                                dtos.Amount = (chargeNetAmountDiscounted * (int)itemcharge.Rate / 100);
                                                dtos.TaxCode = itemcharge.TaxCode;

                                                fixedChargeBillWHApplieds.Add(dtos);
                                            }
                                        }
                                    }

                                    FixedChargeBillDetail dto = new FixedChargeBillDetail()
                                    {
                                        BillType = "Constructed",
                                        Description = charge.Description,
                                        SapAccount = _commonBLL.GetSapAccountByChargeTypeId(Convert.ToInt32(charge.ChargeType)),
                                        Unit = charge.MultiplyBySize == true ? Convert.ToDecimal(actualSize) : chargeSetup.Unit,
                                        Amount = Convert.ToDecimal(chargeSetup.Rate),
                                        Surcharge = 0,
                                        OtherDuesDescription = "",
                                        OtherDuesAmount = 0,
                                        SaleTax = (int)GetSaleTax(item.ConstracutionStatus),
                                        SaleTaxAmount = salaTaxAmount,
                                        WTaxAmountLine = wTaxAmountLine,
                                        GrossAmount = Convert.ToDecimal(chargeNetAmount),
                                        Discount = (int)chargeSetup.ChargeDiscount,
                                        NetAmount = Convert.ToDecimal(chargeNetAmountDiscounted),
                                        ChargeTypeId = Convert.ToInt32(charge.ChargeType)
                                    };

                                    individualBillDetails.Add(dto);
                                }
                            }
                        }

                        decimal sumNetAmount = individualBillDetails.Sum(x => x.NetAmount);
                        decimal WHTaxAmount = individualBillDetails.Sum(x => x.WTaxAmountLine);

                        //individualBill.TotalAmount = sumNetAmount + WHTaxAmount;
                        individualBill.TotalAmount = sumNetAmount;
                        individualBill.WTaxAmount = WHTaxAmount;

                        individualBill.FixedChargeBillDetail = individualBillDetails;
                        individualBill.FixedChargeBillWHApplied = fixedChargeBillWHApplieds;

                        _db.FixedChargeBill.Add(individualBill);
                        _db.SaveChanges();
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

        //end

        //Todo : RunNonConstructed

        [HttpGet]
        [Route("AddNewMonthlyFixedChargesRunNonConstructed")]
        public IActionResult AddNewMonthlyFixedChargesRunNonConstructed(string month, string gracePeriodDate)
        {
            try
            {
                var isExist = _db.FixedChargeBill.Where(x => x.Month == month &&
                                                            x.BillFor == "Fixed Dues")
                                                     .FirstOrDefault();

                DateTime gracedate = Convert.ToDateTime(gracePeriodDate);

                //if (isExist != null)
                //{
                //    return Ok(new ApiResponse<object>
                //    {
                //        Code = ResponseCode.BadRequest,
                //        Message = "Detail Already Exist! You can only update them",
                //        Data = null
                //    });
                //}

                var properties = _db.StockCreations.Where(x => x.MemberProfileId != null &&
                                                          x.IsBillGenerationEnabled == true &&
                                                          x.PossessionStatus == true &&
                                                          x.PropertyNo != null &&
                                                          x.PropertyNo != "" &&
                                                          x.RegistrationNo != null &&
                                                          x.RegistrationNo != "" &&
                                                          x.ConstracutionStatus == "Non-Constructed" &&
                                                          x.DemarcationExpireOn < gracedate.Date
                                                          )
                                                   .ToList();
                foreach (var item in properties)
                {

                    GlobalChargeSetupDetailFixedChargFilterDTO model = new GlobalChargeSetupDetailFixedChargFilterDTO()
                    {
                        FormId = 5,
                        RealStateTypeId = Convert.ToInt32(item.RealStateType),
                        ProjectId = Convert.ToInt32(item.Project),
                        PhaseId = Convert.ToInt32(item.Phase),
                        BlockId = Convert.ToInt32(item.Block),
                        CategoryId = Convert.ToInt32(item.Category),
                        PropertyTypeId = Convert.ToInt32(item.Type),
                        NatureId = Convert.ToInt32(item.Nature),
                        PossessionStatus = item.PossessionStatus,
                        ConstructionStatus = item.ConstracutionStatus,
                        GracePeriod = item.GrancePeriodForBillGenration < gracedate.Date ? false : true,
                    };

                    List<GlobalChargeDetail> chargeDetails = GetGlobalChargeDetail(model);

                    if (chargeDetails.Count() > 0)
                    {
                        int wTaxAmount = 0;

                        FixedChargeBill individualBill = new FixedChargeBill();

                        individualBill.BillFor = "Non-Constructed";
                        individualBill.Month = month;
                        individualBill.RegistrationNo = item.RegistrationNo;
                        individualBill.StockCreationID = item.ID;
                        individualBill.CreatedOn = DateTime.Now;
                        individualBill.LastModified = DateTime.Now;
                        individualBill.IsActive = true;
                        individualBill.IsDeleted = false;

                        List<FixedChargeBillDetail> individualBillDetails = new List<FixedChargeBillDetail>();
                        List<FixedChargeBillWHApplied> fixedChargeBillWHApplieds = new List<FixedChargeBillWHApplied>();
                        if (chargeDetails.Count() > 0)
                        {
                            foreach (var charge in chargeDetails)
                            {
                                ChargeDiscountDTO chargeSetup = ChargeEnable(charge.Id, item.ID);

                                if (chargeSetup.IsEnabled == true)
                                {
                                    decimal chargeAmount = 0;
                                    decimal salaTaxAmount = 0;
                                    decimal discount = 0;

                                    double actualSize = double.Parse(item.ActualSize);
                                    chargeAmount = charge.MultiplyBySize == true ? (decimal)(Convert.ToDecimal(chargeSetup.Rate) * Convert.ToDecimal(actualSize)) : Convert.ToDecimal(chargeSetup.Rate) * (int)chargeSetup.Unit;

                                    if (item.IsSaleTaxEnabled == true && charge.Status == true)
                                    {
                                        if (charge.MultiplyBySize == true)
                                        {
                                            salaTaxAmount = (chargeAmount * GetSaleTax(item.ConstracutionStatus) / 100);
                                        }
                                        else
                                        {
                                            salaTaxAmount = (chargeAmount * GetSaleTax(item.ConstracutionStatus) / 100);
                                        }
                                    }

                                    decimal chargeNetAmount = chargeAmount + salaTaxAmount;
                                    decimal chargeNetAmountDiscounted = chargeNetAmount - (int)chargeSetup.ChargeDiscount;

                                    decimal wTaxAmountLine = 0;

                                    if (item.IsWithHoldingTaxEnabled == true && charge.WHStatus == true)
                                    {
                                        wTaxAmountLine = (chargeNetAmountDiscounted * GetWTax(item.ID) / 100);

                                        List<WHTaxAplliedDTO> charges = GetEnabledWHTax(item.ID);

                                        if (charges.Count() > 0)
                                        {
                                            foreach (var itemcharge in charges)
                                            {
                                                FixedChargeBillWHApplied dtos = new FixedChargeBillWHApplied();

                                                dtos.RegistrationNo = item.RegistrationNo;
                                                dtos.Month = month;
                                                dtos.NetAmount = chargeNetAmountDiscounted;
                                                dtos.WHPercentage = (int)itemcharge.Rate;
                                                dtos.Amount = (chargeNetAmountDiscounted * (int)itemcharge.Rate / 100);
                                                dtos.TaxCode = itemcharge.TaxCode;

                                                fixedChargeBillWHApplieds.Add(dtos);
                                            }
                                        }
                                    }

                                    FixedChargeBillDetail dto = new FixedChargeBillDetail()
                                    {
                                        BillType = "Non-Constructed",
                                        Description = charge.Description,
                                        SapAccount = _commonBLL.GetSapAccountByChargeTypeId(Convert.ToInt32(charge.ChargeType)),
                                        Unit = charge.MultiplyBySize == true ? Convert.ToDecimal(actualSize) : chargeSetup.Unit,
                                        Amount = Convert.ToDecimal(chargeSetup.Rate),
                                        Surcharge = 0,
                                        OtherDuesDescription = "",
                                        OtherDuesAmount = 0,
                                        SaleTax = (int)GetSaleTax(item.ConstracutionStatus),
                                        SaleTaxAmount = salaTaxAmount,
                                        WTaxAmountLine = wTaxAmountLine,
                                        GrossAmount = Convert.ToDecimal(chargeNetAmount),
                                        Discount = (int)chargeSetup.ChargeDiscount,
                                        NetAmount = Convert.ToDecimal(chargeNetAmountDiscounted),
                                        ChargeTypeId = Convert.ToInt32(charge.ChargeType)
                                    };

                                    individualBillDetails.Add(dto);
                                }
                            }
                        }

                        decimal sumNetAmount = individualBillDetails.Sum(x => x.NetAmount);
                        decimal WHTaxAmount = individualBillDetails.Sum(x => x.WTaxAmountLine);

                        //individualBill.TotalAmount = sumNetAmount + WHTaxAmount;
                        individualBill.TotalAmount = sumNetAmount;
                        individualBill.WTaxAmount = WHTaxAmount;

                        individualBill.FixedChargeBillDetail = individualBillDetails;
                        individualBill.FixedChargeBillWHApplied = fixedChargeBillWHApplieds;

                        _db.FixedChargeBill.Add(individualBill);
                        _db.SaveChanges();
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

        //end

        [HttpGet]
        [Route("AddNewMonthlyFixedCharges")]
        public IActionResult AddNewMonthlyFixedCharges(string month, string gracePeriodDate)
        {
            try
            {
                var isExist = _db.FixedChargeBill.Where(x => x.Month == month &&
                                                            x.BillFor == "Fixed Dues")
                                                     .FirstOrDefault();

                DateTime gracedate = Convert.ToDateTime(gracePeriodDate);

                if (isExist != null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "Detail Already Exist! You can only update them",
                        Data = null
                    });
                }

                var properties = _db.StockCreations.Where(x => x.MemberProfileId != null &&
                                                          x.IsBillGenerationEnabled == true &&
                                                          x.PossessionStatus == true &&
                                                          x.PropertyNo != null &&
                                                          x.PropertyNo != "" &&
                                                          x.RegistrationNo != null &&
                                                          x.RegistrationNo != "" &&
                                                          x.DemarcationExpireOn < gracedate.Date
                                                          )
                                                   .ToList();
                foreach (var item in properties)
                {


                    GlobalChargeSetupDetailFixedChargFilterDTO model = new GlobalChargeSetupDetailFixedChargFilterDTO()
                    {
                        FormId = 5,
                        RealStateTypeId = Convert.ToInt32(item.RealStateType),
                        ProjectId = Convert.ToInt32(item.Project),
                        PhaseId = Convert.ToInt32(item.Phase),
                        BlockId = Convert.ToInt32(item.Block),
                        CategoryId = Convert.ToInt32(item.Category),
                        PropertyTypeId = Convert.ToInt32(item.Type),
                        NatureId = Convert.ToInt32(item.Nature),
                        PossessionStatus = item.PossessionStatus,
                        ConstructionStatus = item.ConstracutionStatus,
                        GracePeriod = item.GrancePeriodForBillGenration < gracedate.Date ? false : true,
                    };

                    List<GlobalChargeDetail> chargeDetails = GetGlobalChargeDetail(model);

                    if (chargeDetails.Count() > 0)
                    {
                        int wTaxAmount = 0;

                        FixedChargeBill individualBill = new FixedChargeBill();

                        individualBill.BillFor = "Fixed Dues";
                        individualBill.Month = month;
                        individualBill.RegistrationNo = item.RegistrationNo;
                        individualBill.StockCreationID = item.ID;
                        individualBill.CreatedOn = DateTime.Now;
                        individualBill.LastModified = DateTime.Now;
                        individualBill.IsActive = true;
                        individualBill.IsDeleted = false;

                        List<FixedChargeBillDetail> individualBillDetails = new List<FixedChargeBillDetail>();
                        List<FixedChargeBillWHApplied> fixedChargeBillWHApplieds = new List<FixedChargeBillWHApplied>();
                        if (chargeDetails.Count() > 0)
                        {
                            foreach (var charge in chargeDetails)
                            {
                                ChargeDiscountDTO chargeSetup = ChargeEnable(charge.Id, item.ID);

                                if (chargeSetup.IsEnabled == true)
                                {
                                    decimal chargeAmount = 0;
                                    decimal salaTaxAmount = 0;
                                    decimal discount = 0;

                                    double actualSize = double.Parse(item.ActualSize);
                                    chargeAmount = charge.MultiplyBySize == true ? (decimal)(Convert.ToDecimal(chargeSetup.Rate) * Convert.ToDecimal(actualSize)) : Convert.ToDecimal(chargeSetup.Rate) * (int)chargeSetup.Unit;

                                    if (item.IsSaleTaxEnabled == true && charge.Status == true)
                                    {
                                        if (charge.MultiplyBySize == true)
                                        {
                                            salaTaxAmount = (chargeAmount * GetSaleTax(item.ConstracutionStatus) / 100);
                                        }
                                        else
                                        {
                                            salaTaxAmount = (chargeAmount * GetSaleTax(item.ConstracutionStatus) / 100);
                                        }
                                    }

                                    decimal chargeNetAmount = chargeAmount + salaTaxAmount;
                                    decimal chargeNetAmountDiscounted = chargeNetAmount - (int)chargeSetup.ChargeDiscount;

                                    decimal wTaxAmountLine = 0;

                                    if (item.IsWithHoldingTaxEnabled == true && charge.WHStatus == true)
                                    {
                                        wTaxAmountLine = (chargeNetAmountDiscounted * GetWTax(item.ID) / 100);

                                        List<WHTaxAplliedDTO> charges = GetEnabledWHTax(item.ID);

                                        if (charges.Count() > 0)
                                        {
                                            foreach (var itemcharge in charges)
                                            {
                                                FixedChargeBillWHApplied dtos = new FixedChargeBillWHApplied();

                                                dtos.RegistrationNo = item.RegistrationNo;
                                                dtos.Month = month;
                                                dtos.NetAmount = chargeNetAmountDiscounted;
                                                dtos.WHPercentage = (int)itemcharge.Rate;
                                                dtos.Amount = (chargeNetAmountDiscounted * (int)itemcharge.Rate / 100);
                                                dtos.TaxCode = itemcharge.TaxCode;

                                                fixedChargeBillWHApplieds.Add(dtos);
                                            }
                                        }
                                    }

                                    FixedChargeBillDetail dto = new FixedChargeBillDetail()
                                    {
                                        BillType = "Fixed Dues",
                                        Description = charge.Description,
                                        SapAccount = _commonBLL.GetSapAccountByChargeTypeId(Convert.ToInt32(charge.ChargeType)),
                                        Unit = charge.MultiplyBySize == true ? Convert.ToDecimal(actualSize) : chargeSetup.Unit,
                                        Amount = Convert.ToDecimal(chargeSetup.Rate),
                                        Surcharge = 0,
                                        OtherDuesDescription = "",
                                        OtherDuesAmount = 0,
                                        SaleTax = (int)GetSaleTax(item.ConstracutionStatus),
                                        SaleTaxAmount = salaTaxAmount,
                                        WTaxAmountLine = wTaxAmountLine,
                                        GrossAmount = Convert.ToDecimal(chargeNetAmount),
                                        Discount = (int)chargeSetup.ChargeDiscount,
                                        NetAmount = Convert.ToDecimal(chargeNetAmountDiscounted),
                                        ChargeTypeId = Convert.ToInt32(charge.ChargeType)
                                    };

                                    individualBillDetails.Add(dto);
                                }
                            }
                        }

                        decimal sumNetAmount = individualBillDetails.Sum(x => x.NetAmount);
                        decimal WHTaxAmount = individualBillDetails.Sum(x => x.WTaxAmountLine);

                        //individualBill.TotalAmount = sumNetAmount + WHTaxAmount;
                        individualBill.TotalAmount = sumNetAmount;
                        individualBill.WTaxAmount = WHTaxAmount;

                        individualBill.FixedChargeBillDetail = individualBillDetails;
                        individualBill.FixedChargeBillWHApplied = fixedChargeBillWHApplieds;

                        _db.FixedChargeBill.Add(individualBill);
                        _db.SaveChanges();
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
        public List<GlobalChargeDetail> GetGlobalChargeDetail(GlobalChargeSetupDetailFixedChargFilterDTO dto)
        {
            var globalChargeGroupIds = _db.FormsChargeGroup
                .Where(x => x.FormId == dto.FormId && !x.IsDeleted)
                .Select(x => x.ChargeGroupId)
                .ToList();

            var globalChargeSetupDetails = new List<GlobalChargeDetail>();

            if (globalChargeGroupIds.Any())
            {
                foreach (var groupId in globalChargeGroupIds)
                {
                    var globalChargeSetupsDetail = _db.GlobalChargeSetup
                        .Where(x => !x.IsDeleted
                            && x.GlobalChargeGroupId == groupId
                            && (x.RealStateTypeId == dto.RealStateTypeId || x.RealStateTypeId == null)
                            && (x.ProjectId == dto.ProjectId || x.ProjectId == null)
                            && (x.PhaseId == dto.PhaseId || x.PhaseId == null || x.PhaseId == -1)
                            && (x.BlockId == dto.BlockId || x.BlockId == null)
                            && (x.CategoryId == dto.CategoryId || x.CategoryId == null)
                            && (x.PropertyTypeId == dto.PropertyTypeId || x.PropertyTypeId == null)
                            && (x.NatureId == dto.NatureId || x.NatureId == null)
                            && x.PossessionStatus == dto.PossessionStatus
                            && (x.ConstructionStatus == dto.ConstructionStatus || x.ConstructionStatus == null)
                            && (x.GracePeriod == dto.GracePeriod || x.GracePeriod == null)
                        )
                        .SelectMany(x => x.GlobalChargeDetail)
                        .Where(x => !x.IsDeleted)
                        .ToList();

                    if (globalChargeSetupsDetail.Any())
                    {
                        globalChargeSetupDetails.AddRange(globalChargeSetupsDetail);
                    }
                }
            }

            return globalChargeSetupDetails;
        }

        public decimal GetSaleTax(string constracutionStatus)
        {
            var saleTax = constracutionStatus == "Constructed" ? _db.SaleTax.SingleOrDefault()?.RateConstructed : _db.SaleTax.SingleOrDefault().RateNonConstructed;
            return saleTax ?? 0; // return 0 if gracePeriod is null
        }

        public int GetWTax(int id)
        {
            var wTax = _db.WithHoldingTaxPropertyWise
                             .Where(x => x.StockCreationId == id && x.IsEnabled == true)
                             .Select(x => x.Rate)
                             .ToList();

            if (wTax.Count() == 0)
            {
                return 0;
            }

            int sumOfRates = (int)wTax.Sum(); // convert the sum to an integer
            return sumOfRates;
        }

        public ChargeDiscountDTO ChargeEnable(int id, int stockId)
        {
            var chargeSetup = _db.PropertyFixedChargesSetup
                .Where(x => x.MatchId == id && x.StockCreationId == stockId && x.IsEnabled == true)
                .Select(x => new ChargeDiscountDTO
                {
                    IsEnabled = (bool)x.IsEnabled,
                    ChargeDiscount = (int)x.Discount,
                    Unit = (int)x.Unit,
                    Rate = x.Rate
                })
                .FirstOrDefault();

            return chargeSetup ?? new ChargeDiscountDTO();
        }

        public List<WHTaxAplliedDTO> GetEnabledWHTax(int stockId)
        {
            var chargeSetup = _db.WithHoldingTaxPropertyWise
                .Where(x => x.StockCreationId == stockId && x.IsEnabled == true)
                .Select(x => new WHTaxAplliedDTO
                {
                    TaxCode = x.TaxCode,
                    Rate = x.Rate
                })
                .ToList();

            return chargeSetup ?? new List<WHTaxAplliedDTO>();
        }

        // Discard
        [HttpDelete]
        [Route("Discard")]
        public IActionResult Discard(string month, string billFor)
        {
            var sapPost = _db.SAPBillPostingCheck.Where(x => x.Month == month && x.BillFor == billFor).FirstOrDefault();
            if (sapPost == null)
            {
                var bill = _db.FixedChargeBill.Where(x => x.Month == month && x.BillFor == billFor).FirstOrDefault();

                if (bill != null)
                {

                    var fixedChargeBill = _db.FixedChargeBill.Where(x => x.Month == month && x.BillFor == billFor)
                                                             .Include(x => x.FixedChargeBillDetail)
                                                             .Include(x => x.FixedChargeBillWHApplied)
                                                             .ToList();
                    if (fixedChargeBill.Count() > 0)
                    {
                        foreach (var charge in fixedChargeBill)
                        {
                            _db.FixedChargeBillDetail.RemoveRange(charge.FixedChargeBillDetail);
                            _db.FixedChargeBillWHApplied.RemoveRange(charge.FixedChargeBillWHApplied);
                            _db.FixedChargeBill.Remove(charge);
                            _db.SaveChanges();

                        }
                    }
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Fixed Charges Record Discard For This Month Successfully",
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
            }
            return Ok(new ApiResponse<object>
            {
                Code = ResponseCode.Conflict,
                Message = "You can't discard beacuse its already posted in SAP",
                Data = null
            });
        }
    }
}
