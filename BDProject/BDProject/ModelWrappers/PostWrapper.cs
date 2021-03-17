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

        public PostWrapper(Post post)
        {
            postID = post.IdPost;
            base64Photo = post.Photo;
            description = post.Description;
        }

        private int postID;
        public int PostID
        {
            get => postID;
            set => postID = value;
        }

        private string base64Photo;
        public string Base64Photo
        {
            get => base64Photo;
            set => base64Photo = value;
        }
        public ImageSource PhotoSource
        {
            get
            {
                return ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(base64Photo)));
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
        private string base64UserPhoto;
        public string Base64UserPhoto
        {
            get => base64UserPhoto;
            set => base64UserPhoto = value;
        }
        public ImageSource UserPhotoSource
        {
            get
            {
                return ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(base64UserPhoto)));
            }
        }

        private string username;
        public string Username
        {
            get => username;
            set => username = value;
        }
    }
}
