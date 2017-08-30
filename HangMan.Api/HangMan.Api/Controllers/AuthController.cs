using HangMan.Data.EFContext;
using HangMan.Models;
using HangMan.Service;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace HangMan.Api.Controllers
{
    public class AuthController : ApiController
    {
        private static string Secret { get { return ConfigurationManager.AppSettings.Get("Secret"); } }

        private static int RefreshTokenExpiresIn { get { return Convert.ToInt32(ConfigurationManager.AppSettings.Get("RefreshTokenExpiresIn")); } }
        private static int AccessTokenExpiresIn { get { return Convert.ToInt32(ConfigurationManager.AppSettings.Get("AccessTokenExpiresIn")); } }

        private IUserService userService;

        public AuthController() : this(new UserService(new HangManContext()))
        {
        }

        private AuthController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        [Route("api/Auth/Register")]
        [ResponseType(typeof(OAuthRefreshTokenSchema))]
        public async Task<IHttpActionResult> Register(User user)
        {
            await userService.Add(user);

            return Ok(new OAuthRefreshTokenSchema()
            {
                Expires_in = AccessTokenExpiresIn,
                Token_type = "Bearer",
                Access_token = GetJwtToken(user.UserName, AccessTokenExpiresIn),
                Refresh_token = GetJwtToken(user.UserName, RefreshTokenExpiresIn)
            });
        }

        [Route("api/Auth/token")]
        [ResponseType(typeof(OAuthRefreshTokenSchema))]
        public async Task<IHttpActionResult> Post(string grant_type, string userName, string password)
        {
            if (grant_type != "password")
                return BadRequest("unsupported_grant_type");

            var oAuthSchema = await GetOAuthSchema(userName, password);

            if (oAuthSchema != null)
                return Ok(oAuthSchema);
            else
                return Unauthorized();
        }

        [Route("api/Auth/token")]
        [ResponseType(typeof(OAuthRefreshTokenSchema))]
        public async Task<IHttpActionResult> Post(string grant_type, string refresh_token)
        {
            if (grant_type != "refresh_token")
                return BadRequest("unsupported_grant_type");

            var oAuthSchema = await GetOAuthRefreshSchema(refresh_token);

            if (oAuthSchema != null)
                return Ok(oAuthSchema);
            else
                return Unauthorized();
        }

        private async Task<OAuthRefreshTokenSchema> GetOAuthSchema(string userName, string password)
        {
            if (await userService.Get().AnyAsync(u => u.UserName == userName && u.Password == password))
            {
                return new OAuthRefreshTokenSchema()
                {
                    Expires_in = AccessTokenExpiresIn,
                    Token_type = "Bearer",
                    Access_token = GetJwtToken(userName, AccessTokenExpiresIn),
                    Refresh_token = GetJwtToken(userName, RefreshTokenExpiresIn)
                };
            }
            else
            {
                return null;
            }
        }

        public async Task<OAuthRefreshTokenSchema> GetOAuthRefreshSchema(string refresh_token)
        {
            var simplePrinciple = GetPrincipal(refresh_token);

            if (simplePrinciple == null)
                return null;

            var identity = simplePrinciple.Identity as ClaimsIdentity;

            if (identity == null && !identity.IsAuthenticated)
                return null;

            var username = identity.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(username) && !await userService.Get().AnyAsync(u => u.UserName == username))
                return null;

            return new OAuthRefreshTokenSchema()
            {
                Expires_in = AccessTokenExpiresIn,
                Token_type = "Bearer",
                Access_token = GetJwtToken(username, AccessTokenExpiresIn)
            };
        }

        private string GetJwtToken(string username, int expireSeconds = 300)
        {
            var symmetricKey = Convert.FromBase64String(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Name, username)
                        }),

                Expires = now.AddSeconds(Convert.ToInt32(expireSeconds)),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }

        public ClaimsPrincipal GetPrincipal(string token)
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
