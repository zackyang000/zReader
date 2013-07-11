using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Reader.Domain;
using Reader.Infrastructure;
using Reader.Services;
using Reader.Utility;
using YangKai.RssReader.Services;

namespace Reader.Spider
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = ConfigurationManager.AppSettings["ContainerConfigPath"];
            IUnityContainer container = UnityContainerHelper.Create(path);
            InstanceLocator.SetLocator(new MyInstanceLocator(container));

            var i = 1;
                    Console.Write("第[" + i + "]次 开始于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    var downloader = new RssDownloader();
                    downloader.LogAction += (level, msg) => Console.WriteLine(level + "   " + msg);
                    downloader.Start();
        }
    }
}
