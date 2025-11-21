using Newtonsoft.Json;
using Pospointe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace Pospointe.MainMenu
{
    /// <summary>
    /// Interaction logic for FrmShiftReport.xaml
    /// </summary>
    public partial class FrmShiftReport : Window
    {
        public FrmShiftReport()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                using (var context = new PosDb1Context())
                {
                    var users = context.TblUsers
                        .Where(u => u.UserStatus == "OK")                        
                        .ToList();

                    UserDataGrid.ItemsSource = users;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}", "Database Error");
            }
        }




        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void BtnGetReport_Click(object sender, RoutedEventArgs e)
        {
            
        }

        

    }
}
