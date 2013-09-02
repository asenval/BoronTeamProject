namespace VotingSystem.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using VotingSystem.Model;

    public sealed class Configuration : DropCreateDatabaseIfModelChanges<VotingSystem.Data.VotingSystemContext> //CreateDatabaseIfNotExists
    {
        public Configuration()
        {
            //AutomaticMigrationsEnabled = true;
            //AutomaticMigrationDataLossAllowed = true;
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
                DisplayName = "userTest",
                AuthCode = "c8bbfb35608f4be64645e29c1b2ce3b17f9d8e38",
                Username = "userTest"
            });

            context.Users.AddOrUpdate(x => x.Username, new User
            {
                DisplayName = "Anonymous",
                AuthCode = "9876598765987659876598765987659876598765",
                Username = "Anonymous"
            });

            context.Statuses.AddOrUpdate(x => x.Name, new Status
            {
                Name = "Open",
            });

            context.Statuses.AddOrUpdate(x => x.Name, new Status
            {
                Name = "Closed",
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
                User = context.Users.FirstOrDefault(x => x.DisplayName == "UserDisplayname1"),
                Title = "Election1",
                Status = context.Statuses.FirstOrDefault(x => x.Name == "Open"),
                StartDate = new DateTime(2013, 8, 1),
                EndDate = new DateTime(2013, 9, 1),
                State = context.States.FirstOrDefault(x => x.Name == "Public"),
                Questions = new List<Question>()
                {
                    new Question 
                    {
                        Content = "What is life?",
                        QuestionType =  "Boolean",
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
                        QuestionType =  "Boolean",
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

            context.SaveChanges();

            context.Elections.AddOrUpdate(x => x.Title, new Election()
            {
                User = context.Users.FirstOrDefault(x => x.DisplayName == "UserDisplayname1"),
                Title = "Election222",
                Status = context.Statuses.FirstOrDefault(x => x.Name == "Open"),
                StartDate = new DateTime(2013, 8, 1),
                EndDate = new DateTime(2013, 9, 1),
                State = context.States.FirstOrDefault(x => x.Name == "Public"),
                Questions = new List<Question>()
                {
                    new Question 
                    {
                        Content = "What is life?",
                        QuestionType =  "Boolean",
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
                        QuestionType =  "Boolean",
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

            context.SaveChanges();
        }
    }
}
