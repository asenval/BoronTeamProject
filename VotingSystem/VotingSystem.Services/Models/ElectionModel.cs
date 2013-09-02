using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using VotingSystem.Model;

namespace VotingSystem.Services.Models
{
    [DataContract]
    public class ElectionBaseModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "startDate")]
        public DateTime? StartDate { get; set; }

        [DataMember(Name = "endDate")]
        public DateTime EndDate { get; set; }

        [DataMember(Name = "tags")]
        public virtual ICollection<TagModel> Tags { get; set; }

        [DataMember(Name = "state")]
        public virtual string StateName { get; set; }

        [DataMember(Name = "status")]
        public virtual string StatusName { get; set; }

        [DataMember(Name = "ownerNickname")]
        public virtual string Owner { get; set; }

        public ElectionBaseModel(Election election)
        {
            CopyClassProperties.Fill(this, election);
            this.StateName = election.State.Name;
            this.StatusName = election.Status.Name;
            this.Owner = election.User.DisplayName;
        }

        public ElectionBaseModel()
        {
        }
    }

    [DataContract]
    public class ElectionModel: ElectionBaseModel
    {
        [DataMember(Name = "questions")]
        public virtual ICollection<QuestionGetModel> Questions { get; set; }

        public ElectionModel(Election election)
            :base(election)
        {
            this.Questions = new HashSet<QuestionGetModel>();

            foreach (var question in election.Questions)
            {
                this.Questions.Add(new QuestionGetModel(question));
            }
        }

        public ElectionModel()
            :base()
        {
            this.Questions = new HashSet<QuestionGetModel>();
        }
    }

    public class ElectionResultModel: ElectionBaseModel
    {        
        [DataMember(Name = "questions")]
        public virtual ICollection<QuestionResultModel> Questions { get; set; }

        public ElectionResultModel(Election election)
            :base(election)
        {            
            this.Questions = new HashSet<QuestionResultModel>();

            foreach (var question in election.Questions)
            {
                this.Questions.Add(new QuestionResultModel(question));
            }
        }
    }
}
