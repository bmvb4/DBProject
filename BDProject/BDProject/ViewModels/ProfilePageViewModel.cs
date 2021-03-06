using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels
{
    public class ProfilePageViewModel : BaseViewModel
    {

        public ProfilePageViewModel()
        {
            //========TEST=======
            YourPostsCollection = new ObservableCollection<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //========TEST=======

            this.SetColectionHeight();

            // Assigning functions to the commands
            OpenSettingsCommand = new Command(OpenSettingsFunction);
        }

        // Parameters
        // Your Posts Collection parameter
        // ================= zadaden e int za testvane na dizaina
        private ObservableCollection<int> yourPostsCollection = new ObservableCollection<int>();
        public ObservableCollection<int> YourPostsCollection
        {
            get => yourPostsCollection;
            set
            {
                if (value == yourPostsCollection) { return; }
                yourPostsCollection = value;
                OnPropertyChanged(nameof(YourPostsCollection));
            }
        }

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

        private double postHeight = 0;
        public double PostHeight
        {
            get => postHeight;
            set
            {
                if (value == postHeight) { return; }
                postHeight = value;
                OnPropertyChanged(nameof(PostHeight));
            }
        }

        // Commands PostHeight
        // Back to post command
        public ICommand OpenSettingsCommand { get; set; }
        private async void OpenSettingsFunction(object user)
        {
            await Shell.Current.GoToAsync("//SettingsPage");
        }

        // Functions
        // Set Collceton height function
        private void SetColectionHeight()
        {
            PostHeight = App.Current.MainPage.Width * 0.6;

            if (YourPostsCollection.Count % 3 == 0)
            {
                CollectionHeight = PostHeight * (YourPostsCollection.Count / 3);
            }
            else
            {
                CollectionHeight = PostHeight * (Math.Ceiling(((double)YourPostsCollection.Count) / 3.0));
            }
        }

    }
}
