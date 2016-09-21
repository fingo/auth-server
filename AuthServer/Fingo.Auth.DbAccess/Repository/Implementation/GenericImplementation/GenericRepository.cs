using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fingo.Auth.DbAccess.Context.Interfaces;
using Fingo.Auth.DbAccess.Models.Base;
using Fingo.Auth.DbAccess.Repository.Interfaces.GenericInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Fingo.Auth.DbAccess.Repository.Implementation.GenericImplementation
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly DbSet<T> Dbset;
        protected IAuthServerContext Entities;

        protected GenericRepository(IAuthServerContext context)
        {
            Entities = context;
            Dbset = context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return Dbset.AsEnumerable();
        }

        public virtual T GetById(int id)
        {
            var project = Dbset.FirstOrDefault(p => p.Id == id);
            return project;
        }

        public virtual IEnumerable<T> FindBy(Expression<Func<T , bool>> predicate)
        {
            var query = Dbset.Where(predicate).AsEnumerable();
            return query;
        }

        public virtual void Add(T entity)
        {
            Dbset.Add(entity);
            Entities.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            Dbset.Remove(entity);
            Entities.SaveChanges();
        }

        public virtual void Edit(T entity)
        {
            Entities.Entry(entity).State = EntityState.Modified;
            Entities.SaveChanges();
        }
    }
}