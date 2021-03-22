﻿using BDProject.Helpers;
using BDProject.ModelWrappers;
using System;
using System.IO;
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
        private byte[] imageBytes;
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
                _Globals.GlobalMainUser.ImageBytes = imageBytes;
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
