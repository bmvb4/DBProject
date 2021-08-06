using BDProject.DatabaseModels;
using BDProject.Helpers;
using BDProject.Models;
using BDProject.Views._PopUps;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace BDProject.ViewModels
{
    public class ProfilePageViewModel : BaseViewModel
    {
        public async void SetUserData()
        {
            try
            {
                var success = await ServerServices.SendGetRequestAsync($"profile/user/get/{ _Globals.GlobalMainUser.Username}", new JObject());

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

                    JObject oJsonObject = new JObject();
                    oJsonObject.Add("Username", _Globals.GlobalMainUser.Username);

                    success = await ServerServices.SendGetRequestAsync($"posts/getAll/{_Globals.GlobalMainUser.Username}/0", oJsonObject);

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

        public ProfilePageViewModel()
        {
            IsBusy = true;
            SetUserData();

            // Assigning functions to the commands
            RefreshCommand = new Command(RefreshFunction);

            // like commands
            LikePostCommand = new Command<Post>(LikePostFunction);
            SearchTagCommand = new Command<Tag>(SearchTagFunction);
            ShowTagsCommand = new Command(ShowTagsFunction);

            // open commands
            OpenSettingsCommand = new Command(async () => await OpenSettingsFunction());
            OpenEditProfileCommand = new Command(async () => await OpenEditProfileFunction());
            OpenPostCommentsCommand = new Command<Post>(OpenPostCommentsFunction);
            OpenTagsCommand = new Command<Post>(OpenTagsFunction);

            // More command
            MoreCommand = new Command<Post>(MoreFunction);
            LoadMoreCommand = new Command(async () => await LoadMoreFunction());
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
            IsBusy = true;
            SetUserData();
            IsBusy = false;
            IsRefreshing = false;
        }

        // load more command 
        public ICommand LoadMoreCommand { get; set; }
        private async Task LoadMoreFunction()
        {
            if (IsBusy) { return; }
            IsBusy = true;

            if (YourPostsCollection.Count % 10 == 0 && YourPostsCollection.Count != 1 && YourPostsCollection.Count != 0) 
            {
                JObject oJsonObject = new JObject();
                oJsonObject.Add("Username", _Globals.GlobalMainUser.Username);

                var success = await ServerServices.SendGetRequestAsync($"posts/getAll/{_Globals.GlobalMainUser.Username}/{YourPostsCollection.Count / 10}", oJsonObject);

                if (success.IsSuccessStatusCode)
                {

                    var earthquakesJson = success.Content.ReadAsStringAsync().Result;
                    var postList = JsonConvert.DeserializeObject<List<BigPostDB>>(earthquakesJson);

                    foreach (BigPostDB post in postList)
                        YourPostsCollection.Add(new Post(post));

                    PostsCount = YourPostsCollection.Count;
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
                if (success.IsSuccessStatusCode) post.IsLiked = true;
                if (success.StatusCode == HttpStatusCode.Unauthorized) await ServerServices.RefreshTokenAsync();
                return;
            }
            if (post.IsLiked)
            {
                var success = await ServerServices.SendDeleteRequestAsync("posts/unlike", oJsonObject);
                if (success.IsSuccessStatusCode) post.IsLiked = false;
                if (success.StatusCode == HttpStatusCode.Unauthorized) await ServerServices.RefreshTokenAsync();
                return;
            }
        }

        // Open Post Comments command
        public ICommand OpenPostCommentsCommand { get; set; }
        private async void OpenPostCommentsFunction(Post post)
        {
            _Globals.OpenID = (int)post.IdPost;
            await Shell.Current.GoToAsync("PostComments");
        }

        // Edit profile command
        public ICommand MoreCommand { get; set; }
        private async void MoreFunction(Post post)
        {
            _Globals.OpenID = post.IdPost;
            await PopupNavigation.Instance.PushAsync(new PostPopUp());
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
