using System;
using System.Collections.Generic;
using System.Linq;
using Reader.Domain;

namespace Reader.Services
{
    public class RssRepository
    {
        public static RssRepository Instance
        {
            get { return new RssRepository(); }
        }

        /// <summary>
        /// 根据URL判断该条目是否已存在
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public bool IsExist(string link)
        {
           return Repository.Rss.Exist(p => p.Link == link);
        }

        /// <summary>
        /// 添加条目
        /// </summary>
        /// <param name="localRss"></param>
        public void Add(Rss rss)
        {
            Repository.Rss.Add(rss);
        }

        /// <summary>
        /// 载入需要下载的条目
        /// </summary>
        /// <param name="failedTimesLimit"></param>
        /// <returns></returns>
        public IList<Rss> LoadNeedDownload(int failedTimesLimit)
        {
            return Repository.Rss.GetAll(p => p.IsDownload == "0" && p.DownloadFailedTimes == "0").ToList();
        }

        /// <summary>
        /// 载入需要下载的条目
        /// </summary>
        /// <param name="failedTimesLimit"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public IList<Rss> LoadNeedDownload(int failedTimesLimit, string title)
        {
            return
                Repository.Rss.GetAll(p => p.IsDownload == "0"
                                           && p.DownloadFailedTimes == "0"
                                           && p.Site == "title")
                          .ToList();
        }

        /// <summary>
        /// 更新下载状态&路径
        /// </summary>
        /// <param name="id"></param>
        /// <param name="directory"></param>
        public void UpdateDownloadState(Guid id, string directory)
        {
            var item = Repository.Rss.Get(p => p.Id == id);
            item.IsDownload = "1";
            item.Directory = directory;
            Repository.Rss.Update(item);
        }

        /// <summary>
        /// 更新失败次数
        /// </summary>
        /// <param name="id"></param>
        public void UpdateFailedTimes(Guid id)
        {
            var item = Repository.Rss.Get(p => p.Id == id);
            item.DownloadFailedTimes = (Convert.ToInt32(item.DownloadFailedTimes) + 1).ToString();
            Repository.Rss.Update(item);
        }
    }
}
