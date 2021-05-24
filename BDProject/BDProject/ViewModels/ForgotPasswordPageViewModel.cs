using BDProject.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels
{
    public class ForgotPasswordPageViewModel : BaseViewModel
    {

        public ForgotPasswordPageViewModel()
        {

            BackCommand = new Command(async () => await BackFunction());
            SendCommand = new Command(SendFunction);
        }

        // Parameters
        // Email parameter
        private string username = "";
        public string Username
        {
            get => username;
            set
            {
                if (value == username) { return; }
                username = value;

                RestrictLenght(username, 260);

                OnPropertyChanged();
            }
        }

        // Email parameter
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

        // Commands
        // Back to post command
        public ICommand BackCommand { get; set; }
        private async Task BackFunction()
        {
            await Shell.Current.GoToAsync("//LogInPage");
            ClearEverything();
        }

        // Open Posts command
        public ICommand SendCommand { get; set; }
        private async void SendFunction()
        {
            if (EmailValidator() == false) { return; }

            //if (CheckParameters() == true) { return; }

            JObject oJsonObject = new JObject();
            oJsonObject.Add("Username", Username);

            var success = await ServerServices.SendPostRequestAsync("forgetpassword", oJsonObject);

            if (success.IsSuccessStatusCode)
            {
                _Globals.UsernameTemp = Username;
                await Shell.Current.GoToAsync("ResetPasswordPage");
            }
            else
            {
                UsernameAlert = "Something is incorrect";
            }
            
            ClearEverything();
        }

        // Functions
        // Restrict lenght function
        private void RestrictLenght(string text, int restriction)
        {
            if (text.Length >= restriction)
            {
                Username = text.Remove(text.Length - 1);
            }
        }

        // Email validation
        private bool EmailValidator()
        {
            try
            {
                UsernameAlert = "";

                return true;
            }
            catch
            {
                UsernameAlert = "Invalid Username";
                return false;
            }
        }

        // Check parameters
        private bool CheckParameters()
        {
            // error checer
            bool flag = false;

            // Check Email parameter
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrEmpty(Username)) // empty box
            {
                UsernameAlert = "Email is required";
                flag = true;
            }
            else if (Username.Length >= 255) // max letter length
            {
                UsernameAlert = "Email should be less than 255 characters";
                flag = true;
            }
            else if (Username.Length < 3) // min letter length
            {
                UsernameAlert = "Email should be more than 3 characters";
                flag = true;
            }
            else
            {
                UsernameAlert = "";
            }

            // return flag result
            return flag;
        }

        // Clear everything
        private void ClearEverything()
        {
            Username = "";
            UsernameAlert = "";
        }

    }
}
