namespace conveyorpoller.Http
{
    using conveyorpoller.Http_Skillyvitus;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Threading;

    public class HttpRequestDispatcher
    {
        private readonly HttpListener _requestListener = new HttpListener();
        public readonly Dictionary<string, Action<HttpRequestContext>> DynamicRoutes = new Dictionary<string, Action<HttpRequestContext>>();

        public HttpRequestDispatcher()
        {
            this._requestListener.Prefixes.Add("http://*:81/");
            this._requestListener.Prefixes.Add("https://*:443/");
            this._requestListener.IgnoreWriteExceptions = true;
        }

        public virtual void EndGetContext(IAsyncResult result)
        {
            try
            {
                HttpListenerContext context;
                string route;
                Action<HttpRequestContext> dynamicHandler;
                if ((this._requestListener != null) && this._requestListener.IsListening)
                {
                    string str;
                    context = this._requestListener.EndGetContext(result);
                    this._requestListener.BeginGetContext(new AsyncCallback(this.EndGetContext), this._requestListener);
                    route = context.Request.RawUrl.TrimStart(new char[] { '/' });
                    int index = route.IndexOf("?");
                    if (index != -1)
                    {
                        route = route.Substring(0, index);
                    }
                    if (((str = context.Request.HttpMethod) != null) && (str == "GET" || str == "POST" || str == "OPTIONS"))
                    {
                        if(str== "OPTIONS")
                        {
                            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                            context.Response.Headers.Add("Access-Control-Allow-Headers", "*");
                            context.Response.Headers.Add("Access-Control-Allow-Method", "*");
                            context.Response.Headers.Add(System.Net.HttpResponseHeader.ContentType, "raw");
                            context.Response.Headers.Add(System.Net.HttpResponseHeader.Allow, "*/*");
                            context.Response.Headers.Add(System.Net.HttpResponseHeader.KeepAlive, "true");
                            context.Response.Close();
                            return;
                        }
                        else { 
                        this.DynamicRoutes.TryGetValue(route, out dynamicHandler);
                        }
                    }
                    else
                    {
                        context.Response.Close();
                        return;
                    }
                    if (dynamicHandler == null)
                    {
                        Console.WriteLine("No handler for route " + route);
                    }
                    else
                    {
                        ThreadPool.QueueUserWorkItem(delegate (object state) {
                            try
                            {
                                try
                                {
                                    dynamicHandler(new HttpRequestContext(this, context, route));
                                    context.Response.Close();
                                }
                                catch (Exception exception)
                                {
                                    if (Debugger.IsAttached)
                                    {
                                        Debugger.Break();
                                    }
                                    Console.WriteLine("Route handler [{0}] exception -> {1}", route, exception);
                                }
                                if (context.Response.OutputStream.CanWrite)
                                {
                                    context.Response.Close();
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EndContext exception -> {1}", ex);
            }
        }

        public void Run()
        {
            Console.WriteLine("Httpd started");
            try
            {
                this._requestListener.Start();
            }
            catch (HttpListenerException exception)
            {
                if (exception.ErrorCode == 0x20)
                {
                    throw new Exception("HttpCore failed to start listening, port is most likely in use!");
                }
                if (exception.ErrorCode == 0x4be)
                {
                    throw new Exception("HttpCore failed to start listening, couldnt bind");
                }
            }
            this._requestListener.BeginGetContext(new AsyncCallback(this.EndGetContext), this._requestListener);
        }

        public void Stop()
        {
            this._requestListener.Stop();
        }
    }
}

