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

namespace Pospointe
{
    /// <summary>
    /// Interaction logic for FrmPermissionShow.xaml
    /// </summary>
    public partial class FrmPermissionShow : Window
    {
        public string reqpermssion { get; set; }
        public FrmPermissionShow()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnOverride_Click(object sender, RoutedEventArgs e)
        {
            FrmOverrideUsers over = new FrmOverrideUsers();
            over.requestedpermssion = reqpermssion;
           var result = over.ShowDialog();
            if (result == true)
            {
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}
