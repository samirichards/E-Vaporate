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
            //Temp list of custom class
            List<PublisherTitle> publisherTitles = new List<PublisherTitle>();
            using (var context = new EVaporateModel())
            {
                //Just add every publisher with all of their games
                //Adds publisher even if they have no games published
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
                ((Main)Application.Current.MainWindow).ShowProgress(false);
            }));
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            CloseStoreItem();
        }

        //When there is a double click on the list, show the selected game
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

        public Task Refresh()
        {
            //Asynchronously run GetStore()
            ((Main)Application.Current.MainWindow).ShowProgress(true);
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
        //Class to show publisher name and list of games 
        public string DevName { get; set; }
        public List<Game> Games { get; set; } = new List<Game>();
    }
}
