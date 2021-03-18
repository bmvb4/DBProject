using BDProject.Models;
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
            PostsCollection = new ObservableCollection<PostWrapper>(_Globals.GlobalFeedPosts);
        }

        // setting application defaults
        public HomePageViewModel()
        {            
            SetCollection();

            FollowButtonText = "Follow";
            DaysCount = $"{daysCounter}";
            LikesCount = $"{likeCounter}";
            CommentsCount = $"{commentsCounter}";

            // Assigning functions to the commands
            // refresh command
            RefreshCommand = new Command(async () => await RefreshFunction());
            
            // like commands
            LikePostItemCommand = new Command(LikePostItemFunction);
            DoubleTapLikePostItemCommand = new Command(DoubleTapLikePostItemFunction);
            
            // ========
            SendCommentToPostItemCommand = new Command(SendCommentToPostItemFunction);
            
            // open commands
            OpenPostCommentsItemCommand = new Command(async () => await OpenPostCommentsItemFunction());
            OpenSearchCommand = new Command(async () => await OpenSearchFunction());
            OpenPersonsProfileCommand = new Command(async () => await OpenPersonsProfileFunction());

            // follow command
            FollowProfileCommand= new Command(FollowProfileFunction);

            // edit post command
            EditPostCommand = new Command(async () => await EditPostFunction());
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

        // Days count parameter
        private int daysCounter = 0;
        private string daysCount = "";
        public string DaysCount
        {
            get => daysCount;
            set
            {
                if (value == daysCount) { return; }
                daysCount = value;
                OnPropertyChanged(nameof(DaysCount));
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

        // follow text parameter
        private string followButtonText = "Follow";
        public string FollowButtonText
        {
            get => followButtonText;
            set
            {
                if (value == followButtonText) { return; }
                followButtonText = value;
                OnPropertyChanged(nameof(FollowButtonText));
            }
        }

        // follow text parameter
        private PostWrapper postParameter;
        public PostWrapper PostParameter
        {
            get => postParameter;
            set
            {
                if (value == postParameter) { return; }
                postParameter = value;
                OnPropertyChanged(nameof(PostParameter));
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

        //DubleTapLikePostItemCommand
        public ICommand DoubleTapLikePostItemCommand { get; set; }
        private void DoubleTapLikePostItemFunction()
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

        // Open Search command
        public ICommand OpenSearchCommand { get; set; }
        private async Task OpenSearchFunction()
        {
            await Shell.Current.GoToAsync("SearchPage");
        }

        // Open Persons profile command
        public ICommand OpenPersonsProfileCommand { get; set; }
        private async Task OpenPersonsProfileFunction()
        {
            await Shell.Current.GoToAsync("PersonsProfilePage");
        }

        // Follow profile command
        public ICommand FollowProfileCommand { get; set; }
        private void FollowProfileFunction()
        {
            if (FollowButtonText == "Follow")
            {
                _Globals.GlobalMainUser.AddFollowing(PostParameter.Username);

                FollowButtonText = "Following";
            }
            else
            {
                _Globals.GlobalMainUser.RemoveFollowing(PostParameter.Username);

                FollowButtonText = "Follow";
            }
            //await Shell.Current.GoToAsync("PersonsProfilePage");
        }

        // Follow profile command
        public ICommand EditPostCommand { get; set; }
        private async Task EditPostFunction()
        {
            int id = 2; //==================TEST
            await Shell.Current.GoToAsync($"EditPostPage?PostID={id}");
        }
    }
}
