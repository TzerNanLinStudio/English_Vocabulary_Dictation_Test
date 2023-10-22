using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using Config_sharp;
using RestSharp;

namespace VocaburaryCore
{
    public class OneVocaburary
    {
        public string English;
        public string PartOfSpeech;
        public string Chinese;

        public OneVocaburary()
        {
            English = "";
            PartOfSpeech = "";
            Chinese = "";
        }

        public OneVocaburary(string r_English, string r_PartOfSpeech, string r_Chinese)
        {
            English = r_English;
            PartOfSpeech = r_PartOfSpeech;
            Chinese = r_Chinese;
        }

        public OneVocaburary(OneVocaburary r_Object)
        {
            English = r_Object.English;
            PartOfSpeech = r_Object.PartOfSpeech;
            Chinese = r_Object.Chinese;
        }

        public bool IsEmpty()
        {
            if (English == string.Empty & PartOfSpeech == string.Empty & Chinese == string.Empty)
                return true;
            else
                return false;
        }

        public override string ToString()
        {
            return "English: " + English + " / Part of speech: " + PartOfSpeech + " / Chinese: " + Chinese;
        }
    }
    
    public class VocaburariesOfOneDay
    {
        private string r_PersonalListFilePath;
        private string r_PastGradeFilePath;
        private int r_Date;
        private OneVocaburary[] r_Vocaburaries;

        public VocaburariesOfOneDay()
        {
            r_PersonalListFilePath = "";
            r_Date = -1;
            r_Vocaburaries = new OneVocaburary[100];
            for (int i = 0; i < 100; i++)
                r_Vocaburaries[i] = new OneVocaburary();
        }

        public string PersonalListFilePath { get => r_PersonalListFilePath; set => r_PersonalListFilePath = value; }
        public string PastGradeFilePath { get => r_PastGradeFilePath; set => r_PastGradeFilePath = value; }

        public int Date
        {
            set
            {
                r_Date = (value / 10000 <= 2100 && value / 10000 >= 2000 && value / 100 % 100 <= 12 && value / 100 % 100 >= 1 && value % 100 <= 31 && value % 100 >= 1) ? value : -1;
            }

            get
            {
                return r_Date;
            }
        }

        public OneVocaburary[] Vocaburaries { get => r_Vocaburaries; set => r_Vocaburaries = value; }

        public int VocaburaryNumber()
        {
            int number = 0;

            for (int j = 0; j < Vocaburaries.Length; j++)
                if (Vocaburaries[j].IsEmpty())
                    break;
                else
                    number++;

            return number;
        }
    }

    public class VocaburariesOfDays
    {
        private VocaburariesOfOneDay[] r_Days;

        public VocaburariesOfDays()
        {
            r_Days = new VocaburariesOfOneDay[365];
            for (int i = 0; i < 365; i++)
                r_Days[i] = new VocaburariesOfOneDay();
        }

        public VocaburariesOfOneDay[] Days { get => r_Days; set => r_Days = value; }
    }

    /// <summary>
    /// This property always returns a value &lt; 1.
    /// </summary>
    public class VocaburaryManagement
    {
        // Variables For Vocaburay.
        // ---------------------------------------------------------------
        public VocaburariesOfDays VocabularySet;
        private string SingleWordFolderPath = System.Environment.CurrentDirectory + @"\Appendix\Vocaburary\SingleWord";
        public string InputString = "";
        public int InputNumber = 0;
        public bool InputFlag = false;
        public double InputTime = 1.5;
        public List<string> StringList = new List<string>();
        public List<List<string>> ResultList = new List<List<string>>();
        // ---------------------------------------------------------------

        // Event For Returning Result.
        // ------------------------------
        public delegate void MessageReturnHandler(string mode, int number, string message);
        public event MessageReturnHandler MessageReturnEvent;
        // ------------------------------

        // Variables For Config.
        // ---------------------------------------------------------------
        public CustomConfig_EnglishVocaburary VocaburaryConfig;
        private string p_RecipeDirectoryPath = System.Environment.CurrentDirectory + @"\Appendix\Config\";
        private string p_RecipeFilename = "Vocaburary";
        private string p_RecipeSubtitle = ".dat";
        private string p_RecipeFullPath = "";
        // ---------------------------------------------------------------

        public VocaburaryManagement()
        {
            // Initailization For Vocaburay.
            // ---------------------------------------------------------------
            VocabularySet = new VocaburariesOfDays();
            // ---------------------------------------------------------------

            // Initailization For Config.
            // ---------------------------------------------------------------
            Config_Init(false, p_RecipeDirectoryPath + p_RecipeFilename + p_RecipeSubtitle);
            // ---------------------------------------------------------------
        }

        private void Config_Init(bool r_WhetherLoad, string r_Path)
        {
            p_RecipeFullPath = r_Path;

            VocaburaryConfig = new CustomConfig_EnglishVocaburary(p_RecipeFullPath);

            if (r_WhetherLoad)
                if (VocaburaryConfig.Load() == false) return;
        }

        private string PresentTime()
        {
            System.DateTime datetime = DateTime.Now;
            return datetime.Month.ToString("D2") + "/" + datetime.Day.ToString("D2") + " " + datetime.Hour.ToString("D2") + ":" + datetime.Minute.ToString("D2");
        }

        public bool WhetherDateCorrect(int r_Date)
        {
            int r_Year = r_Date / 10000;
            int r_Month = (r_Date % 10000) / 100;
            int r_Day = r_Date % 100;

            if (r_Year < 2000 | r_Year > 3000)
                return false;
            if (r_Month < 1 | r_Month > 12)
                return false;
            if ((r_Month == 1 | r_Month == 3 | r_Month == 5 | r_Month == 7 | r_Month == 8 | r_Month == 10 | r_Month == 12) & (r_Day < 1 | r_Day > 31))
                return false;
            if ((r_Month == 4 | r_Month == 6 | r_Month == 9 | r_Month == 11) & (r_Day < 1 | r_Day > 30))
                return false;
            if ((r_Year % 4 == 0) & (r_Month == 2) & (r_Day < 1 | r_Day > 29))
                return false;
            if ((r_Year % 4 != 0) & (r_Month == 2) & (r_Day < 1 | r_Day > 28))
                return false;
            return true;
        }

        public bool InputDocument(int date, string personal_list_file_path, string pass_grade_file_path)
        {
            if (WhetherDateCorrect(date))
            {
                for (int i = 0; i < VocabularySet.Days.Length; i++)
                {
                    if (VocabularySet.Days[i].Date == -1)
                    {
                        VocabularySet.Days[i].Date = date;
                        VocabularySet.Days[i].PersonalListFilePath = personal_list_file_path;
                        VocabularySet.Days[i].PastGradeFilePath = pass_grade_file_path;
                        break;
                    }
                }
            }

            return false;
        }

        public bool InputWord(int r_Date, string r_English, string r_PartOfSpeech, string r_Chinese, bool r_Save)
        {
            if (r_English != string.Empty & r_PartOfSpeech != string.Empty & r_Chinese != string.Empty & IsWordCorrect(r_English))
            {
                for (int i = 0; i < VocabularySet.Days.Length; i++)
                {
                    if (VocabularySet.Days[i].Date == r_Date)
                    {
                        for (int j = 0; j < VocabularySet.Days[i].Vocaburaries.Length; j++)
                        {
                            if (VocabularySet.Days[i].Vocaburaries[j].English == r_English)
                            {
                                Console.WriteLine("The word (" + r_English + ") had been added.");
                                break;
                            }

                            if (VocabularySet.Days[i].Vocaburaries[j].English == string.Empty & VocabularySet.Days[i].Vocaburaries[j].PartOfSpeech == string.Empty & VocabularySet.Days[i].Vocaburaries[j].Chinese == string.Empty)
                            {
                                VocabularySet.Days[i].Vocaburaries[j].English = r_English;
                                VocabularySet.Days[i].Vocaburaries[j].PartOfSpeech = r_PartOfSpeech;
                                VocabularySet.Days[i].Vocaburaries[j].Chinese = r_Chinese;

                                if (r_Save)
                                {
                                    StreamWriter notebook = new StreamWriter(VocabularySet.Days[i].PersonalListFilePath, true, System.Text.Encoding.Default/*Encoding.GetEncoding("big5")*/);
                                    notebook.WriteLine(r_English + "@" + r_PartOfSpeech + "@" + r_Chinese);
                                    notebook.Close();
                                }

                                return true;
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Is " + r_English + " a correct word? " + IsWordCorrect(r_English));
            }

            return false;
        }

        public bool IsWordCorrect(string word)
        {
            for (char i = 'a'; i <= 'z'; i++)
            {
                if (word[0] == i)
                {
                    StreamReader notebook = new StreamReader(SingleWordFolderPath + @"\" + Char.ToString(i) + ".txt", Encoding.GetEncoding("big5"));
                    string line = notebook.ReadLine();
                    while (line != null)
                    {
                        if (line == word)
                            return true;

                        line = notebook.ReadLine();
                    }
                    notebook.Close();

                    break;
                }
            }

            return false;
        }

        public void ListWord()
        {
            for (int i = 0; i < VocabularySet.Days.Length; i++)
            {
                if (VocabularySet.Days[i].Date == -1)
                {
                    break;
                }
                else 
                {
                    Console.WriteLine("-----Date: " + VocabularySet.Days[i].Date + "-----");
                    for (int j = 0; j < VocabularySet.Days[i].Vocaburaries.Length; j++)
                        if (VocabularySet.Days[i].Vocaburaries[j].English == string.Empty & VocabularySet.Days[i].Vocaburaries[j].PartOfSpeech == string.Empty & VocabularySet.Days[i].Vocaburaries[j].Chinese == string.Empty)
                            break;
                        else
                            Console.WriteLine("English: " + VocabularySet.Days[i].Vocaburaries[j].English
                                                    + "; PartOfSpeech: " + VocabularySet.Days[i].Vocaburaries[j].PartOfSpeech
                                                    + "; Chinese: " + VocabularySet.Days[i].Vocaburaries[j].Chinese);
                    Console.WriteLine("----------");
                }
            }
        }

        /// <summary>
        /// No explanation now
        /// </summary>
        public void TestWord(bool keyboard_answer, bool order_pronouncement, int vocabulary_date, int repeat_time)
        {
            try
            {
                for (int i = 0; i < VocabularySet.Days.Length; i++)
                {
                    if (vocabulary_date == VocabularySet.Days[i].Date & WhetherDateCorrect(vocabulary_date))
                    {
                        List<int> vocaburary_sort = SortWord(VocabularySet.Days[i], order_pronouncement);

                        int correct_number = 0;
                        int total_number = 0;
                        string result = "";
                        string record = "";
                        var synthesizer = new SpeechSynthesizer();
                        synthesizer.SetOutputToDefaultAudioDevice();
                        synthesizer.Speak("The test is starting.");

                        

                        for (int j = 0; j < vocaburary_sort.Count; j++)
                        {
                            for (int k = 0; k < repeat_time; k++)
                            {
                                System.Threading.Thread.Sleep(500);

                                synthesizer.Speak(VocabularySet.Days[i].Vocaburaries[vocaburary_sort[j]].English);
                            }

                            if (keyboard_answer)
                                while (!InputFlag)
                                    continue;

                            if (keyboard_answer)
                            {
                                //舊版
                                //-----
                                result += "題目: " + VocabularySet.Days[i].Vocaburaries[vocaburary_sort[j]].English + " 答案: " + InputString + " 結果: " + (InputString == VocabularySet.Days[i].Vocaburaries[vocaburary_sort[j]].English ? "正確" : "錯誤") + "\n";
                                //-----

                                List<string> words = new List<string>();
                                words.Add(VocabularySet.Days[i].Vocaburaries[vocaburary_sort[j]].English);
                                words.Add(InputString);
                                ResultList.Add(words);
                            }
                            else
                            {   //舊版
                                //-----
                                result += "題目: " + VocabularySet.Days[i].Vocaburaries[vocaburary_sort[j]].English + "\n";
                                //-----

                                List<string> words = new List<string>();
                                words.Add(VocabularySet.Days[i].Vocaburaries[vocaburary_sort[j]].English);
                                words.Add("On Paper");
                                ResultList.Add(words);
                            }

                            if (keyboard_answer)
                            {
                                total_number++;
                                if (InputString == VocabularySet.Days[i].Vocaburaries[vocaburary_sort[j]].English)
                                    correct_number++;
                            }
                                
                            if (keyboard_answer) //位置?
                                InputFlag = false;

                            if (!keyboard_answer)
                                MessageReturnEvent("Information", (j + 1), "");

                            if (keyboard_answer)
                                System.Threading.Thread.Sleep(1000);
                            else
                                System.Threading.Thread.Sleep(Convert.ToInt32(InputTime * 1000));

                            InputNumber++;
                        }

                        synthesizer.Speak("The test is ending.");

                        MessageReturnEvent("Result", -1, "");

                        if (keyboard_answer)
                        {
                            double grade = Math.Round((double)correct_number / (double)total_number * 100.0, 2); 

                            StreamWriter notebook_writer = new StreamWriter(VocabularySet.Days[i].PastGradeFilePath, true, Encoding.GetEncoding("big5"));
                            notebook_writer.WriteLine(PresentTime() + "@" + correct_number.ToString() + "/" + total_number.ToString() + "@" + grade.ToString() + "%");
                            notebook_writer.Close();

                            StreamReader notebook_reader = new StreamReader(VocabularySet.Days[i].PastGradeFilePath, Encoding.GetEncoding("big5"));
                            string line = notebook_reader.ReadLine();
                            int number = 0;
                            while (line != null)
                            {
                                number++;
                                string[] words = line.Split('@');
                                record += "時間:" + words[0] + " 結果:" + words[1] + " 分數:" + words[2] + "\n"; //舊版?
                                line = notebook_reader.ReadLine();
                            }
                            notebook_reader.Close();
                            MessageReturnEvent("Record", number, record);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception Occurred In ProcessWord. Message:" + Ex.Message);
            }
        }

        public bool DeleteWord(int date, string word)
        {
            bool word_is_found = false;

            for (int i = 0; i < VocabularySet.Days.Length; i++)
            {
                if (VocabularySet.Days[i].Date == date)
                {
                    for (int j = 0; j < VocabularySet.Days[i].Vocaburaries.Length; j++)
                    {
                        // 先確定是否有此單字
                        if (VocabularySet.Days[i].Vocaburaries[j].English == word)
                            word_is_found = true;
                        
                        if (word_is_found)
                        {
                            // 條件成立:用下一個單字覆蓋單前單字 // 條件不成立:代表已無下一個單字，將其的txt檔案覆寫
                            if (VocabularySet.Days[i].Vocaburaries[j].English != string.Empty & VocabularySet.Days[i].Vocaburaries[j].PartOfSpeech != string.Empty & VocabularySet.Days[i].Vocaburaries[j].Chinese != string.Empty)
                            {
                                VocabularySet.Days[i].Vocaburaries[j].English = VocabularySet.Days[i].Vocaburaries[j + 1].English;
                                VocabularySet.Days[i].Vocaburaries[j].PartOfSpeech = VocabularySet.Days[i].Vocaburaries[j + 1].PartOfSpeech;
                                VocabularySet.Days[i].Vocaburaries[j].Chinese = VocabularySet.Days[i].Vocaburaries[j + 1].Chinese;
                            }
                            else
                            {
                                StreamWriter notebook = new StreamWriter(VocabularySet.Days[i].PersonalListFilePath, false, System.Text.Encoding.Default/*Encoding.GetEncoding("big5")*/);
                                for (int k = 0; j < VocabularySet.Days[i].Vocaburaries.Length; k++)
                                    if (VocabularySet.Days[i].Vocaburaries[k].English != string.Empty & VocabularySet.Days[i].Vocaburaries[k].PartOfSpeech != string.Empty & VocabularySet.Days[i].Vocaburaries[k].Chinese != string.Empty)
                                        notebook.WriteLine(VocabularySet.Days[i].Vocaburaries[k].English + "@" + VocabularySet.Days[i].Vocaburaries[k].PartOfSpeech + "@" + VocabularySet.Days[i].Vocaburaries[k].Chinese);
                                    else
                                        break;
                                notebook.Close();

                                break;
                            }
                        }
                    }                   
                }
            }

            return word_is_found;
        }

        public void PronounceWord(string word)
        {
            var synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToDefaultAudioDevice();
            synthesizer.Speak(word);
        }

        public void SearchWord(string word)
        {
            // Search from WordAPI

            string website = "https://wordsapiv1.p.rapidapi.com/words/" + word;
            var client = new RestClient(website); 
            var request = new RestRequest();
            request.AddHeader("X-RapidAPI-Key", "2d9e1af5b2msh5723b89b205de04p187a04jsna64798002a38");
            request.AddHeader("X-RapidAPI-Host", "wordsapiv1.p.rapidapi.com");
            RestResponse response = client.Execute(request);

            if (response != null)
            {
                string[] words = response.Content.Split('"');
                bool flag = false;
                int number = 0;

                for (int i = 0; i < words.Length; i++)
                {

                    if (words[i].Length > 2)
                    {
                        if (words[i] == "definition")
                        {
                            flag = true;
                            number++;
                            continue;
                        }

                        if (flag)
                        {
                            MessageReturnEvent("Definition", number, words[i]);
                            flag = false;
                        }
                    }
                }
            }
        }

        private List<int> SortWord(VocaburariesOfOneDay daily_vocaburary, bool order_pronouncement)
        {
            if (order_pronouncement)
            {
                List<int> vocaburary_sort = new List<int>();

                for (int i = 0; i < daily_vocaburary.VocaburaryNumber(); i++)
                    vocaburary_sort.Add(i);

                return vocaburary_sort;
            }
            else
            {
                Random rand = new Random(Guid.NewGuid().GetHashCode());

                List<int> vocaburary_sort = new List<int>(Enumerable.Range(0, daily_vocaburary.VocaburaryNumber())); // ???

                vocaburary_sort = vocaburary_sort.OrderBy(num => rand.Next()).ToList<int>(); // ???

                return vocaburary_sort;
            }
        }

        public List<List<string>> FindGrade(int date, int page)
        {
            List<List<string>> past_grade = new List<List<string>>();
            for (int i = 0; i < 5; i++)
            {
                List<string> null_space = new List<string>();
                for (int j = 0; j < 2; j++)
                    null_space.Add("");
                past_grade.Add(null_space);
            }
           
            for (int i = 0; i < VocabularySet.Days.Length; i++)
            {
                if (date == VocabularySet.Days[i].Date)
                {
                    int read_number = 0;
                    int write_number = 0;

                    StreamReader notebook_reader = new StreamReader(VocabularySet.Days[i].PastGradeFilePath, Encoding.GetEncoding("big5"));

                    if (page == -1)
                    {


                    }
                    else if (page > 0)
                    {
                        string line = notebook_reader.ReadLine();
                        while (line != null)
                        {
                            if ((read_number >= (page - 1) * 5) & (read_number < page * 5))
                            {
                                string[] words = line.Split('@');
                                past_grade[write_number][0] = words[0];
                                past_grade[write_number][1] = words[1];
                                write_number++;
                            }

                            read_number++;
                            line = notebook_reader.ReadLine();
                        }
                    }

                    notebook_reader.Close();
                }
            }

            return past_grade;
        }
    }
}
