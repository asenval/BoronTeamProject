using VotingSystem.Model;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace VotingSystem.Repository
{
    public class StatusesRepository : EntityRepository<Status>
    {
        public StatusesRepository(IDbContextFactory<DbContext> contextFactory)
            : base(contextFactory)
        {
        }
    }
}