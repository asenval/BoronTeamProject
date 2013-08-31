using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VotingSystem.Model
{
    public class Vote
    {
        public int Id { get; set; }
        [Required]
        public int Value { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public virtual User User { get; set; }
        [Required]
        public virtual Answer Answer { get; set; }
    }
}