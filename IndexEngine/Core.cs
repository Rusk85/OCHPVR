using Flurl;
using HtmlAgilityPack;
using IndexEngine.Indexer;
using System.Collections.Generic;
using System.Linq;

namespace IndexEngine
{
    public class Core
    {
        public static void Main(string[] args)
        {
        }

        public static HtmlDocument Load(IndexerType Indexer, Url IndexSite)
        {
            return IndexerFactory.GetIndexer(Indexer).Load(IndexSite);
        }

        public static List<HtmlDocument> Load(IndexerType Indexer, params Url[] IndexSites)
        {
            var retList = new List<HtmlDocument>();
            var indexer = IndexerFactory.GetIndexer(Indexer);
            IndexSites.ToList().ForEach(ix => retList.Add(indexer.Load(ix)));
            return retList;
        }
    }
}