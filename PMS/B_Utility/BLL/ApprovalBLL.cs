using B_DB_Context;
using B_DB_Model;
using B_Utility.Common;
using B_Utility.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace B_Utility.BLL
{
    public class ApprovalBLL : IDisposable
    {
        private readonly DataBase_Context _db;

        public ApprovalBLL(DataBase_Context context)
        {
            _db = context;
        }

        //AddRequestApprovalSetup new request adding and automatically configure with stages and users
        
        public bool AddNewApprovalSetup(int requestId, int approvalUIId, bool? skip = false)
        {
                var approvalSetup = _db.ApprovalSetup.Where(x => x.ApprovalUIId == approvalUIId).ToList();
                
                // check only incase Re-design Request
                if(approvalUIId == (int)ApprovalUIIds.ClearanceForm)
                { 
                    var cancelledDemarcation = _db.TestApproval.Where(x=>x.RequestId == requestId && x.ApprovalUIId == (int)ApprovalUIIds.ClearanceForm).ToList();
                    if(cancelledDemarcation.Count > 0)
                    {
                       foreach(var item in cancelledDemarcation)
                        {
                            item.IsCancelled = true;
                            _db.SaveChanges();
                        }
                    }
                }

            if (approvalSetup.Count > 0)
            {
                foreach (var approvalUIIdItem in approvalSetup)
                {
                    var approvalUsers = _db.ApprovalUsers.Where(x => x.ApprovalSetupId == approvalUIIdItem.Id && x.IsActive).ToList();

                    if (approvalUsers.Count > 0)
                    {
                        foreach (var user in approvalUsers)
                        {
                            TestApproval testApproval = new TestApproval();
                            {
                                testApproval.RequestId = requestId;
                                testApproval.ApprovalUIId = approvalUIIdItem.ApprovalUIId;
                                testApproval.ApprovalSetupId = approvalUIIdItem.Id;
                                testApproval.StageNo = approvalUIIdItem.StageNo;
                                testApproval.NumberOfApprovalRequired = approvalUIIdItem.NumberOfApprovalRequired;
                                testApproval.UserId = user.UserId;
                                testApproval.UserDesignation = user.UserDesignation;
                                testApproval.ApprovalStatus = UHelper.ApprovalStatus(1);
                                testApproval.CreatedOn = DateTime.Now;
                                testApproval.IsActive = true;
                                testApproval.IsDeleted = false;

                                _db.TestApproval.Add(testApproval);
                                _db.SaveChanges();
                            }
                        }
                    }
                }
                if (skip == true)
                {
                    var currentStage = _db.TestApproval.Where(x => x.RequestId == requestId && x.ApprovalUIId == approvalUIId).Select(x => x.StageNo).FirstOrDefault();

                    var updateIsAssigned = _db.TestApproval.Where(x => x.RequestId == requestId && x.ApprovalUIId == approvalUIId && x.StageNo == currentStage).ToList();

                    if (updateIsAssigned.Count > 0)
                    {
                        foreach (var user in updateIsAssigned)
                        {
                            user.Is_Assigned = true;
                            user.AssignedDateTime = DateTime.Now;
                            user.ApprovalStatus = UHelper.ApprovalStatus(2);
                            _db.SaveChanges();
                        }

                        return true;
                    }
                }
                else
                {
                    var updateIsAssigned = _db.TestApproval.Where(x => x.RequestId == requestId && x.ApprovalUIId == approvalUIId && x.StageNo == 1).ToList();

                    if (updateIsAssigned.Count > 0)
                    {
                        foreach (var user in updateIsAssigned)
                        {
                            user.Is_Assigned = true;
                            user.AssignedDateTime = DateTime.Now;
                            user.ApprovalStatus = UHelper.ApprovalStatus(2);
                            _db.SaveChanges();
                        }

                        return true;
                    }
                }
            }

            return false;
        }

        //UpdateRequestApprovalSetup update request adding and automatically configure with stages and users
        public bool UpdateRequestApprovalSetup(int requestId, int approvalUIId)
        {

            var approvalUsers = _db.TestApproval.Where(x => x.ApprovalUIId == approvalUIId && x.RequestId == requestId).ToList();

            if (approvalUsers.Count > 0)
            {
                foreach (var user in approvalUsers)
                {
                    user.Is_Assigned = false;
                    user.ApprovalStatus = UHelper.ApprovalStatus(1);
                    _db.SaveChanges();

                }

                var updateIsAssigned = _db.TestApproval.Where(x => x.RequestId == requestId && x.ApprovalUIId == approvalUIId && x.StageNo == 1).ToList();

                if (updateIsAssigned.Count > 0)
                {
                    foreach (var user in updateIsAssigned)
                    {
                        user.Is_Assigned = true;
                        user.ApprovalStatus = UHelper.ApprovalStatus(2);
                        user.AssignedDateTime = DateTime.Now;
                        _db.SaveChanges();
                    }

                    return true;
                }
            }

            return false;
        }

        //UpdateApprovalStatus from approval loop users and automatically move next according to NumberOfApprovalRequired
        public bool UpdateApprovalStatus(RequestApprovalStatusUpdateDTO dto)
        {
            try
            {
                var updaterequeststatus = _db.TestApproval.Where(x => x.RequestId == dto.RequestId && x.ApprovalUIId == dto.ApprovalUIId && x.UserId == dto.UserId).OrderByDescending(x => x.Id).FirstOrDefault();

                if (updaterequeststatus != null)
                {
                    updaterequeststatus.ApprovalStatus = UHelper.ApprovalStatus(dto.IsApproved);
                    updaterequeststatus.ActionDateTime = DateTime.Now;
                    updaterequeststatus.LastActionComment = dto.Comment;
                    _db.SaveChanges();

                    var assignNextStage = _db.TestApproval.Where(x => x.RequestId == dto.RequestId && x.ApprovalUIId == dto.ApprovalUIId && x.StageNo == updaterequeststatus.StageNo).ToList();

                    if (assignNextStage.Count > 0)
                    {
                        int approvalCount = assignNextStage.Where(x => x.ApprovalStatus == "Approved").Count();

                        if (approvalCount >= updaterequeststatus.NumberOfApprovalRequired)
                        {
                            var updateAssignNextStage = _db.TestApproval.Where(x => x.RequestId == dto.RequestId && x.ApprovalUIId == dto.ApprovalUIId && x.StageNo == updaterequeststatus.StageNo + 1).ToList();

                            if (updateAssignNextStage.Count > 0)
                            {
                                foreach (var item in updateAssignNextStage)
                                {
                                    item.Is_Assigned = true;
                                    item.ApprovalStatus = UHelper.ApprovalStatus(2);
                                    item.AssignedDateTime = DateTime.Now;
                                    _db.SaveChanges();
                                }
                            }

                            else
                            {
                                if (dto.ApprovalUIId == (int)ApprovalUIIds.StockCreation)
                                {
                                    //bool updated = _commonBLL.UpdateStockCreation();
                                    var stock = _db.StockCreations.Where(x => !x.is_deleted
                                                                   && x.is_active == true
                                                                   && x.ID == dto.RequestId
                                                                   && x.Is_StockCreationRequested == true)
                                                                  .FirstOrDefault();

                                    if (stock != null)
                                    {
                                        stock.Is_StockCreationApproved = true;
                                        stock.Updated_at = DateTime.Now;
                                        stock.Status = "Approved";

                                        _db.SaveChanges();
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.PossessionAnnocement)
                                {
                                    //bool updated = _commonBLL.UpdateStockCreation();
                                    var stock = _db.StockCreations.Where(x => !x.is_deleted
                                                                   && x.is_active == true
                                                                   && x.ID == dto.RequestId
                                                                   && x.PossessionStatus == false
                                                                   && x.Is_StockCreationApproved == true
                                                                   && x.PossessionEffectDate != null)
                                                                  .FirstOrDefault();

                                    if (stock != null)
                                    {
                                        stock.PossessionStatus = true;
                                        stock.Is_PossessionApproved = true;
                                        stock.Updated_at = DateTime.Now;
                                        stock.UnderLitigation = true;

                                        _db.SaveChanges();
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.ClearanceForm)
                                {
                                    var stock = _db.StockCreations.Where(x => !x.is_deleted
                                                               && x.is_active == true
                                                               && x.ID == dto.RequestId
                                                               )
                                                              .FirstOrDefault();

                                    if (stock != null)
                                    {
                                        stock.Is_ClearnceApproved = true;
                                        stock.ClearanceOn = DateTime.Now;

                                        _db.SaveChanges();
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.MapApproval)
                                {
                                    var stock = _db.StockCreations.Where(x => !x.is_deleted
                                                               && x.is_active == true
                                                               && x.ID == dto.RequestId
                                                               )
                                                              .FirstOrDefault();

                                    if (stock != null)
                                    {
                                        stock.Is_MapApprovalApproved = true;
                                        stock.Updated_at = DateTime.Now;

                                        _db.SaveChanges();
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.DemarcationForm)
                                {
                                    var stock = _db.StockCreations.Where(x => !x.is_deleted
                                                               && x.is_active == true
                                                               && x.ID == dto.RequestId
                                                               )
                                                              .FirstOrDefault();

                                    if (stock != null)
                                    {
                                        stock.Is_DemarcationFormApproved = true;
                                        stock.Updated_at = DateTime.Now;

                                        _db.SaveChanges();
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.ConstructionSecurity)
                                {
                                    var stock = _db.StockCreations.Where(x => !x.is_deleted
                                                               && x.is_active == true
                                                               && x.ID == dto.RequestId
                                                               )
                                                              .FirstOrDefault();

                                    if (stock != null)
                                    {
                                        stock.Is_ConstructionSecurityApproved = true;
                                        stock.Updated_at = DateTime.Now;

                                        _db.SaveChanges();
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.ConstructionMonitoring)
                                {
                                    var stock = _db.StockCreations.Where(x => !x.is_deleted
                                                               && x.is_active == true
                                                               && x.ID == dto.RequestId
                                                               )
                                                              .FirstOrDefault();

                                    if (stock != null)
                                    {
                                        stock.Is_ConstructionMonitoringApproved = true;
                                        stock.Updated_at = DateTime.Now;

                                        _db.SaveChanges();
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.MemberRegistrationForm)
                                {
                                    var member = _db.MemberProfile.Where(x => !x.IsDeleted
                                                               && x.IsActive == true
                                                               && x.Id == dto.RequestId
                                                               )
                                                              .FirstOrDefault();

                                    if (member != null)
                                    {
                                        member.IsMemberProfileApproved = true;
                                        member.LastModified = DateTime.Now;

                                        _db.SaveChanges();
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.DealerRegistrationForm)
                                {
                                    var dealer = _db.Dealers.Where(x => !x.IsDeleted
                                                               && x.IsActive == true
                                                               && x.Id == dto.RequestId
                                                               )
                                                              .FirstOrDefault();

                                    if (dealer != null)
                                    {
                                        dealer.IsDealerProfileApproved = true;
                                        dealer.LastModified = DateTime.Now;

                                        _db.SaveChanges();
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.PreSale)
                                {
                                    var stockCreationId = _db.PreSale.Find(dto.RequestId).StockCreationId;
                                    var stock = _db.StockCreations.Where(x => !x.is_deleted
                                                               && x.is_active == true
                                                               && x.ID == stockCreationId
                                                               )
                                                              .FirstOrDefault();

                                    if (stock != null)
                                    {
                                        stock.IsPreSaleApproved = true;
                                        stock.Updated_at = DateTime.Now;

                                        _db.SaveChanges();
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.BookingForm)
                                {
                                    var stockCreationId = _db.Booking.Find(dto.RequestId).StockCreationId;
                                    var stock = _db.StockCreations.Where(x => !x.is_deleted
                                                               && x.is_active == true
                                                               && x.ID == stockCreationId
                                                               )
                                                              .FirstOrDefault();

                                    if (stock != null)
                                    {
                                        stock.IsBookingApproved = true;
                                        stock.Updated_at = DateTime.Now;

                                        _db.SaveChanges();
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.NDCRequestForMember)
                                {
                                    var nDCRequestForMember = _db.NDCRequestForMember.Where(x => !x.IsDeleted
                                                               && x.IsActive == true
                                                               && x.Id == dto.RequestId
                                                               )
                                                              .FirstOrDefault();

                                    if (nDCRequestForMember != null)
                                    {
                                        nDCRequestForMember.IsNDCRequestForMemberApproved = true;
                                        nDCRequestForMember.LastModified = DateTime.Now;

                                        _db.SaveChanges();
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.FileVerification)
                                {
                                    var fileVerificationRequest = _db.FileVerificationRequests.Where(x => !x.IsDeleted
                                                               && x.IsActive == true
                                                               && x.Id == dto.RequestId
                                                               )
                                                              .FirstOrDefault();

                                    if (fileVerificationRequest != null)
                                    {
                                        fileVerificationRequest.IsFileVerificationApproved = true;
                                        fileVerificationRequest.LastModified = DateTime.Now;

                                        _db.SaveChanges();
                                    }
                                }

                                if (dto.ApprovalUIId == (int)ApprovalUIIds.DemandNote)
                                {
                                    var demandNote = _db.DemandNote.Where(x => !x.IsDeleted
                                                               && x.IsActive == true
                                                               && x.Id == dto.RequestId
                                                               )
                                                              .FirstOrDefault();

                                    if (demandNote != null)
                                    {
                                        demandNote.IsDemandNoteApproved = true;
                                        demandNote.LastModified = DateTime.Now;

                                        _db.SaveChanges();
                                    }
                                }
                            }
                        }
                    }

                    ApprovalHistery approvalHistory = new ApprovalHistery();
                    {
                        approvalHistory.RequestId = dto.RequestId;
                        approvalHistory.ApprovalUIId = dto.ApprovalUIId;
                        approvalHistory.ActionTakenByName = "current login user name";
                        approvalHistory.ActionTakenUserRole = "current login user role";
                        approvalHistory.ActionDateTime = DateTime.Now;
                        approvalHistory.Action = UHelper.ApprovalStatus(dto.IsApproved);
                        approvalHistory.ActionComment = dto.Comment;

                        _db.ApprovalHistery.Add(approvalHistory);
                        _db.SaveChanges();
                    }

                }

                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }
        public void Dispose()
        {
              _db.Dispose();
        }

        public class RequestApprovalStatusUpdateDTO
        {
            [Required]
            public int RequestId { get; set; }
            [Required]
            public int ApprovalUIId { get; set; }
            [Required]
            public int UserId { get; set; }
            [Required]
            public int IsApproved { get; set; }
            public string? Comment { get; set; }
        }
    }
}
