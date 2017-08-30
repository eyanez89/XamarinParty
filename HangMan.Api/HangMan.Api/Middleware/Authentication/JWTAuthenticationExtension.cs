using HangMan.Service;
using Owin;

namespace HangMan.API.Middleware.Authentication
{
    public static class JWTAuthenticationExtension
    {
        public static void UseJWTAuthentication(this IAppBuilder appBuilder, IUserService userService)
        {
            appBuilder.Use<JWTAuthMiddleware>(new JWTAuthenticationOptions(), userService);
        }
    }
}