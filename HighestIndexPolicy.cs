using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduling
{
    class HighestIndexPolicy : SchedulingPolicy
    {
        private Queue<int> HI_processes = new Queue<int>(); 

 
        public override int NextProcess(Dictionary<int, ProcessTableEntry> dProcessTable)
        {
            int highestIndex = 0;

            // הוספת הרעבה לכל התהליכים שמוכנים לרוץ
            foreach (int processID in HI_processes)
            {
                if (!dProcessTable[processID].Done && !dProcessTable[processID].Blocked)
                {
                    dProcessTable[processID].MaxStarvation++;
                }
            }

            foreach (int processID in HI_processes)
            {
                if (!dProcessTable[processID].Done && !dProcessTable[processID].Yield && !dProcessTable[processID].Blocked)
                {
                    if (processID > highestIndex || highestIndex == 0)
                        highestIndex = processID;
                }
            }

            // עדכון הרעבה עבור כל התהליכים שלא נבחרו
            foreach (int processID in HI_processes)
            {
                if (processID != highestIndex && !dProcessTable[processID].Done && !dProcessTable[processID].Blocked)
                {
                    dProcessTable[processID].MaxStarvation++;
                }
            }

            if (highestIndex != 0 && dProcessTable.ContainsKey(highestIndex) && !dProcessTable[highestIndex].Blocked)
            {
                dProcessTable[highestIndex].MaxStarvation = 0; // איפוס הרעבה של התהליך שנבחר
                return highestIndex;
            }
            return 0;
        }


        public override void AddProcess(int iProcessId)
        {
            HI_processes.Enqueue(iProcessId);
        }

        public override bool RescheduleAfterInterrupt()
        {
            return true; 
        }
    }
}
