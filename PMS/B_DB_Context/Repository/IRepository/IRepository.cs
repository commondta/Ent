using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Context.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {

        //T-will be User   
        T GetFirstOrDeafult(Expression<Func<T, bool>> filter);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll_WithFilters(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);

    }
}
