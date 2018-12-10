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
        private User CurrentUser { get; set; }
        public StorePageItem(Game inputGame, User user)
        {
            InitializeComponent();
            this.DataContext = inputGame;
            CurrentGame = inputGame;
            CurrentUser = user;
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
                    if (context.GameOwnerships.Where(u=> u.UserID == CurrentUser.UserID).Select(g=> g.GameID).Contains(CurrentGame.GameID))
                    {
                        Dispatcher.Invoke((() =>
                        {
                            Btn_BuyGame.Content = "Owned";
                            Btn_BuyGame.IsEnabled = false;
                        }));

                    }
                }
                Dispatcher.Invoke((() =>
                {
                    Ite_CategoryDisplay.ItemsSource = categories;
                    Ite_CategoryDisplay.DataContext = categories;
                    Prog_ProgressRing.Visibility = Visibility.Hidden;
                }));
            });
        }

        private void Btn_BuyGame_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Windows.OfType<GameTransaction>().Count() == 0)
            {
                if ((CurrentUser.AccountFunds - CurrentGame.Price) < 0)
                {
                    MessageBox.Show("Insufficient funds" + Environment.NewLine + "You can add more funds to your account under the account section");
                }
                else
                {
                    var transaction = new GameTransaction(CurrentGame, CurrentUser);

                    if ((bool)transaction.ShowDialog())
                    {
                        Populate();
                    }
                    else
                    {
                        MessageBox.Show("Yeah nah that didn't work out");
                    }
                }
            }
        }
    }
}
