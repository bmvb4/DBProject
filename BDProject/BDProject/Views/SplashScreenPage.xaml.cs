using BDProject.Helpers;
using BDProject.Models;
using BDProject.ModelWrappers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDProject.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashScreenPage : ContentPage
    {
        public SplashScreenPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CheckAutoLogin();
        }

        private async void CheckAutoLogin()
        {
            if (Preferences.Get("UsernameKey", string.Empty) != string.Empty && Preferences.Get("PasswordKey", string.Empty) != string.Empty)
            {
                JObject oJsonObject = new JObject();
                oJsonObject.Add("Username", Preferences.Get("UsernameKey", string.Empty));
                oJsonObject.Add("Password", Preferences.Get("PasswordKey", string.Empty));

                var success = await ServerServices.SendPostRequestAsync("login", oJsonObject);

                if (success.IsSuccessStatusCode)
                {
                    var earthquakesJson = success.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<User>(earthquakesJson);

                    _Globals.GlobalMainUser = new UserWrapper(rootobject);

                    success = await ServerServices.SendGetRequestAsync($"posts/getAll/{rootobject.Username}", oJsonObject);

                    if (success.IsSuccessStatusCode)
                    {
                        earthquakesJson = success.Content.ReadAsStringAsync().Result;
                        var postList = JsonConvert.DeserializeObject<List<PostUser>>(earthquakesJson);
                        _Globals.GlobalMainUser.AddPostsFromDB(postList);
                        _Globals.AddPostsFromDB(postList);

                        await Shell.Current.GoToAsync("//HomePage");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Warning!", "Couldn't get Posts", "OK");
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Warning!", "Couldn't load profile", "OK");
                }
            }
            else
            {
                await Shell.Current.GoToAsync("//LogInPage");
            }
        }
    }
}