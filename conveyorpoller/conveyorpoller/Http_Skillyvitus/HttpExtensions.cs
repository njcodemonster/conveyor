namespace conveyorpoller.Http_Skillyvitus
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;

    public static class HttpExtensions
    {
        public static bool PostReply(this HttpListenerContext h, byte[] data)
        {
            if (data == null)
            {
                Console.WriteLine("PostReply() call on null data from {0}!", new StackTrace().GetFrame(1).GetMethod().Name);
                return false;
            }
            
            h.Response.Headers.Set(HttpResponseHeader.Server, "HonD");
            h.Response.KeepAlive = true;
            h.Response.ContentEncoding = Encoding.UTF8;
            h.Response.ContentLength64 = data.Length;
            return WriteLow(h.Response.OutputStream, data);
        }

        private static bool WriteLow(Stream output, byte[] data)
        {
            //output.WriteTimeout = 300;
            try
            {
                using (Stream stream = output)
                {
                    if(stream.CanWrite)
                    stream.Write(data, 0, data.Length);
                }
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (HttpListenerException exception)
            {
                Console.WriteLine(exception);
                return false;
            }
        }
    }
}

