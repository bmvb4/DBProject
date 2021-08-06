using BDProject.Helpers;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BDProject.ViewModels.ProfileViewModels
{
    public class EditProfilePageViewModel : BaseViewModel
    {

        private void SetUserData()
        {
            FirstName = _Globals.GlobalMainUser.FirstName;
            LastName = _Globals.GlobalMainUser.LastName;
            Description = _Globals.GlobalMainUser.Description;
            ProfilePictureSource = _Globals.GlobalMainUser.PhotoSource;
        }

        public EditProfilePageViewModel()
        {
            SetUserData();

            // Assigning functions to the commands
            BackCommand = new Command(async () => await BackFunction());
            TakePhotoCommand = new Command(async () => await TakePhotoFunction());
            PickPhotoCommand = new Command(async () => await PickPhotoFunction());
            SaveChangesCommand = new Command(async () => await SaveChangesFunction());
        }

        // Parameters
        // profile image source parameter
        private byte[] imageBytes;
        private ImageSource profilePictureSource;
        public ImageSource ProfilePictureSource
        {
            get => profilePictureSource;
            set
            {
                if (value == profilePictureSource) { return; }
                profilePictureSource = value;
                OnPropertyChanged();
            }
        }

        // Your first Name parameter
        private string firstname = "";
        public string FirstName
        {
            get => firstname;
            set
            {
                if (value == firstname) { return; }
                firstname = value;
                OnPropertyChanged();
            }
        }

        // Your last Name parameter
        private string lastname = "";
        public string LastName
        {
            get => lastname;
            set
            {
                if (value == lastname) { return; }
                lastname = value;
                OnPropertyChanged();
            }
        }

        // Your Description parameter
        private string description = "";
        public string Description
        {
            get => description;
            set
            {
                if (value == description) { return; }
                description = value;
                OnPropertyChanged();
            }
        }

        // Commands PostHeight
        // Back to post command
        public ICommand BackCommand { get; set; }
        private async Task BackFunction()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        // Take Photo command
        private bool isChanged = false;
        public ICommand TakePhotoCommand { get; set; }
        private async Task TakePhotoFunction()
        {
            ProfilePictureSource = null;

            var result = await MediaPicker.CapturePhotoAsync();
            if (result == null) { return; }

            imageBytes = File.ReadAllBytes(result.FullPath);

            ProfilePictureSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
            isChanged = true;
        }

        // Pick Photo command
        public ICommand PickPhotoCommand { get; set; }
        private async Task PickPhotoFunction()
        {
            ProfilePictureSource = null;

            var result = await MediaPicker.PickPhotoAsync();
            if (result == null) { return; }

            imageBytes = File.ReadAllBytes(result.FullPath);

            ProfilePictureSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
            isChanged = true;
        }

        // SaveChangesCommand
        public ICommand SaveChangesCommand { get; set; }
        private async Task SaveChangesFunction()
        {
            // save photo
            if (isChanged == true)
            {
                _Globals.GlobalMainUser.Photo = imageBytes;
            }

            // save first name
            if (_Globals.GlobalMainUser.FirstName != FirstName) 
            { 
                _Globals.GlobalMainUser.FirstName = FirstName; 
            }

            // save last name
            if(_Globals.GlobalMainUser.LastName != LastName)
            {
                _Globals.GlobalMainUser.LastName = LastName;
            }

            // save description
            if (_Globals.GlobalMainUser.Description != Description)
            {
                _Globals.GlobalMainUser.Description = Description;
            }

            _Globals.Refresh = true;

            JObject oJsonObject = new JObject();
            oJsonObject.Add("Username", _Globals.GlobalMainUser.Username);
            oJsonObject.Add("Photo", _Globals.GlobalMainUser.Photo);
            oJsonObject.Add("Description", _Globals.GlobalMainUser.Description);
            oJsonObject.Add("FirstName", _Globals.GlobalMainUser.FirstName);
            oJsonObject.Add("LastName", _Globals.GlobalMainUser.LastName);

            var success = await ServerServices.SendPutRequestAsync($"profile/{_Globals.GlobalMainUser.Username}", oJsonObject);

            if (success.IsSuccessStatusCode)
            {
                await Shell.Current.Navigation.PopAsync();
            }
            else if (success.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ServerServices.RefreshTokenAsync();
                await SaveChangesFunction();
            }
        }

    }
}
