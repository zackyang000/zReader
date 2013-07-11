using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reader.Domain
{
    public class Rss
    {
        public Guid Id { get; set; }
        public string Site { get; set; }
        public string SiteUrl { get; set; }
        public string Channel { get; set; }
        public string ChannelDescription { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Categories { get; set; }
        public string Pubdate { get; set; }
        public string IsRead { get; set; }
        public string CreateDate { get; set; }
        public string Link { get; set; }
        public string IsDownload { get; set; }
        public string Directory { get; set; }
        public string DownloadFailedTimes { get; set; }
    }
}
