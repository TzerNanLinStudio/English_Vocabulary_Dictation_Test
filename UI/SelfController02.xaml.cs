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
    /// TwoLabel.xaml 的互動邏輯
    /// </summary>
    public partial class SelfController02 : System.Windows.Controls.UserControl
    {
        #region Parameter
        public delegate void Handler(string mode, string word);
        public event Handler HandleEvent;
        #endregion

        public SelfController02()
        {
            InitializeComponent();
        }

        #region Function
        public void ShowInformation(string button_one_content, string label_two_content)
        {
            Button_One.Content = button_one_content;
            Label_Two.Content = label_two_content;
        }
        #endregion

        #region UI
        private void Button_One_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Button_One.Content.ToString() != System.String.Empty)
                {
                    HandleEvent("Definition_From_SelfController02", Button_One.Content.ToString());
                    HandleEvent("Pronounce", Button_One.Content.ToString());
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception Occurred In the click event Of the button. Message:" + Ex.Message);
            }
        }
        #endregion
    }
}
