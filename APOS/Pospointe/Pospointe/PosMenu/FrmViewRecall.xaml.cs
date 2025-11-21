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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Pospointe.LocalData;
using Pospointe.Trans_Api;

namespace Pospointe.PosMenu
{
    /// <summary>
    /// Interaction logic for FrmViewRecall.xaml
    /// </summary>
    public partial class FrmViewRecall : UserControl
    {
        public event EventHandler UserControlHidden;
        public FrmViewRecall()
        {
            InitializeComponent();
        }

        public void SetData(TransData.Root data)
        {
            if (data != null)
            {
                TxtInvoiceID.Text = data.transmain.invoiceId.ToString();
                TxtDateTime.Text = data.transmain.saleDateTime.ToString("g");
                TxtPaidMethod.Text = data.transmain.paidby;
                TxtCashierID.Text = data.transmain.cashierId;
                TxtTranstype.Text = data.transmain.transType;

                TxtCashAmount.Visibility = Visibility.Collapsed;
                TxtCardAmount.Visibility = Visibility.Collapsed;
                TxtTipAmount.Visibility = Visibility.Collapsed;
                TxtDiscount.Visibility = Visibility.Collapsed;
                TxtTranstype.Visibility = Visibility.Visible;


                if (data.transmain.cashAmount > 0)
                {
                    TxtCashAmount.Text = $"Cash: ${data.transmain.cashAmount:0.00}";
                    TxtCashAmount.Visibility = Visibility.Visible;
                }

                if (data.transmain.cardAmount > 0)
                {
                    TxtCardAmount.Text = $"Card: ${data.transmain.cardAmount:0.00}";
                    TxtCardAmount.Visibility = Visibility.Visible;
                }

                if (data.transmain.tipAmount > 0)
                {
                    TxtTipAmount.Text = $"Tip: ${data.transmain.tipAmount:0.00}";
                    TxtTipAmount.Visibility = Visibility.Visible;
                }

                if (data.transmain.invoiceDiscount > 0)
                {
                    TxtDiscount.Text = $"Discount: ${data.transmain.invoiceDiscount:0.00}";
                    TxtDiscount.Visibility = Visibility.Visible;
                }
            }
        }



        private void BtnRefund_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Testin");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //TxtInvoiceID.Text = CurrentTransData.MainData.invoiceId.ToString();
        }

        private void BtnEndview_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            UserControlHidden?.Invoke(this, EventArgs.Empty);
        }
    }
}
