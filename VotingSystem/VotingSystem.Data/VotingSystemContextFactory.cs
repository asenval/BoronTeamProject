using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace VotingSystem.Data
{
    public class VotingSystemContextFactory : IDbContextFactory<DbContext>
    {
        public DbContext Create()
        {
            return new VotingSystemContext();
        }
    }
}
