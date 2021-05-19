using System;
using System.Collections.Generic;
using System.Text;

namespace BDProject.DatabaseModels
{
    public class CommentDB
    {
        public long IdComment { get; set; }
        public string IdUser { get; set; }
        public long IdPost { get; set; }
        public string CommentText { get; set; }
        public byte[] UserPhoto { get; set; }
    }
}
