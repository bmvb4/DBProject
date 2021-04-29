using BDProject.Helpers;
using BDProject.Models;
using MvvmHelpers;
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

        public PostWrapper(byte[] Photo, string Descript, string User, byte[] UserPhoto)
        {
            imageBytes = Photo;
            description = Descript;
            username = User;
            userImageBytes = UserPhoto;
        }

        public PostWrapper(PostUser post)
        {
            PostID = post.IdPost;
            Username = post.IdUser;
            ImageBytes = post.Photo;
            UserImageBytes = post.UserPhoto;
            Description = post.Description;
            LikesCount = post.LikesCounter;
            CommentsCount = post.CommentsCounter;

            if (post.isFollow) { Following = "Following"; }
            else { Following = "Follow"; }
        }

        public PostWrapper(Post post)
        {
            postID = post.IdPost;
            imageBytes = post.Photo;
            description = post.Description;
        }

        private long postID;
        public long PostID
        {
            get => postID;
            set => postID = value;
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
        public void RemoveComment(CommentWrapper comment) { comments.RemoveAll(x => x.ID == comment.ID); CommentsCount = comments.Count; }
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
        public void AddLike(LikeWrapper like) { likes.Add(like); LikesCount = likes.Count; }
        public void RemoveLike(LikeWrapper like) { likes.RemoveAll(x => x.Username == like.Username); LikesCount = likes.Count; }
        public bool IsLikeInside(LikeWrapper like) { return likes.Contains(like); }
        public bool IsLikeUsernameInside(string username) { return likes.Exists(x => x.Username == username); }



        private int tagsCount;
        public int TagsCount
        {
            get => tagsCount;
            set
            {
                tagsCount = value;
                OnPropertyChanged(nameof(TagsCount));
            }
        }
        private List<Tag> tags = new List<Tag>();
        public List<Tag> Tags
        {
            get => tags;
            set
            {
                tags = value;
                OnPropertyChanged(nameof(Tags));
            }
        }
        public ObservableRangeCollection<Tag> TagsCollection { get=> new ObservableRangeCollection<Tag>(tags); }
        public void AddTag(Tag tag) { tags.Add(tag); TagsCount = tags.Count; }
        public void RemoveTag(Tag tag) { tags.RemoveAll(x => x.TagName == tag.TagName); TagsCount = tags.Count; }



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
