using BDProject.Helpers;
using BDProject.Models;
using BDProject.ModelWrappers;
using BDProject.Views._PopUps;
using MvvmHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
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
                AllPostsCollection = new ObservableRangeCollection<PostWrapper>(_Globals.GlobalFeedPosts);

                if (AllPostsCollection.Count - PostsCollection.Count < 10)
                {
                    PostsCollection.AddRange(AllPostsCollection);
                }
                else
                {
                    PostsCollection.AddRange(AllPostsCollection.Take(10));
                }
            }

            _Globals.IsBusy = false;
        }

        // setting application defaults
        public HomePageViewModel()
        {
            _Globals.IsBusy = true;
            SetCollection();

            // Assigning functions to the commands
            // refresh command
            RefreshCommand = new Command(RefreshFunction);

            // like commands
            LikePostCommand = new Command<PostWrapper>(LikePostFunction);
            SearchTagCommand = new Command<Tag>(SearchTagFunction);

            // open commands
            OpenPostCommentsCommand = new Command<PostWrapper>(OpenPostCommentsFunction);
            OpenSearchCommand = new Command(async () => await OpenSearchFunction());
            OpenMyProfileCommand= new Command(async () => await OpenMyProfileFunction());
            OpenProfileCommand = new Command<PostWrapper>(OpenProfileFunction);

            // More command
            MoreCommand = new Command<PostWrapper>(MoreFunction);
            LoadMoreCommand = new Command(async () => await LoadMoreFunction());

            FollowProfileCommand = new Command<PostWrapper>(FollowProfileFunction);
        }

        private ObservableRangeCollection<PostWrapper> AllPostsCollection = new ObservableRangeCollection<PostWrapper>();

        // Parameters
        // Posts Collection parameter
        private ObservableRangeCollection<PostWrapper> postsCollection = new ObservableRangeCollection<PostWrapper>();
        public ObservableRangeCollection<PostWrapper> PostsCollection
        {
            get => postsCollection;
            set
            {
                if (value == postsCollection) { return; }
                postsCollection = value;
                OnPropertyChanged(nameof(PostsCollection));
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
        private void RefreshFunction()
        {
            IsRefreshing = true;
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

        // load more command 
        public ICommand LoadMoreCommand { get; set; }
        private async Task LoadMoreFunction()
        {
            if (_Globals.IsBusy) { return; }
            _Globals.IsBusy = true;

            await Task.Delay(1000);

            if (AllPostsCollection.Count - PostsCollection.Count < 10)
            {
                PostsCollection.AddRange(AllPostsCollection.Skip(PostsCollection.Count));
            }
            else
            {
                PostsCollection.AddRange(AllPostsCollection.Skip(PostsCollection.Count).Take(10));
            }

            _Globals.IsBusy = false;
        }

        public ICommand SearchTagCommand { get; set; }
        private async void SearchTagFunction(Tag tag)
        {
            await Task.Delay(1000);
        }
    }
}
