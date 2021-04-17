using BDProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace BDProject.ModelWrappers
{
    public class UserWrapper : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null) { return; }
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public UserWrapper()
        {

        }

        public UserWrapper(User user)
        {
            username = user.Username;
            email = user.Email;
            firstname = user.FirstName;
            lastname = user.LastName;
            description = user.Description;
            imageBytes = user.Photo;
            accessToken = user.AccessToken;
            refreshToken = user.RefreshToken;
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

        private string email;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string firstname;
        public string FirstName
        {
            get => firstname;
            set
            {
                firstname = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string lastname;
        public string LastName
        {
            get => lastname;
            set
            {
                lastname = value;
                OnPropertyChanged(nameof(LastName));
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



        private List<string> followings = new List<string>();
        public List<string> Followings
        {
            get => followings;
            set
            {
                followings = value;
                OnPropertyChanged(nameof(Followings));
            }
        }
        public void AddFollowing(string username) { followings.Add(username); }
        public void RemoveFollowing(string username) { followings.Remove(username); }
        public bool IsFollowingInside(string username) { return followings.Contains(username); }



        private List<string> followers = new List<string>();
        public List<string> Followers
        {
            get => followers;
            set
            { 
                followers = value;
                OnPropertyChanged(nameof(Followers));
            }
        }
        public void AddFollower(string username) { followers.Add(username); }
        public void RemoveFollower(string username) { followers.Remove(username); }
        public bool IsFollowerInside(string username) { return followers.Contains(username); }



        private List<PostWrapper> myPosts = new List<PostWrapper>();
        public List<PostWrapper> MyPosts
        {
            get => myPosts;
            set
            {
                myPosts = value;
                OnPropertyChanged(nameof(MyPosts));
            }
        }
        public void AddPost(PostWrapper post) 
        {
            myPosts.Insert(0, post); 
        }
        public bool IsInside(PostWrapper post)
        {
            return myPosts.Any(x => x.Username == post.Username);
        }
        public void EditPost(PostWrapper post)
        {
            myPosts.First(x => x.PostID==post.PostID).Description = post.Description;
        }
        public PostWrapper GetPost(long id)
        {
            try
            {
                return myPosts.First(x => x.PostID == id);
            }
            catch(Exception ex)
            {
                // not found
                return new PostWrapper();
            }
        }
        public void RemovePost(long id) { myPosts.RemoveAll(x => x.PostID == id); }
        public void AddPostsFromDB(List<PostUser> posts)
        {
            foreach(PostUser p in posts)
            {
                AddPost(new PostWrapper(p));
            }
        }



        private string accessToken;
        public string AccessToken
        {
            get => accessToken;
            set => accessToken = value;
        }

        private string refreshToken;
        public string RefreshToken
        {
            get => refreshToken;
            set => refreshToken = value; 
        }
    }
}
