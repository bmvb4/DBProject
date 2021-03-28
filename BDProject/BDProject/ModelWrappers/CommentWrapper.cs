using BDProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace BDProject.ModelWrappers
{
    public class CommentWrapper : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null) { return; }
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public CommentWrapper(byte[] image, string uname, string msg)
        {
            userImageBytes = image;
            username = uname;
            message = msg;
        }

        public CommentWrapper(Comment comment)
        {
            id = comment.IdComment;
            username = comment.IdUser;
            userImageBytes = comment.UserPhoto;
            message = comment.CommentText;
            postID = comment.IdPost;
        }

        public CommentWrapper()
        {

        }

        private long id;
        public long ID
        {
            get => id;
            set => id = value;
        }

        private long postID;
        public long PostID
        {
            get => postID;
            set => postID = value;
        }

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

        private string message;
        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

    }
}
