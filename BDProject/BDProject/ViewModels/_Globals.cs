using BDProject.Models;
using BDProject.ModelWrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BDProject.ViewModels
{
    public static class _Globals
    {
        private static UserWrapper MainUser;
        private static List<PostWrapper> Posts = new List<PostWrapper>();


        public static void SetMainUser(UserWrapper user) { MainUser = user; }
        public static UserWrapper GetMainUser() { return MainUser; }

        public static void AddPost(PostWrapper post) { Posts.Add(post); }
        public static List<PostWrapper> GetPosts() { return Posts; } 
    }
}
