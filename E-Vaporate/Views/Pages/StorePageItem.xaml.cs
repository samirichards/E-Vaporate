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
    /// Interaction logic for StorePageItem.xaml
    /// </summary>
    public partial class StorePageItem : UserControl, IDisposable
    {
        private Game CurrentGame { get; set; }
        public StorePageItem(Game inputGame)
        {
            InitializeComponent();
            this.DataContext = inputGame;
            CurrentGame = inputGame;
            Populate();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private Task Populate()
        {
            Prog_ProgressRing.Visibility = Visibility.Visible;
            return Task.Run(() =>
            {
                List<Category> categories = new List<Category>();
                using (var context = new EVaporateModel())
                {
                    categories = context.Categories.Where(c => c.CategoryAssignments.Where(g => g.GameID == CurrentGame.GameID).Select(b => b.CategoryID).Contains(c.CategoryID)).ToList();
                }
                Dispatcher.Invoke((Action)(() =>
                {
                    Ite_CategoryDisplay.ItemsSource = categories;
                    Ite_CategoryDisplay.DataContext = categories;
                    Prog_ProgressRing.Visibility = Visibility.Hidden;
                }));
            });
        }
    }
}
