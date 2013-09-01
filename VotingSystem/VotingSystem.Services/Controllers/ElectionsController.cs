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

            IQueryable<Election> elections = this.data.Elections.All().Include("Questions.Answers");

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

    }
}
