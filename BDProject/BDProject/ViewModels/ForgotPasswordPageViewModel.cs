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
        private string email = "";
        public string Email
        {
            get => email;
            set
            {
                if (value == email) { return; }
                email = value;

                RestrictLenght(email, 260);

                OnPropertyChanged(nameof(Email));
            }
        }

        // Email parameter
        private string emailAlert = "";
        public string EmailAlert
        {
            get => emailAlert;
            set
            {
                if (value == emailAlert) { return; }
                emailAlert = value;
                OnPropertyChanged(nameof(EmailAlert));
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

            if (CheckParameters() == true) { return; }

            // send email

            await Shell.Current.GoToAsync("ResetPasswordPage");
            
            ClearEverything();
        }

        // Functions
        // Restrict lenght function
        private void RestrictLenght(string text, int restriction)
        {
            if (text.Length >= restriction)
            {
                Email = text.Remove(text.Length - 1);
            }
        }

        // Email validation
        private bool EmailValidator()
        {
            try
            {
                MailAddress m = new MailAddress(Email);

                EmailAlert = "";

                return true;
            }
            catch
            {
                EmailAlert = "Invalid Email";
                return false;
            }
        }

        // Check parameters
        private bool CheckParameters()
        {
            // error checer
            bool flag = false;

            // Check Email parameter
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrEmpty(Email)) // empty box
            {
                EmailAlert = "Email is required";
                flag = true;
            }
            else if (Email.Length >= 255) // max letter length
            {
                EmailAlert = "Email should be less than 255 characters";
                flag = true;
            }
            else if (Email.Length < 3) // min letter length
            {
                EmailAlert = "Email should be more than 3 characters";
                flag = true;
            }
            else
            {
                EmailAlert = "";
            }

            // return flag result
            return flag;
        }

        // Clear everything
        private void ClearEverything()
        {
            Email = "";
            EmailAlert = "";
        }

    }
}
