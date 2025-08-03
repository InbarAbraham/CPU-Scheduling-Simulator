using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Scheduling
{
    class FirstComeFirstServedPolicy : SchedulingPolicy
    {
        List<int> FCFS_processes = new List<int>();
        int curProcessId = 1;
        


        public override int NextProcess(Dictionary<int, ProcessTableEntry> dProcessTable)
        {            
            foreach (int processID in FCFS_processes)
            {
                if (!dProcessTable[processID].Done && !dProcessTable[processID].Blocked)
                {
                    dProcessTable[processID].MaxStarvation++;
                }
            }
            var curProcess = dProcessTable[curProcessId];
            if (FCFS_processes.Count == 0)
                return -1;

            if (curProcessId == 1 && !curProcess.Blocked && !curProcess.Done && !curProcess.Yield)
            {
                return curProcessId;
            }

            if (curProcessId == FCFS_processes.Count()-1)
            {
                for (int i = 1; i < FCFS_processes.Count(); i++)
                {
                    int nextProcessId = i;
                    var nextProcess = dProcessTable[nextProcessId];

                    if (!nextProcess.Done && !nextProcess.Blocked && !nextProcess.Yield)
                    {
                        curProcessId = nextProcessId;
                        nextProcess.MaxStarvation = 0;
                        return nextProcessId;
                    }
                }
            }
            else
            {
                for (int i = curProcessId; i < FCFS_processes.Count()-1; i++)
                {
                    int nextProcessId = i+1;
                    var nextProcess = dProcessTable[nextProcessId];


                    if (!nextProcess.Done && !nextProcess.Blocked && !nextProcess.Yield)
                    {
                        curProcessId = nextProcessId;
                        nextProcess.MaxStarvation = 0;
                        return nextProcessId;
                    }
                }
            }
            curProcessId = 1;
            for(int i = 1; i < FCFS_processes.Count(); i++)
            {
                int nextProcessId = i;
                var nextProcess = dProcessTable[nextProcessId];
                if (!nextProcess.Done && !nextProcess.Blocked && !nextProcess.Yield)
                {
                    curProcessId = nextProcessId;
                    nextProcess.MaxStarvation = 0;
                    return nextProcessId;
                }
            }
            return 0;


       //     return 0; // החזר את מזהה התהליך הנוכחי
        }



        public override void AddProcess(int iProcessId)
        {
            FCFS_processes.Add(iProcessId); 
        }

        public override bool RescheduleAfterInterrupt()
        {
            return true; 
        }
    }
}
