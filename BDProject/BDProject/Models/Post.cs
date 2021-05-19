using BDProject.DatabaseModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace BDProject.Models
{
    public class Post : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
            catch(Exception ex)
            {
                Console.Write(ex + "in Post Class");
            }
        }

        public Post(BigPostDB post)
        {
            IdPost = post.IdPost;
            IdUser = post.IdUser;
            Photo = post.Photo;
            UserPhoto = post.UserPhoto;
            Description = post.Description;
            LikesCount = post.LikesCounter;
            commentsCount = post.CommentsCounter;
            IsFollow = post.isFollow;
            isLiked = post.isLiked;

            CreateDate = post.CreateDate;
            DeleteDate = post.DeleteDate;
        }

        public Post(PostDB post)
        {
            IdPost = post.IdPost;
            IdUser = post.IdUser;
            Photo = post.Photo;
            Description = post.Description;
            tags = post.tags;
        }

        public Post()
        {

        }



        public long IdPost { get; set; } // ========================= POST ID
        public string IdUser { get; set; } // ========================= USERNAME



        public DateTime CreateDate { get; set; } // ========================= CREATED DATE
        public DateTime DeleteDate { get; set; } // ========================= DELETE DATE
        public string DeleteDateString 
        { get
            {
                return $"{DeleteDate.ToLocalTime()}";
            } 
        }
        



        private byte[] photo;
        public byte[] Photo // ========================= POST PHOTO
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



        private byte[] userPhoto;
        public byte[] UserPhoto // ========================= USER PHOTO
        {
            get => userPhoto;
            set
            {
                userPhoto = value;
                OnPropertyChanged();
            }
        }
        public ImageSource UserPhotoSource
        {
            get
            {
                return ImageSource.FromStream(() => new MemoryStream(UserPhoto));
            }
        }



        private string description = "";
        public string Description // ========================= DESCRIPTION
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }



        public bool isLiked { get; set; } // ========================= LIKES
        private int likesCount;
        public int LikesCount // ========================= LIKES COUNTER
        {
            get => likesCount;
            set
            {
                likesCount = value;
                OnPropertyChanged(nameof(LikesCount));
            }
        }
        private List<Like> likes = new List<Like>();
        public List<Like> Likes
        {
            get => likes;
            set
            {
                likes = value;
                OnPropertyChanged();
            }
        }
        public void AddLike(Like like) { likes.Add(like); LikesCount = likes.Count; }
        public void RemoveLike(Like like) { likes.RemoveAll(x => x.Username == like.Username); LikesCount = likes.Count; }
        //public void AddLike(LikeWrapper like) { likes.Add(like); LikesCount = likes.Count; }
        //public void RemoveLike(LikeWrapper like) { likes.RemoveAll(x => x.Username == like.Username); LikesCount = likes.Count; }



        private int commentsCount;
        public int CommentsCount // ========================= COMMENTS COUNTER
        {
            get => commentsCount;
            set
            {
                commentsCount = value;
                OnPropertyChanged();
            }
        }
        private List<Comment> comments = new List<Comment>();
        public List<Comment> Comments
        {
            get => comments;
            set
            {
                comments = value;
                OnPropertyChanged();
            }
        }
        public void AddComment(Comment comment) { comments.Add(comment); CommentsCount = comments.Count; }
        public void RemoveComment(Comment comment) { comments.RemoveAll(x => x.IdComment == comment.IdComment); CommentsCount = comments.Count; }
        //public void AddComment(Comment comment) { comments.Add(comment); CommentsCount = comments.Count; }
        //public void RemoveComment(Comment comment) { comments.RemoveAll(x => x.ID == comment.ID); CommentsCount = comments.Count; }



        private bool isFollow { get; set; } // ========================= FOLLOWINGS
        public bool IsFollow
        {
            get => isFollow;
            set
            {
                isFollow = value;

                if (isFollow)
                    IsFollowString = "Following";
                else
                    IsFollowString = "Follow";
            }
        }
        private string isFollowString = "";
        public string IsFollowString
        {
            get => isFollowString;
            set
            {
                isFollowString = value;
                OnPropertyChanged();
            }
        }



        public List<string> tags { get; set; } // ========================= TAGS
        public List<Tag> Tags
        {
            get
            {
                List<Tag> list = new List<Tag>();

                foreach (string s in tags)
                    list.Add(new Tag(s));

                return list;
            }
            set
            {
                List<Tag> list = value;

                foreach (Tag t in list)
                    tags.Add(t.TagName);

                OnPropertyChanged();
            }
        }
        public ObservableRangeCollection<Tag> TagsCollection { get => new ObservableRangeCollection<Tag>(Tags); }
        public void AddTag(Tag tag) { tags.Add(tag.TagName); }
        public void RemoveTag(Tag tag) { tags.RemoveAll(x => x == tag.TagName); }
    }
}
