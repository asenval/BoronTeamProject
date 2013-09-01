using VotingSystem.Services.Models;
using VotingSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using VotingSystem.Model;
using System.Web.Http.ValueProviders;
using VotingSystem.Services.Attributes;

namespace VotingSystem.Services.Controllers
{
    public class ElectionsController : ApiController
    {
        private UnitOfWork data;

        public ElectionsController(IDbContextFactory<DbContext> contextFactory)
        {
            this.data = new UnitOfWork(contextFactory);
        }

        [HttpGet]
        [ActionName("get")]
        public IEnumerable<ElectionModel> GetAll()
        {
            //var user = this.data.Users.GetUserBySessionKey(sessionKey);
            //if (user == null)
            //{
            //    var httpError = new HttpError("Invalid username or password.");
            //    var response = this.Request.CreateResponse(HttpStatusCode.BadRequest, httpError);
            //    throw new HttpResponseException(response);
            //}

            IQueryable<Election> elections = this.data.Elections.All().Include("Questions.Answers").Include("Users");

            var result = elections.ToList().Select(x => new ElectionModel(x));
            return result;
        }

        [HttpGet]
        public ElectionModel GetById(int electionId)
        {
            //var user = this.data.Users.GetUserBySessionKey(sessionKey);
            //if (user == null)
            //{
            //    var httpError = new HttpError("Invalid username or password.");
            //    var response = this.Request.CreateResponse(HttpStatusCode.BadRequest, httpError);
            //    throw new HttpResponseException(response);
            //}

            var election = this.data.Elections.Get(electionId);

            if (election == null)
            {
                var httpError = new HttpError("No such election exists.");
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, httpError);
                throw new HttpResponseException(response);
                //    var httpError = new HttpError("No such election exists.");
                //    return this.Request.CreateResponse(HttpStatusCode.BadRequest, httpError);
            }

            var electionModel = new ElectionModel(election);
            //var response = Request.CreateResponse<ElectionModel>(HttpStatusCode.OK, electionModel);
            //var resourceLink = Url.Link("ElectionsApi", new { id = election.Id });
            //response.Headers.Location = new Uri(resourceLink);

            //return response;
            return electionModel;
        }

        [HttpPost]
        [ActionName("votes")]
        public HttpResponseMessage PostVotes(int electionId,
            ICollection<VoteModel> voteModels,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var user = this.data.Users.GetUserBySessionKey(sessionKey);
            if (user == null)
            {
                var httpError = new HttpError("User is not logged in.");
                var response = this.Request.CreateResponse(HttpStatusCode.Unauthorized, httpError);
                return response;
            }

            var election = this.data.Elections.Get(electionId);
            if (election == null)
            {
                var httpError = new HttpError(String.Format("No election with id {0} exists.", 
                    electionId));
                var response = this.Request.CreateResponse(HttpStatusCode.NotFound, httpError);
                return response;
            }

            var electionQuestions = election.Questions.ToList();
            
            HashSet<int> electionAllAnswerIds = new HashSet<int>();
            
            // gather all the answerIds for the given election questions
            foreach (var question in electionQuestions)
            {
                var thisQuestionAnswersIds = this.data.Answers.Find(
                    a => a.Question.Id == question.Id).Select(q => q.Id).ToList();

                foreach (var answerId in thisQuestionAnswersIds)
	            {
                    electionAllAnswerIds.Add(answerId);
	            }
            }

            Dictionary<int, int> answerIdVoteValuePairs = new Dictionary<int, int>();

            var currentDateTime = DateTime.Now;
            foreach (var voteModel in voteModels)
            {
                // confirm that we are trying to put a value to an answer that
                // is FROM this election!
                if (!electionAllAnswerIds.Contains(voteModel.AnswerId))
                {
                    var httpError = new HttpError(
                        "Giving a vote to an answer with ID which is not from this election.");
                    var response = Request.CreateResponse(HttpStatusCode.Unauthorized,
                        httpError);
                    throw new HttpResponseException(response);
                }

                // find the answer with id the current answerId from voteModel
                // and give it a new vote
                var answer = this.data.Answers.Get(voteModel.AnswerId);
                              
                if (answer == null)
                {
                    var httpError = new HttpError(
                        String.Format("Answer with id {0} does not exist.", 
                        voteModel.AnswerId));
                    var response = Request.CreateResponse(HttpStatusCode.NotFound,
                        httpError);
                    throw new HttpResponseException(response);
                }

                var newVote = new Vote
                {
                    Answer = answer,
                    User = user,
                    Value = voteModel.Value,
                    CreationDate = currentDateTime,
                };

                this.data.Votes.Add(newVote);
            }
            
            
            return Request.CreateResponse(HttpStatusCode.Created);
        }
    }
}
