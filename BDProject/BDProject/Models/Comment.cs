using BDProject.DatabaseModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace BDProject.Models
{
    public class Comment : INotifyPropertyChanged
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
                Console.Write(ex + "in Comment Class");
            }
        }

        public Comment(byte[] imageBytes, string username, string message)
        {
            UserPhoto = imageBytes;
            IdUser = username;
            CommentText = message;
        }

        public Comment(CommentDB comment)
        {
            IdComment = comment.IdComment;
            IdPost = comment.IdPost;
            IdUser = comment.IdUser;
            CommentText = comment.CommentText;
            UserPhoto = comment.UserPhoto;
        }

        public Comment()
        {

        }



        public long IdComment { get; set; } // ====================== COMMENT ID
        public long IdPost { get; set; } // ====================== POST ID
        public string IdUser { get; set; } // ====================== USER ID



        private string commentText = "";
        public string CommentText // ====================== COMMENT TEXT
        {
            get => commentText;
            set
            {
                commentText = value;
                OnPropertyChanged();
            }
        }



        public byte[] UserPhoto { get; set; } // ========================= USER PHOTO
        public ImageSource UserPhotoSource
        {
            get
            {
                return ImageSource.FromStream(() => new MemoryStream(UserPhoto));
            }
        }
    }
}
