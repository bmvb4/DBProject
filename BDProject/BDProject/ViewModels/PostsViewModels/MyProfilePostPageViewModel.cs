using BDProject.Helpers;
using BDProject.ModelWrappers;
using BDProject.Views._PopUps;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels.PostsViewModels
{
    public class MyProfilePostPageViewModel : BaseViewModel
    {

        public MyProfilePostPageViewModel()
        {
            post = _Globals.GlobalMainUser.GetPost(_Globals.OpenID);

            BackCommand = new Command(async () => await BackFunction());
            RefreshCommand = new Command(RefreshFunction);
            LikePostCommand = new Command<PostWrapper>(LikePostFunction);
            OpenPostCommentsCommand = new Command<PostWrapper>(OpenPostCommentsFunction);
            MoreCommand = new Command<PostWrapper>(MoreFunction);
        }

        // Parameters
        private PostWrapper post = new PostWrapper();

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
            _Globals.OpenID = 0;
            await Shell.Current.Navigation.PopAsync();
        }

        // Like Post command
        public ICommand LikePostCommand { get; set; }
        private async void LikePostFunction(PostWrapper post)
        {
            JObject oJsonObject = new JObject();
            oJsonObject.Add("idUser", _Globals.GlobalMainUser.Username);
            oJsonObject.Add("idPost", post.PostID);

            if (post.IsLikeUsernameInside(_Globals.GlobalMainUser.Username) == false)
            {
                var success = await ServerServices.SendPostRequestAsync("posts/like", oJsonObject);
                if (success.IsSuccessStatusCode)
                {
                    _Globals.GlobalFeedPosts.First(x => x.PostID == post.PostID).AddLike(new LikeWrapper(_Globals.GlobalMainUser.ImageBytes, _Globals.GlobalMainUser.Username));
                }
            }
            else
            {
                var success = await ServerServices.SendDeleteRequestAsync("posts/unlike", oJsonObject);
                if (success.IsSuccessStatusCode)
                {
                    _Globals.GlobalFeedPosts.First(x => x.PostID == post.PostID).RemoveLike(new LikeWrapper(_Globals.GlobalMainUser.ImageBytes, _Globals.GlobalMainUser.Username));
                }
            }
        }

        // Open Post Comments command
        public ICommand OpenPostCommentsCommand { get; set; }
        private async void OpenPostCommentsFunction(PostWrapper post)
        {
            _Globals.OpenID = (int)post.PostID;
            await Shell.Current.GoToAsync("PostComments");
        }

        // Refresh collection view command
        public ICommand RefreshCommand { get; set; }
        private void RefreshFunction()
        {
            IsRefreshing = true;
            //
            IsRefreshing = false;
        }

        // Edit profile command
        public ICommand MoreCommand { get; set; }
        private async void MoreFunction(PostWrapper post)
        {
            _Globals.OpenID = post.PostID;
            await PopupNavigation.Instance.PushAsync(new PostPopUp());
        }

    }
}
