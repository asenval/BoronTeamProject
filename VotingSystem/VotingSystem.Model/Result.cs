using System;
using System.Collections.Generic;

namespace VotingSystem.Model
{
    public class Result
    {
        public int Id { get; set; }

        public virtual Question Question { get; set; }

        public decimal Value { get; set; }
    }
}