using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using VotingSystem.Data;

namespace VotingSystem.Repository
{
    public class EntityRepository<T> : IRepository<T> where T : class
    {
        protected IDbContextFactory<DbContext> contextFactory;

        //protected DbContext Context { get; set; }

        //protected IDbSet<T> DbSet { get; set; }

        public EntityRepository(IDbContextFactory<DbContext> contextFactory)
        {
            if (contextFactory == null)
            {
                throw new ArgumentException("An instance of DbContext is required to use this repository.", "context");
            }

            this.contextFactory = contextFactory;
        }

        public virtual IQueryable<T> Find(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            var context = contextFactory.Create();
            var result = context.Set<T>().Where(predicate);
            return result;
        }

        public virtual IQueryable<T> All()
        {
            var context = contextFactory.Create();
            return context.Set<T>();
        }

        public virtual T Get(int id)
        {
            var context = contextFactory.Create();
            var result = context.Set<T>().Find(id);
            //context.Entry(result).State = System.Data.EntityState.Detached;
            return result;
        }

        public virtual T Add(T item)
        {
            var context = contextFactory.Create();
            context.Set<T>().Add(item);
            context.SaveChanges();
            return item;
        }

        public virtual void Delete(T entity)
        {
            var context = contextFactory.Create();
            DbEntityEntry entry = context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                context.Set<T>().Attach(entity);
            }

            context.Set<T>().Remove(entity);
            context.SaveChanges();
        }

        public virtual void Delete(int id)
        {
            var entity = this.Get(id);

            if (entity != null)
            {
                this.Delete(entity);
            }
        }

        public virtual T Update(int id, T item)
        {
            var context = contextFactory.Create();
            DbEntityEntry entry = context.Entry(item);
            if (entry.State == EntityState.Detached)
            {
                context.Set<T>().Attach(item);
            }

            entry.State = EntityState.Modified;

            context.SaveChanges();

            return item;
        }

        /*public virtual void SaveChanges()
        {
            Context.SaveChanges();
        }

        public virtual void Dispose()
        {
            if (Context != null)
                Context.Dispose();
        }*/
    }
}
