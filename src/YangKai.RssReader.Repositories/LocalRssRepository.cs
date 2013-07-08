using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using YangKai.RssReader.Domain;
using YangKai.RssReader.Infrastructure;

namespace YangKai.RssReader.Repositories
{
    public class LocalRssRepository
    {
        public static LocalRssRepository Instance
        {
            get { return new LocalRssRepository(); }
        }

        /// <summary>
        /// 根据URL判断该条目是否已存在
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public bool IsExist(string link)
        {
            var result = SqliteHelper.ExecuteScalar("select count(0) from Rss where Link=@Link",
                                                    new SQLiteParameter("@Link", link));
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 添加条目
        /// </summary>
        /// <param name="localRss"></param>
        public void Add(LocalRss localRss)
        {
            var sql = "insert into Rss(Id,Site,SiteUrl,Channel,ChannelDescription,Title,Description,Categories,Link,Pubdate,IsRead,IsDownload,CreateDate,Directory,DownloadFailedTimes)"
                               +
                               " values(@Id,@Site,@SiteUrl,@Channel,@ChannelDescription,@Title,@Description,@Categories,@Link,@Pubdate,@IsRead,@IsDownload,@CreateDate,@Directory,@DownloadFailedTimes)";
            var parmas = new[]
                           {
                               new SQLiteParameter("@Id", localRss.Id.ToString()),
                               new SQLiteParameter("@Site", localRss.Site),
                               new SQLiteParameter("@SiteUrl", localRss.SiteUrl),
                               new SQLiteParameter("@Channel", localRss.Channel),
                               new SQLiteParameter("@ChannelDescription", localRss.ChannelDescription),
                               new SQLiteParameter("@Title", localRss.Title),
                               new SQLiteParameter("@Description", localRss.Description),
                               new SQLiteParameter("@Categories", localRss.Categories),
                               new SQLiteParameter("@Link", localRss.Link),
                               new SQLiteParameter("@Pubdate", localRss.Pubdate),
                               new SQLiteParameter("@IsRead", localRss.IsRead),
                               new SQLiteParameter("@IsDownload", localRss.IsDownload),
                               new SQLiteParameter("@CreateDate", localRss.CreateDate),
                               new SQLiteParameter("@Directory", localRss.Directory),
                               new SQLiteParameter("@DownloadFailedTimes", localRss.DownloadFailedTimes),
                           };
            SqliteHelper.ExecuteNonQuery(sql, parmas);
        }

        /// <summary>
        /// 载入需要下载的条目
        /// </summary>
        /// <param name="failedTimesLimit"></param>
        /// <returns></returns>
        public IList<LocalRss> LoadNeedDownload(int failedTimesLimit)
        {
            var sql = string.Format("select * from Rss where IsDownload=0 and DownloadFailedTimes<={0}" , failedTimesLimit);
            var ds = SqliteHelper.ExecuteDataSet(sql);
            var list = ConvertToLocalRss(ds);
            return list;
        }

        /// <summary>
        /// 载入需要下载的条目
        /// </summary>
        /// <param name="failedTimesLimit"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public IList<LocalRss> LoadNeedDownload(int failedTimesLimit,string title)
        {
            var sql = string.Format("select * from Rss where IsDownload=0 and DownloadFailedTimes<={0} and site='{1}'", failedTimesLimit,title);
            var ds = SqliteHelper.ExecuteDataSet(sql);
            var list = ConvertToLocalRss(ds);
            return list;
        }

        private static List<LocalRss> ConvertToLocalRss(DataSet ds)
        {
            var list = new List<LocalRss>();
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                var localRss = new LocalRss()
                    {
                        Id = Guid.Parse(r["Id"].ToString()),
                        Site = r["Site"].ToString(),
                        SiteUrl = r["SiteUrl"].ToString(),
                        Channel = r["Channel"].ToString(),
                        ChannelDescription = r["ChannelDescription"].ToString(),
                        Title = r["Title"].ToString(),
                        Description = r["Description"].ToString(),
                        Categories = r["Categories"].ToString(),
                        Pubdate = Convert.ToDateTime(r["Pubdate"]),
                        IsRead = r["IsRead"].ToString() == "1",
                        IsDownload = r["IsDownload"].ToString() == "1",
                        DownloadFailedTimes = Convert.ToInt32(r["DownloadFailedTimes"]),
                        Directory = r["Directory"].ToString(),
                        CreateDate = Convert.ToDateTime(r["CreateDate"]),
                        Link = r["Link"].ToString(),
                    };
                list.Add(localRss);
            }
            return list;
        }

        /// <summary>
        /// 更新下载状态&路径
        /// </summary>
        /// <param name="id"></param>
        /// <param name="directory"></param>
        public void UpdateDownloadState(string id, string directory)
        {
            var sql = "update Rss set IsDownload=1,Directory=@Directory where Id=@Id";
            var parmas = new[]
                           {
                               new SQLiteParameter("@Id", id),
                               new SQLiteParameter("@Directory", directory),
                           };
            SqliteHelper.ExecuteNonQuery(sql, parmas);
        }

        /// <summary>
        /// 更新失败次数
        /// </summary>
        /// <param name="id"></param>
        public void UpdateFailedTimes(string id)
        {
            var sql = "update Rss set DownloadFailedTimes=DownloadFailedTimes+1 where Id=@Id";
            var parmas = new[]
                           {
                               new SQLiteParameter("@Id", id),
                           };
            SqliteHelper.ExecuteNonQuery(sql, parmas);
        }
    }
}
