using BDProject.DatabaseModels;
using BDProject.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels
{
    public class ResetPasswordPageViewModel : BaseViewModel
    {

        public ResetPasswordPageViewModel()
        {

            BackCommand = new Command(async () => await BackFunction());
            ResetCommand = new Command(ResetFunction);
            ResendCodeCommand = new Command(ResendCodeFunction);
        }

        // Parameters
        // Code parameter
        private string code = "";
        public string Code
        {
            get => code;
            set
            {
                if (value == code) { return; }
                code = value;
                OnPropertyChanged(nameof(Code));
            }
        }

        // Code Alert parameter
        private string codeAlert = "";
        public string CodeAlert
        {
            get => codeAlert;
            set
            {
                if (value == codeAlert) { return; }
                codeAlert = value;
                OnPropertyChanged(nameof(CodeAlert));
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

                RestrictLenght(password, 133, 1);

                OnPropertyChanged(nameof(Password));
            }
        }

        // Password Alert parameter
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

        // Confirm PasswordAlert parameter
        private string confirmPassword = "";
        public string ConfirmPassword
        {
            get => confirmPassword;
            set
            {
                if (value == confirmPassword) { return; }
                confirmPassword = value;

                RestrictLenght(confirmPassword, 133, 2);

                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }

        // Confirm Password Alert parameter
        private string confirmPasswordAlert = "";
        public string ConfirmPasswordAlert
        {
            get => confirmPasswordAlert;
            set
            {
                if (value == confirmPasswordAlert) { return; }
                confirmPasswordAlert = value;
                OnPropertyChanged(nameof(ConfirmPasswordAlert));
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
        public ICommand ResetCommand { get; set; }
        private async void ResetFunction()
        {
            if (CheckParameters() == true) { return; }

            try
            {
                JObject oJsonObject = new JObject();
                oJsonObject.Add("Username", _Globals.UsernameTemp);

                var success = await ServerServices.SendPostRequestAsync($"forgetpassword/{Code}", oJsonObject);

                if (success.IsSuccessStatusCode)
                {
                    var earthquakesJson = success.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<UserDB>(earthquakesJson);

                    _Globals.GlobalMainUser.AccessToken = rootobject.AccessToken;

                    JObject oJsonObject1 = new JObject();
                    oJsonObject1.Add("Username", _Globals.UsernameTemp);
                    oJsonObject1.Add("Password", Password);
                    success = await ServerServices.SendPutRequestAsync("forgetpassword", oJsonObject1);

                    if (success.IsSuccessStatusCode)
                    {
                        await Shell.Current.GoToAsync("//LogInPage");
                    }
                    else
                    {
                        CodeAlert = "Something is incorrect 2";
                    }
                }
                else
                {
                    CodeAlert = "Something is incorrect";
                }

                ClearEverything();
            }
            catch(Exception ex)
            {
                string s = ex.Message;
            }
        }

        // Open Posts command
        public ICommand ResendCodeCommand { get; set; }
        private async void ResendCodeFunction()
        {
            JObject oJsonObject = new JObject();
            oJsonObject.Add("Username", _Globals.UsernameTemp);

            var success = await ServerServices.SendPostRequestAsync("forgetpassword", oJsonObject);

            //if (success.IsSuccessStatusCode)
            //{
            //    _Globals.UsernameTemp = Username;
            //}
        }

        // Functions
        // Restrict lenght function
        private void RestrictLenght(string text, int restriction, int choice)
        {
            if (text.Length >= restriction)
                switch (choice)
                {
                    case 1: Password = text.Remove(text.Length - 1); break;
                    case 2: ConfirmPassword = text.Remove(text.Length - 1); break;
                    default: break;
                }
        }

        // Check parameters
        private bool CheckParameters()
        {
            // Check Password parameter
            if (string.IsNullOrEmpty(Password.Trim())) // empty box
            {
                PasswordAlert = "Password is required";
                return true;
            }
            if (Password.Length >= 128) // max letter length
            {
                PasswordAlert = "Password should be less than 128 characters";
                return true;
            }
            if (Password.Length <= 6) // min letter length
            {
                PasswordAlert = "Password should be more than 6 characters";
                return true;
            }

            // Check Confirm Password parameter
            if (string.IsNullOrEmpty(Password.Trim())) // empty box
            {
                PasswordAlert = "Password is required";
                return true;
            }
            if (ConfirmPassword.Length >= 128) // max letter length
            {
                ConfirmPasswordAlert = "Password should be less than 128 characters";
                return true;
            }
            if (ConfirmPassword.Length <= 6) // min letter length
            {
                ConfirmPasswordAlert = "Password should be more than 6 characters";
                return true;
            }
            if (ConfirmPassword != Password) // matching
            {
                ConfirmPasswordAlert = "Passwords doesn't match";
                return true;
            }

            ClearEverything();
            return false;
        }

        // Clear everything
        private void ClearEverything()
        {
            Password = "";
            PasswordAlert = "";
            ConfirmPassword = "";
            ConfirmPasswordAlert = "";
        }

    }
}
