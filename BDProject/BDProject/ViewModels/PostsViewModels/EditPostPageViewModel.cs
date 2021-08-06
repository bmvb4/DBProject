using BDProject.Helpers;
using BDProject.Models;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDProject.ViewModels.PostsViewModels
{
    public class EditPostPageViewModel : BaseViewModel
    {

        private void SetParameters()
        {
            Post SelectedPost = _Globals.HomePageViewModelInstance.PostsCollection?.FirstOrDefault(x => x.IdPost == _Globals.OpenID);
            idPost = SelectedPost.IdPost;
            idUser = SelectedPost.IdUser;

            PhotoSource = SelectedPost.PhotoSource;
            UserPhotoSource = SelectedPost.UserPhotoSource;
            Username = SelectedPost.IdUser;
            Description = SelectedPost.Description;

            AllTags = new ObservableCollection<Tag>(SelectedPost.Tags);

            _Globals.OpenID = 0;
        }

        private long idPost = 0;
        private string idUser = "";

        public EditPostPageViewModel()
        {
            SetParameters();

            // Assigning functions to the commands
            BackCommand = new Command(async () => await BackFunction());
            SaveChangesCommand = new Command(async () => await SaveChangesFunction());
            DeleteTagCommand = new Command<Tag>(DeleteTagFunction);
            AddTagCommand = new Command(AddTagFunction);
        }

        // Parameters
        // post Image parameter
        private ImageSource photoSource = null;
        public ImageSource PhotoSource
        {
            get => photoSource;
            set
            {
                if (value == photoSource) { return; }
                photoSource = value;
                OnPropertyChanged();
            }
        }

        // User Image parameter
        private ImageSource userPhotoSource = null;
        public ImageSource UserPhotoSource
        {
            get => userPhotoSource;
            set
            {
                if (value == userPhotoSource) { return; }
                userPhotoSource = value;
                OnPropertyChanged();
            }
        }

        // Post description
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

        // Username parameter
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

        // Post Tag
        private string tagText = "";
        public string TagText
        {
            get => tagText;
            set
            {
                if (value == tagText) { return; }
                tagText = value;
                OnPropertyChanged();
            }
        }

        // All tags
        private ObservableCollection<Tag> allTags = new ObservableCollection<Tag>();
        public ObservableCollection<Tag> AllTags
        {
            get => allTags;
            set
            {
                if (value == allTags) { return; }
                allTags = value;
                OnPropertyChanged();
            }
        }

        // Commands
        // Back to post command
        public ICommand BackCommand { get; set; }
        private async Task BackFunction()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        // save changes command
        public ICommand SaveChangesCommand { get; set; }
        private async Task SaveChangesFunction()
        {
            JObject oJsonObject = new JObject();
            oJsonObject.Add("IdPost", idPost);
            oJsonObject.Add("IdUser", idUser);
            oJsonObject.Add("Description", Description);

            var success = await ServerServices.SendPutRequestAsync($"posts/update/{idPost}", oJsonObject);

            if (success.IsSuccessStatusCode)
            {
                _Globals.Refresh = true;
                await Shell.Current.Navigation.PopAsync();

                PhotoSource = null;
            }
        }

        // Delete tag command
        public ICommand DeleteTagCommand { get; set; }
        private void DeleteTagFunction(Tag tag)
        {
            AllTags.Remove(tag);
        }

        // Add tag command
        public ICommand AddTagCommand { get; set; }
        private void AddTagFunction()
        {
            if (!string.IsNullOrEmpty(TagText) || !string.IsNullOrWhiteSpace(TagText))
            {
                AllTags.Add(new Tag(TagText));
                TagText = "";
            }
        }

    }
}
