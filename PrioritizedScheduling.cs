using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduling
{
    class PrioritizedScheduling : SchedulingPolicy
    {
        private Queue<int> PS_processes; 
        private int Priorit_Quantum;

        public PrioritizedScheduling(int iQuantum)
        {
            PS_processes = new Queue<int>();
            Quantum = iQuantum;
        }


        public override int NextProcess(Dictionary<int, ProcessTableEntry> dProcessTable)
        {
            if (PS_processes.Count == 0)
            {
                return -1; // אין תהליכים בתור
            }

            List<int> sortedProcesses = PS_processes
                .OrderByDescending(id => dProcessTable[id].Priority)
                .ToList(); // מיון התהליכים לפי עדיפות

            PS_processes = new Queue<int>(sortedProcesses); // עדכון התור לפי סדר מיון
            int initialCount = PS_processes.Count;

            for (int i = 0; i < initialCount; i++)
            {
                int curProcessID = PS_processes.Dequeue();
                ProcessTableEntry curProcess = dProcessTable[curProcessID];

                // הוספת הרעבה לכל התהליכים שמוכנים לרוץ
                foreach (int processID in PS_processes)
                {
                    if (!dProcessTable[processID].Done && !dProcessTable[processID].Blocked)
                    {
                        dProcessTable[processID].MaxStarvation++;
                    }
                }

                if (!curProcess.Done && !curProcess.Blocked && !curProcess.Yield)
                {
                    if (curProcess.Quantum == 0)
                    {
                        curProcess.Quantum = Priorit_Quantum; 
                    }
                    else
                    {
                        curProcess.Quantum--; 
                    }

                    dProcessTable[curProcessID].MaxStarvation = 0; // איפוס הרעבה של התהליך שנבחר
                    PS_processes.Enqueue(curProcessID); 
                    return curProcessID; 
                }

                // תהליך לא מוכן, החזר אותו לתור
                PS_processes.Enqueue(curProcessID);
            }

            return 0;
        }



        public override void AddProcess(int iProcessId)
        {
            PS_processes.Enqueue(iProcessId); 
        }

        public override bool RescheduleAfterInterrupt()
        {
            return true; 
        }
    }
}
