using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using B_DB_Context.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace B_DB_Context.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly DataBase_Context _db;


        // we don't know which method called this class so we have to specifited the table name at there so 
        internal DbSet<T> dbSet;
        public Repository(DataBase_Context db)
        {

            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {

            try
            {
                dbSet.Add(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<T> GetAll()
        {
            try
            {
                IQueryable<T> query = dbSet;


                return query.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IEnumerable<T> GetAll_WithFilters(Expression<Func<T, bool>> filter)
        {
            try
            {

                IQueryable<T> query = dbSet.Where(filter);
                query.ToList();
                return query;
            }
            catch (Exception)
            {
                throw;

            }
        }

        public T GetFirstOrDeafult(Expression<Func<T, bool>> filter)
        {
            try
            {
                IQueryable<T> query = dbSet;

                query = query.Where(filter);
                return query.FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Remove(T entity)
        {
            try
            {

                dbSet.Remove(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
