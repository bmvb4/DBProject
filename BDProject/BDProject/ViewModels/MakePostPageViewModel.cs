using BDProject.Models;
using BDProject.ModelWrappers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
        private string base64ImageRepresentation = "";
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

            base64ImageRepresentation = "";
            TakenPhoto = null;
        }

        // Take Photo command
        public ICommand TakePhotoCommand { get; set; }
        private async Task TakePhotoFunction()
        {
            base64ImageRepresentation = "";
            TakenPhoto = null;

            var result = await MediaPicker.CapturePhotoAsync();
            if (result == null) { return; }

            // image path
            var path = Path.Combine(FileSystem.CacheDirectory, result.FileName);

            //byte[] imageBytes = File.ReadAllBytes(path);
            //base64ImageRepresentation = Convert.ToBase64String(imageBytes);

            TakenPhoto = ImageSource.FromFile(path);
        }

        // Pick Photo command
        public ICommand PickPhotoCommand { get; set; }
        private async Task PickPhotoFunction()
        {
            base64ImageRepresentation = "";
            TakenPhoto = null;

            var result = await MediaPicker.PickPhotoAsync();
            if (result == null) { return; }

            byte[] imageBytes = File.ReadAllBytes(result.FullPath);
            base64ImageRepresentation = Convert.ToBase64String(imageBytes);

            //============TEST
            //_Globals.GlobalMainUser = new UserWrapper(new User()
            //{
            //    FirstName = "Daniel",
            //    LastName = "Kostov",
            //    Username = "DanielRK",
            //    Password = "",
            //    Email = "",
            //    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
            //    Photo = base64ImageRepresentation
            //});
            //============TEST

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
            const string URL = "https://10.0.2.2:5001/posts/image/post";
            const string sContentType = "application/json";

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient _client = new HttpClient(clientHandler);

            JObject oJsonObject = new JObject();
            UserWrapper usr = _Globals.GlobalMainUser;
            oJsonObject.Add("IdUser", usr.Username);
            oJsonObject.Add("Photo", Convert.FromBase64String(base64ImageRepresentation));
            oJsonObject.Add("Description", Description);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usr.Token);
            var postResult = await _client.PostAsync(URL, new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType));
            if (postResult.IsSuccessStatusCode)
            {
                var earthquakesJson = postResult.Content.ReadAsStringAsync().Result;
                var rootobject = JsonConvert.DeserializeObject<Post>(earthquakesJson);

            }
            else
            {
            }

            //=================TEST
            _Globals.AddPost(new PostWrapper(new Post()
            {
                Photo = Convert.FromBase64String(base64ImageRepresentation),
                Description = "Test description 1"
            })
            {
                Username = "Stranger1",
                Base64UserPhoto = _Globals.GlobalMainUser.Base64Photo
            });
            _Globals.AddPost(new PostWrapper(new Post()
            {
                Photo = Convert.FromBase64String(base64ImageRepresentation),
                Description = "Test description 2"
            })
            {
                Username = "Stranger2",
                Base64UserPhoto = _Globals.GlobalMainUser.Base64Photo
            });
            //=================TEST

            _Globals.GlobalMainUser.AddPost(new PostWrapper(new Post()
            {
                Photo = Convert.FromBase64String(base64ImageRepresentation),
                Description = description
            })
            {
                Username = _Globals.GlobalMainUser.Username,
                Base64UserPhoto = _Globals.GlobalMainUser.Base64Photo
            });

            await Shell.Current.GoToAsync("//HomePage");

            base64ImageRepresentation = "";
            TakenPhoto = null;
        }

    }
}
