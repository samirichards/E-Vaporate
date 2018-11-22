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
            if (!ValidateReg())
            {
                MessageBox.Show("Please fill in all required boxes");
                return;
            }
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
                        HashedPassword = GeneratePasswordSalt(Txt_RegPassword.Password, Txt_RegUsername.Text),
                        AccountFunds = 0
                    };
                    try
                    {
                        context.Users.Add(user);
                        context.SaveChanges();
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

        private bool ValidateReg()
        {
            if (Txt_RegUsername.Text == string.Empty || Txt_RegPassword.Password == string.Empty || Txt_RegPasswordConf.Password == string.Empty || Txt_FirstName.Text == string.Empty || Txt_LastName.Text == string.Empty || Txt_Email.Text == string.Empty)
            {
                return false;
            }
            else return true;
        }

        protected byte[] GeneratePasswordSalt(string password, string _salt)
        {
            byte[] plainText = password.ToCharArray().Select(c => (byte)c).ToArray();
            byte[] salt = _salt.ToCharArray().Select(c => (byte)c).ToArray();
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }
    }
}
