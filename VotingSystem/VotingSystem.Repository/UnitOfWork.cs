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
            this.Answers = new AnswersRepository(contextFactory);
            this.Questions = new QuestionsRepository(contextFactory);
            this.Votes = new VotesRepository(contextFactory);
            this.State = new StatesRepository(contextFactory);
            this.Status = new StatusesRepository(contextFactory);
        }

        public ElectionRepository Elections { get; set; }
        public UserRepository Users { get; set; }
        public AnswersRepository Answers { get; set; }
        public QuestionsRepository Questions { get; set; }
        public VotesRepository Votes { get; set; }
        public StatusesRepository Status { get; set; }
        public StatesRepository State { get; set; }


        protected IDbContextFactory<DbContext> contextFactory { get; set; }
    }
}
