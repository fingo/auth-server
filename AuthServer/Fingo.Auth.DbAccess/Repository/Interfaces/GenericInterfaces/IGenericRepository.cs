using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Fingo.Auth.DbAccess.Repository.Interfaces.GenericInterfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> FindBy(Expression<Func<T , bool>> predicate);
        T GetById(int id);
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
    }
}