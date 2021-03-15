﻿using BDProject.Models;
using BDProject.ModelWrappers;
using BDProject.Views;
using BDProject.Views.PostsViews;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels
{
    //[QueryProperty(nameof(FName), "FirstName")]
    //[QueryProperty(nameof(LName), "LastName")]
    //[QueryProperty(nameof(UName), "Username")]
    public class HomePageViewModel : BaseViewModel
    {
        // User data object
        //private string fNameData; // users first name
        //public string FName
        //{
        //    get => fNameData;
        //    set
        //    {
        //        fNameData = Uri.UnescapeDataString(value);
        //        OnPropertyChanged(nameof(FName));
        //    }
        //}
        //private string lNameData; // users last name
        //public string LName
        //{
        //    get => lNameData;
        //    set
        //    {
        //        lNameData = Uri.UnescapeDataString(value);
        //        OnPropertyChanged(nameof(LName));
        //    }
        //}
        //private string uNameData; // users username
        //public string UName
        //{
        //    get => uNameData;
        //    set
        //    {
        //        uNameData = Uri.UnescapeDataString(value);
        //        OnPropertyChanged(nameof(UName));
        //    }
        //}

        //private void SetUserData()
        //{
        //    Username = UName;
        //    PostDescription = FName = " " + LName;
        //}

        private void SetCollection()
        {
            PostsCollection.Clear();
            PostsCollection = new ObservableCollection<PostWrapper>(_Globals.GetPosts());
        }

        // setting application defaults
        public HomePageViewModel()
        {
            //========TEST=======
            Username = "Username";
            PostDescription = "this is some description on a post";
            //========TEST=======
            
            SetCollection();

            LikesCount = $"{likeCounter}";
            CommentsCount = $"{commentsCounter}";

            // Assigning functions to the commands
            LikePostItemCommand = new Command(LikePostItemFunction);
            OpenPostCommentsItemCommand = new Command(async () => await OpenPostCommentsItemFunction());
            SendCommentToPostItemCommand = new Command(SendCommentToPostItemFunction);
            RefreshCommand = new Command(async () => await RefreshFunction());
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

        // Likes count parameter
        private int likeCounter = 0;
        private string likesCount = "";
        public string LikesCount
        {
            get => likesCount;
            set
            {
                if (value == likesCount) { return; }
                likesCount = value;
                OnPropertyChanged(nameof(LikesCount));
            }
        }

        // Comments count parameter
        private int commentsCounter = 0;
        private string commentsCount = "";
        public string CommentsCount
        {
            get => commentsCount;
            set
            {
                if (value == commentsCount) { return; }
                commentsCount = value;
                OnPropertyChanged(nameof(CommentsCount));
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

        // Post description parameter
        private string postDescription = "";
        public string PostDescription
        {
            get => postDescription;
            set
            {
                if (value == postDescription) { return; }
                postDescription = value;
                OnPropertyChanged(nameof(PostDescription));
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
                OnPropertyChanged(nameof(Comment));
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
        public ICommand LikePostItemCommand { get; set; }
        private void LikePostItemFunction()
        {
            likeCounter++;
            LikesCount = $"{likeCounter}";
        }

        // Send Post Comment command
        public ICommand SendCommentToPostItemCommand { get; set; }
        private void SendCommentToPostItemFunction()
        {
            Comment = "";
            commentsCounter++;
            CommentsCount = $"{commentsCounter}";
        }

        // Open Post Comments command
        public ICommand OpenPostCommentsItemCommand { get; set; }
        private async Task OpenPostCommentsItemFunction()
        {
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
    }
}
