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
            email = user.Email;
            firstname = user.FirstName;
            lastname = user.LastName;
            description = user.Description;
            imageBytes = user.Photo;
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



        private static List<string> followings = new List<string>();
        public List<string> Followings
        {
            get => followings;
            set => followings = value;
        }
        public void AddFollowing(string username) { followings.Add(username); }
        public void RemoveFollowing(string username) { followings.Remove(username); }
        public bool IsFollowerInside(string username) { return followings.Contains(username); }




        private static List<PostWrapper> myPosts = new List<PostWrapper>();
        public List<PostWrapper> MyPosts
        {
            get => myPosts;
            set => myPosts = value;
        }
        public void AddPost(PostWrapper post) 
        {
            post.MyID = myPosts.Count;
            myPosts.Add(post); 
            myPosts.Sort((x, y) => x.MyID.CompareTo(y.MyID));
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
            myPosts[post.MyID].Description = post.Description;
        }
        public PostWrapper GetPost(int id)
        {
            try
            {
                return myPosts[id];
            }
            catch(Exception ex)
            {
                // not found
                return new PostWrapper();
            }
        }



        private string token;
        public string Token
        {
            get => token;
            set => token = value; 
        }
    }
}
