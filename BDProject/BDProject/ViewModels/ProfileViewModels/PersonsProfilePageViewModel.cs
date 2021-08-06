using BDProject.DatabaseModels;
using BDProject.Helpers;
using BDProject.Models;
using BDProject.Views._PopUps;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace BDProject.ViewModels.ProfileViewModels
{
    public class PersonsProfilePageViewModel : BaseViewModel
    {

        public async void SetUserData()
        {
            try
            {
                JObject oJsonObject = new JObject();
                oJsonObject.Add("Username", _Globals.GlobalMainUser.Username);

                var success = await ServerServices.SendGetRequestAsync($"profile/user/get/{ _Globals.UsernameTemp}", oJsonObject);

                if (success.IsSuccessStatusCode)
                {
                    var earthquakesJson = success.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<UserDB>(earthquakesJson);

                    Name = $"{rootobject.FirstName} {rootobject.LastName}";
                    Username = $"{rootobject.Username}";
                    Description = rootobject.Description;

                    if (rootobject.Photo == null)
                        rootobject.Photo = _Globals.Base64Bytes;

                    ProfilePictureSource = ImageSource.FromStream(() => new MemoryStream(rootobject.Photo));

                    FollowingCount = rootobject.Followed;
                    FollowersCount = rootobject.Follower;
                    PostsCount = rootobject.PostCount;

                    IsFollowing = rootobject.isFollow;

                    success = await ServerServices.SendGetRequestAsync($"posts/getAll/{_Globals.UsernameTemp}/0", oJsonObject);

                    if (success.IsSuccessStatusCode)
                    {
                        earthquakesJson = success.Content.ReadAsStringAsync().Result;
                        var postList = JsonConvert.DeserializeObject<List<BigPostDB>>(earthquakesJson);

                        YourPostsCollection.Clear();

                        foreach (BigPostDB post in postList)
                            YourPostsCollection.Add(new Post(post));
                    }
                }
                else if (success.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await ServerServices.RefreshTokenAsync();
                    SetUserData();
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }

        public PersonsProfilePageViewModel()
        {
            IsBusy = true;
            SetUserData();

            // Assigning functions to the commands
            RefreshCommand = new Command(async () => await RefreshFunction());

            // like commands
            LikePostCommand = new Command<Post>(LikePostFunction);
            SearchTagCommand = new Command<Tag>(SearchTagFunction);
            ShowTagsCommand = new Command(ShowTagsFunction);

            // open commands
            OpenPostCommentsCommand = new Command<Post>(OpenPostCommentsFunction);
            OpenTagsCommand = new Command<Post>(OpenTagsFunction);

            // More command
            BackCommand = new Command(async () => await BackFunction());
            LoadMoreCommand = new Command(async () => await LoadMoreFunction());
            FollowProfileCommand = new Command(FollowProfileFunction);

            IsBusy = false;
        }

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
                if (value == postsCount || value < 0) { return; }
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
                if (value == followersCount || value < 0) { return; }
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
                if (value == followingCount || value < 0) { return; }
                followingCount = value;
                OnPropertyChanged();
            }
        }

        // following parameter
        private bool isFollowing = false;
        private bool IsFollowing 
        { 
            get => isFollowing;
            set
            {
                isFollowing = value;
                OnPropertyChanged(nameof(IsFollowingString));
            }
        }
        public string IsFollowingString => (isFollowing) ? "Following" : "Follow";

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

        // Refreshing parameter
        private bool isTagsVisible = false;
        public bool IsTagsVisible
        {
            get => isTagsVisible;
            set
            {
                if (value == isTagsVisible) { return; }
                isTagsVisible = value;
                OnPropertyChanged();
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
            if (!IsFollowing)
            {
                JObject oJsonObject = new JObject();
                oJsonObject.Add("idFollowed", Username);
                oJsonObject.Add("idFollower", _Globals.GlobalMainUser.Username);

                var success = await ServerServices.SendPostRequestAsync("follow", oJsonObject);

                if (success.IsSuccessStatusCode)
                {
                    FollowingCount++;
                    _Globals.Refresh = true;
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
                    FollowingCount--;
                    _Globals.Refresh = true;
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
            if (IsBusy) { return; }
            IsBusy = true;

            if (YourPostsCollection.Count % 10 == 0)
            {
                JObject oJsonObject = new JObject();
                oJsonObject.Add("Username", _Globals.GlobalMainUser.Username);

                var success = await ServerServices.SendGetRequestAsync($"posts/getAll/{YourPostsCollection.Count / 10}", oJsonObject);

                if (success.IsSuccessStatusCode)
                {

                    var earthquakesJson = success.Content.ReadAsStringAsync().Result;
                    var postList = JsonConvert.DeserializeObject<List<BigPostDB>>(earthquakesJson);

                    foreach (BigPostDB post in postList)
                        YourPostsCollection.Add(new Post(post));
                }
            }

            IsBusy = false;
        }

        // Like Post command
        public ICommand LikePostCommand { get; set; }
        private async void LikePostFunction(Post post)
        {
            JObject oJsonObject = new JObject();
            oJsonObject.Add("idUser", _Globals.GlobalMainUser.Username);
            oJsonObject.Add("idPost", post.IdPost);

            if (!post.IsLiked)
            {
                var success = await ServerServices.SendPostRequestAsync("posts/like", oJsonObject);
                if (success.IsSuccessStatusCode)
                {
                    post.IsLiked = true;
                    post.LikesCount++;
                }
                else if (success.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await ServerServices.RefreshTokenAsync();
                }
            }
            else
            {
                var success = await ServerServices.SendDeleteRequestAsync("posts/unlike", oJsonObject);
                if (success.IsSuccessStatusCode)
                {
                    post.IsLiked = false;
                    post.LikesCount--;
                }
                else if (success.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await ServerServices.RefreshTokenAsync();
                }
            }
        }

        // Open Post Comments command
        public ICommand OpenPostCommentsCommand { get; set; }
        private async void OpenPostCommentsFunction(Post post)
        {
            _Globals.OpenID = (int)post.IdPost;
            await Shell.Current.GoToAsync("PostComments");
        }

        public ICommand SearchTagCommand { get; set; }
        private async void SearchTagFunction(Tag tag)
        {
            await Task.Delay(1000);
        }

        public ICommand ShowTagsCommand { get; set; }
        private async void ShowTagsFunction()
        {
            await Task.Delay(0);
            if (IsTagsVisible)
                IsTagsVisible = false;
            else
                IsTagsVisible = true;
        }

        public ICommand OpenTagsCommand { get; set; }
        private async void OpenTagsFunction(Post post)
        {
            if (post.tags != null && post.tags.Count != 0)
                await PopupNavigation.Instance.PushAsync(new AllTagsPopUp(post.TagsCollection));
            else
                await PopupNavigation.Instance.PushAsync(new AllTagsPopUp(new ObservableRangeCollection<Tag>()));
        }

    }
}
