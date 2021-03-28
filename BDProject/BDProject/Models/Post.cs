using System;
using System.Collections.Generic;
using System.Text;

namespace BDProject.Models
{
    public class Post
    {
        public Post(byte[] photo, string description)
        {
            Photo = photo;
            Description = description;
        }

        public long IdPost { get; set; }
        public string IdUser { get; set; }
        public byte[] Photo { get; set; }
        public string Description { get; set; }

    }
}
