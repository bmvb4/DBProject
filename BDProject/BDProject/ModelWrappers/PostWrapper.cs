using BDProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace BDProject.ModelWrappers
{
    public class PostWrapper
    {
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
            set => imageBytes = value;
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
            set => description = value;
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
            set => userImageBytes = value;
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
            set => username = value;
        }

        private string isFollowed = "Follow";
        public string IsFollowed
        {
            get => isFollowed;
            set => isFollowed = value;
        }
    }
}
