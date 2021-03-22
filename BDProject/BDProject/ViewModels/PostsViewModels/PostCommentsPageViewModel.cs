using BDProject.Helpers;
using BDProject.ModelWrappers;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels.PostsViewModels
{
    public class PostCommentsPageViewModel : BaseViewModel
    {

        private void SetParameters()
        {
            PostWrapper SelectedPost = _Globals.GetPost(_Globals.OpenID);

            Username = SelectedPost.Username;
            Description = SelectedPost.Description;

            _Globals.OpenID = 0;
        }

        public PostCommentsPageViewModel()
        {
            //========TEST=======
            CommentsCollection = new ObservableCollection<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //========TEST=======

            SetParameters();

            LikesCount = $"{likeCounter}";
            CommentsCount = $"{commentsCounter}";

            // Assigning functions to the commands
            BackCommand = new Command(async () => await BackFunction());
            LikePostCommand = new Command(LikePostFunction);
        }

        // Parameters
        // Posts Collection parameter
        // ================= zadaden e int za testvane na dizaina
        private ObservableCollection<int> commentsCollection = new ObservableCollection<int>();
        public ObservableCollection<int> CommentsCollection
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

        // Commands
        // Back to post command
        public ICommand BackCommand { get; set; }
        private async Task BackFunction()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        // Like Post command
        public ICommand LikePostCommand { get; set; }
        private void LikePostFunction()
        {
            likeCounter++;
            LikesCount = $"{likeCounter}";
        }

    }
}
