using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVLibrary.LibraryObjects;
using RestSharpWrapper;
using RestSharp;

namespace TVLibrary.LibraryManagers
{
    public class NzbDroneLibraryManager : HttpLibraryManager
    {

        private Parameter _X_Api_Key = new Parameter
        {
            Name = "X-Api-Key",
            Value = "7785842fe2da45a6923f98beac0561a8",
            Type = ParameterType.HttpHeader
        };
        private Client _HttpClient = new Client();
        private string _Api_SystemStatus = "system/status";

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
            throw new NotImplementedException();
        }

        public override List<Episode> GetMissingEpisodes(Show Show)
        {
            throw new NotImplementedException();
        }

        public override List<Episode> GetMissingEpisodes(Show Show, Season Season)
        {
            throw new NotImplementedException();
        }

        public override List<Episode> GetMissingEpisodes(DateTime Date)
        {
            throw new NotImplementedException();
        }

        #region NzbDroneApi


        private string GetHistory()
        {
            return null;
        }



        #endregion

    }
}
