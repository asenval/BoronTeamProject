using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using VotingSystem.Model;

namespace VotingSystem.Services.Models
{
    [DataContract]
    public class ElectionModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "startDate")]
        public DateTime StartDate { get; set; }

        [DataMember(Name = "endDate")]
        public DateTime EndDate { get; set; }

        [DataMember(Name = "questions")]
        public virtual ICollection<QuestionGetModel> Questions { get; set; }

        [DataMember(Name = "tags")]
        public virtual ICollection<TagModel> Tags { get; set; }

        [DataMember(Name = "state")]
        public virtual string State { get; set; }

        public ElectionModel(Election election)
        {
            this.Questions = new HashSet<QuestionGetModel>();
            this.Tags = new HashSet<TagModel>();
        }
    }
}