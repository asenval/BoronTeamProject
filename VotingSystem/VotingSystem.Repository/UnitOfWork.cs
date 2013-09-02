using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using VotingSystem.Data;

namespace VotingSystem.Repository
{
    public class UnitOfWork
    {
        public UnitOfWork(IDbContextFactory<DbContext> contextFactory)
        {
            this.contextFactory = new VotingSystemContextFactory();
            this.Elections = new ElectionRepository(this.contextFactory);
            this.Users = new UserRepository(this.contextFactory);
            this.Answers = new AnswersRepository(this.contextFactory);
            this.Questions = new QuestionsRepository(this.contextFactory);
            this.Votes = new VotesRepository(this.contextFactory);
            this.State = new StatesRepository(this.contextFactory);
            this.Status = new StatusesRepository(this.contextFactory);
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
