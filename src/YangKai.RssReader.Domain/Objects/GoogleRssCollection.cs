using System.Collections.Generic;
using System.Linq;

namespace YangKai.RssReader.Domain
{
    public class GoogleRssCollection
    {
        public List<GoogleRssFolder> Folders { get; set; }

        public GoogleRssCollection()
        {
            Folders = new List<GoogleRssFolder>();
        }

        public int RssItemsCount
        {
            get { return Folders.Sum(googleRssFolder => googleRssFolder.RssItems.Count); }
        }

        public override string ToString()
        {
            return "Folders:"+Folders.Count+ " Items:" + RssItemsCount;
        }
    }
}
