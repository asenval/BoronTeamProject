using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using VotingSystem.Data;

namespace VotingSystem.Repositories
{
    public class EntityRepository<T> : IRepository<T> where T : class
    {
        protected DbContext Context { get; set; }

        protected IDbSet<T> DbSet { get; set; }
        
        public EntityRepository(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentException("An instance of DbContext is required to use this repository.", "context");
            }

            this.Context = context;
            this.DbSet = this.Context.Set<T>();
        }

        public virtual IQueryable<T> Find(System.Linq.Expressions.Expression<Func<T, int, bool>> predicate)
        {
            return this.DbSet.Where(predicate);
        }

        public virtual IQueryable<T> All()
        {
            return this.DbSet;
        }

        public virtual T Get(int id)
        {
            //var db = new BlogSystemContext();
            //return db.Set<T>().Find(id);
            return this.DbSet.Find(id);
        }

        public virtual T Add(T item)
        {
            this.DbSet.Add(item);
            this.Context.SaveChanges();
            return item;
        }

        public virtual void Delete(T entity)
        {
            DbEntityEntry entry = this.Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            this.DbSet.Remove(entity);
            this.Context.SaveChanges();
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
            DbEntityEntry entry = this.Context.Entry(item);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(item);
            }

            entry.State = EntityState.Modified;

            this.Context.SaveChanges();

            return item;
        }


        public virtual void SaveChanges()
        {
            Context.SaveChanges();
        }

        public virtual void Dispose()
        {
            if (Context != null)
                Context.Dispose();
        }
    }
}
