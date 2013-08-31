using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace VotingSystem.Services.Models
{
    [DataContract]
    public class VoteModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "value")]
        public int Value { get; set; }

        [DataMember(Name = "creationDate")]
        public DateTime CreationDate { get; set; }

        [DataMember(Name = "user")]
        public virtual UserModel User { get; set; }

        [DataMember(Name = "answer")]
        public virtual AnswerGetModel Answer { get; set; }
    }

    [DataContract]
    public class VoteGetAllModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "value")]
        public int Value { get; set; }

        [DataMember(Name = "creationDate")]
        public DateTime CreationDate { get; set; }
    }
}