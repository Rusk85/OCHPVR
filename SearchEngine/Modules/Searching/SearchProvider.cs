using Flurl;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine.Searching
{
    /// <summary>
    /// google.com
    /// duckduckgo.com
    /// ...
    /// </summary>
    public interface ISearchProvider
    {
        Url ProviderUrl { get; }

        Url ResourceUrl { get; }

        string QueryParameter { get; }

        string IndexerSiteOperator { get; }

        List<SearchResult> ParseSearchResult(string ResponseContent);
    }

    public class DuckDuckGoSearchProvider : ISearchProvider
    {

        public Url ProviderUrl
        {
            get { return new Url("https://duckduckgo.com"); }
        }

        public Url ResourceUrl
        {
            get { return new Url("html"); }
        }

        public string QueryParameter
        {
            get { return "q"; }
        }

        public string IndexerSiteOperator
        {
            get { return "site:"; }
        }


        public List<SearchResult> ParseSearchResult(string ResponseContent)
        {
            var retList = new List<SearchResult>();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(ResponseContent);
            var LinkSnippetBlocks = htmlDoc.DocumentNode.Descendants().Where(d =>
                d.Attributes.Contains("class")
                && d.Attributes["class"].Value == "links_main links_deep");
            LinkSnippetBlocks.ToList().ForEach(n =>
            {
                try
                {
                    retList.Add(new SearchResult
                    {
                        Link = new Url(n.ChildNodes.FirstOrDefault(cn => 
                            cn.Attributes.Contains("href")).Attributes["href"].Value),
                        Snippet = n.Descendants().FirstOrDefault(d => d.Attributes.Contains("class")
                        && d.Attributes["class"].Value == "snippet").InnerText
                    });
                }
                catch (NullReferenceException) {  }
            });
            return retList;
        }
    }


}
