using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HangMan.Service;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Data.Entity;

namespace HangMan.API.Middleware.Authentication
{
    public class JWTAuthenticationHandler : AuthenticationHandler<JWTAuthenticationOptions>
    {
        private readonly IUserService userService;
        private static string Secret { get { return ConfigurationManager.AppSettings.Get("Secret"); } }

        public JWTAuthenticationHandler(IUserService userService)
        {
            this.userService = userService ?? throw new ArgumentNullException("PinAuthenticationService");
        }

        protected override async Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            if (!Request.Headers.TryGetValue("Authorization", out string[] authorizationHedders))
                return null;

            var authorization = authorizationHedders[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (authorization.Count() != 2 && authorization[0] != "Bearer")
                return null;

            string username = ValidateToken(authorization[1]);
            if (string.IsNullOrEmpty(username))
                return null;

            if (!await userService.Get().AnyAsync(u => u.UserName == username))
                return null;

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false
            };

            var claimCollection = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username)
                };

            var claimsIdentity = new ClaimsIdentity(claimCollection, "Custom");
            var ticket = new AuthenticationTicket(claimsIdentity, authProperties);
            return ticket;
        }

        private string ValidateToken(string token)
        {
            var simplePrinciple = GetPrincipal(token);

            if (simplePrinciple == null)
                return null;

            var identity = simplePrinciple.Identity as ClaimsIdentity;

            if (identity == null && !identity.IsAuthenticated)
                return null;

            var usernameClaim = identity.FindFirst(ClaimTypes.Name);
            return usernameClaim?.Value;
        }

        private ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Convert.FromBase64String(Secret);

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);

                return principal;
            }

            catch (Exception)
            {
                return null;
            }
        }
    }
}