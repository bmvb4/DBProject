using System;
using System.Collections.Generic;
using System.Text;

namespace BDProject.DatabaseModels
{
    public class BigPostDB
    {
        public long IdPost { get; set; }
        public string IdUser { get; set; }
        public byte[] Photo { get; set; }
        public byte[] UserPhoto { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime DeleteDate { get; set; }
        public int LikesCounter { get; set; }
        public int CommentsCounter { get; set; }
        public bool isFollow { get; set; }
        public bool isLiked { get; set; }
        public List<string> tags { get; set; }
    }
}
