using BDProject.Helpers;
using BDProject.ModelWrappers;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels.SettingsViewModels
{
    public class ManageAccountPageViewModel : BaseViewModel
    {
        private void SetUserData()
        {
            UserWrapper user = _Globals.GlobalMainUser;

            try
            {
                Username = user.Username;
                Email = user.Email;
            }
            catch (Exception ex)
            {

            }
        }

        public ManageAccountPageViewModel()
        {
            SetUserData();

            // Assigning functions to the commands
            BackCommand = new Command(async () => await BackFunction());
            SaveChangesCommand = new Command(async () => await SaveChangesFunction());
        }

        // Parameters
        // Your Username parameter
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

        // Your last Name parameter
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
        // Back to post command
        public ICommand BackCommand { get; set; }
        private async Task BackFunction()
        {
            await Shell.Current.Navigation.PopAsync();
        }
        
        // SaveChangesCommand
        public ICommand SaveChangesCommand { get; set; }
        private async Task SaveChangesFunction()
        {

            // save username
            if (_Globals.GlobalMainUser.Username != Username)
            {
                _Globals.GlobalMainUser.Username = Username;
            }

            // save last name
            if (_Globals.GlobalMainUser.Email != Email)
            {
                _Globals.GlobalMainUser.Email = Email;
            }

            _Globals.Refresh = true;
            await Shell.Current.Navigation.PopAsync();

            /*
            JObject oJsonObject = new JObject();
            oJsonObject.Add("Username", _Globals.GlobalMainUser.Username);
            oJsonObject.Add("Photo", _Globals.GlobalMainUser.ImageBytes);
            oJsonObject.Add("Description", _Globals.GlobalMainUser.Description);
            oJsonObject.Add("FirstName", _Globals.GlobalMainUser.FirstName);
            oJsonObject.Add("LastName", _Globals.GlobalMainUser.LastName);

            bool success = await ServerServices.SendRequestAsync("profile/update/", oJsonObject);

            if (success)
            {
                await Shell.Current.Navigation.PopAsync();
            }
            */
        }

    }
}
