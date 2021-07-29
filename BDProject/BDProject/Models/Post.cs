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
            CommentsCount = post.CommentsCounter;
            IsFollow = post.isFollow;
            IsLiked = post.isLiked;

            CreateDate = post.CreateDate;

            DeleteDate = new DateTime(post.DeleteDate.Year, post.DeleteDate.Month, post.DeleteDate.Day, post.DeleteDate.Hour, post.DeleteDate.Minute, 0, DateTimeKind.Local);
            //DeleteDate = post.DeleteDate;

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
        public string DeleteDateString => $"{DeleteDate.ToLocalTime().AddMinutes(1)}";
        



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

                if(DeleteDate.Year != 1)
                {
                    if(IsLiked)
                        DeleteDate.AddMinutes(-5);
                    else
                        DeleteDate.AddMinutes(5);
                }

                OnPropertyChanged(nameof(DeleteDateString));
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
                if (value < 0) return;
                likesCount = value;
                OnPropertyChanged();
                OnPropertyChanged(LikesCountString);
            }
        }
        public string LikesCountString => FormatNumber(likesCount);


        private int commentsCount;
        public int CommentsCount // ========================= COMMENTS COUNTER
        {
            get => commentsCount;
            set
            {
                if (value < 0) return;
                commentsCount = value;
                OnPropertyChanged();
                OnPropertyChanged(CommentsCountString);
            }
        }
        public string CommentsCountString => FormatNumber(commentsCount);


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



        private static string FormatNumber(int num)
        {
            // Ensure number has max 3 significant digits (no rounding up can happen)
            int i = (int)Math.Pow(10, Math.Max(0, Math.Log10(num) - 2));
            num = num / i * i;

            if (num >= 1000000000) return (num / 1000000000D).ToString("0.##") + "B";
            if (num >= 1000000) return (num / 1000000D).ToString("0.##") + "M";
            if (num >= 1000) return (num / 1000D).ToString("0.##") + "K";
            return num.ToString("#,0");
        }
    }
}
