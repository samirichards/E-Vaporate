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
using System.Media;
using E_Vaporate.Model;
using Microsoft.Win32;

namespace E_Vaporate.Views
{
    /// <summary>
    /// Interaction logic for UploadGame.xaml
    /// </summary>
    public partial class UploadGame : MahApps.Metro.Controls.MetroWindow
    {
        public User LoggedInUser { get; set; }
        public Game game = new Game();
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

        private void Btn_UploadHeader_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Upload Header Image",
                Filter = "Image files(*.png; *.jpeg; *.jpg)| *.png; *.jpeg; *.jpg",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Multiselect = false
            };
            dialog.ShowDialog();

            try
            {
                if (dialog.OpenFile() != null)
                {
                    game.HeaderImage = System.IO.File.ReadAllBytes(dialog.FileName);
                }
            }
            catch (Exception) { };
            var image = new BitmapImage();
            using (var ms = new System.IO.MemoryStream(game.HeaderImage))
            {
                ms.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = ms;
                image.EndInit();
            }
            Img_HeaderPreview.Source = image;
        }

        private void Btn_UploadThumbnail_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Upload Thumbnail Image",
                Filter = "Image files(*.png; *.jpeg; *.jpg)| *.png; *.jpeg; *.jpg",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Multiselect = false
            };
            dialog.ShowDialog();

            if (dialog.OpenFile() != null)
            {
                game.Thumbnail = System.IO.File.ReadAllBytes(dialog.FileName);
            }

            var image = new BitmapImage();
            using (var ms = new System.IO.MemoryStream(game.Thumbnail))
            {
                ms.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = ms;
                image.EndInit();
            }
            Img_ThumbnailPreview.Source = image;
        }

        private void Btn_PublishGame_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateUpload())
            {
                MessageBox.Show("There are empty fields still");
                return;
            }
            else
            {
                game.Description = Txt_GameDescription.Text;
                game.Directory = Txt_GameUrl.Text;
                game.Available = (bool)Chk_IsAvailable.IsChecked;
                game.Price = double.Parse(Txt_GamePrice.Text);
                game.Title = Txt_GameName.Text;
                game.Publisher = LoggedInUser.UserID;
            }
            Prog_UploadProg.IsActive = true;
            try
            {
                using (var context = new EVaporateModel())
                {
                    context.Games.Add(game);
                    foreach (var item in Lst_CategoryAssignment.SelectedItems)
                    {
                        CategoryAssignment assignment = new CategoryAssignment
                        {
                            GameID = game.GameID,
                            CategoryID = (item as Category).CategoryID
                        };
                        context.CategoryAssignments.Add(assignment);
                    }
                    context.SaveChanges();
                    MessageBox.Show("Game uploaded successfully");
                }
            }
            catch (Exception b)
            {
                MessageBox.Show("There was a problem uploading your game" + Environment.NewLine + b.Message);
            }
            Prog_UploadProg.IsActive = false;
            Close();
        }

        private bool ValidateUpload()
        {
            if (Txt_GameName.Text == string.Empty)
            {
                return false;
            }
            if (Txt_GamePrice.Text == string.Empty)
            {
                return false;
            }
            if (Txt_GameUrl.Text == string.Empty)
            {
                return false;
            }
            if (Txt_GameDescription.Text == string.Empty)
            {
                return false;
            }
            return true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("[^0-9.]+$");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}