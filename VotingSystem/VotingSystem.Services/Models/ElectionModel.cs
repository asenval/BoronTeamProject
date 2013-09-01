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
        public virtual string StateName { get; set; }

        [DataMember(Name = "status")]
        public virtual string StatusName { get; set; }

        public ElectionModel(Election election)
        {
            CopyClassProperties.Fill(this, election);
            this.StateName = election.State.Name;
            this.StatusName = election.Status.Name;
            this.Questions = new HashSet<QuestionGetModel>();

            foreach (var question in election.Questions)
            {
                this.Questions.Add(new QuestionGetModel(question));
            }
        }
    }
}