using BDProject.Models;
using BDProject.ModelWrappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels.PostsViewModels
{
    [QueryProperty(nameof(ID),"PostID")]
    public class EditPostPageViewModel : BaseViewModel
    {
        private void SetParameters()
        {
            PhotoSource = SelectedPost.PhotoSource;
            UserPhotoSource = SelectedPost.UserPhotoSource;
            Username = SelectedPost.Username;
            Description = SelectedPost.Description;
        }

        public EditPostPageViewModel()
        {
            // Assigning functions to the commands
            BackCommand = new Command(async () => await BackFunction());
            SaveChangesCommand = new Command(async () => await SaveChangesFunction());
        }

        // Parameters
        // post parameter
        private PostWrapper SelectedPost;
        public int ID
        {
            set
            {
                SelectedPost = _Globals.GlobalMainUser.GetPost(value);
                SetParameters();
            }
        }


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

            await Shell.Current.Navigation.PopAsync();

            PhotoSource = null;
        }

    }
}
