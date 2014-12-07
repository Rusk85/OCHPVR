using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DownloadingEngine.Downloader;
using System.IO;
using Flurl;

namespace DownloadingEngine
{
    internal class Core
    {
        public static void Main(string[] args)
        {
            
        }

        private static void Test()
        {
            var pyLoad = new pyLoadDownloader();
            //http://netload.in/dateiqKSz3SaYCx/ppp315b3.rar.htm
            var pid = pyLoad.AddDownload(new Url("http://dlserver.com/datei.rar"));
        }

    }
}
