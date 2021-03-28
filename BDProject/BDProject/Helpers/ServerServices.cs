using BDProject.Models;
using BDProject.ModelWrappers;
using BDProject.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BDProject.Helpers
{
    public class ServerServices
    {

        public static async Task<HttpResponseMessage> SendPostRequestAsync(string path, JObject oJsonObject)
        {
            string URL = "https://10.0.2.2:5001/" + path;
            const string sContentType = "application/json";

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient _client = new HttpClient(clientHandler);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _Globals.GlobalMainUser.Token);

            return await _client.PostAsync(URL, new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType));
        }

        public static async Task<HttpResponseMessage> SendGetRequestAsync(string path, JObject oJsonObject)
        {
            string URL = "https://10.0.2.2:5001/" + path;
            const string sContentType = "application/json";

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient _client = new HttpClient(clientHandler);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _Globals.GlobalMainUser.Token);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(URL),
                Content = new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType)
            };
            return await _client.SendAsync(request).ConfigureAwait(false);
        }

        public static async Task<HttpResponseMessage> SendPost2RequestAsync(string path, JObject oJsonObject)
        {
            string URL = "https://10.0.2.2:5001/" + path;
            const string sContentType = "application/json";

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient _client = new HttpClient(clientHandler);

            return await _client.PostAsync(URL, new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType));
        }

        public static async Task<HttpResponseMessage> SendPutRequestAsync(string path, JObject oJsonObject)
        {
            string URL = "https://10.0.2.2:5001/" + path;
            const string sContentType = "application/json";

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient _client = new HttpClient(clientHandler);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _Globals.GlobalMainUser.Token);

            return await _client.PutAsync(URL, new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType));
        }

        public static async Task<HttpResponseMessage> SendDeleteRequestAsync(string path, JObject oJsonObject)
        {
            string URL = "https://10.0.2.2:5001/" + path;
            const string sContentType = "application/json";

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient _client = new HttpClient(clientHandler);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _Globals.GlobalMainUser.Token);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(URL),
                Content = new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType)
            };
            return await _client.SendAsync(request).ConfigureAwait(false);
        }
    }
}
