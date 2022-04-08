using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Mm.DataAccessLayer
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext context;
        protected DbSet<T> dbSet;

        public Repository(DbContext datacontext)
        {
            this.context = datacontext;
            dbSet = datacontext.Set<T>();

        }

        public void Insert(T entity)
        {
            context.Entry(entity).State = System.Data.EntityState.Added;
            context.SaveChanges();
        }

        public void Delete(T entity)
        {
            context.Entry(entity).State = System.Data.EntityState.Deleted;
            context.SaveChanges();
        }

        public void Update(T entity)
        {

            context.Entry(entity).State = System.Data.EntityState.Modified;
            context.SaveChanges();

        }

        public T GetById(int id)
        {
            return context.Set<T>().Find(id);

        }

        public IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return context.Set<T>().ToList();
        }

        public T GetSingle(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties)
        //This method will find the related records by passing two argument
        //First argument: lambda expression to search a record such as d => d.StandardName.Equals(standardName) to search am record by standard name
        //Second argument: navigation property that leads to the related records such as d => d.Students
        //The method returns the related records that met the condition in the first argument.
        //An example of the method GetStandardByName(string standardName)
        //public Standard GetStandardByName(string standardName)
        //{
        //return _standardRepository.GetSingle(d => d.StandardName.Equals(standardName), d => d.Students);
        //}
        {
            T item = null;
            IQueryable<T> dbQuery = null;
            foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                dbQuery = dbQuery.Include<T, object>(navigationProperty);

            item = dbQuery
                .AsNoTracking()
                .FirstOrDefault(where);
            return item;

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}