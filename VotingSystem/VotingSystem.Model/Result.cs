using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VotingSystem.Model
{
    public class Result
    {
        public int Id { get; set; }
        [Required]
        public virtual Question Question { get; set; }

        public decimal Value { get; set; }
    }
}