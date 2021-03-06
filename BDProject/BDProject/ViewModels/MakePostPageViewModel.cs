using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
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
            await Shell.Current.GoToAsync("//ProfilePage");
        }

        // Take Photo command
        public ICommand TakePhotoCommand { get; set; }
        private async void TakePhotoFunction()
        {
            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

            if (photo != null)
            {
                TakenPhoto = ImageSource.FromStream(() => { return photo.GetStream(); });
            }
        }

    }
}
