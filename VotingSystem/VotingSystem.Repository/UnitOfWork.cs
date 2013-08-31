using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace VotingSystem.Repository
{
    public class UnitOfWork
    {
        public UnitOfWork(IDbContextFactory<DbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
            this.Elections = new ElectionRepository(contextFactory);
            this.Users = new UserRepository(contextFactory);
            this.Results = new ResultRepository(contextFactory);
        }

        public ElectionRepository Elections { get; set; }
        public UserRepository Users { get; set; }
        public ResultRepository Results { get; set; }


        protected IDbContextFactory<DbContext> contextFactory { get; set; }
    }
}
