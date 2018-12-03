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
    /// Interaction logic for PublisherGameItem.xaml
    /// </summary>
    public partial class PublisherGameItem : UserControl, IDisposable
    {
        public Game GameItem { get; set; }
        public PublisherGameItem(Game game)
        {
            GameItem = game;
            DataContext = GameItem;
            InitializeComponent();
            var context = new EVaporateModel();
            Lst_CategoryAssignment.ItemsSource = context.Categories.AsParallel();
            List<Category> categories = new List<Category>();
            try
            {
                foreach (var item in context.CategoryAssignments.Where(g => g.GameID == GameItem.GameID))
                {
                    categories.Add(context.Categories.Where(c => c.CategoryID == item.CategoryID).Single());
                }
                foreach (var item in categories)
                {
                    Lst_CategoryAssignment.SelectedItems.Add(item);
                }
            }
            catch (Exception) { }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private async void Btn_SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).IsEnabled = false;
            Prog_SaveProgress.IsActive = true;
            Grd_Background.Visibility = Visibility.Visible;
            await SaveChanges();
        }

        private async Task SaveChanges()
        {
            try
            {
                using (var context = new EVaporateModel())
                {
                    context.Games.Where(g => g.GameID == GameItem.GameID).Single().Description = GameItem.Description;
                    context.Games.Where(g => g.GameID == GameItem.GameID).Single().Directory = GameItem.Directory;
                    context.Games.Where(g => g.GameID == GameItem.GameID).Single().Available = GameItem.Available;
                    //context.Games.Where(g => g.GameID == GameItem.GameID).Single().Categories = GameItem.Categories;
                    context.Games.Where(g => g.GameID == GameItem.GameID).Single().HeaderImage = GameItem.HeaderImage;
                    context.Games.Where(g => g.GameID == GameItem.GameID).Single().Thumbnail = GameItem.Thumbnail;
                    context.Games.Where(g => g.GameID == GameItem.GameID).Single().Title = GameItem.Title;
                    context.Games.Where(g => g.GameID == GameItem.GameID).Single().Price = GameItem.Price;
                    await context.SaveChangesAsync();
                    MessageBox.Show("Changes saved sucessfully");
                }
            }
            catch (Exception a)
            {
                MessageBox.Show("There was an issue saving your changes" + Environment.NewLine + a.Message);
            }
            Prog_SaveProgress.IsActive = false;
            Btn_SaveChanges.IsEnabled = true;
            Grd_Background.Visibility = Visibility.Hidden;
        }
    }
}
