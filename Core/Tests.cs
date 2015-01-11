using Core.Searching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    class Tests
    {
        public static void Main(string[] args)
        {
            Test_MissingEpisodeSearch();
        }


        private static void Test_MissingEpisodeSearch()
        {
            var s = new Searcher();
            var result = s.SearchMissingEpisodes(new DateTime(2014, 10, 28));
            var indexBasedShows = result.Where(e => !e.Episode.Show.IsDateBasedEpisodeIndex).ToList();
        }


    }
}
