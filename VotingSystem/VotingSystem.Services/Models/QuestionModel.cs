using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace VotingSystem.Services.Models
{
    [DataContract]
    public class QuestionGetModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "content")]
        public string Content { get; set; }

        [DataMember(Name = "answers")]
        public virtual ICollection<AnswerGetModel> AnswersModels { get; set; }

        [DataMember(Name = "voteType")]
        public string VoteType { get; set; }

        public QuestionGetModel()
        {
            this.AnswersModels = new HashSet<AnswerGetModel>();
        }
    }
}
