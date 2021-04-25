using System;
using System.Collections.Generic;
using System.Text;

namespace BDProject.Models
{
    public class Tag
    {
        public Tag(string tag)
        {
            TagName = tag;
        }

        public Tag()
        {

        }

        public string TagName { get; set; }
    }
}
