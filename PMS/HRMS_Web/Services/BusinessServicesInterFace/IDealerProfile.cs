using B_DB_Model;
using HRMS_Web.Models.DTOs;

namespace HRMS_Web.Services.BusinessServicesInterFace
{
    public interface IDealerProfile
    {
        DealerProfile Get(int id);
        List<DealerProfile> GetAll();
        int Create(DealerProfile dto);
        DealerProfile Update(DealerProfile dto);
        int Delete(int id);
    }
}
