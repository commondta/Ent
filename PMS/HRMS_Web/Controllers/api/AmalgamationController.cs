using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using HRMS_Web.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmalgamationController : ControllerBase
    {
        #region Variables
        private readonly DataBase_Context _context;
        private readonly IConfiguration _configuration;
        CommonBLL _commonBLL;
        #endregion

        #region Constructor
        public AmalgamationController(DataBase_Context context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _commonBLL = new CommonBLL(_context);
        }
        #endregion

        #region Public Methods

        [HttpGet]
        [Route("GetAllAmalgamation")]
        public IActionResult GetAllAmalgamation(
       int draw,
       int start,
       int length,
       string? search = ""
   )
        {
            try
            {
                var query = _context.Amalgamation
                    .IgnoreQueryFilters()
                    .Include(a => a.StockCreation)
                    .Include(a => a.AmalgamationDetails)
                        .ThenInclude(d => d.StockCreation)
                    .AsQueryable();

                // 🔍 SEARCH (optional)
                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(a =>
                        (a.StockCreation != null && a.StockCreation.RegistrationNo.Contains(search)) ||
                        a.AmalgamationDetails.Any(d => d.StockCreation != null && d.StockCreation.RegistrationNo.Contains(search)) ||
                        (a.Remarks != null && a.Remarks.Contains(search))
                    );
                }

                var recordsTotal = query.Count();

                var data = query
                    .OrderByDescending(a => a.Id)
                    .Skip(start)
                    .Take(length)
                    .Select(a => new
                    {
                        AmalgamationId = a.Id,
                        RegistrationNo = a.StockCreation != null ? a.StockCreation.RegistrationNo : string.Empty,
                        PropertyNo = a.StockCreation != null ? a.StockCreation.PropertyNo : string.Empty,
                        Remarks = a.Remarks,
                        OldRegistrationNums = string.Join(", ",
                            a.AmalgamationDetails
                                .Where(d => d.StockCreation != null && d.StockCreation.RegistrationNo != null)
                                .Select(d => d.StockCreation.RegistrationNo)
                        ),
                        OldPropertyNums = string.Join(", ",
                            a.AmalgamationDetails
                                .Where(d => d.StockCreation != null && d.StockCreation.PropertyNo != null)
                                .Select(d => d.StockCreation.PropertyNo)
                        ),
                        Date = a.CreatedOn.ToString("dd-MM-yyyy")
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

        [HttpPost("Amalgamate")]
        public async Task<IActionResult> Amalgamate([FromBody] AmalgamationRequest request)
        {
            if (request == null || request.PropertyIds == null || request.PropertyIds.Count < 2)
                return BadRequest("Minimum 2 properties required");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                int totalSize = 0;

                // =========================
                // FETCH BASE PROPERTY
                // =========================
                var baseProperty = await _context.StockCreations
                    .FirstOrDefaultAsync(x => x.ID == request.PropertyIds.First());

                if (baseProperty == null)
                    return BadRequest("Base property not found");

                // =========================
                // INITIAL VALUES
                // =========================
                string smallestPropertyNo = baseProperty.PropertyNo;
                string prefixProperty = baseProperty.PrefixProperty;
                string postfixProperty = baseProperty.postfixForProperty;

                // =========================
                // LOOP ALL PROPERTIES
                // =========================
                foreach (var id in request.PropertyIds)
                {
                    var property = await _context.StockCreations.FindAsync(id);

                    if (property == null)
                        return BadRequest($"Property not found: {id}");


                    // sum size
                    totalSize += Convert.ToInt32(property.ActualSize);

                    // find smallest property number
                    if (int.TryParse(property.PropertyNo, out int currentNo) &&
                        int.TryParse(smallestPropertyNo, out int smallestNo))
                    {
                        if (currentNo < smallestNo)
                        {
                            smallestPropertyNo = property.PropertyNo;
                            prefixProperty = property.PrefixProperty;
                            postfixProperty = property.postfixForProperty;
                        }
                    }
                }

                // =========================
                // BUILD REGISTRATION NO
                // =========================
                if (request.Postfix != "-1" && request.Number > 0)
                {
                    if (request.Postfix != "-1")
                    {
                        request.NewRegistrationNo = request.Prefix + String.Format("{0:0000}", request.Number) + request.Postfix;
                    }
                    else
                    {
                        request.NewRegistrationNo = request.Prefix + String.Format("{0:0000}", request.Number);
                    }
                }


                // =========================
                // CLONE BASE PROPERTY
                // =========================
                var newStock = new StockCreation();

                _context.Entry(newStock).CurrentValues.SetValues(baseProperty);

                newStock.ID = 0; // reset PK

                // =========================
                // OVERRIDE REQUIRED FIELDS
                // =========================

                // Registration
                newStock.RegistrationNo = request.NewRegistrationNo;
                newStock.PrefixRegistration = request.Prefix;
                newStock.numForRegistration = request.Number;
                newStock.postfixForRegistration = request.Postfix;

                // Property Info
                newStock.PropertyNo = smallestPropertyNo;
                newStock.PrefixProperty = prefixProperty;
                newStock.postfixForProperty = postfixProperty;
                newStock.numForProperty = int.TryParse(smallestPropertyNo, out int propNo) ? propNo : 0;

                // Size
                newStock.ActualSize = totalSize.ToString();

                // Category / Type
                newStock.Category = request.CategoryId?.ToString();
                newStock.Type = request.TypeId?.ToString();

                // =========================
                // SAVE NEW PROPERTY
                // =========================
                _context.StockCreations.Add(newStock);
                await _context.SaveChangesAsync();

                // =========================
                // CREATE AMALGAMATION MASTER
                // =========================
                var amalgamation = new Amalgamation
                {
                    StockCreationId = newStock.ID,
                    Remarks = request.Remarks,
                    CreatedOn = DateTime.Now,
                    LastModifiedUserName = request.UserName
                };

                _context.Amalgamation.Add(amalgamation);
                await _context.SaveChangesAsync();

                // =========================
                // CREATE DETAILS + UPDATE OLD
                // =========================
                foreach (var id in request.PropertyIds)
                {
                    _context.AmalgamationDetails.Add(new AmalgamationDetails
                    {
                        AmalgamationId = amalgamation.Id,
                        StockCreationId = id
                    });

                    var property = await _context.StockCreations.FindAsync(id);
                }

                await _context.SaveChangesAsync();

                // =========================
                // COMMIT
                // =========================
                await transaction.CommitAsync();


                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Amalgamation successful",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.BadRequest,
                    Message = "Amalgamation successful",
                    Data = null
                });
            }
        }

        [HttpGet]
        [Route("GetAllotmentLetter")]
        public IActionResult GetAmalgamationLetter(int id)
        {
            try
            {
                var query = _context.Amalgamation
    .IgnoreQueryFilters()
    .Include(a => a.StockCreation)
    .Include(a => a.AmalgamationDetails)
        .ThenInclude(d => d.StockCreation)
    .FirstOrDefault(x => x.Id == id);


                // Main stock
                var stockDetails = query.StockCreation;

                // Safe access for details
                var stockDetails1 = query.AmalgamationDetails
                    .ElementAtOrDefault(0)?.StockCreation;

                var stockDetails2 = query.AmalgamationDetails
                    .ElementAtOrDefault(1)?.StockCreation;

                var buyerProfile = _context.MemberProfile.FirstOrDefault(d => d.Id == stockDetails.MemberProfileId);
                var propertyType = _context.PropertyTypes.FirstOrDefault(y => y.ID == Convert.ToInt32(stockDetails.Type));
                var realEstateType = _context.Real_Estates.FirstOrDefault(y => y.ID == Convert.ToInt32(stockDetails.RealStateType));
                var block = _context.Blocks.FirstOrDefault(y => y.ID == Convert.ToInt32(stockDetails.Block));
                var natureEntity = _context.Natures.FirstOrDefault(y => y.ID == Convert.ToInt32(stockDetails.Nature));
                var phase = _context.Phases.FirstOrDefault(y => y.ID == Convert.ToInt32(stockDetails.Phase));
                var category = _context.Categories.FirstOrDefault(y => y.ID == Convert.ToInt32(stockDetails.Category));
                int? prefixId = null;
                if (int.TryParse(stockDetails?.PrefixProperty, out var parsed))
                {
                    prefixId = parsed;
                }

                var sector = prefixId.HasValue
                    ? _context.Sectors.FirstOrDefault(y => y.ID == prefixId.Value)
                    : null;

                var detail = _context.SAPOperations.FirstOrDefault();

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
                    AmalgamatedPlot1 = stockDetails1.PropertyNo,
                    AmalgamatedPlot2 = stockDetails2.PropertyNo,
                    AmalgamatedReg1 = stockDetails1.RegistrationNo,
                    AmalgamatedReg2 = stockDetails2.RegistrationNo,
                    AmalgamatedSector = _context.Sectors.FirstOrDefault(s => s.ID == int.Parse(stockDetails1.PrefixProperty))?.Description,
                    AmalgamatedPhase = _context.Phases.FirstOrDefault(p => p.ID == int.Parse(stockDetails1.Phase))?.Description

                };


                if (result != null)
                {
                    result.ImageUrl = GetImagesUrl(id, buyerProfile.Id);
                    result.BuyerWithJointMembers = GetJointMembersByStockIdAndUserId(id, (int)stockDetails.MemberProfileId, buyerProfile.Cnic);
                    // result.SellerWithJointMembers = GetSellerAndJointMembersByStockIdAndUserId(id);
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

        private string GetImagesUrl(int stockId, int memberId)
        {
            string url = string.Empty;

            var principalMemberImage = _context.TransferHistery
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
                var bookingImage = _context.Booking
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
                    var memberImage = _context.MemberProfile
                          .Where(x => x.Id == memberId).Select(x => x.ImageURL)
                          .FirstOrDefault();

                    url = memberImage;

                    return url;
                }
            }

            return url;
        }

        private string FormatMembers(List<JointMemberDto> jointMembers)
        {
            if (jointMembers == null || !jointMembers.Any())
                return string.Empty;

            var memberCnics = jointMembers
                .Select(j => j.Cnic)
                .Distinct() 
                .ToList();

            var memberProfiles = _context.MemberProfile
                .Where(m => memberCnics.Contains(m.Cnic))
                .ToList() 
                .GroupBy(m => m.Cnic) 
                .Select(g => g.First()) 
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

        public string GetJointMembersByStockIdAndUserId(int stockId, int userId, string cnic)
        {
            try
            {
                var members = new List<JointMemberDto>();

                var principalMember = _context.MemberProfile
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
                var currentPropTransfer = _context.TransferHistery
                    .Where(x => !x.IsDeleted &&
                                x.StockCreationId == stockId &&
                                x.MemberProfileId == userId &&
                                x.Remarks != "Hosted Ownery")
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();

                if (currentPropTransfer != null)
                {
                    var transferMembers = _context.TransferHisteryJointMember
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


                var currentPropBooking = _context.Booking
                    .FirstOrDefault(x => !x.IsDeleted &&
                                         x.StockCreationId == stockId &&
                                         x.MemberProfileId == userId);

                if (currentPropBooking != null)
                {
                    var bookingMembers = _context.BookingJointMember
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

                var historicalMembers = _context.JointMemberHistoricalData
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

        #endregion
    }
}
