using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace VotingSystem.Model
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public virtual ICollection<Election> Elections { get; set; }

        public Tag()
        {
            this.Elections = new HashSet<Election>();
        }
    }
}
