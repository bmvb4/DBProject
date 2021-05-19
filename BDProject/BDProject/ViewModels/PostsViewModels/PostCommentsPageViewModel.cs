﻿using BDProject.DatabaseModels;
using BDProject.Helpers;
using BDProject.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public void SetParameters()
        {
            Post SelectedPost = _Globals.GetPost(_Globals.OpenID);

            Username = SelectedPost.IdUser;
            Description = SelectedPost.Description;
        }

        public async void SetCollection()
        {
            JObject oJsonObject = new JObject();
            oJsonObject.Add("Username", _Globals.GlobalMainUser.Username);

            var success = await ServerServices.SendGetRequestAsync($"posts/comment/get/{_Globals.OpenID}", oJsonObject);

            if (success.IsSuccessStatusCode)
            {
                var earthquakesJson = success.Content.ReadAsStringAsync().Result;
                var rootobject = JsonConvert.DeserializeObject<List<CommentDB>>(earthquakesJson);

                List<Comment> list = new List<Comment>();
                foreach (CommentDB comment in rootobject)
                    list.Add(new Comment(comment));

                AllComments = new ObservableRangeCollection<Comment>(list);

                CommentsCollection.Clear();
                if (AllComments.Count >= 20)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        CommentsCollection.Add(AllComments[i]);
                    }
                }
                else
                {
                    CommentsCollection = AllComments;
                }

                CollectionHeight = 77 * CommentsCollection.Count;
            }
            else if (success.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ServerServices.RefreshTokenAsync();
            }
        }

        private ObservableRangeCollection<Comment> AllComments = new ObservableRangeCollection<Comment>();

        public PostCommentsPageViewModel()
        {
            SetParameters();
            SetCollection();

            // Assigning functions to the commands
            BackCommand = new Command(async () => await BackFunction());
            RefreshCommand = new Command(RefreshFunction);
            CommentCommand = new Command(CommentFunction);
            DeleteCommentCommand = new Command<Comment>(DeleteCommentFunction);
            LoadMoreCommand = new Command(async () => await LoadMoreFunction());

            OpenProfileCommand = new Command<Comment>(OpenProfileFunctionAsync);
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

        // Your Posts height parameter
        private double collectionHeight = 0;
        public double CollectionHeight
        {
            get => collectionHeight;
            set
            {
                if (value == collectionHeight) { return; }
                collectionHeight = value;
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
                if (value == isRefreshing) { return; }
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
                _Globals.GlobalFeedPosts.First(x => x.IdPost == _Globals.OpenID).RemoveComment(comment);
                SetCollection();
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
                _Globals.GlobalFeedPosts.First(x => x.IdPost == post.IdPost).AddComment(new Comment(_Globals.GlobalMainUser.Photo, _Globals.GlobalMainUser.Username, Comment));
                SetCollection();
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
            if (_Globals.IsBusy) { return; }
            _Globals.IsBusy = true;

            await Task.Delay(1000);

            if (AllComments.Count - CommentsCollection.Count < 10) 
            {
                CommentsCollection.AddRange(AllComments.Skip(CommentsCollection.Count));
            }
            else
            {
                CommentsCollection.AddRange(AllComments.Skip(CommentsCollection.Count).Take(10));
            }

            _Globals.IsBusy = false;
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
