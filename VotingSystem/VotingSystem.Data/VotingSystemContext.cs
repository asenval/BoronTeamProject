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
        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(x => x.Username).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(x => x.DisplayName).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(x => x.AuthCode).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(x => x.SessionKey).IsOptional().HasMaxLength(50);
            //modelBuilder.Entity<User>().HasMany(x => x.Elections).WithMany(e => e.Users);

            modelBuilder.Entity<Question>().Property(x => x.Content).HasMaxLength(200);
            modelBuilder.Entity<Question>().Property(x => x.QuestionType).HasMaxLength(20);
            modelBuilder.Entity<Answer>().Property(x => x.Content).HasMaxLength(100);
            modelBuilder.Entity<State>().Property(x => x.Name).HasMaxLength(20);
            modelBuilder.Entity<Election>().Property(x => x.Title).HasMaxLength(50);
            modelBuilder.Entity<Tag>().Property(x => x.Title).HasMaxLength(20);
            
            base.OnModelCreating(modelBuilder);
        }

    }
}
