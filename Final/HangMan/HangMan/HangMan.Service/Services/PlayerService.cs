using HangMan.Model.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace HangMan.Service.Services
{
    public class PlayerService : ServiceBase<Player>
    {
        public PlayerService(string securityToken) : this("api/Players", securityToken) { }

        private PlayerService(string baseAddress, string securityToken) : base(baseAddress, securityToken) { }

        public async Task<IEnumerable<Player>> GetMaxScoreAsync(int count)
        {
            try
            {
                // Get the response from the server url and REST path for the data  
                var response = await m_HttpClient.GetAsync(new Uri($"{baseAddress}/GetMaxScore?count={count}"));

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException("Access Denied");

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<IEnumerable<Player>>(await response.Content.ReadAsStringAsync());

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
