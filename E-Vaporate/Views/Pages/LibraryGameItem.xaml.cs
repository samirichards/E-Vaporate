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
            try
            {
                Populate();
            }
            catch (Exception)
            {
                MessageBox.Show("There was a problem displaying categories");
            }
        }

        private Task Populate()
        {
            return Task.Run(() =>
            {
                try
                {
                    List<Category> categories = new List<Category>();
                    using (var context = new EVaporateModel())
                    {
                        context.Games.Attach(CurrentGame);
                        categories = context.Categories.Where(c => c.CategoryAssignments.Select(g => g.GameID).Contains(CurrentGame.GameID)).ToList();
                    }
                    Dispatcher.Invoke((() =>
                    {
                        Ite_CategoryDisplay.ItemsSource = categories;
                        Ite_CategoryDisplay.DataContext = categories;
                    }));
                }
                catch (Exception)
                { }
            });
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
