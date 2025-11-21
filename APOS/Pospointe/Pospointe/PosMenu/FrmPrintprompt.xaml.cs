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
    /// Interaction logic for FrmPrintprompt.xaml
    /// </summary>
    public partial class FrmPrintprompt : Window
    {
        public FrmPrintprompt()
        {
            InitializeComponent();
        }

        private void BtnNone_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            bool isPrinted;
            PrintService.PrinterReceipt(out isPrinted);

            if (isPrinted)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("Printing failed. Please check the printer and try again.", "Printing Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void BtnSms_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSmsPrint_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
