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
using Rss;

namespace Reader.Spider
{
    class Program
    {
        private static void Main(string[] args)
        {
            var path = ConfigurationManager.AppSettings["ContainerConfigPath"];
            var container = UnityContainerHelper.Create(path);
            InstanceLocator.SetLocator(new MyInstanceLocator(container));

            var i = 1;
            var rss = RssFeed.Read("http://feeds.feedburner.com/cool3c-show");
           
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("第[" + i + "]次 开始于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                var downloader = new RssDownloader();
                downloader.LogAction += (level, msg) =>
                {
                    switch (level)
                    {
                        case MessageLevel.Info:
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            break;
                        case MessageLevel.Error:
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            break;
                        case MessageLevel.Warning:
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            break;
                    }
                    Console.WriteLine(msg);
                };
                downloader.Start();

                i++;

                System.Threading.Thread.Sleep(30*60*1000);
            }

            Console.ReadLine();
        }
    }
}
