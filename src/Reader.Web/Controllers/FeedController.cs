using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YangKai.RssReader.Domain;
using YangKai.RssReader.Services;

namespace Reader.Web.Controllers
{
    public class FeedController : ApiController
    {
        public GoogleRssCollection Get()
        {
            return  RssCollectionServices.Current;
        }
    }
}