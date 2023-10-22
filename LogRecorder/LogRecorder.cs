using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;

namespace LogRecorder_sharp
{
    class LogRcdr
    {
        ////////////////////////////////////////////
        //	Properties
        //------------------------------------------
        //	LogPath
        //	LogFilename
        //	Date		
        private bool m_LogFileOpened;
        private string m_LogPath;
        private string m_LogFilename;
        private DateTime m_LogDate;
        private StreamWriter m_LogFile;

        ////////////////////////////////////////////
        // Other
        //------------------------------------------
        private readonly object m_WriteLocker = new object();

        ////////////////////////////////////////////
        //	Method
        //------------------------------------------
        //	Open
        //	Close
        //	Write

        public LogRcdr()
        {
            m_LogPath = "";
            m_LogFilename = "";
            m_LogDate = DateTime.Now;
            m_LogFileOpened = false;
        }

        ~LogRcdr()
        {                      
            m_LogFile?.Close();
            m_LogFile?.Dispose();
        }

        public void Open(String Path, String Filename)
        {
            m_LogDate = DateTime.Now;
            m_LogFilename = Filename;
            m_LogPath = Path;
            m_LogFileOpened = false;

            try
            {
                string StringDate = String.Format("{0:yyyyMMdd}", m_LogDate);
                string path = m_LogPath + "\\" + m_LogFilename + "_" + StringDate + ".Log";

                m_LogFile = File.AppendText(path);
                m_LogFile.AutoFlush = true;
                m_LogFile.WriteLine("=============== LOG START - {0:yyyy}.{0:MM}.{0:dd} {0:HH}:{0:mm}:{0:ss}:{0:ffff} ===============", m_LogDate);
                m_LogFileOpened = true;
            }
            catch (Exception e)
            {
                //MessageBox.Show("LogRcdr.Open() - " + e.ToString());
                Console.WriteLine("LogRcdr.Open() - " + e.ToString());
            }
        }

        public void Close()
        {
            DateTime LogCloseDate = DateTime.Now;

            try
            {
                if (m_LogFileOpened == true)
                {
                    m_LogFile.WriteLine("===============   LOG END - {0:yyyy}.{0:MM}.{0:dd} {0:HH}:{0:mm}:{0:ss}:{0:ffff} ===============", LogCloseDate);
                    m_LogFileOpened = false;
                    m_LogFile.Close();
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show("LogRcdr.Close() - "+ e.ToString());
                Console.WriteLine("LogRcdr.Close() - " + e.ToString());
            }
        }

        public void Write(string Msg)
        {
            lock (m_WriteLocker)
            {
                if (m_LogFileOpened == true)
                    Write(Msg, 0);
            }
        }

        public void Write(string Msg, int redundant)
        {
            DateTime MsgWriteDate = DateTime.Now;
            TimeSpan s = new TimeSpan(MsgWriteDate.Ticks - m_LogDate.Ticks);

            try
            {
                if (s.Days != 0)
                {
                    m_LogFile.WriteLine("Data write Date is not same as File Date. Close file and Open a new file.");
                    Close();
                    Open(m_LogPath, m_LogFilename);
                }

                m_LogFile.WriteLine(
                        "{0:yyyy}/{0:MM}/{0:dd}, {0:HH}:{0:mm}:{0:ss}:{0:ffff} - {1}",
                        MsgWriteDate,
                        Msg
                    );

            }
            catch (Exception e)
            {
                MessageBox.Show(String.Format("{0}{1}\n{2}\\{3}",
                                "LogRcdr.Write() - ",
                                e.Message,
                                m_LogPath,
                                m_LogFilename));
            }
        }
    }
}
