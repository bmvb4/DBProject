﻿using BDProject.Helpers;
using BDProject.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BDProject.ViewModels
{
    public class MakePostPageViewModel : BaseViewModel
    {
        private void ClearEveryting()
        {
            TakenPhoto = null;
            Description = "";
            AllTags.Clear();
            AllTagsHeight = 0;
        }

        public MakePostPageViewModel()
        {
            ImageHeight = App.Current.MainPage.Width;
            ImageWidth = App.Current.MainPage.Width / 2;

            // Assigning functions to the commands
            BackCommand = new Command(async () => await BackFunction());
            DeleteTagCommand = new Command<Tag>(DeleteTagFunction);
            TakePhotoCommand = new Command(async () => await TakePhotoFunction());
            PickPhotoCommand = new Command(async () => await PickPhotoFunction());
            PostCommand = new Command(async () => await PostFunction());
        }

        // Parameters
        // Taken Image parameter
        private byte[] imageBytes;
        private ImageSource takenPhoto = null;
        public ImageSource TakenPhoto
        {
            get => takenPhoto;
            set
            {
                if (value == takenPhoto) { return; }
                takenPhoto = value;
                OnPropertyChanged();
            }
        }

        // Image width
        private double imageWidth = 0.0;
        public double ImageWidth
        {
            get => imageWidth;
            set
            {
                if (value == imageWidth) { return; }
                imageWidth = value;
                OnPropertyChanged();
            }
        }

        // Image height
        private double imageHeight = 0.0;
        public double ImageHeight
        {
            get => imageHeight;
            set
            {
                if (value == imageHeight) { return; }
                imageHeight = value;
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

        // All tagsheight
        private int allTagsHeight = 0;
        public int AllTagsHeight
        {
            get => allTagsHeight;
            set
            {
                if (value == allTagsHeight) { return; }
                allTagsHeight = value;
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
            await Shell.Current.GoToAsync("//HomePage");
            ClearEveryting();
        }

        // Back to post command
        public ICommand DeleteTagCommand { get; set; }
        private void DeleteTagFunction(Tag tag)
        {
            AllTags.Remove(tag);
            AllTagsHeight -= 45;
        }

        // Take Photo command
        public ICommand TakePhotoCommand { get; set; }
        private async Task TakePhotoFunction()
        {
            TakenPhoto = null;

            var result = await MediaPicker.CapturePhotoAsync();
            if (result == null) { return; }

            imageBytes = File.ReadAllBytes(result.FullPath);

            TakenPhoto = ImageSource.FromStream(() => new MemoryStream(imageBytes));
        }

        // Pick Photo command
        public ICommand PickPhotoCommand { get; set; }
        private async Task PickPhotoFunction()
        {
            TakenPhoto = null;

            var result = await MediaPicker.PickPhotoAsync();
            if (result == null) { return; }

            imageBytes = File.ReadAllBytes(result.FullPath);

            TakenPhoto = ImageSource.FromFile(result.FullPath);
        }

        // post command
        public ICommand PostCommand { get; set; }
        private async Task PostFunction()
        {
            if (TakenPhoto == null) 
            { 
                await App.Current.MainPage.DisplayAlert("Warning!", "You can't continue without a photo", "OK");
                return;
            }
            else
            {
                List<string> tags = new List<string>();
                foreach (Tag t in AllTags)
                {
                    tags.Add(t.TagName);
                }

                //=================================ТЕСТ
                //for(int i=0; i<50; i++)
                //{
                //    Description = $"Test{i}";

                //    JObject oJsonObject = new JObject();
                //    oJsonObject.Add("IdUser", _Globals.GlobalMainUser.Username);
                //    oJsonObject.Add("Photo", imageBytes);
                //    oJsonObject.Add("Description", Description);
                //    oJsonObject.Add("tags", JToken.FromObject(tags));

                //    var success = await ServerServices.SendPostRequestAsync("posts/post", oJsonObject);

                //    if (success.IsSuccessStatusCode)
                //    {
                //        PostWrapper post = new PostWrapper(imageBytes, Description, _Globals.GlobalMainUser.Username, _Globals.GlobalMainUser.ImageBytes);
                //        foreach (string t in tags)
                //        {
                //            post.AddTag(new Tag(t));
                //        }

                //        _Globals.GlobalMainUser.AddPost(post);
                //        _Globals.AddMyPost(post);

                //        _Globals.Refresh = true;
                //        await Shell.Current.GoToAsync("//HomePage");
                //    }
                //    else if (success.StatusCode == HttpStatusCode.Unauthorized)
                //    {
                //        await ServerServices.RefreshTokenAsync();
                //    }
                //}
                //=================================ТЕСТ

                JObject oJsonObject = new JObject();
                oJsonObject.Add("IdUser", _Globals.GlobalMainUser.Username);
                oJsonObject.Add("Photo", imageBytes);
                oJsonObject.Add("Description", Description);
                oJsonObject.Add("tags", JToken.FromObject(tags));

                var success = await ServerServices.SendPostRequestAsync("posts/post", oJsonObject);

                if (success.IsSuccessStatusCode)
                {
                    _Globals.Refresh = true;
                    await Shell.Current.GoToAsync("//HomePage");
                }
                else if (success.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await ServerServices.RefreshTokenAsync();
                }

                ClearEveryting();
            }
        }

    }
}
