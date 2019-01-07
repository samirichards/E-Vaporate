using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Account.xaml
    /// </summary>
    public partial class Account : Page
    {
        public User CurrentUser { get; set; }
        public Account(User _user)
        {
            InitializeComponent();
            CurrentUser = _user;
            DataContext = CurrentUser;
        }

        private void Txt_InputFunds_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static readonly Regex _regex = new Regex("[^0-9.-]+");
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void Btn_AddFunds_Click(object sender, RoutedEventArgs e)
        {
            if (Txt_InputFunds.Text != string.Empty)
            {
                using (var context = new EVaporateModel())
                {
                    context.Users.Single(u => u.UserID == CurrentUser.UserID).AccountFunds += int.Parse(Txt_InputFunds.Text);
                    context.SaveChanges();
                    CurrentUser = context.Users.Single(u => u.UserID == CurrentUser.UserID);
                }
            }
            else
            {
                MessageBox.Show("Please input a value to add to your account funds");
            }
        }
    }
}
