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
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using E_Vaporate.Model;
using System.Windows.Media.Animation;

namespace E_Vaporate.Views
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : MetroWindow
    {
        public User LoggedInUser { get; set; }

        public Main(User _user)
        {
            LoggedInUser = _user;
            InitializeComponent();
            List<NavMenuButton> MainNavList = new List<NavMenuButton>
            {
                new NavMenuButton { ButtonText = "Store", LinkedPage = new Pages.Store(LoggedInUser) },
                new NavMenuButton { ButtonText = "Library", LinkedPage = new Pages.Library(LoggedInUser) },
                new NavMenuButton { ButtonText = "Account", LinkedPage = new Pages.Account() },
                new NavMenuButton { ButtonText = "Settings", LinkedPage = new Pages.Settings() }
            };
            using (var context = new EVaporateModel())
            {
                if (Classes.Utilities.IsPublisher(LoggedInUser))
                {
                    MainNavList.Add(new NavMenuButton { ButtonText = "Publisher", LinkedPage = new Pages.PublisherPage(LoggedInUser) });
                }
            }
            LstView_MainNav.ItemsSource = MainNavList;

            List<Page> pages = new List<Page>()
            {
                new Pages.Store(LoggedInUser),
                new Pages.Library(LoggedInUser),
                new Pages.Account(),
                new Pages.Settings(),
                new Pages.PublisherPage(LoggedInUser)
            };

            LstView_MainNav.SelectedIndex = 0;
        }
        /*
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
        */

        private void LstView_MainNav_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavMenuButton menuButton = (NavMenuButton)LstView_MainNav.SelectedItem;
            Frm_ContentArea.Navigate(menuButton.LinkedPage);
            Lbl_ToolbarTitle.Content = menuButton.ButtonText;
        }

        private void Btn_Logout_Click(object sender, RoutedEventArgs e)
        {
            AccountVerification verification = new AccountVerification();
            verification.Show();
            Close();
        }

        private void Btn_Action_Click(object sender, RoutedEventArgs e)
        {
            if (Frm_ContentArea.Content.GetType() == typeof(Pages.Library))
            {
                ((Pages.Library)Frm_ContentArea.Content).Refresh();
            }
            if (Frm_ContentArea.Content.GetType() == typeof(Pages.PublisherPage))
            {
                ((Pages.PublisherPage)Frm_ContentArea.Content).Refresh();
            }
            if (Frm_ContentArea.Content.GetType() == typeof(Pages.Store))
            {
                ((Pages.Store)Frm_ContentArea.Content).Refresh();
            }
        }
    }

    public class NavMenuButton
    {
        public string ButtonText { get; set; }
        public Page LinkedPage { get; set; }
    }
}
