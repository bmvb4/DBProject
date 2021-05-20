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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
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
            IsLiked = post.isLiked;

            CreateDate = post.CreateDate;
            DeleteDate = post.DeleteDate;

            tags = post.tags;
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
        public string DeleteDateString => $"{DeleteDate.ToLocalTime()}";
        



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
        public ImageSource PhotoSource => ImageSource.FromStream(() => new MemoryStream(Photo));



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
        public ImageSource UserPhotoSource => ImageSource.FromStream(() => new MemoryStream(UserPhoto));



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



        private bool isLiked { get; set; } // ========================= LIKES
        public bool IsLiked
        {
            get => isLiked;
            set
            {
                isLiked = value;
                OnPropertyChanged(nameof(LikeIconFamily));
            }
        }
        public string LikeIconFamily => (isLiked) ? "FA-S" : "FA-R";
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



        public List<string> tags { get; set; } // ========================= TAGS
        public List<Tag> Tags
        {
            get
            {
                List<Tag> list = new List<Tag>();

                for (int i=0; i<tags.Count; i++)
                    list.Add(new Tag(tags[i]));

                return list;
            }
            set
            {
                List<Tag> list = value;

                for (int i = 0; i < list.Count; i++)
                    tags.Add(list[i].TagName);

                OnPropertyChanged();
            }
        }
        public ObservableRangeCollection<Tag> TagsCollection => new ObservableRangeCollection<Tag>(Tags);
    }
}
