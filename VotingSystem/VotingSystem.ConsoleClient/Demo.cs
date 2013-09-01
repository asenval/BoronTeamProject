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
                foreach (var user in context.Users)
                {
                    user.SessionKey = null;   
                }

                context.SaveChanges();
            }
        }
    }
}
