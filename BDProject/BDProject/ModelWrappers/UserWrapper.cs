using BDProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace BDProject.ModelWrappers
{
    public class UserWrapper
    {
        public UserWrapper()
        {

        }

        public UserWrapper(User user)
        {
            id = user.IdUser;
            username = user.Username;
            password = user.Password;
            email = user.Email;
            firstname = user.FirstName;
            lastname = user.LastName;
            description = user.Description;
            base64Photo = user.Photo;
            token = user.token;
        }

        private int id;
        public int ID
        {
            get => id;
            set => id = value; 
        }

        private string username;
        public string Username
        {
            get => username;
            set => username = value; 
        }

        private string password;
        public string Password
        {
            get => password;
            set => password = value; 
        }

        private string email;
        public string Email
        {
            get => email;
            set => email = value; 
        }

        private string firstname;
        public string FirstName
        {
            get => firstname;
            set => firstname = value;
        }

        private string lastname;
        public string LastName
        {
            get => lastname;
            set => lastname = value;
        }

        private string description;
        public string Description
        {
            get => description;
            set => description = value;
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



        private List<UserWrapper> followings;
        public List<UserWrapper> Followings
        {
            get => followings;
        }
        public void AddFollowing(UserWrapper user) { followings.Add(user); }



        private List<PostWrapper> myPosts = new List<PostWrapper>();
        public List<PostWrapper> MyPosts
        {
            get => myPosts;
        }
        public void AddPost(PostWrapper post) { myPosts.Add(post); }
        public bool IsInside(PostWrapper post)
        {
            foreach(PostWrapper p in myPosts)
            {
                if (p.Username == post.Username) { return true; }
            }
            return false;
        }



        private string token;
        public string Token
        {
            get => token;
            set => token = value; 
        }
    }
}
