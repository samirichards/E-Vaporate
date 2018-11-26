using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.Entity;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace E_Vaporate.Views
{
    /// <summary>
    /// Interaction logic for AccountVerification.xaml
    /// </summary>
    public partial class AccountVerification : MahApps.Metro.Controls.MetroWindow
    {
        public bool Expanded { get; set; }
        public AccountVerification()
        {
            InitializeComponent();
            Expanded = false;
        }

        private void Chk_Publisher_Checked(object sender, RoutedEventArgs e)
        {
            VerifyPubCode verify = new VerifyPubCode();
            if (verify.ShowDialog() == false)
            {
                MessageBox.Show("Publisher Code Invalid");
                Chk_Publisher.IsChecked = false;
            }
        }

        private void Btn_ExpandReg_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (Expanded == false)
            {
                btn.Content = "Less Options";

                DoubleAnimation anim = new DoubleAnimation
                {
                    From = 400,
                    To = 800,
                    Duration = TimeSpan.FromSeconds(0.2),
                    EasingFunction = new CubicEase()
                };
                BeginAnimation(WidthProperty, anim);
                Expanded = true;
            }
            else
            {
                btn.Content = "More Options";

                DoubleAnimation anim = new DoubleAnimation
                {
                    From = 800,
                    To = 400,
                    Duration = TimeSpan.FromSeconds(0.2),
                    EasingFunction = new CubicEase()
                };
                BeginAnimation(WidthProperty, anim);
                Expanded = false;
            }
        }

        private void Btn_BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            if (Expanded == true)
            {
                DoubleAnimation anim = new DoubleAnimation
                {
                    From = 800,
                    To = 400,
                    Duration = TimeSpan.FromSeconds(0.8),
                    EasingFunction = new CubicEase()
                };
                BeginAnimation(WidthProperty, anim);
                Expanded = false;
                Btn_ExpandReg.Content = "More Options";
            }
        }

        private void Btn_Register_Click(object sender, RoutedEventArgs e)
        {
            
            switch (ValidateReg())
            {
                case 2:
                    MessageBox.Show("Passwords dont match");
                    return;
                case 1:
                    MessageBox.Show("There are empty fields");
                    return;
                case 0:
                    break;
            }
            RegisterAccount();
        }

        private async Task RegisterAccount()
        {
            using (var context = new Model.EVaporateModel())
            {
                if (context.Users.Where(b => b.Username == Txt_RegUsername.Text).SingleOrDefault() != null)
                {
                    MessageBox.Show("That user already exists");
                    Txt_RegUsername.Text = string.Empty;
                }
                else
                {
                    Model.User user = new Model.User
                    {
                        Username = Txt_RegUsername.Text,
                        FirstName = Txt_FirstName.Text,
                        LastName = Txt_LastName.Text,
                        Email = Txt_Email.Text,
                        Postcode = Txt_Postcode.Text,
                        AddrLine1 = Txt_AddrLine1.Text,
                        AddrLine2 = Txt_AddrLine2.Text,
                        AddrLine3 = Txt_AddrLine3.Text,
                        HashedPassword = Classes.Utilities.GeneratePasswordSalt(Txt_RegPassword.Password, Txt_RegUsername.Text.ToLower()),
                        AccountFunds = 0
                    };
                    if (Chk_Publisher.IsChecked.Value)
                    {
                        Model.Publisher publisher = new Model.Publisher
                        {
                            PublisherID = user.UserID,
                            DeveloperName = user.Username
                        };
                        context.Publishers.Add(publisher);
                    }
                    try
                    {
                        context.Users.Add(user);
                        await context.SaveChangesAsync();
                        MessageBox.Show("Successfully signed up");
                        Txt_Username.Text = Txt_RegUsername.Text;
                        Txt_Password.Password = Txt_RegPassword.Password;
                    }
                    catch (Exception b)
                    {
                        MessageBox.Show("Could not create an account" + Environment.NewLine + b.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Checks to see if the required fields are filled in
        /// </summary>
        /// <returns>Int to represent the kind of error or if it passes</returns>
        private int ValidateReg()
        {
            if (Txt_RegPasswordConf.Password != Txt_RegPassword.Password)
            {
                return 2;
            }
            if (Txt_RegUsername.Text == string.Empty || Txt_RegPassword.Password == string.Empty || Txt_RegPasswordConf.Password == string.Empty || Txt_FirstName.Text == string.Empty || Txt_LastName.Text == string.Empty || Txt_Email.Text == string.Empty)
            {
                return 1;
            }
            else return 0;
        }

        private void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            Prog_ProgressRing.IsActive = true;
            Login();
        }

        private async Task Login()
        {
            Model.User temp = await Classes.Utilities.Test(Txt_Username.Text, Txt_Password.Password);
            if (temp != null)
            {
                Main main = new Main(temp);
                main.Show();
                Prog_ProgressRing.IsActive = true;
                Hide();
            }
            else
            {
                MessageBox.Show("User not found");
                Prog_ProgressRing.IsActive = false;
            }
        }
    }
}
