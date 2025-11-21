using Pospointe.Services;
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

namespace Pospointe.PosMenu
{
    /// <summary>
    /// Interaction logic for FrmMoreOptions.xaml
    /// </summary>
    public partial class FrmMoreOptions : Window
    {
        public string resp { get; set; }
        public FrmMoreOptions()
        {
            InitializeComponent();
        }

        private void BtnOpncashdr_Click(object sender, RoutedEventArgs e)
        {
            PrintService.OpenCashDrawer();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        

        private void BtnManualRefund_Click_1(object sender, RoutedEventArgs e)
        {
            this.resp = "manualreturn";
            this.DialogResult = true;
            this.Close();

            

        }
    }
}
