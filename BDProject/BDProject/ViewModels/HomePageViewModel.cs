using BDProject.Helpers;
using BDProject.ModelWrappers;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {

        private void SetCollection()
        {
            PostsCollection.Clear();
            PostsCollection = new ObservableCollection<PostWrapper>(_Globals.GlobalFeedPosts);
        }

        // setting application defaults
        public HomePageViewModel()
        {            
            SetCollection();

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
            OpenPostCommentsCommand = new Command<PostWrapper>(OpenPostCommentsFunction);
            OpenSearchCommand = new Command(async () => await OpenSearchFunction());
            OpenPersonsProfileCommand = new Command(async () => await OpenPersonsProfileFunction());
            OpenMyProfileCommand= new Command(async () => await OpenMyProfileFunction());

            // follow command
            FollowProfileCommand = new Command<PostWrapper>(FollowProfileFunction);

            // edit post command
            EditPostCommand = new Command<PostWrapper>(EditPostFunction);
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
        public ICommand OpenPostCommentsCommand { get; set; }
        private async void OpenPostCommentsFunction(PostWrapper post)
        {
            _Globals.OpenID = post.FeedID;
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

        // Open my profile command
        public ICommand OpenMyProfileCommand { get; set; }
        private async Task OpenMyProfileFunction()
        {
            await Shell.Current.GoToAsync("//ProfilePage");
        }

        // Follow profile command
        public ICommand FollowProfileCommand { get; set; }
        private void FollowProfileFunction(PostWrapper post)
        {
            if (post.IsFollowed == "Follow")
            {
                _Globals.GlobalMainUser.AddFollowing(post.Username);

                post.IsFollowed = "Following";
                _Globals.EditPost(post);
            }
            else
            {
                _Globals.GlobalMainUser.RemoveFollowing(post.Username);

                post.IsFollowed = "Follow";
                _Globals.EditPost(post);
            }
            PostsCollection[post.FeedID].IsFollowed = post.IsFollowed;
        }

        // Follow profile command
        public ICommand EditPostCommand { get; set; }
        private async void EditPostFunction(PostWrapper post)
        {
            _Globals.OpenID = post.FeedID;
            await Shell.Current.GoToAsync("EditPostPage");
        }
    }
}
