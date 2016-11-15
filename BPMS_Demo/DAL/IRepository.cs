using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Domain.Entities;

namespace DAL
{
    public interface IRepository<T> where T : EntityBase
    {
        IEnumerable<T> GetAll();

        T FirstOrDefault(Expression<Func<T, bool>> query);

        IEnumerable<T> Where(Expression<Func<T, bool>> query);

        void SaveOrUpdate(T entity);
    }
}
