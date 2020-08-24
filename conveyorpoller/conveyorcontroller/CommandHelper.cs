using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using conveyorcontroller;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Mime;
using System.Threading;

namespace conveyorcontroller
{
    class CommandHelper
    {
        private static readonly string URLENDPOINT = "https://poller.hautedb.com";
        private static readonly HttpClient client = new HttpClient();
        private static conveyor conveyor = new conveyor();
        private ConveyorDetails ConveyorDetails = new ConveyorDetails();
        public async Task<Command> GetNextCommandAsync()
        {
            Command command = null;
            string responseString = await client.GetStringAsync(URLENDPOINT+"/Getnext");
            if (responseString != "null")
            {
                JObject Jo = (JObject)JsonConvert.DeserializeObject(responseString);
                command = Jo.ToObject<Command>();
            }
            return command;
        }
        
        public async Task MoveCommand(Command command)
        {
            Boolean processResponce = false;
            //Move the conveyour here!!!!
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(URLENDPOINT+"/Getfullinfo"),
                Content = new StringContent("{\"DistinctID\":\""+command.DistinctID+"\"}"),
            };
            string responseString = await (await client.SendAsync(request).ConfigureAwait(false)).Content.ReadAsStringAsync();
            Console.WriteLine("SENT TO MOVE" + this.ConveyorDetails.adjustSlot(command.Heading));
            processResponce = conveyor.getItem(this.ConveyorDetails.adjustSlot(command.Heading),this.ConveyorDetails.getConnection(command.Heading),this.ConveyorDetails.getConveyourNumber(command.Heading));
            if (processResponce)
            {
                await this.MarkCompleted(command);
            }
            else
            {
                await this.MarkFailed(command);
            }
            //await this.Distroy(command);
        }
        public async Task Distroy(Command command)
        {
           
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(URLENDPOINT+"/Distroy"),
                Content = new StringContent("{\"DistinctID\":\"" + command.DistinctID + "\"}"),
            };
            string responseString = await (await client.SendAsync(request).ConfigureAwait(false)).Content.ReadAsStringAsync();

        }
        public async Task MarkCompleted(Command command)
        {
           
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(URLENDPOINT+"/Markcomplete"),
                Content = new StringContent("{\"DistinctID\":\"" + command.DistinctID + "\"}"),
            };
            string responseString = await (await client.SendAsync(request).ConfigureAwait(false)).Content.ReadAsStringAsync();

        }
        public async Task MarkFailed(Command command)
        {
           
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(URLENDPOINT+"/Markfailed"),
                Content = new StringContent("{\"DistinctID\":\"" + command.DistinctID + "\",\"Message\":\"dummmy errror Generated\"}"),
            };
            string responseString = await (await client.SendAsync(request).ConfigureAwait(false)).Content.ReadAsStringAsync();

        }
        public async Task ReadSlot(Command command)
        {
            //READ SLOT HERE...
            int currentPossition = 0;
            try
            {
                currentPossition = conveyor.getPos(command.ConveyorNumber);
                Console.WriteLine(currentPossition);
                command.CurrentSlot = currentPossition;
            }
            catch(Exception ex)
            {
                Console.WriteLine("error in read slot " + ex.Message);
                command.CurrentSlot = 0;
            }
            Console.WriteLine(command.CurrentSlot);
            await this.UpdateSlot(command,command.CurrentSlot);
        }
        public async Task UpdateSlot(Command command,Int32 slot)
        {
            //Read the conveyour here!!!!
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(URLENDPOINT + "/Update"),
                Content = new StringContent("{\"DistinctID\":\"" + command.DistinctID + "\",\"slot\":"+slot+"}"),
            };
            string responseString = await (await client.SendAsync(request).ConfigureAwait(false)).Content.ReadAsStringAsync();

        }
    }
}
