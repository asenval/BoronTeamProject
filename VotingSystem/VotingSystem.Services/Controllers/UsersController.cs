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
using System.Web.Http.ValueProviders;
using VotingSystem.Services.Attributes;


namespace VotingSystem.Services.Controllers
{
    public class UsersController : ApiController
    {
        private UnitOfWork data;

        public UsersController(IDbContextFactory<DbContext> contextFactory)
        {
            this.data = new UnitOfWork(contextFactory);
        }

        [HttpPost]
        [ActionName("register")]
        public HttpResponseMessage RegisterUser(UserRegisterModel user)
        {
            try
            {
                UserRepository userRepository = this.data.Users;
                userRepository.CreateUser(user.Username, user.DisplayName, user.AuthCode);
                string nickname = string.Empty;
                var sessionKey = userRepository.LoginUser(user.Username, user.AuthCode, out nickname);
                UserLoggedModel result = new UserLoggedModel()
                {
                    DisplayName = user.DisplayName,
                    SessionKey = sessionKey
                };

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (InvalidOperationException e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [HttpPost]
        [ActionName("login")]
        public HttpResponseMessage LoginUser(UserLoginModel user)
        {
            try
            {
                UserRepository userRepository = this.data.Users;
                string displayName = string.Empty;
                var sessionKey = userRepository.LoginUser(user.Username, user.AuthCode, out displayName);
                UserLoggedModel result = new UserLoggedModel()
                {
                    DisplayName = displayName,
                    SessionKey = sessionKey
                };

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (InvalidOperationException e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [HttpPut]
        [ActionName("logout")]
        public HttpResponseMessage LogoutUser(
             [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            try
            {
                UserRepository userRepository = data.Users;
                userRepository.LogoutUser(sessionKey);

                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (InvalidOperationException e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}
