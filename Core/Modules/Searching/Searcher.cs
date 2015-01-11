using Core.Exceptions;
using SearchEngine.Searching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVLibrary.LibraryManagers;
using TVLibrary.LibraryObjects;

namespace Core.Searching
{

    public class SearcherConfiguration
    {
        public BuilderConfiguration SearchQueryBuilderConfiguration { get; set; }
        private SearchQueryBuilder _queryBuilder;
        public SearchQueryBuilder QueryBuilder
        {
            get
            {
                if (_queryBuilder == null)
                {
                    _queryBuilder = new SearchQueryBuilder(
                        SearchQueryBuilderConfiguration);
                }
                return _queryBuilder;
            }
        }
        public List<ILibraryManager> LibraryManagers { get; set; }
        public List<ISearchProvider> SearchProviders { get; set; }
        public List<IIndexer> Indexer { get; set; }
    }


    public class Searcher
    {
        public SearcherConfiguration Configuration { get; private set; }
        private SearcherConfiguration conf;

        public Searcher()
        {
            Configuration = new SearcherConfiguration
            {
                SearchQueryBuilderConfiguration = new BuilderConfiguration(),
                LibraryManagers = new List<ILibraryManager> { new NzbDroneLibraryManager() },
                SearchProviders = new List<ISearchProvider> { new DuckDuckGoSearchProvider() },
                Indexer = new List<IIndexer> { new TehParadoxIndexer() }
            };
            conf = Configuration;
        }

        public Searcher(SearcherConfiguration Configuration)
        {
            this.Configuration = Configuration;
            validateConfiguration();
            conf = Configuration;
        }

        private void validateConfiguration()
        {
            if (!Configuration.LibraryManagers.Any())
            {
                throw new OCHPVRInitializationEception(
                    "No LibraryManagers specified. Retrieving missing episodes not possible.");
            }
            if (!Configuration.SearchProviders.Any())
            {
                throw new OCHPVRInitializationEception(
                    "No SearchProviders specified. Searching Indexers for missing episodes not possible.");
            }
            if (!Configuration.Indexer.Any())
            {
                throw new OCHPVRInitializationEception(
                    "No Indexers specified. Indexers are resources such as forums" +
                    "that contain the actual links to OCHs. Searching for missing episodes not possible");
            }
        }


        public List<EpisodeSearchResult> SearchMissingEpisodes(DateTime Since)
        {
            var missingEpisodes = new List<Episode>();
            conf.LibraryManagers.ForEach(lm => 
                missingEpisodes.AddRange(lm.GetMissingEpisodes(Since)));
            var searchQueries = buildSearchQueries(missingEpisodes);
            var missingEpisodeSearches = createMissingEpisodeSearches(searchQueries);
            return runSearchesForMissingEpisodes(missingEpisodeSearches);
        }

        private List<EpisodeSearcher> createMissingEpisodeSearches(List<SearchQuery> Queries)
        {
            var searches = new List<EpisodeSearcher>();
            Queries.ForEach(q => searches.Add(new EpisodeSearcher
            (
                q.Episode,
                conf.SearchProviders,
                conf.Indexer,
                q.Keywords.ToArray()
            )));
            return searches;
        }

        private List<EpisodeSearchResult> runSearchesForMissingEpisodes(
            List<EpisodeSearcher> EpisodeSearches)
        {
            var esr = new List<EpisodeSearchResult>();
            EpisodeSearches.ForEach(search =>
            {
                var result = search.StartSearch();
                result.ToList().ForEach(r =>
                {
                    esr.Add(new EpisodeSearchResult
                    {
                        Link = r.Link,
                        Snippet = r.Snippet,
                        Episode = search.Episode
                    });
                });
            });
            return esr;
        }

        private List<SearchQuery> buildSearchQueries(List<Episode> MissingEpisodes)
        {
            var queries = new List<SearchQuery>();
            MissingEpisodes.ForEach(e => 
                queries.Add(conf.QueryBuilder.BuildSearchQuery(e)));
            return queries;
        }

    }
}
