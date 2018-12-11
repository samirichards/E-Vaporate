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
    /// Interaction logic for Library.xaml
    /// </summary>
    public partial class Library : Page
    {
        public User LoggedInUser { get; set; }
        public Library(User user)
        {
            InitializeComponent();
            LoggedInUser = user;
            Refresh();
        }

        private void Refresh()
        {
            Lst_LibGames.UnselectAll();
            if (Frm_GamePage.Content != null)
            {
                ((LibraryGameItem)Frm_GamePage.Content).Dispose();
            }
            Frm_GamePage.Content = null;
            List<Game> games = new List<Game>();
            using (var context = new EVaporateModel())
            {
                var attachedUser =  context.Users.Attach(LoggedInUser);
                games.AddRange(context.Games.Where(g=> context.GameOwnerships.Where(u=> u.UserID == LoggedInUser.UserID).Select(p=> p.GameID).Contains(g.GameID)).ToList());
            }
            Lst_LibGames.ItemsSource = games;
        }

        private void Lst_LibGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Frm_GamePage.Content != null)
            {
                ((LibraryGameItem)Frm_GamePage.Content).Dispose();
            }
            Frm_GamePage.Content = new LibraryGameItem((Game)Lst_LibGames.SelectedItem);
            GC.Collect(4);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
    }
}
