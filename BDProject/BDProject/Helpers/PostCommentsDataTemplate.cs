using BDProject.ModelWrappers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BDProject.Helpers
{
    public class PostCommentsDataTemplate : DataTemplateSelector
    {
        public DataTemplate MyComments { get; set; }

        public DataTemplate OthersComments { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            CommentWrapper comment = (CommentWrapper)item;

            if (_Globals.GlobalMainUser.Username == comment.Username)
            {
                return MyComments;
            }
            else
            {
                return OthersComments;
            }
        }

    }
}
