using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VotingSystem.Model
{
    public class Election
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public virtual ICollection<Question> Questions { get; set; }

        [Required]
        public virtual State State { get; set; }

        [Required]
        public virtual ICollection<Tag> Tags { get; set; }

        public Election()
        {
            this.Questions = new HashSet<Question>();
        }
    }
}
