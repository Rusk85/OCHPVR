using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVLibrary.LibraryManagers;

namespace TVLibrary
{
    internal class Tests
    {
        public static void Main(string[] args)
        {
            Test_Auth();
        }

        private static void Test_Auth()
        {
            var libMgm = new NzbDroneLibraryManager();
            var eps = libMgm.GetMissingEpisodes(new DateTime(2014, 5, 5));
        }
    }
}
