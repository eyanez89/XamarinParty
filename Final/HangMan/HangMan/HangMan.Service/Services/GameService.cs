using HangMan.Model.Model;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HangMan.Service.Services
{
    public class GameService : ServiceBase<Game>
    {
        public GameService(string securityToken) : this("api/Games", securityToken) { }

        private GameService(string baseAddress, string securityToken) : base(baseAddress, securityToken) { }

        public async Task<Game> NewGame(WordDifficulty wordDifficulty)
        {
            try
            {
                // Get the response from the server url and REST path for the data  
                var response = await m_HttpClient.PostAsync(new Uri(baseAddress + "/NewGame"),
                    new StringContent(JsonConvert.SerializeObject(wordDifficulty), Encoding.UTF8, "application/json"));

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException("Access Denied");

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<Game>(await response.Content.ReadAsStringAsync());

                throw new WebException(response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                // TODO:        
                throw ex;
            }
        }

        public async Task<Player> Win(Game game)
        {
            try
            {
                // Get the response from the server url and REST path for the data  
                var response = await m_HttpClient.PostAsync(new Uri(baseAddress + "/Win"),
                    new StringContent(JsonConvert.SerializeObject(game), Encoding.UTF8, "application/json"));

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException("Access Denied");

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<Player>(await response.Content.ReadAsStringAsync());

                throw new WebException(response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                // TODO:        
                throw ex;
            }
        }

        public async Task<Player> Loose(Game game)
        {
            try
            {
                // Get the response from the server url and REST path for the data  
                var response = await m_HttpClient.PostAsync(new Uri(baseAddress + "/Loose"),
                    new StringContent(JsonConvert.SerializeObject(game), Encoding.UTF8, "application/json"));

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException("Access Denied");

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<Player>(await response.Content.ReadAsStringAsync());

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
