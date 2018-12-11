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
    /// Interaction logic for Store.xaml
    /// </summary>
    public partial class Store : Page
    {
        public User CurrentUser { get; set; }
        public Store(User loggedInUser)
        {
            InitializeComponent();
            CurrentUser = loggedInUser;
            Refresh();
        }

        public void GetStore()
        {
            List<PublisherTitle> publisherTitles = new List<PublisherTitle>();
            using (var context = new EVaporateModel())
            {
                foreach (var publisher in context.Publishers)
                {
                    PublisherTitle titles = new PublisherTitle
                    {
                        DevName = context.Publishers.Single(p => p.PublisherID == publisher.PublisherID).DeveloperName
                    };
                    foreach (var item in context.Games.Where(u => u.Publisher == publisher.PublisherID && u.Available == true))
                    {
                        titles.Games.Add(item);
                    }
                    publisherTitles.Add(titles);
                }
            }
            Dispatcher.Invoke((Action)(() =>
            {
                Lst_DevList.ItemsSource = publisherTitles;
                Lst_DevList.DataContext = publisherTitles;
                Prog_ProgressRing.Visibility = Visibility.Hidden;
            }));
        }

        private void Btn_RefreshStore_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            CloseStoreItem();
        }

        private void Lst_GameList_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Frm_GameDisplay.Content != null)
            {
                ((StorePageItem)Frm_GameDisplay.Content).Dispose();
                Frm_GameDisplay.Content = null;
            }
            Frm_GameDisplay.Content = new StorePageItem(((Game)((ListView)sender).SelectedItem), CurrentUser);
            Tran_StoreTransitioner.SelectedIndex = 1;
        }

        private Task Refresh()
        {
            Prog_ProgressRing.Visibility = Visibility.Visible;
            return Task.Run(() =>
            {
                GetStore();
            });
        }

        public void CloseStoreItem()
        {
            Tran_StoreTransitioner.SelectedIndex = 0;
            //((StorePageItem)Frm_GameDisplay.Content).Dispose();
            //Frm_GameDisplay.Content = null;
        }
    }

    public class PublisherTitle
    {
        public string DevName { get; set; }
        public List<Game> Games { get; set; } = new List<Game>();
    }
}
