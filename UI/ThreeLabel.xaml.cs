using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace UI
{
    /// <summary>
    /// ThreeLabel.xaml 的互動邏輯
    /// </summary>
    public partial class ThreeLabel : System.Windows.Controls.UserControl
    {
        #region Parameter
        public delegate void ThreeLabelHandler(string mode, string word);
        public event ThreeLabelHandler ThreeLabelHandleEvent;
        #endregion

        public ThreeLabel()
        {
            InitializeComponent();
        }

        #region Function
        public void ShowVocabularyInformation(string english, string part_of_speech, string chinese)
        {
            Button_English.Content = english;
            Label_PartOfSpeech.Content = part_of_speech;
            Label_Chinese.Content = chinese;
        }
        #endregion

        #region UI
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as System.Windows.Controls.MenuItem).Name)
            {
                case "MenuItem_Pronounce":
                    {
                        if (Button_English.Content.ToString() != System.String.Empty)
                            ThreeLabelHandleEvent("Pronounce", Button_English.Content.ToString());
                    }
                    break;

                case "MenuItem_Definition":
                    {
                        if (Button_English.Content.ToString() != System.String.Empty)
                            ThreeLabelHandleEvent("Definition", Button_English.Content.ToString());
                    }
                    break;

                case "MenuItem_Delete":
                    {
                        if (Button_English.Content.ToString() != System.String.Empty)
                            if (System.Windows.Forms.MessageBox.Show("是否刪除?", "訊息", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                                ThreeLabelHandleEvent("Delete", Button_English.Content.ToString());
                    }
                    break;

                default:
                    break;
            }
        }

        private void Button_English_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Button_English.Content.ToString() != System.String.Empty)
                {
                    ThreeLabelHandleEvent("Definition", Button_English.Content.ToString());
                    ThreeLabelHandleEvent("Pronounce", Button_English.Content.ToString());
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception Occurred In MouseDown Event Of Label. Message:" + Ex.Message);
            }
        }
        #endregion

        #region Reserve
        public void AddLastPageIcon()
        {
            BitmapImage next_page_icon = new BitmapImage();
            next_page_icon.BeginInit();
            next_page_icon.UriSource = new Uri(System.Environment.CurrentDirectory + @"\Appendix\Icon\StartAOI.ico", UriKind.RelativeOrAbsolute);
            next_page_icon.EndInit();

            System.Windows.Controls.Image next_page_image = new System.Windows.Controls.Image();
            next_page_image.Height = 15;
            next_page_image.Width = 15;
            next_page_image.Source = next_page_icon;

            System.Windows.Controls.StackPanel next_page_stackpanel = new System.Windows.Controls.StackPanel();
            next_page_stackpanel.Children.Add(next_page_image);

            System.Windows.Controls.Button next_page_button = new System.Windows.Controls.Button();
            next_page_button.Content = next_page_stackpanel;
            next_page_button.BorderThickness = new Thickness(0, 0, 0, 0);
            next_page_button.Style = (Style)FindResource(System.Windows.Controls.ToolBar.ButtonStyleKey);

            StackPanel_English.Children.Add(next_page_button);

            var temp = StackPanel_English.Children[0];
            StackPanel_English.Children.RemoveAt(0);
            StackPanel_English.Children.Insert(1, temp);

            StackPanel_English.ContextMenu = null;
        }

        public void AddNextPageIcon()
        {
            BitmapImage last_page_icon = new BitmapImage();
            last_page_icon.BeginInit();
            last_page_icon.UriSource = new Uri(System.Environment.CurrentDirectory + @"\Appendix\Icon\StopAOI.ico", UriKind.RelativeOrAbsolute);
            last_page_icon.EndInit();

            System.Windows.Controls.Image last_page_image = new System.Windows.Controls.Image();
            last_page_image.Height = 15;
            last_page_image.Width = 15;
            last_page_image.Source = last_page_icon;

            System.Windows.Controls.StackPanel LastPageStackpanel = new System.Windows.Controls.StackPanel();
            LastPageStackpanel.Children.Add(last_page_image);

            System.Windows.Controls.Button last_page_button = new System.Windows.Controls.Button();
            last_page_button.Content = LastPageStackpanel;
            last_page_button.BorderThickness = new Thickness(0, 0, 0, 0);
            last_page_button.Style = (Style)FindResource(System.Windows.Controls.ToolBar.ButtonStyleKey);

            StackPanel_Chinese.Children.Add(last_page_button);
        }
        #endregion
    }
}
