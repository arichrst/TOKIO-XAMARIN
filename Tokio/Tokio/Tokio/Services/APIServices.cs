using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Tokio.Services
{
    public class APIServices : IAPIServices
    {
        protected HttpClient httpClient;

        public APIServices()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://hehehe");
            httpClient.Timeout = TimeSpan.FromMinutes(1);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("token", "yeyeye");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task CallApi<T>(ApiMethod method, string apiUrl, ICallback<T> callback, object body = null)
        {
            HttpResponseMessage client = null;
            callback.OnStart();
            if (method == ApiMethod.GET)
            {
                client = await httpClient.GetAsync(apiUrl);
            }
            else
            {
                var parameter = new StringContent(JsonConvert.SerializeObject(body));
                client = await httpClient.PostAsync(apiUrl,parameter);
            }
            if (client.IsSuccessStatusCode)
            {
                string content = await client.Content.ReadAsStringAsync();
                callback.OnSuccess(JsonConvert.DeserializeObject<T>(content)); 
            }
            else
            {
                callback.OnFailure(client.ReasonPhrase);
            }
            callback.OnEnd();
        }
    }
}
