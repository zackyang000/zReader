using System;
using System.IO;

namespace YangKai.RssReader.Domain
{
   public class Setting
    {
       public static string DownloadPath = Environment.CurrentDirectory + "/rss/";

       public static string GetFileName(string filename)
       {
           if (filename.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
           {
               filename = filename.Replace("//", string.Empty)
                   .Replace(@"\", string.Empty)
                   .Replace("/", string.Empty)
                   .Replace(":", string.Empty)
                   .Replace("*", string.Empty)
                   .Replace("?", string.Empty)
                   .Replace("\"", string.Empty)
                   .Replace("<", string.Empty)
                   .Replace(">", string.Empty)
                   .Replace("|", string.Empty);
           }
           return filename;
       }
    }
}
