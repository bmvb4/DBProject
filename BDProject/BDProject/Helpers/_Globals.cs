using BDProject.Models;
using BDProject.ModelWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BDProject.Helpers
{
    public static class _Globals
    {
        // ================= Main User
        private static UserWrapper MainUser = new UserWrapper();
        public static UserWrapper GlobalMainUser
        {
            get => MainUser;
            set => MainUser = value;
        }


        // ================= Other Users
        private static List<UserWrapper> otherUsers = new List<UserWrapper>();
        public static List<UserWrapper> OtherUsers
        {
            get => otherUsers;
            set => otherUsers = value;
        }
        public static void AddUser(UserWrapper user) { otherUsers.Add(user); }
        public static void RemoveUser(UserWrapper user) { otherUsers.RemoveAll(x => x.Username == user.Username); }
        public static UserWrapper GetUser(string username) { return otherUsers.First(x => x.Username == username); }
        public static void AddFollowing(string username) 
        {
            //otherUsers.Where(x => x.Username == username).First().AddFollower(GlobalMainUser.Username);
            for (int i=0; i<otherUsers.Count; i++)
            {
                if (otherUsers[i].Username == username) { otherUsers[i].AddFollower(GlobalMainUser.Username); }
            }
        }
        public static void RemoveFollowing(string username) 
        {
            for (int i = 0; i < otherUsers.Count; i++)
            {
                if (otherUsers[i].Username == username) { otherUsers[i].RemoveFollower(GlobalMainUser.Username); }
            }
        }

        // ================= Main Feed
        private static List<PostWrapper> FeedPosts = new List<PostWrapper>();
        public static List<PostWrapper> GlobalFeedPosts
        {
            get => FeedPosts;
            set => FeedPosts = value;
        }
        public static void AddPost(PostWrapper post) { FeedPosts.Add(post); }
        public static void AddMyPost(PostWrapper post) { FeedPosts.Insert(0, post); }
        public static PostWrapper GetPost(long id)
        {
            try
            {
                return FeedPosts.First(x => x.PostID == id);
            }
            catch (Exception ex)
            {
                // not found
                return new PostWrapper();
            }
        }
        public static void EditPost(PostWrapper post) { FeedPosts.First(x => x.PostID == post.PostID).Description = post.Description; }
        public static void RemovePost(PostWrapper post) { FeedPosts.RemoveAll(x => x.PostID == post.PostID); }
        public static void RemovePost(long id) { FeedPosts.RemoveAll(x => x.PostID == id); }
        public static void AddPostsFromDB(List<PostUser> posts)
        {
            FeedPosts.Clear();
            foreach (PostUser p in posts)
            {
                AddPost(new PostWrapper(p));
            }
        }



        private static long openID = 0;
        public static long OpenID
        {
            get => openID;
            set => openID = value;
        }

        private static bool isBusy = false;
        public static bool IsBusy
        {
            get => isBusy;
            set => isBusy = value;
        }

        private static bool refresh = false;
        public static bool Refresh
        {
            get => refresh;
            set => refresh = value;
        }

        public static List<PostWrapper> FromPostToWrapperList(List<Post> posts)
        {
            List<PostWrapper> list = new List<PostWrapper>();
            foreach (Post p in posts)
            {
                list.Add(new PostWrapper(p));
            }
            return list;
        }
    }
}
