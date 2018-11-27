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

namespace E_Vaporate.Views
{
    /// <summary>
    /// Interaction logic for UploadGame.xaml
    /// </summary>
    public partial class UploadGame : MahApps.Metro.Controls.MetroWindow
    {
        public Model.User LoggedInUser { get; set; }
        public UploadGame(Model.User user)
        {
            LoggedInUser = user;
            InitializeComponent();
        }
    }
}
