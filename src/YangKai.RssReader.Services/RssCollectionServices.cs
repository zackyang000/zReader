using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YangKai.RssReader.Domain;
using YangKai.RssReader.Repositories;

namespace YangKai.RssReader.Services
{
   public class RssCollectionServices
    {
        /// <summary>
        /// 当前实例(含数据)
        /// </summary>
        public static GoogleRssCollection Current
        {
            get { return GoogleRssRepository.Instance.GetAll(); }
        }

        public static int Load(string path)
        {
            var googleRss = RssCollectionHelper.ReadGoogleRssXml(path);
            GoogleRssRepository.Instance.Save(googleRss);
            return googleRss.RssItemsCount;
        }
    }
}
