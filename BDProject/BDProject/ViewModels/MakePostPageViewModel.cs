using BDProject.Helpers;
using BDProject.Models;
using BDProject.ModelWrappers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BDProject.ViewModels
{
    public class MakePostPageViewModel : BaseViewModel
    {

        public MakePostPageViewModel()
        {
            ImageHeight = App.Current.MainPage.Width;
            ImageWidth = App.Current.MainPage.Width / 2;

            // Assigning functions to the commands
            BackCommand = new Command(async () => await BackFunction());
            TakePhotoCommand = new Command(async () => await TakePhotoFunction());
            PickPhotoCommand = new Command(async () => await PickPhotoFunction());
            AddTagsCommand = new Command(async () => await AddTagsFunction());
            PostCommand = new Command(async () => await PostFunction());
        }

        // Parameters
        // Taken Image parameter
        private byte[] imageBytes;
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

        // Image width
        private double imageWidth = 0.0;
        public double ImageWidth
        {
            get => imageWidth;
            set
            {
                if (value == imageWidth) { return; }
                imageWidth = value;
                OnPropertyChanged(nameof(ImageWidth));
            }
        }

        // Image height
        private double imageHeight = 0.0;
        public double ImageHeight
        {
            get => imageHeight;
            set
            {
                if (value == imageHeight) { return; }
                imageHeight = value;
                OnPropertyChanged(nameof(ImageHeight));
            }
        }

        // Post description
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

        // Commands
        // Back to post command
        public ICommand BackCommand { get; set; }
        private async Task BackFunction()
        {
            await Shell.Current.GoToAsync("//HomePage");

            TakenPhoto = null;
            Description = "";
        }

        // Take Photo command
        public ICommand TakePhotoCommand { get; set; }
        private async Task TakePhotoFunction()
        {
            TakenPhoto = null;

            var result = await MediaPicker.CapturePhotoAsync();
            if (result == null) { return; }

            // image path
            var path = Path.Combine(FileSystem.CacheDirectory, result.FileName);

            imageBytes = File.ReadAllBytes(path);

            TakenPhoto = ImageSource.FromStream(() => new MemoryStream(imageBytes));
        }

        // Pick Photo command
        public ICommand PickPhotoCommand { get; set; }
        private async Task PickPhotoFunction()
        {
            TakenPhoto = null;

            var result = await MediaPicker.PickPhotoAsync();
            if (result == null) { return; }

            imageBytes = File.ReadAllBytes(result.FullPath);

            TakenPhoto = ImageSource.FromFile(result.FullPath);
        }

        // add tags command
        public ICommand AddTagsCommand { get; set; }
        private async Task AddTagsFunction()
        {
            await Shell.Current.GoToAsync("AddTagsPage");
        }

        // post command
        public ICommand PostCommand { get; set; }
        private async Task PostFunction()
        {
            if (TakenPhoto == null) 
            { 
                await App.Current.MainPage.DisplayAlert("Warning!", "You can't continue without a photo", "OK");
                return;
            }
            else
            {
                JObject oJsonObject = new JObject();
                oJsonObject.Add("IdUser", _Globals.GlobalMainUser.Username);
                oJsonObject.Add("Photo", imageBytes);
                oJsonObject.Add("Description", Description);

                var success = await ServerServices.SendPostRequestAsync("posts/post", oJsonObject);

                if (success.IsSuccessStatusCode)
                {
                    _Globals.GlobalMainUser.AddPost(new PostWrapper(imageBytes, Description, _Globals.GlobalMainUser.Username, _Globals.GlobalMainUser.ImageBytes));
                    _Globals.AddMyPost(new PostWrapper(imageBytes, Description, _Globals.GlobalMainUser.Username, _Globals.GlobalMainUser.ImageBytes));

                    _Globals.Refresh = true;
                    await Shell.Current.GoToAsync("//HomePage");
                }

                TakenPhoto = null;
                Description = "";
            }
        }

    }
}
