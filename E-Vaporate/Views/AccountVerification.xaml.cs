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
using E_Vaporate.Model;

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
                case 3:
                    MessageBox.Show("Invalid Email format");
                    return;
                case 2:
                    MessageBox.Show("Passwords dont match");
                    return;
                case 1:
                    MessageBox.Show("There are empty fields");
                    return;
                case 0:
                    break;
            }
            Prog_RegisterProgressBar.Visibility = Visibility.Visible;
            LockRegInputs();
#pragma warning disable CS4014
            RegisterAccount();
#pragma warning restore CS4014
        }

        private async Task RegisterAccount()
        {
            using (var context = new EVaporateModel())
            {
                if (context.Users.Where(b => b.Username == Txt_RegUsername.Text).SingleOrDefault() != null)
                {
                    MessageBox.Show("That user already exists");
                    Txt_RegUsername.Text = string.Empty;
                    UnlockRegInputs();
                }
                else
                {
                    User user = new User
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
                        Publisher publisher = new Publisher
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
                        UnlockRegInputs();
                        Prog_RegisterProgressBar.Visibility = Visibility.Hidden;
                        Txt_Username.Text = Txt_RegUsername.Text;
                        Txt_Password.Password = Txt_RegPassword.Password;
                    }
                    catch (Exception b)
                    {
                        MessageBox.Show("Could not create an account" + Environment.NewLine + b.Message);
                        UnlockRegInputs();
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
            return IsEmailValid(Txt_Email.Text);
        }

        private void LockRegInputs()
        {
            Txt_FirstName.IsEnabled = false;
            Txt_LastName.IsEnabled = false;
            Txt_RegPassword.IsEnabled = false;
            Txt_RegPasswordConf.IsEnabled = false;
            Txt_RegUsername.IsEnabled = false;
            Txt_Email.IsEnabled = false;
            Txt_Postcode.IsEnabled = false;
            Txt_AddrLine1.IsEnabled = false;
            Txt_AddrLine2.IsEnabled = false;
            Txt_AddrLine3.IsEnabled = false;
            Btn_BackToLogin.IsEnabled = false;
            Btn_Register.IsEnabled = false;
            Chk_Publisher.IsEnabled = false;
        }

        private void UnlockRegInputs()
        {
            Txt_FirstName.IsEnabled = true;
            Txt_LastName.IsEnabled = true;
            Txt_RegPassword.IsEnabled = true;
            Txt_RegPasswordConf.IsEnabled = true;
            Txt_RegUsername.IsEnabled = true;
            Txt_Email.IsEnabled = true;
            Txt_Postcode.IsEnabled = true;
            Txt_AddrLine1.IsEnabled = true;
            Txt_AddrLine2.IsEnabled = true;
            Txt_AddrLine3.IsEnabled = true;
            Btn_BackToLogin.IsEnabled = true;
            Btn_Register.IsEnabled = true;
            Chk_Publisher.IsEnabled = true;
        }

        private async void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            LoadingVisibility(true);
            ((Button)sender).IsEnabled = false;
            Login();
        }

        private async void Login()
        {
            User temp = Classes.Utilities.GetUser(Txt_Username.Text, Txt_Password.Password);
            if (temp != null)
            {
                Main main = new Main(temp);
                main.Show();
                LoadingVisibility(true);
                Hide();
            }
            else
            {
                MessageBox.Show("User not found");
                Btn_Login.IsEnabled = true;
                LoadingVisibility(false);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        private void LoadingVisibility(bool visability)
        {
            if (visability)
            {
                Grd_ProgBackdrop.Visibility = Visibility.Visible;
                Grd_ProgBackdrop.Opacity = 0;
                DoubleAnimation anim = new DoubleAnimation
                {
                    From = 0,
                    To = 0.52,
                    EasingFunction = new QuadraticEase(),
                    Duration = TimeSpan.FromSeconds(0.3)
                };
                Grd_ProgBackdrop.BeginAnimation(OpacityProperty, anim);
                Prog_ProgressRing.Visibility = Visibility.Visible;
            }
            else
            {
                Prog_ProgressRing.Visibility = Visibility.Visible;
                Grd_ProgBackdrop.Visibility = Visibility.Hidden;
            }
        }

        public int IsEmailValid(string emailaddress)
        {
            try
            {
                System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(emailaddress);

                return 1;
            }
            catch (FormatException)
            {
                return 0;
            }
        }
    }
}
