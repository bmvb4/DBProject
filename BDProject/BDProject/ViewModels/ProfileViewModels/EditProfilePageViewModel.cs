using BDProject.Models;
using BDProject.ModelWrappers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
            UserWrapper user = _Globals.GlobalMainUser;

            try
            {
                FName = user.FirstName;
                LName = user.LastName;
                Description = user.Description;
                ProfilePictureSource = user.PhotoSource;
            }
            catch (Exception ex)
            {

            }
        }

        public EditProfilePageViewModel()
        {
            //========TEST=======
            SetUserData();
            //========TEST=======

            // Assigning functions to the commands
            BackCommand = new Command(async () => await BackFunction());
            TakePhotoCommand = new Command(async () => await TakePhotoFunction());
            PickPhotoCommand = new Command(async () => await PickPhotoFunction());
            SaveChangesCommand = new Command(async () => await SaveChangesFunction());
        }

        // Parameters
        // profile image source parameter
        private string base64ImageRepresentation = "";
        private ImageSource profilePictureSource;
        public ImageSource ProfilePictureSource
        {
            get => profilePictureSource;
            set
            {
                if (value == profilePictureSource) { return; }
                profilePictureSource = value;
                OnPropertyChanged(nameof(ProfilePictureSource));
            }
        }

        // Your first Name parameter
        private string fname = "";
        public string FName
        {
            get => fname;
            set
            {
                if (value == fname) { return; }
                fname = value;
                OnPropertyChanged(nameof(FName));
            }
        }

        // Your last Name parameter
        private string lname = "";
        public string LName
        {
            get => lname;
            set
            {
                if (value == lname) { return; }
                lname = value;
                OnPropertyChanged(nameof(LName));
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
                OnPropertyChanged(nameof(Description));
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
            base64ImageRepresentation = "";
            ProfilePictureSource = null;

            var result = await MediaPicker.CapturePhotoAsync();
            if (result == null) { return; }

            // image path
            var path = Path.Combine(FileSystem.CacheDirectory, result.FileName);

            //byte[] imageBytes = File.ReadAllBytes(path);
            //base64ImageRepresentation = Convert.ToBase64String(imageBytes);

            ProfilePictureSource = ImageSource.FromFile(path);
            isChanged = true;
        }

        // Pick Photo command
        public ICommand PickPhotoCommand { get; set; }
        private async Task PickPhotoFunction()
        {
            base64ImageRepresentation = "";
            ProfilePictureSource = null;

            var result = await MediaPicker.PickPhotoAsync();
            if (result == null) { return; }

            byte[] imageBytes = File.ReadAllBytes(result.FullPath);
            base64ImageRepresentation = Convert.ToBase64String(imageBytes);

            ProfilePictureSource = ImageSource.FromFile(result.FullPath);
            isChanged = true;
        }

        // SaveChangesCommand
        public ICommand SaveChangesCommand { get; set; }
        private async Task SaveChangesFunction()
        {
            // save photo
            if (isChanged == true)
            {
                _Globals.GlobalMainUser.Base64Photo = base64ImageRepresentation;
            }

            // save first name
            if (_Globals.GlobalMainUser.FirstName != FName) 
            { 
                _Globals.GlobalMainUser.FirstName = FName; 
            }

            // save last name
            if(_Globals.GlobalMainUser.LastName != LName)
            {
                _Globals.GlobalMainUser.LastName = LName;
            }

            if (_Globals.GlobalMainUser.Description != Description)
            {
                _Globals.GlobalMainUser.Description = Description;
            }
            UserWrapper usr = _Globals.GlobalMainUser;
            string URL = "https://10.0.2.2:5001/profile/update/" + usr.Username;
            const string sContentType = "application/json";

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient _client = new HttpClient(clientHandler);

            JObject oJsonObject = new JObject();
            oJsonObject.Add("Username", usr.Username);
            oJsonObject.Add("Photo", Convert.FromBase64String(base64ImageRepresentation));
            oJsonObject.Add("Description", usr.Description);
            oJsonObject.Add("FirstName", usr.FirstName);
            oJsonObject.Add("LastName", usr.LastName);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usr.Token);
            var postResult = await _client.PutAsync(URL, new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType));
            if (postResult.IsSuccessStatusCode)
            {
                var earthquakesJson = postResult.Content.ReadAsStringAsync().Result;
                var rootobject = JsonConvert.DeserializeObject<Post>(earthquakesJson);

            }
            else
            {
            }
            await Shell.Current.Navigation.PopAsync();
        }

    }
}
