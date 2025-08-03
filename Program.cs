using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading;
using System.IO;

namespace Scheduling
{
    class Program
    {
        static void Example1(OperatingSystem os)
        {
            for (int i = 0; i < 3; i++)
            {
                os.CreateProcess("a.code");
                os.CreateProcess("b.code");
            }
        }
        static void YieldExample(OperatingSystem os)
        {
             os.CreateProcess("e.code");
             os.CreateProcess("e_yield.code");
            
        }
        static void Example1_1(OperatingSystem os)
        {
            for (int i = 0; i < 2; i++)
            {
                os.CreateProcess("a.code");
                os.CreateProcess("b.code");
                os.CreateProcess("c.code");
                os.CreateProcess("d.code");
            }
        }
        static void Example1_1Priority(OperatingSystem os)
        {
            for (int i = 0; i < 2; i++)
            {
                os.CreateProcess("a.code", i);
                os.CreateProcess("b.code", i);
                os.CreateProcess("c.code", i + 1);
                os.CreateProcess("d.code", i + 2);
            }
        }
        static void Example2(OperatingSystem os)
        {
            for (int i = 0; i < 3; i++)
            {
                os.CreateProcess("ReadFile1.code");
                os.CreateProcess("ReadFile2.code");
            }
        }
        static void Example3(OperatingSystem os)
        {
            for (int i = 0; i < 3; i++)
            {
                os.CreateProcess("c.code");
                os.CreateProcess("d.code");
            }
        }
        static void Example4(OperatingSystem os)
        {
            for (int i = 0; i < 2; i++)
            {
                os.CreateProcess("a.code");
                os.CreateProcess("b.code");
                os.CreateProcess("c.code");
                os.CreateProcess("d.code");
                os.CreateProcess("ReadFile1.code");
                os.CreateProcess("ReadFile2.code");
                os.CreateProcess("PrimeNumbers.code");
            }
        }
        static void Example4Priorities(OperatingSystem os)
        {
            for (int i = 0; i < 2; i++)
            {
                os.CreateProcess("a.code", i);
                os.CreateProcess("b.code", i);
                os.CreateProcess("c.code", i);
                os.CreateProcess("d.code", i + 1);
                os.CreateProcess("ReadFile1.code", i + 1);
                os.CreateProcess("ReadFile2.code", i + 2);
                os.CreateProcess("PrimeNumbers.code", i  + 2);
            }
        }
        static void ContextSwitchTest()
        {
            Disk disk = new Disk();
            CPU cpu = new CPU(disk);
            cpu.Debug = false;
            OperatingSystem os = new OperatingSystem(cpu, disk, new HighestIndexPolicy());
            Example1(os);
            os.ActivateScheduler();
            cpu.Execute();
           // ProcessConsole.CloseAll();
            Thread.Sleep(1000);
        }
        static void TestIdle()
        {
            Disk disk = new Disk();
            CPU cpu = new CPU(disk);
            cpu.Debug = false;
            OperatingSystem os = new OperatingSystem(cpu, disk, new HighestIndexPolicy());
            os.ActivateScheduler();
            cpu.Execute();
            // ProcessConsole.CloseAll();
            Thread.Sleep(1000);
        }
        static void ContextSwitchTest2()
        {
            Disk disk = new Disk();
            CPU cpu = new CPU(disk);
            cpu.Debug = false;
            OperatingSystem os = new OperatingSystem(cpu, disk, new HighestIndexPolicy());
            Example1_1(os);
            os.ActivateScheduler();
            cpu.Execute();
            cpu.Done = true;
            Thread.Sleep(1000);
        }
        static void InterruptTest()
        {
            Disk disk = new Disk();
            CPU cpu = new CPU(disk);
            cpu.Debug = false;
            OperatingSystem os = new OperatingSystem(cpu, disk, new HighestIndexPolicy());
            Example2(os);
            os.ActivateScheduler();
            cpu.Execute();
           // ProcessConsole.CloseAll();
            Thread.Sleep(1000);
        }
        static void InterruptTest2()
        {
            Disk disk = new Disk();
            CPU cpu = new CPU(disk);
            cpu.Debug = false;
            OperatingSystem os = new OperatingSystem(cpu, disk, new HighestIndexPolicy());
            Example4(os);
            os.ActivateScheduler();
            cpu.Execute();
            cpu.Done = true;
            Thread.Sleep(1000);
        }

        static void YieldTest()
        {
            Disk disk = new Disk();
            CPU cpu = new CPU(disk);
            cpu.Debug = false;
            OperatingSystem os = new OperatingSystem(cpu, disk, new FirstComeFirstServedPolicy());
            Example1(os);
            os.ActivateScheduler();
            cpu.Execute();
            // ProcessConsole.CloseAll();
            Thread.Sleep(1000);
        }



        static void FirstComeFirstServedPolicySimpleWithOutFiles()
        {
            Disk disk = new Disk();
            CPU cpu = new CPU(disk);
            cpu.Debug = false;
            OperatingSystem os = new OperatingSystem(cpu, disk, new FirstComeFirstServedPolicy());
            Example1_1(os);
            os.ActivateScheduler();
            cpu.Execute();
            cpu.Done = true;
            Thread.Sleep(1000);
        }
     
        static void FirstComeFirstServedPolicySimpleWithFiles()
        {
            Disk disk = new Disk();
            CPU cpu = new CPU(disk);
            cpu.Debug = false;
            OperatingSystem os = new OperatingSystem(cpu, disk, new FirstComeFirstServedPolicy());
            Example4(os);
            os.ActivateScheduler();
            cpu.Execute();
            cpu.Done = true;
            Thread.Sleep(1000);
        }

        static void RoundRobinPolicySimpleWithOutFiles(int x)
        {
            Disk disk = new Disk();
            CPU cpu = new CPU(disk);
            cpu.Debug = false;
            OperatingSystem os = new OperatingSystem(cpu, disk, new RoundRobin(x));
            Example1_1(os);
            os.ActivateScheduler();
            cpu.Execute();
            cpu.Done = true;
            Thread.Sleep(1000);
        }

        static void PriorityPolicySimpleWithOutFiles(int x)
        {
            Disk disk = new Disk();
            CPU cpu = new CPU(disk);
            cpu.Debug = false;
            OperatingSystem os = new OperatingSystem(cpu, disk, new PrioritizedScheduling(x));
            Example1_1Priority(os);
            os.ActivateScheduler();
            cpu.Execute();
            cpu.Done = true;
            Thread.Sleep(1000);
        }

        static void RoundRobinPolicySimpleWithFiles(int x)
        {
            Disk disk = new Disk();
            CPU cpu = new CPU(disk);
            cpu.Debug = false;
            OperatingSystem os = new OperatingSystem(cpu, disk, new RoundRobin(x));
            Example4(os);
            os.ActivateScheduler();
            cpu.Execute();
            cpu.Done = true;
            Thread.Sleep(1000);
        }
        static void PriorityPolicySimpleWithFiles(int x)
        {
            Disk disk = new Disk();
            CPU cpu = new CPU(disk);
            cpu.Debug = false;
            OperatingSystem os = new OperatingSystem(cpu, disk, new PrioritizedScheduling(x));
            Example4Priorities(os);
            os.ActivateScheduler();
            cpu.Execute();
            cpu.Done = true;
            Thread.Sleep(1000);
        }

        static void FirstComeFirstServedPolicySimpleWithFilesAM(out double avg, out double max)
        {
            Disk disk = new Disk();
            CPU cpu = new CPU(disk);
            cpu.Debug = false;
            OperatingSystem os = new OperatingSystem(cpu, disk, new FirstComeFirstServedPolicy());
            Example4(os);
            os.ActivateScheduler();
            cpu.Execute();
            cpu.Done = true;
            Thread.Sleep(1000);
            avg = os.AverageTurnaround();
            max = os.MaximalStarvation();
        }
        static void RoundRobinPolicySimpleWithFilesAM(int x, out double avg, out double max)
        {
            Disk disk = new Disk();
            CPU cpu = new CPU(disk);
            cpu.Debug = false;
            OperatingSystem os = new OperatingSystem(cpu, disk, new RoundRobin(x));
            Example4(os);
            os.ActivateScheduler();
            cpu.Execute();
            cpu.Done = true;
            Thread.Sleep(1000);

            avg = os.AverageTurnaround();
            max = os.MaximalStarvation();
        }


        static bool CompareToExpected(int cProcesses, int iTestType)
        {
            for (int i = 1; i <= cProcesses; i++)
            {
                try
                {
                    if (!CompareFiles("ProcessLog." + i + "." + iTestType + ".expected.txt", "ProcessLog." + i + ".txt"))
                        return false;
                }
                catch (IOException e)
                {
                    return false;
                }

            }
            return true;
        }
        static bool CompareFiles(string sFile1, string sFile2)
        {
            StreamReader sr1 = null, sr2 = null;
            try
            {
                sr1 = new StreamReader(sFile1);
                sr2 = new StreamReader(sFile2);
                string s1 = sr1.ReadToEnd();
                string s2 = sr2.ReadToEnd();
                sr1.Close();
                sr2.Close();
                if (s1 != s2)
                {
                    int t = 9;
                }
                return s1 == s2;
            }
            catch(Exception ex)
            {
                if (sr1 != null)
                    sr1.Close();
                if (sr2 != null)
                    sr2.Close();
                return false;
            }
        }

        static bool WaitForThread(Thread t)
        {
            t.Join(60000);
            if (t.IsAlive)
            {
                t.Abort();
                ProcessConsole.CloseAll();
                return false;
            }
            return true;
        }

        static StreamWriter sw = null;

        static void WriteResults(string sMsg)
        {
            sw = new StreamWriter("results.txt", true);
            sw.Write(sMsg);
            sw.Close();
        }
        static void WriteResultsLine(string sMsg)
        {
            sw = new StreamWriter("results.txt", true);
            sw.WriteLine(sMsg);
            sw.Close();
        }
        static void WriteResultsLine()
        {
            WriteResultsLine("");
        }

        static void Main(string[] args)
        {

            Directory.SetCurrentDirectory("C:\\Users\\97252\\Desktop\\לימודים\\תואר\\שנה ג\\סמסטר א\\מערכות הפעלה\\תרגילי בית\\תרגיל בית 2\\סופי ואחרון- להציג בהגנה השנייה\\תרגיל בית 2 גיל ועינבר עם כל התיקונים מושלם\\Scheduling\\Code files");

            sw = new StreamWriter("results.txt");
            sw.Close();
            IdleTest(sw);
            ContextSwitchTest1(sw);
            ContextSwitchTest2(sw);
            InterruptTest1(sw);
            InterruptTest2(sw);
            YieldTest(sw);
            FirstComeFirstServedTest1(sw);
            FirstComeFirstServedTest2(sw);
            RoundRobinTest1(sw);
            RoundRobinTest2(sw);
            RoundRobinTest3(sw);
            RoundRobinTest4(sw);
            FirstComeFirstServedATAndMaxStarvationTest(sw);
            RoundRobinATAndMaxStarvationTest(sw);
        }

        static public void IdleTest(StreamWriter sr)
        {
            try
            {
                Thread t = null;
                WriteResults("Idle Test_");
                ProcessConsole.Init();
                t = new Thread(TestIdle);
                t.Start();

                if (WaitForThread(t))
                {
                    ProcessConsole.CloseAll();
                    if (CompareFiles("TerminateOrder.txt", "Idle.expected.txt"))
                        WriteResults("Succeeded_");
                    else
                        WriteResults("Fail_");
                }
                else
                {
                    WriteResults("Process stuck in loop_");
                }
            }
            catch (Exception ex)
            {

                WriteResults("Fail - Process throw exception: - " + ex.ToString());
            }
            WriteResultsLine();
        }
        static public void ContextSwitchTest1(StreamWriter sr)
        {
            try
            {
                Thread t;
                WriteResults("Test ContextSwitch method - NUMBER 1_");

                ProcessConsole.Init();
                t = new Thread(ContextSwitchTest);
                t.Start();

                if (WaitForThread(t))
                {
                    ProcessConsole.CloseAll();
                    if (CompareToExpected(6, 1))
                    {
                        
                    }
                    else
                    {
                        WriteResults("Wrong processes output_");
                        WriteResultsLine();
                        return;
                    }

                    if (CompareFiles("TerminateOrder.txt", "TerminateOrder.HighestIndexPolicy.1.expected.txt"))
                        WriteResults("Succeeded_");
                    else
                        WriteResults("Wrong order of process termination_");
                }
                else
                {
                    WriteResults("Process stuck in loop_");
                }
            }
            catch (Exception ex)
            {
                WriteResults("Fail - Process throw exception: - " + ex.ToString());
            }
            WriteResultsLine();
        }
        static public void ContextSwitchTest2(StreamWriter sr)
        {
            try
            {
                Thread t;
                WriteResults("Test ContextSwitch method - NUMBER 2_");
                ProcessConsole.Init();
                t = new Thread(ContextSwitchTest2);
                t.Start();
                if (WaitForThread(t))
                {
                    ProcessConsole.CloseAll();
                    if (CompareToExpected(4, 4))
                    {

                    }
                    else
                    {
                        WriteResults("Wrong processes output_");
                        WriteResultsLine();
                        return;
                    }

                    if (CompareFiles("TerminateOrder.txt", "TerminateOrder.HighestIndexPolicy.2.expected.txt"))
                        WriteResults("Succeeded_");
                    else
                        WriteResults("Wrong order of process termination_");
                }
                else
                {
                    WriteResults("Process stuck in loop_");
                }
            }
            catch (Exception ex)
            {

                WriteResults("Fail - Process throw exception: - " + ex.ToString());
            }
            WriteResultsLine();
        }
        static public void InterruptTest1(StreamWriter sr)
        {
            try
            {
                Thread t;
                WriteResults("Test Interrupt method - NUMBER 1 (processes access files)_");
                ProcessConsole.Init();
                t = new Thread(InterruptTest);
                t.Start();
                if (WaitForThread(t))
                {
                    ProcessConsole.CloseAll();
                    if (CompareToExpected(6, 2))
                    {

                    }
                    else
                    {
                        WriteResults("Wrong processes output_");
                        WriteResultsLine();
                        return;
                    }

                    if (CompareFiles("TerminateOrder.txt", "TerminateOrder.HighestIndexPolicy.3.expected.txt"))
                        WriteResults("Succeeded ");
                    else
                        WriteResults("Wrong order of process termination_");
                }
                else
                {
                    WriteResults("Process stuck in loop_");
                }
            }
            catch (Exception ex)
            {

                WriteResults("Fail - Process throw exception: " + ex.ToString());
            }
            WriteResultsLine();
        }
        static public void InterruptTest2(StreamWriter sr)
        {
            try
            {
                Thread t;
                WriteResults("Test ContextSwitch method - NUMBER 2 (processes access files)_");
                ProcessConsole.Init();
                t = new Thread(InterruptTest2);
                t.Start();
                if (WaitForThread(t))
                {
                    ProcessConsole.CloseAll();
                    if (CompareToExpected(14, 3))
                    {

                    }
                    else
                    {
                        WriteResults("Wrong processes output_");
                        WriteResultsLine();
                        return;
                    }

                    if (CompareFiles("TerminateOrder.txt", "TerminateOrder.HighestIndexPolicy.4.expected.txt"))
                        WriteResults("Succeeded ");
                    else
                        WriteResults("Wrong order of process termination_");
                }
                else
                {
                    WriteResults("Process stuck in loop_");
                }
            }
            catch (Exception ex)
            {

                WriteResults("Fail-Process throw exception: " + ex.ToString());
            }
            WriteResultsLine();
        }
        static public void YieldTest(StreamWriter sr)
        {
            try
            {
                WriteResults("Yield Test_");
                Thread t;
                ProcessConsole.Init();
                t = new Thread(YieldTest);
                t.Start();
                if (WaitForThread(t))
                {
                    WriteResults("Succeeded_");
                }
                else
                {
                    WriteResults("Process stuck in loop_");
                }
                ProcessConsole.CloseAll();
            }
            catch (Exception ex)
            {

                WriteResults("Fail - Process throw exception: - " + ex.ToString());
            }
            WriteResultsLine();
        }
        static public void FirstComeFirstServedTest1(StreamWriter sr)
        {
            try
            {
                Thread t;
                WriteResults("Test FirstComeFirstServed Policy - NUMBER 1 (processes dont access files)_");
                ProcessConsole.Init();
                t = new Thread(FirstComeFirstServedPolicySimpleWithOutFiles);
                t.Start();
                if (WaitForThread(t))
                {
                    ProcessConsole.CloseAll();
                    if (CompareToExpected(4, 4))
                    {

                    }
                    else
                    {
                        WriteResults("Wrong processes output_");
                        WriteResultsLine();
                        return;
                    }

                    if (CompareFiles("TerminateOrder.txt", "TerminateOrder.FirstComeFirstServed.1.expected.txt"))
                        WriteResults("Succeeded_");
                    else
                        WriteResults("Wrong order of process termination_");
                }
                else
                {
                    WriteResults("Process stuck in loop_");
                }
            }
            catch (Exception ex)
            {

                WriteResults("Fail - Process throw exception: - " + ex.ToString());
            }
            WriteResultsLine();
        }
        static public void FirstComeFirstServedTest2(StreamWriter sr)
        {
            try
            {
                Thread t;
                WriteResults("Test FirstComeFirstServed Policy - NUMBER 2 (processes access files)_");
                ProcessConsole.Init();
                t = new Thread(FirstComeFirstServedPolicySimpleWithFiles);
                t.Start();
                if (WaitForThread(t))
                {
                    ProcessConsole.CloseAll();
                    if (CompareToExpected(14, 3))
                    {

                    }
                    else
                    {
                        WriteResults("Wrong processes output_");
                        WriteResultsLine();
                        return;
                    }


                    if (CompareFiles("TerminateOrder.txt", "TerminateOrder.FirstComeFirstServed.2.expected.txt"))
                        WriteResults("Succeeded_");
                    else
                        WriteResults("Wrong order of process termination_");
                }
                else
                {
                    WriteResults("Process stuck in loop_");
                }

            }
            catch (Exception ex)
            {

                WriteResults("Fail - Process throw exception: - " + ex.ToString());
            }
            WriteResultsLine();
        }
        static public void RoundRobinTest1(StreamWriter sr)
        {
            try
            {
                Thread t;
                WriteResults("Test RoundRobin (quantum=2) Policy - NUMBER 1 (processes dont access files)_");
                ProcessConsole.Init();
                t = new Thread(() => RoundRobinPolicySimpleWithOutFiles(2));
                t.Start();
                if (WaitForThread(t))
                {
                    ProcessConsole.CloseAll();
                    if (CompareToExpected(4, 4))
                    {

                    }
                    else
                    {
                        WriteResults("Wrong processes output_");
                        WriteResultsLine();
                        return;
                    }
                    if (CompareFiles("TerminateOrder.txt", "TerminateOrder.RoundRobin.1.expected.txt"))
                        WriteResults("Succeeded_");
                    else
                    {
                        WriteResults("Wrong order of process termination_");
                        File.Move("TerminateOrder.txt", "TerminateOrder.RoundRobin.1.actual.txt");
                    }
                }
                else
                {
                    WriteResults("Process stuck in loop_");
                }
            }
            catch (Exception ex)
            {

                WriteResults("Fail - Process throw exception: - " + ex.ToString());
            }
            WriteResultsLine();
        }
        static public void RoundRobinTest2(StreamWriter sr)
        {
            try
            {
                Thread t;
                WriteResults("Test RoundRobin (quantum=7) Policy - NUMBER 2 (processes dont access files)_");
                ProcessConsole.Init();
                t = new Thread(() => RoundRobinPolicySimpleWithOutFiles(7));
                t.Start();
                if (WaitForThread(t))
                {
                    ProcessConsole.CloseAll();
                    if (CompareToExpected(4, 4))
                    {

                    }
                    else
                    {
                        WriteResults("Wrong processes output_");
                        WriteResultsLine();
                        return;
                    }
                    if (CompareFiles("TerminateOrder.txt", "TerminateOrder.RoundRobin.2.expected.txt"))
                        WriteResults("Succeeded_");
                    else
                    {
                        WriteResults("Wrong order of process termination_");
                        File.Move("TerminateOrder.txt", "TerminateOrder.RoundRobin.2.actual.txt");
                    }
                }
                else
                {
                    WriteResults("Process stuck in loop_");
                }
            }
            catch (Exception ex)
            {

                WriteResults("Fail - Process throw exception: - " + ex.ToString());
            }
            WriteResultsLine();
        }
        static public void RoundRobinTest3(StreamWriter sr)
        {
            try
            {
                Thread t;
                WriteResults("Test RoundRobin (quantum=2) Policy - NUMBER 3 (processes access files)_");
                ProcessConsole.Init();
                t = new Thread(() => RoundRobinPolicySimpleWithFiles(2));
                t.Start();
                if (WaitForThread(t))
                {
                    ProcessConsole.CloseAll();
                    if (CompareToExpected(14, 3))
                    {

                    }
                    else
                    {
                        WriteResults("Wrong processes output_");
                        WriteResultsLine();
                        return;
                    }

                    if (CompareFiles("TerminateOrder.txt", "TerminateOrder.RoundRobin.3.expected.txt"))
                        WriteResults("Succeeded_");
                    else
                    {
                        WriteResults("Wrong order of process termination_");
                        File.Move("TerminateOrder.txt", "TerminateOrder.RoundRobin.3.actual.txt");
                    }
                }
                else
                {
                    WriteResults("Process stuck in loop_");
                }
            }
            catch (Exception ex)
            {

                WriteResults("Fail - Process throw exception: - " + ex.ToString());
            }
            WriteResultsLine();
        }
        static public void PriorityTest2(StreamWriter sr)
        {
            try
            {
                Thread t;
                WriteResults("Test RoundRobin (quantum=7) Policy - NUMBER 2 (processes dont access files)_");
                ProcessConsole.Init();
                t = new Thread(() => PriorityPolicySimpleWithOutFiles(7));
                t.Start();
                if (WaitForThread(t))
                {
                    ProcessConsole.CloseAll();
                    if (CompareToExpected(4, 4))
                    {

                    }
                    else
                    {
                        WriteResults("Wrong processes output_");
                        WriteResultsLine();
                        return;
                    }
                    if (CompareFiles("TerminateOrder.txt", "TerminateOrder.RoundRobin.2.expected.txt"))
                        WriteResults("Succeeded_");
                    else
                    {
                        WriteResults("Wrong order of process termination_");
                        File.Move("TerminateOrder.txt", "TerminateOrder.RoundRobin.2.actual.txt");
                    }
                }
                else
                {
                    WriteResults("Process stuck in loop_");
                }
            }
            catch (Exception ex)
            {

                WriteResults("Fail - Process throw exception: - " + ex.ToString());
            }
            WriteResultsLine();
        }
        static public void PriorityTest3(StreamWriter sr)
        {
            try
            {
                Thread t;
                WriteResults("Test RoundRobin (quantum=2) Policy - NUMBER 3 (processes access files)_");
                ProcessConsole.Init();
                t = new Thread(() => PriorityPolicySimpleWithFiles(2));
                t.Start();
                if (WaitForThread(t))
                {
                    ProcessConsole.CloseAll();
                    if (CompareToExpected(14, 3))
                    {

                    }
                    else
                    {
                        WriteResults("Wrong processes output_");
                        WriteResultsLine();
                        return;
                    }

                    if (CompareFiles("TerminateOrder.txt", "TerminateOrder.RoundRobin.3.expected.txt"))
                        WriteResults("Succeeded_");
                    else
                    {
                        WriteResults("Wrong order of process termination_");
                        File.Move("TerminateOrder.txt", "TerminateOrder.RoundRobin.3.actual.txt");
                    }
                }
                else
                {
                    WriteResults("Process stuck in loop_");
                }
            }
            catch (Exception ex)
            {

                WriteResults("Fail - Process throw exception: - " + ex.ToString());
            }
            WriteResultsLine();
        }
        static public void RoundRobinTest4(StreamWriter sr)
        {
            try
            {
                Thread t;
                WriteResults("Test RoundRobin (quantum=7) Policy - NUMBER 4 (processes access files)_");
                ProcessConsole.Init();
                t = new Thread(() => RoundRobinPolicySimpleWithFiles(7));
                t.Start();
                if (WaitForThread(t))
                {
                    ProcessConsole.CloseAll();
                    if (CompareToExpected(14, 3))
                    {

                    }
                    else
                    {
                        WriteResults("Wrong processes output_");
                        WriteResultsLine();
                        return;
                    }

                    if (CompareFiles("TerminateOrder.txt", "TerminateOrder.RoundRobin.4.expected.txt"))
                        WriteResults("Succeeded_");
                    else
                    {
                        WriteResults("Wrong order of process termination_");
                        File.Move("TerminateOrder.txt", "TerminateOrder.RoundRobin.4.actual.txt");
                    }
                }
                else
                {
                    WriteResults("Process stuck in loop_");
                }
            }
            catch (Exception ex)
            {

                WriteResults("Fail - Process exception: - " + ex.ToString());
            }
            WriteResultsLine();
        }
        static public void RoundRobinATAndMaxStarvationTest(StreamWriter sr)
        {
            double max = 0;
            double avg = 0;
            try
            {
                Thread t;
                WriteResults("Test AverageTurnaround and MaxStarvation in RoundRobin Policy_");
                ProcessConsole.Init();
                t = new Thread(() => RoundRobinPolicySimpleWithFilesAM(1, out avg, out max));
                t.Start();
                if (WaitForThread(t))
                {
                    ProcessConsole.CloseAll();
                    if (Math.Round(avg - 34788) > 1000)
                    {
                        WriteResults("AV Failed_");
                        WriteResultsLine();
                        return;
                    }


                    if (Math.Round(max - 92) > 10)
                        WriteResults("MaxStarvation Failed_");
                    else
                        WriteResults("Succeeded_");
                }
                else
                {
                    WriteResults("Process stuck in loop_");
                }
            }
            catch (Exception ex)
            {

                WriteResults("Fail - Process exception: - " + ex.ToString());
            }
            WriteResultsLine();
        }
        static public void FirstComeFirstServedATAndMaxStarvationTest(StreamWriter sr)
        {
            double max = 0;
            double avg = 0;
            try
            {
                WriteResults("Test AT And MaxStarvation in First Come First Served Policy_");
                ProcessConsole.Init();
                Thread t;
                t = new Thread(() => FirstComeFirstServedPolicySimpleWithFilesAM(out avg, out max));
                t.Start();
                if (WaitForThread(t))
                {
                    ProcessConsole.CloseAll();
                    if (Math.Round(avg - 27052) > 1000)
                    {
                        WriteResults("AV Failed_");
                        WriteResultsLine();
                        return;
                    }


                    if (Math.Round(max - 49457) > 1000)
                        WriteResults("MaxStarvation Failed_");
                    else
                        WriteResults("Succeeded_");
                }
                else
                {
                    WriteResults("Process stuck in loop_");
                }
            }
            catch (Exception ex)
            {
                WriteResults("Fail - Process exception: - " + ex.ToString());
            }
            WriteResultsLine();
        }
       
        static void Main2(string[] args)
        {
            Disk disk = new Disk();
            CPU cpu = new CPU(disk);
            cpu.Debug = false;
            OperatingSystem os = new OperatingSystem(cpu, disk, new FirstComeFirstServedPolicy());
            Example1(os);
            //Example2(os);
            //Example3(os);
            os.ActivateScheduler();
            cpu.Execute();
            Thread.Sleep(1000);
            Console.WriteLine("Average turnaround " + os.AverageTurnaround());
            Console.WriteLine("Maximal starvation " + os.MaximalStarvation());
        }
    }
}
