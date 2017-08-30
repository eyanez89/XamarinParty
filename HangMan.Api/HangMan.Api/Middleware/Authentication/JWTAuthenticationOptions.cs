using Microsoft.Owin.Security;

namespace HangMan.API.Middleware.Authentication
{
    public class JWTAuthenticationOptions : AuthenticationOptions
    {
        public JWTAuthenticationOptions() : base("Authorization")
        {
        }
    }
}