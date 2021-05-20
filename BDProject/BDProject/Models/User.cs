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
        public ImageSource PhotoSource => ImageSource.FromStream(() => new MemoryStream(Photo));



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

        private bool isFollow { get; set; } // ========================= FOLLOWINGS
        public bool IsFollow
        {
            get => isFollow;
            set
            {
                isFollow = value;
                OnPropertyChanged(nameof(IsFollowString));
            }
        }
        public string IsFollowString => (isFollow) ? "Following" : "Follow";


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
        public Post GetPost(long id)
        {
            Post post = posts?.First(x => x.IdPost == id);
            return (post != null) ? post : new Post();
        }
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
