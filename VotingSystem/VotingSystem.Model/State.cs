using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VotingSystem.Model
{
    public class State
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Election> Elections { get; set; }

        public State()
        {
            this.Elections = new HashSet<Election>();
        }
    }
}