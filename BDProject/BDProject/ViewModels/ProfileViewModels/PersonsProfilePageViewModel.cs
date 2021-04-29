using BDProject.Helpers;
using BDProject.Models;
using BDProject.ModelWrappers;
using MvvmHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels.ProfileViewModels
{
    public class PersonsProfilePageViewModel : BaseViewModel
    {

        private async void SetUserData()
        {
            YourPostsCollection.Clear();
            string uName = _Globals.GlobalFeedPosts.First(x => x.PostID == _Globals.OpenID).Username;
            JObject oJsonObject = new JObject();

            var success = await ServerServices.SendGetRequestAsync($"profile/user/get/{uName}", oJsonObject);

            if (success.IsSuccessStatusCode)
            {
                var earthquakesJson = success.Content.ReadAsStringAsync().Result;
                var rootobject = JsonConvert.DeserializeObject<Profile>(earthquakesJson);

                Name = rootobject.FirstName + " " + rootobject.LastName;
                realUsername = rootobject.Username;
                Username = "(" + rootobject.Username + ")";
                Description = rootobject.Description;
                ProfilePictureSource = ImageSource.FromStream(() => new MemoryStream(rootobject.Photo));

                FollowingCount = rootobject.Followed;
                FollowersCount = rootobject.Follower;

                Following = _Globals.GlobalFeedPosts.First(x => x.PostID == _Globals.OpenID).Following;

                AllPostsCollection = new ObservableRangeCollection<PostWrapper>(_Globals.FromPostToWrapperList(rootobject.Posts));
                PostsCount = AllPostsCollection.Count;

                YourPostsCollection.Clear();
                if (AllPostsCollection.Count - YourPostsCollection.Count < 3 * 10)
                {
                    YourPostsCollection.AddRange(AllPostsCollection);
                    SetCollectionHeight();
                }
                else
                {
                    YourPostsCollection.AddRange(AllPostsCollection.Take(3 * 10));
                    SetCollectionHeight();
                }
            }
        }

        public PersonsProfilePageViewModel()
        {
            SetUserData();

            // Assigning functions to the commands
            BackCommand = new Command(async () => await BackFunction());
            RefreshCommand = new Command(async () => await RefreshFunction());
            FollowProfileCommand = new Command(FollowProfileFunction);
            LoadMoreCommand = new Command(async () => await LoadMoreFunction());
        }

        private ObservableRangeCollection<PostWrapper> AllPostsCollection = new ObservableRangeCollection<PostWrapper>();

        // Parameters
        // Your Posts Collection parameter
        private ObservableRangeCollection<PostWrapper> yourPostsCollection = new ObservableRangeCollection<PostWrapper>();
        public ObservableRangeCollection<PostWrapper> YourPostsCollection
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
            IsRefreshing = false;
        }

        // Follow profile command
        public ICommand FollowProfileCommand { get; set; }
        private async void FollowProfileFunction()
        {
            if (Following == "Follow")
            {
                JObject oJsonObject = new JObject();
                oJsonObject.Add("idFollowed", Username);
                oJsonObject.Add("idFollower", _Globals.GlobalMainUser.Username);

                var success = await ServerServices.SendPostRequestAsync("follow", oJsonObject);

                if (success.IsSuccessStatusCode)
                {
                    _Globals.GlobalMainUser.AddFollowing(realUsername);
                    _Globals.GlobalFeedPosts.First(x => x.PostID == _Globals.OpenID).Following = "Following";
                    Following = "Following";
                }
            }
            else
            {
                JObject oJsonObject = new JObject();
                oJsonObject.Add("idFollowed", Username);
                oJsonObject.Add("idFollower", _Globals.GlobalMainUser.Username);

                var success = await ServerServices.SendDeleteRequestAsync("follow", oJsonObject);

                if (success.IsSuccessStatusCode)
                {
                    _Globals.GlobalMainUser.RemoveFollowing(realUsername);
                    _Globals.GlobalFeedPosts.First(x => x.PostID == _Globals.OpenID).Following = "Follow";
                    Following = "Follow";
                }
            }
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
