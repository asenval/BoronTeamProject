using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using VotingSystem.Model;


namespace VotingSystem.Repository
{
    public class ElectionRepository: EntityRepository<Election>
    {
        public ElectionRepository(IDbContextFactory<DbContext> contextFactory)
            : base(contextFactory)
        {
        }
    }
}
