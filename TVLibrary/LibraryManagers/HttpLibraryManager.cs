using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVLibrary.LibraryObjects;
using Flurl;

namespace TVLibrary.LibraryManagers
{
    public abstract class HttpLibraryManager : ILibraryManager
    {

        public string Host { get; protected set; }
        public string Port { get; protected set; }
        public string ApiResourceUrl { get; protected set; }
        public Url LibraryManagerBaseUrl
        {
            get
            {
                return new Url("http://"
                    .AppendPathSegment(Host + ":" + Port)
                    .AppendPathSegment(ApiResourceUrl));
            }
        }

        public HttpLibraryManager()
        {
            Initialize();
        }

        public HttpLibraryManager(string Host, 
            string Port, string ApiResourceUrl)
        {
            this.Host = Host;
            this.Port = Port;
            this.ApiResourceUrl = ApiResourceUrl;
        }

        public virtual void Initialize()
        {
            if (Host == null
                || Port == null
                || ApiResourceUrl == null)
            {
                throw new HttpLibraryManagerException(
                    "Host, Port and ApiResourceUrl must be assigned.");
            }
        }

        public abstract List<Episode> GetMissingEpisodes();

        public abstract List<Episode> GetMissingEpisodes(Show Show);

        public abstract List<Episode> GetMissingEpisodes(Show Show, Season Season);

        public abstract List<Episode> GetMissingEpisodes(DateTime Date);

    }

    public class HttpLibraryManagerException : Exception
    {
        public HttpLibraryManagerException(string Message) 
            : base(Message) { }
    }

}
