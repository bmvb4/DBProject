using BDProject.Helpers;
using BDProject.Models;
using BDProject.ModelWrappers;
using BDProject.Views._PopUps;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {

        public async void SetCollection()
        {
            PostsCollection.Clear();

            JObject oJsonObject = new JObject();
            oJsonObject.Add("Username", _Globals.GlobalMainUser.Username);

            var success = await ServerServices.SendGetRequestAsync("posts/getlast", oJsonObject);

            if (success.IsSuccessStatusCode)
            {
                var earthquakesJson = success.Content.ReadAsStringAsync().Result;
                var rootobject = JsonConvert.DeserializeObject<List<PostUser>>(earthquakesJson);

                _Globals.AddPostsFromDB(rootobject);
                PostsCollection = new ObservableCollection<PostWrapper>(_Globals.GlobalFeedPosts);
            }
        }

        // setting application defaults
        public HomePageViewModel()
        {            
            SetCollection();

            // Assigning functions to the commands
            // refresh command
            RefreshCommand = new Command(async () => await RefreshFunction());

            // like commands
            LikePostCommand = new Command<PostWrapper>(LikePostFunction);
            
            // open commands
            OpenPostCommentsCommand = new Command<PostWrapper>(OpenPostCommentsFunction);
            OpenSearchCommand = new Command(async () => await OpenSearchFunction());
            OpenMyProfileCommand= new Command(async () => await OpenMyProfileFunction());
            OpenProfileCommand = new Command<PostWrapper>(OpenProfileFunction);

            // More command
            MoreCommand = new Command<PostWrapper>(MoreFunction);

            FollowProfileCommand = new Command<PostWrapper>(FollowProfileFunction);
        }

        // Parameters
        // Posts Collection parameter
        private ObservableCollection<PostWrapper> postsCollection = new ObservableCollection<PostWrapper>();
        public ObservableCollection<PostWrapper> PostsCollection
        {
            get => postsCollection;
            set
            {
                if (value == postsCollection) { return; }
                postsCollection = value;
                OnPropertyChanged(nameof(PostsCollection));
            }
        }

        // Search parameter
        private string search = "";
        public string Search
        {
            get => search;
            set
            {
                if (value == search) { return; }
                search = value;
                OnPropertyChanged(nameof(Search));
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

        // Refreshing parameter
        private bool isRefreshing = false;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                if (value == isRefreshing) { return; }
                isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        // Commands
        // Like Post command
        public ICommand LikePostCommand { get; set; }
        private async void LikePostFunction(PostWrapper post)
        {
            JObject oJsonObject = new JObject();
            oJsonObject.Add("idUser", post.Username);
            oJsonObject.Add("idPost", post.PostID);

            if (post.IsLikeUsernameInside(_Globals.GlobalMainUser.Username) == false)
            {
                var success = await ServerServices.SendPostRequestAsync("posts/like", oJsonObject);
                if (success.IsSuccessStatusCode)
                {
                    _Globals.GlobalFeedPosts.First(x => x.PostID==post.PostID).AddLike(new LikeWrapper(_Globals.GlobalMainUser.ImageBytes, _Globals.GlobalMainUser.Username));
                }
            }
            else
            {
                var success = await ServerServices.SendDeleteRequestAsync("posts/unlike", oJsonObject);
                if (success.IsSuccessStatusCode)
                {
                    _Globals.GlobalFeedPosts.First(x => x.PostID == post.PostID).RemoveLike(new LikeWrapper(_Globals.GlobalMainUser.ImageBytes, _Globals.GlobalMainUser.Username));
                }
            }
        }

        // Open Post Comments command
        public ICommand OpenPostCommentsCommand { get; set; }
        private async void OpenPostCommentsFunction(PostWrapper post)
        {
            _Globals.OpenID = (int)post.PostID;
            await Shell.Current.GoToAsync("PostComments");
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

        // Open Search command
        public ICommand OpenSearchCommand { get; set; }
        private async Task OpenSearchFunction()
        {
            await Shell.Current.GoToAsync("SearchPage");
        }

        // Open my profile command
        public ICommand OpenMyProfileCommand { get; set; }
        private async Task OpenMyProfileFunction()
        {
            await Shell.Current.GoToAsync("//ProfilePage");
        }

        // open profile command
        public ICommand OpenProfileCommand { get; set; }
        private async void OpenProfileFunction(PostWrapper post)
        {
            _Globals.OpenID = post.PostID;
            await Shell.Current.GoToAsync("PersonsProfilePage");
        }

        // Edit profile command
        public ICommand MoreCommand { get; set; }
        private async void MoreFunction(PostWrapper post)
        {
            _Globals.OpenID = post.PostID;
            await PopupNavigation.Instance.PushAsync(new PostPopUp());

            //await Shell.Current.GoToAsync("EditPostPage");
        }

        // Follow profile command 
        public ICommand FollowProfileCommand { get; set; }
        private async void FollowProfileFunction(PostWrapper post)
        {
            if (post.Following == "Follow")
            {
                JObject oJsonObject = new JObject();
                oJsonObject.Add("idFollowed", post.Username);
                oJsonObject.Add("idFollower", _Globals.GlobalMainUser.Username);

                var success = await ServerServices.SendPostRequestAsync("follow", oJsonObject);

                if (success.IsSuccessStatusCode)
                {
                    _Globals.GlobalMainUser.AddFollowing(post.Username);
                    _Globals.AddFollowing(post.Username);
                    post.Following = "Following";
                }
            }
            else
            {
                JObject oJsonObject = new JObject();
                oJsonObject.Add("idFollowed", post.Username);
                oJsonObject.Add("idFollower", _Globals.GlobalMainUser.Username);

                var success = await ServerServices.SendDeleteRequestAsync("follow", oJsonObject);

                if (success.IsSuccessStatusCode)
                {
                    _Globals.GlobalMainUser.RemoveFollowing(post.Username);
                    _Globals.RemoveFollowing(post.Username);
                    post.Following = "Follow";
                }
            }
        }
    }
}
