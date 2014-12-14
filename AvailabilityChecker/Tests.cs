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
                new Url("http://somelink.com/file.rar"),
            };
            var working = new CheckerLinkComChecker().IsAvailable(links);
            Debug.WriteLine(working);
        }
    }
}
