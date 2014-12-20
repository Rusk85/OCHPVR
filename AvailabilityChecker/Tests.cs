using Flurl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvailabilityChecker.Modules;
using System.Diagnostics;

namespace AvailabilityChecker
{
    class Tests
    {
        public static void Main(string[] args)
        {
            TestCheckerLinkCom();
        }

        private static void TestCheckerLinkCom()
        {
            var links = new List<Url>{
                //new Url("http://somelink.com/file.rar"),
                new Url("http://ul.to/rppodvab"),
                new Url("http://ul.to/3vgwzyim"),
                new Url("http://ul.to/6zvad925"),
                new Url("http://netload.in/dateijX1L9oKjwX.htm"),
            };
            IChecker checker = new CheckerLinkComChecker();
            var supportedLinks = checker.GetSupportedLinks(links);
            var working = checker.GetAvailability(supportedLinks);
            Debug.WriteLine(working);
        }
    }
}
