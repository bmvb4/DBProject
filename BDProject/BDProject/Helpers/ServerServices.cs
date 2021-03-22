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

        public static async Task<bool> SendRequestAsync(string path, JObject oJsonObject)
        {
            string URL = "https://10.0.2.2:5001/" + path;
            const string sContentType = "application/json";

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient _client = new HttpClient(clientHandler);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _Globals.GlobalMainUser.Username);

            var result = await _client.PostAsync(URL, new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType));

            if (result.IsSuccessStatusCode)
            {
                if (path == "login")
                {
                    var earthquakesJson = result.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<User>(earthquakesJson);

                    _Globals.GlobalMainUser = new UserWrapper(rootobject);

                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", rootobject.token);
                    result = await _client.GetAsync("https://10.0.2.2:5001/posts/image/getAll/" + rootobject.Username);

                    if (result.IsSuccessStatusCode)
                    {
                        earthquakesJson = result.Content.ReadAsStringAsync().Result;
                        var postList = JsonConvert.DeserializeObject<List<Post>>(earthquakesJson);
                        //_Globals.GlobalMainUser.MyPosts = postList;
                    }

                    return true;
                }

                if (path == "register")
                {
                    var earthquakesJson = result.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<User>(earthquakesJson);

                    _Globals.GlobalMainUser = new UserWrapper(rootobject);

                    return true;
                }

                if (path == "posts/image/post") 
                {
                    var earthquakesJson = result.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<Post>(earthquakesJson);

                    return true;
                }

                if (path == "profile/update/")
                {
                    var earthquakesJson = result.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<Post>(earthquakesJson);

                    return true;
                }
            }

            return false;
        }

    }
}
