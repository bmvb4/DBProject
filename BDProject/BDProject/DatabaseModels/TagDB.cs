using System;
using System.Collections.Generic;
using System.Text;

namespace BDProject.DatabaseModels
{
    public class TagDB
    {
        public TagDB(string tag)
        {
            TagName = tag;
        }

        public string TagName { get; set; }
    }
}
