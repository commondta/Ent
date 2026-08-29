using B_DB_Model;
using HRMS_Web.Models.DTOs;

namespace HRMS_Web.Services.BusinessServicesInterFace
{
    public interface IDealer
    {
        Dealer Get(int id);
        List<Dealer> GetAll();
        int Create(Dealer dto);
        Dealer Update(Dealer dto);
        int Delete(int id);
    }
}
