using BDProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BDProject.Helpers
{
    public class ProfilePostsDataTemplateSelector : DataTemplateSelector 
    {
        public DataTemplate MyPosts { get; set; }
        public DataTemplate MyPostsWithoutDescription { get; set; }

        public DataTemplate OthersPosts { get; set; }
        public DataTemplate OthersPostsWithoutDescription { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            Post post = (Post)item;

            if (post.IdUser == _Globals.GlobalMainUser.Username)
            {
                if(string.IsNullOrEmpty(post.Description) || string.IsNullOrWhiteSpace(post.Description))
                {
                    return MyPostsWithoutDescription;
                }
                else
                {
                    return MyPosts;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(post.Description) || string.IsNullOrWhiteSpace(post.Description))
                {
                    return OthersPostsWithoutDescription;
                }
                else
                {
                    return OthersPosts;
                }
            }
        }
    }
}
