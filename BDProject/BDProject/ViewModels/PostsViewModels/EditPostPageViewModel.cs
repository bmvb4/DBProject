using BDProject.Helpers;
using BDProject.ModelWrappers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels.PostsViewModels
{
    public class EditPostPageViewModel : BaseViewModel
    {

        private void SetParameters()
        {
            SelectedPost = _Globals.GetPost(_Globals.OpenID);

            PhotoSource = SelectedPost.PhotoSource;
            UserPhotoSource = SelectedPost.UserPhotoSource;
            Username = SelectedPost.Username;
            Description = SelectedPost.Description;

            _Globals.OpenID = 0;
        }

        public EditPostPageViewModel()
        {
            SetParameters();

            // Assigning functions to the commands
            BackCommand = new Command(async () => await BackFunction());
            SaveChangesCommand = new Command(async () => await SaveChangesFunction());
        }

        // Parameters
        // editet post
        PostWrapper SelectedPost = new PostWrapper();

        // post Image parameter
        private ImageSource photoSource = null;
        public ImageSource PhotoSource
        {
            get => photoSource;
            set
            {
                if (value == photoSource) { return; }
                photoSource = value;
                OnPropertyChanged(nameof(PhotoSource));
            }
        }

        // User Image parameter
        private ImageSource userPhotoSource = null;
        public ImageSource UserPhotoSource
        {
            get => userPhotoSource;
            set
            {
                if (value == userPhotoSource) { return; }
                userPhotoSource = value;
                OnPropertyChanged(nameof(UserPhotoSource));
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

        // Username parameter
        private string username = "";
        public string Username
        {
            get => username;
            set
            {
                if (value == username) { return; }
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        // Commands
        // Back to post command
        public ICommand BackCommand { get; set; }
        private async Task BackFunction()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        // save changes command
        public ICommand SaveChangesCommand { get; set; }
        private async Task SaveChangesFunction()
        {
            SelectedPost.Description = Description;

            _Globals.GlobalMainUser.EditPost(SelectedPost);
            _Globals.EditPost(SelectedPost);

            _Globals.Refresh = true;
            await Shell.Current.Navigation.PopAsync();

            PhotoSource = null;
        }

    }
}
