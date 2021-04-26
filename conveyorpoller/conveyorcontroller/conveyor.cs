using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.IO;
using System.Net;
using System.Web;
using System.Collections.Concurrent;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Management;
using System.IO.Ports;
using System.Threading;


namespace conveyorcontroller
{
    class conveyor
    {
        public String orderID = "";
        
        private static SerialPort conveyorConnection = new SerialPort();
        private static SerialPort conveyorConnection2 = new SerialPort();
        private static Boolean conveyorConnection1Opening = false;
        private static Boolean conveyorConnection2Opening = false;
        private enum conveyerstatus
        {
            busy = 2,
            stoperd = 1,
            waiting = 0,
            bottmFetching ,
            topFetching,
            cutomeRequest,
            notconnected

        };
        private static object lockerSerial = new object();
        private static object lockerSerial2 = new object();
        public String status = conveyerstatus.notconnected.ToString();
        public  bool CloseSerial(Int32 conneection=1)
        {
            try
            {
                Console.WriteLine("Closing COM"+conneection.ToString()+"...");
                if (conneection == 2)
                {
                    conveyorConnection2.Close();
                }
                else
                {
                    conveyorConnection.Close();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error closing COM"+conneection.ToString());
                status = conveyerstatus.notconnected.ToString();
                return false;
            }
            Console.WriteLine("Close COM1 Successful.");
            status = conveyerstatus.notconnected.ToString();
            return true;
        }
        private  bool ConnectSerial(Int32 connection=1)
        {
            Console.WriteLine("CONNECT IS CALLED");
            SerialPort conveyorConnectioninternal = new SerialPort();
            if(connection == 2)
            {
                Console.WriteLine("CONN2");
                conveyorConnectioninternal = conveyorConnection2;
            }
            else
            {
                Console.WriteLine("CONN1");
                conveyorConnectioninternal = conveyorConnection;
            }
            if (conveyorConnectioninternal.IsOpen)
            {
                return true;
            }
            if(connection  == 2 && conveyorConnection2Opening)
            {
                Console.WriteLine("Another thread is stuck in trying to open connection2 on port COM4");
                return false;
            }
            if(connection ==1 && conveyorConnection1Opening)
            {
                Console.WriteLine("Another thread is stuck in trying to open connection1 on port COM3");
                return false;
            }
            
            try
            {
                Console.WriteLine("Opening COM"+connection.ToString()+"...");
                string selectedPort = "";
                var ports = COMPortInfo.GetCOMPortsInfo();
                //foreach (COMPortInfo comPort in ports)
                //{
                //    Console.WriteLine(comPort.Description);
                //    if (comPort.Description.Contains("Communications Port"))
                //    {
                //        selectedPort = comPort.Name;
                //        break;
                //    }
                //  }
                if (connection == 2)
                {
                    selectedPort = "COM4";
                    conveyorConnection2.NewLine = "\r\n";
                    conveyorConnection2.PortName = selectedPort;
                    conveyorConnection2.BaudRate = 19200;
                    conveyorConnection2.Parity = Parity.None;
                    conveyorConnection2.DataBits = 8;
                    conveyorConnection2.StopBits = StopBits.One;
                    conveyorConnection2.Handshake = Handshake.None;
                    conveyorConnection2.DtrEnable = true;
                    conveyorConnection2.ReadTimeout = 5000;
                    conveyorConnection2.WriteBufferSize = 1024;
                    conveyorConnection2Opening = true;
                    conveyorConnection2.Open();
                    conveyorConnection2Opening = false;
                }
                else
                {
                    selectedPort = "COM3";
                    conveyorConnection.NewLine = "\r\n";
                    conveyorConnection.PortName = selectedPort;
                    conveyorConnection.BaudRate = 19200;
                    conveyorConnection.Parity = Parity.None;
                    conveyorConnection.DataBits = 8;
                    conveyorConnection.StopBits = StopBits.One;
                    conveyorConnection.Handshake = Handshake.None;
                    conveyorConnection.DtrEnable = true;
                    conveyorConnection.ReadTimeout = 5000;
                    conveyorConnection.WriteBufferSize = 1024;
                    conveyorConnection1Opening = true;
                    conveyorConnection.Open();
                    conveyorConnection1Opening = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error opening COM"+connection.ToString()+"!");
                Console.WriteLine(ex.Message);
                if (connection == 1)
                {
                    conveyorConnection1Opening = false;
                }
                else
                {
                    conveyorConnection2Opening = false;
                }
                return false;
            }

            Console.WriteLine("Open COM1"+connection.ToString()+"Successful.");
            status = conveyerstatus.waiting.ToString();
            if (connection == 1)
            {
                conveyorConnection1Opening = false;
            }
            else
            {
                conveyorConnection2Opening = false;
            }
            return true;
        }
        public bool connectPort(Int32 connection = 1)
        {
            return ConnectSerial(connection);
        }

        //private String stop ()
        //{
        //    lock (lockerSerial)  //always lock this before doing serial stuff because http requests are coming in on multiple threads 
        //    {
        //        try
        //        {
        //            ConnectSerial();
        //            WriteCommand("$0SSTOP");
        //            string response = ReadLine();
        //            CloseSerial();
        //            return response;
        //        }
        //        catch (Exception ee)
        //        {
        //            return ee.Message;
        //        }
        //    }

        //   // ctx.SendReply("FAIL");
        //}
        public static bool WriteCommand(string cmd , Int32 connection = 1)
        {
            SerialPort internalserial = null;
            if (connection == 1)
            {
                internalserial = conveyorConnection;
            }
            else
            {
                internalserial = conveyorConnection2;
            }
            try
            {
                Console.WriteLine("Writing COM"+connection.ToString()+"...");
                internalserial.BaseStream.Flush();
                internalserial.WriteLine(cmd);
                internalserial.BaseStream.Flush();
            }
            catch (Exception)
            {
                Console.WriteLine("Error writing COM" + connection.ToString() );
                return false;
            }
            Console.WriteLine("Write COM" + connection.ToString() + " Successful.");
            return true;
        }

        private string ReadLine(Int32 connection = 1)
        {
            SerialPort internalserial = null;
            if (connection == 1)
            {
                internalserial = conveyorConnection;
            }
            else
            {
                internalserial = conveyorConnection2;
            }
            try
            {
                Console.WriteLine("Reading COM" + connection.ToString() + "...");

                StringBuilder sb = new StringBuilder();
                char rdChar = (char)0;
                if (internalserial.BytesToRead > 2)
                {
                    while ((rdChar = (char)internalserial.ReadByte()) != '\r')
                        sb.Append(rdChar);
                    sb.Append(rdChar);
                    Console.WriteLine("Read COM" + connection.ToString() + " Successful.");
                }
                else
                {
                    sb.Append('x'); sb.Append('x'); sb.Append('x'); sb.Append('x'); sb.Append('x'); sb.Append('x'); sb.Append('x'); sb.Append('x'); sb.Append('x'); sb.Append('x'); sb.Append('x'); sb.Append('x'); sb.Append('x');
                }
                return sb.ToString();
            }
            catch (Exception)
            {

                Console.WriteLine("Error writing COM" + connection.ToString() + "");
                return "";
            }
        }


        //public Boolean forceStop()
        //{
        //    lock (lockerSerial)  //always lock this before doing serial stuff because http requests are coming in on multiple threads 
        //    {
        //        status = conveyerstatus.busy.ToString();
        //        try
        //        {
        //            ConnectSerial();
        //            WriteCommand("$0SSTOP");
        //            string response = ReadLine();
        //            //CloseSerial();
        //            status = conveyerstatus.waiting.ToString();
        //            return true;
        //        }
        //        catch (Exception ee)
        //        {
        //            status = conveyerstatus.waiting.ToString();
        //            Console.WriteLine(ee.Message + ":\n" + ee.StackTrace);
        //            return false;
        //        }
        //    }


        //}
        public Int32 getPos(Int32 conveyor,Int32 connection = 1)
        {
            SerialPort internalserial = null;
            if (connection == 1)
            {
                internalserial = conveyorConnection;
            }
            else
            {
                internalserial = conveyorConnection2;
            }
            
            lock (lockerSerial)  //always lock this before doing serial stuff because http requests are coming in on multiple threads 
            {
                try
                {
                    ConnectSerial(connection);
                    WriteCommand("$0R"+conveyor.ToString(),connection);
                   
                    string response = ReadLine(connection);
                    //CloseSerial();
                    Int32 currentPos;
                    try
                    {
                        currentPos = Convert.ToInt32(response.Split(',')[1]);
                    }
                    catch (Exception ex)
                    {
                        currentPos = 1;
                    }
                    return currentPos;
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message + ":\n" + ee.StackTrace);
                    return 1;
                }
            }


        }
        public string customCommand(string command,Int32 connection = 1)
        {
            
            string response = "-1";
            lock (lockerSerial)  //always lock this before doing serial stuff because http requests are coming in on multiple threads 
            {
                try
                {
                    ConnectSerial(connection);
                    
                    WriteCommand(command, connection);
                    
                        
                    

                     response = ReadLine(connection);
                    //CloseSerial();
                    
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message + ":\n" + ee.StackTrace);
                    
                }
                
            }
            return response;
        }
        public Boolean getItem(Int32 heading, Int32 connection = 1,Int32 conveyor_number=0)
        {
            Console.WriteLine("---------------------------------------------------------------------");
            lock (lockerSerial)  //always lock this before doing serial stuff because http requests are coming in on multiple threads 
            {
                
                status = conveyerstatus.busy.ToString();
                try
                {
                    ConnectSerial(connection);
                    Console.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");

                    Console.WriteLine("$0G" + heading.ToString());
                    if(conveyor_number != 0)
                    {
                        Console.WriteLine("clearmem----------------------------$" + conveyor_number.ToString() + "SCLRM");
                        WriteCommand("$" + conveyor_number.ToString() + "SCLRM",connection);
                        string rresp = ReadLine(connection);
                        Console.WriteLine("clear response for :" + heading.ToString() + " : " + rresp);
                        Thread.Sleep(1000);
                    }
                    if (!WriteCommand("$0G" + heading.ToString(), connection))
                    {
                        Console.WriteLine("WRITE ERROR");
                        Thread.Sleep(500);
                        WriteCommand("$0G" + heading.ToString(), connection);
                    }


                     string response = ReadLine(connection);
                    //string response = "pppppp";
                    //CloseSerial();
                    if (response.ToLower().Contains("ack"))
                    {
                        
                        Console.WriteLine(response);
                        status = conveyerstatus.waiting.ToString();
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("NO ACKNOWLEDGMENT FOR :" + heading.ToString());
                        Console.WriteLine(response);
                        status = conveyerstatus.waiting.ToString();
                        return false;
                    }


                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message + ":\n" + ee.StackTrace);
                    status = conveyerstatus.waiting.ToString();
                    return false;
                }
            }


        }

    }
}
