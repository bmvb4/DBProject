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
    public class SearchBubble : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null) { return; }
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public SearchBubble(UserWrapper value)
        {
            user = value;
            imageBytes = value.ImageBytes;
            name = value.Username;

            IsTag = false;
        }

        public SearchBubble(Tag value)
        {
            tag = value;
            name = value.TagName;

            IsTag = true;
        }

        public SearchBubble()
        {

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

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public bool IsTag { get; set; }

        public UserWrapper user { get; set; }
        public Tag tag { get; set; }

    }
}
