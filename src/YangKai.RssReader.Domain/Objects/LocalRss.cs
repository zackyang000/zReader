using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YangKai.RssReader.Domain
{
    public class LocalRss
    {
        public Guid Id { get; set; }
        public string Site { get; set; }
        public string SiteUrl { get; set; }
        public string Channel { get; set; }
        public string ChannelDescription { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Categories { get; set; }
        public string Link { get; set; }
        public DateTime Pubdate { get; set; }
        public Boolean IsRead { get; set; }
        public Boolean IsDownload { get; set; }
        public int DownloadFailedTimes { get; set; }
        public string Directory { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
