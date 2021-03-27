using BDProject.Helpers;
using BDProject.ModelWrappers;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels
{
    public class ProfilePageViewModel : BaseViewModel
    {
        public void SetUserData()
        {
            UserWrapper user = _Globals.GlobalMainUser;

            try
            {
                Name = user.FirstName + " " + user.LastName;
                Username = "(" + user.Username + ")";
                Description = user.Description;
                ProfilePictureSource = user.PhotoSource;

                FollowingCount = _Globals.GlobalMainUser.Followings.Count;
                FollowersCount = _Globals.GlobalMainUser.Followers.Count;
            }
            catch(Exception ex)
            {
                
            }
        }

        public void SetCollection()
        {
            PostsCount = 0;
            YourPostsCollection.Clear();
            YourPostsCollection = new ObservableCollection<PostWrapper>(_Globals.GlobalMainUser.MyPosts);

            if (YourPostsCollection.Count != 0)
            {
                this.SetCollectionHeight();
                PostsCount = YourPostsCollection.Count;
            }
        }

        public ProfilePageViewModel()
        {
            SetUserData();
            SetCollection();

            // Assigning functions to the commands
            OpenSettingsCommand = new Command(async () => await OpenSettingsFunction());
            OpenEditProfileCommand=new Command(async () => await OpenEditProfileFunction());
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

        // Your username parameter
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

        // Your following count parameter
        private int postsCount = 0;
        public int PostsCount
        {
            get => postsCount;
            set
            {
                if (value == postsCount) { return; }
                postsCount = value;
                OnPropertyChanged(nameof(PostsCount));
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
        // Settings command
        public ICommand OpenSettingsCommand { get; set; }
        private async Task OpenSettingsFunction()
        {
            await Shell.Current.GoToAsync("SettingsPage");
        }

        // Edit profile command
        public ICommand OpenEditProfileCommand { get; set; }
        private async Task OpenEditProfileFunction()
        {
            await Shell.Current.GoToAsync("EditProfilePage");
        }

        // Refresh collection view command
        public ICommand RefreshCommand { get; set; }
        private async Task RefreshFunction()
        {
            IsRefreshing = true;
            await Task.Delay(TimeSpan.FromSeconds(1));
            SetUserData();
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
