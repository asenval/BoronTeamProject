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
            //    throw new InvalidOperationException("Invalid username or password");
            //}
            IQueryable<Election> elections = this.data.Elections.All().Include("Questions.Answers");

            var result = elections.ToList().Select(x => new ElectionModel(x));
            return result;
            
        }

    }
}
