﻿using BDProject.Helpers;
using BDProject.ModelWrappers;
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

        // log out profile command LogOutCommand
        public ICommand LogOutCommand { get; set; }
        private async Task LogOutFunction()
        {
            bool result = await App.Current.MainPage.DisplayAlert("Warning", "Do you want to log out?", "Yes", "No");

            if (result == false) { return; }

            Preferences.Remove("UsernameKey");
            Preferences.Remove("PasswordKey");

            _Globals.OpenID = 0;

            _Globals.GlobalFeedPosts = new List<PostWrapper>();

            _Globals.GlobalMainUser.Followings = new List<string>();
            _Globals.GlobalMainUser.MyPosts = new List<PostWrapper>();
            _Globals.GlobalMainUser = new UserWrapper();

            await Shell.Current.Navigation.PopAsync();
            Shell.Current.CurrentItem = new SplashScreenPage();
        }

        // delete profile command
        public ICommand DeleteAccountCommand { get; set; }
        private async Task DeleteAccountFunction()
        {
            bool result = await App.Current.MainPage.DisplayAlert("Warning", "If you delete your account you will lose all of your data", "Yes", "No");

            if (result == false) { return; }

            _Globals.OpenID = 0;

            _Globals.GlobalFeedPosts = new List<PostWrapper>();

            _Globals.GlobalMainUser.Followings = new List<string>();
            _Globals.GlobalMainUser.MyPosts = new List<PostWrapper>();
            _Globals.GlobalMainUser = new UserWrapper();

            await Shell.Current.Navigation.PopAsync();
            await Shell.Current.GoToAsync("//LogInPage");
        }

    }
}
