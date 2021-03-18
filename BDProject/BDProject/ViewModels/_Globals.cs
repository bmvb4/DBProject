using BDProject.Models;
using BDProject.ModelWrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BDProject.ViewModels
{
    public static class _Globals
    {
        private static UserWrapper MainUser=new UserWrapper();
        public static UserWrapper GlobalMainUser
        {
            get => MainUser;
            set => MainUser = value;
        }

        private static List<PostWrapper> FeedPosts = new List<PostWrapper>();
        public static List<PostWrapper> GlobalFeedPosts
        {
            get
            {
                foreach(PostWrapper p in MainUser.MyPosts)
                {
                    p.PostID = FeedPosts.Count;
                    FeedPosts.Add(p);
                }
                return FeedPosts;
            }
            set => FeedPosts = value;
        }
        public static void AddPost(PostWrapper post) 
        {
            post.PostID = FeedPosts.Count;
            FeedPosts.Add(post); 
        }
    }
}
