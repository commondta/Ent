using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common.Enums;
using B_Utility.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HRMS_Web.Models.DTOs;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Web.Http.Results;
using HRMS_Web.Services.AlertService;
using HRMS_Web.Extensions;
using B_DB_Context.Migrations;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransferReceiptProcessingController : ControllerBase
    {
        private readonly DataBase_Context _db;
        private readonly IAlertService alertService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        CommonBLL _commonBLL;
        ApprovalBLL _approvalBLL;
        public TransferReceiptProcessingController(DataBase_Context db, IAlertService alertService, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            this.alertService = alertService;
            _httpContextAccessor = httpContextAccessor;
            _approvalBLL = new ApprovalBLL(_db);
            _commonBLL = new CommonBLL(_db);
        }

        [HttpGet]
        [Route("GetOpenTransferAgreement")]
        public IActionResult GetOpenTransferAgreement(int id)
        {
            try
            {

                var result = _db.NDCRequestForMember.Where(x => x.Id == id)
                                                .Include(x => x.MemberProfile)
                                                .Include(x => x.StockCreation)
                                                .Select(x => new ownershipAgreementDto
                                                {
                                                    SellerName = x.MemberProfile.MemberName,
                                                    DealerName = x.DealerName,
                                                    EstateName = x.EstateName,
                                                    DealerCnic = _db.Dealers.Where(d => d.Id == Convert.ToInt32(x.DealerCode)).FirstOrDefault().CNIC,
                                                    RelationshipSeller = x.MemberProfile.Relationship,
                                                    RelationshipWithSeller = x.MemberProfile.RelationshipWith,
                                                    RelationshipBuyer = x.MemberProfile.Relationship,
                                                    RelationshipWithBuyer = x.MemberProfile.RelationshipWith,
                                                    BuyerName = x.MemberProfile.MemberName,
                                                    BuyerCnic = x.MemberProfile.Cnic,
                                                    SellerCnic = x.MemberProfile.Cnic,
                                                    DealerRegistrationNo = _db.Dealers.Where(d => d.Id == Convert.ToInt32(x.DealerCode)).FirstOrDefault().DelaerRegisrationCode,
                                                    PermanentAddress = x.MemberProfile.PermanentAddress,
                                                    RegistrationNo = x.StockCreation.RegistrationNo,
                                                    PropertyNo = x.StockCreation.PropertyNo,
                                                    Area = x.StockCreation.ActualSize,
                                                    UnitArea = x.StockCreation.ActualSizeUnit,
                                                    Sqft = x.StockCreation.coveredArea == null ? "N/A" : x.StockCreation.coveredArea.ToString(),
                                                    Type = _db.PropertyTypes.Where(y => y.ID == Convert.ToInt32(x.StockCreation.Type)).FirstOrDefault().Description ?? "N/A",
                                                    Block = _db.Blocks.Where(y => y.ID == Convert.ToInt32(x.StockCreation.Block)).FirstOrDefault().Description ?? "N/A",
                                                    Nature = _db.Natures.Where(y => y.ID == Convert.ToInt32(x.StockCreation.Nature)).FirstOrDefault().Description == "Plot" && x.StockCreation.ConstracutionStatus == "Constructed" ? "House" : _db.Natures.Where(y => y.ID == Convert.ToInt32(x.StockCreation.Nature)).FirstOrDefault().Description,
                                                    Phase = _db.Phases.Where(y => y.ID == Convert.ToInt32(x.StockCreation.Phase)).FirstOrDefault().Description ?? "N/A",
                                                    SectorName = _db.Sectors.Where(y => y.ID == Convert.ToInt32(x.StockCreation.PrefixProperty)).FirstOrDefault().Description ?? "N/A",
                                                    OpenSlotDate = x.SlotDate,
                                                    ApplyStation = x.ApplyStation,
                                                    TransferType = x.TransferType.Description,
                                                })
                                                .FirstOrDefault();

                return Ok(result);

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }


        [HttpGet]
        [Route("GetOwnershipAgreement")]
        public IActionResult GetOwnershipAgreement(int id, string? transferType = "")
        {
            try
            {
                //var ImagesUrls = GetAllImagesUrl(id);
                // var MemberName = GetAllMemberNames(id);
                var result = _db.TransferReceiptProcessing.Where(x => x.Id == id)
                                                .Include(x => x.StockCreation)
                                                .Select(x => new ownershipAgreementDto
                                                {
                                                    StockId = x.StockCreationId,
                                                    BuyerId = x.BuyerId,
                                                    SellerId = x.SellerId,
                                                    SellerName = x.SellerName,
                                                    DealerName = x.DealerName,
                                                    EstateName = x.EstateName,
                                                    DealerCnic = _db.Dealers.Where(d => d.Id == Convert.ToInt32(x.DealerCode)).FirstOrDefault().CNIC,
                                                    BuyerName = x.BuyerName,
                                                    BuyerCnic = x.CNIC,
                                                    DealerRegistrationNo = x.DealerCode,
                                                    RegistrationNo = x.StockCreation.RegistrationNo ?? "N/A",
                                                    PropertyNo = x.StockCreation.PropertyNo ?? "N/A",
                                                    Area = $"{x.StockCreation.ActualSize ?? "N/A"} {x.StockCreation.ActualSizeUnit}",
                                                    UnitArea = x.StockCreation.ActualSizeUnit ?? "N/A",
                                                    Sqft = x.StockCreation.coveredArea == null ? "N/A" : x.StockCreation.coveredArea.ToString(),
                                                    Phase = _db.Phases.Where(y => y.ID == Convert.ToInt32(x.StockCreation.Phase)).FirstOrDefault().Description ?? "N/A",
                                                    Type = _db.PropertyTypes.Where(y => y.ID == Convert.ToInt32(x.StockCreation.Type)).FirstOrDefault().Description ?? "N/A",
                                                    Block = _db.Blocks.Where(y => y.ID == Convert.ToInt32(x.StockCreation.Block)).FirstOrDefault().Description ?? "N/A",
                                                    Nature = _db.Natures.Where(y => y.ID == Convert.ToInt32(x.StockCreation.Nature)).FirstOrDefault().Description == "Plot" && x.StockCreation.ConstracutionStatus == "Constructed" ? "House" : _db.Natures.Where(y => y.ID == Convert.ToInt32(x.StockCreation.Nature)).FirstOrDefault().Description,
                                                    SectorName = _db.Sectors.Where(y => y.ID == Convert.ToInt32(x.StockCreation.PrefixProperty)).FirstOrDefault().Description ?? "N/A",
                                                    docDate = x.CreatedOn.Date,
                                                    ApplyStation = x.ApplyStation ?? "N/A",
                                                    BuyerRepresentativeName = x.BuyerRepresentativeName ?? "N/A",
                                                    BuyerRepresentativeRelationshipWith = x.BuyerRepresentativeRelationshipWith ?? "N/A",
                                                    BuyerRepresentativeCnic = x.BuyerRepresentativeCnic ?? "N/A",
                                                    TransferType = string.IsNullOrEmpty(transferType) ? x.TransferType : transferType,
                                                    SlotDate = x.SlotDate.HasValue ? x.SlotDate.Value.ToString("dd/MM/yyyy") : "",
                                                })
                                                .FirstOrDefault();

                if (result != null)
                {

                    var Seller = GetMemberProfile((int)result.SellerId);
                    var joinMembers = GetJointMembersByStockId((int)result.StockId);
                    if (Seller != null)
                    {
                        result.SellerCnic = $"{Seller.Cnic}{(string.IsNullOrEmpty(result.SellerJointMembers) ? "" : $", {result.SellerJointMembers}")}";
                        result.RelationshipWithSeller = Seller.RelationshipWith;
                        result.RelationshipSeller = Seller.Relationship;
                    }

                    var buyer = GetMemberProfile((int)result.BuyerId);
                    if (buyer != null)
                    {
                        result.RelationshipWithBuyer = buyer.RelationshipWith;
                        result.RelationshipBuyer = buyer.Relationship;
                        result.PermanentAddress = buyer.PermanentAddress;
                    }
                    result.BuyerMemberNames = GetBuyerWithAllMemberNames((int)result.BuyerId, id);
                    result.SellerMemberNames = GetSellerWithAllMemberNames((int)result.SellerId, (int)result.StockId);
                    var config = _db.SAPOperations.FirstOrDefault();
                    result.TransferCertificateTimeLineStatement = config?.TransferCertificateTimeLineStatement;

                }
                return Ok(result);

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }
        private string GetSellerWithAllMemberNames(int memberId, int stockId)
        {
            var MemberLists = new List<Models.DTOs.JointMemberDto>();

            var principalMemberImage = _db.MemberProfile
                .Where(th => th.Id == memberId)
                .Select(x => new Models.DTOs.JointMemberDto
                {
                    Cnic = x.Cnic
                }).FirstOrDefault();

            MemberLists.Add(principalMemberImage);


            var jointMembers = _db.JointMemberHistoricalData
                     .Where(x => x.StockCreationId == stockId)
                     .Select(x => new JointMemberDto
                     {
                         Cnic = x.CNIC,
                     })
                     .ToList();

            MemberLists.AddRange(jointMembers);


            return FormatMembers(MemberLists);

          
        }
        private string GetBuyerWithAllMemberNames(int memberId,int id)
        {
            var MemberLists = new List<Models.DTOs.JointMemberDto>();

            var principalMemberImage = _db.MemberProfile
                .Where(th => th.Id == memberId)
                .Select(x => new Models.DTOs.JointMemberDto
                {
                   Cnic = x.Cnic
                }).FirstOrDefault();

            MemberLists.Add(principalMemberImage);


            var jointMemberCnics = _db.TransferReceiptJointMember
                .Where(jm => jm.TransferReceiptProcessingId == id)
                .Select(x => new Models.DTOs.JointMemberDto
                {
                    Cnic = x.CNIC
                }).Distinct()
                .ToList(); // Materialize result first

            MemberLists.AddRange(jointMemberCnics);
            return FormatMembers(MemberLists);
        }

        private string GetJointMembersByStockId(int stockId)
        {
            try
            {
                var jointMembers = new List<JointMemberDto>();

                // 🔹 Transfer Joint Members
                var currentPropTransfer = _db.TransferHistery
                    .Where(x => !x.IsDeleted && x.StockCreationId == stockId)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();

                if (currentPropTransfer != null)
                {
                    var jointMembersTransfer = _db.TransferHisteryJointMember
                        .Where(x => x.TransferHisteryId == currentPropTransfer.Id)
                        .Select(x => new JointMemberDto
                        {
                            Name = x.Name,
                            Cnic = x.CNIC,
                            Mobile = x.Mobile
                        })
                        .ToList();

                    jointMembers.AddRange(jointMembersTransfer);
                }

                // 🔹 Booking Joint Members
                var currentPropBooking = _db.Booking
                    .FirstOrDefault(x => !x.IsDeleted && x.StockCreationId == stockId);

                if (currentPropBooking != null)
                {
                    var jointMembersBooking = _db.BookingJointMember
                        .Where(x => x.BookingId == currentPropBooking.Id)
                        .Select(x => new JointMemberDto
                        {
                            Name = x.Name,
                            Cnic = x.CNIC,
                            Mobile = x.Mobile
                        })
                        .ToList();

                    jointMembers.AddRange(jointMembersBooking);
                }

                // 🔹 Historical Joint Members
                var historicalJointMembers = _db.JointMemberHistoricalData
                    .Where(x => x.StockCreationId == stockId)
                    .Select(x => new JointMemberDto
                    {
                        Name = x.Name,
                        Cnic = x.CNIC,
                        Mobile = x.Mobile
                    })
                    .ToList();

                jointMembers.AddRange(historicalJointMembers);

                // 🔹 Now get data from MemberProfile using CNIC (if exists)
                var memberCnics = jointMembers.Select(j => j.Cnic).ToList();

                var memberProfiles = _db.MemberProfile
                    .Where(m => memberCnics.Contains(m.Cnic))
                    .Select(m => new
                    {
                        m.HonorificsName,
                        m.MemberName,
                        m.Relationship,
                        m.RelationshipWith,
                        m.Cnic
                    })
                    .ToList();

                // 🔹 Merge & format
                var finalList = memberProfiles.Select(mp =>
                    $"{mp.HonorificsName}. {mp.MemberName} {mp.Relationship} {mp.RelationshipWith} ({mp.Cnic})"
                ).ToList();

                var combinedSellerData = string.Join(", ", finalList);

                return combinedSellerData;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private string FormatMembers(List<JointMemberDto> jointMembers)
        {
            if (jointMembers == null || !jointMembers.Any())
                return string.Empty;

            var memberCnics = jointMembers
                .Select(j => j.Cnic)
                .Distinct() // 🔹 also dedupe CNICs coming from DTO list
                .ToList();

            var memberProfiles = _db.MemberProfile
                .Where(m => memberCnics.Contains(m.Cnic))
                .ToList() // Materialize first
                .GroupBy(m => m.Cnic) // Group duplicates in memory
                .Select(g => g.First()) // Keep one of each CNIC
                .Select(mp => new
                {
                    mp.HonorificsName,
                    mp.MemberName,
                    mp.Relationship,
                    mp.RelationshipWith,
                    mp.Cnic
                })
                .ToList();

            var finalList = memberProfiles
                .Select(mp => $"{mp.HonorificsName}. {mp.MemberName} {mp.Relationship} {mp.RelationshipWith} ({mp.Cnic})")
                .ToList();

            return string.Join(", ", finalList);
        }


        [HttpGet]
        [Route("PrintReceipt")]
        public async Task<IActionResult> PrintReceipt(int id, string type, string? invoiceType = "GovtTaxes")
        {
            try
            {
                if (type == "Seller")
                {
                    var result = _db.TransferReceiptProcessing.Where(x => x.Id == id)
                                                              .Include(x => x.GovtSellerCharges)
                                                              .Include(x => x.StockCreation)
                                                              .Include(x => x.SellerTaxes)
                                                              .ThenInclude(x => x.TaxType)
                                                              .FirstOrDefault();

                    result.ChallanNo = string.IsNullOrEmpty(result.ChallanNoSellerTaxes) ? await _commonBLL.GetNextChallanNumberAsync("CHALLAN") : result.ChallanNoSellerTaxes;

                    if (result != null && result.SellerTaxes != null)
                    {
                        foreach (var tax in result.SellerTaxes)
                        {
                            tax.ChargeName = tax.TaxType?.Description;
                        }
                    }

                    if (result != null)
                    {
                        if (result.StockCreation.Type != null && result.StockCreation.Type != "")
                        {
                            result.StockCreation.TypeName = _db.PropertyTypes.Where(x => x.ID == Convert.ToInt64(result.StockCreation.Type)).Select(x => x.Description).FirstOrDefault();
                            result.StockCreation.CategoryName = _db.Categories.Where(x => x.ID == Convert.ToInt64(result.StockCreation.Category)).Select(x => x.Description).FirstOrDefault();
                        }

                    }

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
                }
                else
                {
                    var result = _db.TransferReceiptProcessing.Where(x => x.Id == id)
                                                              .Include(x => x.GovtBuyerCharges.Where(x => x.InvoiceType == invoiceType))
                                                              .Include(x => x.StockCreation)
                                                              .Include(x => x.BuyerTaxes)
                                                              .ThenInclude(x => x.TaxType)
                                                              .FirstOrDefault();

                    result.ChallanNo = string.IsNullOrEmpty(result.ChallanNoBuyerTaxes) ? await _commonBLL.GetNextChallanNumberAsync("CHALLAN") : result.ChallanNoBuyerTaxes;

                    if (result != null && result.BuyerTaxes != null)
                    {
                        foreach (var tax in result.BuyerTaxes)
                        {
                            tax.ChargeName = tax.TaxType?.Description;
                        }
                    }

                    if (result != null)
                    {
                        if (result.StockCreation.Type != null && result.StockCreation.Type != "")
                        {
                            result.StockCreation.TypeName = _db.PropertyTypes.Where(x => x.ID == Convert.ToInt64(result.StockCreation.Type)).Select(x => x.Description).FirstOrDefault();
                            result.StockCreation.CategoryName = _db.Categories.Where(x => x.ID == Convert.ToInt64(result.StockCreation.Category)).Select(x => x.Description).FirstOrDefault();

                        }

                    }
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Success",
                        Data = result
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "Not Found",
                    Data = null
                });

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetSingleRecord")]
        public IActionResult GetSingleRecord(int id)
        {
            try
            {
                var result = _db.TransferReceiptProcessing.Where(x => !x.IsDeleted && x.Id == id)
                                                          .Include(x => x.GovtSellerCharges)
                                                          .Include(x => x.GovtBuyerCharges)
                                                          .Include(x => x.TransferAttachments)
                                                          .Include(x => x.StockCreation)
                                                          .Include(x => x.TransferReceiptJointMember)
                                                          .Include(x => x.TransferReceiptNominee)
                                                          .Include(x => x.SellerTaxes)
                                                          .Include(x => x.BuyerTaxes)
                                                          .FirstOrDefault();

                result.TransferSetReceivingAttachments = GetTransferSetReceveingAttachments(result.StockCreationId);

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
        [Route("GetAllFilterTransferReceipt")]
        public IActionResult GetAllFilterTransferReceipt()
        {
            try
            {
                var result = _db.TransferReceiptProcessing.Where(x => !x.IsDeleted && x.SellerName != null)
                                                    .Select(x => new
                                                    {
                                                        x.Id,
                                                        x.StockCreationId,
                                                        x.StockCreation.MemberProfile.MemberName,
                                                        x.StockCreation.MemberProfile.Cnic,
                                                        x.StockCreation.RegistrationNo,
                                                        x.StockCreation.PropertyNo,
                                                        x.CreatedOn
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
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _db.TransferReceiptProcessing.Where(x => !x.IsDeleted)
                                                          .Include(x => x.StockCreation)
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
        [AllowAnonymous]
        [Route("GetNDC1FilterList")]
        public IActionResult GetNDC1FilterList()
        {
            try
            {
                var result = _db.NDC1.Where(x => !x.IsDeleted && x.IsGovtTaxRequested == true && x.IsGovtTaxApproved != true)
                                               .Include(x => x.StockCreation)
                                               .Include(x => x.MemberProfile)
                                               .ToList()
                                               .OrderByDescending(x => x.Id)
                                               .DistinctBy(x => x.StockCreationId)
                                               .Select(x => new
                                               {
                                                   x.MemberProfile.MemberName,
                                                   x.MemberProfile.Cnic,
                                                   x.StockCreation.PropertyNo,
                                                   x.StockCreation.RegistrationNo,
                                                   x.StockCreation.Status,
                                                   x.Id,
                                                   x.CreatedOn
                                               });

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

        //Tax Esitmation
        [HttpGet]
        [Route("GetProperty")]
        public IActionResult GetProperty(int id)
        {
            try
            {
                var result = _db.StockCreations.Where(x => x.ID == id)

                                                .Select(x => new TransferReciptDTO
                                                {
                                                    BlockName = x.Block,
                                                    RegistrationNo = x.RegistrationNo,
                                                    PropertyNo = x.PropertyNo,
                                                    CategoryName = x.Category,
                                                    PlotSize = x.ActualSize,
                                                    ConstructionStatus = x.ConstracutionStatus,
                                                    Filer = x.MemberProfile.TaxStatus,
                                                    MemberProfileId = x.MemberProfile.Id,
                                                    SellerName = x.MemberProfile.MemberName,
                                                    StockCreationId = x.ID,
                                                    CoveredArea = x.coveredArea == null ? x.ActualSize : x.coveredArea.ToString(),
                                                    TimeAgo = 0,
                                                    CategoryId = x.Category,
                                                    NatureId = x.Nature,
                                                    PropertyTypeId = x.Type,
                                                    EffectiveDateTime = x.Created_at,
                                                    PropertyTaxYear = 0
                                                }).FirstOrDefault();

                if (result != null)
                {
                    result.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(result.BlockName));
                    result.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(result.CategoryName));

                    //GetSecondLastFromTransferDTO seller = GetSecondLastById(Convert.ToInt32(result.StockCreationId));
                    //result.SellerName = seller.MemberName;

                    if (result.ConstructionStatus == "Constructed")
                    {
                        result.ConstructedDateTime = _commonBLL.GetConstructedDateTime(Convert.ToInt32(result.StockCreationId));

                        if (result.ConstructedDateTime != null)
                        {
                            result.TimeAgo = UHelper.GetYearsSinceDate(result.ConstructedDateTime.Value);
                        }
                        else
                        {
                            result.TimeAgo = 0;
                        }
                    }

                    result.PropertyTaxYear = GetPropertyTaxYear(result.StockCreationId);
                    
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

        [HttpGet]
        [Route("Get")]
        public IActionResult Get(int id)
        {
            try
            {
                var result = _db.NDC1.Where(x => !x.IsDeleted && x.Id == id)
                                                .Select(x => new TransferReciptDTO
                                                {
                                                    Id=x.Id,
                                                    BlockName = x.StockCreation.Block,
                                                    RegistrationNo = x.StockCreation.RegistrationNo,
                                                    PropertyNo = x.StockCreation.PropertyNo,
                                                    CategoryName = x.StockCreation.Category,
                                                    PlotSize = x.StockCreation.ActualSize,
                                                    ConstructionStatus = x.StockCreation.ConstracutionStatus,
                                                    Filer = x.MemberProfile.TaxStatus,
                                                    MemberProfileId = x.MemberProfile.Id,
                                                    SellerName = x.MemberProfile.MemberName,
                                                    StockCreationId = x.StockCreation.ID,
                                                    CNIC = x.MemberProfile.Cnic,
                                                    ApplyStation = x.ApplyStation,
                                                    CoveredArea = x.StockCreation.coveredArea == null ? x.StockCreation.ActualSize : x.StockCreation.coveredArea.ToString(),
                                                    TimeAgo = 0,
                                                    CategoryId = x.StockCreation.Category,
                                                    NatureId = x.StockCreation.Nature,
                                                    PropertyTypeId = x.StockCreation.Type,
                                                    EffectiveDateTime = x.StockCreation.Created_at,
                                                    PropertyTaxYear = 0,
                                                    EstateName = x.EstateName,
                                                    DealerName = x.DealerName,
                                                    DealerCode = x.DealerCode,
                                                }).FirstOrDefault();

                if (result != null)
                {
                    NDCReadDto nDCReadDto = GetNDCData(result.StockCreationId);
                    if(nDCReadDto != null) {
                        result.NDCRequestType = nDCReadDto.NDCRequestType;
                        result.TransferType = nDCReadDto.TransferType;
                        result.SlotDate = nDCReadDto.SlotDate;
                        result.SlotMintues = nDCReadDto.SlotMintues;
                        result.SlotHour = nDCReadDto.SlotHour;
                        result.Day = nDCReadDto.Day;
                        result.PossessionStatus = nDCReadDto.PossessionStatus;
                        result.ValidateDate = nDCReadDto.ValidateDate;
                        result.DealerCode = nDCReadDto.DealerCode;
                        result.DealerName = nDCReadDto.DealerName;
                        result.EstateName = nDCReadDto.EstateName;
                        result.ApplyStation = nDCReadDto.ApplyStation;
                    }

                    result.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(result.BlockName));
                    result.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(result.CategoryName));

                    if (result.ConstructionStatus == "Constructed")
                    {
                        result.ConstructedDateTime = _commonBLL.GetConstructedDateTime(Convert.ToInt32(result.StockCreationId));

                        if (result.ConstructedDateTime != null)
                        {
                            result.TimeAgo = UHelper.GetYearsSinceDate(result.ConstructedDateTime.Value);
                        }
                        else
                        {
                            result.TimeAgo = 0;
                        }
                    }

                    result.PropertyTaxYear = GetPropertyTaxYear(result.StockCreationId);
                    result.TransferSetReceivingAttachments = GetTransferSetReceveingAttachments(result.StockCreationId);
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
        [Route("AddNewTransferReceiptProcessing")]
        public async Task<IActionResult> AddNewTransferReceiptProcessingAsync(TransferReceiptProcessing model)
        {
            try
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
                bool isApprovalActive = true;

                var approvalStatus = _db.ApprovalUI.Find((int)ApprovalUIIds.TransferReceipt);
                if (approvalStatus != null)
                {
                    if (approvalStatus.Checked != true)
                    {
                        isApprovalActive = false;
                    }
                }

                var approvalSetup = _db.ApprovalSetup.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.TransferReceipt).ToList();
                if (approvalSetup.Count <= 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Approval setup not defined or In-active",
                        Data = null
                    });
                }

                Response_Result sellerInvoicePosting = null;
                Response_Result buyerInvoicePosting = null;

                model.ChallanNoSellerTaxes = await _commonBLL.GetNextChallanNumberAsync("CHALLAN");


                sellerInvoicePosting = new SapIntegrationController(_db).PostingTransferRecieptSellerARInvoice(model);

                if (sellerInvoicePosting.code != 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = sellerInvoicePosting.message + " _while Posting Seller Charges",
                        Data = null
                    });
                }

                model.ChallanNoBuyerTaxes = await _commonBLL.GetNextChallanNumberAsync("CHALLAN");

                buyerInvoicePosting = new SapIntegrationController(_db).PostingTransferRecieptBuyerARInvoice(model);

                if (buyerInvoicePosting.code != 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = buyerInvoicePosting.message + " _while Posting Buyer Charges",
                        Data = null
                    });
                }

                model.IsActive = true;
                model.CreatedOn = model.CreatedOn.Date.Add(DateTime.Now.TimeOfDay);
                model.CreatedBy = model.CreatedBy;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;

                if (model.TransferReceiptJointMember?.Count > 0)
                {
                    foreach (var item in model.TransferReceiptJointMember)
                    {
                        item.ModifiedBy = model.ModifiedBy;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                if (model.TransferReceiptNominee?.Count > 0)
                {
                    foreach (var item in model.TransferReceiptNominee)
                    {
                        item.ModifiedBy = model.ModifiedBy;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                if (model.TransferAttachments?.Count() > 0)
                {
                    foreach (var item in model.TransferAttachments)
                    {
                        item.ModifiedBy = model.ModifiedBy;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                var path = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";
                model.CombineImage = string.IsNullOrEmpty(model.CombineImage) ? "" : $"{path}{await model.CombineImage.SaveBase64FileAsync()}";

                
                _db.TransferReceiptProcessing.Add(model);
               
                var transferReceiptProcessing = _db.NDC1.Where(x => x.StockCreationId == model.StockCreationId)
                                                        .ToList();
                if (transferReceiptProcessing.Count() > 0)
                {
                    foreach (var item in transferReceiptProcessing)
                    {
                        item.IsGovtTaxApproved = true;
                        // _db.SaveChanges();
                    }
                }

                string message = "";
                TransferHistery transferHistorySaved = _db.TransferHistery
                                                          .Where(x => x.StockCreationId == model.StockCreationId)
                                                          .OrderByDescending(x => x.Id)
                                                          .FirstOrDefault();


                if (transferHistorySaved != null)
                {
                   
                    addorUpdateJointMemberHistoricalDatas(model.JointMemberHistoricalDatas, model.StockCreationId, model.CreatedBy, model.LastModifiedUserName);
                   
                    _db.SaveChanges();
                    transferHistorySaved.IsGovtProcessingTaxRequested = true;
                    transferHistorySaved.IsRequestClosed = false;
                    transferHistorySaved.ReciptPrpcessingId = model.Id;
                    transferHistorySaved.ApplyStation = model.ApplyStation;
                    transferHistorySaved.TransferType = model.TransferType;
                    transferHistorySaved.NDCRequestType = model.NDCRequestType;
                    transferHistorySaved.SellerStation = model.SellerStation;
                    transferHistorySaved.BuyerStation = model.BuyerStation;
                    transferHistorySaved.BuyerRepresentativeName = model.BuyerRepresentativeName;
                    transferHistorySaved.BuyerRepresentativeRelationshipWith = model.BuyerRepresentativeRelationshipWith;
                    transferHistorySaved.BuyerRepresentativeCnic = model.BuyerRepresentativeCnic;
                    transferHistorySaved.SellerRepresentativeName = model.SellerRepresentativeName;
                    transferHistorySaved.SellerRepresentativeRelationshipWith = model.SellerRepresentativeRelationshipWith;
                    transferHistorySaved.SellerRepresentativeCnic = model.SellerRepresentativeCnic;

                    _db.Update(transferHistorySaved);
                    _db.SaveChanges();

                    if (isApprovalActive == true)
                    {
                        bool result = _approvalBLL.AddNewApprovalSetup(transferHistorySaved.Id, (int)ApprovalUIIds.TransferReceipt);
                        message = "Client Transfer Receipt Request added succesfully and moved for approval";
                        if (result)
                        {
                            return Ok(new ApiResponse<object>
                            {
                                Code = ResponseCode.Success,
                                Message = message,
                                Data = null
                            });
                        }
                    }
                    else
                    {
                        transferHistorySaved.IsGovtProcessingTaxApproved = true;
                        _db.SaveChanges();

                        message = "Client Transfer Receipt Request added succesfully";

                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = message,
                            Data = null
                        });
                    }

                    var stock = _db.StockCreations.Where(x => x.ID == model.StockCreationId).Include(x => x.MemberProfile).Select(x => new { x.RegistrationNo, x.MemberProfile.MemberName }).FirstOrDefault();
                    string narration = $"Transfer Receipt of Seller: {stock.MemberName} and Buyer {model.BuyerName} having ReferenceNo: {stock.RegistrationNo} is submitted by {model.LastModifiedUserName}";
                    alertService.PushAlert(5, narration);
                }
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "something went bad",
                    Data = null
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        private void addorUpdateJointMemberHistoricalDatas(
            ICollection<JointMemberHistoricalData> jointMemberHistoricalDatas,
            int stockCreationId,
            int? createdBy,
            string? lastModifiedUserName)
        {
            if (jointMemberHistoricalDatas == null)
                return;

            var existingRecords = _db.JointMemberHistoricalData
                                     .Where(x => x.StockCreationId == stockCreationId)
                                     .ToList();
           
             _db.JointMemberHistoricalData.RemoveRange(existingRecords);


            foreach (var item in jointMemberHistoricalDatas)
            {
                var newItem = new JointMemberHistoricalData
                {
                    MemberProfileId = item.MemberProfileId,
                    StockCreationId = stockCreationId,
                    Name = item.Name,
                    Relationship = item.Relationship,
                    CNIC = item.CNIC,
                    Mobile = item.Mobile,
                    Address = item.Address
                };

                _db.JointMemberHistoricalData.Add(newItem);
            }

            _db.SaveChanges();
        }


        [HttpPost]
        [Route("UpdateTransferReceiptProcessing")]
        public async Task<IActionResult> UpdateTransferReceiptProcessingAsync(TransferReceiptProcessing model)
        {
            try
            {
                var data = _db.TransferReceiptProcessing.Find(model.Id);

                if (data != null)
                {
                    if (model.CombineImage != data.CombineImage)
                    {
                        if (!string.IsNullOrEmpty(data.CombineImage))
                        {
                            data.CombineImage.DeleteFile();
                        }

                        if (!string.IsNullOrEmpty(model.CombineImage))
                        {
                            var path = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";
                            data.CombineImage = $"{path}{await model.CombineImage.SaveBase64FileAsync()}";
                        }
                        else
                        {
                            data.CombineImage = "";
                        }
                    }
                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;
                    data.LastModified = DateTime.Now;
                    data.BuyerId = model.BuyerId;
                    data.BuyerName = model.BuyerName;
                    data.CNIC = model.CNIC;
                    data.Address = model.Address;
                    data.ContactNo = model.ContactNo;
                    data.Filer = model.Filer;
                    data.SellerRepresentativeName = model.SellerRepresentativeName;
                    data.SellerRepresentativeCnic = model.SellerRepresentativeCnic;
                    data.SellerRepresentativeRelationshipWith = model.SellerRepresentativeRelationshipWith;
                    data.SellerStation = model.SellerStation;
                    data.BuyerRepresentativeName = model.BuyerRepresentativeName;
                    data.BuyerRepresentativeRelationshipWith = model.BuyerRepresentativeRelationshipWith;
                    data.BuyerRepresentativeCnic = model?.BuyerRepresentativeCnic;
                    data.BuyerStation = model?.BuyerStation;
                    data.ChangeOverStatement = model?.ChangeOverStatement;

                    _db.Entry(data).State = EntityState.Modified;

                    var sellerTaxes = _db.SellerTaxes.Where(x => x.TransferReceiptProcessingId == model.Id).ToList();

                    _db.SellerTaxes.RemoveRange(sellerTaxes);


                    foreach (var item in model.SellerTaxes)
                    {
                        item.TransferReceiptProcessingId = data.Id;
                    }

                    _db.SellerTaxes.AddRange(model.SellerTaxes);

                    var buyerTaxes = _db.BuyerTaxes.Where(x => x.TransferReceiptProcessingId == model.Id).ToList();

                    _db.BuyerTaxes.RemoveRange(buyerTaxes);


                    foreach (var item in model.BuyerTaxes)
                    {
                        item.TransferReceiptProcessingId = data.Id;
                    }

                    _db.BuyerTaxes.AddRange(model.BuyerTaxes);

                    var result = _db.TransferReceiptJointMember.Where(x => x.TransferReceiptProcessingId == model.Id).ToList();

                        _db.TransferReceiptJointMember.RemoveRange(result);

                   
                        foreach (var item in model.TransferReceiptJointMember)
                        {
                            item.TransferReceiptProcessingId = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                        }

                        _db.TransferReceiptJointMember.AddRange(model.TransferReceiptJointMember);

                        var result1 = _db.TransferReceiptNominee.Where(x => x.TransferReceiptProcessingId == model.Id).ToList();

                        _db.TransferReceiptNominee.RemoveRange(result1);

                   
                        foreach (var item in model.TransferReceiptNominee)
                        {
                            item.TransferReceiptProcessingId = data.Id;
                            item.ModifiedBy = model.ModifiedBy;
                            item.LastModifiedUserName = model.LastModifiedUserName;
                            item.LastModified = DateTime.Now;
                            item.IsActive = true;
                            item.IsDeleted = false;
                    }

                    _db.TransferReceiptNominee.AddRange(model.TransferReceiptNominee);

                    TransferHistery transferHistorySaved = _db.TransferHistery
                                                          .Where(x => x.ReciptPrpcessingId == model.Id)
                                                          .OrderByDescending(x => x.Id)
                                                          .FirstOrDefault();


                    if (transferHistorySaved != null)
                    {

                        transferHistorySaved.SellerStation = model.SellerStation;
                        transferHistorySaved.BuyerStation = model.BuyerStation;
                        transferHistorySaved.BuyerRepresentativeName = model.BuyerRepresentativeName;
                        transferHistorySaved.BuyerRepresentativeRelationshipWith = model.BuyerRepresentativeRelationshipWith;
                        transferHistorySaved.BuyerRepresentativeCnic = model.BuyerRepresentativeCnic;
                        transferHistorySaved.SellerRepresentativeName = model.SellerRepresentativeName;
                        transferHistorySaved.SellerRepresentativeRelationshipWith = model.SellerRepresentativeRelationshipWith;
                        transferHistorySaved.SellerRepresentativeCnic = model.SellerRepresentativeCnic;

                        _db.Update(transferHistorySaved);
                    }

                    addorUpdateJointMemberHistoricalDatas(model.JointMemberHistoricalDatas, model.StockCreationId, model.CreatedBy, model.LastModifiedUserName);

                    _db.SaveChanges();
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Specific record update successfully",
                        Data = null
                    });
                }
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.NotFound,
                    Message = "something went bad",
                    Data = null
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        private int GetPropertyTaxYear(int stockId)
        {
            DateTime date = DateTime.Now;

            bool IsExistInTransfer = _db.TransferHistery.Any(x => x.StockCreationId == stockId);
            if (IsExistInTransfer)
            {
                date = _db.TransferHistery.Where(x => !x.IsDeleted && x.StockCreationId == stockId)
                                                             .OrderByDescending(x => x.Id)
                                                             .FirstOrDefault().CreatedOn;
            }
            else
            {
                date = _db.Booking.FirstOrDefault(x => !x.IsDeleted && x.StockCreationId == stockId)?.CreatedOn ?? date;
            }

            DateTime cutoffDate = new DateTime(2016, 7, 1);
            if (date < cutoffDate)
            {
                date = cutoffDate;
            }

            return UHelper.GetYearsSinceDate(date);
        }

        private List<TransferSetReceivingAttachments> GetTransferSetReceveingAttachments(int stockId)
        {
            return _db.TransferSetReceivingAttachments.Where(x=>x.TransferSetReceiving.StockCreationId == stockId)
                                                      .ToList();
        }

        private NDCReadDto GetNDCData(int id)
        {
            var result = _db.NDCRequestForMember.Where(x => !x.IsDeleted && x.StockCreationId == id && x.IsRequestedClosed != true)
                                                .Include(x => x.TransferType)
                                                .OrderByDescending(x => x.Id) 
                                                .Select(x => new NDCReadDto
                                                {
                                                    NDCRequestType = x.NDCRequestType,
                                                    TransferType = x.TransferType.Description,
                                                    SlotDate = x.SlotDate,
                                                    SlotHour = x.SlotHour,
                                                    SlotMintues = x.SlotMintues,
                                                    Day = x.Day,
                                                    PossessionStatus = x.PossessionStatus,
                                                    ValidateDate = x.ValidityDate,
                                                    DealerCode = x.DealerCode,
                                                    DealerName = x.DealerName,
                                                    EstateName = x.EstateName,
                                                    ApplyStation = x.ApplyStation

                                                })
                                                .FirstOrDefault();
            return result;
        }


        [HttpPost]
        [Route("CancelRequest")]
        public IActionResult CancelRequest(int id)
        {
            try
            {
                var model = _db.TransferReceiptProcessing.Find(id);

                if (model != null)
                {
                    var request = _db.NDC1.Where(x => x.StockCreationId == model.StockCreationId && x.IsCanceled != true)
                                               .OrderBy(x => x.Id)
                                               .LastOrDefault();
                    var ndcrequest = _db.NDCRequestForMember.Where(x => x.StockCreationId == model.StockCreationId && x.IsCanceled != true)
                                              .OrderBy(x => x.Id)
                                              .LastOrDefault();


                    if (request != null && ndcrequest != null)
                    {
                        request.IsGovtTaxRequested = false;
                        request.IsGovtTaxApproved = false;
                        request.IsCanceled = true;
                        ndcrequest.IsRequestedClosed = false;
                        ndcrequest.IsActive = true;
                        
                        model.ModifiedBy = model.ModifiedBy;
                        model.LastModified = DateTime.Now;
                        model.IsActive = false;
                        model.IsDeleted = false;

                        _db.SaveChanges();

                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "Request Cancelled",
                            Data = null
                        });

                    }
                }
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.BadRequest,
                    Message = "Already cancelled",
                    Data = null
                });

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        private MemberProfile GetMemberProfile(int id)
        {
            return _db.MemberProfile.Find(id);
        }
    }
}

