using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Resources;
using System.Security.Cryptography;
using System.Speech.Synthesis;
using Microsoft.Win32;
using VocaburaryCore;
using Config_sharp;
using LogRecorder_sharp;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace UI
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        #region "Parameter"
        // Variables For Commend Window.
        // --------------------------------------------------
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        IntPtr Hwnd = GetConsoleWindow();
        // --------------------------------------------------

        // Variables For UI.
        // --------------------------------------------------
        private string PersonalListFolderPath = System.Environment.CurrentDirectory + @"\Appendix\Vocaburary\PersonalList";
        private string PassGradeFolderPath = System.Environment.CurrentDirectory + @"\Appendix\Vocaburary\PassGrade";
        public bool[] PrintFlag = new bool[10] { false, false, false, false, false, false, false, false, false, false };
        public bool IsInitializationComplete = false;
        public bool TestModeFlag = false;
        public int EditAreaDate = -1;
        public int EditAreaPageNumber = 1;
        public int TestAreaDate = -1;
        public int TestAreaResultPageNumber = 1;
        public int TestAreaRecordPageNumber = 1;
        private System.Windows.Controls.RichTextBox DefinitionBoard;
        // --------------------------------------------------

        // Variables For Vocaburary.
        // --------------------------------------------------
        public VocaburaryManagement MainVocabularyManagement;
        public Thread VocaburaryTestThread;
        // --------------------------------------------------

        // Variables For Config.
        // ---------------------------------------------------------------
        public CustomConfig_UI UI_Config;
        private string m_RecipeDirectoryPath = System.Environment.CurrentDirectory + @"\Appendix\Config\";
        private string m_RecipeFilename = "UI";
        private string m_RecipeSubtitle = ".dat";
        private string m_RecipeFullPath = "";
        // ---------------------------------------------------------------

        // Variables For LogRecorder. 
        // ---------------------------------------------------------------
        public InfoMgr UI_LogWritter;
        private bool IsLogInitSuccess = false;
        private string m_LogFileRecipeDirectionPath = System.Environment.CurrentDirectory + @"\Appendix\Log\";
        private string m_LogFileNameHeader = "UI";
        // ---------------------------------------------------------------

        // Variables for Debug.
        // --------------------------------------------------
        public Stopwatch[] DebugWatch = new Stopwatch[5] { new Stopwatch(), new Stopwatch(), new Stopwatch(), new Stopwatch(), new Stopwatch() };
        public Image<Bgr, byte>[] DebugBgrImage = new Image<Bgr, byte>[5] { null, null, null, null, null };
        public static Thread DebugThread;
        public bool[] DebugFlag = new bool[10] { false, false, false, false, false, false, false, false, false, false };
        public int[] DebugIntValue = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public double[] DebugDoubleValue = new double[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        // --------------------------------------------------
        #endregion

        #region "Constructor"
        public MainWindow()
        {
            InitializeComponent();
            InitializeUI();
        }
        #endregion

        #region "Event"
        private void VocaburaryManagement_MessageEvent(string mode, int number, string message)
        {
            switch (mode)
            {
                case "Result":
                    {
                        TestAreaResultPageNumber = 1;
                        ShowTestResult();
                        TestMode(false);
                    }
                    break;

                case "Record":
                    {
                        if (number != 0)
                        {
                            if (number % 5 == 0)
                                TestAreaRecordPageNumber = number / 5;
                            else
                                TestAreaRecordPageNumber = number / 5 + 1;

                            ShowPastGrade();
                        }
                    }
                    break;

                case "Definition":
                    {
                        Dispatcher.Invoke(() => DefinitionBoard.Document.Blocks.Add(new Paragraph(new Run("定義[" + number.ToString() + "]:"))));
                        Dispatcher.Invoke(() => DefinitionBoard.Document.Blocks.Add(new Paragraph(new Run(message))));
                    }
                    break;

                case "Information":
                    {
                        if (Dispatcher.Invoke(() => CheckBox_Paper.IsChecked == true))
                        {
                            for (int i = (TestAreaResultPageNumber - 1) * 10; i < TestAreaResultPageNumber * 10; i++)
                            {
                                int index = i % 10;

                                if (index < 5)
                                {
                                    if (i >= number) //???  VocabularyCore.ResultList.Count?
                                    {
                                        Dispatcher.Invoke(() => ((StackPanel_LeftResultPage.Children[1] as StackPanel).Children[index] as SelfController02).Button_One.Content = "");
                                        Dispatcher.Invoke(() => ((StackPanel_LeftResultPage.Children[1] as StackPanel).Children[index] as SelfController02).Label_Two.Content = "");
                                    }
                                    else
                                    {
                                        Dispatcher.Invoke(() => ((StackPanel_LeftResultPage.Children[1] as StackPanel).Children[index] as SelfController02).Button_One.Content = "???");
                                        Dispatcher.Invoke(() => ((StackPanel_LeftResultPage.Children[1] as StackPanel).Children[index] as SelfController02).Label_Two.Content = "On Paper");
                                    }
                                }
                                else
                                {
                                    if (i >= MainVocabularyManagement.StringList.Count)
                                    {
                                        Dispatcher.Invoke(() => ((StackPanel_RightResultPage.Children[1] as StackPanel).Children[index - 5] as SelfController02).Button_One.Content = "");
                                        Dispatcher.Invoke(() => ((StackPanel_RightResultPage.Children[1] as StackPanel).Children[index - 5] as SelfController02).Label_Two.Content = "");
                                    }
                                    else
                                    {
                                        Dispatcher.Invoke(() => ((StackPanel_RightResultPage.Children[1] as StackPanel).Children[index - 5] as SelfController02).Button_One.Content = "???");
                                        Dispatcher.Invoke(() => ((StackPanel_RightResultPage.Children[1] as StackPanel).Children[index - 5] as SelfController02).Label_Two.Content = "On Paper");
                                    }
                                }
                            }
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        private void SelfController_WordEvent(string mode, string word)
        {
            switch (mode)
            {
                case "Pronounce":
                    {
                        MainVocabularyManagement.PronounceWord(word);
                    }
                    break;

                case "Delete":
                    {
                        if (MainVocabularyManagement.DeleteWord(EditAreaDate, word))
                        {
                            Dispatcher.Invoke(() => RichTextBox_EditArea_Definition.Document.Blocks.Clear());
                            Dispatcher.Invoke(() => RichTextBox_EditArea_Definition.AppendText("單字[" + word + "]刪除成功!"));

                            for (int i = 0; i < MainVocabularyManagement.VocabularySet.Days.Length; i++)
                            {
                                if (EditAreaDate == MainVocabularyManagement.VocabularySet.Days[i].Date)
                                {
                                    for (int j = (EditAreaPageNumber - 1) * 10; j < EditAreaPageNumber * 10; j++)
                                    {
                                        int index = j % 10;

                                        if (index < 5)
                                            ((StackPanel_LeftVocabularyPage.Children[1] as StackPanel).Children[index] as ThreeLabel).ShowVocabularyInformation(MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].English, MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].PartOfSpeech, MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].Chinese);
                                        else
                                            ((StackPanel_RightVocabularyPage.Children[1] as StackPanel).Children[index - 5] as ThreeLabel).ShowVocabularyInformation(MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].English, MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].PartOfSpeech, MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].Chinese);
                                    }

                                    break;
                                }
                            }
                        }
                        else
                        {
                            Dispatcher.Invoke(() => RichTextBox_EditArea_Definition.Document.Blocks.Clear());
                            Dispatcher.Invoke(() => RichTextBox_EditArea_Definition.AppendText("單字[" + word + "]刪除失敗!"));
                        }
                    }
                    break;

                case "Definition":
                    {
                        DefinitionBoard = RichTextBox_EditArea_Definition;
                        Dispatcher.Invoke(() => DefinitionBoard.Document.Blocks.Clear());
                        MainVocabularyManagement.SearchWord(word);
                    }
                    break;

                case "Definition_From_SelfController02":
                    {
                        DefinitionBoard = RichTextBox_TestArea_Definition;
                        Dispatcher.Invoke(() => DefinitionBoard.Document.Blocks.Clear());

                        for (int i = 0; i < MainVocabularyManagement.VocabularySet.Days.Length; i++)
                        {
                            if (TestAreaDate == MainVocabularyManagement.VocabularySet.Days[i].Date)
                            {
                                for (int j = 0; j < MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries.Length; j++)
                                {
                                    if (word == MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].English)
                                    {
                                        Dispatcher.Invoke(() => DefinitionBoard.Document.Blocks.Add(new Paragraph(new Run("英文:" + MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].English))));
                                        Dispatcher.Invoke(() => DefinitionBoard.Document.Blocks.Add(new Paragraph(new Run("詞性:" + MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].PartOfSpeech))));
                                        Dispatcher.Invoke(() => DefinitionBoard.Document.Blocks.Add(new Paragraph(new Run("中文:" + MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].Chinese))));

                                        break;
                                    }
                                }

                                break;
                            }
                        }

                        MainVocabularyManagement.SearchWord(word);
                    }
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region "Function"
        private void InitializeUI()
        {
            try
            {
                if (true)
                {
                    IsInitializationComplete = false;
                    ShowWindow(Hwnd, SW_SHOW);
                    Console.WriteLine("====================================================================================================");
                    Console.WriteLine("====================================================================================================");
                    Console.WriteLine("====================================================================================================");
                }

                // Config Initialization Or Setting
                // --------------------------------------------------
                InitializeConfig(true, m_RecipeDirectoryPath + m_RecipeFilename + m_RecipeSubtitle);
                // --------------------------------------------------

                // Log Initialization Or Setting
                // --------------------------------------------------
                InitializeLog(m_LogFileRecipeDirectionPath + m_LogFileNameHeader);
                // --------------------------------------------------

                // Vocaburary Initialization Or Setting
                // --------------------------------------------------
                MainVocabularyManagement = new VocaburaryManagement();
                MainVocabularyManagement.MessageReturnEvent += VocaburaryManagement_MessageEvent;
                // --------------------------------------------------

                // UI Initialization Or Setting
                // --------------------------------------------------
                GetDateFromFolder();

                TestMode(false);

                TabControl_ChooseFunction.SelectedIndex = 0;

                MenuItem_Debug.Visibility = System.Windows.Visibility.Hidden;
                TabItem_Debug.Visibility = System.Windows.Visibility.Hidden;

                Dispatcher.Invoke(() => RichTextBox_TestArea_Definition.AppendText("定義:")); 
                Dispatcher.Invoke(() => RichTextBox_EditArea_Definition.AppendText("定義:")); 

                for (int i = 0; i < 5; i++)
                {
                    ((StackPanel_LeftVocabularyPage.Children[1] as StackPanel).Children[i] as ThreeLabel).ThreeLabelHandleEvent += SelfController_WordEvent;
                    ((StackPanel_RightVocabularyPage.Children[1] as StackPanel).Children[i] as ThreeLabel).ThreeLabelHandleEvent += SelfController_WordEvent;
                    ((StackPanel_LeftResultPage.Children[1] as StackPanel).Children[i] as SelfController02).HandleEvent += SelfController_WordEvent;
                    ((StackPanel_RightResultPage.Children[1] as StackPanel).Children[i] as SelfController02).HandleEvent += SelfController_WordEvent;
                }
                // --------------------------------------------------

                // Debug Code Initialization Or Setting
                // --------------------------------------------------

                // --------------------------------------------------

                if (true)
                {
                    IsInitializationComplete = true;
                    //ShowWindow(Hwnd, SW_HIDE);
                    Console.WriteLine("====================================================================================================");
                    Console.WriteLine("====================================================================================================");
                    Console.WriteLine("====================================================================================================" + "\n");
                }
            }
            catch (Exception EX)
            {
                Console.WriteLine("Error Occured When Software Initialized. Message: " + EX.Message);
            }
        }

        private void Restart()
        {
            Close();

            System.Threading.Thread thtmp = new System.Threading.Thread(new
            System.Threading.ParameterizedThreadStart(ReRun));

            object appName = System.Windows.Forms.Application.ExecutablePath;
            System.Threading.Thread.Sleep(5000);
            thtmp.Start(appName);
        }

        private void ReRun(System.Object obj)
        {
            System.Diagnostics.Process ps = new System.Diagnostics.Process();
            ps.StartInfo.FileName = obj.ToString();
            ps.Start();
        }

        public void InitializeConfig(bool r_WhetherLoad, string r_Path)
        {
            m_RecipeFullPath = r_Path;

            UI_Config = new CustomConfig_UI(m_RecipeFullPath);

            if (r_WhetherLoad)
                if (UI_Config.Load() == false) return;
        }

        public void InitializeLog(string r_Path)
        {
            try
            {
                string r_GenLog, r_WarningLog, r_ErrLog, r_DebugLog;

                r_GenLog = r_Path + "./GeneralLog";
                r_WarningLog = r_Path + "./WarningLog";
                r_ErrLog = r_Path + "./ErrorLog";
                r_DebugLog = r_Path + "./DebugLog";

                UI_LogWritter = new InfoMgr(r_GenLog, r_WarningLog, r_ErrLog, r_DebugLog);

                if (UI_LogWritter != null) IsLogInitSuccess = true;
            }
            catch (Exception ex)
            {
                ChangeRichTextBox("Error", "Initialized Error In Log. Message: " + ex.Message);
            }
        }

        private string ChangeTimeFormat(int r_Mode, System.DateTime r_Datetime)
        {
            string date;
            string time;
            string str;

            switch (r_Mode)
            {
                case 1:
                    date = r_Datetime.ToShortDateString();
                    time = r_Datetime.ToString("hh:mm:ss.FFF", System.Globalization.CultureInfo.InvariantCulture).PadRight(12, '0');
                    str = " [ " + date + " " + time + " ] : ";
                    return str;

                case 2:
                    str = r_Datetime.Year.ToString() + "." + r_Datetime.Month.ToString() + "." + r_Datetime.Day.ToString() + "," + r_Datetime.Hour.ToString() + "." + r_Datetime.Minute.ToString() + "." + r_Datetime.Second.ToString();
                    return str;

                case 3:
                    date = r_Datetime.ToShortDateString();
                    time = r_Datetime.ToString("hh:mm:ss.FFF", System.Globalization.CultureInfo.InvariantCulture).PadRight(12, '0');
                    str = date + " " + time;
                    return str;

                case 4:
                    str = r_Datetime.Year.ToString("D4") + r_Datetime.Month.ToString("D2") + r_Datetime.Day.ToString("D2");
                    return str;

                default:
                    date = r_Datetime.ToShortDateString();
                    time = r_Datetime.ToString("hh:mm:ss.FFF", System.Globalization.CultureInfo.InvariantCulture).PadRight(12, '0');
                    str = " [ " + date + " " + time + " ] : ";
                    return str;
            }
        }

        private void ChangeRichTextBox(string r_Mode, string r_Word)
        {
            string m_DateTime = ChangeTimeFormat(1, DateTime.Now);

            switch (r_Mode)
            {
                case "System":
                    Dispatcher.Invoke(() => RichTextBox_SystemLog.Document.Blocks.Add(new Paragraph(new Run(m_DateTime + "<System> " + r_Word))));
                    Dispatcher.Invoke(() => RichTextBox_SystemLog.ScrollToEnd());
                    break;

                case "Exception":
                    Dispatcher.Invoke(() => RichTextBox_SystemLog.Document.Blocks.Add(new Paragraph(new Run(m_DateTime + "<Exception> " + r_Word))));
                    Dispatcher.Invoke(() => RichTextBox_SystemLog.ScrollToEnd());
                    break;

                default:
                    Dispatcher.Invoke(() => RichTextBox_SystemLog.Document.Blocks.Add(new Paragraph(new Run(m_DateTime + "<Default> " + r_Word))));
                    Dispatcher.Invoke(() => RichTextBox_SystemLog.ScrollToEnd());
                    break;
            }
        }

        private void GetDateFromFolder()
        {
            try
            {
                //判斷資料夾是否還存在
                if (Directory.Exists(PersonalListFolderPath))
                {
                    foreach (string f in Directory.GetFileSystemEntries(PersonalListFolderPath))
                    {
                        if (File.Exists(f))
                        {
                            string a = System.IO.Path.GetFileNameWithoutExtension(f);
                            string b = a.Insert(4, "/").Insert(7, "/");

                            string path = f;
                            int date = Convert.ToInt32(System.IO.Path.GetFileNameWithoutExtension(path));
                            if (System.IO.Path.GetExtension(path).CompareTo(".txt") == 0)
                            {
                                if (MainVocabularyManagement.WhetherDateCorrect(date))
                                {
                                    ComboBox_EditArea_Date.Items.Add(b);
                                    ComboBox_TestArea_Date.Items.Add(b);

                                    MainVocabularyManagement.InputDocument(date, path, PassGradeFolderPath + @"\" + a + ".txt");

                                    StreamReader notebook = new StreamReader(path, System.Text.Encoding.Default/*Encoding.GetEncoding("big5")*/);
                                    string line = notebook.ReadLine();
                                    while (line != null)
                                    {
                                        string[] words = line.Split('@');
                                        if (words.Length == 3)
                                            MainVocabularyManagement.InputWord(date, words[0], words[1], words[2], false);
                                        line = notebook.ReadLine();
                                    }

                                    notebook.Close();
                                }
                                else
                                {
                                    Console.WriteLine("This Date Is Not A Suitable Date.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("This File Is Not A Text File.");
                            }
                        }
                    }

                    ComboBox_EditArea_Date.Items.Add("New");
                }
                else
                {
                    Console.WriteLine("The folder path is not correct.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("The Message Of The Exception: " + ex.Message);
                throw ex;
            }
        }

        private void ShowPastGrade()
        {
            List<List<string>> grades = MainVocabularyManagement.FindGrade(TestAreaDate, TestAreaRecordPageNumber);

            for (int i = 0; i < 5; i++)
            {
                Dispatcher.Invoke(() => ((StackPanel_RecordPage.Children[1] as StackPanel).Children[i] as Controller03).Label_One.Content = grades[i][0]);
                Dispatcher.Invoke(() => ((StackPanel_RecordPage.Children[1] as StackPanel).Children[i] as Controller03).Label_Two.Content = grades[i][1]);
            }
        }

        private void ShowTestResult()
        {
            for (int i = (TestAreaResultPageNumber - 1) * 10; i < TestAreaResultPageNumber * 10; i++)
            {
                int index = i % 10;

                if (index < 5)
                {
                    if (i >= MainVocabularyManagement.ResultList.Count)
                    {
                        Dispatcher.Invoke(() => ((StackPanel_LeftResultPage.Children[1] as StackPanel).Children[index] as SelfController02).Button_One.Content = "");
                        Dispatcher.Invoke(() => ((StackPanel_LeftResultPage.Children[1] as StackPanel).Children[index] as SelfController02).Label_Two.Content = "");
                    }
                    else
                    {
                        Dispatcher.Invoke(() => ((StackPanel_LeftResultPage.Children[1] as StackPanel).Children[index] as SelfController02).Button_One.Content = MainVocabularyManagement.ResultList[i][0]);
                        Dispatcher.Invoke(() => ((StackPanel_LeftResultPage.Children[1] as StackPanel).Children[index] as SelfController02).Label_Two.Content = MainVocabularyManagement.ResultList[i][1]);
                    }
                }
                else
                {
                    if (i >= MainVocabularyManagement.ResultList.Count)
                    {
                        Dispatcher.Invoke(() => ((StackPanel_RightResultPage.Children[1] as StackPanel).Children[index - 5] as SelfController02).Button_One.Content = "");
                        Dispatcher.Invoke(() => ((StackPanel_RightResultPage.Children[1] as StackPanel).Children[index - 5] as SelfController02).Label_Two.Content = "");
                    }
                    else
                    {
                        Dispatcher.Invoke(() => ((StackPanel_RightResultPage.Children[1] as StackPanel).Children[index - 5] as SelfController02).Button_One.Content = MainVocabularyManagement.ResultList[i][0]);
                        Dispatcher.Invoke(() => ((StackPanel_RightResultPage.Children[1] as StackPanel).Children[index - 5] as SelfController02).Label_Two.Content = MainVocabularyManagement.ResultList[i][1]);
                    }
                }
            }
        }

        private void ShowInputWord()
        {
            for (int i = (TestAreaResultPageNumber - 1) * 10; i < TestAreaResultPageNumber * 10; i++)
            {
                int index = i % 10;

                if (index < 5)
                {
                    if (i >= MainVocabularyManagement.StringList.Count)
                    {
                        Dispatcher.Invoke(() => ((StackPanel_LeftResultPage.Children[1] as StackPanel).Children[index] as SelfController02).Button_One.Content = "");
                        Dispatcher.Invoke(() => ((StackPanel_LeftResultPage.Children[1] as StackPanel).Children[index] as SelfController02).Label_Two.Content = "");
                    }
                    else
                    {
                        Dispatcher.Invoke(() => ((StackPanel_LeftResultPage.Children[1] as StackPanel).Children[index] as SelfController02).Button_One.Content = "???");
                        Dispatcher.Invoke(() => ((StackPanel_LeftResultPage.Children[1] as StackPanel).Children[index] as SelfController02).Label_Two.Content = MainVocabularyManagement.StringList[i]);
                    }
                }
                else
                {
                    if (i >= MainVocabularyManagement.StringList.Count)
                    {
                        Dispatcher.Invoke(() => ((StackPanel_RightResultPage.Children[1] as StackPanel).Children[index - 5] as SelfController02).Button_One.Content = "");
                        Dispatcher.Invoke(() => ((StackPanel_RightResultPage.Children[1] as StackPanel).Children[index - 5] as SelfController02).Label_Two.Content = "");
                    }
                    else
                    {
                        Dispatcher.Invoke(() => ((StackPanel_RightResultPage.Children[1] as StackPanel).Children[index - 5] as SelfController02).Button_One.Content = "???");
                        Dispatcher.Invoke(() => ((StackPanel_RightResultPage.Children[1] as StackPanel).Children[index - 5] as SelfController02).Label_Two.Content = MainVocabularyManagement.StringList[i]);
                    }
                }
            }
        }

        private void TestMode(bool on)
        {
            if (on)
            {
                TestModeFlag = true;
                
                Dispatcher.Invoke(() => ComboBox_TestArea_Date.IsEnabled = false);
                Dispatcher.Invoke(() => WindowsFormsHost_Repeat.IsEnabled = false);
                Dispatcher.Invoke(() => WindowsFormsHost_Time.IsEnabled = false);
                Dispatcher.Invoke(() => RadioButton_Order.IsEnabled = false);
                Dispatcher.Invoke(() => RadioButton_Random.IsEnabled = false);
                Dispatcher.Invoke(() => CheckBox_Computer.IsEnabled = false);
                Dispatcher.Invoke(() => CheckBox_Paper.IsEnabled = false);
                Dispatcher.Invoke(() => Btn_WordTest.IsEnabled = false);
                //Dispatcher.Invoke(() => StackPanel_LeftVocabularyPage.IsEnabled = false);
                //Dispatcher.Invoke(() => StackPanel_RightVocabularyPage.IsEnabled = false);
                //Dispatcher.Invoke(() => StackPanel_LeftResultPage.IsEnabled = false);
                //Dispatcher.Invoke(() => StackPanel_RightResultPage.IsEnabled = false);

                if (CheckBox_Computer.IsChecked == true)
                {
                    Dispatcher.Invoke(() => TextBox_WordInput.IsEnabled = true);
                    Dispatcher.Invoke(() => Btn_WordInput.IsEnabled = true);
                }

                Dispatcher.Invoke(() => ComboBox_EditArea_Date.IsEnabled = false);
                Dispatcher.Invoke(() => TextBox_English.IsEnabled = false);
                Dispatcher.Invoke(() => TextBox_PartOfSpeech.IsEnabled = false);
                Dispatcher.Invoke(() => TextBox_Chinese.IsEnabled = false);
                Dispatcher.Invoke(() => Btn_WordSave.IsEnabled = false);

                Dispatcher.Invoke(() => RichTextBox_TestArea_Definition.Document.Blocks.Clear());
                Dispatcher.Invoke(() => TextBox_WordInput.Text = "");
            }
            else
            {
                TestModeFlag = false;

                Dispatcher.Invoke(() => ComboBox_TestArea_Date.IsEnabled = true);
                Dispatcher.Invoke(() => WindowsFormsHost_Repeat.IsEnabled = true);
                Dispatcher.Invoke(() => WindowsFormsHost_Time.IsEnabled = true);
                Dispatcher.Invoke(() => RadioButton_Order.IsEnabled = true);
                Dispatcher.Invoke(() => RadioButton_Random.IsEnabled = true);
                Dispatcher.Invoke(() => CheckBox_Computer.IsEnabled = true);
                Dispatcher.Invoke(() => CheckBox_Paper.IsEnabled = true);
                Dispatcher.Invoke(() => Btn_WordTest.IsEnabled = true);
                //Dispatcher.Invoke(() => StackPanel_LeftVocabularyPage.IsEnabled = true);
                //Dispatcher.Invoke(() => StackPanel_RightVocabularyPage.IsEnabled = true);
                //Dispatcher.Invoke(() => StackPanel_LeftResultPage.IsEnabled = true);
                //Dispatcher.Invoke(() => StackPanel_RightResultPage.IsEnabled = true);

                Dispatcher.Invoke(() => TextBox_WordInput.IsEnabled = false);
                Dispatcher.Invoke(() => Btn_WordInput.IsEnabled = false);

                Dispatcher.Invoke(() => ComboBox_EditArea_Date.IsEnabled = true);
                Dispatcher.Invoke(() => TextBox_English.IsEnabled = true);
                Dispatcher.Invoke(() => TextBox_PartOfSpeech.IsEnabled = true);
                Dispatcher.Invoke(() => TextBox_Chinese.IsEnabled = true);
                Dispatcher.Invoke(() => Btn_WordSave.IsEnabled = true);

                Dispatcher.Invoke(() => RichTextBox_TestArea_Definition.Document.Blocks.Clear());
                Dispatcher.Invoke(() => TextBox_WordInput.Text = "");
            }
        }
        #endregion

        #region "UI"
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UI_LogWritter.CloseAll(); ;

            ShowWindow(Hwnd, SW_HIDE);
        }

        private void MenuItem_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                switch ((sender as System.Windows.Controls.MenuItem).Name)
                {
                    default:
                        break;
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception Occurred When MenuItem Checked. Message:" + Ex.Message);
            }
        }

        private void MenuItem_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                switch ((sender as System.Windows.Controls.MenuItem).Name)
                {
                    default:
                        break;
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception Occurred When MenuItem Unchecked. Message:" + Ex.Message);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch ((sender as System.Windows.Controls.MenuItem).Name)
                {
                    case "MenuItem_RestartSoftware":
                        {
                            System.Windows.Forms.Application.ExitThread();
                            Restart();
                        }
                        break;

                    case "MenuItem_CheckDescription":
                        {
                            System.Diagnostics.Process.Start(System.Environment.CurrentDirectory + "/Appendix/Document/Description.pdf");
                        }
                        break;

                    default:
                        {
                            ChangeRichTextBox("Default", "MenuItem_Click Went Into Default.");
                        }
                        break;
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception Occurred When MenuItem Click. Message : " + Ex.Message);
            }
        }

        private void TabControl_Selected(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                switch ((sender as System.Windows.Controls.TabControl).Name)
                {
                    case "TabControl_Player":
                        {

                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception Occurred When TabControl Selected. Message : " + Ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsInitializationComplete)
                    switch ((sender as System.Windows.Controls.Button).Name)
                    {
                        case "Btn_WordInput":
                            {
                                if (!MainVocabularyManagement.InputFlag)
                                {
                                    string word = TextBox_WordInput.Text;

                                    Dispatcher.Invoke(() => TextBox_WordInput.Text = "");

                                    MainVocabularyManagement.StringList.Add(word);
                                    MainVocabularyManagement.InputString = word;
                                    MainVocabularyManagement.InputFlag = true;

                                    if (MainVocabularyManagement.StringList.Count() == 0)
                                        TestAreaResultPageNumber = 0; // =1?
                                    else if (MainVocabularyManagement.StringList.Count() % 10 == 0)
                                        TestAreaResultPageNumber = MainVocabularyManagement.StringList.Count() / 10;
                                    else
                                        TestAreaResultPageNumber = MainVocabularyManagement.StringList.Count() / 10 + 1;
                                    ShowInputWord();
                                }
                            }
                            break;

                        case "Btn_WordSave":
                            {
                                if (MainVocabularyManagement.WhetherDateCorrect(EditAreaDate))
                                {
                                    if (TextBox_English.Text != string.Empty & TextBox_PartOfSpeech.Text != string.Empty & TextBox_Chinese.Text != string.Empty)
                                        if (MainVocabularyManagement.InputWord(EditAreaDate, TextBox_English.Text, TextBox_PartOfSpeech.Text, TextBox_Chinese.Text, true))
                                        {
                                            Dispatcher.Invoke(() => RichTextBox_EditArea_Definition.Document.Blocks.Clear());
                                            Dispatcher.Invoke(() => RichTextBox_EditArea_Definition.AppendText("單字[" + TextBox_English.Text + "]新增成功!"));

                                            for (int i = 0; i < MainVocabularyManagement.VocabularySet.Days.Length; i++)
                                            {
                                                if (EditAreaDate == MainVocabularyManagement.VocabularySet.Days[i].Date)
                                                {
                                                    EditAreaPageNumber = MainVocabularyManagement.VocabularySet.Days[i].VocaburaryNumber() % 10 == 0 ? MainVocabularyManagement.VocabularySet.Days[i].VocaburaryNumber() / 10 : MainVocabularyManagement.VocabularySet.Days[i].VocaburaryNumber() / 10 + 1;

                                                    for (int j = (EditAreaPageNumber - 1) * 10; j < EditAreaPageNumber * 10; j++)
                                                    {
                                                        int index = j % 10;

                                                        if (index < 5)
                                                            ((StackPanel_LeftVocabularyPage.Children[1] as StackPanel).Children[index] as ThreeLabel).ShowVocabularyInformation(MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].English, MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].PartOfSpeech, MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].Chinese);
                                                        else
                                                            ((StackPanel_RightVocabularyPage.Children[1] as StackPanel).Children[index - 5] as ThreeLabel).ShowVocabularyInformation(MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].English, MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].PartOfSpeech, MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].Chinese);
                                                    }

                                                    break;
                                                }
                                            }
                                        }                          
                                        else
                                        {
                                            Dispatcher.Invoke(() => RichTextBox_EditArea_Definition.Document.Blocks.Clear());
                                            Dispatcher.Invoke(() => RichTextBox_EditArea_Definition.AppendText("單字[" + TextBox_English.Text + "]新增失敗!"));
                                        }
                                    else
                                        System.Windows.MessageBox.Show("請輸入英文,詞性與中文!", "訊息");
                                }
                                else
                                    System.Windows.MessageBox.Show("請選擇日期!", "訊息");

                                Dispatcher.Invoke(() => TextBox_English.Text = "");
                                Dispatcher.Invoke(() => TextBox_PartOfSpeech.Text = "");
                                Dispatcher.Invoke(() => TextBox_Chinese.Text = "");
                            }
                            break;

                        case "Btn_WordTest":
                            {
                                MainVocabularyManagement.InputFlag = false;
                                MainVocabularyManagement.InputNumber = 0;
                                MainVocabularyManagement.InputString = "";
                                MainVocabularyManagement.InputTime = Convert.ToDouble(NumericUpDown_Time.Value);
                                MainVocabularyManagement.StringList = new List<string>();
                                MainVocabularyManagement.ResultList = new List<List<string>>();

                                if (CheckBox_Computer.IsChecked == true)
                                {
                                    if (RadioButton_Order.IsChecked == true)
                                    {
                                        Thread vocaburarytestthread = new Thread(() => MainVocabularyManagement.TestWord(true, true, TestAreaDate, Convert.ToInt32(NumericUpDown_Repeat.Value)));
                                        vocaburarytestthread.Start();
                                    }

                                    if (RadioButton_Random.IsChecked == true)
                                    {
                                        Thread vocaburarytestthread = new Thread(() => MainVocabularyManagement.TestWord(true, false, TestAreaDate, Convert.ToInt32(NumericUpDown_Repeat.Value)));
                                        vocaburarytestthread.Start();
                                    }
                                }
                                else if (CheckBox_Paper.IsChecked == true)
                                {
                                    if (RadioButton_Order.IsChecked == true)
                                    {
                                        Thread vocaburarytestthread = new Thread(() => MainVocabularyManagement.TestWord(false, true, TestAreaDate, Convert.ToInt32(NumericUpDown_Repeat.Value)));
                                        vocaburarytestthread.Start();
                                    }

                                    if (RadioButton_Random.IsChecked == true)
                                    {
                                        Thread vocaburarytestthread = new Thread(() => MainVocabularyManagement.TestWord(false, false, TestAreaDate, Convert.ToInt32(NumericUpDown_Repeat.Value)));
                                        vocaburarytestthread.Start();
                                    }
                                }

                                TestMode(true);

                                TestAreaResultPageNumber = 1;
                                ShowInputWord();
                            }
                            break;

                        case "Btn_LastPage":
                            {
                                if (EditAreaPageNumber > 1)
                                    EditAreaPageNumber--;

                                for (int i = 0; i < MainVocabularyManagement.VocabularySet.Days.Length; i++)
                                {
                                    if (EditAreaDate == MainVocabularyManagement.VocabularySet.Days[i].Date)
                                    {
                                        for (int j = (EditAreaPageNumber - 1) * 10; j < EditAreaPageNumber * 10; j++)
                                        {
                                            int index = j % 10;

                                            if (index < 5)
                                                ((StackPanel_LeftVocabularyPage.Children[1] as StackPanel).Children[index] as ThreeLabel).ShowVocabularyInformation(MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].English, MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].PartOfSpeech, MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].Chinese);
                                            else
                                                ((StackPanel_RightVocabularyPage.Children[1] as StackPanel).Children[index - 5] as ThreeLabel).ShowVocabularyInformation(MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].English, MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].PartOfSpeech, MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].Chinese);
                                        }

                                        break;
                                    }
                                }
                            }
                            break;

                        case "Btn_NextPage":
                            {
                                // 每一頁10單字 一天最多100單字 所以頁數不得超過10
                                if (EditAreaPageNumber < 10)
                                    EditAreaPageNumber++;

                                for (int i = 0; i < MainVocabularyManagement.VocabularySet.Days.Length; i++)
                                {
                                    if (EditAreaDate == MainVocabularyManagement.VocabularySet.Days[i].Date)
                                    {
                                        for (int j = (EditAreaPageNumber - 1) * 10; j < EditAreaPageNumber * 10; j++)
                                        {
                                            int index = j % 10;

                                            if (index < 5)                                           
                                                ((StackPanel_LeftVocabularyPage.Children[1] as StackPanel).Children[index] as ThreeLabel).ShowVocabularyInformation(MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].English, MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].PartOfSpeech, MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].Chinese);                                           
                                            else
                                                ((StackPanel_RightVocabularyPage.Children[1] as StackPanel).Children[index - 5] as ThreeLabel).ShowVocabularyInformation(MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].English, MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].PartOfSpeech, MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].Chinese);
                                        }

                                        break;
                                    }
                                }
                            }
                            break;

                        case "Btn_LastResultPage":
                            {
                                if (TestAreaResultPageNumber > 1)
                                    TestAreaResultPageNumber--;

                                if (TestModeFlag)
                                    ShowInputWord();
                                else
                                    ShowTestResult();
                            }
                            break;

                        case "Btn_NextResultPage":
                            {
                                if (TestAreaResultPageNumber < int.MaxValue)
                                    TestAreaResultPageNumber++;

                                if (TestModeFlag)
                                    ShowInputWord();
                                else
                                    ShowTestResult();
                            }
                            break;

                        case "Btn_LastRecordPage":
                            {
                                if (TestAreaRecordPageNumber > 1)
                                    TestAreaRecordPageNumber--;

                                ShowPastGrade();
                            }
                            break;

                        case "Btn_NextRecordPage":
                            {
                                if (TestAreaRecordPageNumber < int.MaxValue)
                                    TestAreaRecordPageNumber++;

                                ShowPastGrade();
                            }
                            break;

                        default:
                            {
                                ChangeRichTextBox("Default", "Btn_Click Went Into Default.");
                            }
                            break;
                    }
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception Occurred When Button Clicked. Message:" + Ex.Message);
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                switch ((sender as System.Windows.Controls.RadioButton).Name)
                {
                    case "RadioButton_Order":
                        {

                        }
                        break;

                    case "RadioButton_Random":
                        {

                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception Occurred When RadioButton Checked. Message:" + Ex.Message);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsInitializationComplete)
                    switch ((sender as System.Windows.Controls.CheckBox).Name)
                    {
                        case "CheckBox_Paper":
                            {
                                if (CheckBox_Paper.IsChecked == true & CheckBox_Computer.IsChecked == true)
                                    CheckBox_Computer.IsChecked = false;
                            }
                            break;

                        case "CheckBox_Computer":
                            {
                                if (CheckBox_Paper.IsChecked == true & CheckBox_Computer.IsChecked == true)
                                    CheckBox_Paper.IsChecked = false;
                            }
                            break;

                        default:
                            break;
                    }
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception Occurred When CheckBox Checked. Message:" + Ex.Message);
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsInitializationComplete)
                    switch ((sender as System.Windows.Controls.CheckBox).Name)
                    {
                        case "CheckBox_Paper":
                            {

                            }
                            break;


                        case "CheckBox_Computer":
                            {

                            }
                            break;

                        default:
                            break;
                    }
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception Occurred When CheckBox Unchecked. Message:" + Ex.Message);
            }
        }

        private void NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (IsInitializationComplete)
                {
                    switch ((sender as System.Windows.Forms.NumericUpDown).AccessibleName)
                    {
                        case "NumericUpDown_Time":
                            {

                            }
                            break;

                        case "NumericUpDown_Repeat":
                            {

                            }
                            break;

                        default:
                            {
                                ChangeRichTextBox("Default", "NumericUpDown_ValueChanged Went Into Default.");
                            }
                            break;
                    }
                }
            }
            catch (Exception Ex)
            {
                ChangeRichTextBox("Exception", "Exception Occurred When NumericUpDown Value Changed. Message:" + Ex.Message);
                Console.WriteLine("Exception Occurred When NumericUpDown Value Changed. Message:" + Ex.Message);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsInitializationComplete)
            {
                try
                {
                    switch ((sender as System.Windows.Controls.ComboBox).Name)
                    {
                        case "ComboBox_EditArea_Date":
                            {
                                if (ComboBox_EditArea_Date.SelectedValue.ToString() == "New")
                                {
                                    string date = ChangeTimeFormat(4, System.DateTime.Now);
                                    string file = @"\" + date + ".txt";
                                    string path_01 = PersonalListFolderPath + file;
                                    string path_02 = PassGradeFolderPath + file;
                                    EditAreaDate = Convert.ToInt32(date);
                                    EditAreaPageNumber = 1; //待修正

                                    if (File.Exists(path_01))
                                    {
                                        ComboBox_EditArea_Date.SelectedIndex = ComboBox_EditArea_Date.Items.Count - 2;
                                        System.Windows.MessageBox.Show("今日文件已存在", "訊息");
                                    }
                                    else
                                    {
                                        FileStream fs_01 = File.Create(path_01);
                                        fs_01.Close();

                                        FileStream fs_02 = File.Create(path_02);
                                        fs_02.Close();

                                        MainVocabularyManagement.InputDocument(EditAreaDate, path_01, path_02);

                                        ComboBox_EditArea_Date.Items.Insert(ComboBox_EditArea_Date.Items.Count - 1, date.Insert(4, "/").Insert(7, "/"));
                                        ComboBox_TestArea_Date.Items.Add(date.Insert(4, "/").Insert(7, "/"));
                                        ComboBox_EditArea_Date.SelectedIndex = ComboBox_EditArea_Date.Items.Count - 2;
                                    }
                                }
                                else
                                {
                                    EditAreaDate = Convert.ToInt32(ComboBox_EditArea_Date.SelectedValue.ToString().Remove(4, 1).Remove(6, 1));

                                    for (int i = 0; i < MainVocabularyManagement.VocabularySet.Days.Length; i++)
                                    {
                                        if (EditAreaDate == MainVocabularyManagement.VocabularySet.Days[i].Date)
                                        {
                                            EditAreaPageNumber = 1;

                                            for (int j = (EditAreaPageNumber - 1) * 10; j < EditAreaPageNumber * 10; j++)
                                            {
                                                int index = j % 10;

                                                if (index < 5)
                                                    ((StackPanel_LeftVocabularyPage.Children[1] as StackPanel).Children[index] as ThreeLabel).ShowVocabularyInformation(MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].English, MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].PartOfSpeech, MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].Chinese);
                                                else
                                                    ((StackPanel_RightVocabularyPage.Children[1] as StackPanel).Children[index - 5] as ThreeLabel).ShowVocabularyInformation(MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].English, MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].PartOfSpeech, MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].Chinese);
                                            }

                                            Dispatcher.Invoke(() => RichTextBox_EditArea_Definition.Document.Blocks.Clear());
                                            Dispatcher.Invoke(() => RichTextBox_EditArea_Definition.AppendText("定義:"));

                                            break;
                                        }
                                    }
                                }
                            }
                            break;

                        case "ComboBox_TestArea_Date":
                            {
                                TestAreaDate = Convert.ToInt32(ComboBox_TestArea_Date.SelectedValue.ToString().Remove(4, 1).Remove(6, 1));
                                TestAreaResultPageNumber = 1;
                                TestAreaRecordPageNumber = 1;

                                ShowPastGrade();
                            }
                            break;

                        default:
                            break;
                    }
                }
                catch (Exception Ex)
                {
                    Console.WriteLine("Exception Occurred When ComboBox Selected. Message:" + Ex.Message);
                }
            }
        }

        private void Controller_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (IsInitializationComplete)
            {
                try
                {
                    switch (sender.GetType().Name)
                    {
                        case "Grid":
                            {
                                switch ((sender as System.Windows.Controls.Grid).Name)
                                {
                                    case "Grid_UI":
                                        {
                                            if (e.Key == Key.S && (Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Shift)) == (ModifierKeys.Control | ModifierKeys.Shift))
                                            {
                                                ShowWindow(Hwnd, SW_SHOW);
                                                MenuItem_Debug.Visibility = System.Windows.Visibility.Visible;
                                                TabItem_Debug.Visibility = System.Windows.Visibility.Visible;
                                            }

                                            if (e.Key == Key.H && (Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Shift)) == (ModifierKeys.Control | ModifierKeys.Shift))
                                            {
                                                ShowWindow(Hwnd, SW_HIDE);
                                                MenuItem_Debug.Visibility = System.Windows.Visibility.Hidden;
                                                TabItem_Debug.Visibility = System.Windows.Visibility.Hidden;
                                            }
                                        }
                                        break;

                                    default:
                                        break;
                                }
                            }
                            break;

                        case "TextBox":
                            {
                                switch ((sender as System.Windows.Controls.TextBox).Name)
                                {
                                    case "TextBox_WordInput":
                                        {
                                            if (e.Key == Key.Return)
                                            {
                                                if (!MainVocabularyManagement.InputFlag)
                                                {
                                                    string word = TextBox_WordInput.Text;

                                                    Dispatcher.Invoke(() => TextBox_WordInput.Text = "");

                                                    MainVocabularyManagement.StringList.Add(word);
                                                    MainVocabularyManagement.InputString = word;
                                                    MainVocabularyManagement.InputFlag = true;

                                                    if (MainVocabularyManagement.StringList.Count() == 0)
                                                        TestAreaResultPageNumber = 0; // =1?
                                                    else if (MainVocabularyManagement.StringList.Count() % 10 == 0)
                                                        TestAreaResultPageNumber = MainVocabularyManagement.StringList.Count() / 10;
                                                    else
                                                        TestAreaResultPageNumber = MainVocabularyManagement.StringList.Count() / 10 + 1;
                                                    ShowInputWord();
                                                }
                                            }
                                        }
                                        break;

                                    case "TextBox_English":
                                    case "TextBox_PartOfSpeech":
                                    case "TextBox_Chinese":
                                        {
                                            if (!TestModeFlag)
                                                if (e.Key == Key.S && (Keyboard.Modifiers & (ModifierKeys.Control)) == (ModifierKeys.Control))
                                            {
                                                if (MainVocabularyManagement.WhetherDateCorrect(EditAreaDate))
                                                {
                                                    if (TextBox_English.Text != string.Empty & TextBox_PartOfSpeech.Text != string.Empty & TextBox_Chinese.Text != string.Empty)
                                                        if (MainVocabularyManagement.InputWord(EditAreaDate, TextBox_English.Text, TextBox_PartOfSpeech.Text, TextBox_Chinese.Text, true))
                                                        {
                                                            Dispatcher.Invoke(() => RichTextBox_EditArea_Definition.Document.Blocks.Clear());
                                                            Dispatcher.Invoke(() => RichTextBox_EditArea_Definition.AppendText("單字[" + TextBox_English.Text + "]新增成功!"));

                                                            for (int i = 0; i < MainVocabularyManagement.VocabularySet.Days.Length; i++)
                                                            {
                                                                if (EditAreaDate == MainVocabularyManagement.VocabularySet.Days[i].Date)
                                                                {
                                                                    EditAreaPageNumber = MainVocabularyManagement.VocabularySet.Days[i].VocaburaryNumber() % 10 == 0 ? MainVocabularyManagement.VocabularySet.Days[i].VocaburaryNumber() / 10 : MainVocabularyManagement.VocabularySet.Days[i].VocaburaryNumber() / 10 + 1;

                                                                    for (int j = (EditAreaPageNumber - 1) * 10; j < EditAreaPageNumber * 10; j++)
                                                                    {
                                                                        int index = j % 10;

                                                                        if (index < 5)
                                                                            ((StackPanel_LeftVocabularyPage.Children[1] as StackPanel).Children[index] as ThreeLabel).ShowVocabularyInformation(MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].English, MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].PartOfSpeech, MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].Chinese);
                                                                        else
                                                                            ((StackPanel_RightVocabularyPage.Children[1] as StackPanel).Children[index - 5] as ThreeLabel).ShowVocabularyInformation(MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].English, MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].PartOfSpeech, MainVocabularyManagement.VocabularySet.Days[i].Vocaburaries[j].Chinese);
                                                                    }

                                                                    break;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Dispatcher.Invoke(() => RichTextBox_EditArea_Definition.Document.Blocks.Clear());
                                                            Dispatcher.Invoke(() => RichTextBox_EditArea_Definition.AppendText("單字[" + TextBox_English.Text + "]新增失敗!"));
                                                        }
                                                    else
                                                        System.Windows.MessageBox.Show("請輸入英文,詞性與中文!", "訊息");
                                                }
                                                else
                                                    System.Windows.MessageBox.Show("請選擇日期!", "訊息");

                                                Dispatcher.Invoke(() => TextBox_English.Text = "");
                                                Dispatcher.Invoke(() => TextBox_PartOfSpeech.Text = "");
                                                Dispatcher.Invoke(() => TextBox_Chinese.Text = "");
                                            }
                                        }
                                        break;

                                    default:
                                        break;
                                }
                            }
                            break;

                        default:
                            break;
                    }
                }
                catch (Exception Ex)
                {
                    Console.WriteLine("Exception Occurred When keyboard was pressed. Message:" + Ex.Message);
                }
            }
        }
        #endregion

        #region "Debug"
        private void MenuItem_Test_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                switch ((sender as System.Windows.Controls.MenuItem).Name)
                {
                    case "MenuItem_DebugFunction01":
                        {
                            System.Windows.MessageBox.Show("無功能", "訊息");
                        }
                        break;

                    case "MenuItem_DebugFunction02":
                        {
                            System.Windows.MessageBox.Show("無功能", "訊息");
                        }
                        break;

                    case "MenuItem_DebugFunction03":
                        {
                            System.Windows.MessageBox.Show("無功能", "訊息");
                        }
                        break;

                    case "MenuItem_DebugFunction04":
                        {
                            System.Windows.MessageBox.Show("無功能", "訊息");
                        }
                        break;

                    case "MenuItem_DebugFunction05":
                        {
                            System.Windows.MessageBox.Show("無功能", "訊息");
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception Occurred When MenuItem Been Clicked. Message:" + Ex.Message);
            }
        }

        private void MenuItem_Test_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                switch ((sender as System.Windows.Controls.MenuItem).Name)
                {
                    case "MenuItem_DebugFunction01":
                        {
                            System.Windows.MessageBox.Show("無功能", "訊息");
                        }
                        break;

                    case "MenuItem_DebugFunction02":
                        {
                            System.Windows.MessageBox.Show("無功能", "訊息");
                        }
                        break;

                    case "MenuItem_DebugFunction03":
                        {
                            System.Windows.MessageBox.Show("無功能", "訊息");
                        }
                        break;

                    case "MenuItem_DebugFunction04":
                        {
                            System.Windows.MessageBox.Show("無功能", "訊息");
                        }
                        break;

                    case "MenuItem_DebugFunction05":
                        {
                            System.Windows.MessageBox.Show("無功能", "訊息");
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception Occurred When MenuItem Been Unclicked. Message:" + Ex.Message);
            }
        }

        private void MenuItem_Test_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch ((sender as System.Windows.Controls.MenuItem).Name)
                {
                    case "MenuItem_DebugFunction06":
                        {
                            var synthesizer = new SpeechSynthesizer();
                            synthesizer.SetOutputToDefaultAudioDevice();
                            synthesizer.Speak("All we need to do is to make sure we keep talking");
                            synthesizer.Speak("發音囉你好");
                            Console.WriteLine("中文語音結束!");
                        }
                        break;

                    case "MenuItem_DebugFunction07":
                        {
                            UI_Config.Save();

                            MainVocabularyManagement.VocaburaryConfig.Save();
                        }
                        break;

                    case "MenuItem_DebugFunction08":
                        {
                            StreamWriter sw = new StreamWriter(@"C:\Users\Larry\Desktop\test\test\20221016.txt", true, Encoding.GetEncoding("big5"));
                            sw.WriteLine("apple@名詞@蘋果");
                            sw.WriteLine("cat@名詞@貓");
                            sw.Close();
                        }
                        break;

                    case "MenuItem_DebugFunction09":
                        {
                            int a = 5, b = 10;
                            Console.WriteLine("{0,22} {1,22}", a, b);
                        }
                        break;

                    case "MenuItem_DebugFunction10":
                        {

                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception Occurred When MenuItem Been Clicked. Message:" + Ex.Message);
                Console.WriteLine(string.Format("{0}", Ex.ToString()));
            }
        }

        private void Button_Test_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch ((sender as System.Windows.Controls.Button).Name)
                {
                    case "Button_Test01":
                        {
                            MainVocabularyManagement.ListWord();
                            MainVocabularyManagement.SearchWord("apple");
                        }
                        break;

                    case "Button_Test02":
                        {
                            Console.WriteLine("第三個測試功能:將單字分到a-z資料夾中");

                            string path_01 = System.Environment.CurrentDirectory + @"\Appendix\Vocaburary\SimpleWord\FullEnglishWord.txt";
                            string path_02 = System.Environment.CurrentDirectory + @"\Appendix\Vocaburary\SimpleWord\";

                            StreamReader notebook_01 = new StreamReader(path_01, Encoding.GetEncoding("big5"));
                            string line = notebook_01.ReadLine();
                            char word = ' ';
                            while (line != null)
                            {
                                Console.WriteLine(line);

                                if (line[0] != word)
                                {
                                    /*for (char i = 'a'; i <= 'z'; i++)
                                    {
                                        if (line[0] == i)
                                        {
                                            word = i;
                                            break;
                                        }
                                    }*/

                                    word = line[0];
                                }

                                string path_03 = path_02 + Char.ToString(word) + ".txt";
                                StreamWriter notebook_02 = new StreamWriter(path_03, true, Encoding.GetEncoding("big5"));
                                notebook_02.WriteLine(line);
                                notebook_02.Close();

                                line = notebook_01.ReadLine();
                            }
                            notebook_01.Close();
                        }
                        break;

                    case "Button_Test03":
                        {
                            Console.WriteLine("第二個測試按鈕:建立a-z資料夾");

                            string path_01 = System.Environment.CurrentDirectory + @"\Appendix\Vocaburary\SimpleWord\";
                           
                            for (char i = 'a'; i <= 'z'; i++)
                            {
                                string path_02 = path_01 + Char.ToString(i) + ".txt";
                                FileStream fs = File.Create(path_02);
                                fs.Close();
                            }
                        }
                        break;

                    case "Button_Test04":
                        {
                            Random crandom = new Random();
                            Console.WriteLine("Test_0401: " + crandom.Next());

                            Random rand = new Random(Guid.NewGuid().GetHashCode());

                            List<int> listLinq = new List<int>(Enumerable.Range(1, 10));
                            listLinq = listLinq.OrderBy(num => rand.Next()).ToList<int>();

                            for (int i = 0; i < 10; i++)
                            {
                                Console.WriteLine("Test_0402: " + listLinq[i].ToString());
                            }
                        }
                        break;

                    case "Button_Test05":
                        {
                            ChoiceList threeoption = new ChoiceList(sender);
                            //threeoption.ChoiceReturnEvent += ChoiceList_DecisionEvent;
                            threeoption.Show();
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception Occurred When Button Was Clicked. Message:" + Ex.Message);      
            }
        }

        private void CheckBox_Test_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                switch ((sender as System.Windows.Controls.CheckBox).Name)
                {
                    case "CheckBox_TestFlag01":
                        {

                            System.Windows.MessageBox.Show("無功能", "訊息");
                        }
                        break;

                    case "CheckBox_TestFlag02":
                        {
                            System.Windows.MessageBox.Show("無功能", "訊息");
                        }
                        break;

                    case "CheckBox_TestFlag03":
                        {
                            System.Windows.MessageBox.Show("無功能", "訊息");
                        }
                        break;

                    case "CheckBox_TestFlag04":
                        {
                            System.Windows.MessageBox.Show("無功能", "訊息");
                        }
                        break;

                    case "CheckBox_TestFlag05":
                        {
                            System.Windows.MessageBox.Show("無功能", "訊息");
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception Occurred When CheckBox Been Checked. Message:" + Ex.Message);
            }
        }

        private void CheckBox_Test_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                switch ((sender as System.Windows.Controls.CheckBox).Name)
                {
                    case "CheckBox_TestFlag01":
                        {
                            System.Windows.MessageBox.Show("無功能", "訊息");
                        }
                        break;

                    case "CheckBox_TestFlag02":
                        {
                            System.Windows.MessageBox.Show("無功能", "訊息");
                        }
                        break;

                    case "CheckBox_TestFlag03":
                        {
                            System.Windows.MessageBox.Show("無功能", "訊息");
                        }
                        break;

                    case "CheckBox_TestFlag04":
                        {
                            System.Windows.MessageBox.Show("無功能", "訊息");
                        }
                        break;

                    case "CheckBox_TestFlag05":
                        {
                            System.Windows.MessageBox.Show("無功能", "訊息");
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception Occurred When CheckBox Been Unchecked. Message:" + Ex.Message);
            }
        }
        #endregion

        #region "Reserve"
        public void DeleteFolder(string path, bool itself)
        {
            try
            {
                //去除資料夾的只讀屬性 //自己無實試與理解此段Code
                //System.IO.DirectoryInfo fileInfo = new System.IO.DirectoryInfo(path);
                //fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;

                //去除檔案的只讀屬性 //自己無實試與理解此段Code
                //System.IO.File.SetAttributes(path, System.IO.FileAttributes.Normal);

                //判斷資料夾是否還存在
                if (Directory.Exists(path))
                {
                    foreach (string f in Directory.GetFileSystemEntries(path))
                    {
                        if (File.Exists(f))
                            File.Delete(f);//如果有子檔案刪除檔案
                        else
                            DeleteFolder(f, true);//迴圈遞迴刪除子資料夾 
                    }

                    if (itself)
                        Directory.Delete(path);//刪除空資料夾
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("The Message Of The Exception: " + ex.Message);
            }
        }

        public void GetFolder()
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "請選擇資料夾";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                    System.Windows.MessageBox.Show(this, "資料夾路徑不能為空", "提示");
            }
            else
            {
                System.Windows.MessageBox.Show(this, "資料夾路徑並未選取", "提示");
            }
        }

        public void GetFileDetail(string path)
        {
            if (File.Exists(path))
            {
                Console.WriteLine("取得檔名(不包含附檔名):" + System.IO.Path.GetFileNameWithoutExtension(path));
                Console.WriteLine("取得副檔名:" + System.IO.Path.GetExtension(path));
                Console.WriteLine("取得根目錄:" + System.IO.Path.GetPathRoot(path));
                Console.WriteLine("取得路徑:" + System.IO.Path.GetFullPath(path));
            }
        }

        public void GetImageFromFolder()
        {
            try
            {
                Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
                openFileDialog.Multiselect = false;
                openFileDialog.Filter = "Bitmap files (*.bmp)|*.bmp|JPEG files (*.jpg)|*.jpg|All files (*.*)|*.*";

                if (openFileDialog.ShowDialog() == true)
                {
                    DebugBgrImage[0] = new Image<Bgr, byte>(openFileDialog.FileName);
                }
                else
                {
                    Console.WriteLine("Image Did not Open.");
                }
            }
            catch(Exception Ex)
            {
                Console.WriteLine(string.Format("{0}", Ex.ToString()));
            }
        }
        #endregion
    }
}
