using BDProject.Helpers;
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



        private int commentsCount;
        public int CommentsCount
        {
            get => commentsCount;
            set
            {
                commentsCount = value;
                OnPropertyChanged(nameof(CommentsCount));
            }
        }
        private List<CommentWrapper> comments = new List<CommentWrapper>();
        public List<CommentWrapper> Comments
        {
            get => comments;
            set
            {
                comments = value;
                OnPropertyChanged(nameof(Comments));
            }
        }
        public void AddComment(CommentWrapper comment) { comments.Add(comment); CommentsCount = comments.Count; }
        public void RemoveComment(CommentWrapper comment) { comments.Remove(comment); CommentsCount = comments.Count; }
        public bool IsCommentInside(CommentWrapper comment) { return comments.Contains(comment); }



        private int likesCount;
        public int LikesCount
        {
            get => likesCount;
            set
            {
                likesCount = value;
                OnPropertyChanged(nameof(LikesCount));
            }
        }
        private List<LikeWrapper> likes = new List<LikeWrapper>();
        public List<LikeWrapper> Likes
        {
            get => likes;
            set
            {
                likes = value;
                OnPropertyChanged(nameof(Likes));
            }
        }
        public void AddLike(LikeWrapper like) 
        {
            likes.Add(like); 
            LikesCount = likes.Count; 
        }
        public void RemoveLike(LikeWrapper like) 
        {
            int i = 0;
            for (i=0; i<likes.Count; i++)
            {
                if (likes[i].Username == like.Username) { break; }
            }
            likes.RemoveAt(i);
            LikesCount = likes.Count; 
        }
        public bool IsLikeInside(LikeWrapper like) { return likes.Contains(like); }
        public bool IsLikeUsernameInside(string username) 
        { 
            foreach(LikeWrapper like in likes)
            {
                if (like.Username == username)
                {
                    return true;
                }
            }
            return false;
        }



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
