using BDProject.DatabaseModels;
using BDProject.Helpers;
using BDProject.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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
            //Preferences.Set("UsernameKey", string.Empty);
            //Preferences.Set("PasswordKey", string.Empty);

            //=================== TESTING ========================
            _Globals.GlobalMainUser = new User(new UserDB { FirstName = "John", LastName = "Johnson", Username = "jjohnson", Description = "Traveler, Musician and a Chef", Follower = 513, Followed = 102435, PostCount = 104, Email = "johnjhonson@gmail.com" }) { Photo = Convert.FromBase64String("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") };
            //=================== TESTING ========================

            if (Preferences.Get("UsernameKey", string.Empty) != string.Empty || Preferences.Get("PasswordKey", string.Empty) != string.Empty)
            {
                JObject oJsonObject = new JObject();
                oJsonObject.Add("Username", Preferences.Get("UsernameKey", string.Empty));
                oJsonObject.Add("Password", Preferences.Get("PasswordKey", string.Empty));

                var success = await ServerServices.SendPostRequestAsync("login", oJsonObject);

                if (success.IsSuccessStatusCode)
                {
                    var earthquakesJson = success.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<UserDB>(earthquakesJson);


                    if (rootobject.Photo == null)
                        _Globals.GlobalMainUser = new User(rootobject) { Photo = _Globals.Base64Bytes };
                    else
                        _Globals.GlobalMainUser = new User(rootobject);

                    await Shell.Current.GoToAsync("//HomePage");
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