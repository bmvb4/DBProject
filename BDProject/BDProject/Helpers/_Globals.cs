using BDProject.ModelWrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BDProject.Helpers
{
    public static class _Globals
    {
        private static UserWrapper MainUser = new UserWrapper();
        public static UserWrapper GlobalMainUser
        {
            get => MainUser;
            set => MainUser = value;
        }

        private static List<PostWrapper> FeedPosts = new List<PostWrapper>();
        public static List<PostWrapper> GlobalFeedPosts
        {
            get => FeedPosts;
            set => FeedPosts = value;
        }
        public static void AddPost(PostWrapper post)
        {
            post.FeedID = FeedPosts.Count;
            FeedPosts.Add(post);
            FeedPosts.Sort((x, y) => x.FeedID.CompareTo(y.FeedID));
        }
        public static void AddMyPost(PostWrapper post)
        {
            if (FeedPosts.Count != 0)
            {
                for (int i = FeedPosts.Count - 1; i >= 0; i--)
                {
                    FeedPosts[i].FeedID++;
                }
            }
            post.FeedID = 0;
            FeedPosts.Add(post);
            FeedPosts.Sort((x, y) => x.FeedID.CompareTo(y.FeedID));
        }
        public static PostWrapper GetPost(int id)
        {
            try
            {
                return FeedPosts[id];
            }
            catch (Exception ex)
            {
                // not found
                return new PostWrapper();
            }
        }
        public static void EditPost(PostWrapper post)
        {
            for (int i = 0; i < FeedPosts.Count; i++)
            {
                if (FeedPosts[i].FeedID == post.FeedID)
                {
                    FeedPosts[i].Description = post.Description;

                    FeedPosts[i].IsFollowed = post.IsFollowed;
                }
            }
        }



        private static int openID = 0;
        public static int OpenID
        {
            get => openID;
            set => openID = value;
        }
    }
}
