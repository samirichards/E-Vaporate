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
        public Store(User loggedInUser)
        {
            InitializeComponent();
            List<PublisherTitle> publisherTitles = new List<PublisherTitle>();
            using (var context = new EVaporateModel())
            {
                foreach (var publisher in context.Publishers)
                {
                    PublisherTitle titles = new PublisherTitle
                    {
                        DevName = context.Publishers.Single(p => p.PublisherID == publisher.PublisherID).DeveloperName
                    };
                    foreach (var item in context.Games.Where(u => u.Publisher == publisher.PublisherID))
                    {
                        PublisherGame game = new PublisherGame
                        {
                            GameID = item.GameID,
                            GameTitle = item.Title,
                            Thumbnail = item.Thumbnail
                        };
                        titles.Games.Add(game);
                    }
                    publisherTitles.Add(titles);
                }
            }
            Lst_DevList.ItemsSource = publisherTitles;
            Lst_DevList.DataContext = publisherTitles;
        }
    }

    public class PublisherTitle
    {
        public string DevName { get; set; }
        public List<PublisherGame> Games { get; set; } = new List<PublisherGame>();
    }
    public class PublisherGame
    {
        public int GameID { get; set; }
        public string GameTitle { get; set; }
        public byte[] Thumbnail { get; set; }
    }

}
