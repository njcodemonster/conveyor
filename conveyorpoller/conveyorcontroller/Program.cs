using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace conveyorcontroller
{
    class Program
    {
        public static CommandHelper commandHelper = new CommandHelper();
        static   void Main()
        {
            Int32 i = 0;
            while (true )
            {
                // Start computation.
                 GetNewCommand();
                // Handle user input.
                Thread.Sleep(1500);
                //Console.WriteLine("called:"+i);
                //i++;
            }
            string result = Console.ReadLine();
        }
        
        static async Task GetNewCommand()
        {
            // This method runs asynchronously.
            Command  TC = null;
            try
            {
                TC = await Task.Run(() => NextCommandAsync()); ;
            }catch(Exception ex)
            {
                Console.WriteLine("Get Next command failed due to the error:" + ex.Message);
            }
            if (TC != null)
            {
                Console.WriteLine("Command recived!");
                Console.WriteLine("COmmand type:" + TC.type);
                if (TC.type == 1)
                {
                    Int32 DummyStatus = await Task.Run(() => MoveCommandAsync(TC));
                }
                if (TC.type == 2)
                {
                    Int32 DummyStatus = await Task.Run(() => ReadCommandAsync(TC));
                }
                Console.WriteLine("Command Processed!");
            }
          
        }

        static async Task<Command> NextCommandAsync()
        {
           
            return await commandHelper.GetNextCommandAsync();
        }
        
        static async Task<Int32> MoveCommandAsync(Command Tc)
        {
            await commandHelper.MoveCommand(Tc);
            return 1;
        }
        static async Task<Int32> ReadCommandAsync(Command Tc)
        {
            await commandHelper.ReadSlot(Tc);
            return 1;
        }
    }
}
    

