using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Scheduling
{
    class RoundRobin : SchedulingPolicy
    {
        int curProcessId = 0;
        int Round_Quantum;
        List<int> RR_processes;

        public RoundRobin(int quantum)
        {
            Quantum = quantum;
            RR_processes = new List<int>();
        }

        public override int NextProcess(Dictionary<int, ProcessTableEntry> dProcessTable)
        {
            foreach (int processID in RR_processes)//הרעבה
            {
                if (!dProcessTable[processID].Done && !dProcessTable[processID].Blocked)
                {
                    dProcessTable[processID].MaxStarvation++;
                }
            }

            if (RR_processes.Count == 0)
                return -1;
            if (curProcessId == 0)
            {
                curProcessId = 1;
                return curProcessId;
            }

            if (curProcessId == RR_processes.Count - 1)
            {
                for (int i = 1; i < RR_processes.Count; i++)
                {
                    int nextProcessId = RR_processes[i];
                    var nextProcess = dProcessTable[nextProcessId];
                    if (!nextProcess.Yield &&  !nextProcess.Blocked && !nextProcess.Done)
                    {
                        if(dProcessTable[curProcessId].Quantum == 0)
                            dProcessTable[curProcessId].Quantum = Quantum;
                        curProcessId = nextProcessId;
                        dProcessTable[nextProcessId].MaxStarvation = 0;
                        return nextProcessId; // החזר את מזהה התהליך הנוכחי
                    }
                }
            }
            else
            {
                for (int i = curProcessId; i < RR_processes.Count()-1; i++)
                {
                    int nextProcessId = i+1;
                    var nextProcess = dProcessTable[nextProcessId];

                    if (!nextProcess.Done && !nextProcess.Blocked && !nextProcess.Yield)
                    {
                        if (dProcessTable[curProcessId].Quantum == 0)
                            dProcessTable[curProcessId].Quantum = Quantum; curProcessId = nextProcessId;
                        nextProcess.MaxStarvation = 0;
                        return nextProcessId;
                    }
                }
            }
            if (dProcessTable[curProcessId].Quantum == 0)
                dProcessTable[curProcessId].Quantum = Quantum;
            curProcessId = 1;
            for (int i = 1; i < RR_processes.Count(); i++)
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
        }



        public override void AddProcess(int iProcessId)
        {
            RR_processes.Add(iProcessId);
        }

        public override bool RescheduleAfterInterrupt()
        {
            return true;
        }
    }
}
