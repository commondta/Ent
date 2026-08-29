using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_DB_Model;

namespace B_DB_Context.Repository.IRepository
{
    public class RealEstateRepository : Repository<Real_Estate>, IRealEstateRepository
    {

        private DataBase_Context _db;
        public RealEstateRepository(DataBase_Context db) : base(db)
        {

            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Real_Estate obj)
        {
            _db.Real_Estates.Update(obj);
        }
    }

}
