using BDProject.Helpers;
using Newtonsoft.Json.Linq;
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
            if (string.IsNullOrEmpty(Username))
            {
                UsernameAlert = "Username is required";
                return;
            }

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
            if (text.Length >= restriction) Username = text.Remove(text.Length - 1);
        }

        // Clear everything
        private void ClearEverything()
        {
            Username = "";
            UsernameAlert = "";
        }

    }
}
