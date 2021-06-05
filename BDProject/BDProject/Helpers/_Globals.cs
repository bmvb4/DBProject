﻿using BDProject.DatabaseModels;
using BDProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BDProject.Helpers
{
    public static class _Globals
    {
        // ================= Main User
        private static User MainUser = new User();
        public static User GlobalMainUser
        {
            get => MainUser;
            set => MainUser = value;
        }



        // ================= Main Feed
        private static List<Post> FeedPosts = new List<Post>();
        public static List<Post> GlobalFeedPosts
        {
            get => FeedPosts;
            set => FeedPosts = value;
        }
        public static Post GetPost(long id)
        {
            Post post = FeedPosts?.First(x => x.IdPost == id);
            return (post != null) ? post : new Post();
        }
        public static void AddPostsFromDB(List<PostDB> posts)
        {
            FeedPosts.Clear();
            foreach (PostDB post in posts)
                FeedPosts.Add(new Post(post));
        }
        public static void AddPostsFromDB(List<BigPostDB> posts)
        {
            FeedPosts.Clear();
            foreach (BigPostDB post in posts)
                FeedPosts.Add(new Post(post));
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



        public static string UsernameTemp = "";
        public static string PasswordTemp = "";



        public static string Base64DefaultPhoto = "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cRGH6arlEJAFOj4+Nq3Fg6lfwW7NbXlMYS1QTGvFS0j4FTFFFdNV8fAn01XLJCALJiRkCAeVgCAk/CvCwbcEhBu3QxJXbF1cXBSIxfEIx+bmZoHbBITv1JBcXl6WT58+ufx3gWKEEYvicR+Hq6r4EVdh8VMxpXV+fr6KyefPnwvzFaONiEacQJim4mcEhHupo5KY3rJWMg9GG2QJCGkxKqlrJWIyLaLBQ7AGQloceOrBR0z6Fzf8RTBEg4ciIDyI2zGJaa4IydnZmSu5RhSjjHjqXwQjrqDyECcemiksmovRScQkFuAFpa1YBK/RWFtbsxBOU05JaO726OT6+vpmhBKviEr8GfdXRxgRjXgJBkMTEAYVB7jbQQkRlFg3qUERle/VWNRXvH+mpBibLZDRxZlzvG7f6RxRiYjcHqUsISwRighDDUX8Hu+NWNAjWyVdioNm+PZqoToFVoMSI5d4xT/Xnz2LQNTRRI1FDcaTJ09MQTEpAsKk1CmwcNd3M0VA4nV1dXXze72suAamRub27/Xv/41vRwP1n+vPGonbf17jIBDMiYAwK98evIF2nA4BkCIgAKQICAApAgJAioAAkCIgAKQICAApAgJAioAAkCIgAKQICAApAgJAioAAkCIgAKQICAApAgJAioAAkCIgAKQICAApAgJAioAAkCIgAKQICAApAgJAioAAkCIgAKQICAApAgJAioAAkCIgAKQICAApAgJAioAAkCIgAKQICAApAgJAioAAkCIgAKQICAApAgJAioAAkCIgAKQICAApAgJAioAAkCIgAKQICAApAgJAioAAkCIgAKQICAApAgJAioAAkCIgAKQICAApAgJAioAAkCIgAKQICAApAgJAioAAkCIgAKQICAApAgJAioAAkCIgAKQICAApAgJAioAAkCIgAKQICAApAgJAioAAkCIgAKQICAApAgJAioAAkCIgAKQICAApAgJAioAAkCIgAKQICAApAgJAioAAkCIgAKQICAApAgJAioAAkCIgAKQICAAp/wfErcem9QTQvAAAAABJRU5ErkJggg==";
        public static byte[] Base64Bytes => Convert.FromBase64String(Base64DefaultPhoto);
    }
}
