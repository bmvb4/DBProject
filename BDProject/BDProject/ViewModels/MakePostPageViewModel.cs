using BDProject.Helpers;
using BDProject.Models;
using BDProject.ModelWrappers;
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

            //============TEST
            _Globals.GlobalMainUser = new UserWrapper(new User()
            {
                FirstName = "Daniel",
                LastName = "Kostov",
                Username = "DanielRK",
                Password = "",
                Email = "",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                Photo = imageBytes
            });
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

            //=================TEST
            UserWrapper temp1 = new UserWrapper(new User()
            {
                FirstName = "Some",
                LastName = "Stranger",
                Username = "Stranger1",
                Password = "",
                Email = "",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                Photo = imageBytes
            });
            PostWrapper temp1post = new PostWrapper(new Post(imageBytes, "Test description 1"), temp1.Username, _Globals.GlobalMainUser.ImageBytes);
            temp1.AddPost(temp1post);
            temp1post.AddComment(new CommentWrapper(imageBytes, temp1.Username, "Test message 1"));
            temp1post.AddComment(new CommentWrapper(imageBytes, _Globals.GlobalMainUser.Username, "my test message"));
            temp1post.AddComment(new CommentWrapper(imageBytes, temp1.Username, "Test message 2"));
            temp1post.AddComment(new CommentWrapper(imageBytes, temp1.Username, "Test message 3"));
            temp1post.AddComment(new CommentWrapper(imageBytes, temp1.Username, "Test message 4"));
            temp1post.AddComment(new CommentWrapper(imageBytes, temp1.Username, "Test message 5"));
            temp1post.AddComment(new CommentWrapper(imageBytes, temp1.Username, "Test message 6"));
            temp1post.AddComment(new CommentWrapper(imageBytes, temp1.Username, "Test message 7"));

            UserWrapper temp2 = new UserWrapper(new User()
            {
                FirstName = "Other",
                LastName = "Stranger",
                Username = "Stranger2",
                Password = "",
                Email = "",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                Photo = imageBytes
            });
            PostWrapper temp2post = new PostWrapper(new Post(imageBytes, "Test description 1"), temp2.Username, _Globals.GlobalMainUser.ImageBytes);
            temp2.AddPost(temp2post);

            _Globals.AddUser(temp1);
            _Globals.AddUser(temp2);
            _Globals.AddPost(temp1post);
            _Globals.AddPost(temp2post);
            //=================TEST

            _Globals.AddMyPost(new PostWrapper(new Post(imageBytes, Description), _Globals.GlobalMainUser.Username, _Globals.GlobalMainUser.ImageBytes));
            _Globals.GlobalMainUser.AddPost(new PostWrapper(new Post(imageBytes, Description), _Globals.GlobalMainUser.Username, _Globals.GlobalMainUser.ImageBytes));

            _Globals.Refresh = true;
            await Shell.Current.GoToAsync("//HomePage");

            /*
            JObject oJsonObject = new JObject();
            oJsonObject.Add("IdUser", _Globals.GlobalMainUser.Username);
            oJsonObject.Add("Photo", imageBytes);
            oJsonObject.Add("Description", Description);

            bool success = await ServerServices.SendRequestAsync("posts/image/post", oJsonObject);

            if (success)
            {
                await Shell.Current.GoToAsync("//HomePage");
            }
            */

            TakenPhoto = null;
            Description = "";
        }

    }
}
