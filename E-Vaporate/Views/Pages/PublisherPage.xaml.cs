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
using System.Windows.Media.Animation;

namespace E_Vaporate.Views.Pages
{
    /// <summary>
    /// Interaction logic for PublisherPage.xaml
    /// </summary>
    public partial class PublisherPage : Page
    {
        public Model.User LoggedInUser { get; set; }
        public PublisherPage(Model.User user)
        {
            InitializeComponent();
            LoggedInUser = user;
            Refresh();
        }

        private void Btn_AddNewGame_Click(object sender, RoutedEventArgs e)
        {
            if (!Application.Current.Windows.OfType<MahApps.Metro.Controls.MetroWindow>().Any(w=> w.GetType().Equals(typeof(UploadGame))))
            {
                UploadGame upload = new UploadGame(LoggedInUser);
                upload.Show();
            }
        }

        private void Refresh()
        {
            List<Model.Game> games = new List<Model.Game>();
            using (var context = new Model.EVaporateModel())
            {
                games.AddRange(context.Games.Where(p => p.Publisher == LoggedInUser.UserID).ToList());
            }
            Lst_PublishedGames.ItemsSource = games;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
    }
}
