using BDProject.DatabaseModels;
using System.ComponentModel;
using System.IO;
using Xamarin.Forms;

namespace BDProject.Models
{
    public class SearchBubble : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null) { return; }
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public SearchBubble(UserDB value)
        {
            imageBytes = value.Photo;
            name = value.Username;

            IsTag = false;
        }

        public SearchBubble(Tag value)
        {
            name = value.TagName;

            IsTag = true;
        }

        public SearchBubble(string value)
        {
            name = value;

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

    }
}
