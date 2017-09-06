using HangMan.Model.Model;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HangMan.Service.Services
{
    public class AuthService : ServiceBase<User>
    {
        public AuthService() : base("api/Auth", "") { }

        public AuthService(string baseAddress, string securityToken) : base(baseAddress, securityToken) { }

        public async Task<OAuthRefreshTokenSchema> Token(string userName, string password)
        {
            try
            {
                var uri = new Uri($"{baseAddress}/token?grant_type=password&userName={userName}&password={password}");

                //// Get the response from the server url and REST path for the data  
                var response = await m_HttpClient.GetAsync(uri);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException("Access Denied");

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<OAuthRefreshTokenSchema>(await response.Content.ReadAsStringAsync());

                //var content = await response.ReadAsStringAsync();

                throw new WebException(response.ToString());
            }
            catch (Exception ex)
            {
                // TODO:        
                throw ex;
            }
        }

        public async Task<OAuthRefreshTokenSchema> Token(string refresh_token)
        {
            try
            {
                // Get the response from the server url and REST path for the data  
                var response = await m_HttpClient.GetAsync(new Uri($"{baseAddress}/token?grant_type=refresh_token&refresh_token={refresh_token}"));

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException("Access Denied");

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<OAuthRefreshTokenSchema>(await response.Content.ReadAsStringAsync());

                throw new WebException(response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                // TODO:        
                throw ex;
            }
        }
    }
}
