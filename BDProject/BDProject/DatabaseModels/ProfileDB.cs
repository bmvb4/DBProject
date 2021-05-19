using System;
using System.Collections.Generic;
using System.Text;

namespace BDProject.DatabaseModels
{
    public class ProfileDB
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public byte[] Photo { get; set; }
        public List<PostDB> Posts { get; set; }
        public int Follower { get; set; } // following
        public int Followed { get; set; } // followers
    }
}
