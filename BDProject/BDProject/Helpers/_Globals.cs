using BDProject.ModelWrappers;
using System;
using System.Collections.Generic;
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
            user.ID = otherUsers.Count;
            otherUsers.Add(user);
        }
        public static void RemoveUser(UserWrapper user)
        {
            otherUsers.RemoveAt(user.ID);
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
                }
            }
        }



        private static int openID = 0;
        public static int OpenID
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
