using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVLibrary.LibraryObjects;
using RestSharpWrapper;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Newtonsoft.Json;
using TVLibrary.LibraryManagers.NzbDroneApiClasses;

namespace TVLibrary.LibraryManagers
{
    public class NzbDroneLibraryManager : HttpLibraryManager
    {

        private CustomParameter _X_Api_Key = new CustomParameter
        {
            Name = "X-Api-Key",
            Value = "7785842fe2da45a6923f98beac0561a8",
            Type = ParameterType.HttpHeader,
            IsDefaultParameter = true
        };
        private Client _HttpClient = new Client();
        private string _Api_SystemStatus = "system/status";
        private string _Api_Missing = "missing";

        private readonly DateTime _DefaultCutOffDate = new DateTime(1900, 1, 1);

        public NzbDroneLibraryManager() :
            base("192.168.1.35", "8989", "api")
        {
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            authenticate();
        }

        private void authenticate()
        {
            _HttpClient.Login(base.LibraryManagerBaseUrl, _Api_SystemStatus,
                Method.GET, _X_Api_Key);
        }

        public override List<Episode> GetMissingEpisodes()
        {
            return GetMissing(_DefaultCutOffDate);
        }

        public override List<Episode> GetMissingEpisodes(Show Show)
        {
            throw new NotImplementedException();
        }

        public override List<Episode> GetMissingEpisodes(Show Show, TVLibrary.LibraryObjects.Season Season)
        {
            throw new NotImplementedException();
        }

        public override List<Episode> GetMissingEpisodes(DateTime Date)
        {
            return GetMissing(Date);
        }

        #region NzbDroneApi


        private string GetHistory()
        {
            return null;
        }

        private List<Episode> GetMissing(DateTime CutOffDate)
        {
            var jsonString = _HttpClient.GetContent<string>
                (
                    _Api_Missing, 
                    Method.GET,
                    new Parameter
                    {
                        Name = "page",
                        Value = "0",
                        Type = ParameterType.GetOrPost
                    },
                    new Parameter
                    {
                        Name = "pageSize",
                        Value = "10000",
                        Type = ParameterType.GetOrPost
                    },
                    new Parameter
                    {
                        Name = "sortKey",
                        Value = "series.title",
                        Type = ParameterType.GetOrPost
                    },
                    new Parameter
                    {
                        Name = "sortDir",
                        Value = "desc",
                        Type = ParameterType.GetOrPost
                    }
                );
            var missing = JsonConvert.DeserializeObject<Missing>(jsonString);
            return GetMissingEpisodesFromResponse(missing,CutOffDate);
        }


        #endregion

        #region Json2CsharpMapping of MissingResponse

        private List<Episode> GetMissingEpisodesFromResponse(Missing Missing, DateTime CutOffDate)
        {
            var shows = new List<Show>();
            Missing.records.ForEach(missingEpisode =>
            {
                if (Convert.ToDateTime(missingEpisode.airDate)
                    .CompareTo(CutOffDate) == -1 ) { return; }
                if (shows.FirstOrDefault(s => 
                    s.LibraryManagerSpecificId == missingEpisode.seriesId) == null)
                {
                    var show = MapNzb2LibraryProvider(missingEpisode);
                    var season = MapNzb2LibraryProvider(show, missingEpisode);
                    var episode = MapNzb2LibraryProvider(show, season, missingEpisode);

                    season.Episodes.Add(episode);
                    show.Seasons.Add(season);
                    shows.Add(show);
                }
                else
                {
                    var show = shows.FirstOrDefault(s => 
                        s.LibraryManagerSpecificId == missingEpisode.seriesId);
                    var season = show.Seasons.FirstOrDefault(se => se.SeasonNumber 
                        == missingEpisode.seasonNumber);
                    if (season == null)
                    {
                        season = MapNzb2LibraryProvider(show, missingEpisode);
                        var episode = MapNzb2LibraryProvider(show, season, missingEpisode);

                        season.Episodes.Add(episode);
                        show.Seasons.Add(season);
                    }
                    else
                    {
                        var episode = MapNzb2LibraryProvider(show, season, missingEpisode);
                        var alreadyAdded = season.Episodes.FirstOrDefault(e =>
                            e.EpisodeNumber == episode.EpisodeNumber
                            && e.Season.SeasonNumber == episode.Season.SeasonNumber);
                        if (alreadyAdded == null) { season.Episodes.Add(episode); }
                    }
                }
            });
            return shows.Select(sh => sh.Seasons)
                .SelectMany(se => se)
                .Select(e => e.Episodes)
                .SelectMany(e => e).ToList();
        }


        private Show MapNzb2LibraryProvider(Record Record)
        {
            return new Show
            {
                LibraryManagerSpecificId = Record.seriesId,
                Name = Record.series.title,
                Status = (Show.AirStatus)Enum.Parse(typeof(Show.AirStatus),
                    Record.series.status, true),
                Seasons = new HashSet<LibraryObjects.Season>()
            };
        }


        private TVLibrary.LibraryObjects.Season MapNzb2LibraryProvider(Show Show, Record Record)
        {
            return new TVLibrary.LibraryObjects.Season
            {
                Show = Show,
                EpisodeCount = Record.series.seasonCount,
                SeasonNumber = Record.seasonNumber,
                Episodes = new HashSet<Episode>()
            };
        }

        private Episode MapNzb2LibraryProvider(Show Show, 
            TVLibrary.LibraryObjects.Season Season, Record Record)
        {
            return new Episode
            {
                Show = Show,
                Season = Season,
                EpisodeNumber = Record.episodeNumber,
                Title = Record.title,
                AirDate = Convert.ToDateTime(Record.airDate)
            };
        }


        #endregion



    }

}
