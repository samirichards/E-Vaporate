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

            SqlConnection conn = new SqlConnection
            {
                ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ServerDB"].ConnectionString
            };
            SqlCommand comm = new SqlCommand
            {
                CommandText = "SELECT * FROM [User] WHERE Username=@uname",
                Connection = conn
            };

            comm.Parameters.AddWithValue("@uname", Txt_RegUsername.Text);
            using (conn)
            {
                conn.Open();
                if (comm.ExecuteNonQuery() != -1)
                {
                    MessageBox.Show("That user already exists");
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
    }
}
