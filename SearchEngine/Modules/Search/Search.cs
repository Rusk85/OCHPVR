using Flurl;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine.Modules.Search
{
    public class Search
    {
        public List<string> Keywords { get; private set; }
        
        public List<ISearchProvider> SearchProviders { get; private set; }
        
        public List<IIndexer> Indexers { get; private set; }
        
        private List<Tuple<RestClient, List<RestRequest>, ISearchProvider>> SearchesToRun = 
            new List<Tuple<RestClient, List<RestRequest>, ISearchProvider>>();
        
        private Dictionary<ISearchProvider, List<IRestResponse>> ExecutedSearches 
            = new Dictionary<ISearchProvider, List<IRestResponse>>();

        public Search(List<ISearchProvider> SearchProviders, List<IIndexer> Indexers,
            params string[] Keywords)
        {
            this.SearchProviders = SearchProviders;
            this.Indexers = Indexers;
            this.Keywords = Keywords.ToList();
        }

        public Search(params string[] Keywords)
        {
            SearchProviders = new List<ISearchProvider> 
                { new DuckDuckGoSearchProvider() };
            Indexers = new List<IIndexer> 
                { new TehParadoxIndexer() };
            this.Keywords = Keywords.ToList();
        }


        private void GenerateSearchesToRun()
        {
            SearchProviders.ForEach(sp =>
            {
                var client = new RestClient(sp.ProviderUrl.ToString());
                var requests = new List<RestRequest>();
                Indexers.ForEach(idx =>
                {
                    requests.Add(new RestRequest(
                        sp.ResourceUrl.AppendPathSegment("").SetQueryParam(sp.QueryParameter,
                        Keywords.Aggregate(sp.IndexerSiteOperator + idx.Indexer + " ", (kw1, kw2) => kw1 + kw2 + " ")).ToString()));
                });
                SearchesToRun.Add(Tuple.Create(client, requests, sp));
            });
        }

        private void ExecuteGeneratedSearches()
        {
            SearchesToRun.ForEach(s =>
            {
                s.Item2.ForEach(request =>
                {
                    var response = s.Item1.Execute(request);
                    if (!ExecutedSearches.ContainsKey(s.Item3))
                    {
                        var rL = new List<IRestResponse>();
                        rL.Add(response);
                        ExecutedSearches.Add(s.Item3, rL);
                    }
                    else
                    {
                        ExecutedSearches[s.Item3].Add(response);
                    }
                });
            });
        }


        private void GenerateAndExecuteSearches()
        {
            GenerateSearchesToRun();
            ExecuteGeneratedSearches();
        }

        public IEnumerable<SearchResult> StartSearch()
        {
            GenerateAndExecuteSearches();
            var results = new List<SearchResult>();
            var es = ExecutedSearches;
            es.Keys.ToList().ForEach(sp =>
            {
                es[sp].ForEach(response =>
                {
                    results.AddRange(sp.ParseSearchResult(response.Content));
                });
            });
            return results;
        }






    }
}
