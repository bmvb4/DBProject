using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BDProject.ViewModels
{
    public class MakePostPageViewModel : BaseViewModel
    {

        public MakePostPageViewModel()
        {
            // Assigning functions to the commands
            BackCommand = new Command(BackFunction);
            TakePhotoCommand = new Command(TakePhotoFunction);
            PickPhotoCommand = new Command(PickPhotoFunction);
        }

        // Parameters
        // Taken Image parameter
        private ImageSource takenPhoto = null;
        public ImageSource TakenPhoto
        {
            get => takenPhoto;
            set
            {
                if (value == takenPhoto) { return; }
                takenPhoto = value;
                OnPropertyChanged(nameof(TakenPhoto));
            }
        }

        // Commands
        // Back to post command
        public ICommand BackCommand { get; set; }
        private async void BackFunction()
        {
            await Shell.Current.GoToAsync("//HomePage");
        }

        // Take Photo command
        public ICommand TakePhotoCommand { get; set; }
        private async void TakePhotoFunction()
        {
            var result = await MediaPicker.CapturePhotoAsync();

            if (result == null) { return; }

            var stream = await result.OpenReadAsync();

            TakenPhoto = ImageSource.FromStream(() => stream);
        }

        // Pick Photo command
        public ICommand PickPhotoCommand { get; set; }
        private async void PickPhotoFunction()
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Pick a photo"
            });

            if (result == null) { return; }

            var stream = await result.OpenReadAsync();

            TakenPhoto = ImageSource.FromStream(() => stream);
        }

    }
}
