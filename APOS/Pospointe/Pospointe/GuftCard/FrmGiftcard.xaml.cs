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

namespace Pospointe.GuftCard
{
    /// <summary>
    /// Interaction logic for FrmGiftcard.xaml
    /// </summary>
    public partial class FrmGiftcard : Window
    {
        public string giftoptionresponse { get; set; }

        public decimal amount { get; set; }
        public FrmGiftcard()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Btn25_Click(object sender, RoutedEventArgs e)
        {
            giftoptionresponse = "ACTIVENEW";
            amount = 25;
            this.DialogResult = true;
            this.Close();
        }

        private void Btn50_Click(object sender, RoutedEventArgs e)
        {
            giftoptionresponse = "ACTIVENEW";
            amount = 50;
            this.DialogResult = true;
            this.Close();
        }

        private void Btn75_Click(object sender, RoutedEventArgs e)
        {
            giftoptionresponse = "ACTIVENEW";
            amount = 75;
            this.DialogResult = true;
            this.Close();
        }

        private void Btn100_Click(object sender, RoutedEventArgs e)
        {
            giftoptionresponse = "ACTIVENEW";
            amount = 100;
            this.DialogResult = true;
            this.Close();
        }

        private void Btn125_Click(object sender, RoutedEventArgs e)
        {
            giftoptionresponse = "ACTIVENEW";
            amount = 125;
            this.DialogResult = true;
            this.Close();
        }

        private void Btn150_Click(object sender, RoutedEventArgs e)
        {
            giftoptionresponse = "ACTIVENEW";
            amount = 150;
            this.DialogResult = true;
            this.Close();
        }

        private void Btn175_Click(object sender, RoutedEventArgs e)
        {
            giftoptionresponse = "ACTIVENEW";
            amount = 175;
            this.DialogResult = true;
            this.Close();
        }

        private void Btn200_Click(object sender, RoutedEventArgs e)
        {
            giftoptionresponse = "ACTIVENEW";
            amount = 200;
            this.DialogResult = true;
            this.Close();
        }

        private void BtnBalanceCheck_Click(object sender, RoutedEventArgs e)
        {
            
            giftoptionresponse = "GIFTBALANCE";

            this.DialogResult = true;
            this.Close();
        }

        private void BtnReload_Click(object sender, RoutedEventArgs e)
        {
            giftoptionresponse = "RELOAD";
            FrmNumbpad frmFN = new FrmNumbpad();
           // frmFN.lblRequest.Text = "Enter Reload Amount";
            var result = frmFN.ShowDialog();
            if (result == true)
            {
                string val = frmFN.returnvalue;
                decimal updateprice = Convert.ToDecimal(val);
                decimal maxreload = 200;
                if (updateprice < maxreload)
                {
                    amount = updateprice;
                    this.DialogResult = true;
                    this.Close();
                }

                else
                {
                    MessageBox.Show($"Request AMount ${updateprice}");
                    MessageBox.Show("Amount Should be less than $200.00");
                }
            }
        }

        private void BtnDeactivate_Click(object sender, RoutedEventArgs e)
        {
            giftoptionresponse = "DIACTIVE";
            this.DialogResult = true;
            this.Close();
        }

        private void BtnActivate_Click(object sender, RoutedEventArgs e)
        {
            giftoptionresponse = "REACTIVE";
            this.DialogResult = true;
            this.Close();
        }
    }
}
