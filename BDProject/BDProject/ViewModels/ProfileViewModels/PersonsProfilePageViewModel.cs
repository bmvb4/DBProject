using BDProject.DatabaseModels;
using BDProject.Helpers;
using BDProject.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace BDProject.ViewModels.ProfileViewModels
{
    public class PersonsProfilePageViewModel : BaseViewModel
    {

        private async void SetUserData()
        {
            YourPostsCollection.Clear();

            try
            {
                var success = await ServerServices.SendGetRequestAsync($"profile/user/get/{ _Globals.UsernameTemp}", new JObject());

                if (success.IsSuccessStatusCode)
                {
                    var earthquakesJson = success.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<ProfileDB>(earthquakesJson);

                    Name = $"{rootobject.FirstName} {rootobject.LastName}";
                    realUsername = rootobject.Username;
                    Username = $"({ rootobject.Username})";
                    Description = rootobject.Description;

                    if (rootobject.Photo == null)
                        rootobject.Photo = Convert.FromBase64String(_Globals.Base64DefaultPhoto);

                    ProfilePictureSource = ImageSource.FromStream(() => new MemoryStream(rootobject.Photo));

                    FollowingCount = rootobject.Follower;
                    FollowersCount = rootobject.Followed;
                    IsFollowing = _Globals.GlobalFeedPosts.First(x => x.IdPost == _Globals.OpenID).IsFollow;

                    //AllPostsCollection = new ObservableRangeCollection<Post>(user.Posts);
                    AllPostsCollection = new ObservableRangeCollection<Post>();
                    PostsCount = AllPostsCollection.Count;

                    YourPostsCollection.Clear();
                    if (AllPostsCollection.Count - YourPostsCollection.Count < 3 * 10)
                    {
                        YourPostsCollection.AddRange(AllPostsCollection);
                    }
                    else
                    {
                        YourPostsCollection.AddRange(AllPostsCollection.Take(3 * 10));
                    }
                }
                else if (success.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await ServerServices.RefreshTokenAsync();
                    SetUserData();
                }
            }
            catch(Exception ex)
            {
                string s = ex.Message;
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
        private string realUsername = "";
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

        // following parameter
        private bool IsFollowing 
        { 
            get => IsFollowing;
            set
            {
                IsFollowing = value;
                OnPropertyChanged(nameof(IsFollowingString));
            }
        }
        public string IsFollowingString => (IsFollowing) ? "Fllowing" : "Follow";

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
            if (IsFollowing)
            {
                JObject oJsonObject = new JObject();
                oJsonObject.Add("idFollowed", Username);
                oJsonObject.Add("idFollower", _Globals.GlobalMainUser.Username);

                var success = await ServerServices.SendPostRequestAsync("follow", oJsonObject);

                if (success.IsSuccessStatusCode)
                {
                    _Globals.GlobalMainUser.FollowingsCount++;
                    _Globals.GlobalFeedPosts.First(x => x.IdPost == _Globals.OpenID).IsFollow = true;
                    IsFollowing = true;
                }
                else if (success.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await ServerServices.RefreshTokenAsync();
                    FollowProfileFunction();
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
                    _Globals.GlobalMainUser.FollowingsCount--;
                    _Globals.GlobalFeedPosts.First(x => x.IdPost == _Globals.OpenID).IsFollow = false;
                    IsFollowing = false;
                }
                else if (success.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await ServerServices.RefreshTokenAsync();
                    FollowProfileFunction();
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

    }
}
