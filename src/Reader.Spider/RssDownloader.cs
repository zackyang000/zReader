using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Reader.Domain;
using Reader.Services;
using Rss;

namespace Reader.Spider
{
   public class RssDownloader
   {
       /// <summary>
       /// 记录日志
       /// </summary>
       public Action<MessageLevel,string> LogAction;

       private IList<RssFeedItems> _rssCollection;

       public void Start()
       {
           _rssCollection = Repository.RssFeedItems.GetAll().ToList();
           Run();
       }

       private void Run()
       {
           foreach (var item in _rssCollection)
           {
               Run(item);
               //Task.Factory.StartNew(() => Run(item));
           }
       }

       #region Run

       private void Run(RssFeedItems item)
       {
           try
           {
               var rss = RssFeed.Read(item.XmlUrl);
               int addItemsCount = 0;
               foreach (RssChannel channel in rss.Channels)
               {
                   foreach (RssItem rssItem in channel.Items)
                   {
                           var localRss = CreateLocalRss(item, channel, rssItem);

                           //判断是否存在
                           var isExist = RssRepository.Instance.IsExist(rssItem.Link.AbsoluteUri);
                           if (isExist) continue;

                           //保存
                           RssRepository.Instance.Add(localRss);
                           addItemsCount++;
                   }
               }
               if (addItemsCount > 0) LogItem(MessageLevel.Info,item, "更新{0}条", addItemsCount);
           }
           catch (WebException ex)
           {
               LogItem(MessageLevel.Error, item, "Error:{0}", ex.Message);
           }
           catch (Exception ex)
           {
               LogItem(MessageLevel.Error, item, "Error:{0}", ex.Message);
           }
       }

       private static Domain.Rss CreateLocalRss(RssFeedItems item, RssChannel channel, RssItem rssItem)
       {
           //创建Rss条目
           var localRss = new Domain.Rss()
               {
                   Id = Guid.NewGuid(),
                   Site = item.Title,
                   SiteUrl = item.HtmlUrl,
                   Channel = channel.Title,
                   ChannelDescription = channel.Description,
                   Title = rssItem.Title,
                   Description = rssItem.Description,
                   Categories = string.Empty,
                   Pubdate = rssItem.PubDate.ToString(),
                   IsRead = "0",
                   IsDownload = "0",
                   DownloadFailedTimes = 0.ToString(),
                   CreateDate = DateTime.Now.ToString(),
                   Directory=string.Empty,
                   Link = rssItem.Link.AbsoluteUri
               };
           foreach (RssCategory category in rssItem.Categories)
           {
               localRss.Categories += "," + category.Name;
           }
           localRss.Categories = localRss.Categories.Length > 0
                                     ? localRss.Categories.Substring(1)
                                     : string.Empty;
           return localRss;
       }

       #endregion

       #region Log

       private void LogItem(MessageLevel level, RssFeedItems item, string msg, params object[] s)
       {
           var content = s == null ? msg : string.Format(msg, s);
           Log(level,"[{0}] {1}" ,item.Title, content);
       }

       private void Log(MessageLevel level,string msg, params object[] s)
       {
           if (LogAction != null)
           {
               var content = s == null ? msg : string.Format(msg, s);
               LogAction(level,string.Format("{0} {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), content));
           }
       }

       #endregion
    }

   public enum MessageLevel
   { 
       Info = 1,
       Error = 2,
       Warning = 4,
   }
}
