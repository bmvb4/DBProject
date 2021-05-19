using BDProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BDProject.Helpers
{
    public class SearcherDataTemplate : DataTemplateSelector
    {
        public DataTemplate Accounts { get; set; }
        public DataTemplate Tags { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            SearchBubble bubble = (SearchBubble)item;

            if (bubble.IsTag == true)
            {
                return Tags;
            }
            else
            {
                return Accounts;
            }
        }
    }
}
