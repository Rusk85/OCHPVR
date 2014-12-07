using Newtonsoft.Json;
using RestSharp;
using System;
using System.Linq;
using System.Net;

namespace RestSharpWrapper
{
    public class Client
    {

        private RestClient _HttpClient;


        public RestClient Login(string BaseUrl, string ResourceUrl,
            Method Method, params Parameter[] Parameters)
        {
            if (_HttpClient != null) { return _HttpClient; }
            var pr = prepareRequest(BaseUrl, ResourceUrl, Method, Parameters);
            pr.Item1.CookieContainer = new CookieContainer();
            execute<string>(pr.Item1, pr.Item2);
            _HttpClient = pr.Item1;
            return pr.Item1;
        }

        public IRestResponse GetResponse(string BaseUrl, string ResourceUrl)
        {
            var pr = prepareRequest(BaseUrl, ResourceUrl);
            return execute(pr.Item1, pr.Item2);
        }

        public IRestResponse GetResponse
        (
            string BaseUrl,
            string ResourceUrl,
            Method Method,
            params Parameter[] Parameters
        )
        {
            var pr = prepareRequest(BaseUrl, ResourceUrl, Method, Parameters);
            return execute(pr.Item1, pr.Item2);
        }

        public T GetContent<T>(string ResourceUrl)
        {
            var pr = prepareRequest(ResourceUrl, Method.GET);
            return execute<T>(pr.Item1, pr.Item2);
        }

        public T GetContent<T>(string BaseUrl, string ResourceUrl)
        {
            var pr = prepareRequest(BaseUrl, ResourceUrl);
            return execute<T>(pr.Item1, pr.Item2);
        }

        public T GetContent<T>(string BaseUrl, string ResourceUrl,
            Method Method, params Parameter[] Parameters)
        {
            var pr = prepareRequest(BaseUrl, ResourceUrl, Method, Parameters);
            return execute<T>(pr.Item1, pr.Item2);
        }

        public T GetContent<T>(string ResourceUrl, Method Method,
            params Parameter[] Parameters)
        {
            var pr = prepareRequest(ResourceUrl, Method, Parameters);
            return execute<T>(pr.Item1, pr.Item2);
        }

        public string GetContent(string BaseUrl, string ResourceUrl)
        {
            return GetContent<string>(BaseUrl, ResourceUrl);
        }

        public string GetContent(string BaseUrl, string ResourceUrl,
            Method Method, params Parameter[] Parameters)
        {
            return GetContent<string>(BaseUrl, ResourceUrl, Method, Parameters);
        }

        private T execute<T>(RestClient Client, RestRequest Request)
        {
            var response = Client.Execute(Request);
            wasSuccessfulRequest(response);
            if (typeof(T) == typeof(string))
            {
                return (T)(response.Content as object);
            }
            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        private IRestResponse execute(RestClient Client, RestRequest Request)
        {
            var response = Client.Execute(Request);
            wasSuccessfulRequest(response);
            return response;
        }

        private Tuple<RestClient, RestRequest> prepareRequest(
            string BaseUrl, string ResourceUrl)
        {
            if (_HttpClient == null)
            {
                throw new RestException("Method Login() must be called first");
            }
            _HttpClient.BaseUrl = new Uri(BaseUrl);
            return Tuple.Create(_HttpClient,
                new RestRequest(ResourceUrl ?? String.Empty));
        }

        private Tuple<RestClient, RestRequest> prepareRequest
        (
            string BaseUrl,
            string ResourceUrl,
            Method Method,
            params Parameter[] Parameters
        )
        {
            var retTu = Tuple.Create(new RestClient(BaseUrl),
                new RestRequest(ResourceUrl ?? String.Empty, Method));
            Parameters.ToList().ForEach(p => retTu.Item2.AddParameter(p));
            return retTu;
        }

        private Tuple<RestClient,RestRequest> prepareRequest
        (
            string ResourceUrl,
            Method Method,
            params Parameter[] Parameters
        )
        {
            if (_HttpClient == null)
            {
                throw new RestException(
                    "Initialize Client by calling its Login()-Method first");
            }
            var reTu = Tuple.Create(_HttpClient, 
                new RestRequest(ResourceUrl ?? String.Empty, Method));
            reTu.Item2.RequestFormat = DataFormat.Json;
            Parameters.ToList().ForEach(p => reTu.Item2.AddParameter(p));
            return reTu;
        }


        private void wasSuccessfulRequest(IRestResponse Response)
        {
            if (Response.StatusCode != HttpStatusCode.OK)
            {
                throw Response.ErrorException ??
                    new RestException(String.Format("Request failed with HttpStatusCode: {0}",
                        Response.StatusCode));
            }
        }
    }

    public class RestException : Exception
    {
        public RestException(string Message)
            : base(Message) { }

        public RestException(string Message, Exception InnerExc)
            : base(Message, InnerExc) { }
    }
}