using Flurl;
using HtmlAgilityPack;
using IndexEngine.Indexer;
using System;
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


        /// <summary>
        /// Try to find a matching Indexer and retrieve the specified link content.
        /// If no matching Idexer can be found, null is returned.
        /// </summary>
        /// <param name="IndexSite"></param>
        /// <returns></returns>
        public static HtmlDocument TryLoad(Url IndexSite)
        {
            var idx = IndexerFactory.GetIndexer(IndexSite);
            if (idx == null) { return null; }
            return idx.Load(IndexSite);
        }

        /// <summary>
        /// Try to find matching indexers and retrieve the corresponding link contents.
        /// Missing indexers result in an omitted entry in the returned list.
        /// </summary>
        /// <param name="IndexSites"></param>
        /// <returns></returns>
        public static List<HtmlDocument> TryLoad(params Url[] IndexSites)
        {
            var retList = new List<HtmlDocument>();
            IndexSites.ToList().ForEach(site =>
            {
                var idx = IndexerFactory.GetIndexer(site);
                if (idx != null)
                {
                    retList.Add(idx.Load(site));
                }
            });
            return retList;
        }

    }
}