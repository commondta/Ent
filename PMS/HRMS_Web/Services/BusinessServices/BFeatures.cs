using B_DB_Context;
using B_DB_Model;
using HRMS_Web.Models.DTOs;
using HRMS_Web.Services.BusinessServicesInterFace;

namespace HRMS_Web.Services.BusinessServices
{
    public class BFeatures : IFeatures
    {
        private readonly DataBase_Context _context;

        public BFeatures(DataBase_Context context)
        {
            _context = context;
        }

        public Feature Get(int id)
        {
            return _context.Features.Find(id);
        }

        public List<Feature> GetAll()
        {
            return _context.Features.Where(x => !x.is_deleted).ToList();
        }

        public int Create(FeatureDTO dto)
        {
            Feature model = new Feature
            {
                Code = dto.Code,
                Description = dto.Description,
                is_active = dto.is_active,
                Created_By = 1,
                Created_at = DateTime.Now,
                is_deleted = false,
            };

            _context.Features.Add(model);
            _context.SaveChanges();

            return model.ID;
        }

        public Feature Update(FeatureDTO dto)
        {
            Feature model = _context.Features.Find(dto.ID);

            model.Code = dto.Code;
            model.Description = dto.Description;
            model.is_active = dto.is_active;
            model.Updated_By = 1;
            model.Updated_at = DateTime.Now;

            _context.SaveChanges();

            return model;
        }

        public int Delete(int id)
        {
            Feature model = _context.Features.Find(id);

            model.is_deleted = true;
            model.Updated_at = DateTime.Now;
            model.Updated_By = 1;

            return _context.SaveChanges();
        }
    }
}
