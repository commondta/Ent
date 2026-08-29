using B_DB_Context;
using B_DB_Model;
using HRMS_Web.Models.DTOs;
using HRMS_Web.Services.BusinessServicesInterFace;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Web.Services.BusinessServices
{
    public class BDealerCategory : IDealerCategory
    {
        private readonly DataBase_Context _context;

        public BDealerCategory(DataBase_Context context)
        {
            _context = context;
        }

        public int Create(DealerCategory dto)
        {
            dto.IsActive = true;
            dto.CreatedBy = 1;
            dto.CreatedOn = DateTime.Now;
            dto.IsDeleted = false;
           
            _context.DealerCategories.Add(dto); 
            _context.SaveChanges();

            return dto.Id;
         }

        public DealerCategory Get(int id)
        {
           return _context.DealerCategories.Find(id);
        }

        public List<DealerCategory> GetAll()
        {
            return _context.DealerCategories.ToList();
        }

        public DealerCategory Update(DealerCategory dto)
        {
            DealerCategory dealerCategory = _context.DealerCategories.Find(dto.Id);

            if (dealerCategory != null)
            {
                dealerCategory.CategoryCode = dto.CategoryCode;
                dealerCategory.Name = dto.Name;
                dealerCategory.Description = dto.Description;
                dealerCategory.LastModified = dto.LastModified;
                dealerCategory.ModifiedBy = 1;
                dealerCategory.IsActive = true;

                _context.SaveChanges();
            }

            return dealerCategory;
        }

        public int Delete(int id)
        {
            DealerCategory dealerCategory = _context.DealerCategories.Find(id);
            
            if(dealerCategory != null)
            {
                dealerCategory.IsDeleted = true;
                dealerCategory.IsActive = false;
            }

            return _context.SaveChanges();  
        }

    }
}
