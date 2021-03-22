﻿using System;
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

        public int IdPost { get; set; }
        public byte[] Photo { get; set; }
        public string Description { get; set; }
        
        
        public int IdUser { get; set; }
    }
}
