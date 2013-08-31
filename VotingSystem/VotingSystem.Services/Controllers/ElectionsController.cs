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

        /*[HttpPost]
        [ActionName("get")]
        public IEnumerable<ElectionModel> GetAll()
        {
            //var user = this.data.Users.GetUserBySessionKey(sessionKey);
            //if (user == null)
            //{
            //    throw new InvalidOperationException("Invalid username or password");
            //}
            var elections = this.data.Elections.All();

            return elections.ToList().Select(x => new ElectionModel (x));
            }
        }*/

    }
}
