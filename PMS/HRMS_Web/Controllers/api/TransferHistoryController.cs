using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using B_Utility.Common.Enums;
using HRMS_Web.Extensions;
using HRMS_Web.Models.DTOs;
using HRMS_Web.Services.AlertService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]

    public class TransferHisteryController : ControllerBase
    {
        private readonly DataBase_Context _db;
        private readonly IAlertService alertService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        CommonBLL _commonBLL;
        ApprovalBLL _approvalBLL;
        public TransferHisteryController(DataBase_Context db, IAlertService alertService, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            this.alertService = alertService;
            _httpContextAccessor = httpContextAccessor;
            _approvalBLL = new ApprovalBLL(_db);
            _commonBLL = new CommonBLL(_db);
        }

        [HttpGet]
        [Route("GetOwnershipAgreement")]
        public IActionResult GetOwnershipAgreement(int id)
        {
            try
            {
                var transferHistory = _db.TransferHistery
                    .Include(x => x.MemberProfile)
                    .Include(x => x.StockCreation)
                    .FirstOrDefault(x => x.Id == id && x.IsTransferApproved == true);

                if (transferHistory == null)
                    return NotFound("No record found.Please wait for letter print approval");

                var dealer = string.IsNullOrEmpty(transferHistory.DealerCode) ? null : _db.Dealers.FirstOrDefault(d => d.Id == Convert.ToInt32(transferHistory.DealerCode));
                var sellerProfile = _db.MemberProfile.FirstOrDefault(d => d.Cnic == transferHistory.SellerCnic);
                var buyerProfile = _db.MemberProfile.FirstOrDefault(d => d.Id == transferHistory.BuyerId);
                var propertyType = _db.PropertyTypes.FirstOrDefault(y => y.ID == Convert.ToInt32(transferHistory.StockCreation.Type));
                var block = _db.Blocks.FirstOrDefault(y => y.ID == Convert.ToInt32(transferHistory.StockCreation.Block));
                var natureEntity = _db.Natures.FirstOrDefault(y => y.ID == Convert.ToInt32(transferHistory.StockCreation.Nature));
                var phase = _db.Phases.FirstOrDefault(y => y.ID == Convert.ToInt32(transferHistory.StockCreation.Phase));
                var category = _db.Categories.FirstOrDefault(y => y.ID == Convert.ToInt32(transferHistory.StockCreation.Category));
                int? prefixId = null;
                if (int.TryParse(transferHistory.StockCreation?.PrefixProperty, out var parsed))
                {
                    prefixId = parsed;
                }

                var sector = prefixId.HasValue
                    ? _db.Sectors.FirstOrDefault(y => y.ID == prefixId.Value)
                    : null;

                var detail = _db.SAPOperations.FirstOrDefault();

                var result = new ownershipAgreementDto
                {
                    SignatoryRank = detail.SignatoryRank,
                    SignatoryDesignation = detail.SignatoryDesignation,
                    SignatoryName = detail.SignatoryName,
                    SellerName = $"{sellerProfile?.HonorificsName}. {transferHistory.SellerName}",
                    DealerName = transferHistory.DealerName,
                    EstateName = transferHistory.EstateName,
                    LegalHeireType = transferHistory.LegalHeireType,
                    LetterDate = transferHistory.LetterDate,
                    LagalHeireContent = transferHistory.LagalHeireContent,
                    DealerCnic = dealer?.CNIC,
                    RelationshipSeller = sellerProfile?.Relationship,
                    RelationshipWithSeller = sellerProfile?.RelationshipWith,
                    RelationshipBuyer = transferHistory.MemberProfile?.Relationship,
                    RelationshipWithBuyer = transferHistory.MemberProfile?.RelationshipWith,
                    BuyerName = $"{transferHistory.MemberProfile?.HonorificsName}. {transferHistory.MemberProfile?.MemberName}",
                    BuyerCnic = transferHistory.MemberProfile?.Cnic,
                    MembershipNo = transferHistory.MemberProfile?.MEMBERSHIPNO ?? "",
                    BuyerPhone = transferHistory.MemberProfile?.Mobile ?? "",
                    SellerCnic = transferHistory.SellerCnic,
                    DealerRegistrationNo = transferHistory.DealerCode,
                    PermanentAddress = transferHistory.MemberProfile?.CurrentAddress,
                    RegistrationNo = transferHistory.StockCreation?.RegistrationNo,
                    IsLetterPrint = transferHistory.IsTransferApproved,
                    PropertyNo = transferHistory.StockCreation?.PropertyNo ?? "",
                    ConstructionStatus = transferHistory.StockCreation?.ConstracutionStatus,
                    Area = transferHistory.StockCreation?.ActualSize,
                    UnitArea = transferHistory.StockCreation?.ActualSizeUnit,
                    Sqft = transferHistory.StockCreation?.coveredArea == null ? "N/A" : transferHistory.StockCreation.coveredArea.ToString(),
                    Type = propertyType?.Description ?? "N/A",
                    Block = block?.Description ?? "N/A",
                    Category = category?.Description ?? "N/A",
                    Nature = (natureEntity.Description == "Plot" && transferHistory.StockCreation?.ConstracutionStatus == "Constructed")
                                ? "House"
                                : natureEntity?.Description,
                    SectorName = sector?.Description ?? "",
                    Phase = phase?.Description ?? "N/A",
                    docDate = transferHistory.CreatedOn.Date,
                    ApplyStation = transferHistory.ApplyStation,
                    TransferReciptId = transferHistory.ReciptPrpcessingId,
                    BuyerRepresentativeName = transferHistory.BuyerRepresentativeName,
                    BuyerRepresentativeRelationshipWith = transferHistory.BuyerRepresentativeRelationshipWith,
                    BuyerRepresentativeCnic = transferHistory.BuyerRepresentativeCnic,
                    Statement = transferHistory.Statement,
                    Images = GetAllImagesUrl(id),
                    MemberNames = GetAllMemberNames(id),
                    SellerJointMembers = GetJointMembersByStockIdAndUserId(transferHistory.StockCreation.ID,sellerProfile.Id,sellerProfile.Cnic)
                };

                if (result != null)
                {
                    result.TransferType = GetTransferType((int)result.TransferReciptId);
                    result.SwapOverStatement = GetChangeOverStatement((int)result.TransferReciptId);
                    result.Title = string.IsNullOrEmpty(result.PropertyNo) ? $"Reference Number {result?.RegistrationNo}" : $"{result.Nature} Number {result?.Phase}/{result?.SectorName}/{result?.PropertyNo}";
                    result.PlotNo = $"{result.Nature} Number {result?.PropertyNo}";
                    result.BuyerDetail = $"{sellerProfile?.HonorificsName}.{sellerProfile?.MemberName} {sellerProfile?.Relationship} {sellerProfile?.RelationshipWith} ({sellerProfile?.Cnic}){(string.IsNullOrEmpty(result.SellerJointMembers) ? "" : $", {result.SellerJointMembers}")}";
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

        private List<Models.DTOs.MemberName> GetAllMemberNames(int id)
        {
            var MemberLists = new List<Models.DTOs.MemberName>();

            var principalMemberImage = _db.TransferHistery
                .Where(th => th.Id == id)
                .Select(x => new Models.DTOs.MemberName
                {
                    MemeberName = $"{x.MemberProfile.HonorificsName}. {x.MemberProfile.MemberName}",
                    Relationhipwith = x.MemberProfile.RelationshipWith,
                    RelationName = x.MemberProfile.Relationship,
                    Cnic = $"({x.MemberProfile.Cnic})"
                }).FirstOrDefault();

            MemberLists.Add(principalMemberImage);


            var jointMemberCnics = _db.TransferHisteryJointMember
                .Where(jm => jm.TransferHisteryId == id)
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


        public string GetJointMembersByStockIdAndUserId(int stockId, int userId, string cnic)
        {
            try
            {
                List<JointMemberDto> jointMembers;

                // 🔹 1️⃣ Check Transfer Joint Members first
                var currentPropTransfer = _db.TransferHistery
                    .Where(x => !x.IsDeleted && x.StockCreationId == stockId && x.MemberProfileId == userId && x.Remarks != "Hosted Ownery")
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();

                if (currentPropTransfer != null)
                {
                    jointMembers = _db.TransferHisteryJointMember
                        .Where(x => x.TransferHisteryId == currentPropTransfer.Id)
                        .Select(x => new JointMemberDto
                        {
                            Name = x.Name,
                            Cnic = x.CNIC,
                            Mobile = x.Mobile
                        })
                        .ToList();

                    return FormatMembers(jointMembers);
                }

                // 🔹 2️⃣ If not transfer, check Booking
                var currentPropBooking = _db.Booking
                    .FirstOrDefault(x => !x.IsDeleted && x.StockCreationId == stockId && x.MemberProfileId == userId);

                if (currentPropBooking != null)
                {
                    jointMembers = _db.BookingJointMember
                        .Where(x => x.BookingId == currentPropBooking.Id)
                        .Select(x => new JointMemberDto
                        {
                            Name = x.Name,
                            Cnic = x.CNIC,
                            Mobile = x.Mobile
                        })
                        .ToList();

                    return FormatMembers(jointMembers);
                }

                // 🔹 3️⃣ If neither Transfer nor Booking, get historical
                jointMembers = _db.JointMemberHistoricalData
                    .Where(x => x.StockCreationId == stockId)
                    .Select(x => new JointMemberDto
                    {
                        Name = x.Name,
                        Cnic = x.CNIC,
                        Mobile = x.Mobile
                    })
                    .ToList();

                return FormatMembers(jointMembers);
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

        private List<Url> GetAllImagesUrl(int id)
        {
            var urls = new List<Url>();

            var principalMemberImage = _db.TransferHistery
                .Where(th => th.Id == id)
                .Select(th => th.CombineImage)
                .FirstOrDefault();

            if (!string.IsNullOrEmpty(principalMemberImage) && principalMemberImage != "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcThtjU0d_BQklzBkT7Hn7t48a5yaBVWIJa4i6PcFbFgt91JYcN-FPV0laysIBBD-VC-p-s&usqp=CAU")
            {
                urls.Add(new Url { imageUrl = principalMemberImage });
            }
            else
            {
                var member = _db.TransferHistery.Where(th => th.Id == id).FirstOrDefault();

                if (member != null)
                {
                    var jointMembersImageUrls = _db.MemberProfile
                          .Where(x => x.Id == member.MemberProfileId).Select(x => x.ImageURL)
                          .FirstOrDefault();



                    urls.Add(new Url { imageUrl = jointMembersImageUrls });
                }
            }

                //var jointMembersCnics = _db.TransferHisteryJointMember
                //    .Where(jm => jm.TransferHisteryId == id)
                //    .ToList();

                //var jointMembersImageUrls = _db.MemberProfile
                //    .Where(mp => jointMembersCnics.Select(jm => jm.CNIC).Contains(mp.Cnic))
                //    .Select(mp => mp.ImageURL)
                //    .ToList();

                //urls.AddRange(jointMembersImageUrls.Select(url => new Url { imageUrl = url }));

                return urls;
        }



        [HttpGet]
        [Route("GetAllAvailablePropertiesForPrint")]
        public IActionResult GetAllAvailablePropertiesForPrint()
        {
            try
            {
                var result = _db.TransferHistery.Where(x => !x.IsDeleted && x.IsActive != false)
                                                .Include(x => x.MemberProfile)
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
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _db.TransferHistery.Where(x => !x.IsDeleted)
                                                       .Include(x => x.TransferHistoryJointMember.Where(x => !x.IsDeleted))
                                                       .Include(x => x.TransferHistoryNominee.Where(x => !x.IsDeleted))
                                                       .Include(x => x.StockCreation)
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
                var result = _db.TransferHistery.Where(x => !x.IsDeleted && x.Id == id)
                                                       .Include(x => x.TransferHistoryJointMember.Where(x => !x.IsDeleted))
                                                       .Include(x => x.TransferHistoryNominee.Where(x => !x.IsDeleted))
                                                       .Include(x => x.MemberProfile)
                                                       .Include(x => x.StockCreation)
                                                       .AsNoTracking()
                                                       .FirstOrDefault();
                if (result != null)
                {
                    result.TransferSetReceivingAttachments = GetTransferSetReceveingAttachments((int)result.StockCreationId);
                    var transferReceipt = _db.TransferReceiptProcessing.Where(x => x.StockCreationId == result.StockCreationId)
                                                                  .OrderByDescending(x => x.Id)
                                                                  .Select(x => new
                                                                  {
                                                                      x.BuyerId,
                                                                      x.DealerCode,
                                                                      x.EstateName,
                                                                      x.DealerName,
                                                                      x.LegalHeirType,
                                                                      x.LegalHeirContent,
                                                                      x.CombineImage,
                                                                      Attachments = x.TransferAttachments,
                                                                      TransferReceiptJointMember = x.TransferReceiptJointMember,
                                                                      TransferReceiptNominee = x.TransferReceiptNominee,
                                                                      BuyerTaxes = x.BuyerTaxes,
                                                                      SellerTaxes = x.SellerTaxes,
                                                                  })
                                                                  .AsNoTracking()
                                                                  .FirstOrDefault();

                    result.CombineImage = transferReceipt.CombineImage;
                    result.LegalHeireType = transferReceipt.LegalHeirType;
                    result.LagalHeireContent = transferReceipt.LegalHeirContent;
                    result.TransferReceiptNominee = transferReceipt.TransferReceiptNominee;

                    var cnics = transferReceipt.TransferReceiptJointMember
                         .Select(x => x.CNIC)
                         .ToList();

                    var profiles = _db.MemberProfile
                         .Where(x => cnics.Contains(x.Cnic))
                         .AsEnumerable()    // or .ToList()
                         .GroupBy(x => x.Cnic)
                         .ToDictionary(
                             g => g.Key,
                             g => g.First().ImageURL
                         );

                    foreach (var transfer in transferReceipt.TransferReceiptJointMember)
                    {
                        transfer.ImageURL = profiles.TryGetValue(transfer.CNIC, out var imageUrl)
                            ? imageUrl
                            : "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcThtjU0d_BQklzBkT7Hn7t48a5yaBVWIJa4i6PcFbFgt91JYcN-FPV0laysIBBD-VC-p-s&usqp=CAU";
                    }
                    result.TransferReceiptJointMember = transferReceipt.TransferReceiptJointMember;
                    result.TransferAttachments = transferReceipt.Attachments;
                    result.BuyerTaxes = transferReceipt.BuyerTaxes;
                    result.SellerTaxes = transferReceipt.SellerTaxes;
                    result.BuyerId = transferReceipt.BuyerId;
                    result.DealerCode = transferReceipt.DealerCode;
                    result.DealerName = transferReceipt.DealerName;
                    result.EstateName = transferReceipt.EstateName;
                    result.StockCreation.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(result.StockCreation.RealStateType));
                    result.StockCreation.ProjectName = _commonBLL.GetProjectName(Convert.ToInt32(result.StockCreation.Project));
                    result.StockCreation.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(result.StockCreation.Phase));
                    result.StockCreation.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(result.StockCreation.Category));
                    result.StockCreation.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(result.StockCreation.Block));
                    result.StockCreation.NatureName = _commonBLL.GetNatureName(Convert.ToInt32(result.StockCreation.Nature));
                    result.StockCreation.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(result.StockCreation.Type));
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

        private List<TransferSetReceivingAttachments> GetTransferSetReceveingAttachments(int stockId)
        {
            return _db.TransferSetReceivingAttachments.Where(x => x.TransferSetReceiving.StockCreationId == stockId)
                                                      .ToList();
        }

        [HttpGet]
        [Route("GetByIdFindMode")]
        public IActionResult GetByIdFindMode(int id)
        {
            try
            {
                var result = _db.TransferHistery.Where(x => !x.IsDeleted && x.Id == id)
                                                       .Include(x => x.TransferHistoryJointMember.Where(x => !x.IsDeleted))
                                                       .Include(x => x.TransferHistoryNominee.Where(x => !x.IsDeleted))
                                                       .Include(x => x.TransferHistoryAttachments)
                                                       .Include(x => x.MemberProfile)
                                                       .Include(x => x.StockCreation)
                                                       .AsSplitQuery()
                                                       .AsNoTracking()
                                                       .FirstOrDefault();
                if (result != null)
                {
                    var transferReceipt = _db.TransferReceiptProcessing.Where(x => x.StockCreationId == result.StockCreationId)
                                                                .OrderByDescending(x => x.Id)
                                                                .Select(x => new
                                                                {
                                                                    x.BuyerId,
                                                                    x.DealerCode,
                                                                    x.EstateName,
                                                                    x.DealerName,
                                                                    Attachments = x.TransferAttachments,
                                                                    BuyerTaxes = x.BuyerTaxes,
                                                                    SellerTaxes = x.SellerTaxes,
                                                                })
                                                                .AsNoTracking()
                                                                .FirstOrDefault();
                    result.TransferAttachments = transferReceipt.Attachments;
                    result.SellerTaxes = transferReceipt.SellerTaxes;
                    result.BuyerTaxes = transferReceipt.BuyerTaxes;
                    result.BuyerId = _db.MemberProfile.Where(x => x.Cnic == result.SellerCnic)
                                                                  .FirstOrDefault().Id;

                    var cnics = result.TransferHistoryJointMember
                         .Select(x => x.CNIC)
                         .ToList();

                    var profiles = _db.MemberProfile
    .Where(x => cnics.Contains(x.Cnic))
    .AsEnumerable()    // or .ToList()
    .GroupBy(x => x.Cnic)
    .ToDictionary(
        g => g.Key,
        g => g.First().ImageURL
    );

                    foreach (var transfer in result.TransferHistoryJointMember)
                    {
                        transfer.ImageURL = profiles.TryGetValue(transfer.CNIC, out var imageUrl)
                            ? imageUrl
                            : "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcThtjU0d_BQklzBkT7Hn7t48a5yaBVWIJa4i6PcFbFgt91JYcN-FPV0laysIBBD-VC-p-s&usqp=CAU";
                    }

                    result.StockCreation.RealStateTypeName = _commonBLL.GetRealEstateName(Convert.ToInt32(result.StockCreation.RealStateType));
                    result.StockCreation.ProjectName = _commonBLL.GetProjectName(Convert.ToInt32(result.StockCreation.Project));
                    result.StockCreation.PhaseName = _commonBLL.GetPhaseName(Convert.ToInt32(result.StockCreation.Phase));
                    result.StockCreation.CategoryName = _commonBLL.GetCategoryName(Convert.ToInt32(result.StockCreation.Category));
                    result.StockCreation.BlockName = _commonBLL.GetBlockName(Convert.ToInt32(result.StockCreation.Block));
                    result.StockCreation.NatureName = _commonBLL.GetNatureName(Convert.ToInt32(result.StockCreation.Nature));
                    result.StockCreation.TypeName = _commonBLL.GetTypeName(Convert.ToInt32(result.StockCreation.Type));
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
        [Route("AddNewTransferHistery")]
        public async Task<IActionResult> AddNewTransferHisteryAsync(TransferHistery model)
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

                var approvalStatus = _db.ApprovalUI.Find((int)ApprovalUIIds.Transfer);
                if (approvalStatus != null)
                {
                    if (approvalStatus.Checked != true)
                    {
                        isApprovalActive = false;
                    }
                }

                var approvalSetup = _db.ApprovalSetup.Where(x => x.ApprovalUIId == (int)ApprovalUIIds.Transfer).ToList();
                if (approvalSetup.Count <= 0 && isApprovalActive == true)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Approval setup not defined or In-active",
                        Data = null
                    });
                }

                //if (_db.MemberProfile.Any(x => x.MEMBERSHIPNO == model.SellerMembershipNo))
                //{
                //    return Ok(new ApiResponse<object>
                //    {
                //        Code = ResponseCode.BadRequest,
                //        Message = "Seller Membership No already exist",
                //        Data = null
                //    });
                //}

                if (_db.MemberProfile.Any(x => x.MEMBERSHIPNO == model.BuyerMembershipNo))
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "Buyer Membership No already exist",
                        Data = null
                    });
                }

                model.IsActive = true;
                model.CreatedOn = model.CreatedOn.Date.Add(DateTime.Now.TimeOfDay);
                model.CreatedBy = model.CreatedBy;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;

                // use in revenue report
                int ReciptPrpcessingId = (int)_db.TransferHistery
                                                          .Where(x => x.StockCreationId == model.StockCreationId)
                                                          .OrderByDescending(x => x.Id)
                                                          .FirstOrDefault().ReciptPrpcessingId;

                model.ReciptPrpcessingId = ReciptPrpcessingId;
                // end

                if (model.TransferHistoryJointMember?.Count > 0)
                {
                    foreach (var item in model.TransferHistoryJointMember)
                    {
                        UpdateJointMember(item);
                        item.ModifiedBy = model.ModifiedBy;
                        item.CreatedBy = model.CreatedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.LastModified = DateTime.Now;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }
                }

                if (model.TransferHistoryNominee?.Count > 0)
                {
                    foreach (var item in model.TransferHistoryNominee)
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

                if (model.TransferHistoryAttachments?.Count() > 0)
                {
                    foreach (var item in model.TransferHistoryAttachments)
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
                model.CombineImage = (string.IsNullOrEmpty(model.CombineImage) || !model.CombineImage.StartsWith("data:")) ? model.CombineImage : $"{path}{await model.CombineImage.SaveBase64FileAsync()}";
                model.TransferFromImage = (string.IsNullOrEmpty(model.TransferFromImage) || !model.TransferFromImage.StartsWith("data:")) ? model.TransferFromImage : $"{path}{await model.TransferFromImage.SaveBase64FileAsync()}";
                model.TransferToImage = (string.IsNullOrEmpty(model.TransferToImage) || !model.TransferToImage.StartsWith("data:")) ? model.TransferToImage : $"{path}{await model.TransferToImage.SaveBase64FileAsync()}";

                _db.TransferHistery.Add(model);
                UpdateSeller(model);
                UpdateBuyer(model);

                var request = _db.NDCRequestForMember.Where(x => x.StockCreationId == model.StockCreationId && x.IsCanceled != true).ToList();
                if (request.Count() > 0)
                {
                    foreach(var item in request)
                    {
                      item.IsRequestedClosed = true;
                      item.ValidityDate = DateTime.Now.AddDays(-1);
                    }
                }

                StockCreation stockCreation = _db.StockCreations.Find(model.StockCreationId);
                if (stockCreation != null)
                {
                    if(stockCreation.ConstracutionStatus == "Non-Constructed") {
                        stockCreation.GrancePeriodForBillGenration = DateTime.Now.AddMonths(GetTransferGracePeriod());
                    }
                    stockCreation.MemberProfileId = model.MemberProfileId;
                    if (!string.IsNullOrEmpty(model.DealerCode))
                    {
                        stockCreation.DealerId = Convert.ToInt32(model.DealerCode);
                    }
                    _db.Entry(stockCreation).State = EntityState.Modified;
                }

                if (stockCreation != null)
                {
                    var surrenderList = _db.Surrender.Where(x => x.StockCreationId == stockCreation.ID).ToList();
                    foreach (var item in surrenderList)
                    {
                        item.IsRequestClosed = true;
                    }
                }

                Response_Result responseForContactPersonAddition = new SapIntegrationController(_db).UpdateMemberProfileToAddContactPerson((int)model.StockCreationId,(int)model.MemberProfileId);
                //if (responseForContactPersonAddition != null)
                //{

                //}

                var stock = _db.StockCreations.Where(x => x.ID == model.StockCreationId).Include(x => x.MemberProfile).Select(x => new { x.RegistrationNo, x.MemberProfile.MemberName }).FirstOrDefault();
                string narration = $"Transfer of Seller: {stock.MemberName} and Buyer {model.BuyerName} having ReferenceNo: {stock.RegistrationNo} is successfully executed by {model.LastModifiedUserName}";
                alertService.PushAlert(5, narration);

                // save all queries once
                _db.SaveChanges();

                string message = string.Empty;
                TransferHistery TransferHistery = (TransferHistery)_db.TransferHistery.Where(x => x.Id == model.Id)
                                                                                      .FirstOrDefault();
                if (TransferHistery != null)
                {

                    TransferHistery.IsTransferRequested = true;

                    var transferList = _db.TransferHistery.Where(x => x.StockCreationId == stockCreation.ID).ToList();
                    foreach (var item in transferList)
                    {
                        item.IsRequestClosed = true;
                    }

                    _db.SaveChanges();

                    if (isApprovalActive == true)
                    {
                        bool result = _approvalBLL.AddNewApprovalSetup(model.Id, (int)ApprovalUIIds.Transfer);
                        message = "Transfer added succesfully and moved for approval";
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
                        TransferHistery.IsTransferApproved = true;

                        _db.SaveChanges();
                       
                      
                        message = "Transfer added succesfully";

                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = message,
                            Data = null
                        });
                    }
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

        [HttpPost]
        [Route("UpdateTransferHistery")]
        public async Task<IActionResult> UpdateTransferHisteryAsync(TransferHistery model)
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

                var data = _db.TransferHistery.Find(model.Id);

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
                    data.InternalDocumentNo = model.InternalDocumentNo;
                    data.InternalDocumentNoOptional = model.InternalDocumentNoOptional;
                    data.MemberProfileId = model.MemberProfileId;
                    data.Remarks = model.Remarks;
                    data.ModifiedBy = model.ModifiedBy;
                    data.LastModifiedUserName = model.LastModifiedUserName;
                    data.LastModified = DateTime.Now;
                    data.SellerRepresentativeName = model.SellerRepresentativeName;
                    data.SellerRepresentativeCnic = model.SellerRepresentativeCnic;
                    data.SellerRepresentativeRelationshipWith = model.SellerRepresentativeRelationshipWith;
                    data.SellerStation = model.SellerStation;
                    data.BuyerRepresentativeName = model.BuyerRepresentativeName;
                    data.BuyerRepresentativeRelationshipWith = model.BuyerRepresentativeRelationshipWith;
                    data.BuyerRepresentativeCnic = model?.BuyerRepresentativeCnic;
                    data.BuyerStation = model?.BuyerStation;
                    data.LagalHeireContent = model?.LagalHeireContent;
                    data.LegalHeireType = model?.LegalHeireType;
                    data.LetterDate = model?.LetterDate;
                    data.Statement = model?.Statement;
                    UpdateSeller(model);
                    UpdateBuyer(model);
                    _db.Entry(data).State = EntityState.Modified;

                    var result = _db.TransferHisteryJointMember.Where(x => x.TransferHisteryId == model.Id).ToList();

                    _db.TransferHisteryJointMember.RemoveRange(result);

                    foreach (var item in model.TransferHistoryJointMember)
                    {
                        UpdateJointMember(item);
                        item.TransferHisteryId = data.Id;
                        item.ModifiedBy = model.ModifiedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.LastModified = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;
                    }

                    _db.TransferHisteryJointMember.AddRange(model.TransferHistoryJointMember);


                    var result1 = _db.TransferHisteryNominee.Where(x => x.TransferHisteryId == model.Id).ToList();

                    _db.TransferHisteryNominee.RemoveRange(result1);


                    foreach (var item in model.TransferHistoryNominee)
                    {
                        item.TransferHisteryId = data.Id;
                        item.ModifiedBy = model.ModifiedBy;
                        item.LastModifiedUserName = model.LastModifiedUserName;
                        item.LastModified = DateTime.Now;
                        item.IsActive = true;
                        item.IsDeleted = false;

                        _db.TransferHisteryNominee.AddRange(model.TransferHistoryNominee);

                        StockCreation stockCreation = _db.StockCreations.Find(model.StockCreationId);
                        if (stockCreation != null)
                        {
                            stockCreation.MemberProfileId = model.MemberProfileId;
                            _db.Entry(stockCreation).State = EntityState.Modified;
                        }

                        Response_Result responseForContactPersonAddition = new SapIntegrationController(_db).UpdateMemberProfileToAddContactPerson((int)model.StockCreationId, (int)model.MemberProfileId);
                        //if (responseForContactPersonAddition != null)
                        //{

                        //}

                        
                    }

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
        [Route("DeleteTransferHistery")]
        public IActionResult DeleteTransferHistery(int id)
        {
            try
            {
                var model = _db.TransferHistery.Find(id);

                if (model != null)
                {
                    model.ModifiedBy = model.ModifiedBy;
                    model.LastModified = DateTime.Now;
                    model.IsActive = false;
                    model.IsDeleted = true;

                    _db.SaveChanges();

                    var TransferHisteryJointMembers = _db.TransferHisteryJointMember.Where(x => x.TransferHisteryId == model.Id).ToList();

                    if (TransferHisteryJointMembers?.Count > 0)
                    {
                        foreach (var item in TransferHisteryJointMembers)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;
                            _db.SaveChanges();
                        }
                    }

                    var TransferHisteryNominees = _db.TransferHisteryNominee.Where(x => x.TransferHisteryId == model.Id).ToList();

                    if (TransferHisteryNominees?.Count > 0)
                    {
                        foreach (var item in TransferHisteryNominees)
                        {
                            item.LastModified = DateTime.Now;
                            item.IsActive = false;
                            item.IsDeleted = true;
                            _db.SaveChanges();
                        }
                    }
                    var TransferHisteryAttachments = _db.TransferHisteryAttachments.Where(x => x.TransferHisteryId == model.Id).ToList();

                    if (TransferHisteryAttachments?.Count > 0)
                    {
                        foreach (var item in TransferHisteryAttachments)
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

        [HttpPost]
        [Route("CancelRequest")]
        public IActionResult CancelRequest(int id)
        {
            try
            {
                var model = _db.TransferHistery.Find(id);

                if (model != null)
                {
                    var request = _db.NDC1.Where(x => x.StockCreationId == model.StockCreationId && x.IsCanceled != true)
                                               .OrderBy(x => x.Id)
                                               .LastOrDefault();
                    var tranferrequest = _db.TransferHistery.Where(x => x.StockCreationId == model.StockCreationId && x.IsActive == true)
                                              .OrderBy(x => x.Id)
                                              .LastOrDefault();

                    int previousMember = _db.MemberProfile.Where(x => x.Cnic == model.SellerCnic).FirstOrDefault().Id;

                    var property = _db.StockCreations.FirstOrDefault(x => x.ID == model.StockCreationId);

                    if (request != null && tranferrequest != null && property != null && previousMember != 0)
                    {
                        request.IsGovtTaxRequested = true;
                        request.IsGovtTaxApproved = false;
                        tranferrequest.IsGovtProcessingTaxRequested = false;
                        tranferrequest.IsGovtProcessingTaxApproved = false;

                        property.MemberProfileId = previousMember;
                        model.MemberProfileId = previousMember;
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
                    Message = "Not found",
                    Data = null
                });

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }


        private void UpdateJointMember(TransferHisteryJointMember model)
        {
            var membetList = _db.MemberProfile.Where(x => x.Cnic == model.CNIC).ToList();

            foreach (var member in membetList)
            {
                member.MemberName = model.Name;
                member.RelationshipWith = model.Relationship;
                member.Cnic = model.CNIC;
                member.Mobile = model.Mobile;
                member.CurrentAddress = model.Address;
            }  
        }

        private void UpdateSeller(TransferHistery model)
        {
            var currentRecord = _db.MemberProfile.Find(model.SellerId);
            if (currentRecord != null)
            {
                var membetList = _db.MemberProfile.Where(x => x.Cnic == currentRecord.Cnic).ToList();

                foreach (var mem in membetList)
                {
                    mem.MemberName = model.SellerName;
                    mem.RelationshipWith = model.SellerRelationshipWith;
                    mem.Cnic = model.SellerCnic;
                    mem.PermanentAddress = model.SellerPermanentAddress;
                    mem.CurrentAddress = model.SellerCurrentAddress;
                }       
            }

            var member = _db.MemberProfile.Find(model.SellerId);
            if (member != null)
            {
                member.MEMBERSHIPNO = model.SellerMembershipNo;
            }
        }
        private void UpdateBuyer(TransferHistery model)
        {
            var currentRecord = _db.MemberProfile.Find(model.MemberProfileId);
            if (currentRecord != null)
            {
                var membetList = _db.MemberProfile.Where(x => x.Cnic == currentRecord.Cnic).ToList();

                foreach (var mem in membetList)
                {
                    mem.MemberName = model.BuyerName;
                    mem.RelationshipWith = model.BuyerRelationshipWith;
                    mem.Cnic = model.BuyerCnic;
                    mem.PermanentAddress = model.BuyerPermanentAddress;
                    mem.CurrentAddress = model.BuyerCurrentAddress;
                }
            }

            var member = _db.MemberProfile.Find(model.MemberProfileId);
            if (member != null)
            {
                member.MEMBERSHIPNO = model.BuyerMembershipNo;
            }
        }

        private int GetTransferGracePeriod()
        {
            var gracePeriod = _db.GracePeriodSetup.SingleOrDefault()?.TransferGracePeriod;
            return gracePeriod ?? 0; // return 0 if gracePeriod is null
        }
    }
}
