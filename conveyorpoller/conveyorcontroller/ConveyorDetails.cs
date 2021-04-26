using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace conveyorcontroller
{
    internal class Command
    {
        public Int32 ConveyorNumber { get; set; }
        public Int32 CurrentSlot { get; set; }
        public Int32 Heading { get; set; }
        public string DistinctID { get; set; }
        public Boolean Completed { get; set; }
        public string Message { get; set; }
        public Boolean Error { get; set; }
        public Boolean Reported { get; set; }
        public Int32 type { get; set; }

       

}
   
    internal class conveyorRange
    {
        
        public  conveyorRange(Int32 _start,Int32 _end,Int32 _cn)
        {
            start = _start;
            end = _end;
            conveyor_number = _cn;
        }
        public Int32 start { get; set; }
        public Int32 end { get; set; }
        public Int32 conveyor_number { get; set; }
    }
    internal class ConveyorDetails
    {
        private Int32 conveyorConnectionRange = 22840;
        Int32 TotalConveyors = 9;

        Dictionary<conveyorRange, Int32> conveyorAdjustment = new Dictionary<conveyorRange, int>();
        public Int32 adjustSlot(Int32 slot)
        {
            foreach (KeyValuePair<conveyorRange, Int32> entry in conveyorAdjustment)
            {
                if(slot> entry.Key.start-1 && slot < entry.Key.end + 1)
                {
                    return slot + entry.Value;
                }
            }
            return slot;
        }
        public Int32 getConveyourNumber(Int32 slot)
        {
            foreach (KeyValuePair<conveyorRange, Int32> entry in conveyorAdjustment)
            {
                if (slot > entry.Key.start - 1 && slot < entry.Key.end + 1)
                {
                    return entry.Key.conveyor_number;
                }
            }
            return 0;
        }
        public Int32 getConnection (Int32 slot)
        {
            if(slot> conveyorConnectionRange)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        } 
        public  ConveyorDetails()
        {
            conveyorAdjustment.Add(new conveyorRange(1    ,  1151,1) , 0);
            conveyorAdjustment.Add(new conveyorRange(1151 ,  2560,2) , 1410);
            conveyorAdjustment.Add(new conveyorRange(2561 ,  3970,2) , 0);
            conveyorAdjustment.Add(new conveyorRange(3971 ,  5600,3) , 1630);
            conveyorAdjustment.Add(new conveyorRange(5601 ,  7230,3) , 0);
            conveyorAdjustment.Add(new conveyorRange(7231 ,  8380,4) , 0);
            conveyorAdjustment.Add(new conveyorRange(8381 ,  9790,5) , 1410);
            conveyorAdjustment.Add(new conveyorRange(9791 ,  11200,5), 0);
            conveyorAdjustment.Add(new conveyorRange(11201, 12830,6) , 1630);
            conveyorAdjustment.Add(new conveyorRange(12831, 14460,6) , 0);
            conveyorAdjustment.Add(new conveyorRange(14461, 15610,7) , 1150);
            conveyorAdjustment.Add(new conveyorRange(15611, 16760,7) , 0);
            conveyorAdjustment.Add(new conveyorRange(16761, 18170,8) , 1410);
            conveyorAdjustment.Add(new conveyorRange(18171, 19580,8) , 0);
            conveyorAdjustment.Add(new conveyorRange(19581, 21210,9) , 1630);
            conveyorAdjustment.Add(new conveyorRange(21211, 22840,9) , 0);
            conveyorAdjustment.Add(new conveyorRange(22841, 23990,10), 1150);
            conveyorAdjustment.Add(new conveyorRange(23991, 25140,10), 0);
            conveyorAdjustment.Add(new conveyorRange(25141, 26550,11), 1410);
            conveyorAdjustment.Add(new conveyorRange(26551, 27960,11), 0);
            conveyorAdjustment.Add(new conveyorRange(27961, 29590,12), 1630);
            conveyorAdjustment.Add(new conveyorRange(29591, 31220,12), 0);
            conveyorAdjustment.Add(new conveyorRange(31221, 32370,13), 1150);
            conveyorAdjustment.Add(new conveyorRange(32371, 33520,13), 0);
            conveyorAdjustment.Add(new conveyorRange(33521, 34930,14), 1410);
            conveyorAdjustment.Add(new conveyorRange(34931, 36340,14), 0);
            conveyorAdjustment.Add(new conveyorRange(36341, 37970,15), 1630);
            conveyorAdjustment.Add(new conveyorRange(37971, 39600,15), 0);
            ////66666////
            conveyorAdjustment.Add(new conveyorRange(39601, 40740, 16), 0);
            conveyorAdjustment.Add(new conveyorRange(40741, 42140, 17), 1400);
            conveyorAdjustment.Add(new conveyorRange(42141, 43540, 17), 0);
            conveyorAdjustment.Add(new conveyorRange(43541, 45170, 18), 1630);
            conveyorAdjustment.Add(new conveyorRange(45171, 46800, 18), 0);
            
        }
      
    }
    
}
