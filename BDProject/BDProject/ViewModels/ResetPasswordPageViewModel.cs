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

                RestrictLenght(confirmPassword, 133, 1);

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

            // Reset password
            await Shell.Current.GoToAsync("//LogInPage");

            ClearEverything();
        }

        // Functions
        // Restrict lenght function
        private void RestrictLenght(string text, int restriction, int choice)
        {
            if (text.Length >= restriction)
            {
                switch (choice)
                {
                    case 1:
                        Password = text.Remove(text.Length - 1);
                        break;

                    case 2:
                        ConfirmPassword = text.Remove(text.Length - 1);
                        break;

                    default: break;
                }
            }
        }

        // Check parameters
        private bool CheckParameters()
        {
            // error checer
            bool flag = false;

            // Check Password parameter
            if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrEmpty(Password)) // empty box
            {
                PasswordAlert = "Password is required";
                flag = true;
            }
            else if (Password.Length >= 128) // max letter length
            {
                PasswordAlert = "Password should be less than 128 characters";
                flag = true;
            }
            else if (Password.Length <= 6) // min letter length
            {
                PasswordAlert = "Password should be more than 6 characters";
                flag = true;
            }
            else
            {
                PasswordAlert = "";
            }

            // Check Confirm Password parameter
            if (ConfirmPassword != Password) // matching
            {
                ConfirmPasswordAlert = "Passwords doesn't match";
                flag = true;
            }
            else
            {
                ConfirmPasswordAlert = "";
            }

            // return flag result
            return flag;
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
