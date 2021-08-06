﻿using BDProject.DatabaseModels;
using BDProject.Helpers;
using BDProject.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BDProject.ViewModels
{
    public class LogInPageViewModel : BaseViewModel
    {
        
        public LogInPageViewModel()
        {
            // Assigning functions to the commands
            LogInCommand = new Command(async () => await LogInFunction());
            SignUpCommand = new Command(async () => await SignUpFunction());
            ShowHidePasswordCommand = new Command(ShowHidePasswordFunction);
            ForgotCommand = new Command(async () => await ForgotFunction());
        }

        // Parameters
        // Username parameter
        private string username = "";
        public string Username
        {
            get => username;
            set
            {
                if (value == username) { return; }
                username = value;

                RestrictLenght(username, 35, 1);

                OnPropertyChanged();
            }
        }

        // Username Alert parameter
        private string usernameAlert = "";
        public string UsernameAlert
        {
            get => usernameAlert;
            set
            {
                if (value == usernameAlert) { return; }
                usernameAlert = value;
                OnPropertyChanged();
            }
        }

        // Password parameter
        private string password = "";
        public string Password
        {
            get => password;
            set
            {
                if (value == password) { return; }
                password = value;

                RestrictLenght(password, 133, 2);

                OnPropertyChanged();
            }
        }

        // Password parameter
        private string passwordAlert = "";
        public string PasswordAlert
        {
            get => passwordAlert;
            set
            {
                if (value == passwordAlert) { return; }
                passwordAlert = value;
                OnPropertyChanged();
            }
        }

        // Show password parameter
        private bool hidePassword = true;
        public bool HidePassword
        {
            get => hidePassword;
            set
            {
                if (value == hidePassword) { return; }
                hidePassword = value;
                OnPropertyChanged();
            }
        }

        // Password parameter
        private string eye = "";
        public string Eye
        {
            get => eye;
            set
            {
                if (value == eye) { return; }
                eye = value;
                OnPropertyChanged();
            }
        }

        // Commands
        // Log In command
        public ICommand LogInCommand { get; set; }
        private async Task LogInFunction()
        {
            if (CheckParameters() == true) { return; }

            JObject oJsonObject = new JObject();
            oJsonObject.Add("Username", Username);
            oJsonObject.Add("Password", Password);

            var success = await ServerServices.SendPostRequestAsync("login", oJsonObject);

            if (success.IsSuccessStatusCode)
            {
                Preferences.Set("UsernameKey", Username);
                Preferences.Set("PasswordKey", Password);

                var earthquakesJson = success.Content.ReadAsStringAsync().Result;
                var rootobject = JsonConvert.DeserializeObject<UserDB>(earthquakesJson);

                if(rootobject.Photo == null)
                    _Globals.GlobalMainUser = new User(rootobject) { Photo = _Globals.Base64Bytes };
                else
                    _Globals.GlobalMainUser = new User(rootobject);

                await Shell.Current.GoToAsync("//HomePage");

                Username = "";
                Password = "";
                UsernameAlert = "";
                PasswordAlert = "";
            }
            else
            {
                PasswordAlert = "Username or Password is incorrect";
            }
        }

        // Sign Up command
        public ICommand SignUpCommand { get; set; }
        private async Task SignUpFunction()
        {
            await Shell.Current.GoToAsync("//SignUpPage");

            Username = "";
            Password = "";

            UsernameAlert = "";
            PasswordAlert = "";
        }

        // Show Hide Password command
        public ICommand ShowHidePasswordCommand { get; set; }
        private void ShowHidePasswordFunction()
        {

            if (HidePassword == true)
            {
                HidePassword = false;
                Eye = "";
            }
            else
            {
                HidePassword = true;
                Eye = "";
            }
        }

        // Forgot password comman
        public ICommand ForgotCommand { get; set; }
        private async Task ForgotFunction()
        {
            await Shell.Current.GoToAsync("ForgotPasswordPage");
        }

        // Functions
        // Check parameter
        private bool CheckParameters()
        {
            // error checker
            bool flag = false;

            // Check Username parameter
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrEmpty(Username)) // empty box
            {
                UsernameAlert = "Username is required";
                flag = true;
            }
            else
            {
                UsernameAlert = "";
            }

            // Check Password parameter
            if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrEmpty(Password)) // empty box
            {
                PasswordAlert = "Password is required";
                flag = true;
            }
            else
            {
                PasswordAlert = "";
            }

            // return flag result
            return flag;
        }

        // Restrict lenght function
        private void RestrictLenght(string text, int restriction, int choice)
        {
            if (text.Length >= restriction)
            {
                switch (choice)
                {
                    case 1:
                        Username = text.Remove(text.Length - 1);
                        break;

                    case 2:
                        Password = text.Remove(text.Length - 1);
                        break;

                    default: break;
                }
            }
        }

    }
}
