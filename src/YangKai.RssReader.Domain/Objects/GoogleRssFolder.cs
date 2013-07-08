using System.Collections.Generic;

namespace YangKai.RssReader.Domain
{
    public class GoogleRssFolder
    {
        public string FolderName { get; set; }
        public List<GoogleRssItem> RssItems { get; set; }

        public GoogleRssFolder()
        {
            RssItems = new List<GoogleRssItem>();
        }

        public override string ToString()
        {
            return FolderName;
        }
    }
}