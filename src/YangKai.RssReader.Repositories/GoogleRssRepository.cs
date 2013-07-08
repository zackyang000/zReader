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
    public class GoogleRssRepository
    {
        public static GoogleRssRepository Instance
        {
            get { return new GoogleRssRepository(); }
        }

        /// <summary>
        /// 保存GoogleRss条目
        /// </summary>
        /// <param name="googleRss"></param>
        public void Save(GoogleRssCollection googleRss)
        {
            DeleteFoldersAndItems();

            int folderId = 1;
            foreach (var folder in googleRss.Folders)
            {
                AddFolder(folderId, folder);
                AddItems(folderId,folder.RssItems);
                folderId++;
            }
        }

        private void DeleteFoldersAndItems()
        {
            var sql = "delete from RssFeedFolders";
            SqliteHelper.ExecuteNonQuery(sql);

            sql = "delete from RssFeedItems";
            SqliteHelper.ExecuteNonQuery(sql);
        }

        private void AddFolder(int folderId, GoogleRssFolder folder)
        {
            var sql = "insert into RssFeedFolders(Id,Name) values(@Id,@Name)";
            var parmas = new[]
                {
                    new SQLiteParameter("@Id", folderId),
                    new SQLiteParameter("@Name", folder.FolderName),
                };
            SqliteHelper.ExecuteNonQuery(sql, parmas);
        }

        private void AddItems(int folderId,IEnumerable<GoogleRssItem> items)
        {
            foreach (var item in items)
            {
                var sql = "insert into RssFeedItems(ParentId,Title,XmlUrl,HtmlUrl) values(@ParentId,@Title,@XmlUrl,@HtmlUrl)";
                var parmas = new[]
                {
                    new SQLiteParameter("@ParentId", folderId),
                    new SQLiteParameter("@Title", item.Title),
                    new SQLiteParameter("@XmlUrl", item.XmlUrl),
                    new SQLiteParameter("@HtmlUrl", item.HtmlUrl),
                };
                SqliteHelper.ExecuteNonQuery(sql, parmas);
            }
        }

        /// <summary>
        /// 载入GoogleRss条目
        /// </summary>
        /// <returns></returns>
        public GoogleRssCollection GetAll()
        {
            var googleRssCollection = new GoogleRssCollection();
            foreach (DataRow r in GetFolders().Tables[0].Rows)
            {
                var folder = new GoogleRssFolder()
                {
                    FolderName = r["Name"].ToString()
                };

                foreach (DataRow r2 in GetItems().Tables[0].Select("ParentId=" + r["Id"]))
                {
                    var item = new GoogleRssItem()
                        {
                            Title = r2["Title"].ToString(),
                            XmlUrl = r2["XmlUrl"].ToString(),
                            HtmlUrl = r2["HtmlUrl"].ToString(),
                        };
                    folder.RssItems.Add(item);
                }

                googleRssCollection.Folders.Add(folder);
            }
            return googleRssCollection;
        }

        private DataSet GetFolders()
        {
            var sql = string.Format("select * from RssFeedFolders");
            return SqliteHelper.ExecuteDataSet(sql);
        }

        private DataSet _items = null;
        private DataSet GetItems()
        {
            if (_items == null)
            {
                var sql = string.Format("select * from RssFeedItems");
                _items= SqliteHelper.ExecuteDataSet(sql);
            }
            return _items;
        }
    }
}
