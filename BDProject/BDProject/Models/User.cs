using BDProject.DatabaseModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace BDProject.Models
{
    public class User : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
            catch (Exception ex)
            {
                Console.Write(ex + "in User Class");
            }
        }

        public User(ProfileDB user)
        {
            Username = user.Username;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Description = user.Description;
            Photo = user.Photo;

            AddPostsFromDB(user.Posts);

            FollowingsCount = user.Follower;
            FollowersCount = user.Followed;
        }

        public User(UserDB user)
        {
            Username = user.Username;
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Description = user.Description;
            Photo = user.Photo;

            AccessToken = user.AccessToken;
            RefreshToken = user.RefreshToken;
        }

        public User()
        {

        }



        private string username;
        public string Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged();
            }
        }



        private string email;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }



        private string firstname;
        public string FirstName
        {
            get => firstname;
            set
            {
                firstname = value;
                OnPropertyChanged();
            }
        }



        private string lastname;
        public string LastName
        {
            get => lastname;
            set
            {
                lastname = value;
                OnPropertyChanged();
            }
        }



        private string description;
        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }



        private byte[] photo;
        public byte[] Photo
        {
            get => photo;
            set
            {
                photo = value;
                OnPropertyChanged();
            }
        }
        public ImageSource PhotoSource
        {
            get
            {
                return ImageSource.FromStream(() => new MemoryStream(Photo));
            }
        }



        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }



        private int followingsCount = 0;
        public int FollowingsCount
        {
            get => followingsCount;
            set
            {
                followingsCount = value;
                OnPropertyChanged();
            }
        }
        private List<string> followings = new List<string>();
        public List<string> Followings
        {
            get => followings;
            set
            {
                followings = value;
                OnPropertyChanged();
            }
        }
        public void AddFollowing(string username) { followings.Add(username); FollowingsCount++; }
        public void RemoveFollowing(string username) { followings.Remove(username); FollowingsCount--; }
        public bool IsFollowingInside(string username) { return followings.Contains(username); }



        private int followersCount = 0;
        public int FollowersCount
        {
            get => followersCount;
            set
            {
                followersCount = value;
                OnPropertyChanged();
            }
        }
        private List<string> followers = new List<string>();
        public List<string> Followers
        {
            get => followers;
            set
            {
                followers = value;
                OnPropertyChanged();
            }
        }
        public void AddFollower(string username) { followers.Add(username); FollowersCount++; }
        public void RemoveFollower(string username) { followers.Remove(username); FollowersCount--; }
        public bool IsFollowerInside(string username) { return followers.Contains(username); }



        private List<Post> posts = new List<Post>();
        public List<Post> Posts
        {
            get => posts;
            set
            {
                posts = value;
                OnPropertyChanged();
            }
        }
        public void AddPost(Post post) { posts.Insert(0, post); }
        public bool IsInside(Post post) { return posts.Any(x => x.IdUser == post.IdUser); }
        public void EditPost(Post post) { posts.First(x => x.IdPost == post.IdPost).Description = post.Description; }
        public Post GetPost(long id)
        {
            try
            {
                return posts.First(x => x.IdPost == id);
            }
            catch (Exception ex)
            {
                // not found
                return new Post();
            }
        }
        public void RemovePost(long id) { posts.RemoveAll(x => x.IdPost == id); }
        public void AddPostsFromDB(List<PostDB> posts)
        {
            foreach (PostDB post in posts)
                Posts.Add(new Post(post));
        }
        public void AddPostsFromDB(List<BigPostDB> posts)
        {
            foreach (BigPostDB post in posts)
                Posts.Add(new Post(post));
        }
    }
}
