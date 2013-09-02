using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using VotingSystem.Model;

namespace VotingSystem.Services.Models
{
    [DataContract]
    public class QuestionBaseModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "content")]
        public string Content { get; set; }

        [DataMember(Name = "questionType")]
        public string QuestionType { get; set; }

        public QuestionBaseModel(Question question)
        {
            CopyClassProperties.Fill(this, question);
        }

        public QuestionBaseModel()
        {
        }
    }

    [DataContract]
    public class QuestionGetModel: QuestionBaseModel
    {
        [DataMember(Name = "answers")]
        public virtual ICollection<AnswerModel> Answers { get; set; }

        public QuestionGetModel(Question question)
            :base(question)
        {
            this.Answers = new HashSet<AnswerModel>();

            foreach (var answer in question.Answers)
            {
                this.Answers.Add(new AnswerModel(answer));
            }
        }

        public QuestionGetModel()
            :base()
        {
            this.Answers = new HashSet<AnswerModel>();
        }
    }

    [DataContract]
    public class QuestionResultModel : QuestionBaseModel
    {
        [DataMember(Name = "answers")]
        public virtual ICollection<AnswerResultModel> Answers { get; set; }

        public QuestionResultModel(Question question)
            : base(question)
        {
            this.Answers = new HashSet<AnswerResultModel>();

            foreach (var answer in question.Answers)
            {
                this.Answers.Add(new AnswerResultModel(answer));
            }
        }
    }
}
