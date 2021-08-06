using BDProject.Views._PopUps;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BDProject.ViewModels.SettingsViewModels
{
    public class ColorsPageViewModel : BaseViewModel
    {
        public ColorsPageViewModel()
        {
            ColorWidth = App.Current.MainPage.Width - 32;

            IconColor = (Color)Application.Current.Resources["IconsColor"];
            TextColor = (Color)Application.Current.Resources["TextColor"];
            BackgroundColor = (Color)Application.Current.Resources["BackgroundColor"];

            // Assigning functions to the commands
            BackCommand = new Command(async () => await BackFunction());
            SetIconsCommand = new Command(SetIconsFunction);
            SetTextCommand = new Command(SetTextFunction);
            SetBackgroundCommand = new Command(SetBackgroundFunction);
            ResetCommand = new Command(ResetFunction);
            SaveChangesCommand = new Command(SaveChangesFunction);
        }

        // Parameters
        // Selected color parameter
        public Color SelectedColor = Color.Black;
        public double ColorWidth = 0;

        // Icon color
        public Color iconColor = Color.Black;
        public Color IconColor 
        {
            get => iconColor;
            set
            {
                if (value == iconColor) { return; }
                iconColor = value;
                OnPropertyChanged();
            }
        }

        // Text color
        public Color textColor = Color.Black;
        public Color TextColor
        {
            get => textColor;
            set
            {
                if (value == textColor) { return; }
                textColor = value;
                OnPropertyChanged();
            }
        }

        // Background color
        public Color backgroundColor = Color.White;
        public Color BackgroundColor
        {
            get => backgroundColor;
            set
            {
                if (value == backgroundColor) { return; }
                backgroundColor = value;
                OnPropertyChanged();
            }
        }

        // Commands
        // Back to post command
        public ICommand BackCommand { get; set; }
        private async Task BackFunction()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        // Set Icons Command
        public ICommand SetIconsCommand { get; set; }
        private async void SetIconsFunction()
        {
            await PopupNavigation.Instance.PushAsync(new ColorPickerPopUp(this, 1));
        }

        // Set Text Command
        public ICommand SetTextCommand { get; set; }
        private async void SetTextFunction()
        {
            await PopupNavigation.Instance.PushAsync(new ColorPickerPopUp(this, 2));
        }

        // Set Background Command
        public ICommand SetBackgroundCommand { get; set; }
        private async void SetBackgroundFunction()
        {
            await PopupNavigation.Instance.PushAsync(new ColorPickerPopUp(this, 3));
        }

        // Save Changes Command
        public ICommand ResetCommand { get; set; }
        private void ResetFunction()
        {
            Preferences.Set("IconsColor", Color.Black.ToHex());
            Preferences.Set("TextColor", Color.Black.ToHex());
            Preferences.Set("BackgroundColor", Color.White.ToHex());

            IconColor = Color.Black;
            TextColor = Color.Black;
            BackgroundColor = Color.White;
        }

        // Save Changes Command
        public ICommand SaveChangesCommand { get; set; }
        private async void SaveChangesFunction()
        {
            Preferences.Set("IconsColor", IconColor.ToHex());
            Preferences.Set("TextColor", TextColor.ToHex());
            Preferences.Set("BackgroundColor", BackgroundColor.ToHex());

            Application.Current.Resources["IconsColor"] = IconColor;
            Application.Current.Resources["TextColor"] = TextColor;
            Application.Current.Resources["BackgroundColor"] = BackgroundColor;

            await Shell.Current.Navigation.PopAsync();
        }

    }
}
