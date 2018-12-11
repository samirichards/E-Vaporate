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
    /// Interaction logic for LibraryGameItem.xaml
    /// </summary>
    public partial class LibraryGameItem : UserControl, IDisposable
    {
        private Game CurrentGame { get; set; }
        public LibraryGameItem(Game game)
        {
            InitializeComponent();
            CurrentGame = game;
            DataContext = CurrentGame;
            Populate();
        }

        private Task Populate()
        {
            return Task.Run(() =>
            {
                List<Category> categories = new List<Category>();
                using (var context = new EVaporateModel())
                {
                    categories = context.CategoryAssignments.Where(g => g.Game.GameID == CurrentGame.GameID).Select(f => f.Category).ToList();
                }
                Dispatcher.Invoke((() =>
                {
                    Ite_CategoryDisplay.ItemsSource = categories;
                    Ite_CategoryDisplay.DataContext = categories;
                }));
            });
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
