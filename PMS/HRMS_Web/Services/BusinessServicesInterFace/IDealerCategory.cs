using B_DB_Model;
using HRMS_Web.Models.DTOs;

namespace HRMS_Web.Services.BusinessServicesInterFace
{
    public interface IDealerCategory
    {
        DealerCategory Get(int id);
        List<DealerCategory> GetAll();
        int Create(DealerCategory dto);
        DealerCategory Update(DealerCategory dto);
        int Delete(int id);
    }
}
