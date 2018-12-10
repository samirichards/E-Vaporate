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
    /// Interaction logic for GameTransaction.xaml
    /// </summary>
    public partial class GameTransaction : MahApps.Metro.Controls.MetroWindow
    {
        private User CurrentUser { get; set; }
        private Game CurrentGame { get; set; }
        public bool PurchaseSuccessful { get; set; }
        public GameTransaction(Game game, User user)
        {
            InitializeComponent();
            CurrentUser = user;
            CurrentGame = game;
            DataContext = game;
            Stk_AccFund.DataContext = user;
            Lbl_AfterTransactionResult.Content = user.AccountFunds - game.Price;
        }

        private void Btn_Accept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new EVaporateModel())
                {
                    User tempUser = context.Users.Where(u => u.UserID == CurrentUser.UserID).Single();
                    Game tempGame = context.Games.Where(g => g.GameID == CurrentGame.GameID).Single();

                    context.Users.Where(u => u.UserID == tempUser.UserID).Single().AccountFunds -= tempGame.Price;

                    GameOwnership ownership = new GameOwnership
                    {
                        GameID = tempGame.GameID,
                        UserID = tempUser.UserID
                    };
                    if (!context.GameOwnerships.Where(u=> u.UserID == CurrentUser.UserID).Select(g=> g.GameID).Contains(CurrentGame.GameID))
                    {
                        context.GameOwnerships.Add(ownership);
                        context.SaveChangesAsync();
                        MessageBox.Show("Purchase successful");
                        PurchaseSuccessful = true;
                        DialogResult = true;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Game already owned");
                        DialogResult = false;
                        Close();
                    }
                }
            }
            catch (Exception f)
            {
                MessageBox.Show("There was a problem with the transaction" + Environment.NewLine + f.Message);
                DialogResult = false;
                Close();
                throw;
            }
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
