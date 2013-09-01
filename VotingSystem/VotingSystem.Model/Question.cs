using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingSystem.Model
{
    public class Question
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string VoteType { get; set; }

        [Required]
        public virtual Election Election { get; set; }

        [Required]
        public virtual ICollection<Answer> Answers { get; set; }

        [Required]
        public virtual ICollection<Result> Results { get; set; }

        public Question()
        {
            this.Answers = new HashSet<Answer>();
            this.Results = new HashSet<Result>();
        }
    }
}
