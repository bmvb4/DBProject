using BDProject.DatabaseModels;
using BDProject.Helpers;
using BDProject.Models;
using BDProject.Views._PopUps;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
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
                var rootobject = JsonConvert.DeserializeObject<List<BigPostDB>>(earthquakesJson);

                _Globals.AddPostsFromDB(rootobject);
                AllPostsCollection = new ObservableRangeCollection<Post>(_Globals.GlobalFeedPosts);

                if (AllPostsCollection.Count - PostsCollection.Count < 10)
                {
                    PostsCollection.AddRange(AllPostsCollection);
                }
                else
                {
                    PostsCollection.AddRange(AllPostsCollection.Take(10));
                }
            }
            else if (success.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ServerServices.RefreshTokenAsync();
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
            LikePostCommand = new Command<Post>(LikePostFunction);
            SearchTagCommand = new Command<Tag>(SearchTagFunction);

            // open commands
            OpenPostCommentsCommand = new Command<Post>(OpenPostCommentsFunction);
            OpenSearchCommand = new Command(async () => await OpenSearchFunction());
            OpenMyProfileCommand= new Command(async () => await OpenMyProfileFunction());
            OpenProfileCommand = new Command<Post>(OpenProfileFunction);

            // More command
            MoreCommand = new Command<Post>(MoreFunction);
            LoadMoreCommand = new Command(async () => await LoadMoreFunction());

            FollowProfileCommand = new Command<Post>(FollowProfileFunction);
        }

        private ObservableRangeCollection<Post> AllPostsCollection = new ObservableRangeCollection<Post>();

        // Parameters
        // Posts Collection parameter
        private ObservableRangeCollection<Post> postsCollection = new ObservableRangeCollection<Post>();
        public ObservableRangeCollection<Post> PostsCollection
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
        private async void LikePostFunction(Post post)
        {
            JObject oJsonObject = new JObject();
            oJsonObject.Add("idUser", post.IdUser);
            oJsonObject.Add("idPost", post.IdPost);

            if (!post.isLiked)
            {
                var success = await ServerServices.SendPostRequestAsync("posts/like", oJsonObject);
                if (success.IsSuccessStatusCode)
                {
                    _Globals.GlobalFeedPosts.First(x => x.IdPost == post.IdPost).AddLike(new Like(_Globals.GlobalMainUser.Photo, _Globals.GlobalMainUser.Username));
                    post.isLiked = true;
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
                    _Globals.GlobalFeedPosts.First(x => x.IdPost == post.IdPost).RemoveLike(new Like(_Globals.GlobalMainUser.Photo, _Globals.GlobalMainUser.Username));
                    post.isLiked = false;
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
        private async void OpenProfileFunction(Post post)
        {
            _Globals.UsernameTemp = post.IdUser;
            await Shell.Current.GoToAsync("PersonsProfilePage");
        }

        // Edit profile command
        public ICommand MoreCommand { get; set; }
        private async void MoreFunction(Post post)
        {
            _Globals.OpenID = post.IdPost;
            await PopupNavigation.Instance.PushAsync(new PostPopUp());
        }

        // Follow profile command 
        public ICommand FollowProfileCommand { get; set; }
        private async void FollowProfileFunction(Post post)
        {
            if (!post.IsFollow)
            {
                JObject oJsonObject = new JObject();
                oJsonObject.Add("idFollowed", post.IdUser);
                oJsonObject.Add("idFollower", _Globals.GlobalMainUser.Username);

                var success = await ServerServices.SendPostRequestAsync("follow", oJsonObject);

                if (success.IsSuccessStatusCode)
                {
                    _Globals.GlobalMainUser.AddFollowing(post.IdUser);
                    _Globals.AddFollowing(post.IdUser);

                    post.IsFollow = true;
                }
                else if (success.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await ServerServices.RefreshTokenAsync();
                }
            }
            else
            {
                JObject oJsonObject = new JObject();
                oJsonObject.Add("idFollowed", post.IdUser);
                oJsonObject.Add("idFollower", _Globals.GlobalMainUser.Username);

                var success = await ServerServices.SendDeleteRequestAsync("follow", oJsonObject);

                if (success.IsSuccessStatusCode)
                {
                    _Globals.GlobalMainUser.RemoveFollowing(post.IdUser);
                    _Globals.RemoveFollowing(post.IdUser);

                    post.IsFollow = false;
                }
                else if (success.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await ServerServices.RefreshTokenAsync();
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
