using BDProject.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels
{
    public class SignUpPageViewModel : BaseViewModel
    {

        public SignUpPageViewModel()
        {
            // Assigning functions to the commands
            SignUpCommand = new Command(SignUpFunction);
        }

        // Parameters
        // First name parameter
        private string firstName = "";
        public string FirstName
        {
            get => firstName;
            set
            {
                if (value == firstName) { return; }
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        // Last name parameter
        private string lastName = "";
        public string LastName
        {
            get => lastName;
            set
            {
                if (value == lastName) { return; }
                lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

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

        // Confirm Password parameter
        private string confirmPassword = "";
        public string ConfirmPassword
        {
            get => confirmPassword;
            set
            {
                if (value == confirmPassword) { return; }
                confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }

        // Email parameter
        private string email = "";
        public string Email
        {
            get => email;
            set
            {
                if (value == email) { return; }
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        // Commands
        // Sign Up command
        public ICommand SignUpCommand { get; set; }
        private void SignUpFunction()
        {
            App.Current.MainPage = new AppShell();
        }

    }
}
