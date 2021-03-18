using BDProject.Views;
using BDProject.Views.PostsViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {

        public HomePageViewModel()
        {
            //========TEST=======
            PostsCollection = new ObservableCollection<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //========TEST=======

            LikesCount = $"{likeCounter}";
            CommentsCount = $"{commentsCounter}";

            // Assigning functions to the commands
            LikePostItemCommand = new Command(LikePostItemFunction);
            OpenPostCommentsItemCommand = new Command(OpenPostCommentsItemFunction);
            SendCommentToPostItemCommand = new Command(SendCommentToPostItemFunction);
        }

        // Parameters
        // Posts Collection parameter
        // ================= zadaden e int za testvane na dizaina
        private ObservableCollection<int> postsCollection = new ObservableCollection<int>();
        public ObservableCollection<int> PostsCollection
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
        //=================================== za testvane
        private string username = "Daniel";
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
        //=================================== za testvane
        private string postDescription = "tova e prosto nqkakvo opisanie za testvane na dizaina";
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

        // Commands
        // Like Post command
        public ICommand LikePostItemCommand { get; set; }
        private void LikePostItemFunction(object user)
        {
            likeCounter++;
            LikesCount = $"{likeCounter}";
        }

        // Send Post Comment command
        public ICommand SendCommentToPostItemCommand { get; set; }
        private void SendCommentToPostItemFunction(object user)
        {
            Comment = "";
            commentsCounter++;
            CommentsCount = $"{commentsCounter}";
        }

        // Open Post Comments command
        public ICommand OpenPostCommentsItemCommand { get; set; }
        private async void OpenPostCommentsItemFunction(object post)
        {
            //if (post == null) { return; }
            //await Shell.Current.GoToAsync($"//PostComments?id={(Post)post.ID}");

            await Shell.Current.GoToAsync("//PostComments");

            //App.Current.MainPage = new PostCommentsPage();
        }

    }
}
