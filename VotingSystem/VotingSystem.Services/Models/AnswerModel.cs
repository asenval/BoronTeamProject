using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using VotingSystem.Model;

namespace VotingSystem.Services.Models
{
    [DataContract]
    public class AnswerModel
    {
        [DataMember(Name="id")]
        public int Id { get; set; }

        [DataMember(Name = "content")]
        public string Content { get; set; }

        public AnswerModel(Answer answer)
        {
            CopyClassProperties.Fill(this, answer);
        }

        public AnswerModel()
        {
        }
    }

    [DataContract]
    public class AnswerResultModel: AnswerModel
    {
        [DataMember(Name = "result")]
        public int Result { get; set; }

        public AnswerResultModel(Answer answer)
        {
            CopyClassProperties.Fill(this, answer);
            if (answer.Votes.Count > 0)
            {
                Result = answer.Votes.Sum(x => x.Value);
            }
        }
    }   
}