using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DownloadingEngine.Downloader;
using System.Net.Sockets;
using RestSharp;
using System.Net;
using Flurl;
using System.IO;
using RestSharpWrapper;
using Newtonsoft;
using Newtonsoft.Json;


namespace DownloadingEngine.Downloader
{
    public class pyLoadDownloader : BaseHttpDownloader
    {

        private Url getServerVersion_Method = new Url("getServerVersion");
        private Url login_Method = new Url("login");
        private Url addPackage_Method = new Url("addPackage");
        private Func<Url, string> ts = u => u.ToString();
        private readonly string DownloadSubfolder = "tv";


        public pyLoadDownloader() : 
            base
            (
                new Url("http://192.168.1.35:8000"),
                new Url("api"),
                "nas",
                "nas"
            ) { }

        /// <summary>
        /// Host portion includes port. 
        /// Example: new Url("http://192.168.1.35:8000")
        /// </summary>
        /// <param name="Host"></param>
        /// <param name="ApiBase"></param>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        public pyLoadDownloader(Url Host, Url ApiBase, 
            string Username, string Password) 
            : base(Host, ApiBase, Username, Password) { }

        protected override Client Login()
        {
            if (base.HttpClient != null) { return base.HttpClient; }
            var client = new Client();
            client.Login
            (
                base.BaseUrl.ToString(),
                ts(CreateMethodUrl(login_Method)),
                Method.POST,
                new Parameter{Name="username", Value = Username, Type = ParameterType.GetOrPost},
                new Parameter{Name="password", Value = Password, Type = ParameterType.GetOrPost}
            );
            base.HttpClient = client;
            return base.HttpClient;
        }


        public override Download AddDownload(Url DownloadLink)
        {
            return addPackage(DownloadSubfolder, DownloadLink);
        }

        #region pyLoad API Methods


        private string getServerVersion()
        {
            var version = HttpClient.GetContent(base.BaseUrl.ToString(), 
                base.CreateMethodUrl(getServerVersion_Method));
            return version;
        }

        private Download addPackage(string PackageName, Url DownloadLink)
        {
            Func<string, string> q = s => "\"" + s + "\"";
            var packageId = HttpClient.GetContent<string>
                (
                    CreateMethodUrl(addPackage_Method),
                    Method.POST,
                    new Parameter { Name = "name", Value = q(PackageName), Type = ParameterType.GetOrPost },
                    new Parameter { Name = "links", Value = SerializeObject(new List<string>{ts(DownloadLink)}), Type = ParameterType.GetOrPost }
                );
            return new Download
            {
                Link = DownloadLink,
            };
        }

        private string SerializeObject(Object Object)
        {
            return JsonConvert.SerializeObject(Object);
        }

        #endregion


    }
}
