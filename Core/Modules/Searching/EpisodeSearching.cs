using SearchEngine.Searching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVLibrary.LibraryObjects;

namespace Core.Searching
{
    public class EpisodeSearcher : Search
    {
        public Episode Episode { get; private set; }

        public EpisodeSearcher(Episode Episode, List<ISearchProvider> SearchProviders, List<IIndexer> Indexers,
            params string[] Keywords)
            : base(SearchProviders, Indexers, Keywords)
        {
            this.Episode = Episode;
        }
    }

    public class EpisodeSearchResult : SearchResult
    {
        public Episode Episode { get; set; }
    }
}
