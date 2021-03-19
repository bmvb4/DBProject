using System;
using System.Collections.Generic;
using System.Text;

namespace BDProject.Models
{
    public class Post
    {
        public int IdPost { get; set; }
        public byte[] Photo { get; set; }
        public string Description { get; set; }
        
        
        public string IdUser { get; set; }
    }
}
