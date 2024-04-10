using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSkinScrapper.SkinType
{
    public class Skin
    {
        public string name;
        public bool statTrak;
        public string type;
        public string condition;

        public new string ToString()
        {
            string complete = string.Empty;
            if (statTrak)
                complete += "StatTrak ";
            complete += type + " " + name + " " + condition;
            return complete;
        }
    }
}
