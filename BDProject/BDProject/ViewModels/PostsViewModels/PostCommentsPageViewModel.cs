using BDProject.DatabaseModels;
using BDProject.Helpers;
using BDProject.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace BDProject.ViewModels.PostsViewModels
{
    public class PostCommentsPageViewModel : BaseViewModel
    {
        public async void SetCollection()
        {
            JObject oJsonObject = new JObject();
            oJsonObject.Add("Username", _Globals.GlobalMainUser.Username);
            var success = await ServerServices.SendGetRequestAsync($"posts/comment/get/{_Globals.OpenID}/0", oJsonObject);

            if (success.IsSuccessStatusCode)
            {
                var earthquakesJson = success.Content.ReadAsStringAsync().Result;
                var rootobject = JsonConvert.DeserializeObject<List<CommentDB>>(earthquakesJson);

                CommentsCollection.Clear();
                foreach (CommentDB comment in rootobject)
                    CommentsCollection.Add(new Comment(comment));

                return;
            }
            if (success.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ServerServices.RefreshTokenAsync();
                SetCollection();
                return;
            }
        }

        public void SetParameters()
        {
            if(_Globals.PageNumber == 1)
            {
                Username = _Globals.HomePageViewModelInstance.PostsCollection?.FirstOrDefault(x => x.IdPost == _Globals.OpenID).IdUser;
                Description = _Globals.HomePageViewModelInstance.PostsCollection?.FirstOrDefault(x => x.IdPost == _Globals.OpenID).Description;
                return;
            }
            if(_Globals.PageNumber == 2)
            {
                Username = _Globals.ProfilePageViewModelInstance.YourPostsCollection?.FirstOrDefault(x => x.IdPost == _Globals.OpenID).IdUser;
                Description = _Globals.ProfilePageViewModelInstance.YourPostsCollection?.FirstOrDefault(x => x.IdPost == _Globals.OpenID).Description;
                return;
            }
            if(_Globals.PageNumber == 3)
            {
                Username = _Globals.PersonsProfilePageViewModelInstance.YourPostsCollection?.FirstOrDefault(x => x.IdPost == _Globals.OpenID).IdUser;
                Description = _Globals.PersonsProfilePageViewModelInstance.YourPostsCollection?.FirstOrDefault(x => x.IdPost == _Globals.OpenID).Description;
                return;
            }
        }

        public PostCommentsPageViewModel()
        {
            IsBusy = true;
            SetCollection();
            SetParameters();

            // Assigning functions to the commands
            BackCommand = new Command(async () => await BackFunction());
            RefreshCommand = new Command(RefreshFunction);
            CommentCommand = new Command(CommentFunction);
            DeleteCommentCommand = new Command<Comment>(DeleteCommentFunction);
            LoadMoreCommand = new Command(LoadMoreFunction);

            OpenProfileCommand = new Command<Comment>(OpenProfileFunctionAsync);
            IsBusy = false;
        }

        // Parameters
        // Posts Collection parameter
        private ObservableRangeCollection<Comment> commentsCollection = new ObservableRangeCollection<Comment>();
        public ObservableRangeCollection<Comment> CommentsCollection
        {
            get => commentsCollection;
            set
            {
                if (value == commentsCollection) { return; }
                commentsCollection = value;
                OnPropertyChanged();
            }
        }

        // Comment parameter
        private string comment = "";
        public string Comment
        {
            get => comment;
            set
            {
                if (value == comment) { return; }
                comment = value;
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        // Post description parameter
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
        // Back to post command
        public ICommand BackCommand { get; set; }
        private async Task BackFunction()
        {
            await Shell.Current.Navigation.PopAsync();

            _Globals.OpenID = 0;
        }

        // Refresh collection view command
        public ICommand RefreshCommand { get; set; }
        private void RefreshFunction()
        {
            IsRefreshing = true;
            SetCollection();
            IsRefreshing = false;
        }

        // delete command
        public ICommand DeleteCommentCommand { get; set; }
        private async void DeleteCommentFunction(Comment comment)
        {
            bool result = await App.Current.MainPage.DisplayAlert("Warning", "Do you want to delete this comment", "Yes", "No");
            if (result == false) { return; }

            JObject oJsonObject = new JObject();
            oJsonObject.Add("idUser", _Globals.GlobalMainUser.Username);
            oJsonObject.Add("idPost", comment.IdPost);
            oJsonObject.Add("idcomment", comment.IdComment);

            var success = await ServerServices.SendDeleteRequestAsync("posts/comment", oJsonObject);
            if (success.IsSuccessStatusCode)
            {
                if (_Globals.PageNumber == 1)
                {
                    _Globals.HomePageViewModelInstance.PostsCollection.FirstOrDefault(x => x.IdPost == _Globals.OpenID).CommentsCount--;
                    SetCollection();
                    return;
                }
                if (_Globals.PageNumber == 2)
                {
                    _Globals.ProfilePageViewModelInstance.YourPostsCollection.FirstOrDefault(x => x.IdPost == _Globals.OpenID).CommentsCount--;
                    SetCollection();
                    return;
                }
                if (_Globals.PageNumber == 3)
                {
                    _Globals.PersonsProfilePageViewModelInstance.YourPostsCollection.FirstOrDefault(x => x.IdPost == _Globals.OpenID).CommentsCount--;
                    SetCollection();
                    return;
                }
            }
            if (success.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ServerServices.RefreshTokenAsync();
                return;
            }
        }

        // comment command
        public ICommand CommentCommand { get; set; }
        private async void CommentFunction()
        {
            if (string.IsNullOrEmpty(Comment) || string.IsNullOrWhiteSpace(Comment)) { return; }

            Post post = new Post();
            if (_Globals.PageNumber == 1) post = _Globals.HomePageViewModelInstance.PostsCollection?.FirstOrDefault(x => x.IdPost == _Globals.OpenID);
            if (_Globals.PageNumber == 2) post = _Globals.ProfilePageViewModelInstance.YourPostsCollection?.FirstOrDefault(x => x.IdPost == _Globals.OpenID);
            if (_Globals.PageNumber == 3) post = _Globals.PersonsProfilePageViewModelInstance.YourPostsCollection?.FirstOrDefault(x => x.IdPost == _Globals.OpenID);

            JObject oJsonObject = new JObject();
            oJsonObject.Add("IdPost", post.IdPost);
            oJsonObject.Add("IdUser", _Globals.GlobalMainUser.Username);
            oJsonObject.Add("CommentText", Comment);

            var success = await ServerServices.SendPostRequestAsync("posts/comment", oJsonObject);
            if (success.IsSuccessStatusCode)
            {
                if (_Globals.PageNumber == 1)
                {
                    _Globals.HomePageViewModelInstance.PostsCollection.FirstOrDefault(x => x.IdPost == _Globals.OpenID).CommentsCount++;
                    SetCollection();
                    Comment = "";
                    return;
                }
                if (_Globals.PageNumber == 2)
                {
                    _Globals.ProfilePageViewModelInstance.YourPostsCollection.FirstOrDefault(x => x.IdPost == _Globals.OpenID).CommentsCount++;
                    SetCollection();
                    Comment = "";
                    return;
                }
                if (_Globals.PageNumber == 3)
                {
                    _Globals.PersonsProfilePageViewModelInstance.YourPostsCollection.FirstOrDefault(x => x.IdPost == _Globals.OpenID).CommentsCount++;
                    SetCollection();
                    Comment = "";
                    return;
                }
            }
            if (success.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ServerServices.RefreshTokenAsync();
                return;
            }
        }

        // load more command 
        public ICommand LoadMoreCommand { get; set; }
        private void LoadMoreFunction()
        {
            if (IsBusy) { return; }
            IsBusy = true;

            if (CommentsCollection.Count % 10 == 0 && CommentsCollection.Count != 1 && CommentsCollection.Count != 0)
            {
                var success = ServerServices.SendGetRequestAsync($"posts/comment/get/{_Globals.OpenID}/{CommentsCollection.Count / 10}", new JObject()).Result;

                if (success.IsSuccessStatusCode)
                {
                    var earthquakesJson = success.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<List<CommentDB>>(earthquakesJson);

                    foreach (CommentDB comment in rootobject)
                        CommentsCollection.Add(new Comment(comment));
                }
                else if (success.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ServerServices.RefreshTokenAsync().Wait();
                    LoadMoreFunction();
                }
            }

            IsBusy = false;
        }



        // open profile command
        public ICommand OpenProfileCommand { get; set; }
        private async void OpenProfileFunctionAsync(Comment com)
        {
            if (com.IdUser != _Globals.GlobalMainUser.Username)
            {
                _Globals.UsernameTemp = com.IdUser;
                await Shell.Current.GoToAsync("PersonsProfilePage");
                return;
            }

            await Shell.Current.GoToAsync("//ProfilePage");
        }
    }
}
