using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels.PostsViewModels
{
    public class PostCommentsPageViewModel : BaseViewModel
    {

        public PostCommentsPageViewModel()
        {
            //========TEST=======
            CommentsCollection = new ObservableCollection<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //========TEST=======

            LikesCount = $"{likeCounter}";
            CommentsCount = $"{commentsCounter}";

            // Assigning functions to the commands
            BackCommand = new Command(async () => await BackFunction());
            LikePostItemCommand = new Command(LikePostItemFunction);
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

        // Commands
        // Back to post command
        public ICommand BackCommand { get; set; }
        private async Task BackFunction()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        // Like Post command
        public ICommand LikePostItemCommand { get; set; }
        private void LikePostItemFunction(object user)
        {
            likeCounter++;
            LikesCount = $"{likeCounter}";
        }

    }
}
