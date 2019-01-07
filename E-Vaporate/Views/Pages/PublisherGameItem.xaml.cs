using System;
using System.Collections.Generic;
using System.Globalization;
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
using OxyPlot;
using OxyPlot.Wpf;

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
            //Sets the datacontext to the game given to it so that the games information can be displayed with data binding
            GameItem = game;
            DataContext = GameItem;

            InitializeComponent();
            try
            {
                //Disposes of the connection to the database once the operation completes
                using (var context = new EVaporateModel())
                {
                    //Sets the item source of the list to be the contents of the Categories table (being all the categories)
                    Lst_CategoryAssignment.ItemsSource = context.Categories.AsParallel();
                    List<Category> categories = new List<Category>();
                    //Add each category to the list above where this game's ID is in the category assignment table
                    foreach (var item in context.CategoryAssignments.Where(g => g.GameID == GameItem.GameID))
                    {
                        categories.Add(context.Categories.Where(c => c.CategoryID == item.CategoryID).Single());
                    }
                    //Select the items in the listview which are in this list also
                    foreach (var item in categories)
                    {
                        Lst_CategoryAssignment.SelectedItems.Add(item);
                    }
                }
            }
            catch (Exception)
            { }
            RefreshStats();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private async void Btn_SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            //Show loading indicator on the foreground while the changes are saved
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
                    //Set all of the data for the current game to what is written on this page
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
            //Display error message in the case that there is a problem
            catch (Exception a)
            {
                MessageBox.Show("There was an issue saving your changes" + Environment.NewLine + a.Message);
            }
            Prog_SaveProgress.IsActive = false;
            Btn_SaveChanges.IsEnabled = true;
            Grd_Background.Visibility = Visibility.Hidden;
        }

        //Validation on price tetbox, only numbers and periods can be entered
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("[^0-9.]+$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private Task RefreshStats()
        {
            //Async refresh of the game stats
            return Task.Run(() =>
            {
                //Set showprogress to true on the main thread
                Dispatcher.Invoke(() =>
                {
                    ((Main)Application.Current.MainWindow).ShowProgress(true);
                });

                List<DataPoint> temp = new List<DataPoint>();

                //For each day in the current month add a new datapoint to the temp list
                using (var context = new EVaporateModel())
                {
                    for (int i = 1; i < DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) + 1; i++)
                    {
                        temp.Add(new DataPoint(i, context.GameOwnerships.Where(t=> t.TransactionDate.Day == i && t.GameID == GameItem.GameID).Count()));
                    }
                    //Show the number of sales
                    Dispatcher.Invoke(() => Lbl_TotalOwnerships.Content = "Total copies of " + GameItem.Title + " sold: " + context.GameOwnerships.Where(t => t.GameID == GameItem.GameID).Count());
                }
                //Set datacontext to the datapoint list as well as show the current month
                Dispatcher.Invoke(() =>
                {
                    Grph_Stats_LineSeries.Title = "Sales per day during the month of " + DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);
                    Grph_Stats_LineSeries.ItemsSource = temp;
                    Grph_Stats_LineSeries.Color = (Color)ColorConverter.ConvertFromString(FindResource("SecondaryAccentBrush").ToString());
                    ((Main)Application.Current.MainWindow).ShowProgress(false);
                });
            });
        }
    }
}
