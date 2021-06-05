using BDProject.Helpers;
using BDProject.Models;
using BDProject.Views;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BDProject.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {

        public SettingsPageViewModel()
        {

            // Assigning functions to the commands
            BackCommand = new Command(async () => await BackFunction());
            OpenManageAccountCommand = new Command(async () => await OpenManageAccountFunction());
            OpenColorsCommand = new Command(async () => await OpenColorsFunction());

            // red labels
            DeleteAccountCommand = new Command(async () => await DeleteAccountFunction());
            LogOutCommand = new Command(async () => await LogOutFunction());
        }

        // Commands
        // Back to post command
        public ICommand BackCommand { get; set; }
        private async Task BackFunction()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        // Edit profile command
        public ICommand OpenManageAccountCommand { get; set; }
        private async Task OpenManageAccountFunction()
        {
            await Shell.Current.GoToAsync("ManageAccountPage");
        }

        // Edit profile command
        public ICommand OpenColorsCommand { get; set; }
        private async Task OpenColorsFunction()
        {
            await Shell.Current.GoToAsync("ColorsPage");
        }

        // log out profile command LogOutCommand
        public ICommand LogOutCommand { get; set; }
        private async Task LogOutFunction()
        {
            bool result = await App.Current.MainPage.DisplayAlert("Warning", "Do you want to log out?", "Yes", "No");

            if (result == false) { return; }

            await ServerServices.SendPostRequestAsync("token/revoke", new Newtonsoft.Json.Linq.JObject());

            Preferences.Remove("UsernameKey");
            Preferences.Remove("PasswordKey");

            _Globals.IsLeving = true;

            _Globals.OpenID = 0;

            _Globals.GlobalFeedPosts = new List<Post>();
            _Globals.GlobalMainUser = new User();

            App.Current.MainPage = new AppShell();
        }

        // delete profile command
        public ICommand DeleteAccountCommand { get; set; }
        private async Task DeleteAccountFunction()
        {
            bool result = await App.Current.MainPage.DisplayAlert("Warning", "If you delete your account you will lose all of your data", "Yes", "No");

            if (result == false) { return; }

            await ServerServices.SendPostRequestAsync("token/revoke", new Newtonsoft.Json.Linq.JObject());

            _Globals.IsLeving = true;

            _Globals.OpenID = 0;

            _Globals.GlobalFeedPosts = new List<Post>();
            _Globals.GlobalMainUser = new User();

            App.Current.MainPage = new AppShell();
        }

    }
}
