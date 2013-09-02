using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace VotingSystem.Repository
{
    public interface IRepository<T>
    {
        IQueryable<T> All();
        T Get(int id);
        T Add(T entity);
        T Update(int id, T entity);
        void Delete(int id);
        
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
    }
}
