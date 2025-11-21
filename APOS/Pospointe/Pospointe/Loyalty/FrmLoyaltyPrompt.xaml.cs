using Pospointe.LocalData;
using Pospointe.PosMenu;
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

namespace Pospointe.Loyalty
{
    /// <summary>
    /// Interaction logic for FrmLoyaltyPrompt.xaml
    /// </summary>
    public partial class FrmLoyaltyPrompt : Window
    {
        public string phonenumber { get; set; }
        public FrmLoyaltyPrompt()
        {
            InitializeComponent();
        }

        private void BtnQR_Click(object sender, RoutedEventArgs e)
        {
            FrmPaxscreen pax = new FrmPaxscreen();
            //COM DETAILS
            pax.comtype = LoggedData.comtype;
            if (LoggedData.comtype == "PAX_IP")
            {
                pax.ipaddr = LoggedData.PaxIP;
                pax.portnum = LoggedData.PaxPort;
            }

            else if (LoggedData.comtype == "PAX_USB")
            {

                pax.comport = LoggedData.PaxComPort;
                pax.bautrate = LoggedData.PaxBaudRate;
            }

            // pax.signaturecapture = Clsloggedindata.signaturecapture;
            pax.transtyperequest = "GETVGIFT";
            var result = pax.ShowDialog();
            if (result == true)
            {
                if (pax.responsecode == "000000")
                {
                    phonenumber = pax.cardreaddata;
                    this.DialogResult = true;
                    this.Close();
               }

                else
                {
                    MessageBox.Show(pax.responseMsg, pax.responsecode);
                }

            }
        }

        private void BtnPhone_Click(object sender, RoutedEventArgs e)
        {
            PhonePad frmFN = new PhonePad();
            frmFN.EmterPH.Text = "Enter Customer Phone #";
            frmFN.request = "custonmerphonenum";
            bool? result = frmFN.ShowDialog();
            if (result == true)
            {
                phonenumber = frmFN.ReturnValue1;
                this.DialogResult = true;
                this.Close();
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
