using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using System.IO;
using RestSharp;
using RestSharpWrapper;

namespace DownloadingEngine.Downloader
{

    public struct Download
    {
        public Url Link { get; set; }
    }


    public interface IDownloader
    {
        Download AddDownload(Url DownloadLink);
    }


    public abstract class BaseHttpDownloader : IDownloader
    {

        public Url BaseUrl { get; set; }
        public Url BaseApiUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Client HttpClient { get; set; }

        public BaseHttpDownloader
        (
            Url BaseUrl,
            Url BaseApiUrl,
            string Username,
            string Password
        )
        {
            this.BaseUrl = BaseUrl;
            this.BaseApiUrl = BaseApiUrl;
            this.Username = Username;
            this.Password = Password;
            HttpClient = Login();
        }

        protected abstract Client Login();

        public abstract Download AddDownload(Url DownloadLink);

        protected virtual Url CreateMethodUrl(Url Method)
        {
            var tmp = BaseApiUrl.ToString();
            var ret = BaseApiUrl.AppendPathSegment(Method);
            BaseApiUrl = new Url(tmp);
            return ret;
        }
    }


}
