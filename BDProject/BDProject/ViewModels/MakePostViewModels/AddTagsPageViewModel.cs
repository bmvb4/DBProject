using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels.MakePostViewModels
{
    public class AddTagsPageViewModel : BaseViewModel
    {

        public AddTagsPageViewModel()
        {
            // Assigning functions to the commands
            BackCommand = new Command(async () => await BackFunction());
        }

        // Commands
        // Back to post command
        public ICommand BackCommand { get; set; }
        private async Task BackFunction()
        {
            await Shell.Current.Navigation.PopAsync();
        }

    }
}
