//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using CDO;
//using YangKai.RssReader.Domain;
//
//namespace YangKai.RssReader.Services
//{
//   public class PageDownloader
//   {
//       private string _title;
//
//       public PageDownloader(string title = null)
//       {
//           _title = title;
//       }
//
//
//       private readonly object _locker = new object();
//
//       /// <summary>
//       /// 记录日志
//       /// </summary>
//       public Action<MessageLevel, string> LogAction;
//
//       public void Start()
//       {
//           IList<LocalRss> list;
//
//           if (_title == null)
//           {
//               list = LocalRssRepository.Instance.LoadNeedDownload(1);
//           }
//           else
//           {
//               list = LocalRssRepository.Instance.LoadNeedDownload(1, _title);
//           }
//
//           LogAction(MessageLevel.Info, string.Format("共有{0}条记录需下载", list.Count));
//
//           foreach (var item in list)
//           {
//               Task.Factory.StartNew(() => Run(item));
//           }
//       }
//
//       private void Run(LocalRss item)
//       {
//           var dir = Setting.DownloadPath
//          + @"\" + Setting.GetFileName(item.Site)
//          + @"\" + item.Pubdate.ToString("yyyy-MM");
//
//           var a = Download(item, dir);
//
//           lock (_locker)
//           {
//               if (a)
//               {
//                   LocalRssRepository.Instance.UpdateDownloadState(item.Id.ToString(), dir);
//               }
//               else
//               {
//                   LocalRssRepository.Instance.UpdateFailedTimes(item.Id.ToString());
//               }
//           }
//       }
//
//       private bool Download(LocalRss item,string dir)
//       {
//           if (!System.IO.Directory.Exists(dir))
//           {
//               System.IO.Directory.CreateDirectory(dir);
//           }
//
//           try
//           {
//               Configuration cfg = new ConfigurationClass();
//               Message msg = new MessageClass();
//               msg.Configuration = cfg;
//               msg.MimeFormatted = true;
//               msg.CreateMHTMLBody(item.Link, CdoMHTMLFlags.cdoSuppressNone, "", "");
//               ADODB.Stream stm = msg.GetStream();
//               stm.SaveToFile(dir + @"\" + item.Id + ".mht", ADODB.SaveOptionsEnum.adSaveCreateOverWrite);
//               LogAction(MessageLevel.Info,
//                         string.Format("[{0}][{1}] 保存成功 {2}KB", item.Site, item.Title, (stm.Size/1024).ToString("N0")));
//               stm.Close();
//               return true;
//           }
//           catch (Exception e)
//           {
//               LogAction(MessageLevel.Error,
//                         string.Format("[{0}][{1}] 第[{2}]次保存失败.原因:{3}", item.Site, item.Title,
//                                       item.DownloadFailedTimes++, e.Message));
//               return false;
//           }
//       }
//    }
//}
