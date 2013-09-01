namespace VotingSystem.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using VotingSystem.Model;

    internal sealed class Configuration : DbMigrationsConfiguration<VotingSystem.Data.VotingSystemContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(VotingSystem.Data.VotingSystemContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Users.AddOrUpdate(x => x.Username, new User
            {
                DisplayName = "UserDisplayname1",
                AuthCode = "1234512345123451234512345123451234512345",
                Username = "User1"
            });

            context.Users.AddOrUpdate(x => x.Username, new User
            {
                DisplayName = "UserDisplayname2",
                AuthCode = "1234512345123451234512345123451234512345",
                Username = "User2"
            });

            context.Statuses.AddOrUpdate(x => x.Title, new Status
            {
                Title = "Open",
            });

            context.Statuses.AddOrUpdate(x => x.Title, new Status
            {
                Title = "Closed",
            });

            context.States.AddOrUpdate(x => x.Name, new State
            {
                Name = "Public",
            });

            context.States.AddOrUpdate(x => x.Name, new State
            {
                Name = "Private",
            });

            context.States.AddOrUpdate(x => x.Name, new State
            {
                Name = "Unlisted",
            });

            context.SaveChanges();

            context.Elections.AddOrUpdate(x => x.Title, new Election()
            {
                Title = "Election1",
                Status = context.Statuses.FirstOrDefault(x => x.Title == "Open"),
                StartDate = new DateTime(2013, 8, 1),
                EndDate = new DateTime(2013, 9, 1),
                State = context.States.FirstOrDefault(x => x.Name == "Public"),
                Questions = new List<Question>()
                {
                    new Question 
                    {
                        Content = "What is life?",
                        VoteType =  "Boolean",
                        Answers = new List<Answer>()
                        {
                            new Answer 
                            {
                                Content = "Nobody knows",
                            },
                            new Answer
                            {
                                Content = "It is 42."
                            },
                            new Answer 
                            {
                                Content = "Mama knows best."
                            }
                        }
                    },
                    new Question 
                    {
                        Content = "Why do I have to go to work?",
                        VoteType =  "Boolean",
                        Answers = new List<Answer>()
                        {
                            new Answer 
                            {
                                Content = "Za da ti e gadno.",
                            },
                            new Answer
                            {
                                Content = "Life sucks, get used to it!"
                            },
                            new Answer 
                            {
                                Content = "Mama knows best again."
                            }
                        }
                    }
                }
            });
        }
    }
}
