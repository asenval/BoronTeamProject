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

        public DbSet<Election> Elections { get; set; }

    }
}
