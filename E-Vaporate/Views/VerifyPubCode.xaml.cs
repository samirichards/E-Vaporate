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
using System.Data.SqlClient;
using System.Configuration;

namespace E_Vaporate.Views
{
    /// <summary>
    /// Interaction logic for VerifyPubCode.xaml
    /// </summary>
    public partial class VerifyPubCode : MahApps.Metro.Controls.MetroWindow
    {
        public VerifyPubCode()
        {
            InitializeComponent();
            Prog_Publisher.Visibility = Visibility.Hidden;
            Btn_EnterPublisherCode.Click += (s, e) =>
            {

            };
        }
    }
}
