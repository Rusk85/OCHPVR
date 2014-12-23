using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVLibrary;
using TVLibrary.LibraryObjects;

namespace Core.Searching
{

    public class BuilderConfiguration
    {
        public string EpisodeQuality { get; private set; }

        public BuilderConfiguration()
        {
            EpisodeQuality = "1080p";
        }

        public BuilderConfiguration(string EpisodeQuality)
        {
            this.EpisodeQuality = EpisodeQuality;
        }
    }

    public class SearchQuery
    {
        public List<string> Keywords { get; set; }
        public Episode Episode { get; set; }
    }

    public class SearchQueryBuilder
    {

        public BuilderConfiguration Configuration { get; private set; }

        public SearchQueryBuilder()
        {
            Configuration = new BuilderConfiguration();
        }

        public SearchQueryBuilder(BuilderConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }

        public SearchQuery BuildSearchQuery(Episode Episode)
        {
            return new SearchQuery
            {
                Keywords = new List<string>
                {
                    Episode.Show.Name,
                    Episode.EpisodeIndex,
                    Configuration.EpisodeQuality
                },
                Episode = Episode
            };
        }







    }
}
