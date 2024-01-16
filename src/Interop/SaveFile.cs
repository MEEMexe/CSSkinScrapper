using CSSkinScrapper.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CSSkinScrapper.Interop
{
    public class SaveFile
    {
        [JsonInclude]
        public int runCount;
        [JsonInclude]
        public string filePath;
        [JsonInclude]
        public List<Skin> skinList { get; private set; }

        [JsonIgnore]
        public int skinCount => skinList.Count;

        public SaveFile()
        {
            runCount = 0;
            filePath = "";
            skinList = new List<Skin>();
        }
    }
}
