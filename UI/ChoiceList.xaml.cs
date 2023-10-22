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
using System.Windows.Shapes;

namespace UI
{
    /// <summary>
    /// ChoiceList.xaml 的互動邏輯
    /// </summary>
    public partial class ChoiceList : Window
    {
        // Variable
        // ---------------------------------------------------------------
        public object ControlLabel;
        // ---------------------------------------------------------------

        // Event
        // ------------------------------
        public delegate void ChoiceReturnHandler(string r_Choice, object r_Label);
        public event ChoiceReturnHandler ChoiceReturnEvent;
        // ------------------------------

        public ChoiceList()
        {
            InitializeComponent();
        }

        public ChoiceList(object r_Label)
        {
            InitializeComponent();

            ControlLabel = r_Label;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch ((sender as System.Windows.Controls.Button).Name)
                {
                    case "Btn_CreatNewText":
                        {
                            Hide();
                            ChoiceReturnEvent("CreatNewText", ControlLabel);
                            Close();
                        }
                        break;

                    case "Btn_OpenOddText":
                        {
                            Hide();
                            ChoiceReturnEvent("OpenOddText", ControlLabel);
                            Close();
                        }
                        break;

                    case "Btn_Cancel":
                        {
                            Close();
                        }
                        break;

                    default:
                        {
                            Console.WriteLine("Btn_Click Went Into Default.");
                        }
                        break;
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception Occurred When Button Clicked. Message:" + Ex.Message);
            }
        }
    }
}
