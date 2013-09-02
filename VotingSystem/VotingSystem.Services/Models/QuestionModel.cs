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
        public virtual ICollection<AnswerModel> Answers { get; set; }

        [DataMember(Name = "questionType")]
        public string QuestionType { get; set; }

        public QuestionGetModel(Question question)
        {
            CopyClassProperties.Fill(this, question);
            this.Answers = new HashSet<AnswerModel>();

            foreach (var answer in question.Answers)
            {
                this.Answers.Add(new AnswerModel(answer));
            }
        }

        public QuestionGetModel()
        {
            this.Answers = new HashSet<AnswerModel>();
        }
    }
}
