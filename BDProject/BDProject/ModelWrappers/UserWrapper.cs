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



        private List<string> followings;
        public List<string> Followings
        {
            get => followings;
        }
        public void AddFollowing(string username) { followings.Add(username); }
        public void RemoveFollowing(string username) { followings.Remove(username); }




        private List<PostWrapper> myPosts = new List<PostWrapper>();
        public List<PostWrapper> MyPosts
        {
            get => myPosts;
            set => myPosts = value;
        }
        public void AddPost(PostWrapper post) 
        {
            post.PostID = myPosts.Count;
            myPosts.Add(post); 
        }
        public bool IsInside(PostWrapper post)
        {
            foreach(PostWrapper p in myPosts)
            {
                if (p.Username == post.Username) { return true; }
            }
            // not found
            return false;
        }
        public void EditPost(PostWrapper post)
        {
            myPosts[post.PostID].Description = post.Description;
            /*foreach (PostWrapper p in myPosts)
            {
                if (p.PostID == post.PostID)
                {
                    myPosts[post.PostID] = post;
                    myPosts.Add(post);
                }
            }*/
        }
        public PostWrapper GetPost(int id)
        {
            foreach (PostWrapper p in myPosts)
            {
                if (p.PostID == id)
                {
                    return p;
                }
            }
            // not found
            return new PostWrapper();
        }



        private string token;
        public string Token
        {
            get => token;
            set => token = value; 
        }
    }
}
