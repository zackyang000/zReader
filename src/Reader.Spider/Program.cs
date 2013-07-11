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

namespace Reader.Spider
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = ConfigurationManager.AppSettings["ContainerConfigPath"];
            IUnityContainer container = UnityContainerHelper.Create(path);
            InstanceLocator.SetLocator(new MyInstanceLocator(container));

            Repository.Rss.Add(new Rss() {});
        }
    }
}
