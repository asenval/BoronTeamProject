using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace VotingSystem.Data
{
    public class VotingSystemContextFactory : IDbContextFactory<DbContext>
    {
        private VotingSystemContext context;

        protected DbContext Context
        {
            get
            {
                if (this.context == null)
                {
                    this.context = new VotingSystemContext();
                }
                return this.context;
            }
        }

        public DbContext Create()
        {
            return this.Context;
            //return new VotingSystemContext();
        }
    }
}
