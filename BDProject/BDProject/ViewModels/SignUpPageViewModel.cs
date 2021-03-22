using BDProject.Helpers;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels
{
    public class SignUpPageViewModel : BaseViewModel
    {
        public SignUpPageViewModel()
        {
            // Assigning functions to the commands
            SignUpCommand = new Command(async () => await SignUpFunction());
            CancelCommand = new Command(async () => await CancelFunction());
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

                RestrictLenght(firstName, 31, 1);
                CheckSymbolsAndNumbers(firstName, 1);

                OnPropertyChanged(nameof(FirstName));
            }
        }
        // First name Alert parameter
        private string firstNameAlert = "";
        public string FirstNameAlert
        {
            get => firstNameAlert;
            set
            {
                if (value == firstNameAlert) { return; }
                firstNameAlert = value;
                OnPropertyChanged(nameof(FirstNameAlert));
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

                RestrictLenght(lastName, 31, 2);
                CheckSymbolsAndNumbers(lastName, 2);

                OnPropertyChanged(nameof(LastName));
            }
        }
        // Last name Alert parameter
        private string lastNameAlert = "";
        public string LastNameAlert
        {
            get => lastNameAlert;
            set
            {
                if (value == lastNameAlert) { return; }
                lastNameAlert = value;
                OnPropertyChanged(nameof(LastNameAlert));
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

                RestrictLenght(username, 35, 3);

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

                RestrictLenght(password, 133, 4);

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

        // Confirm Password parameter
        private string confirmPassword = "";
        public string ConfirmPassword
        {
            get => confirmPassword;
            set
            {
                if (value == confirmPassword) { return; }
                confirmPassword = value;

                RestrictLenght(confirmPassword, 133, 5);

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

        // Email parameter
        private string email = "";
        public string Email
        {
            get => email;
            set
            {
                if (value == email) { return; }
                email = value;

                RestrictLenght(email, 260, 6);

                OnPropertyChanged(nameof(Email));
            }
        }
        // Email Alert parameter
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
        // Sign Up command
        public ICommand SignUpCommand { get; set; }
        private async Task SignUpFunction()
        {
            // vaidate email and username
            if (EmailValidator() == false || UsernameValidation() == false) { return; }

            // checking all parameters
            if (CheckParameters() == true) { return; }

            JObject oJsonObject = new JObject();
            oJsonObject.Add("Username", Username);
            oJsonObject.Add("Password", Password);
            oJsonObject.Add("FirstName", FirstName);
            oJsonObject.Add("LastName", LastName);
            oJsonObject.Add("Email", Email);

            bool success = await ServerServices.SendRequestAsync("register", oJsonObject);

            if (success)
            {
                await Shell.Current.GoToAsync($"//HomePage");
            }
        }

        // Sign Up command
        public ICommand CancelCommand { get; set; }
        private async Task CancelFunction()
        {
            await Shell.Current.GoToAsync("//LogInPage");

            FirstName = "";
            LastName = "";
            Username = "";
            Password = "";
            ConfirmPassword = "";
            Email = "";

            FirstNameAlert = "";
            LastNameAlert = "";
            UsernameAlert = "";
            PasswordAlert = "";
            ConfirmPasswordAlert = "";
            EmailAlert = "";
        }

        // Functions
        // Restrict lenght function
        private void RestrictLenght(string text, int restriction, int choice)
        {
            if(text.Length >= restriction)
            {
                switch (choice)
                {
                    case 1:
                        FirstName = text.Remove(text.Length - 1);
                        break;

                    case 2:
                        LastName = text.Remove(text.Length - 1);
                        break;

                    case 3:
                        Username = text.Remove(text.Length - 1);
                        break;

                    case 4:
                        Password = text.Remove(text.Length - 1);
                        break;

                    case 5:
                        ConfirmPassword = text.Remove(text.Length - 1);
                        break;

                    case 6:
                        Email = text.Remove(text.Length - 1);
                        break;

                    default: break;
                }
            }
        }

        // Check for symbols and numbers function
        private void CheckSymbolsAndNumbers(string text, int choice)
        {
            bool result = Regex.IsMatch(text, "^[a-zA-Z]+$");

            switch (choice)
            {
                case 1:
                    if (result == false)
                    {
                        FirstNameAlert = "First name shouldn't have numbers or symbols";
                    }
                    else
                    {
                        FirstNameAlert = "";
                    }
                    break;

                case 2:
                    if (result == false)
                    {
                        LastNameAlert = "Last name shouldn't have numbers or symbols";
                    }
                    else
                    {
                        LastNameAlert = "";
                    }
                    break;

                default: break;
            }
        }

        // Username validation
        private bool UsernameValidation()
        {
            bool result = Regex.IsMatch(Username, @"^[A-z][A-z|\.|\s]+$");

            if (result == false)
            {
                UsernameAlert = "Invalid Username";
                return false;
            }
            else
            {
                UsernameAlert = "";
                return true;
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

            // Check First name parameter
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrEmpty(FirstName)) // empty box
            {
                FirstNameAlert = "First name is required";
                flag = true;
            }
            else if (FirstName.Length >= 26) // max letter length
            {
                FirstNameAlert = "First name should be less than 26 characters";
                flag = true;
            }
            else if (FirstName.Length < 2) // min letter length
            {
                FirstNameAlert = "First name should be more than 2 characters";
                flag = true;
            }
            else
            {
                FirstNameAlert = "";
            }

            // Check Last name parameter
            if (string.IsNullOrWhiteSpace(LastName) || string.IsNullOrEmpty(LastName)) // empty box
            {
                LastNameAlert = "Last name is required";
                flag = true;
            }
            else if (LastName.Length >= 26) // max letter length
            {
                LastNameAlert = "Last name should be less than 26 characters";
                flag = true;
            }
            else if (LastName.Length < 2) // min letter length
            {
                LastNameAlert = "Last name should be more than 2 characters";
                flag = true;
            }
            else
            {
                LastNameAlert = "";
            }

            // Check Username parameter
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrEmpty(Username)) // empty box
            {
                UsernameAlert = "Username is required";
                flag = true;
            }
            else if (Username.Length >= 30) // max letter length
            {
                UsernameAlert = "Username should be less than 30 characters";
                flag = true;
            }
            else if (Username.Length < 6) // min letter length
            {
                UsernameAlert = "Userame should be more than 6 characters";
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
            else if (Password.Length >= 128) // max letter length
            {
                PasswordAlert = "Password should be less than 128 characters";
                flag = true;
            }
            else if (Password.Length < 12) // min letter length
            {
                PasswordAlert = "Password should be more than 12 characters";
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

    }
}
