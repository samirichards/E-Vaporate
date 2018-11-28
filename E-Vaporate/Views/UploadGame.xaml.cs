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
using E_Vaporate.Model;

namespace E_Vaporate.Views
{
    /// <summary>
    /// Interaction logic for UploadGame.xaml
    /// </summary>
    public partial class UploadGame : MahApps.Metro.Controls.MetroWindow
    {
        public User LoggedInUser { get; set; }
        public UploadGame(User user)
        {
            LoggedInUser = user;
            InitializeComponent();
            Lbl_PublisherName.Content = "Publisher Name: " + LoggedInUser.Username.ToString();
            PopulateCategories();
        }

        private void PopulateCategories()
        {
            using (var context = new EVaporateModel())
            {
                List<Category> categories = context.Categories.Take(context.Categories.Count()).ToList<Model.Category>();
                Lst_CategoryAssignment.ItemsSource = categories;
                Lst_CategoryAssignment.DataContext = categories;
            }
        }
    }
}