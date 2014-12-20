using Flurl;
using HtmlAgilityPack;
using RestSharp;
using RestSharpWrapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AvailabilityChecker.Modules
{
    public class CheckerLinkComChecker : HttpChecker
    {
        public CheckerLinkComChecker()
            : base
            (
                SupportedHosters: new List<Url>
                {
                    new Url("ul.to"),
                    new Url("uploaded.to")
                },
                BaseUrl: new Url("http://checkerlink.com")
            ) { }

        public override bool IsAvailable(Url Link)
        {
            return checkAvailability(Link);
        }

        public override bool IsAvailable(IList<Url> Links)
        {
            return checkAvailability(Links);
        }

        public override IList<CheckerResult> GetAvailability(IList<Url> Links)
        {
            return checkLinks(Links);
        }

        private bool checkAvailability(IList<Url> Links)
        {
            return checkLinks(postLinks(Links));
        }

        private bool checkAvailability(Url Link)
        {
            return checkAvailability(new List<Url> { Link });
        }

        private HtmlDocument postLinks(IList<Url> Links)
        {
            var response = new Client().GetContent<string>
                (
                    base.BaseUrl,
                    null,
                    Method.POST,
                    prepareLinks(Links),
                    new Parameter
                    {
                        Name = "submit",
                        Value = "Check Links",
                        Type = ParameterType.GetOrPost
                    }
                );
            var retDoc = new HtmlDocument();
            retDoc.LoadHtml(response);
            return retDoc;
        }

        private Parameter prepareLinks(IList<Url> Links)
        {
            return new Parameter
            {
                Name = "links",
                Value = Links.Select(l => l.Path)
                    .Aggregate("", (a, b) => a + b + "\n"),
                Type = ParameterType.GetOrPost
            };
        }

        private bool checkLinks(HtmlDocument Links)
        {
            var linkTable = getLinkTable(Links);
            var deadLinks = linkTable.Descendants()
                .Where(d => d.Attributes.Contains("class")
                && d.Attributes["class"].Value == "dead");
            var workingLinks = linkTable.Descendants()
                .Where(d => d.Attributes.Contains("class")
                && d.Attributes["class"].Value == "working");
            return !deadLinks.Any();
        }

        private IList<CheckerResult> checkLinks(IList<Url> Links)
        {
            var htmlDoc = postLinks(Links);
            var linkTable = getLinkTable(htmlDoc);
            var linkTblElems = linkTable.Elements("tr").Where(e =>
                !e.Elements("th").Any()).ToList();
            var retList = new List<CheckerResult>();
            linkTblElems.ForEach(e => 
                retList.Add(analyseLinkBlock(e)));
            return retList;
        }

        private HtmlNode getLinkTable(HtmlDocument HtmlDocument)
        {
            return HtmlDocument.DocumentNode.Descendants()
                .Where(d => d.Attributes.Contains("class")
                && d.Attributes["class"].Value == "linkstable")
                .FirstOrDefault();
        }

        private CheckerResult analyseLinkBlock(HtmlNode LinkBlock)
        {
            var tgtNode = LinkBlock.Descendants().FirstOrDefault(d =>
                d.Attributes.Contains("href")
                && d.Attributes.Contains("target")
                && d.Attributes["target"].Value == "_blank"
                && d.Attributes.Contains("class"));
        
            Func<string, bool> isOnline = s => s == "live";
            bool isWorking = isOnline(tgtNode.Attributes["class"].Value);

            return new CheckerResult
            {
                IsAvailable = isWorking,
                Target = new Url(tgtNode.Attributes["href"].Value)
            };
        }
        



    }
}