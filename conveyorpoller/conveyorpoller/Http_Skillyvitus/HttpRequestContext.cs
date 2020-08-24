namespace conveyorpoller.Http_Skillyvitus
{
    using global::conveyorpoller.Http_Skillyvitus;
    using conveyorpoller.Http;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web;

    public class HttpRequestContext
    {
        public HttpListenerContext BaseContext;
        public HttpRequestDispatcher Dispatcher;
        public string RequestBody;
        public string Route;

        public HttpRequestContext(HttpRequestDispatcher dispatcher, HttpListenerContext context, string route)
        {
            this.Dispatcher = dispatcher;
            this.Route = route;
            this.BaseContext = context;
            this.RequestBody = HttpUtility.UrlDecode(context.Request.InputStream.ReadToNull(), Encoding.UTF8);
            this.RemoteAddress = context.Request.RemoteEndPoint.Address.ToString();
            this.UserAgent = context.Request.UserAgent;
            this.Parameters = this.ParseParams();
        }

        public void Abort()
        {
            this.BaseContext.Response.Abort();
        }

        private Dictionary<string, string> ParseParams()
        {
            Dictionary<string, string> dictionary = (Dictionary<string, string>)null;
            if (this.BaseContext.Request.HttpMethod == "POST")
            {
                //dictionary = new Dictionary<string, string>();
                //string str1 = this.RequestBody;
                //char[] chArray = new char[1] { '&' };
                //foreach (string str2 in str1.Split(chArray))
                //{
                //    int length = str2.IndexOf('=');
                //    string key = str2.Substring(0, length);
                //    int num;
                //    string str3 = str2.Substring(num = length + 1);
                //    string str4;
                //    if (dictionary.TryGetValue(key, out str4))
                //        dictionary[key] = str4 + "|" + str3;
                //    else
                //        dictionary.Add(key, str3);
                //}
            }

            if (this.BaseContext.Request.HttpMethod == "GET")
                dictionary = Enumerable.ToDictionary(Enumerable.Select(Enumerable.Cast<string>((IEnumerable)this.BaseContext.Request.QueryString), s => new
                {
                    Key = s,
                    Value = this.BaseContext.Request.QueryString[s]
                }), p => p.Key, p => HttpUtility.UrlDecode(p.Value, Encoding.UTF8));
            return dictionary;
        }

        public void Redirect(string location)
        {
            this.BaseContext.Response.Redirect(location);
            this.BaseContext.Response.Close();
        }

        public void SendReply(string data)
        {
            try
            {
                this.BaseContext.PostReply(Encoding.UTF8.GetBytes(data));
            }
            catch(Exception)
            { }
        }

        public void SendReplyRaw(byte[] data)
        {
            this.BaseContext.PostReply(data);
        }

        public Dictionary<string, string> Parameters { get; set; }

        public string RemoteAddress { get; set; }

        public string UserAgent { get; set; }
    }
}

