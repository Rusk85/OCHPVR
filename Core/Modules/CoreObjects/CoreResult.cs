using Core.Searching;
using Flurl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVLibrary.LibraryObjects;

namespace Core.Modules.CoreObjects
{
    public class CoreResult
    {
        public EpisodeSearchResult SearchResult { get; set; }
        public Episode Episode
        {
            get
            {
                return SearchResult.Episode;
            }
        }
        public List<DownloadLink> DownloadLinks { get; set; }
    }

    public class DownloadLink
    {
        public Url EpisodeLink { get; set; }
        public bool IsAvailable { get; set; }
    }
}
