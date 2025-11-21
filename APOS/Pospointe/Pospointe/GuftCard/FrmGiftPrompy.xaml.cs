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

namespace Pospointe.GuftCard
{
    /// <summary>
    /// Interaction logic for FrmGiftPrompy.xaml
    /// </summary>
    public partial class FrmGiftPrompy : Window
    {
        public string CardNumber { get; set; }
        public FrmGiftPrompy()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
                    CardNumber = pax.cardreaddata;
                    this.DialogResult = true;
                    this.Close();
                }

                else
                {
                    MessageBox.Show(pax.responseMsg, pax.responsecode);
                }

            }
        }

        private void BtnCard_Click(object sender, RoutedEventArgs e)
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
            pax.transtyperequest = "GETPGIFT";
            var result = pax.ShowDialog();
            if (result == true)
            {
                if (pax.responsecode == "000000")
                {
                    CardNumber = pax.cardreaddata;
                    this.DialogResult = true;
                    this.Close();
                }

                else
                {
                    MessageBox.Show(pax.responseMsg, pax.responsecode);
                }

            }
        }
    }
}
