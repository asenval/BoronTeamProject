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
}