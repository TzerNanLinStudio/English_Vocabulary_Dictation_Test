//#define WinForm
#define WPF

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

#if WinForm
using RichTextBox = System.Windows.Forms.RichTextBox;
#elif WPF
using System.Windows.Threading;
using System.Windows.Documents;
using RichTextBox = System.Windows.Controls.RichTextBox;
using Brushes = System.Windows.Media.Brushes;
#endif
using Color = System.Drawing.Color;

namespace LogRecorder_sharp
{
    public delegate void MsgDelegate(RichTextBox o_rtB, Color color, String Msg);

    public class InfoMgr
    {
        ////////////////////////////////////////////
        //	Properties
        //------------------------------------------
        private const int MaxLogLine = 1000;

        private LogRcdr GenLog;
        private LogRcdr WarningLog;
        private LogRcdr ErrLog;
        private LogRcdr DebugLog;

        private RichTextBox rtB_GenLog;
        private RichTextBox rtB_WarningLog;
        private RichTextBox rtB_ErrLog;
        private RichTextBox rtB_DebugLog;

        ////////////////////////////////////////////
        // Other
        //------------------------------------------
        private readonly object MsgToRTBLocker = new object();


        ////////////////////////////////////////////
        //	Method
        //------------------------------------------
        public InfoMgr(string DirGenLog, string DirWarningLog, string DirErrLog, string DirDebugLog)
        {
            this.rtB_GenLog = null;
            this.rtB_WarningLog = null;
            this.rtB_ErrLog = null;
            this.rtB_DebugLog = null;

            DirectoryInfo TmpDir_GenLog = new DirectoryInfo(DirGenLog);
            DirectoryInfo TmpDir_WarningLog = new DirectoryInfo(DirWarningLog);
            DirectoryInfo TmpDir_ErrLog = new DirectoryInfo(DirErrLog);
            DirectoryInfo TmpDir_DebugLog = new DirectoryInfo(DirDebugLog);

            try
            {
                if (!TmpDir_GenLog.Exists)
                {
                    TmpDir_GenLog.Create();
                    System.Threading.Thread.Sleep(20);      //	Wait OS Create Directory
                }
                this.GenLog = new LogRcdr();
                this.GenLog.Open(DirGenLog, "GeneralLog");

                if (!TmpDir_WarningLog.Exists)
                {
                    TmpDir_WarningLog.Create();
                    System.Threading.Thread.Sleep(20);      //	Wait OS Create Directory
                }
                this.WarningLog = new LogRcdr();
                this.WarningLog.Open(DirWarningLog, "WarningLog");

                if (!TmpDir_ErrLog.Exists)
                {
                    TmpDir_ErrLog.Create();
                    System.Threading.Thread.Sleep(20);      //	Wait OS Create Directory
                }
                this.ErrLog = new LogRcdr();
                this.ErrLog.Open(DirErrLog, "ErrorLog");

                if (!TmpDir_DebugLog.Exists)
                {
                    TmpDir_DebugLog.Create();
                    System.Threading.Thread.Sleep(20);      //	Wait OS Create Directory
                }
                this.DebugLog = new LogRcdr();
                this.DebugLog.Open(DirDebugLog, "DebugLog");

            }
            catch (Exception e)
            {
                throw e;
            }


        }

        ~InfoMgr()
        {
            //this.GenLog.Close();
            //this.WarningLog.Close();
            //this.ErrLog.Close();
            //this.DebugLog.Close();
        }

        public void CloseAll()
        {
            this.GenLog.Close();
            this.WarningLog.Close();
            this.ErrLog.Close();
            this.DebugLog.Close();
        }

        public void SetGenLogRTB(RichTextBox Obj_rtB)
        {
            if (Obj_rtB == null)
            {
                throw new System.Exception("RichTextBox is null.");
            }

            this.rtB_GenLog = Obj_rtB;
        }

        public void SetWarningLogRTB(RichTextBox Obj_rtB)
        {
            if (Obj_rtB == null)
            {
                throw new System.Exception("RichTextBox is null.");
            }

            this.rtB_WarningLog = Obj_rtB;
        }

        public void SetErrLogRTB(RichTextBox Obj_rtB)
        {
            if (Obj_rtB == null)
            {
                throw new System.Exception("RichTextBox is null.");
            }

            this.rtB_ErrLog = Obj_rtB;
        }

        public void SetDebugLogRTB(RichTextBox Obj_rtB)
        {
            if (Obj_rtB == null)
            {
                throw new System.Exception("RichTextBox is null.");
            }

            this.rtB_DebugLog = Obj_rtB;
#if WinForm
            this.rtB_DebugLog.SelectionColor = Color.Magenta;
#elif WPF
            this.rtB_DebugLog.SelectionBrush = Brushes.Magenta;
#endif
        }

        public void MsgGenLog(String Msg)
        {
            this.GenLog.Write(Msg);

            if (this.rtB_GenLog != null)
            {
                this.MsgToRTB(this.rtB_GenLog, Color.Black, Msg);
            }
        }

        public void MsgHLLog(String Msg)
        {
            this.GenLog.Write(Msg);

            if (this.rtB_GenLog != null)
            {
                this.MsgToRTB(this.rtB_GenLog, Color.DarkBlue, Msg);
            }
        }

        public void MsgRmtCtrlLog(String Msg)
        {
            this.GenLog.Write(Msg);

            if (this.rtB_GenLog != null)
            {
                this.MsgToRTB(this.rtB_GenLog, Color.DarkGreen, Msg);
            }
        }

        public void MsgUILog(String Msg)
        {
            this.GenLog.Write(Msg);

            if (this.rtB_GenLog != null)
            {
                this.MsgToRTB(this.rtB_GenLog, Color.DarkCyan, Msg);
            }
        }

        public void MsgWarning(String Msg)
        {
            this.GenLog.Write(Msg);
            this.WarningLog.Write(Msg);

            if (this.rtB_GenLog != null)
            {
                this.MsgToRTB(this.rtB_GenLog, Color.Orange, Msg);
            }

            if (this.rtB_WarningLog != null)
            {
                this.MsgToRTB(this.rtB_WarningLog, Color.Orange, Msg);
            }
        }
        public void MsgError(String Msg)
        {
            this.GenLog.Write(Msg);
            this.ErrLog.Write(Msg);

            if (this.rtB_GenLog != null)
            {
                this.MsgToRTB(this.rtB_GenLog, Color.Red, Msg);
            }

            if (this.rtB_ErrLog != null)
            {
                this.MsgToRTB(this.rtB_ErrLog, Color.Red, Msg);
            }
        }

        public void MsgDebug(String Msg)
        {
            this.DebugLog.Write(Msg);

            if (this.rtB_DebugLog != null)
            {
                this.MsgToRTB(this.rtB_DebugLog, Color.Magenta, Msg);
            }
        }

        private void MsgToRTB(RichTextBox o_rtB, Color color, String msg)
        {
            lock (MsgToRTBLocker)
            {
                MsgToRTB(o_rtB, color, msg, 0);
            }
        }

        private void MsgToRTB(RichTextBox o_rtB, Color color, String msg, int redundant)
        {
#if WinForm
            if (o_rtB.InvokeRequired)
            {                
                MsgDelegate action = new MsgDelegate(this.MsgToRTB);

                o_rtB.BeginInvoke(action, o_rtB, color, msg);
            }
            else
            {
                if (o_rtB.Lines.Length > 0)
                    o_rtB.AppendText("\n");

                string AppendText;
                DateTime AppendTextDate = DateTime.Now;

                AppendText = string.Format("{0:yyyy}/{0:MM}/{0:dd}, {0:HH}:{0:mm}:{0:ss}:{0:ffff} > {1}",
                                                AppendTextDate,
                                                msg);

                o_rtB.SelectionColor = color;
                o_rtB.AppendText(AppendText);

                if (o_rtB.Lines.Length > MaxLogLine)
                {
                    string rtfString;

                    o_rtB.Select(o_rtB.GetFirstCharIndexFromLine(1), o_rtB.TextLength);
                    rtfString = o_rtB.SelectedRtf;
                    o_rtB.Rtf = rtfString;
                }

                o_rtB.SelectionStart = o_rtB.TextLength;
                o_rtB.ScrollToCaret();
                    
#elif WPF  
            string AppendText = string.Format("{0:yyyy}/{0:MM}/{0:dd}, {0:HH}:{0:mm}:{0:ss}:{0:ffff} > {1}",
                                            DateTime.Now,
                                            msg);

            o_rtB.Document.Blocks.Add(new Paragraph(new Run(AppendText)
            {
                Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B))
            }));

            if (o_rtB.Document.LineHeight > MaxLogLine)
            {
                //string rtfString;

                //o_rtB.Select(o_rtB.GetFirstCharIndexFromLine(1), o_rtB.TextLength);
                //rtfString = o_rtB.SelectedRtf;
                //o_rtB.Rtf = rtfString;
            }

            o_rtB.ScrollToEnd();

        }
#endif
    }

}

