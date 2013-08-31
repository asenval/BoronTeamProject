using System;
using System.Collections.Generic;

namespace VotingSystem.Model
{
    public class Result
    {
        public int Id { get; set; }

        // ? results but for what exactly? for answer? :
        public virtual Answer Answer { get; set; }

        public decimal Value { get; set; }
    }
}