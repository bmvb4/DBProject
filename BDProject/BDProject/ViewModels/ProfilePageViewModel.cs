using BDProject.Models;
using BDProject.ModelWrappers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels
{
    public class ProfilePageViewModel : BaseViewModel
    {
        private void SetUserData()
        {
            UserWrapper user = _Globals.GetMainUser();

            try
            {
                Name = user.FirstName + " " + user.LastName;
                Description = user.Description;
                ProfilePictureSource = user.PhotoSource;
            }
            catch(Exception ex)
            {
                
            }
        }

        private void SetCollection()
        {
            YourPostsCollection.Clear();
            YourPostsCollection = new ObservableCollection<PostWrapper>(_Globals.GetPosts());

            if (YourPostsCollection.Count != 0)
            {
                this.SetCollectionHeight();
            }
        }

        public ProfilePageViewModel()
        {
            //========TEST=======
            SetUserData();
            SetCollection();
            //========TEST=======

            // Assigning functions to the commands
            OpenSettingsCommand = new Command(async () => await OpenSettingsFunction());
            RefreshCommand = new Command(async () => await RefreshFunction());
        }

        // Parameters
        // Your Posts Collection parameter
        private ObservableCollection<PostWrapper> yourPostsCollection = new ObservableCollection<PostWrapper>();
        public ObservableCollection<PostWrapper> YourPostsCollection
        {
            get => yourPostsCollection;
            set
            {
                if (value == yourPostsCollection) { return; }
                yourPostsCollection = value;
                OnPropertyChanged(nameof(YourPostsCollection));
            }
        }

        // Your Posts Collection height parameter
        private double collectionHeight = 0;
        public double CollectionHeight
        {
            get => collectionHeight;
            set
            {
                if (value == collectionHeight) { return; }
                collectionHeight = value;
                OnPropertyChanged(nameof(CollectionHeight));
            }
        }

        // Your Posts height parameter
        private double postHeight = 0;
        public double PostHeight
        {
            get => postHeight;
            set
            {
                if (value == postHeight) { return; }
                postHeight = value;
                OnPropertyChanged(nameof(PostHeight));
            }
        }

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

        // Your Name parameter
        private string name = "";
        public string Name
        {
            get => name;
            set
            {
                if (value == name) { return; }
                name = value;
                OnPropertyChanged(nameof(Name));
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

        // Your followers count parameter
        private int followersCount = 0;
        public int FollowersCount
        {
            get => followersCount;
            set
            {
                if (value == followersCount) { return; }
                followersCount = value;
                OnPropertyChanged(nameof(FollowersCount));
            }
        }

        // Your following count parameter
        private int followingCount = 0;
        public int FollowingCount
        {
            get => followingCount;
            set
            {
                if (value == followingCount) { return; }
                followingCount = value;
                OnPropertyChanged(nameof(FollowingCount));
            }
        }

        // Refreshing parameter
        private bool isRefreshing = false;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                //if (value == isRefreshing) { return; }
                isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        // Commands PostHeight
        // Back to post command
        public ICommand OpenSettingsCommand { get; set; }
        private async Task OpenSettingsFunction()
        {
            await Shell.Current.GoToAsync("SettingsPage");
        }

        // Refresh collection view command
        public ICommand RefreshCommand { get; set; }
        private async Task RefreshFunction()
        {
            IsRefreshing = true;
            await Task.Delay(TimeSpan.FromSeconds(1));
            SetCollection();
            IsRefreshing = false;
        }

        // Functions
        // Set Collceton height function
        private void SetCollectionHeight()
        {
            PostHeight = App.Current.MainPage.Width * 0.365;

            if (YourPostsCollection.Count % 3 == 0)
            {
                CollectionHeight = PostHeight * (YourPostsCollection.Count / 3) + 16;
            }
            else
            {
                CollectionHeight = PostHeight * (Math.Ceiling(((double)YourPostsCollection.Count) / 3.0)) + 16;
            }
        }

    }
}
