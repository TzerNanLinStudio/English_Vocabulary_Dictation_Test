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
    public partial class Controller03 : System.Windows.Controls.UserControl
    {
        #region Parameter

        #endregion

        #region Constructor
        public Controller03()
        {
            InitializeComponent();
        }
        #endregion

        #region Function
        public void ShowInformation(string label_one_content, string label_two_content)
        {
            Label_One.Content = label_one_content;
            Label_Two.Content = label_two_content;
        }
        #endregion

        #region UI

        #endregion
    }
}
