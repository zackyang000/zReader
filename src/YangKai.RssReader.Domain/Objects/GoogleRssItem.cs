namespace YangKai.RssReader.Domain
{
    public class GoogleRssItem
    {
        public string Title { get; set; }
        public string XmlUrl { get; set; }
        public string HtmlUrl { get; set; }

        public GoogleRssItem()
        {
            
        }

        public override string ToString()
        {
            return Title + " " + XmlUrl;
        }
    }
}