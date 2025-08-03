using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace Scheduling
{
    class ProcessConsole
    {
        public static StreamWriter terminateOrder = null;
        public bool isOpen = true;
        public static Mutex mutex = null;
        private StreamWriter m_swLog;
        private Thread m_tConsole;
        private OutputConsole m_ocOutput;
        private int m_iProcessId;
        private string m_sProcessName;
        public static int WindowsCount = 0;
        public static bool UseGUIConsoles = false;
        public static List<ProcessConsole> AllConsoles = new List<ProcessConsole>();
        public static void Init()
        {
            terminateOrder = new StreamWriter("TerminateOrder.txt", false);
            mutex = new Mutex();
        }

        public static void CloseAll()
        {
            foreach (ProcessConsole pc in AllConsoles)
                pc.Close();
            terminateOrder.Close();
        }

        public ProcessConsole(int iProcessId, string sProcessName)
        {
            AllConsoles.Add(this);
            m_sProcessName = sProcessName;
            m_iProcessId = iProcessId;
            if (UseGUIConsoles)
            {
                m_tConsole = new Thread(OpenConsole);
                m_tConsole.Start();
                while (m_ocOutput == null)
                    Thread.Sleep(10);
                Thread.Sleep(100);
            }
            m_swLog = new StreamWriter("ProcessLog." + m_iProcessId + ".txt");
        }
        public void OpenConsole()
        {
            try
            {
                WindowsCount++;
                m_ocOutput = new OutputConsole();
                m_ocOutput.Text = m_sProcessName + "(pid " + m_iProcessId + ")";
                m_ocOutput.ShowDialog();
            }
            catch (Exception e)
            {
                m_ocOutput.Close();
                m_ocOutput = null;
            }
        }
        public void Close()
        {

            if (UseGUIConsoles)
            {
                m_tConsole.Abort();
                m_tConsole = null;
            }
            if (m_swLog != null)
                m_swLog.Close();

            if (isOpen)
            {
                mutex.WaitOne();
                terminateOrder.WriteLine(m_iProcessId);
                terminateOrder.Flush();
                mutex.ReleaseMutex();
                isOpen = false;
            }
        }
        public void Write(string s)
        {
            if (UseGUIConsoles)
            {
                m_ocOutput.Invoke(new MethodInvoker(m_ocOutput.BringToFront));
                //m_ocOutput.Write(s);
                m_ocOutput.Invoke(new OutputConsole.WriteDelegate(m_ocOutput.Write), new object[] { s });
                Thread.Sleep(100);
            }
            m_swLog.Write(s.Replace("\\n", "\n"));
        }
    }
}
