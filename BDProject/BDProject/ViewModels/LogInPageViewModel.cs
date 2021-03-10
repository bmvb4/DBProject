using Android.Widget;
using BDProject.Models;
using BDProject.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels
{
    public class LogInPageViewModel : BaseViewModel
    {
        
        public LogInPageViewModel()
        {
            // Assigning functions to the commands
            LogInCommand = new Command(LogInFunction);
            SignUpCommand = new Command(SignUpFunction);
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

                OnPropertyChanged(nameof(Username));
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
                OnPropertyChanged(nameof(UsernameAlert));
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

                OnPropertyChanged(nameof(Password));
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
                OnPropertyChanged(nameof(PasswordAlert));
            }
        }

        // Commands
        // Log In command
        public ICommand LogInCommand { get; set; }
        private async void LogInFunction()
        {
            if (CheckParameters() == true) { return; }

            const string URL = "https://10.0.2.2:5001/login";
            const string sContentType = "application/json";

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient _client = new HttpClient(clientHandler);

            JObject oJsonObject = new JObject();
            oJsonObject.Add("Username", Username);
            oJsonObject.Add("Password", Password);

            var result = await _client.PostAsync(URL, new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType));
            
            if (result.IsSuccessStatusCode)
            {
                var earthquakesJson = result.Content.ReadAsStringAsync().Result;
                var rootobject = JsonConvert.DeserializeObject<User>(earthquakesJson);

                await Shell.Current.GoToAsync("//HomePage");
            }
            else
            {
                PasswordAlert = "Username or Pasword are incrrect";
            }
        }

        // Sign Up command
        public ICommand SignUpCommand { get; set; }
        private async void SignUpFunction()
        {
            await Shell.Current.GoToAsync("//SignUpPage");

            Username = "";
            Password = "";

            UsernameAlert = "";
            PasswordAlert = "";
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
