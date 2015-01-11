using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using idx = IndexEngine;
using TVLibrary.LibraryObjects;
using Core.Searching;
using Flurl;
using Core.Modules.CoreObjects;

namespace Core.Modules.SearchResultParsing
{
    static class Parser
    {

        public static List<CoreResult> Parse(
            List<EpisodeSearchResult> EpisodeSearchResults)
        {
            EpisodeSearchResults.ForEach(esr => loadIndexerSiteContent(ref esr));
            return null;
        }

        private static void loadIndexerSiteContent(
            ref EpisodeSearchResult SearchResult)
        {
            SearchResult.IndexerSiteContent = idx.Core.TryLoad(SearchResult.Link);
        }

        /// <summary>
        /// Match IndexerSiteTitle against MissingEpisode.
        /// </summary>
        /// <param name="SearchResult"></param>
        /// <returns></returns>
        private static bool isMatch_IdxSiteTitle_MissingEpisode(
            EpisodeSearchResult SearchResult)
        {
            var idxTitle = SearchResult.Link;
            return true;
        }



    }


}
