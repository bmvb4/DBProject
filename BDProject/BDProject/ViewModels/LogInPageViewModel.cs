using Android.Widget;
using BDProject.API;
using BDProject.Models;
using BDProject.Views;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels
{
    public class LogInPageViewModel : BaseViewModel
    {
        // API Interface
        APIInterface api;

        public LogInPageViewModel()
        {
            test = "Log into the App";
            api = RestService.For<APIInterface>("https://localhost:5001/");

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
                OnPropertyChanged(nameof(Username));
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
                OnPropertyChanged(nameof(Password));
            }
        }

        // Commands
        // Log In command
        public ICommand LogInCommand { get; set; }
        private async void LogInFunction()
        {
            User user = new User()
            {
                Username = Username,
                Password = Password
            };

            var result = await api.LogInUser(user);

            test = result;

            //Toast.MakeText(result, int.Parse(result), ToastLength.Short).Show();

            await Shell.Current.GoToAsync("//HomePage");
        }

        public string test { get; set; }

        // Sign Up command
        public ICommand SignUpCommand { get; set; }
        private async void SignUpFunction()
        {
            await Shell.Current.GoToAsync("//SignUpPage");
        }

    }
}
