using conveyorpoller.Http;
using conveyorpoller.Http_Skillyvitus;
using System;
using Newtonsoft.Json;



namespace conveyorpoller
{
     
    class Program
    {
        private static readonly HttpRequestDispatcher Dispatcher = new HttpRequestDispatcher();
        public static Commands ConveyorAcctions = new Commands();
        public static Boolean fetchingEnabled = true;
        static void Main(string[] args)
        {
            //AppDomain d =   AppDomain.CurrentDomain;
            Dispatcher.DynamicRoutes.Add("//", new Action<HttpRequestContext>(test));
            Dispatcher.DynamicRoutes.Add("/", new Action<HttpRequestContext>(test));
            Dispatcher.DynamicRoutes.Add("\\", new Action<HttpRequestContext>(test));
            Dispatcher.DynamicRoutes.Add("List", new Action<HttpRequestContext>(EchoAllCommands));
            Dispatcher.DynamicRoutes.Add(".well-known/pki-validation/0B6C958312856BC82996968539940B49.txt", new Action<HttpRequestContext>(validation));
            Dispatcher.DynamicRoutes.Add("CheckBussy", new Action<HttpRequestContext>(CheckBussy));
            Dispatcher.DynamicRoutes.Add("CheckStatusAll", new Action<HttpRequestContext>(CheckStatusAll)); 
            Dispatcher.DynamicRoutes.Add("CheckStatusAllAndClear", new Action<HttpRequestContext>(CheckStatusAllAndClear));
            Dispatcher.DynamicRoutes.Add("Add", new Action<HttpRequestContext>(Add));
            Dispatcher.DynamicRoutes.Add("DisableFetching", new Action<HttpRequestContext>(DisableFetching));
            Dispatcher.DynamicRoutes.Add("EnableFetching", new Action<HttpRequestContext>(EnableFetching));
            Dispatcher.DynamicRoutes.Add("ReIssue", new Action<HttpRequestContext>(ReIssue));
            Dispatcher.DynamicRoutes.Add("Markcomplete", new Action<HttpRequestContext>(Markcomplete));
            Dispatcher.DynamicRoutes.Add("Markfailed", new Action<HttpRequestContext>(Markfailed));
            Dispatcher.DynamicRoutes.Add("Getnext", new Action<HttpRequestContext>(Getnext));
            Dispatcher.DynamicRoutes.Add("Getfullinfo", new Action<HttpRequestContext>(Getfullinfo));
            Dispatcher.DynamicRoutes.Add("GetIsCompleted", new Action<HttpRequestContext>(GetIsCompleted));
            Dispatcher.DynamicRoutes.Add("Update", new Action<HttpRequestContext>(UpdateSlot));
            Dispatcher.DynamicRoutes.Add("Addsession", new Action<HttpRequestContext>(AddSession));
            Dispatcher.DynamicRoutes.Add("Getsessionstatus", new Action<HttpRequestContext>(GetSessionStatus));
            Dispatcher.DynamicRoutes.Add("Sessionend", new Action<HttpRequestContext>(SessionEnd));
            Dispatcher.DynamicRoutes.Add("CancelAll", new Action<HttpRequestContext>(CancelAll));
            Dispatcher.DynamicRoutes.Add("CancelAllSessions", new Action<HttpRequestContext>(CancelAllSessions));
            Dispatcher.DynamicRoutes.Add("CancelAllCommands", new Action<HttpRequestContext>(CancelAllCommands));
            Dispatcher.DynamicRoutes.Add("Distroy", new Action<HttpRequestContext>(DistroyCommand)); 
            Dispatcher.DynamicRoutes.Add("ListAllSessions", new Action<HttpRequestContext>(ListAllSessions));
            Dispatcher.Run();
            string text = "";
            while ((text = Console.ReadLine()) != "quit")
            {
                if (text == "save")
                {
                    //do something
                }
                if (text == "reload")
                {
                    //do something
                }
                if (text == "connect")
                {
                    //do something
                }
            }
            Dispatcher.Stop();
            Console.WriteLine("Hello World!");
        }
        public static async void validation(HttpRequestContext ctx)
        {
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            // string url = ctx.Parameters["url"];
            //await ConveyorAcctions.AddCommand(new Command(1, 2));
            ctx.SendReply("53F165AF507077455227E232299C030AD66AA3355C915969CBE6FDBAF1EFD94E comodoca.com 5e25d0d144b50");
            Console.WriteLine("Validator is called");
        }
        public static async void test(HttpRequestContext ctx)
        {
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            // string url = ctx.Parameters["url"];
            ctx.SendReply("Helo!");
            Console.WriteLine("test called");
        }
        public static async void DisableFetching(HttpRequestContext ctx)
        {
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            String Jsonstring = "";
            Jsonstring = ctx.RequestBody;
            // var Serializer = new JavaScriptSerializer();
            try
            {

                fetchingEnabled = false;
                Console.WriteLine("FETCHING DISABLED");



            }
            catch (Exception ex)
            {
               
                
            }
            ctx.SendReply("ok");
        }
        public static async void EnableFetching(HttpRequestContext ctx)
        {
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            String Jsonstring = "";
            Jsonstring = ctx.RequestBody;
            // var Serializer = new JavaScriptSerializer();
            try
            {

                fetchingEnabled = true;
                Console.WriteLine("FETCHING ENABLED");


            }
            catch (Exception ex)
            {


            }
            ctx.SendReply("ok");
        }
        public static async void CheckStatusAll (HttpRequestContext ctx)
        {
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            String Jsonstring = "";
            Jsonstring = ctx.RequestBody;
            // var Serializer = new JavaScriptSerializer();
            try
            {

                Jsonstring = Jsonstring + ConveyorAcctions.sessions.Count.ToString() + "---" + fetchingEnabled.ToString() + "----" + ConveyorAcctions.commands.Count.ToString();
                
                Jsonstring = Jsonstring + ConveyorAcctions.sessions.Count.ToString() + "---" + fetchingEnabled.ToString() + "----" + ConveyorAcctions.commands.Count.ToString();


            }
            catch (Exception ex)
            {
                Console.WriteLine("Session check false with error");
                ctx.SendReply("true");
            }
            ctx.SendReply(Jsonstring);
        }
        public static async void CheckStatusAllAndClear(HttpRequestContext ctx)
        {
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            String Jsonstring = "";
            Jsonstring = ctx.RequestBody;
            // var Serializer = new JavaScriptSerializer();
            try
            {

                Jsonstring = Jsonstring + ConveyorAcctions.sessions.Count.ToString() + "---" + fetchingEnabled.ToString() + "----" + ConveyorAcctions.commands.Count.ToString();
                await ConveyorAcctions.CanceAll();
                ConveyorAcctions.sessions.Clear();
                Jsonstring = Jsonstring + ConveyorAcctions.sessions.Count.ToString() + "---" + fetchingEnabled.ToString() + "----" + ConveyorAcctions.commands.Count.ToString();


            }
            catch (Exception ex)
            {
                Console.WriteLine("Session check false with error");
                ctx.SendReply("true");
            }
        }
        public static async void CheckBussy(HttpRequestContext ctx)
        {
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            String Jsonstring = "";
            Jsonstring = ctx.RequestBody;
            // var Serializer = new JavaScriptSerializer();
            try
            {


              
                    //Console.WriteLine("Session check true"+ ConveyorAcctions.sessions.Count );
                    //Console.WriteLine("enabled" + fetchingEnabled);
                    if(ConveyorAcctions.sessions.Count > 0)
                    {
                        ctx.SendReply("True-" + fetchingEnabled.ToString());
                    }
                    else
                    {
                        ctx.SendReply("False-" + fetchingEnabled.ToString());
                    }
                        
               
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Session check false with error");
                ctx.SendReply("true");
            }
        }
        public static async void EchoAllCommands(HttpRequestContext ctx)
        {
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Command CurrentComand = await ConveyorAcctions.GetNextCommand();
            while(CurrentComand != null && CurrentComand.ConveyorNumber != 0)
            {
                Console.WriteLine(CurrentComand.DistinctID);
                await ConveyorAcctions.MarkAsCompleated(CurrentComand.DistinctID);
                CurrentComand = await ConveyorAcctions.GetNextCommand();
            }
        }
        public static async void AddSession(HttpRequestContext ctx)
        {
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            String Jsonstring = "";
            Jsonstring = ctx.RequestBody;
            // var Serializer = new JavaScriptSerializer();
            try
            {
                dynamic CSession = (dynamic)JsonConvert.DeserializeObject(Jsonstring);

                ConveyorAcctions.AddSession(new Sessions((string)CSession["SessionID"], (Int32)CSession["LiveCount"]));

                ctx.SendReply("true");
            }
            catch (Exception ex)
            {
                ctx.SendReply("false");
            }

        }
        public static async void ListAllSessions(HttpRequestContext ctx)
        {
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            String Jsonstring = "";
            Jsonstring = ctx.RequestBody;
            string tr = "sessionlist";
            // var Serializer = new JavaScriptSerializer();
            try
            {
               foreach (Sessions s in ConveyorAcctions.sessions)
                {
                    tr = tr + s.LiveCount;
                }
                ctx.SendReply(tr);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in getsessionStatus");
                Console.WriteLine(ex.Message);

                ctx.SendReply("null");
            }
        }
        public static async void GetSessionStatus(HttpRequestContext ctx)
        {
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            String Jsonstring = "";
            Jsonstring = ctx.RequestBody;
            // var Serializer = new JavaScriptSerializer();
            try
            {
                dynamic CommandJson = (dynamic)JsonConvert.DeserializeObject(Jsonstring);

                string Cstatus = await ConveyorAcctions.GetSessionStatus((string)CommandJson["SessionID"]);
                ctx.SendReply(Cstatus.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in getsessionStatus");
                Console.WriteLine(ex.Message);

                ctx.SendReply("null");
            }
        }
        public static async void SessionEnd(HttpRequestContext ctx)
        {
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            String Jsonstring = "";
            Jsonstring = ctx.RequestBody;
            // var Serializer = new JavaScriptSerializer();
            try
            {
                dynamic CommandJson = (dynamic)JsonConvert.DeserializeObject(Jsonstring);

                string Cstatus = await ConveyorAcctions.EndSession((string)CommandJson["SessionID"]);
                ctx.SendReply(Cstatus.ToString());
            }
            catch (Exception ex)
            {
                ctx.SendReply("null");
            }
        }
        public static async void ReIssue(HttpRequestContext ctx)
        {
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            String Jsonstring = "";
            Jsonstring = ctx.RequestBody;
            try
            {
                dynamic CommandJson = (dynamic)JsonConvert.DeserializeObject(Jsonstring);
                await ConveyorAcctions.ReIssue((string)CommandJson["DistinctID"]);
                Console.WriteLine(ConveyorAcctions.commands.Count.ToString());
            }
            catch (Exception ex)
            {
                ctx.SendReply("null");
            }
        }
            public static async void Add(HttpRequestContext ctx)
        {
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            String Jsonstring = "";
            Jsonstring = ctx.RequestBody;
           // var Serializer = new JavaScriptSerializer();
            try
            {
                dynamic CommandJson =  (dynamic)JsonConvert.DeserializeObject(Jsonstring);

                string NewID = await ConveyorAcctions.AddCommand(new Command((Int32)CommandJson["Conveyor"], (Int32)CommandJson["Heading"],0,(Int32)CommandJson["Type"]));
                Int32 type = (Int32)CommandJson["Type"];
                if(type == 2)
                {
                    await ConveyorAcctions.AlterType(NewID);
                }
                ctx.SendReply(NewID);
                Console.WriteLine(ConveyorAcctions.commands.Count.ToString());
                Console.WriteLine("SLOT TO GO:");
                Console.WriteLine(CommandJson["Heading"]);
            }
            catch(Exception ex)
            {
                ctx.SendReply("null");
            }

        }
        public static async void UpdateSlot(HttpRequestContext ctx)
        {
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            String Jsonstring = "";
            Jsonstring = ctx.RequestBody;
            // var Serializer = new JavaScriptSerializer();
            try
            {
                dynamic CommandJson =  (dynamic)JsonConvert.DeserializeObject(Jsonstring);

                bool NewID = await ConveyorAcctions.UpdateCurentSlot((string)CommandJson["DistinctID"], (Int32)CommandJson["Slot"]);
                ctx.SendReply(NewID.ToString());
            }
            catch (Exception ex)
            {
                ctx.SendReply("false");
            }

        }
        public static async void DistroyCommand(HttpRequestContext ctx)
        {
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            String Jsonstring = "";
            Jsonstring = ctx.RequestBody;
            // var Serializer = new JavaScriptSerializer();
            try
            {
                dynamic CommandJson = (dynamic)JsonConvert.DeserializeObject(Jsonstring);

                bool NewID = await ConveyorAcctions.DistroyCommand((string)CommandJson["DistinctID"]);
                ctx.SendReply(NewID.ToString());
                Console.WriteLine("command marked distroyed:"+ConveyorAcctions.commands.Count.ToString() +" -- "+DateTime.Now.ToLongDateString());
            }
            catch (Exception ex)
            {
                ctx.SendReply("false");
            }

        }

        public static async void Markcomplete(HttpRequestContext ctx) {
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            String Jsonstring = "";
            Jsonstring = ctx.RequestBody;
            // var Serializer = new JavaScriptSerializer();
            try
            {
                dynamic CommandJson =  (dynamic)JsonConvert.DeserializeObject(Jsonstring);

                bool NewID = await ConveyorAcctions.MarkAsCompleated((string)CommandJson["DistinctID"]);
                ctx.SendReply(NewID.ToString());
                Console.WriteLine("command marked completed "+DateTime.Now.ToLongDateString() + " -- " + DateTime.Now.ToLongDateString());


            }
            catch (Exception ex)
            {
                ctx.SendReply("null");
            }

        }
        public static async void Markfailed(HttpRequestContext ctx)
        {
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            String Jsonstring = "";
            Jsonstring = ctx.RequestBody;
            // var Serializer = new JavaScriptSerializer();
            try
            {
                dynamic CommandJson =  (dynamic)JsonConvert.DeserializeObject(Jsonstring);

                bool NewID = await ConveyorAcctions.MarkAsFailed((string) CommandJson["DistinctID"], (string) CommandJson["Message"]);
                ctx.SendReply(NewID.ToString());
                
                Console.WriteLine("command marked failed " + DateTime.Now.ToLongDateString() + " -- " + DateTime.Now.ToLongDateString());


            }
            catch (Exception ex)
            {
                ctx.SendReply("null");
            }

        }
        public static async void Getnext(HttpRequestContext ctx) {
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            ctx.SendReply(JsonConvert.SerializeObject(await ConveyorAcctions.GetNextCommand()));
        }
        public static async void GetIsCompleted(HttpRequestContext ctx)
        {
            
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Headers", "*");
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Method", "*");
            ctx.BaseContext.Response.Headers.Add(System.Net.HttpResponseHeader.ContentType, "raw");
            ctx.BaseContext.Response.Headers.Add(System.Net.HttpResponseHeader.Allow, "*/*");
            ctx.BaseContext.Response.Headers.Add(System.Net.HttpResponseHeader.KeepAlive, "true");
            String Jsonstring = "";
            Jsonstring = ctx.RequestBody;
            // var Serializer = new JavaScriptSerializer();
            try
            {
                dynamic CommandJson = (dynamic)JsonConvert.DeserializeObject(Jsonstring);

                bool CStatus = await ConveyorAcctions.isCompleted((string)CommandJson["DistinctID"]);
                ctx.SendReply(CStatus.ToString());
            }
            catch (Exception ex)
            {
                ctx.SendReply("null");
            }
        }
        public static async void Getfullinfo(HttpRequestContext ctx) {
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            String Jsonstring = "";
            Jsonstring = ctx.RequestBody;
            // var Serializer = new JavaScriptSerializer();
            try
            {
                dynamic CommandJson =  (dynamic)JsonConvert.DeserializeObject(Jsonstring);

                Command FullCommand = await ConveyorAcctions.GetFullCommand((string)CommandJson["DistinctID"]);
                ctx.SendReply(JsonConvert.SerializeObject(FullCommand));
            }
            catch (Exception ex)
            {
                ctx.SendReply("null");
            }
        }
        public static async void CancelOne(HttpRequestContext ctx)
        {
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            String Jsonstring = "";
            Jsonstring = ctx.RequestBody;
            // var Serializer = new JavaScriptSerializer();
            try
            {
                dynamic CommandJson =  (dynamic) JsonConvert.DeserializeObject(Jsonstring);

                Command FullCommand = await ConveyorAcctions.Cancelit(CommandJson["DistinctID"]);
                ctx.SendReply(JsonConvert.SerializeObject(FullCommand));
            }
            catch (Exception ex)
            {
                ctx.SendReply("null");
            }
        }
        public static async void CancelAll(HttpRequestContext ctx)
        {
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            try
            {
                Console.WriteLine("--------------------------------.......CANCLE ALL CALLED AT........------------...........");
                Console.WriteLine(DateTime.Now.ToShortTimeString());
                await ConveyorAcctions.CanceAll();
                ConveyorAcctions.sessions.Clear();
                
            }
            catch (Exception ex)
            {
                ctx.SendReply("false");
            }
        }
        public static async void CancelAllSessions(HttpRequestContext ctx)
        {
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            try
            {

                ConveyorAcctions.sessions.Clear();

            }
            catch (Exception ex)
            {
                ctx.SendReply("false");
            }
        }
        public static async void CancelAllCommands(HttpRequestContext ctx)
        {
            ctx.BaseContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            try
            {

                await ConveyorAcctions.CanceAll();

            }
            catch (Exception ex)
            {
                ctx.SendReply("false");
            }
        }
    }
}
