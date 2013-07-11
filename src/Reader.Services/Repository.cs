using System;
using Reader.Domain;
using Reader.Infrastructure;

namespace Reader.Services
{
    public static class Repository
    {
        public static Repository<Rss, Guid> Rss
        {
            get { return InstanceLocator.Current.GetInstance<Repository<Rss,Guid>>(); }
        }

        public static Repository<RssFeedFolders, Guid> RssFeedFolders
        {
            get { return InstanceLocator.Current.GetInstance<Repository<RssFeedFolders, Guid>>(); }
        }

        public static Repository<RssFeedItems, Guid> RssFeedItems
        {
            get { return InstanceLocator.Current.GetInstance<Repository<RssFeedItems, Guid>>(); }
        }
    }
}