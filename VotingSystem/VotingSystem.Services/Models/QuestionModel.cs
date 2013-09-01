using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using VotingSystem.Model;

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
        public virtual ICollection<AnswerGetModel> Answers { get; set; }

        [DataMember(Name = "voteType")]
        public string VoteType { get; set; }

        public QuestionGetModel(Question question)
        {
            CopyClassProperties.Fill(this, question);
            this.Answers = new HashSet<AnswerGetModel>();

            foreach (var answer in question.Answers)
            {
                this.Answers.Add(new AnswerGetModel(answer));
            }
        }
    }
}
