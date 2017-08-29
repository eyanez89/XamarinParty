using HangMan.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HangMan.Service
{
    public abstract class ServiceBase<T> : IServiceBase<T>
        where T : class, IEntity
    {
        private Uri baseAddress;
        private string securityToken;
        private HttpClient m_HttpClient;

        public ServiceBase(string baseAddress, string securityToken)
        {
            if (string.IsNullOrEmpty(baseAddress))
                throw new ArgumentNullException("baseAddress");

            this.baseAddress = new Uri(baseAddress);
            // Storing the security token in a class property of type string    
            this.securityToken = securityToken.StartsWith("Bearer") ? securityToken.Substring(7) : securityToken;
            m_HttpClient = CreateHttpClient();
        }

        private HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (string.IsNullOrEmpty(securityToken))
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", securityToken);

            return httpClient;
        }

        public async Task<IEnumerable<T>> Get()
        {
            try
            {
                // Get the response from the server url and REST path for the data  
                var response = await m_HttpClient.GetAsync(baseAddress);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException("Access Denied");

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<IEnumerable<T>>(await response.Content.ReadAsStringAsync());

                throw new WebException(response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                // TODO:        
                throw ex;
            }
        }

        public async Task<T> Get(string path)
        {
            try
            {
                // Get the response from the server url and REST path for the data  
                var response = await m_HttpClient.GetAsync(new Uri(baseAddress, path));

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException("Access Denied");

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());

                throw new WebException(response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                // TODO:        
                throw ex;
            }
        }

        public async Task<T> Post(T entity)
        {
            try
            {
                // Get the response from the server url and REST path for the data  
                var response = await m_HttpClient.PostAsync(baseAddress,
                    new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json"));

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException("Access Denied");

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());

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
