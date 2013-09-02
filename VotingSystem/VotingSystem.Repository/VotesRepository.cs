using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using VotingSystem.Model;

namespace VotingSystem.Repository
{
    public class VotesRepository : EntityRepository<Vote>
    {
        public VotesRepository(IDbContextFactory<DbContext> contextFactory)
            : base(contextFactory)
        {
        }
    }
}
