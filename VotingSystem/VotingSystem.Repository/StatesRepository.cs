using VotingSystem.Model;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace VotingSystem.Repository
{
    public class StatesRepository : EntityRepository<State>
    {
        public StatesRepository(IDbContextFactory<DbContext> contextFactory)
            : base(contextFactory)
        {
        }
    }
}
