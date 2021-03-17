using BDProject.ModelWrappers;
using BDProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BDProject.Helpers
{
    public class ProfilePostsDataTemplateSelector : DataTemplateSelector 
    {
        public DataTemplate MyPosts { get; set; }

        public DataTemplate OthersPosts { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            PostWrapper post = (PostWrapper)item;

            if (_Globals.GlobalMainUser.IsInside(post) == true)
            {
                return MyPosts;
            }
            else
            {
                return OthersPosts;
            }
        }
    }
}
