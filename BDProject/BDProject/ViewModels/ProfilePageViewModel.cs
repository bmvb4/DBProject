using BDProject.Helpers;
using BDProject.Models;
using BDProject.Views._PopUps;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace BDProject.ViewModels
{
    public class ProfilePageViewModel : BaseViewModel
    {
        public void SetUserData()
        {
            User user = _Globals.GlobalMainUser;

            try
            {
                Name = $"{user.FirstName} {user.LastName}";
                Username = $"({user.Username})";
                Description = user.Description;
                if (user.Photo == null)
                    user.Photo = Convert.FromBase64String(_Globals.Base64DefaultPhoto);

                ProfilePictureSource = user.PhotoSource;

                FollowingCount = _Globals.GlobalMainUser.FollowingsCount;
                FollowersCount = _Globals.GlobalMainUser.FollowersCount;

                AllPostsCollection = new ObservableRangeCollection<Post>(_Globals.GlobalMainUser.Posts);
                PostsCount = AllPostsCollection.Count;

                YourPostsCollection.Clear();
                if (AllPostsCollection.Count - YourPostsCollection.Count < 3 * 10)
                {
                    YourPostsCollection.AddRange(AllPostsCollection);
                }
                else
                {
                    YourPostsCollection.AddRange(AllPostsCollection.Take(3 * 10));
                }
            }
            catch(Exception ex)
            {
                
            }
        }

        public ProfilePageViewModel()
        {
            SetUserData();

            // Assigning functions to the commands
            RefreshCommand = new Command(RefreshFunction);

            // like commands
            LikePostCommand = new Command<Post>(LikePostFunction);
            SearchTagCommand = new Command<Tag>(SearchTagFunction);
            ShowTagsCommand = new Command(ShowTagsFunction);

            // open commands
            OpenSettingsCommand = new Command(async () => await OpenSettingsFunction());
            OpenEditProfileCommand = new Command(async () => await OpenEditProfileFunction());
            OpenPostCommentsCommand = new Command<Post>(OpenPostCommentsFunction);
            OpenTagsCommand = new Command<Post>(OpenTagsFunction);

            // More command
            MoreCommand = new Command<Post>(MoreFunction);
            LoadMoreCommand = new Command(async () => await LoadMoreFunction());
        }

        private ObservableRangeCollection<Post> AllPostsCollection = new ObservableRangeCollection<Post>();

        // Parameters
        // Your Posts Collection parameter
        private ObservableRangeCollection<Post> yourPostsCollection = new ObservableRangeCollection<Post>();
        public ObservableRangeCollection<Post> YourPostsCollection
        {
            get => yourPostsCollection;
            set
            {
                if (value == yourPostsCollection) { return; }
                yourPostsCollection = value;
                OnPropertyChanged();
            }
        }

        // Your Posts Collection height parameter
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

        // Your Posts height parameter
        private double postHeight = 0;
        public double PostHeight
        {
            get => postHeight;
            set
            {
                if (value == postHeight) { return; }
                postHeight = value;
                OnPropertyChanged();
            }
        }

        private ImageSource profilePictureSource;
        public ImageSource ProfilePictureSource
        {
            get => profilePictureSource;
            set
            {
                if (value == profilePictureSource) { return; }
                profilePictureSource = value;
                OnPropertyChanged();
            }
        }

        // Your Name parameter
        private string name = "";
        public string Name
        {
            get => name;
            set
            {
                if (value == name) { return; }
                name = value;
                OnPropertyChanged();
            }
        }

        // Your username parameter
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

        // Your Description parameter
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

        // Your following count parameter
        private int postsCount = 0;
        public int PostsCount
        {
            get => postsCount;
            set
            {
                if (value == postsCount) { return; }
                postsCount = value;
                OnPropertyChanged();
            }
        }

        // Your followers count parameter
        private int followersCount = 0;
        public int FollowersCount
        {
            get => followersCount;
            set
            {
                if (value == followersCount) { return; }
                followersCount = value;
                OnPropertyChanged();
            }
        }

        // Your following count parameter
        private int followingCount = 0;
        public int FollowingCount
        {
            get => followingCount;
            set
            {
                if (value == followingCount) { return; }
                followingCount = value;
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
                //if (value == isRefreshing) { return; }
                isRefreshing = value;
                OnPropertyChanged();
            }
        }

        // Refreshing parameter
        private bool isTagsVisible = false;
        public bool IsTagsVisible
        {
            get => isTagsVisible;
            set
            {
                if (value == isTagsVisible) { return; }
                isTagsVisible = value;
                OnPropertyChanged();
            }
        }

        // Commands PostHeight
        // Settings command
        public ICommand OpenSettingsCommand { get; set; }
        private async Task OpenSettingsFunction()
        {
            await Shell.Current.GoToAsync("SettingsPage");
        }

        // Edit profile command
        public ICommand OpenEditProfileCommand { get; set; }
        private async Task OpenEditProfileFunction()
        {
            await Shell.Current.GoToAsync("EditProfilePage");
        }

        // Refresh collection view command
        public ICommand RefreshCommand { get; set; }
        public void RefreshFunction()
        {
            IsRefreshing = true;
            SetUserData();
            IsRefreshing = false;
        }

        // load more command 
        public ICommand LoadMoreCommand { get; set; }
        private async Task LoadMoreFunction()
        {
            if (_Globals.IsBusy) { return; }
            _Globals.IsBusy = true;

            await Task.Delay(1000);

            if (AllPostsCollection.Count - YourPostsCollection.Count < 3 * 10)
            {
                YourPostsCollection.AddRange(AllPostsCollection.Skip(YourPostsCollection.Count));
            }
            else
            {
                YourPostsCollection.AddRange(AllPostsCollection.Skip(YourPostsCollection.Count).Take(3 * 10));
            }

            _Globals.IsBusy = false;
        }

        // Like Post command
        public ICommand LikePostCommand { get; set; }
        private async void LikePostFunction(Post post)
        {
            JObject oJsonObject = new JObject();
            oJsonObject.Add("idUser", post.IdUser);
            oJsonObject.Add("idPost", post.IdPost);

            if (!post.IsLiked)
            {
                var success = await ServerServices.SendPostRequestAsync("posts/like", oJsonObject);
                if (success.IsSuccessStatusCode)
                {
                    post.IsLiked = true;
                    post.LikesCount++;
                }
                else if (success.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await ServerServices.RefreshTokenAsync();
                }
            }
            else
            {
                var success = await ServerServices.SendDeleteRequestAsync("posts/unlike", oJsonObject);
                if (success.IsSuccessStatusCode)
                {
                    post.IsLiked = false;
                    post.LikesCount--;
                }
                else if (success.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await ServerServices.RefreshTokenAsync();
                }
            }
        }

        // Open Post Comments command
        public ICommand OpenPostCommentsCommand { get; set; }
        private async void OpenPostCommentsFunction(Post post)
        {
            _Globals.OpenID = (int)post.IdPost;
            await Shell.Current.GoToAsync("PostComments");
        }

        // Edit profile command
        public ICommand MoreCommand { get; set; }
        private async void MoreFunction(Post post)
        {
            _Globals.OpenID = post.IdPost;
            await PopupNavigation.Instance.PushAsync(new PostPopUp());
        }

        public ICommand SearchTagCommand { get; set; }
        private async void SearchTagFunction(Tag tag)
        {
            await Task.Delay(1000);
        }

        public ICommand ShowTagsCommand { get; set; }
        private async void ShowTagsFunction()
        {
            await Task.Delay(0);
            if (IsTagsVisible)
                IsTagsVisible = false;
            else
                IsTagsVisible = true;
        }

        public ICommand OpenTagsCommand { get; set; }
        private async void OpenTagsFunction(Post post)
        {
            if (post.tags != null && post.tags.Count != 0)
                await PopupNavigation.Instance.PushAsync(new AllTagsPopUp(post.TagsCollection));
            else
                await PopupNavigation.Instance.PushAsync(new AllTagsPopUp(new ObservableRangeCollection<Tag>()));
        }

    }
}
