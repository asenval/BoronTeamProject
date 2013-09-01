using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VotingSystem.Model
{
    public class Answer
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }

        public virtual Question Question { get; set; }

        public Answer()
        {
            this.Votes = new HashSet<Vote>();
        }
    }
}