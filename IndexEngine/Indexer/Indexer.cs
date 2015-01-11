using Flurl;
using HtmlAgilityPack;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Linq;

namespace IndexEngine.Indexer
{

    public enum IndexerType
    {
        tehPARADOX
    }


    public static class IndexerFactory
    {

        private static HashSet<BaseIndexer> _Indexers;

        static IndexerFactory()
        {
            cacheIndexers();
        }

        private static void cacheIndexers()
        {
            if (_Indexers == null)
            {
                var idx = Assembly.GetAssembly(typeof(Core)).DefinedTypes
                    .Where(t => t.BaseType == typeof(BaseIndexer))
                    .Cast<BaseIndexer>();
                _Indexers = new HashSet<BaseIndexer>(idx.ToList());
            }
        }


        public static BaseIndexer GetIndexer(IndexerType Indexer)
        {
            if (Indexer == IndexEngine.Indexer.IndexerType.tehPARADOX)
            {
                return new TehparadoxIndexer();
            }
            return null;
        }


        public static BaseIndexer GetIndexer(Url IndexSite)
        {
            return _Indexers.FirstOrDefault(bi =>
            {
                var idxBaseUrl = IndexSite.Path.Substring(0, bi.BaseUrl.Length);
                return idxBaseUrl.ToLower() == bi.BaseUrl.ToLower();
            });
        }



    }


    public abstract class BaseIndexer
    {
        public abstract List<Parameter> LoginParameters { get; }

        public abstract List<Parameter> LoginHeader { get; }

        public abstract string BaseUrl { get; }

        public abstract string LoginUrl { get; }

        public virtual string UserAgent
        {
            get
            {
                return "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:33.0) Gecko/20100101 Firefox/33.0";
            }
        }

        private RestClient _LoggedIn;

        private RestClient LoggedIn()
        {
            if (_LoggedIn != null) { return _LoggedIn; }
            var client = new RestClient(BaseUrl);
            client.CookieContainer = new CookieContainer();
            var request = new RestRequest(LoginUrl, Method.POST);
            LoginParameters.ForEach(lp => request.AddParameter(lp));
            LoginHeader.ForEach(lh => request.AddParameter(lh));
            client.Execute(request);
            _LoggedIn = client;
            return client;
        }

        public HtmlDocument Load(Url IndexSite)
        {
            var retDoc = new HtmlDocument();
            var html = LoggedIn().Execute(
                new RestRequest(IndexSite.ToString())).Content;
            retDoc.LoadHtml(html);
            return retDoc;
        }
    }

    public class TehparadoxIndexer : BaseIndexer
    {
        public override List<Parameter> LoginParameters
        {
            get
            {
                var body = ParameterType.GetOrPost;
                return new List<Parameter>
                {
                    new Parameter
                    {
                        Name="vb_login_username",
                        Value="YOURUSERNAME",
                        Type=body
                    },
                    new Parameter
                    {
                        Name="vb_login_md5password",
                        Value="YOURPASSWORD",
                        Type=body
                    },
                    new Parameter
                    {
                        Name="vb_login_md5password_utf",
                        Value="YOURPASSWORD",
                        Type=body
                    },
                    new Parameter
                    {
                        Name="securitytoken",
                        Value="guest",
                        Type=body
                    },
                    new Parameter
                    {
                        Name="cookieuser",
                        Value="1",
                        Type=body
                    },
                    new Parameter
                    {
                        Name="do",
                        Value="login",
                        Type=body
                    },
                };
            }
        }

        public override List<Parameter> LoginHeader
        {
            get
            {
                return new List<Parameter>
                {
                    new Parameter
                    {
                        Name="Content-Type",
                        Value="application/x-www-form-urlencoded",
                        Type=ParameterType.HttpHeader
                    }
                };
            }
        }

        public override string BaseUrl
        {
            get
            {
                return "http://tehparadox.com/";
            }
        }

        public override string LoginUrl
        {
            get
            {
                return "forum/login.php";
            }
        }
    }
}