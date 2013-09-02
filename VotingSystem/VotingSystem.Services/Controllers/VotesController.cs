using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ValueProviders;
using VotingSystem.Data;
using VotingSystem.Model;
using VotingSystem.Repository;
using VotingSystem.Services.Attributes;
using VotingSystem.Services.Models;

namespace VotingSystem.Services.Controllers
{
    public class VotesController : ApiController
    {
        private UnitOfWork data;

        public VotesController(IDbContextFactory<DbContext> contextFactory)
        {
            this.data = new UnitOfWork(contextFactory);
        }

        public HttpResponseMessage GetCurrentUserVotes(
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey) 
        {
            User user = this.data.Users.GetUserBySessionKey(sessionKey);

            if (user == null)
            {
                var httpError = new HttpError("You are not logged in.");
                var errResponse = this.Request.CreateResponse(
                    HttpStatusCode.Unauthorized, httpError);
                return errResponse;
            }

            var voteModels = user.Votes.Select(v => new VoteModel(v));

            var response = this.Request.CreateResponse(
                HttpStatusCode.OK, voteModels);
            return response;
        }
    }
}
