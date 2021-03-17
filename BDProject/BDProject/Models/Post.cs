using System;
using System.Collections.Generic;
using System.Text;

namespace BDProject.Models
{
    public class Post
    {
        public int IdPost { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
        
        
        public int IdUser { get; set; }
    }
}
