using VotingSystem.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingSystem.Data
{
    public class VotingSystemContext:DbContext
    {
        public VotingSystemContext()
            : base("VotingSystemDB")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Election> Elections { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Result> Results { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(x => x.Username).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(x => x.DisplayName).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(x => x.AuthCode).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(x => x.SessionKey).IsOptional().HasMaxLength(50);

            //Todo add properties for other models
            
            base.OnModelCreating(modelBuilder);
        }

    }
}
