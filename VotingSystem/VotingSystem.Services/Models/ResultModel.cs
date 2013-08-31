using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace VotingSystem.Services.Models
{
    [DataContract]
    public class ResultModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "answer")]
        public virtual AnswerGetModel Answer { get; set; }

        [DataMember(Name = "value")]
        public decimal Value { get; set; }
    }
}