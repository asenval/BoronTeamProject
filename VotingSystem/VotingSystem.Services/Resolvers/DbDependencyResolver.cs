using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using VotingSystem.Data;
using VotingSystem.Services.Controllers;

namespace VotingSystem.Services.Resolvers
{
    public class DbDependencyResolver : IDependencyResolver
    {
        private static IDbContextFactory<DbContext> contextFactory = new VotingSystemContextFactory();

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(UsersController))
            {
                return new UsersController(contextFactory);
            }
            /*else if (serviceType == typeof(StudentController))
            {
                return new StudentController(allRepositories);
            }*/
            else
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new List<object>();
        }

        public void Dispose()
        {
        }
    }
}