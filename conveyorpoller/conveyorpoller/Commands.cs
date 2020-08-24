using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace conveyorpoller
{
    //private 
    internal class Command
    {
        public Int32 ConveyorNumber = 0;
        public Int32 CurrentSlot = 0;
        public Int32 Heading = 0;
        public string DistinctID = "";
        public Boolean Completed = false;
        public string Message = "";
        public Boolean Error = false;
        public Boolean Reported = false;
        public Int32 type = 1;
        public Boolean Issued = false;
        public static object lockComand = new object();
        public Command(Int32 _ConveyorNumber, Int32 _Heading, Int32 _CurrentSlot = 0,Int32 _type = 1)
        {
            this.ConveyorNumber = _ConveyorNumber;
            this.Heading = _Heading;
            this.CurrentSlot = _CurrentSlot;
            this.type = _type;
            this.DistinctID = Guid.NewGuid().ToString();
        }
        public void MarkCompleted()
        {
            this.Completed = true;
        }
        public void MarkFailed(String _Message)
        {
            this.Error = true;
            this.Message = _Message;
            this.Completed = true;
        }
        public void MarkReported()
        {
            this.Reported = true;
        }
    }
    internal class Sessions
    {
        public string SessionID = "";
        public Int32 LiveCount = 0;
        public Sessions(string _sessionID,Int32 _liveCount)
        {
            this.SessionID = _sessionID;
            this.LiveCount = _liveCount;
        }

    }
   
    internal class Commands
    {
        public List<Command> commands = new List<Command>();
        public List<Sessions> sessions = new List<Sessions>();
        public async Task<string> AddCommand(Command NewCommand)
        {
            lock (Command.lockComand)
            {
                Console.WriteLine(NewCommand.type);
                this.commands.Add(NewCommand);
                return NewCommand.DistinctID;
            }
        }
        public async void AddSession(Sessions _session)
        {
            Sessions localSession = sessions.Find(x => x.SessionID == _session.SessionID);
            if(localSession== null)
            {
                this.sessions.Add(_session);
            }
            else
            {
                sessions.Find(x => x.SessionID == _session.SessionID).LiveCount = _session.LiveCount;
            }
            
        }
        public async Task<string> GetSessionStatus(string SessionID)
        {
            Sessions localSession = sessions.Find(x => x.SessionID == SessionID);
            if (localSession == null)
            {
                return "Null";
            }
            else
            {
                return localSession.LiveCount.ToString(); ;
            }
        }
        public async Task<string> EndSession(string SessionID)
        {
            Sessions localSession = sessions.Find(x => x.SessionID == SessionID);
            if (localSession == null)
            {
                return "False";
            }
            else
            {
                try
                {
                    sessions.Remove(localSession);
                }
                catch(Exception ex)
                {
                    return "False";
                }
                return "True";
            }
        }
        public async Task<bool> ReIssue(string _DistinctID)
        {
            try
            {
                lock (Command.lockComand)
                {
                    Command ToBeMarked = null;
                    ToBeMarked = commands.Find(x => (x.DistinctID == _DistinctID));
                    ToBeMarked.Completed = false;
                    ToBeMarked.Issued = false;
                    ToBeMarked.Error = false;
                    Console.WriteLine("RE-Issue processed for command:" + _DistinctID + "slot" + ToBeMarked.Heading.ToString());
                }
                return true;
            }
            catch(Exception ex)
            {
                lock (Command.lockComand)
                {
                    Console.WriteLine("reIssue error for :" + _DistinctID + " error " + ex.Message);
                    Console.WriteLine("in array");
                    foreach (Command com in commands)
                    {
                        Console.WriteLine(com.DistinctID + "--" + com.Heading);
                    }
                }
                return false;
            }
        }
        public async Task<Command> GetNextCommand()
        {
            Command ToReturn = null;
            if (commands.Count > 0)
            {
                lock (Command.lockComand)
                {
                    ToReturn = commands.Find(x => (x.Completed == false && x.Issued == false));
                }
                  
            }
            if(ToReturn != null)
            {
                Console.WriteLine("returned:" + ToReturn.DistinctID);
                Console.WriteLine(ToReturn.type);
                ToReturn.Issued = true;
            }
            //Console.WriteLine("null returned");
            return ToReturn;
        }
        public async Task<bool> MarkAsCompleated(string _DistinctID)
        {
            Command ToBeMarked = null;
            lock (Command.lockComand)
            {
                ToBeMarked = commands.Find(x => (x.DistinctID == _DistinctID && x.Error == false));
            }
            if (ToBeMarked == null)
            {
                return false;
            }
            else
            {
                lock (Command.lockComand)
                {
                    ToBeMarked.MarkCompleted();
                }
                return true;
            }
        }
        public async Task<bool> MarkAsFailed(string _DistinctID,string _Message)
        {
            Command ToBeMarked = null;
            lock (Command.lockComand)
            {
                ToBeMarked = commands.Find(x => (x.DistinctID == _DistinctID && x.Error == false));
            }
            if (ToBeMarked == null)
            {
                return false;
            }
            else
            {
                lock (Command.lockComand)
                {
                    ToBeMarked.MarkFailed(_Message);
                }
                Console.WriteLine("--------------------------markedfailed--------------------------------------------------");
                Console.WriteLine(ToBeMarked.ConveyorNumber+"---"+ToBeMarked.CurrentSlot+"---"+ToBeMarked.Error+"---"+ToBeMarked.Heading);
                Console.WriteLine("--------------------------markedfailed--------------------------------------------------");
                return true;
            }
        }
        public async Task<bool> isCompleted(string _DistinctID)
        {
            lock (Command.lockComand)
            {
                var ToBeRt = commands.Find(x => (x.DistinctID == _DistinctID));
            
                if(ToBeRt != null)
                {
                    return ToBeRt.Completed;
                }
                else
                {
                    return false;
                }
            }
        }
        public async Task<Command> GetFullCommand(string _DistinctID)
        {
            lock (Command.lockComand)
            {
                var ToBeRt = commands.Find(x => (x.DistinctID == _DistinctID));
            
                if (ToBeRt != null)
                {
                    return ToBeRt;
                }
                else
                {
                    return ToBeRt;
                }
            }
        }
        public async Task<bool> Cancelit(string _DistinctID)
        {
            lock (Command.lockComand)
            {
                Command ToBeCancel = commands.Find(x => (x.DistinctID == _DistinctID));
                if (ToBeCancel != null)
                {
                    return false;
                }
                else
                {
                    commands.Remove(ToBeCancel);
                    return true;
                }
            }
        }
        public async Task<bool> CanceAll()
        {
            lock (Command.lockComand)
            {
                try
                {
                    commands.Clear();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        public async Task<bool> AlterType(string _DistinctID)
        {
            lock (Command.lockComand)
            {
                Command ToBeChanged = commands.Find(x => (x.DistinctID == _DistinctID));
                if (ToBeChanged != null)
                {
                    return false;
                }
                else
                {
                    ToBeChanged.type = 2;
                    return true;
                }
            }
        }
        public async Task<bool> DistroyCommand(string _DistinctID )
        {
            lock (Command.lockComand)
            {
                Command ToBeChanged = commands.Find(x => (x.DistinctID == _DistinctID));
                if (ToBeChanged == null)
                {
                    Console.WriteLine(_DistinctID + "ALREADY DISTROYED");
                    foreach (Command com in commands)
                    {
                        Console.WriteLine(com.DistinctID + "--" + com.Heading);
                    }
                    return false;
                }
                else
                {
                    Console.WriteLine(_DistinctID + "AND slot:" + ToBeChanged.Heading.ToString() + " DISTROYED");
                    commands.Remove(ToBeChanged);
                    return true;
                }
            }
        }
        public async Task<bool> UpdateCurentSlot(string _DistinctID,Int32 slot)
        {
            lock (Command.lockComand)
            {
                Command ToBeChanged = commands.Find(x => (x.DistinctID == _DistinctID));
                if (ToBeChanged == null)
                {
                    return false;
                }
                else
                {
                    ToBeChanged.CurrentSlot = slot;
                    return true;
                }
            }
        }
    }
}
