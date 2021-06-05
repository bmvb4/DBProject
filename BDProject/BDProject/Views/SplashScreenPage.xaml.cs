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