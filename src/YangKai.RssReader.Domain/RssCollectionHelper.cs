using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace YangKai.RssReader.Domain
{
    public class RssCollectionHelper
    {
        public static GoogleRssCollection ReadGoogleRssXml(string path)
        {
            var googleRss = new GoogleRssCollection();
            var defaultFolder = new GoogleRssFolder { FolderName = "Default" };
            googleRss.Folders.Add(defaultFolder);

            var doc = new XmlDocument();
            doc.Load(path);
            if (!doc.HasChildNodes) return googleRss;

            XmlNodeList subNodeList = doc.ChildNodes[1].ChildNodes[1].ChildNodes;
            foreach (XmlNode subNode in subNodeList)
            {
                if (subNode.Attributes == null) continue;
                if (subNode.Attributes.Count == 0) continue;

                if (subNode.HasChildNodes)
                {
                    //新建目录
                    var folder = GetGoogleRssFolder(subNode);
                    googleRss.Folders.Add(folder);

                    //遍历该目录下所有条目
                    foreach (XmlNode itemNode in subNode.ChildNodes)
                    {
                        var googleRssItem = GetGoogleRssItem(itemNode);
                        folder.RssItems.Add(googleRssItem);
                    }
                }
                else
                {
                    var googleRssItem = GetGoogleRssItem(subNode);
                    googleRss.Folders.First().RssItems.Add(googleRssItem);
                }
            }

            return googleRss;
        }

        private static GoogleRssFolder GetGoogleRssFolder(XmlNode subNode)
        {
            var folder = new GoogleRssFolder();
            folder.FolderName = subNode.Attributes["title"].Value;
            return folder;
        }

        private static GoogleRssItem GetGoogleRssItem(XmlNode subNode)
        {
            if (subNode.Attributes != null)
            {
                var googleRssItem = new GoogleRssItem
                    {
                        XmlUrl = subNode.Attributes["xmlUrl"].Value,
                        HtmlUrl = subNode.Attributes["htmlUrl"].Value,
                        Title = subNode.Attributes["title"].Value
                    };

                return googleRssItem;
            }
            return new GoogleRssItem();
        }
    }

   
}
