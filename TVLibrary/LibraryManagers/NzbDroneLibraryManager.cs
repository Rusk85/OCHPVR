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
using System.IO;

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
        private string _Api_EpisodeFile = "episodefile";
        private string _Api_Episode = "episode";

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

        private List<NzbDroneEpisodeFile> GetEpisodeFiles(int seriesId)
        {
            var jsonString = _HttpClient.GetContent<string>
                (
                    _Api_EpisodeFile,
                    Method.GET,
                    new Parameter
                    {
                        Name = "seriesId",
                        Value = seriesId,
                        Type = ParameterType.GetOrPost
                    }
                );
            return JsonConvert.DeserializeObject<List<NzbDroneEpisodeFile>>(jsonString);
        }

        /// <summary>
        /// API seems broken. API itself returns NullPointerReference.
        /// </summary>
        /// <param name="EpisodeId"></param>
        /// <returns></returns>
        private NzbDroneEpisodeFile GetEpisodeFile(int EpisodeId)
        {
            var jsonString = _HttpClient.GetContent<string>
                (
                    _Api_EpisodeFile,
                    Method.GET,
                    new Parameter
                    {
                        Name = "id",
                        Value = EpisodeId,
                        Type = ParameterType.GetOrPost
                    }
                );
            return JsonConvert.DeserializeObject<NzbDroneEpisodeFile>(jsonString);
        }

        private List<NzbDroneEpisode> GetEpisodes(int SeriesId)
        {
            var jsonString = _HttpClient.GetContent<string>
                (
                    _Api_Episode,
                    Method.GET,
                    new Parameter
                    {
                        Name = "seriesId",
                        Value = SeriesId,
                        Type = ParameterType.GetOrPost
                    }
                );
            return JsonConvert.DeserializeObject<List<NzbDroneEpisode>>(jsonString);
        }


        /// <summary>
        /// API seems broken. API itself returns NullPointerReference.
        /// </summary>
        /// <param name="EpisodeId"></param>
        /// <returns></returns>
        private NzbDroneEpisode GetEpisode(int EpisodeId)
        {
            var jsonString = _HttpClient.GetContent<string>
                (
                    _Api_Episode,
                    Method.GET,
                    new Parameter
                    {
                        Name = "id",
                        Value = EpisodeId,
                        Type = ParameterType.GetOrPost
                    }
                );
            return JsonConvert.DeserializeObject<NzbDroneEpisode>(jsonString);
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


        private Show MapNzb2LibraryProvider(NzbDroneEpisode Record)
        {
            return new Show
            {
                LibraryManagerSpecificId = Record.seriesId,
                Name = Record.series.title,
                Status = (Show.AirStatus)Enum.Parse(typeof(Show.AirStatus),
                    Record.series.status, true),
                Seasons = new HashSet<LibraryObjects.Season>(),
                IsDateBasedEpisodeIndex = isDateBasedEpisodeIndex(Record)
            };
        }


        private TVLibrary.LibraryObjects.Season MapNzb2LibraryProvider(Show Show, NzbDroneEpisode Record)
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
            TVLibrary.LibraryObjects.Season Season, NzbDroneEpisode Record)
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

        private bool isDateBasedEpisodeIndex(NzbDroneEpisode Episode)
        {
            var episodes = GetEpisodes(Episode.seriesId);
            var episodeFile = GetEpisodeFiles(Episode.seriesId).FirstOrDefault();

            if (episodeFile == null) { return false; }

            var existingEpisode = episodes.FirstOrDefault(e => e.episodeFileId == episodeFile.id);

            var episodeFileName = Path.GetFileName(episodeFile.path);

            var paddingLength = episodes.Count(e =>
                e.seasonNumber == existingEpisode.seasonNumber);
            paddingLength = paddingLength < 10
                ? 2
                : paddingLength.ToString().Length;


            var index = "S" + existingEpisode.seasonNumber.ToString().PadLeft(2, '0')
                + "E" + existingEpisode.episodeNumber.ToString().PadLeft(
                paddingLength, '0');

            var hasIndex = episodeFileName.ToUpper().Contains(index);

            var airingDate = existingEpisode.airDate.Replace('-', '.');

            var hasAiringDate = episodeFileName.ToUpper().Contains(airingDate);

            return hasAiringDate && !hasIndex;
        }


        #endregion



    }

}
