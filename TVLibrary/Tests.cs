using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVLibrary.LibraryManagers;
using MoreLinq;

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
            //libMgm.GetEpisodeFiles(19);
            var eps = libMgm.GetMissingEpisodes(new DateTime(2014, 12, 1));
            var dateBased = eps.Where(e => e.Show.IsDateBasedEpisodeIndex)
                .Select(e => e.Show.Name).Distinct().ToList();
            var indexBased = eps.Where(e => !e.Show.IsDateBasedEpisodeIndex).ToList()
                .Select(e => e.Show.Name).Distinct().ToList();
            //var colbert = eps.Where(e => e.Show.Name.ToLower().Contains("colbert")).ToList();
            //var allDateBased = colbert.All(c => c.Show.IsDateBasedEpisodeIndex);
            //var others = eps.Where(e => (e.Show.Name.ToLower().Contains("colbert") ||
            //    e.Show.Name.ToLower().Contains("daily")) == false);
            //var indexBased = others.Where(o => !o.Show.IsDateBasedEpisodeIndex).ToList()
            //    .DistinctBy(o => o.Show.Name).Select(o => o.Show.Name);
            //var dateBased = others.Where(o => o.Show.IsDateBasedEpisodeIndex).ToList()
            //    .DistinctBy(o => o.Show.Name).Select(o => o.Show.Name);
        }

    }
}
