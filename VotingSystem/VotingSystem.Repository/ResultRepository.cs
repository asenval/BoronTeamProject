using VotingSystem.Model;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace VotingSystem.Repository
{
    public class ResultRepository : EntityRepository<User>
    {
        public ResultRepository(IDbContextFactory<DbContext> contextFactory)
            : base(contextFactory)
        {
        }
    }
}
