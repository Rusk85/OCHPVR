using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using HtmlAgilityPack;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Net;
using SearchEngine.Searching;

namespace SearchEngine
{
    class Core
    {
        public static void Main(string[] args)
        {
            RunSearch();
        }


        public static void RunSearch()
        {
            var search = new Search("veep", "1080p", "S02e03");
            var result = search.StartSearch();
        }

    }
}
