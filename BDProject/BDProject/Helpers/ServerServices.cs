using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BDProject.Helpers
{
    public class ServerServices
    {
        public const string MainURL = "http://159.65.94.122/";
        public const string RefreshURL = "http://159.65.94.122/token/refresh";

        public static async Task<HttpResponseMessage> SendPostRequestAsync(string path, JObject oJsonObject)
        {
            string URL = $"{MainURL}{path}";
            const string sContentType = "application/json";

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient _client = new HttpClient(clientHandler);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _Globals.GlobalMainUser.AccessToken);

            return await _client.PostAsync(URL, new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType));
        }

        public static async Task<HttpResponseMessage> SendPostRegisterRequestAsync(string path, JObject oJsonObject)
        {
            string URL = $"{MainURL}{path}";
            const string sContentType = "application/json";

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient _client = new HttpClient(clientHandler);

            return await _client.PostAsync(URL, new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType));
        }

        public static async Task<HttpResponseMessage> SendGetRequestAsync(string path, JObject oJsonObject)
        {
            string URL = $"{MainURL}{path}";
            const string sContentType = "application/json";

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient _client = new HttpClient(clientHandler);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _Globals.GlobalMainUser.AccessToken);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(URL),
                Content = new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType)
            };
            return await _client.SendAsync(request).ConfigureAwait(false);
        }

        public static async Task<HttpResponseMessage> SendPutRequestAsync(string path, JObject oJsonObject)
        {
            string URL = $"{MainURL}{path}";
            const string sContentType = "application/json";

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient _client = new HttpClient(clientHandler);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _Globals.GlobalMainUser.AccessToken);

            return await _client.PutAsync(URL, new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType));
        }

        public static async Task<HttpResponseMessage> SendDeleteRequestAsync(string path, JObject oJsonObject)
        {
            string URL = $"{MainURL}{path}";
            const string sContentType = "application/json";

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient _client = new HttpClient(clientHandler);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _Globals.GlobalMainUser.AccessToken);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(URL),
                Content = new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType)
            };
            return await _client.SendAsync(request).ConfigureAwait(false);
        }

        public static async Task RefreshTokenAsync()
        {
            const string sContentType = "application/json";

            JObject oJsonObject = new JObject();
            oJsonObject.Add("AccessToken", _Globals.GlobalMainUser.AccessToken);
            oJsonObject.Add("RefreshToken", _Globals.GlobalMainUser.RefreshToken);

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient _client = new HttpClient(clientHandler);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(RefreshURL),
                Content = new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType)
            };

            var success = await _client.SendAsync(request).ConfigureAwait(false);

            if (success.IsSuccessStatusCode)
            {
                var earthquakesJson = success.Content.ReadAsStringAsync().Result;
                var user = JsonConvert.DeserializeObject<Models.User>(earthquakesJson);

                _Globals.GlobalMainUser.AccessToken = user.AccessToken;
                _Globals.GlobalMainUser.RefreshToken = user.RefreshToken;
            }
        }
    }
}
