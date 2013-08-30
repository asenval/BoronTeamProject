using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;

namespace VotingSystem.Services.Resolvers
{
    public class DbDependencyResolver : IDependencyResolver
    {
        //private static AllRepositories<StudentContext> allRepositories = new AllRepositories<StudentContext>();

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
            /*if (serviceType == typeof(MarkController))
            {
                return new MarkController(allRepositories);
            }
            else if (serviceType == typeof(StudentController))
            {
                return new StudentController(allRepositories);
            }
            else if (serviceType == typeof(SchoolController))
            {
                return new SchoolController(allRepositories);
            }
            else
            {
                return null;
            }*/
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