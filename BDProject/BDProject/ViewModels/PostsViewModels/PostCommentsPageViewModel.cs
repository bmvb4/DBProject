using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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

            // Assigning functions to the commands
            BackCommand = new Command(BackFunction);
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

        // Commands
        // Back to post command
        public ICommand BackCommand { get; set; }
        private async void BackFunction(object user)
        {
            await Shell.Current.GoToAsync("//HomePage");
        }

    }
}
