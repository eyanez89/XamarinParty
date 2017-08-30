using System;
using HangMan.Service;
using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;

namespace HangMan.API.Middleware.Authentication
{
    public class JWTAuthMiddleware : AuthenticationMiddleware<JWTAuthenticationOptions>
    {
        private readonly IUserService userService;

        public JWTAuthMiddleware(OwinMiddleware nextMiddleware, JWTAuthenticationOptions authOptions, IUserService userService)
            : base(nextMiddleware, authOptions)
        {
            this.userService = userService ?? throw new ArgumentNullException("PinAuthenticationService");
        }

        protected override AuthenticationHandler<JWTAuthenticationOptions> CreateHandler()
        {
            return new JWTAuthenticationHandler(userService);
        }
    }
}