using BDProject.Helpers;
using BDProject.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace BDProject.ViewModels
{
    public class ProfilePageViewModel : BaseViewModel
    {
        public void SetUserData()
        {
            User user = _Globals.GlobalMainUser;

            try
            {
                Name = user.FirstName + " " + user.LastName;
                Username = "(" + user.Username + ")";
                Description = user.Description;
                ProfilePictureSource = user.PhotoSource;

                FollowingCount = _Globals.GlobalMainUser.Followings.Count;
                FollowersCount = _Globals.GlobalMainUser.Followers.Count;

                AllPostsCollection = new ObservableRangeCollection<Post>(_Globals.GlobalMainUser.Posts);
                PostsCount = AllPostsCollection.Count;

                YourPostsCollection.Clear();
                if (AllPostsCollection.Count - YourPostsCollection.Count < 3 * 10)
                {
                    YourPostsCollection.AddRange(AllPostsCollection);
                    this.SetCollectionHeight();
                }
                else
                {
                    YourPostsCollection.AddRange(AllPostsCollection.Take(3 * 10));
                    this.SetCollectionHeight();
                }
            }
            catch(Exception ex)
            {
                
            }
        }

        public ProfilePageViewModel()
        {
            SetUserData();

            // Assigning functions to the commands
            OpenSettingsCommand = new Command(async () => await OpenSettingsFunction());
            OpenEditProfileCommand=new Command(async () => await OpenEditProfileFunction());
            RefreshCommand = new Command(RefreshFunction);
            OpenPostsCommand = new Command<Post>(OpenPostsFunction);
            LoadMoreCommand = new Command(async () => await LoadMoreFunction());
        }

        private ObservableRangeCollection<Post> AllPostsCollection = new ObservableRangeCollection<Post>();

        // Parameters
        // Your Posts Collection parameter
        private ObservableRangeCollection<Post> yourPostsCollection = new ObservableRangeCollection<Post>();
        public ObservableRangeCollection<Post> YourPostsCollection
        {
            get => yourPostsCollection;
            set
            {
                if (value == yourPostsCollection) { return; }
                yourPostsCollection = value;
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged(); 
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
        public void RefreshFunction()
        {
            IsRefreshing = true;

            SetUserData();

            IsRefreshing = false;
        }

        // Open Posts command
        public ICommand OpenPostsCommand { get; set; }
        private async void OpenPostsFunction(Post post)
        {
            _Globals.OpenID = post.IdPost;
            await Shell.Current.GoToAsync("MyProfilePostPage");
        }

        // load more command 
        public ICommand LoadMoreCommand { get; set; }
        private async Task LoadMoreFunction()
        {
            if (_Globals.IsBusy) { return; }
            _Globals.IsBusy = true;

            await Task.Delay(1000);

            if (AllPostsCollection.Count - YourPostsCollection.Count < 3 * 10)
            {
                YourPostsCollection.AddRange(AllPostsCollection.Skip(YourPostsCollection.Count));
            }
            else
            {
                YourPostsCollection.AddRange(AllPostsCollection.Skip(YourPostsCollection.Count).Take(3 * 10));
            }

            this.SetCollectionHeight();
            _Globals.IsBusy = false;
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
