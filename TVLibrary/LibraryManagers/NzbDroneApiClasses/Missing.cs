using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVLibrary.LibraryManagers.NzbDroneApiClasses
{
    public class Image
    {
        public string coverType { get; set; }
        public string url { get; set; }
    }

    public class Season
    {
        public int seasonNumber { get; set; }
        public bool monitored { get; set; }
    }

    public class Series
    {
        public string title { get; set; }
        public string sortTitle { get; set; }
        public int seasonCount { get; set; }
        public int episodeCount { get; set; }
        public int episodeFileCount { get; set; }
        public int sizeOnDisk { get; set; }
        public string status { get; set; }
        public string overview { get; set; }
        public string network { get; set; }
        public string airTime { get; set; }
        public List<Image> images { get; set; }
        public List<Season> seasons { get; set; }
        public int year { get; set; }
        public string path { get; set; }
        public int profileId { get; set; }
        public bool seasonFolder { get; set; }
        public bool monitored { get; set; }
        public bool useSceneNumbering { get; set; }
        public int runtime { get; set; }
        public int tvdbId { get; set; }
        public int tvRageId { get; set; }
        public string firstAired { get; set; }
        public string lastInfoSync { get; set; }
        public string seriesType { get; set; }
        public string cleanTitle { get; set; }
        public string imdbId { get; set; }
        public string titleSlug { get; set; }
        public string certification { get; set; }
        public List<string> genres { get; set; }
        public int qualityProfileId { get; set; }
        public int id { get; set; }
    }

    public class Record
    {
        public int seriesId { get; set; }
        public int episodeFileId { get; set; }
        public int seasonNumber { get; set; }
        public int episodeNumber { get; set; }
        public string title { get; set; }
        public string airDate { get; set; }
        public string airDateUtc { get; set; }
        public string overview { get; set; }
        public bool hasFile { get; set; }
        public bool monitored { get; set; }
        public int tvDbEpisodeId { get; set; }
        public Series series { get; set; }
        public int id { get; set; }
    }

    public class Missing
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public string sortKey { get; set; }
        public string sortDirection { get; set; }
        public int totalRecords { get; set; }
        public List<Record> records { get; set; }
    }
}
