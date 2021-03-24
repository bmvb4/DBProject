using BDProject.Helpers;
using BDProject.ModelWrappers;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels.ProfileViewModels
{
    public class PersonsProfilePageViewModel : BaseViewModel
    {

        private void SetUserData()
        {
            string uName = _Globals.GlobalFeedPosts[_Globals.OpenID].Username;
            UserWrapper user = _Globals.GetUser(uName);

            try
            {
                Name = user.FirstName + " " + user.LastName;
                realUsername = user.Username;
                Username = "(" + user.Username + ")";
                Description = user.Description;
                ProfilePictureSource = user.PhotoSource;

                Following = _Globals.GlobalFeedPosts[_Globals.OpenID].Following;
            }
            catch (Exception ex)
            {

            }
        }

        private void SetCollection()
        {
            YourPostsCollection.Clear();
            YourPostsCollection = new ObservableCollection<PostWrapper>(_Globals.GlobalFeedPosts);

            if (YourPostsCollection.Count != 0)
            {
                this.SetCollectionHeight();
                PostsCount = YourPostsCollection.Count;
            }
        }

        public PersonsProfilePageViewModel()
        {
            SetUserData();
            SetCollection();

            // Assigning functions to the commands
            BackCommand = new Command(async () => await BackFunction());
            RefreshCommand = new Command(async () => await RefreshFunction());
            FollowProfileCommand = new Command(FollowProfileFunction);
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
        private string realUsername = "";
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

        // following parameter
        private string following = "Follow";
        public string Following
        {
            get => following;
            set
            {
                if (value == following) { return; }
                following = value;
                OnPropertyChanged(nameof(Following));
            }
        }

        // Commands PostHeight
        // Back to post command
        public ICommand BackCommand { get; set; }
        private async Task BackFunction()
        {
            _Globals.OpenID = 0;
            await Shell.Current.GoToAsync("..");
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

        // Follow profile command
        public ICommand FollowProfileCommand { get; set; }
        private void FollowProfileFunction()
        {
            if (Following == "Follow")
            {
                _Globals.GlobalMainUser.AddFollowing(realUsername);
                _Globals.GlobalFeedPosts[_Globals.OpenID].Following = "Following";
                Following = "Following";
            }
            else
            {
                _Globals.GlobalMainUser.RemoveFollowing(realUsername);
                _Globals.GlobalFeedPosts[_Globals.OpenID].Following = "Follow";
                Following = "Follow";
            }
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
