using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_DB_Model;

namespace B_DB_Context.Repository.IRepository
{
    public interface IRealEstateRepository : IRepository<Real_Estate>
    {

        void Update(Real_Estate obj);
        void Save();


    }
}
