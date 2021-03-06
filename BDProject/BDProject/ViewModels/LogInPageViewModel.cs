using BDProject.Views;
using System;
using System.Collections.Generic;
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
            await Shell.Current.GoToAsync("//HomePage");
        }

        // Sign Up command
        public ICommand SignUpCommand { get; set; }
        private async void SignUpFunction()
        {
            await Shell.Current.GoToAsync("//SignUpPage");
        }

    }
}
