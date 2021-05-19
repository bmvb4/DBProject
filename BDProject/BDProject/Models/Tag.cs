using BDProject.DatabaseModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace BDProject.Models
{
    public class Tag : INotifyPropertyChanged
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
                Console.Write(ex + "in Tags Class");
            }
        }

        public Tag(TagDB tag)
        {
            TagName = tag.TagName;
        }

        public Tag(string s)
        {
            TagName = s;
        }

        public Tag()
        {

        }



        private string tagName = "";
        public string TagName
        {
            get => tagName;
            set
            {
                tagName = value;
                OnPropertyChanged();
            }
        }
    }
}
