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
        public static void AddUser(UserWrapper user)
        {
            otherUsers.Add(user);
        }
        public static void RemoveUser(UserWrapper user)
        {
            int i = 0;
            for (i=0; i< otherUsers.Count; i++)
            {
                if (otherUsers[i].Username == user.Username) { break; }
            }
            otherUsers.RemoveAt(i);
        }
        public static UserWrapper GetUser(string username)
        {
            foreach(UserWrapper user in otherUsers)
            {
                if (user.Username == username) { return user; }
            }
            return new UserWrapper();
        }
        public static void SetFollowing(string username)
        {
            for(int i=0; i<otherUsers.Count; i++)
            {
                if (otherUsers[i].Username == username)
                {
                    otherUsers[i].AddFollower(GlobalMainUser.Username);
                }
            }
        }
        public static void UndoFollowing(string username)
        {
            for (int i = 0; i < otherUsers.Count; i++)
            {
                if (otherUsers[i].Username == username)
                {
                    otherUsers[i].RemoveFollower(GlobalMainUser.Username);
                }
            }
        }

        // ================= Main Feed
        private static List<PostWrapper> FeedPosts = new List<PostWrapper>();
        public static List<PostWrapper> GlobalFeedPosts
        {
            get => FeedPosts;
            set => FeedPosts = value;
        }
        public static void AddPost(PostWrapper post)
        {
            FeedPosts.Add(post);
        }
        public static void AddMyPost(PostWrapper post)
        {
            FeedPosts.Insert(0, post);
        }
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
        public static void EditPost(PostWrapper post)
        {
            FeedPosts.First(x => x.PostID == post.PostID).Description = post.Description;
        }
        public static void AddPostsFromDB(List<PostUser> posts)
        {
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

        private static bool refresh = false;
        public static bool Refresh
        {
            get => refresh;
            set => refresh = value;
        }
    }
}
