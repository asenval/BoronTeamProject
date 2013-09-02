using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public string QuestionType { get; set; }

        public virtual Election Election { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public virtual Result Result { get; set; }

        public Question()
        {
            this.Answers = new HashSet<Answer>();
        }
    }
}
