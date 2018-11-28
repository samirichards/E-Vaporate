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
using System.Data.SqlClient;
using System.Configuration;
using E_Vaporate.Model;

namespace E_Vaporate.Views
{
    /// <summary>
    /// Interaction logic for VerifyPubCode.xaml
    /// </summary>
    public partial class VerifyPubCode : MahApps.Metro.Controls.MetroWindow
    {
        public VerifyPubCode()
        {
            InitializeComponent();
            GetWindow(this).Closing += (s, e) =>
            {
                GetWindow(this).DialogResult = CodeValid;
            };
            Btn_EnterPublisherCode.Click += (s, e) =>
            {
                GetWindow(this).DialogResult = CodeValid;
            };
        }

        public bool CodeValid
        {
            get
            {
                SqlConnection conn = new SqlConnection
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["ServerDB"].ConnectionString
                };
                using (conn)
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand
                    {
                        CommandText = "SELECT Code FROM PublisherCode WHERE Code=@code"
                    };
                    command.Parameters.AddWithValue("Code", Txt_PublisherCode.Text);
                    command.Connection = conn;
                    if ((string)command.ExecuteScalar() == Txt_PublisherCode.Text)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
    }
}
