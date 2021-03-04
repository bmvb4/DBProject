using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels
{
    public class ProfilePageViewModel : BaseViewModel
    {

        public ProfilePageViewModel()
        {

            // Assigning functions to the commands
            OpenSettingsCommand = new Command(OpenSettingsFunction);
        }

        // Commands
        // Back to post command
        public ICommand OpenSettingsCommand { get; set; }
        private async void OpenSettingsFunction(object user)
        {
            await Shell.Current.GoToAsync("//HomePage");
        }

    }
}
