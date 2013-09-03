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
using System.Transactions;

namespace VotingSystem.Services.Controllers
{
    public class ElectionsController : ApiController
    {
        private const string ElectionStatusOpen = "Open";
        private const string ElectionStatusClosed = "Closed";

        private const string AnonymousUserNickname = "Anonymous";

        private const string ElectionStatePublic = "Public";
        private const string ElectionStatePrivate = "Private";
        private const string ElectionStateUnlisted = "Unlisted";

        private UnitOfWork data;

        public ElectionsController(IDbContextFactory<DbContext> contextFactory)
        {
            this.data = new UnitOfWork(contextFactory);
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
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

            IQueryable<Election> elections = this.data.Elections.All().Include("Questions.Answers").Include("User");

            var result = elections.ToList().Select(x => new ElectionModel(x));
            return result;
        }

        [HttpGet]
        [ActionName("GetById")]
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
            }

            var electionModel = new ElectionModel(election);
            return electionModel;
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody]ElectionModel electionModel,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var user = this.data.Users.GetUserBySessionKey(sessionKey);
            if (user == null)
            {
                var httpError = new HttpError("Invalid username or password.");
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, httpError);
            }

            var election = new Election();
            CopyClassProperties.Fill(election, electionModel);
            foreach (var questionModel in electionModel.Questions)
            {
                var question = new Question();
                CopyClassProperties.Fill(question, questionModel);
                election.Questions.Add(question);
                foreach (var answerModel in question.Answers)
                {
                    var answer = new Question();
                    CopyClassProperties.Fill(answer, answerModel);
                    election.Questions.Add(answer);
                }
            }

            election.User = user;
            election.Status = this.data.Status.Find(s => s.Name == electionModel.StatusName).FirstOrDefault();
            election.State = this.data.State.Find(s => s.Name == electionModel.StateName).FirstOrDefault();

            if (election.StartDate == null)
            {
                electionModel.StartDate = DateTime.Now;
                election.StartDate = electionModel.StartDate;
            }

            this.data.Elections.Add(election);

            electionModel.Owner = user.DisplayName;
            var response = Request.CreateResponse<ElectionModel>(HttpStatusCode.OK, electionModel);
            var resourceLink = Url.Link("ElectionsApi", new { id = election.Id });
            response.Headers.Location = new Uri(resourceLink);

            return response;
        }

        [HttpPost]
        [ActionName("votes")]
        public HttpResponseMessage PostVotes(int electionId, ICollection<VoteModel> voteModels,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var election = this.data.Elections.Get(electionId);
            if (election == null)
            {
                var httpError = new HttpError(String.Format("No election with id {0} exists.",
                    electionId));
                var response = this.Request.CreateResponse(HttpStatusCode.NotFound, httpError);
                return response;
            }

            // validate the election is not closed
            if (election.Status.Name == ElectionStatusClosed || election.EndDate > DateTime.Now)
            {
                var httpError = new HttpError("Election is closed. Cannot vote.");
                var response = this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    httpError);
                return response;
            }

            User user = this.data.Users.GetUserBySessionKey(sessionKey);

            if (election.State.Name == ElectionStatePublic)
            {
                if (user == null)
                {
                    user = this.data.Users.Find(u => u.DisplayName ==
                        AnonymousUserNickname).FirstOrDefault();
                }
            }
            else if (user == null)
            {
                var httpError = new HttpError("You are not logged in.");
                var response = this.Request.CreateResponse(
                    HttpStatusCode.Unauthorized, httpError);
                return response;
            }
            else
            {
                // if we have a valid user authentication and the state is private ->
                // check if the user is in the 'invited users' list for the given election
                if (election.State.Name == ElectionStatePrivate)
                {
                    string commaSeparatedInvitedDisplayNames =
                        election.InvitedUsersDisplayNameString;

                    if (!commaSeparatedInvitedDisplayNames.Contains(user.DisplayName))
                    {
                        var httpError = new HttpError(
                            "User has no authority to vote in this election (not invited).");
                        var response = this.Request.CreateResponse(
                            HttpStatusCode.Unauthorized, httpError);
                        return response;
                    }
                }
            }

            // validate election status (allowing votes)
            if (election.Status.Name != ElectionStatusOpen)
            {
                var httpError = new HttpError(
                    String.Format("Election with id {0} has status of closed. Cannot vote.",
                    electionId));
                var response = this.Request.CreateResponse(HttpStatusCode.Forbidden, httpError);
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

            using (TransactionScope tran = new TransactionScope())
            {
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

                tran.Complete();
            }

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpGet]
        [ActionName("results")]
        public HttpResponseMessage GetResults(int electionId
            // ,
            //[ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey
            )
        {
            var election = this.data.Elections.Get(electionId);
            if (election == null)
            {
                var httpError = new HttpError(String.Format("No election with id {0} exists.",
                    electionId));
                var response = this.Request.CreateResponse(HttpStatusCode.NotFound, httpError);
                return response;
            }
            //User user = this.data.Users.GetUserBySessionKey(sessionKey);

            //if (user == null)
            //{
            //    var httpError = new HttpError("You are not logged in.");
            //    var response = this.Request.CreateResponse(
            //        HttpStatusCode.Unauthorized, httpError);
            //    return response;
            //}
            //else
            //{
            //    // if we have a valid user authentication and the state is not public
            //    if (election.State.Name == ElectionStatePrivate)
            //    {
            //        string commaSeparatedInvitedDisplayNames =
            //            election.InvitedUsersDisplayNameString;

            //        if (!commaSeparatedInvitedDisplayNames.Contains(user.DisplayName))
            //        {
            //            var httpError = new HttpError(
            //                "User has no authority to vote in this election (not invited).");
            //            var response = this.Request.CreateResponse(
            //                HttpStatusCode.Unauthorized, httpError);
            //            return response;
            //        }
            //    }
            //}

            var electionResultModel = new ElectionResultModel(election);

            var resultResponse = Request.CreateResponse<ElectionResultModel>(HttpStatusCode.OK, electionResultModel);
            var resourceLink = Url.Link("ElectionsApi", new { id = election.Id });
            resultResponse.Headers.Location = new Uri(resourceLink);

            return resultResponse;
        }

        [HttpPut]
        [ActionName("close")]
        public HttpResponseMessage CloseElection(int electionId,
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

            Election election = this.data.Elections.Get(electionId);
            this.data.State.Get(election.State.Id); // evaluate

            if (election.User != user)
            {
                var httpError = new HttpError(
                    "You are not the onwer of this election and therefore cannot close it.");
                var errResponse = this.Request.CreateResponse(
                    HttpStatusCode.Unauthorized, httpError);
                return errResponse;
            }

            var closedStatus = this.data.Status.Find(x => 
                x.Name == ElectionStatusClosed).FirstOrDefault();

            election.Status = closedStatus;
            this.data.Elections.Update(electionId, election);

            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        [HttpPut]
        [ActionName("Update")]
        public HttpResponseMessage UpdateElection(int electionId, ElectionModel model
            //,[ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey
            )
        {
            //User user = this.data.Users.GetUserBySessionKey(sessionKey);

            //if (user == null)
            //{
            //    var httpError = new HttpError("You are not logged in.");
            //    var errResponse = this.Request.CreateResponse(
            //        HttpStatusCode.Unauthorized, httpError);
            //    return errResponse;
            //}

            Election election = this.data.Elections.Get(electionId);
            this.data.State.Get(election.State.Id); // evaluate
            this.data.Status.Get(election.Status.Id); // evaluate

            //if (election.User != user)
            //{
            //    var httpError = new HttpError(
            //        "You are not the onwer of this election and therefore cannot update it.");
            //    var errResponse = this.Request.CreateResponse(
            //        HttpStatusCode.Unauthorized, httpError);
            //    return errResponse;
            //}

            CopyClassProperties.Fill(election, model);
            this.data.Elections.Update(election.Id, election);

            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}
