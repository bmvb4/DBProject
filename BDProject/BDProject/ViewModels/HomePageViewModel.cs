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

            //=================== TESTING ========================
            List<BigPostDB> bigposts = new List<BigPostDB>()
            {
                new BigPostDB
                {
                    IdPost = 1,
                    IdUser = _Globals.GlobalMainUser.Username,
                    Photo = _Globals.GlobalMainUser.Photo,
                    Description = "My First Artwork",
                    CommentsCounter = 1234,
                    LikesCounter = 245678,
                    isLiked = true,
                    isFollow = true,
                    UserPhoto = _Globals.GlobalMainUser.Photo,
                    DeleteDate = new DateTime(2021, 7, 30),
                    tags = new List<string> { "Cat", "Cats", "Animals", "Nature" }
                }
            };
            _Globals.GlobalFeedPosts.Add(new Post(bigposts[0]));
            PostsCollection.Add(new Post(bigposts[0]));
            //=================== TESTING ========================

            JObject oJsonObject = new JObject();
            oJsonObject.Add("Username", _Globals.GlobalMainUser.Username);

            var success = await ServerServices.SendGetRequestAsync("posts/getlast/0", oJsonObject);

            if (success.IsSuccessStatusCode)
            {
                var earthquakesJson = success.Content.ReadAsStringAsync().Result;
                var rootobject = JsonConvert.DeserializeObject<List<BigPostDB>>(earthquakesJson);

                _Globals.AddPostsFromDB(rootobject);

                foreach (BigPostDB post in rootobject)
                    PostsCollection.Add(new Post(post));
            }
        }

        // setting application defaults
        public HomePageViewModel()
        {
            IsBusy = true;
            SetCollection();

            // Assigning functions to the commands
            // refresh command
            RefreshCommand = new Command(RefreshFunction);

            // like commands
            LikePostCommand = new Command<Post>(LikePostFunction);
            SearchTagCommand = new Command<Tag>(SearchTagFunction);
            ShowTagsCommand = new Command(ShowTagsFunction);

            // open commands
            OpenPostCommentsCommand = new Command<Post>(OpenPostCommentsFunction);
            OpenSearchCommand = new Command(async () => await OpenSearchFunction());
            OpenMyProfileCommand= new Command(async () => await OpenMyProfileFunction());
            OpenProfileCommand = new Command<Post>(OpenProfileFunction);
            OpenTagsCommand = new Command<Post>(OpenTagsFunction);

            // More command
            MoreCommand = new Command<Post>(MoreFunction);
            LoadMoreCommand = new Command(LoadMoreFunction);

            FollowProfileCommand = new Command<Post>(FollowProfileFunction);
            IsBusy = false;
        }

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

        // Commands
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

        // Refresh collection view command
        public ICommand RefreshCommand { get; set; }
        private void RefreshFunction()
        {
            IsRefreshing = true;
            IsBusy = true;
            SetCollection();
            IsBusy = false;
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
                    for (int i = 0; i < PostsCollection.Count; i++)
                        PostsCollection[i].IsFollow = true;
                    for (int i = 0; i < _Globals.GlobalFeedPosts.Count; i++)
                        _Globals.GlobalFeedPosts[i].IsFollow = true;

                    //PostsCollection.First(x => x.IdPost == post.IdPost).IsFollow = true;
                    //_Globals.GlobalFeedPosts.First(x => x.IdPost == post.IdPost).IsFollow = true;
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
                    for (int i = 0; i < PostsCollection.Count; i++)
                        PostsCollection[i].IsFollow = false;
                    for (int i = 0; i < _Globals.GlobalFeedPosts.Count; i++)
                        _Globals.GlobalFeedPosts[i].IsFollow = false;

                    //PostsCollection.First(x => x.IdPost == post.IdPost).IsFollow = false;
                    //_Globals.GlobalFeedPosts.First(x => x.IdPost == post.IdPost).IsFollow = false;
                }
                else if (success.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await ServerServices.RefreshTokenAsync();
                }
            }
        }

        // load more command 
        public ICommand LoadMoreCommand { get; set; }
        private void LoadMoreFunction()
        {
            if (IsBusy) { return; }
            IsBusy = true;

            if(PostsCollection.Count % 10 == 0)
            {
                JObject oJsonObject = new JObject();
                oJsonObject.Add("Username", _Globals.GlobalMainUser.Username);

                var success = ServerServices.SendGetRequestAsync($"posts/getAll/{PostsCollection.Count / 10}", oJsonObject).Result;

                if (success.IsSuccessStatusCode)
                {

                    var earthquakesJson = success.Content.ReadAsStringAsync().Result;
                    var postList = JsonConvert.DeserializeObject<List<BigPostDB>>(earthquakesJson);

                    _Globals.AddPostsFromDB(postList);

                    foreach (BigPostDB post in postList)
                        PostsCollection.Add(new Post(post));
                }
            }

            IsBusy = false;
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
            if(post.tags != null && post.tags.Count != 0)
                await PopupNavigation.Instance.PushAsync(new AllTagsPopUp(post.TagsCollection));
            else
                await PopupNavigation.Instance.PushAsync(new AllTagsPopUp(new ObservableRangeCollection<Tag>()));
        }
    }
}
