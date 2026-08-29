using B_DB_Model;
using HRMS_Web.Models.DTOs;

namespace HRMS_Web.Services.BusinessServicesInterFace
{
    public interface IFeatures
    {
        Feature Get(int id);
        List<Feature> GetAll();
        int Create(FeatureDTO dto);
        Feature Update(FeatureDTO dto);
        int Delete(int id);
    }
}
