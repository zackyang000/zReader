using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reader.Domain
{
    public class RssFeedItems
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string XmlUrl { get; set; }
        public string HtmlUrl { get; set; }
    }
}
