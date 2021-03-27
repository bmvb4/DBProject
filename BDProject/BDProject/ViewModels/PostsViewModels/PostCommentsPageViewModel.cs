using BDProject.Helpers;
using BDProject.ModelWrappers;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels.PostsViewModels
{
    public class PostCommentsPageViewModel : BaseViewModel
    {

        public void SetParameters()
        {
            PostWrapper SelectedPost = _Globals.GetPost(_Globals.OpenID);

            Username = SelectedPost.Username;
            Description = SelectedPost.Description;
        }

        public void SetCollection()
        {
            PostWrapper SelectedPost = _Globals.GetPost(_Globals.OpenID);
            CommentsCollection.Clear();
            CommentsCollection = new ObservableCollection<CommentWrapper>(SelectedPost.Comments);

            CollectionHeight = 77 * CommentsCollection.Count;
        }

        public PostCommentsPageViewModel()
        {
            SetParameters();
            SetCollection();

            // Assigning functions to the commands
            BackCommand = new Command(async () => await BackFunction());
            //LikePostCommand = new Command(LikePostFunction);
            RefreshCommand = new Command(async () => await RefreshFunction());
            CommentCommand = new Command(CommentFunction);
        }

        // Parameters
        // Posts Collection parameter
        // ================= zadaden e int za testvane na dizaina
        private ObservableCollection<CommentWrapper> commentsCollection = new ObservableCollection<CommentWrapper>();
        public ObservableCollection<CommentWrapper> CommentsCollection
        {
            get => commentsCollection;
            set
            {
                if (value == commentsCollection) { return; }
                commentsCollection = value;
                OnPropertyChanged(nameof(CommentsCollection));
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
        private string description = "";
        public string Description
        {
            get => description;
            set
            {
                if (value == description) { return; }
                description = value;
                OnPropertyChanged(nameof(Description));
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
                OnPropertyChanged(nameof(CollectionHeight));
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
        // Back to post command
        public ICommand BackCommand { get; set; }
        private async Task BackFunction()
        {
            await Shell.Current.Navigation.PopAsync();

            _Globals.OpenID = 0;
        }

        // Like Post command
        /*public ICommand LikePostCommand { get; set; }
        private void LikePostFunction()
        {
            
        }*/

        // Refresh collection view command
        public ICommand RefreshCommand { get; set; }
        private async Task RefreshFunction()
        {
            IsRefreshing = true;
            await Task.Delay(TimeSpan.FromSeconds(1));
            SetCollection();
            IsRefreshing = false;
        }

        public ICommand CommentCommand { get; set; }
        private void CommentFunction()
        {
            if (string.IsNullOrEmpty(Comment) || string.IsNullOrWhiteSpace(Comment)) { return; }
            
            PostWrapper post = _Globals.GetPost(_Globals.OpenID);
            _Globals.GlobalFeedPosts[_Globals.OpenID].AddComment(new CommentWrapper(_Globals.GlobalMainUser.ImageBytes, _Globals.GlobalMainUser.Username, Comment));
            SetCollection();
        }
    }
}
