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

namespace E_Vaporate.Views
{
    /// <summary>
    /// Interaction logic for AccountVerification.xaml
    /// </summary>
    public partial class AccountVerification : MahApps.Metro.Controls.MetroWindow
    {
        public AccountVerification()
        {
            InitializeComponent();
        }

        private void Expandy_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation
            {
                From = 400,
                To = 800,
                Duration = TimeSpan.FromSeconds(0.8),
                EasingFunction = new CubicEase()
            };
            BeginAnimation(WidthProperty, anim);
        }

        private void Btn_BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            Width = 400;
        }

        private void Closy_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation
            {
                From = 800,
                To = 400,
                Duration = TimeSpan.FromSeconds(0.8),
                EasingFunction = new CubicEase()
            };
            BeginAnimation(WidthProperty, anim);
        }

        private void Chk_Publisher_Checked(object sender, RoutedEventArgs e)
        {
            VerifyPubCode verify = new VerifyPubCode();
            verify.Show();
        }
    }
}
