using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingSystem.Model;

namespace VotingSystem.Repository
{
    public class AnswersRepository : EntityRepository<Answer>
    {
        public AnswersRepository(IDbContextFactory<DbContext> contextFactory)
            : base(contextFactory)
        {
        }
    }
}
