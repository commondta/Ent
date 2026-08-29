using B_DB_Context;
using B_DB_Model;
using HRMS_Web.Services.BusinessServicesInterFace;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace HRMS_Web.Services.BusinessServices
{
    public class BDealer : IDealer
    {
        private readonly DataBase_Context _context;

        public BDealer(DataBase_Context context)
        {
            _context = context;
        }

        public int Create(Dealer dto)
        {
            // for dealer

            dto.CreatedBy = 1;
            dto.CreatedOn = DateTime.Now;
            dto.IsDeleted = false;
            dto.IsActive = false;

            // for Dealer Deatail

            if (dto.DealerEstateDeatail?.Count > 0)
            {
                foreach (var dealer in dto.DealerEstateDeatail)
                {
                    dealer.CreatedBy = 1;
                    dealer.CreatedOn = DateTime.Now;
                    dealer.IsDeleted = false;
                    dealer.IsActive = false;
                }
            }

            // for Dealer Attachments

            if (dto.DealerAttachments?.Count > 0)
            {
                foreach (var attachment in dto.DealerAttachments)
                {
                    attachment.CreatedBy = 1;
                    attachment.CreatedOn = DateTime.Now;
                    attachment.IsDeleted = false;
                    attachment.IsActive = false;
                }
            }

            _context.Dealers.Add(dto);
            _context.SaveChanges();

            return dto.Id;
        }

        public int Delete(int id)
        {
            Dealer model = _context.Dealers.Find(id);

            var estateDetailList = _context.dealerEstateDeatails.Where(x => x.DealerId == model.Id).ToList();

            if (estateDetailList?.Count > 0)
            { 
                foreach (var item in estateDetailList)
                {
                    item.ModifiedBy = 1;
                    item.LastModified = DateTime.Now;
                    item.IsActive = false;
                    item.IsDeleted = true;

                    _context.SaveChanges();
                }
            }

            var dealerAttachmentsList = _context.DealerAttachments.Where(x => x.DealerId == model.Id).ToList();

            if (dealerAttachmentsList?.Count > 0)
            {
                foreach (var item in dealerAttachmentsList)
                {
                    item.ModifiedBy = 1;
                    item.LastModified = DateTime.Now;
                    item.IsActive = false;
                    item.IsDeleted = true;

                    _context.SaveChanges();
                }
            }
            
            model.IsActive = false;
            model.LastModified = DateTime.Now;
            model.IsDeleted = true;

           return  _context.SaveChanges();

        }

        public Dealer Get(int id)
        {
            return _context.Dealers.Where(x => x.Id == id && x.IsDeleted != true)
                                   .Include(x => x.DealerEstateDeatail.Where(x => !x.IsDeleted))
                                   .Include(x => x.DealerAttachments.Where(x => !x.IsDeleted))
                                   .Include(x => x.DealerCategory)
                                   .FirstOrDefault();
        }

        public List<Dealer> GetAll()
        {
            return _context.Dealers.Include(x => x.DealerEstateDeatail.Where(x => !x.IsDeleted))
                                   .Include(x => x.DealerAttachments.Where(x => !x.IsDeleted))
                                   .Include(x=> x.DealerCategory)
                                   .ToList();
        }

        public Dealer Update(Dealer dto)
        {
            Dealer model = _context.Dealers.Find(dto.Id);
            {
                model.PictureBase64 = dto.PictureBase64;
                model.PrincipalOwner = dto.PrincipalOwner;
                model.RegistrationFee = dto.RegistrationFee;
                model.EstateAddress = dto.EstateAddress;
                model.CNIC = dto.CNIC;
                model.ResidentialAddress = dto.ResidentialAddress;
                model.EstateName = dto.EstateName;
                model.RenewalDate = dto.RenewalDate;
                model.Nationality = dto.Nationality;
                model.Country = dto.Country;
                model.City = dto.City;
                model.IsActive = dto.IsActive;
                model.LastModified = DateTime.Now;
                model.IsDeleted = false;

                _context.SaveChanges();
            }

            if (dto.DealerEstateDeatail?.Count > 0)
            {
                var estateDetailList = _context.dealerEstateDeatails.Where(x => x.DealerId == model.Id).ToList();
                if (estateDetailList?.Count > 0)
                    _context.dealerEstateDeatails.RemoveRange(estateDetailList);
                    _context.SaveChanges();

                foreach (var dealer in dto.DealerEstateDeatail)
                {
                    dealer.CreatedBy = 1;
                    dealer.CreatedOn = DateTime.Now;
                    dealer.IsDeleted = false;
                    dealer.IsActive = false;
                }

                _context.dealerEstateDeatails.AddRange(dto.DealerEstateDeatail);
                
            }

            if (dto.DealerAttachments?.Count > 0)
            {
                var attachmentsList = _context.DealerAttachments.Where(x => x.DealerId == model.Id).ToList();
                if (attachmentsList?.Count > 0)
                    _context.DealerAttachments.RemoveRange(attachmentsList);
                    _context.SaveChanges();

                foreach (var attachment in dto.DealerAttachments)
                {
                    attachment.CreatedBy = 1;
                    attachment.CreatedOn = DateTime.Now;
                    attachment.IsDeleted = false;
                    attachment.IsActive = false;
                }

                _context.DealerAttachments.AddRange(dto.DealerAttachments);
                _context.SaveChanges();
            }

            return dto;
        }
    }
}
