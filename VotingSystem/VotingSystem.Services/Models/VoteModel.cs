﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using VotingSystem.Model;

namespace VotingSystem.Services.Models
{
    [DataContract]
    public class VoteModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "value")]
        public int Value { get; set; }

        [DataMember(Name = "answerId")]
        public int AnswerId { get; set; }

        public VoteModel(Vote vote)
        {
            CopyClassProperties.Fill(this, vote);
        }
    }
}