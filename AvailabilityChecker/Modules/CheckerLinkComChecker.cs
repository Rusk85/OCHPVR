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

        private bool checkAvailability(IList<Url> Links)
        {
            return checkLinks(postLinks(Links), Links.Count);
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

        private bool checkLinks(HtmlDocument Links, int Checksum)
        {
            var linkTable = Links.DocumentNode.Descendants()
                .Where(d => d.Attributes.Contains("class")
                && d.Attributes["class"].Value == "linkstable")
                .FirstOrDefault();
            var deadLinks = linkTable.Descendants()
                .Where(d => d.Attributes.Contains("class")
                && d.Attributes["class"].Value == "dead");
            var workingLinks = linkTable.Descendants()
                .Where(d => d.Attributes.Contains("class")
                && d.Attributes["class"].Value == "working");
            return !deadLinks.Any() && workingLinks.Count() / 2 == Checksum;
        }
    }
}