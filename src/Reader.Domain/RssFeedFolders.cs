using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reader.Domain
{
    public class RssFeedFolders
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public RssFeedItems RssFeedItems { get; set; }
    }
}
