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
using E_Vaporate.Model;

namespace E_Vaporate.Views.Pages
{
    /// <summary>
    /// Interaction logic for PublisherGameItem.xaml
    /// </summary>
    public partial class PublisherGameItem : UserControl, IDisposable
    {
        public Game GameItem { get; set; }
        public PublisherGameItem(Game game)
        {
            GameItem = game;
            DataContext = GameItem;
            InitializeComponent();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
