using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduling
{
    class OperatingSystem
    {
        public Disk Disk { get; private set; }
        public CPU CPU { get; private set; }
        private Dictionary<int, ProcessTableEntry> m_dProcessTable;
        private List<ReadTokenRequest> m_lReadRequests;
        private int m_cProcesses;
        private SchedulingPolicy m_spPolicy;
        private static int IDLE_PROCESS_ID = 0;

        public OperatingSystem(CPU cpu, Disk disk, SchedulingPolicy sp)
        {
            CPU = cpu;
            Disk = disk;
            m_dProcessTable = new Dictionary<int, ProcessTableEntry>();
            m_lReadRequests = new List<ReadTokenRequest>();
            cpu.OperatingSystem = this;
            disk.OperatingSystem = this;
            m_spPolicy = sp;
            
            // יצירת תהליך Idle
            m_cProcesses = 0;

            IdleCode idleCode = new IdleCode();
            m_dProcessTable[m_cProcesses] = new ProcessTableEntry(m_cProcesses, "Idle", idleCode)
            {
                StartTime = CPU.TickCount
            };
            m_dProcessTable[m_cProcesses].StartTime = CPU.TickCount;
            m_spPolicy.AddProcess(m_cProcesses);// הוספת התהליך למדיניות התזמון
            m_cProcesses++;// עדכון מונה התהליכים
        }

        public void CreateProcess(string sCodeFileName)
        {
            Code code = new Code(sCodeFileName);
            m_dProcessTable[m_cProcesses] = new ProcessTableEntry(m_cProcesses, sCodeFileName, code);
            m_dProcessTable[m_cProcesses].StartTime = CPU.TickCount;
            m_dProcessTable[m_cProcesses].Quantum = m_spPolicy.Quantum;
            m_spPolicy.AddProcess(m_cProcesses);
            m_cProcesses++;
        }

        public void CreateProcess(string sCodeFileName, int iPriority)
        {
            Code code = new Code(sCodeFileName);
            m_dProcessTable[m_cProcesses] = new ProcessTableEntry(m_cProcesses, sCodeFileName, code);
            m_dProcessTable[m_cProcesses].Priority = iPriority;
            m_dProcessTable[m_cProcesses].StartTime = CPU.TickCount;
            m_spPolicy.AddProcess(m_cProcesses);
            m_cProcesses++;
        }

        public void ProcessTerminated(Exception e)
        {
            if (e != null)
                Console.WriteLine("Process " + CPU.ActiveProcess + " terminated unexpectedly. " + e);
            m_dProcessTable[CPU.ActiveProcess].Done = true;
            m_dProcessTable[CPU.ActiveProcess].Console.Close();
            m_dProcessTable[CPU.ActiveProcess].EndTime = CPU.TickCount;
            ActivateScheduler();
        }

        public void TimeoutReached()
        {
            m_dProcessTable[CPU.ActiveProcess].Quantum = 0;
            ActivateScheduler();
        }

        public void ReadToken(string sFileName, int iTokenNumber, int iProcessId, string sParameterName)
        {
            ReadTokenRequest request = new ReadTokenRequest();
            request.ProcessId = iProcessId;
            request.TokenNumber = iTokenNumber;
            request.TargetVariable = sParameterName;
            request.Token = null;
            request.FileName = sFileName;
            m_dProcessTable[iProcessId].Blocked = true;
            if (Disk.ActiveRequest == null)
                Disk.ActiveRequest = request;
            else
                m_lReadRequests.Add(request);
            CPU.ProgramCounter = CPU.ProgramCounter + 1;
            ActivateScheduler();
        }
        
        public void Interrupt(ReadTokenRequest rFinishedRequest)  
        {
            double tokenValue;
            if (rFinishedRequest.Token == null)
                tokenValue = double.NaN;
            else
                tokenValue = Convert.ToDouble(rFinishedRequest.Token);

            int processId = rFinishedRequest.ProcessId;
            string targetVariable = rFinishedRequest.TargetVariable;
            m_dProcessTable[processId].AddressSpace[targetVariable] = tokenValue;

            // ביטול מצב חסום של התהליך
            m_dProcessTable[processId].Blocked = false;

            // טיפול בבקשות נוספות בתור והפעלת הדיסק
            if (m_lReadRequests.Count > 0)
            {
                var nextRequest = m_lReadRequests[0];
                m_lReadRequests.RemoveAt(0);
                Disk.ActiveRequest = nextRequest;
            }

            if (m_spPolicy.RescheduleAfterInterrupt())
                ActivateScheduler();
        }



        private ProcessTableEntry ContextSwitch(int iEnteringProcessId) 
        {
            int currentProcessId = CPU.ActiveProcess;
            ProcessTableEntry exitingProcess = null;

            if (currentProcessId != -1 && m_dProcessTable.ContainsKey(currentProcessId))
            {
                m_dProcessTable[currentProcessId].ProgramCounter = CPU.ProgramCounter;
                m_dProcessTable[currentProcessId].LastCPUTime = CPU.TickCount;
                m_dProcessTable[currentProcessId].AddressSpace = CPU.ActiveAddressSpace;
                m_dProcessTable[currentProcessId].Console = CPU.ActiveConsole;
                exitingProcess = m_dProcessTable[currentProcessId]; // שמירת התהליך היוצא
            }

            CPU.ActiveProcess = iEnteringProcessId; 
            CPU.ActiveAddressSpace = m_dProcessTable[iEnteringProcessId].AddressSpace;
            CPU.ActiveConsole = m_dProcessTable[iEnteringProcessId].Console;
            CPU.ProgramCounter = m_dProcessTable[iEnteringProcessId].ProgramCounter;
            
            if (m_spPolicy is PrioritizedScheduling || m_spPolicy is RoundRobin)
            {
                CPU.RemainingTime = m_dProcessTable[iEnteringProcessId].Quantum;
            }
            else
            {
                CPU.RemainingTime = -1; // ערך ברירת מחדל אם אין Quantum
            }
            return exitingProcess;
        }


        public void ActivateScheduler() 
        {
            int iNextProcessId = m_spPolicy.NextProcess(m_dProcessTable);
            if (iNextProcessId == -1)
            {
                Console.WriteLine("All processes terminated or blocked.");
                CPU.Done = true;
            }
            else
            {
                bool bOnlyIdleRemains = true ;
                //add code here to check if only the Idle process remains
                foreach (var process in m_dProcessTable.Values)
                {
                    if (!process.Done && process.ProcessId != IDLE_PROCESS_ID)
                    {
                        bOnlyIdleRemains = false;
                        break;
                    }
                }
                if (bOnlyIdleRemains)
                {
                    Console.WriteLine("Only idle remains.");
                    CPU.Done = true;
                }
                else
                    ContextSwitch(iNextProcessId);
            }
        }


        public double AverageTurnaround() 
        {
            double totalTurnaroundTime = 0;
            int completedProcesses = 0;

            foreach (var process in m_dProcessTable.Values)
            {
                if (process.Done)
                {
                    // חישוב זמן סיום - זמן התחלה
                    totalTurnaroundTime += (process.EndTime - process.StartTime);
                    completedProcesses++;
                }
            }
            // החזרת ממוצע זמן הסיום או 0 אם אין תהליכים שהסתיימו
            if (completedProcesses != 0)
            {
                return totalTurnaroundTime / completedProcesses;
            }
            else
            {
                return 0.0;
            }
        }


        public int MaximalStarvation() 
        {
            int maxStarvation = 0;
            foreach (var process in m_dProcessTable.Values)
            {
                // נבדוק רק תהליכים שסיימו
                if (process.Done)
                {
                    // נוודא שההרעבה שלהם מעודכן ונבדוק אם הוא גדול מהמקסימום הנוכחי
                    if (process.MaxStarvation > maxStarvation)
                    {
                        maxStarvation = process.MaxStarvation;
                    }
                }
            }
            return maxStarvation;
        }


        public void SetYieldState(int processId)   
        {
            m_dProcessTable[processId].Yield = true;
        }
    }
}
