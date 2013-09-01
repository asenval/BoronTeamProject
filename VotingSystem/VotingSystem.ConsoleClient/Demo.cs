using System;
using System.Collections.Generic;
using VotingSystem.Data;
using VotingSystem.Model;

namespace VotingSystem.ConsoleClient
{
    public class Demo
    {
        public static void Main()
        {
            using (var context = new VotingSystemContext())
            {
                //foreach (var user in context.Users)
                //{
                //    user.SessionKey = null;   
                //}

                var election = new Election()
                {
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Questions = new List<Question>(),
                    Status = new Status() { Name = "active" },
                    State = new State() { Name = "proactive" },
                    Title = "Welcome friends !",
                    Tags = new List<Tag>(),
                    
                };

                context.Elections.Add(election);
                context.SaveChanges();
            }
        }
    }
}
