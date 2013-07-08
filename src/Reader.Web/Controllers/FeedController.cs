using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YangKai.RssReader.Domain;
using YangKai.RssReader.Infrastructure;
using YangKai.RssReader.Services;

namespace Reader.Web.Controllers
{
    public class FeedController : ApiController
    {
        public GoogleRssCollection Get()
        {
            var feed=  RssCollectionServices.Current;
            foreach (var folder in feed.Folders)
            {
                foreach (var item in folder.RssItems)
                {
                    var sql = "select count(0) from Rss where Channel=@Title";
                    item.Count = Convert.ToInt32(SqliteHelper.ExecuteScalar(sql,
                        new SQLiteParameter()
                            {
                                ParameterName = "@Title",
                                Value = item.Title,
                            }));
                }
            }
            return feed;
        }
    }
}