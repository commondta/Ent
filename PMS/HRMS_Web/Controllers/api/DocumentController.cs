using B_DB_Context;
using B_Utility.BLL;
using B_Utility.Common;
using HRMS_Web.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly DataBase_Context _db;
        CommonBLL _commonBLL;

        public DocumentController(DataBase_Context db)
        {
            _db = db;
            _commonBLL = new CommonBLL(_db);
        }

        [HttpGet]
        [Route("GetPossesionLetter")]
        public IActionResult GetPossesionLetter(int id)
        {
            try
            {
                var stockDetails = _db.StockCreations.FirstOrDefault(x => x.ID == id);

                var profile = _db.MemberProfile.FirstOrDefault(d => d.Id == stockDetails.MemberProfileId);
                var propertyType = _db.PropertyTypes.FirstOrDefault(y => y.ID == Convert.ToInt32(stockDetails.Type));
                var realEstateType = _db.Real_Estates.FirstOrDefault(y => y.ID == Convert.ToInt32(stockDetails.RealStateType));
                var block = _db.Blocks.FirstOrDefault(y => y.ID == Convert.ToInt32(stockDetails.Block));
                var natureEntity = _db.Natures.FirstOrDefault(y => y.ID == Convert.ToInt32(stockDetails.Nature));
                var phase = _db.Phases.FirstOrDefault(y => y.ID == Convert.ToInt32(stockDetails.Phase));
                var category = _db.Categories.FirstOrDefault(y => y.ID == Convert.ToInt32(stockDetails.Category));
                int? prefixId = null;
                if (int.TryParse(stockDetails?.PrefixProperty, out var parsed))
                {
                    prefixId = parsed;
                }

                var sector = prefixId.HasValue
                    ? _db.Sectors.FirstOrDefault(y => y.ID == prefixId.Value)
                    : null;

                var result = new
                {
                    // =====================================================
                    // MEMBER DETAILS
                    // =====================================================

                    RelationshipBuyer = profile?.Relationship ?? "",
                    RelationshipWithBuyer = profile?.RelationshipWith ?? "",

                    BuyerName = $"{profile?.HonorificsName ?? ""} {profile?.MemberName ?? ""}",

                    BuyerCnic = profile?.Cnic ?? "",
                    BuyerPhone = profile?.Mobile ?? "",
                    BuyerEmail = profile?.EmailId ?? "",

                    PermanentAddress = profile?.CurrentAddress ?? "",

                    // =====================================================
                    // PROPERTY DETAILS
                    // =====================================================
                    Id = stockDetails?.ID ?? 0,
                    RegistrationNo = stockDetails?.RegistrationNo ?? "",
                    PropertyNo = stockDetails?.PropertyNo ?? "",

                    ConstructionStatus = stockDetails?.ConstracutionStatus ?? "",

                    Area = stockDetails?.ActualSize ?? "",
                    UnitArea = stockDetails?.ActualSizeUnit ?? "",

                    Sqft = stockDetails?.coveredArea == null
       ? "N/A"
       : stockDetails.coveredArea.ToString(),

                    RealEstateType = realEstateType?.Description ?? "",
                    Type = propertyType?.Description ?? "",
                    Block = block?.Description ?? "",
                    Category = category?.Description ?? "",

                    Nature = (natureEntity?.Description == "Plot"
       && stockDetails?.ConstracutionStatus == "Constructed")
           ? "House"
           : natureEntity?.Description ?? "",

                    SectorName = sector?.Description ?? "",
                    Phase = phase?.Description ?? "",

                    // =====================================================
                    // PART I
                    // =====================================================

                    TransferRecordOfficerName = stockDetails?.TransferRecordOfficerName ?? "",
                    TransferRecordDirectorName = stockDetails?.TransferRecordDirectorName ?? "",

                    // =====================================================
                    // PART II
                    // =====================================================

                    FrontSide = stockDetails?.FrontSide ?? "",
                    RearSide = stockDetails?.RearSide ?? "",
                    LeftSide = stockDetails?.LeftSide ?? "",
                    RightSide = stockDetails?.RightSide ?? "",

                    FrontBoundary = stockDetails?.FrontBoundary ?? "",
                    RearBoundary = stockDetails?.RearBoundary ?? "",
                    LeftBoundary = stockDetails?.LeftBoundary ?? "",
                    RightBoundary = stockDetails?.RightBoundary ?? "",

                    StandardAreaOfPlot = stockDetails?.StandardAreaOfPlot ?? 0,
                    AreaOfPlot = stockDetails?.AreaOfPlot ?? 0,
                    ExcessArea = stockDetails?.ExcessArea ?? 0,
                    LessArea = stockDetails?.LessArea ?? 0,

                    ApprovedMinSheetReferenceNo = stockDetails?.ApprovedMinSheetReferenceNo ?? "",

                    IsCornerPlot = stockDetails?.IsCornerPlot ?? false,
                    IsParkFacing = stockDetails?.IsParkFacing ?? false,
                    IsMainBoulevard = stockDetails?.IsMainBoulevard ?? false,

                    SurveyorName = stockDetails?.SurveyorName ?? "",
                    BuildingControlDirectorName = stockDetails?.BuildingControlDirectorName ?? "",

                    // =====================================================
                    // PART III
                    // =====================================================

                    DuesClearedTillDate = stockDetails?.DuesClearedTillDate,
                    NdcNo = stockDetails?.NdcNo ?? "",

                    FinanceOfficerName = stockDetails?.FinanceOfficerName ?? "",
                    FinanceDirectorName = stockDetails?.FinanceDirectorName ?? "",

                    // =====================================================
                    // PART IV
                    // =====================================================

                    PossessionHandedOverOn = stockDetails?.PossessionHandedOverOn,
                    PossessionNo = stockDetails?.PossessionNo ?? "",
                    PossessionSurveyorName = stockDetails?.PossessionSurveyorName ?? "",
                    OwnerName = stockDetails?.OwnerName ?? "",

                    // =====================================================

                    ImageUrl = GetImagesUrl(id, profile.Id)

            };

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }


        [HttpGet]
        [Route("GetAllotmentLetter")]
        public IActionResult GetAllotmentLetter(int id)
        {
            try
            {
                var stockDetails = _db.StockCreations.FirstOrDefault(x => x.ID == id);

                var buyerProfile = _db.MemberProfile.FirstOrDefault(d => d.Id == stockDetails.MemberProfileId);
                var propertyType = _db.PropertyTypes.FirstOrDefault(y => y.ID == Convert.ToInt32(stockDetails.Type));
                var realEstateType = _db.Real_Estates.FirstOrDefault(y => y.ID == Convert.ToInt32(stockDetails.RealStateType));
                var block = _db.Blocks.FirstOrDefault(y => y.ID == Convert.ToInt32(stockDetails.Block));
                var natureEntity = _db.Natures.FirstOrDefault(y => y.ID == Convert.ToInt32(stockDetails.Nature));
                var phase = _db.Phases.FirstOrDefault(y => y.ID == Convert.ToInt32(stockDetails.Phase));
                var category = _db.Categories.FirstOrDefault(y => y.ID == Convert.ToInt32(stockDetails.Category));
                int? prefixId = null;
                if (int.TryParse(stockDetails?.PrefixProperty, out var parsed))
                {
                    prefixId = parsed;
                }

                var sector = prefixId.HasValue
                    ? _db.Sectors.FirstOrDefault(y => y.ID == prefixId.Value)
                    : null;

                var detail = _db.SAPOperations.FirstOrDefault();

                var result = new OwnershipAgreementPrintDto
                {
                    SignatoryRank = detail?.SignatoryRank ?? "",
                    SignatoryDesignation = detail?.SignatoryDesignation ?? "",
                    SignatoryName = detail?.SignatoryName ?? "",

                    RelationshipBuyer = buyerProfile?.Relationship ?? "",
                    RelationshipWithBuyer = buyerProfile?.RelationshipWith ?? "",
                    BuyerName = $"{buyerProfile?.HonorificsName ?? ""} {buyerProfile?.MemberName ?? ""}",
                    BuyerCnic = buyerProfile?.Cnic ?? "",
                    MembershipNo = buyerProfile?.MEMBERSHIPNO ?? "",
                    BuyerPhone = buyerProfile?.Mobile ?? "",
                    PermanentAddress = buyerProfile?.CurrentAddress ?? "",

                    RegistrationNo = stockDetails?.RegistrationNo ?? "",
                    PropertyNo = stockDetails?.PropertyNo ?? "",
                    ConstructionStatus = stockDetails?.ConstracutionStatus ?? "",
                    Area = stockDetails?.ActualSize,
                    UnitArea = stockDetails?.ActualSizeUnit ?? "",
                    Sqft = stockDetails?.coveredArea == null ? "N/A" : stockDetails.coveredArea.ToString(),

                    RealEstateType = realEstateType.Description,
                    Type = propertyType?.Description ?? "N/A",
                    Block = block?.Description ?? "N/A",
                    Category = category?.Description ?? "N/A",

                    Nature = (natureEntity?.Description == "Plot"
                    && stockDetails?.ConstracutionStatus == "Constructed")
                      ? "House"
                      : natureEntity?.Description ?? "N/A",

                    SectorName = sector?.Description ?? "",
                    Phase = phase?.Description ?? "N/A",

                };


                if (result != null)
                {
                    result.ImageUrl = GetImagesUrl(id, buyerProfile.Id);
                    result.BuyerWithJointMembers = GetJointMembersByStockIdAndUserId(id, (int)stockDetails.MemberProfileId, buyerProfile.Cnic);
                    result.SellerWithJointMembers = GetSellerAndJointMembersByStockIdAndUserId(id);
                    //  result.SwapOverStatement = GetChangeOverStatement((int)result.TransferReciptId);
                    result.Title = string.IsNullOrEmpty(result.PropertyNo) ? $"Reference Number {result?.RegistrationNo}" : $"{result.Nature} Number {result?.Phase}/{result?.SectorName}/{result?.PropertyNo}";
                    result.PlotNo = $"{result.Nature} Number {result?.PropertyNo}";
                    // result.BuyerDetail = $"{sellerProfile?.HonorificsName}.{sellerProfile?.MemberName} {sellerProfile?.Relationship} {sellerProfile?.RelationshipWith} ({sellerProfile?.Cnic}){(string.IsNullOrEmpty(result.SellerJointMembers) ? "" : $", {result.SellerJointMembers}")}";
                }

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("PrintDuplicateAllocationLetter")]
        public IActionResult PrintDuplicateAllocationLetter(int id)
        {
            try
            {
                var operation = _db.SAPOperations.FirstOrDefault();

                var result = _db.StockCreations
                                .Where(x => !x.is_deleted && x.ID == id)
                                .Include(x => x.MemberProfile)
                                .AsNoTracking()
                                .FirstOrDefault();

                if (result == null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Record not found"
                    });
                }

                var dto = new StockCertificateDto
                {
                    ID = result.ID,

                    RegistrationNo = result.RegistrationNo,
                    CaseCode = result.CaseCode,

                    PlotNo = result.PropertyNo,
                    PropertyNo = result.PropertyNo,

                    ActualSize = result.ActualSize,
                    Mouza = result.Mouza,
                    AllocationNo = result.AllocationNo,

                    SaleDeedNo = result.SaleDeedNo,
                    SaleDeedDate = result.SaleDeedDate,

                    MembershipFee = result.MembershipFee,
                    MiscCharges = result.MiscCharges,

                    AllocationSignatoryDesignation = operation?.AllocationSignatoryDesignation,
                    AllocationSignatoryName = operation?.AllocationSignatoryName,
                    AllocationSignatoryRank = operation?.AllocationSignatoryRank,

                    ImageURL = string.IsNullOrEmpty(GetBookingImage(result.ID))
                    ? result.MemberProfile?.ImageURL
                    : GetBookingImage(result.ID),

                    SalePerson = GetSalePerson(result.ID),

                    MemberNames = GetJointMembersByStockIdAndUserIdWithList(
                        result.ID,
                        Convert.ToInt32(result.MemberProfileId),
                        result.MemberProfile.Cnic),

                    RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(result.RealStateType)),
                    ProjectName = _commonBLL.GetProjectName(Convert.ToInt32(result.Project)),
                    PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(result.Phase)),
                    CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(result.Category)),
                    BlockName = _commonBLL.GetBlockName(Convert.ToInt32(result.Block)),
                    NatureName = _commonBLL.GetNatureName(Convert.ToInt32(result.Nature)),
                    TypeName = _commonBLL.GetTypeName(Convert.ToInt32(result.Type)),
                    PrefixProperty = _commonBLL.GetSectoreName(Convert.ToInt32(result.PrefixProperty)),
                    ConstracutionStatus = _commonBLL.GetConstrcutionStatus(result.ID)
                };

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = new
                    {
                        StockCreation = dto,
                        MemberProfile = result.MemberProfile
                    }
                });
            }
            catch (Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        private string GetSalePerson(int stockId)
        {          
            var sale = _db.PreSale.Where(x => x.StockCreationId == stockId).FirstOrDefault();
            if (sale == null)
            {
                return "";
            }
            return sale.SaleBy ?? "";
        }

        private string GetBookingImage(int stockId)
        {
            var booking = _db.Booking.Where(x => x.StockCreationId == stockId).FirstOrDefault();
             if (booking == null)
            {
                return "";
            }
            return booking.ImageURL ?? "";
        }


        [HttpGet]
        [Route("GetOwnershipAgreement")]
        public IActionResult GetOwnershipAgreement(int id)
        {
            try
            {
                var stockDetails = _db.StockCreations.FirstOrDefault(x => x.ID == id);

                if (stockDetails == null)
                    return NotFound("No record found.Please wait for letter print approval");

                var dealer = _db.Dealers.FirstOrDefault(d => d.Id == stockDetails.DealerId);
                var buyerProfile = _db.MemberProfile.FirstOrDefault(d => d.Id == stockDetails.MemberProfileId);
                var propertyType = _db.PropertyTypes.FirstOrDefault(y => y.ID == Convert.ToInt32(stockDetails.Type));
                var block = _db.Blocks.FirstOrDefault(y => y.ID == Convert.ToInt32(stockDetails.Block));
                var natureEntity = _db.Natures.FirstOrDefault(y => y.ID == Convert.ToInt32(stockDetails.Nature));
                var phase = _db.Phases.FirstOrDefault(y => y.ID == Convert.ToInt32(stockDetails.Phase));
                var category = _db.Categories.FirstOrDefault(y => y.ID == Convert.ToInt32(stockDetails.Category));
                int? prefixId = null;
                if (int.TryParse(stockDetails?.PrefixProperty, out var parsed))
                {
                    prefixId = parsed;
                }

                var sector = prefixId.HasValue
                    ? _db.Sectors.FirstOrDefault(y => y.ID == prefixId.Value)
                    : null;

                var detail = _db.SAPOperations.FirstOrDefault();

                var result = new OwnershipAgreementPrintDto
                {
                    SignatoryRank = detail?.SignatoryRank ?? "",
                    SignatoryDesignation = detail?.SignatoryDesignation ?? "",
                    SignatoryName = detail?.SignatoryName ?? "",

                    DealerName = dealer?.PrincipalOwner ?? "",
                    EstateName = dealer?.EstateName ?? "",
                    DealerCnic = dealer?.CNIC ?? "",
                    DealerRegistrationNo = dealer?.DelaerRegisrationCode ?? "",

                    RelationshipBuyer = buyerProfile?.Relationship ?? "",
                    RelationshipWithBuyer = buyerProfile?.RelationshipWith ?? "",
                    BuyerName = $"{buyerProfile?.HonorificsName ?? ""} {buyerProfile?.MemberName ?? ""}",
                    BuyerCnic = buyerProfile?.Cnic ?? "",
                    MembershipNo = buyerProfile?.MEMBERSHIPNO ?? "",
                    BuyerPhone = buyerProfile?.Mobile ?? "",
                    PermanentAddress = buyerProfile?.CurrentAddress ?? "",

                    RegistrationNo = stockDetails?.RegistrationNo ?? "",
                    PropertyNo = stockDetails?.PropertyNo ?? "",
                    ConstructionStatus = stockDetails?.ConstracutionStatus ?? "",
                    Area = stockDetails?.ActualSize,
                    UnitArea = stockDetails?.ActualSizeUnit ?? "",
                    Sqft = stockDetails?.coveredArea == null ? "N/A" : stockDetails.coveredArea.ToString(),

                    Type = propertyType?.Description ?? "N/A",
                    Block = block?.Description ?? "N/A",
                    Category = category?.Description ?? "N/A",

                    Nature = (natureEntity?.Description == "Plot"
              && stockDetails?.ConstracutionStatus == "Constructed")
                ? "House"
                : natureEntity?.Description ?? "N/A",

                    SectorName = sector?.Description ?? "",
                    Phase = phase?.Description ?? "N/A",

                    ImageUrl = buyerProfile?.ImageURL ?? "",
                    // MemberNames = GetAllMemberNames(id)
                };


                if (result != null)
                {
                    result.BuyerWithJointMembers = GetJointMembersByStockIdAndUserId(id, (int)stockDetails.MemberProfileId, buyerProfile.Cnic);
                    result.SellerWithJointMembers = GetSellerAndJointMembersByStockIdAndUserId(id);
                    //  result.SwapOverStatement = GetChangeOverStatement((int)result.TransferReciptId);
                    result.Title = string.IsNullOrEmpty(result.PropertyNo) ? $"Reference Number {result?.RegistrationNo}" : $"{result.Nature} Number {result?.Phase}/{result?.SectorName}/{result?.PropertyNo}";
                    result.PlotNo = $"{result.Nature} Number {result?.PropertyNo}";
                    // result.BuyerDetail = $"{sellerProfile?.HonorificsName}.{sellerProfile?.MemberName} {sellerProfile?.Relationship} {sellerProfile?.RelationshipWith} ({sellerProfile?.Cnic}){(string.IsNullOrEmpty(result.SellerJointMembers) ? "" : $", {result.SellerJointMembers}")}";
                }

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }


        private string GetTransferType(int id)
        {
            return _db.TransferReceiptProcessing.Find(id).TransferType;
        }

        private string GetChangeOverStatement(int id)
        {
            var value = _db.TransferReceiptProcessing.Find(id)?.ChangeOverStatement;
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value;
        }

        public List<Models.DTOs.MemberName> GetJointMembersByStockIdAndUserIdWithList(
    int stockId,
    int userId,
    string cnic)
        {
            try
            {
                var members = new List<Models.DTOs.MemberName>();

                // Principal Member
                var principalMember = _db.MemberProfile
                    .Where(x => x.Id == userId)
                    .Select(x => new Models.DTOs.MemberName
                    {
                        MemeberName = $"{x.HonorificsName}. {x.MemberName}",
                        Cnic = $"({x.Cnic})",
                        RelationName = x.Relationship,
                        Relationhipwith = x.RelationshipWith,
                        Id = x.Id
                    })
                    .FirstOrDefault();

                if (principalMember != null)
                    members.Add(principalMember);

                // 1️⃣ Transfer Members
                var currentPropTransfer = _db.TransferHistery
                    .Where(x => !x.IsDeleted
                             && x.StockCreationId == stockId
                             && x.MemberProfileId == userId
                             && x.Remarks != "Hosted Ownery")
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();

                if (currentPropTransfer != null)
                {
                    var transferMembers = _db.TransferHisteryJointMember
                        .Where(x => x.TransferHisteryId == currentPropTransfer.Id)
                        .Select(x => new Models.DTOs.MemberName
                        {
                            MemeberName = x.Name,
                            Cnic = $"({x.CNIC})"
                        })
                        .ToList();

                    members.AddRange(transferMembers);
                    return members;
                }

                // 2️⃣ Booking Members
                var currentPropBooking = _db.Booking
                    .FirstOrDefault(x => !x.IsDeleted
                                      && x.StockCreationId == stockId
                                      && x.MemberProfileId == userId);

                if (currentPropBooking != null)
                {
                    var bookingMembers = _db.BookingJointMember
                        .Where(x => x.BookingId == currentPropBooking.Id)
                        .Select(x => new Models.DTOs.MemberName
                        {
                            MemeberName = x.Name,
                            Cnic = $"({x.CNIC})"
                        })
                        .ToList();

                    members.AddRange(bookingMembers);
                    return members;
                }

                // 3️⃣ Historical Members
                var historicalMembers = _db.JointMemberHistoricalData
                    .Where(x => x.StockCreationId == stockId)
                    .Select(x => new Models.DTOs.MemberName
                    {
                        MemeberName = x.Name,
                        Cnic = $"({x.CNIC})"
                    })
                    .ToList();

                members.AddRange(historicalMembers);

                return members;
            }
            catch
            {
                return new List<Models.DTOs.MemberName>();
            }
        }

        private List<Models.DTOs.MemberName> GetAllMemberNames(int id)
        {
            var MemberLists = new List<Models.DTOs.MemberName>();

            var principalMemberImage = _db.TransferHistery
                .Where(th => th.StockCreationId == id && th.Remarks != "Hosted Ownery")
                .Select(x => new Models.DTOs.MemberName
                {
                    MemeberName = $"{x.MemberProfile.HonorificsName}. {x.MemberProfile.MemberName}",
                    Relationhipwith = x.MemberProfile.RelationshipWith,
                    RelationName = x.MemberProfile.Relationship,
                    Cnic = $"({x.MemberProfile.Cnic})",
                    Id = x.Id
                }).FirstOrDefault();

            MemberLists.Add(principalMemberImage);


            var jointMemberCnics = _db.TransferHisteryJointMember
                .Where(jm => jm.TransferHisteryId == principalMemberImage.Id)
                .Select(jm => jm.CNIC)
                .Distinct()
                .ToList(); // Materialize result first

            var jointMembersImageUrls = _db.MemberProfile
                .Where(mp => jointMemberCnics.Contains(mp.Cnic))
                .ToList() // Materialize so GroupBy happens client-side
                .GroupBy(mp => mp.Cnic)
                .Select(g => g.First())
                .Select(x => new Models.DTOs.MemberName
                {
                    MemeberName = x.MemberName,
                    Relationhipwith = x.RelationshipWith,
                    RelationName = x.Relationship,
                    Cnic = $"({x.Cnic})"
                })
                .ToList();

            MemberLists.AddRange(jointMembersImageUrls);
            return MemberLists;
        }

        public string GetSellerAndJointMembersByStockIdAndUserId(int stockId)
        {
            try
            {
                var members = new List<JointMemberDto>();

                var result1 = _db.TransferHistery.Where(x => !x.IsDeleted && x.StockCreationId == stockId)
                                                 .Select(x => new TransferHistoricalDataDTO
                                                 {
                                                     BuyerName = x.MemberProfile.MemberName,
                                                     BuyerCNIC = x.MemberProfile.Cnic,
                                                     SellerName = x.SellerName,
                                                     SellerCNIC = x.SellerCnic,
                                                     TransferDate = x.LastModified,
                                                     Source = "PMS"
                                                 })
                                                 .OrderByDescending(x => x.TransferDate)
                                                 .FirstOrDefault();

                string registrationNo = _db.StockCreations.Where(x => x.ID == stockId).IgnoreQueryFilters().FirstOrDefault().RegistrationNo;

                var result2 = _db.TransferHistoricalData.Where(x => x.RegistrationNo == registrationNo)
                                                .Select(x => new TransferHistoricalDataDTO
                                                {
                                                    BuyerName = x.BuyerName,
                                                    BuyerCNIC = x.BuyerCNIC,
                                                    SellerName = x.SellerName,
                                                    SellerCNIC = x.SellerCNIC,
                                                    TransferDate = x.TransferDate,
                                                    Source = "HISTORICAL"
                                                })
                                                .OrderByDescending(x => x.TransferDate)
                                                .FirstOrDefault();

                string sellerCnic = string.Empty;

                if (result1 != null || result2 != null)
                    sellerCnic = result1 == null ? result2.SellerCNIC : result1.SellerCNIC;

                if (string.IsNullOrEmpty(sellerCnic))
                    return string.Empty;

                var principalMember = _db.MemberProfile
                    .Where(x => x.Cnic == sellerCnic)
                    .Select(x => new JointMemberDto
                    {
                        Name = $"{x.HonorificsName}. {x.MemberName}",
                        Mobile = x.Id.ToString(),
                        Cnic = x.Cnic,
                    })
                    .FirstOrDefault();

                if (principalMember != null)
                    members.Add(principalMember);

                var currentPropTransfer = _db.TransferHistery
                    .Where(x => !x.IsDeleted &&
                                x.StockCreationId == stockId &&
                                x.SellerCnic == sellerCnic &&
                                x.Remarks != "Hosted Ownery")
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();

                if (currentPropTransfer != null)
                {
                    var transferMembers = _db.TransferHisteryJointMember
                        .Where(x => x.TransferHisteryId == currentPropTransfer.Id)
                        .Select(x => new JointMemberDto
                        {
                            Name = x.Name,
                            Cnic = x.CNIC,
                            Mobile = x.Mobile
                        })
                        .ToList();

                    members.AddRange(transferMembers);
                    return FormatMembers(members);
                }


                var currentPropBooking = _db.Booking
                    .FirstOrDefault(x => !x.IsDeleted &&
                                         x.StockCreationId == stockId &&
                                         x.MemberProfileId == Convert.ToInt32(principalMember.Mobile));

                if (currentPropBooking != null)
                {
                    var bookingMembers = _db.BookingJointMember
                        .Where(x => x.BookingId == currentPropBooking.Id)
                        .Select(x => new JointMemberDto
                        {
                            Name = x.Name,
                            Cnic = x.CNIC,
                            Mobile = x.Mobile
                        })
                        .ToList();

                    members.AddRange(bookingMembers);
                    return FormatMembers(members);
                }


                var historicalMembers = _db.JointMemberHistoricalData
                    .Where(x => x.StockCreationId == stockId)
                    .Select(x => new JointMemberDto
                    {
                        Name = x.Name,
                        Cnic = x.CNIC,
                        Mobile = x.Mobile
                    })
                    .ToList();

                members.AddRange(historicalMembers);

                return FormatMembers(members);
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public string GetJointMembersByStockIdAndUserId(int stockId, int userId, string cnic)
        {
            try
            {
                var members = new List<JointMemberDto>();

                var principalMember = _db.MemberProfile
                    .Where(x => x.Id == userId)
                    .Select(x => new JointMemberDto
                    {
                        Name = $"{x.HonorificsName}. {x.MemberName}",
                        Mobile = x.Mobile,
                        Cnic = x.Cnic,
                    })
                    .FirstOrDefault();

                if (principalMember != null)
                    members.Add(principalMember);

                // 🔹 1️⃣ Check Transfer First
                var currentPropTransfer = _db.TransferHistery
                    .Where(x => !x.IsDeleted &&
                                x.StockCreationId == stockId &&
                                x.MemberProfileId == userId &&
                                x.Remarks != "Hosted Ownery")
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();

                if (currentPropTransfer != null)
                {
                    var transferMembers = _db.TransferHisteryJointMember
                        .Where(x => x.TransferHisteryId == currentPropTransfer.Id)
                        .Select(x => new JointMemberDto
                        {
                            Name = x.Name,
                            Cnic = x.CNIC,
                            Mobile = x.Mobile
                        })
                        .ToList();

                    members.AddRange(transferMembers);
                    return FormatMembers(members);
                }


                var currentPropBooking = _db.Booking
                    .FirstOrDefault(x => !x.IsDeleted &&
                                         x.StockCreationId == stockId &&
                                         x.MemberProfileId == userId);

                if (currentPropBooking != null)
                {
                    var bookingMembers = _db.BookingJointMember
                        .Where(x => x.BookingId == currentPropBooking.Id)
                        .Select(x => new JointMemberDto
                        {
                            Name = x.Name,
                            Cnic = x.CNIC,
                            Mobile = x.Mobile
                        })
                        .ToList();

                    members.AddRange(bookingMembers);
                    return FormatMembers(members);
                }

                var historicalMembers = _db.JointMemberHistoricalData
                    .Where(x => x.StockCreationId == stockId)
                    .Select(x => new JointMemberDto
                    {
                        Name = x.Name,
                        Cnic = x.CNIC,
                        Mobile = x.Mobile
                    })
                    .ToList();

                members.AddRange(historicalMembers);

                return FormatMembers(members);
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
                .Select(mp => $"{mp.HonorificsName}. {mp.MemberName} ({mp.Cnic}) {mp.Relationship} {mp.RelationshipWith}")
                .ToList();

            return string.Join(", ", finalList);
        }

        private string GetImagesUrl(int stockId, int memberId)
        {
            string url = string.Empty;

            var principalMemberImage = _db.TransferHistery
              .Where(x => x.StockCreationId == stockId)
              .OrderByDescending(x => x.Id)
              .Select(x => x.CombineImage)
              .FirstOrDefault();

            if (!string.IsNullOrEmpty(principalMemberImage) && principalMemberImage != "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcThtjU0d_BQklzBkT7Hn7t48a5yaBVWIJa4i6PcFbFgt91JYcN-FPV0laysIBBD-VC-p-s&usqp=CAU")
            {
                url = principalMemberImage;
                return url;
            }
            else
            {
                var bookingImage = _db.Booking
               .Where(th => th.StockCreationId == stockId)
               .Select(th => th.ImageURL)
               .FirstOrDefault();

                if (!string.IsNullOrEmpty(principalMemberImage) && principalMemberImage != "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcThtjU0d_BQklzBkT7Hn7t48a5yaBVWIJa4i6PcFbFgt91JYcN-FPV0laysIBBD-VC-p-s&usqp=CAU")
                {
                    url = principalMemberImage;

                    return url;
                }
                else
                {
                    var memberImage = _db.MemberProfile
                          .Where(x => x.Id == memberId).Select(x => x.ImageURL)
                          .FirstOrDefault();

                    url = memberImage;

                    return url;
                }
            }

            return url;
        }

        private string GetAllotmentImagesUrl(int stockId, int memberId)
        {
            string url = string.Empty;

            var principalMemberImage = _db.Booking
                .Where(th => th.StockCreationId == stockId)
                .Select(th => th.ImageURL)
                .FirstOrDefault();

            if (!string.IsNullOrEmpty(principalMemberImage) && principalMemberImage != "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcThtjU0d_BQklzBkT7Hn7t48a5yaBVWIJa4i6PcFbFgt91JYcN-FPV0laysIBBD-VC-p-s&usqp=CAU")
            {
                url = principalMemberImage;

                return url;
            }
            else
            {
                var memberImage = _db.MemberProfile
                      .Where(x => x.Id == memberId).Select(x => x.ImageURL)
                      .FirstOrDefault();

                url = memberImage;

                return url;
            }

            return url;
        }


    }
}
