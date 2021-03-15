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
        public PostWrapper(Post post)
        {
            postID = post.IdPost;
            base64Photo = post.Photo;

            //==========TEST
            base64UserPhoto = post.Photo;
            username = "DanielRK";
            description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent mollis ornare luctus. Etiam sed semper odio, ac posuere enim.";
        }

        private int postID;
        public int PostID
        {
            get => postID;
        }

        private string base64Photo;
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
        }


        // user parameters
        private string base64UserPhoto;
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
        }
    }
}
