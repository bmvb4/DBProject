using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels
{
    public class VerifyProfilePageViewModel: BaseViewModel
    {

        public VerifyProfilePageViewModel()
        {

            BackCommand = new Command(async () => await BackFunction());
            VerifyCommand = new Command(VerifyFunction);
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

        // Commands
        // Back to post command
        public ICommand BackCommand { get; set; }
        private async Task BackFunction()
        {
            await Shell.Current.GoToAsync("//SignUpPage");
            ClearEverything();
        }

        // Open Posts command
        public ICommand VerifyCommand { get; set; }
        private async void VerifyFunction()
        {
            // Reset password
            await Shell.Current.GoToAsync("//HomePage");

            ClearEverything();
        }

        // Functions
        // Clear everything
        private void ClearEverything()
        {
            Code = "";
            CodeAlert = "";
        }
    }
}
