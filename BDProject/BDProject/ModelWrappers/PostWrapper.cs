using BDProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace BDProject.ModelWrappers
{
    public class PostWrapper : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null) { return; }
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public PostWrapper()
        {

        }

        public PostWrapper(int Id, byte[] Photo, string Description, string User, byte[] UserPhoto)
        {
            feedid = Id;
            imageBytes = Photo;
            description = Description;
            username = User;
            userImageBytes = UserPhoto;
        }

        public PostWrapper(Post post, string User, byte[] UserPhoto)
        {
            feedid = post.IdPost;
            imageBytes = post.Photo;
            description = post.Description;
            username = User;
            userImageBytes = UserPhoto;
        }

        public PostWrapper(Post post)
        {
            feedid = post.IdPost;
            imageBytes = post.Photo;
            description = post.Description;
        }

        private int feedid;
        public int FeedID
        {
            get => feedid;
            set => feedid = value;
        }

        private int myid;
        public int MyID
        {
            get => myid;
            set => myid = value;
        }

        private byte[] imageBytes;
        public byte[] ImageBytes
        {
            get => imageBytes;
            set
            {
                imageBytes = value;
                OnPropertyChanged(nameof(ImageBytes));
            }
        }
        public ImageSource PhotoSource
        {
            get
            {
                return ImageSource.FromStream(() => new MemoryStream(imageBytes));
            }
        }

        private string description;
        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }


        //private List<Comments> comments=new List<Comments>();
        //public List<Comments> Comments{
        // get=>comments;
        // set=>comments=value;
        //}


        // user parameters
        private byte[] userImageBytes;
        public byte[] UserImageBytes
        {
            get => userImageBytes;
            set
            {
                userImageBytes = value;
                OnPropertyChanged(nameof(UserImageBytes));
            }
        }
        public ImageSource UserPhotoSource
        {
            get
            {
                return ImageSource.FromStream(() => new MemoryStream(userImageBytes));
            }
        }

        private string username;
        public string Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }



        private string following = "Follow";
        public string Following
        {
            get => following;
            set
            {
                following = value;
                OnPropertyChanged(nameof(Following));
            }
        }
    }
}
