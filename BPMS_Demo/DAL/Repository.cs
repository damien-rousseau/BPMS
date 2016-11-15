using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DAL.Base;
using Domain.Entities;

namespace DAL
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        private readonly WfCustomDatabaseContext _context;

        public Repository(IRepositoryBase repositoryBase)
        {
            _context = repositoryBase.Context;
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsEnumerable();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> query)
        {
            return _context.Set<T>().FirstOrDefault(query);
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> query)
        {
            return _context.Set<T>().Where(query);
        }

        public void SaveOrUpdate(T entity)
        {
            var attachedEntity = _context.Set<T>().Find(entity.Id);

            if (attachedEntity != null)
            {
                var attachedEntry = _context.Entry(attachedEntity);
                attachedEntry.CurrentValues.SetValues(entity);
            }
            else
            {
                _context.Set<T>().Add(entity);
            }

            _context.SaveChanges();
        }
    }
}