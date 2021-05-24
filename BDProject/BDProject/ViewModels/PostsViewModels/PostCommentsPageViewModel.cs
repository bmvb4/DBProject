using BDProject.DatabaseModels;
using BDProject.Helpers;
using BDProject.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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
        public async void SetCollceton()
        {
            CommentsCollection.Clear();

            JObject oJsonObject = new JObject();
            oJsonObject.Add("Username", _Globals.GlobalMainUser.Username);
            var success = await ServerServices.SendGetRequestAsync($"posts/comment/get/{_Globals.OpenID}/0", oJsonObject);

            if (success.IsSuccessStatusCode)
            {
                var earthquakesJson = success.Content.ReadAsStringAsync().Result;
                var rootobject = JsonConvert.DeserializeObject<List<CommentDB>>(earthquakesJson);

                List<Comment> comCollection = new List<Comment>();
                foreach (CommentDB comment in rootobject)
                    comCollection.Add(new Comment(comment));

                CommentsCollection.AddRange(new ObservableRangeCollection<Comment>(comCollection));
            }
            else if (success.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ServerServices.RefreshTokenAsync();
                await LoadMoreFunction();
            }
        }

        public void SetParameters()
        {
            Username = _Globals.GlobalFeedPosts?.First(x => x.IdPost == _Globals.OpenID).IdUser;
            Description = _Globals.GlobalFeedPosts?.First(x => x.IdPost == _Globals.OpenID).Description;
        }

        public PostCommentsPageViewModel()
        {
            IsBusy = true;
            SetCollceton();
            SetParameters();

            // Assigning functions to the commands
            BackCommand = new Command(async () => await BackFunction());
            RefreshCommand = new Command(RefreshFunction);
            CommentCommand = new Command(CommentFunction);
            DeleteCommentCommand = new Command<Comment>(DeleteCommentFunction);
            LoadMoreCommand = new Command(async () => await LoadMoreFunction());

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
            SetCollceton();
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
                _Globals.GlobalFeedPosts.First(x => x.IdPost == _Globals.OpenID).CommentsCount--;
                SetCollceton();
            }
            else if (success.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ServerServices.RefreshTokenAsync();
            }
        }

        // comment command
        public ICommand CommentCommand { get; set; }
        private async void CommentFunction()
        {
            if (string.IsNullOrEmpty(Comment) || string.IsNullOrWhiteSpace(Comment)) { return; }

            Post post = _Globals.GetPost(_Globals.OpenID);

            JObject oJsonObject = new JObject();
            oJsonObject.Add("IdPost", post.IdPost);
            oJsonObject.Add("IdUser", _Globals.GlobalMainUser.Username);
            oJsonObject.Add("CommentText", Comment);

            var success = await ServerServices.SendPostRequestAsync("posts/comment", oJsonObject);
            if (success.IsSuccessStatusCode)
            {
                _Globals.GlobalFeedPosts.First(x => x.IdPost == post.IdPost).CommentsCount++;
                SetCollceton();
                Comment = "";
            }
            else if (success.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ServerServices.RefreshTokenAsync();
            }
        }

        // load more command 
        public ICommand LoadMoreCommand { get; set; }
        private async Task LoadMoreFunction()
        {
            if (IsBusy) { return; }
            IsBusy = true;

            if(CommentsCollection.Count % 10 == 0)
            {
                var success = await ServerServices.SendGetRequestAsync($"posts/comment/get/{_Globals.OpenID}/{(CommentsCollection.Count / 10) + 1}", new JObject());

                if (success.IsSuccessStatusCode)
                {
                    var earthquakesJson = success.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<List<CommentDB>>(earthquakesJson);

                    List<Comment> comCollection = new List<Comment>();
                    foreach (CommentDB comment in rootobject)
                        comCollection.Add(new Comment(comment));

                    CommentsCollection.AddRange(new ObservableRangeCollection<Comment>(comCollection));
                }
                else if (success.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await ServerServices.RefreshTokenAsync();
                    await LoadMoreFunction();
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
            }
            else
                await Shell.Current.GoToAsync("//ProfilePage");
        }
    }
}
