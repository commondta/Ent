//using B_DB_Context;
//using B_DB_Model;
//using HRMS_Web.Models.DTOs;
//using HRMS_Web.Services.BusinessServicesInterFace;
//using Microsoft.EntityFrameworkCore;

//namespace HRMS_Web.Services.BusinessServices
//{
//    public class BDealerProfile : IDealerProfile
//    {
//        private readonly DataBase_Context _context;

//        public BDealerProfile(DataBase_Context context)
//        {
//            _context = context;
//        }

//        public DealerProfile Get(int id)
//        {
//            return _context.DealerProfiles.Where(x => x.Id == id && x.IsDeleted != true)
//                                          .Include(x => x.EstateDetail.Where(x => !x.IsDeleted))
//                                          .Include(x => x.Properties.Where(x => !x.IsDeleted))
//                                          .Include(x => x.Renewal.Where(x => !x.IsDeleted))
//                                          .Include(x => x.Deals.Where(x => !x.IsDeleted))
//                                          .Include(x => x.Finanacials.Where(x => !x.IsDeleted))
//                                          .Include(x => x.DealerHistory.Where(x => !x.IsDeleted))
//                                          .Include(x => x.RelationshipHistory.Where(x => !x.IsDeleted))
//                                          .Include(x => x.Attachments.Where(x => !x.IsDeleted))
//                                          .FirstOrDefault();
//        }

//        public List<DealerProfile> GetAll()
//        {
//            return _context.DealerProfiles.Where(x => !x.IsDeleted)
//                                          .Include(x => x.EstateDetail.Where(x => !x.IsDeleted))
//                                          .Include(x => x.Properties.Where(x => !x.IsDeleted))
//                                          .Include(x => x.Renewal.Where(x => !x.IsDeleted))
//                                          .Include(x => x.Deals.Where(x => !x.IsDeleted))
//                                          .Include(x => x.Finanacials.Where(x => !x.IsDeleted))
//                                          .Include(x => x.DealerHistory.Where(x => !x.IsDeleted))
//                                          .Include(x => x.RelationshipHistory.Where(x => !x.IsDeleted))
//                                          .Include(x => x.Attachments.Where(x => !x.IsDeleted))
//                                          .ToList();
//        }

//        public int Create(DealerProfile dto)
//        {

//            dto.CreatedOn = DateTime.Now;
//            dto.IsActive = true;
//            dto.LastModified = DateTime.Now;
//            dto.IsDeleted = false;

//            _context.DealerProfiles.Add(dto);
//            _context.SaveChanges();

//            return dto.Id;
//        }

//        public DealerProfile Update(DealerProfile dto)
//        {
//            DealerProfile model = _context.DealerProfiles.Find(dto.Id);
//            {
//                model.DocumentNo = dto.DocumentNo;
//                model.DocumentDate = dto.DocumentDate;
//                model.RegistrationNo = dto.RegistrationNo;
//                model.DateOfRegistration = dto.DateOfRegistration;
//                model.DealerCode = dto.DealerCode;
//                model.DealerStatus = dto.DealerStatus;
//                model.CNIC = dto.CNIC;
//                model.ResidentialAddress = dto.ResidentialAddress;
//                model.EstateName = dto.EstateName;
//                model.RenewalDate = dto.RenewalDate;
//                model.Nationality = dto.Nationality;
//                model.Country = dto.Country;
//                model.City = dto.City;
//                model.OutstandingBalance = dto.OutstandingBalance;
//                model.OutstandingAdvance = dto.OutstandingAdvance;
//                model.IsActive = dto.IsActive;
//                model.LastModified = DateTime.Now;
//                model.IsDeleted = false;

//                _context.SaveChanges();
//            }

//            if (dto.EstateDetail?.Count > 0)
//            {
//                var estateDetailList = _context.EstateDetails.Where(x => x.DealerProfileId == model.Id).ToList();
//                if (estateDetailList?.Count > 0)
//                    _context.EstateDetails.RemoveRange(estateDetailList);

//                _context.EstateDetails.AddRange(dto.EstateDetail);
//                _context.SaveChanges();
//            }

//            if (dto.Properties?.Count > 0)
//            {
//                var propertiesList = _context.Properties.Where(x => x.DealerProfileId == model.Id).ToList();
//                if (propertiesList?.Count > 0)
//                    _context.Properties.RemoveRange(propertiesList);

//                _context.Properties.AddRange(dto.Properties);
//                _context.SaveChanges();
//            }

//            if (dto.Renewal?.Count > 0)
//            {
//                var renewalList = _context.Renewals.Where(x => x.DealerProfileId == model.Id).ToList();
//                if (renewalList?.Count > 0)
//                    _context.Renewals.RemoveRange(renewalList);

//                _context.Renewals.AddRange(dto.Renewal);
//                _context.SaveChanges();
//            }

//            if (dto.Deals?.Count > 0)
//            {
//                var dealsList = _context.Deals.Where(x => x.DealerProfileId == model.Id).ToList();
//                if (dealsList?.Count > 0)
//                    _context.Deals.RemoveRange(dealsList);

//                _context.Deals.AddRange(dto.Deals);
//                _context.SaveChanges();
//            }

//            if (dto.Finanacials?.Count > 0)
//            {
//                var finanacialsList = _context.Finanacials.Where(x => x.DealerProfileId == model.Id).ToList();
//                if (finanacialsList?.Count > 0)
//                    _context.Finanacials.RemoveRange(finanacialsList);

//                _context.Finanacials.AddRange(dto.Finanacials);
//                _context.SaveChanges();
//            }

//            if (dto.DealerHistory?.Count > 0)
//            {
//                var dealerHistoriesList = _context.DealerHistory.Where(x => x.DealerProfileId == model.Id).ToList();
//                if (dealerHistoriesList?.Count > 0)
//                    _context.DealerHistory.RemoveRange(dealerHistoriesList);

//                _context.DealerHistory.AddRange(dto.DealerHistory);
//                _context.SaveChanges();
//            }

//            if (dto.RelationshipHistory?.Count > 0)
//            {
//                var relationshipHistoriesList = _context.RelationshipHistory.Where(x => x.DealerProfileId == model.Id).ToList();
//                if (relationshipHistoriesList?.Count > 0)
//                    _context.RelationshipHistory.RemoveRange(relationshipHistoriesList);

//                _context.RelationshipHistory.AddRange(dto.RelationshipHistory);
//                _context.SaveChanges();
//            }

//            if (dto.Attachments?.Count > 0)
//            {
//                var attachmentsList = _context.Attachments.Where(x => x.DealerProfileId == model.Id).ToList();
//                if (attachmentsList?.Count > 0)
//                    _context.Attachments.RemoveRange(attachmentsList);

//                _context.Attachments.AddRange(dto.Attachments);
//                _context.SaveChanges();
//            }

//            return dto;
//        }

//        public int Delete(int id)
//        {

//            DealerProfile model = _context.DealerProfiles.Find(id);

//            var estateDetailList = _context.EstateDetails.Where(x => x.DealerProfileId == model.Id).ToList();

//            if (estateDetailList?.Count > 0)
//                foreach (var item in estateDetailList)
//                {
//                    item.IsDeleted = true;
//                    _context.SaveChanges();
//                }

//            var propertiesList = _context.Properties.Where(x => x.DealerProfileId == model.Id).ToList();

//            if (propertiesList?.Count > 0)
//                foreach (var item in propertiesList)
//                {
//                    item.IsDeleted = true;
//                    _context.SaveChanges();
//                }

//            var renewalList = _context.Renewals.Where(x => x.DealerProfileId == model.Id).ToList();

//            if (renewalList?.Count > 0)
//                foreach (var item in renewalList)
//                {
//                    item.IsDeleted = true;
//                    _context.SaveChanges();
//                }

//            var dealsList = _context.Deals.Where(x => x.DealerProfileId == model.Id).ToList();
//            if (dealsList?.Count > 0)
//                foreach (var item in dealsList)
//                {
//                    item.IsDeleted = true;
//                    _context.SaveChanges();
//                }

//            var finanacialsList = _context.Finanacials.Where(x => x.DealerProfileId == model.Id).ToList();
//            if (finanacialsList?.Count > 0)
//                foreach (var item in finanacialsList)
//                {
//                    item.IsDeleted = true;
//                    _context.SaveChanges();
//                }

//            var dealerHistoriesList = _context.DealerHistory.Where(x => x.DealerProfileId == model.Id).ToList();
//            if (dealerHistoriesList?.Count > 0)
//                foreach (var item in dealerHistoriesList)
//                {
//                    item.IsDeleted = true;
//                    _context.SaveChanges();
//                }

//            var relationshipHistoriesList = _context.RelationshipHistory.Where(x => x.DealerProfileId == model.Id).ToList();
//            if (dealerHistoriesList?.Count > 0)
//                foreach (var item in dealerHistoriesList)
//                {
//                    item.IsDeleted = true;
//                    _context.SaveChanges();
//                }

//            var attachmentsList = _context.Attachments.Where(x => x.DealerProfileId == model.Id).ToList();
//            if (attachmentsList?.Count > 0)
//                foreach (var item in attachmentsList)
//                {
//                    item.IsDeleted = true;
//                    _context.SaveChanges();
//                }


//                    model.IsDeleted = true;
//                    model.LastModified = DateTime.Now;

//            return _context.SaveChanges();

//        }
//    }
//}
