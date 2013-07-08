using System;
using System.Net;
using System.Threading.Tasks;
using Rss;
using YangKai.RssReader.Domain;
using YangKai.RssReader.Repositories;

namespace YangKai.RssReader.Services
{
   public class RssDownloader
   {
       private readonly object _locker=new object();

       /// <summary>
       /// 记录日志
       /// </summary>
       public Action<MessageLevel,string> LogAction;

       /// <summary>
       /// GoogleRss XML文件 分析结果
       /// </summary>
       private GoogleRssCollection _rssCollection;

       public void Start()
       {
           Load();
           Run();
       }

       private void Load()
       {
           try
           {
               _rssCollection = RssCollectionServices.Current;
           }
           catch (WebException ex)
           {
               Log(MessageLevel.Error, "分析XML文件失败,原因:{0}", ex.Message);
           }
       }

       private void Run()
       {
           foreach (var googleRssFolder in _rssCollection.Folders)
           {
               foreach (var googleRssItem in googleRssFolder.RssItems)
               {
                   var item = googleRssItem;
                   Task.Factory.StartNew(() => Run(item));
               }
           }
       }

       #region Run

       private void Run(GoogleRssItem item)
       {
           try
           {
               var rss = RssFeed.Read(item.XmlUrl);
               int addItemsCount = 0;
               foreach (RssChannel channel in rss.Channels)
               {
                   foreach (RssItem rssItem in channel.Items)
                   {
                       lock (_locker)
                       {
                           var localRss = CreateLocalRss(item, channel, rssItem);

                           //判断是否存在
                           var isExist = LocalRssRepository.Instance.IsExist(rssItem.Link.AbsoluteUri);
                           if (isExist) continue;

                           //保存
                           LocalRssRepository.Instance.Add(localRss);
                           addItemsCount++;
                       }
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

       private static LocalRss CreateLocalRss(GoogleRssItem item, RssChannel channel, RssItem rssItem)
       {
           //创建Rss条目
           var localRss = new LocalRss()
               {
                   Id = Guid.NewGuid(),
                   Site = item.Title,
                   SiteUrl = item.HtmlUrl,
                   Channel = channel.Title,
                   ChannelDescription = channel.Description,
                   Title = rssItem.Title,
                   Description = rssItem.Description,
                   Categories = string.Empty,
                   Pubdate = rssItem.PubDate,
                   IsRead = false,
                   IsDownload = false,
                   DownloadFailedTimes = 0,
                   CreateDate = DateTime.Now,
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

       private void LogItem(MessageLevel level,GoogleRssItem item, string msg, params object[] s)
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
