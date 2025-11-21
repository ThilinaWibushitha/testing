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
using System.Globalization;
using Pospointe.Models;
using Pospointe.LocalData;
using System.Drawing.Printing;
using System.Drawing;
using System.Security.Cryptography;

namespace Pospointe.Reports
{
    /// <summary>
    /// Interaction logic for FrmShiftSumm.xaml
    /// </summary>
    public partial class FrmShiftSumm : Window
    {
        public Shiftcls shiftrp { get; set; }
        public FrmShiftSumm()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBusinessInfo();
            LoadReportData();
        }

        private void LoadReportData()
        {
            TxtStartingCash.Text = shiftrp.startcash.ToString("C2");
            TxtCashpayments.Text = shiftrp.totalcash.ToString("C2");
            TxtcashRefunds.Text = shiftrp.refunds.ToString("C2");
            TxtExpectedCash.Text = shiftrp.expectedcash.ToString("C2");
            TxtActual.Text = shiftrp.actualcash.ToString("C2");
            TxtDifferetce.Text = shiftrp.deference.ToString("C2");
            TxtGrossSales.Text = shiftrp.grosssales.ToString("C2");
            TxtRefunds.Text = shiftrp.refunds.ToString("C2");
            TxtDiscounts.Text = shiftrp.discount.ToString("C2");
            TxtCash.Text = shiftrp.actualcash.ToString("C2");
            TxtCard.Text = shiftrp.totalcard.ToString("C2");
            TxtTip.Text = shiftrp.tip.ToString("C2");
            TxtOpenedShift.Text = shiftrp.shiftopen;
            TxtClosedShift.Text = shiftrp.shiftclose;
        }

        private void LoadBusinessInfo()
        {
            TxtUser.Text = LoggedData.loggeduser.UserName;
            

            using (var context = new PosDb1Context())
            {
                var businessInfo = context.TblBusinessInfos.FirstOrDefault(); 

                if (businessInfo != null)
                {
                    TxtBusinessName.Text = businessInfo.BusinessName ?? "Business Name Not Available";
                    TxtAddress.Text = businessInfo.BusinessAddress ?? "Address Not Available";
                    Txtstatezip.Text = businessInfo.CityStatezip ?? "Address Not Available";
                    TxtPhonenumber.Text = businessInfo.BusinessPhone ?? "Phone Number Not Available";
                    TxtEmail.Text = businessInfo.BusinessEmail ?? "Email Not Available";
                }
                else
                {
                    MessageBox.Show("No business information found in the database.", "Data Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(this.pd_rcpt_inv_PrintPageENGPOS);
                PrintDialog printdlg = new PrintDialog();
                pd.PrinterSettings.PrinterName = Properties.Settings.Default.ReceiptPrinter;
                pd.Print();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Alert");
            }
        }

        private void BtnSms_Click(object sender, RoutedEventArgs e)
        {

        }

        private void pd_rcpt_inv_PrintPageENGPOS(object sender, PrintPageEventArgs e)
        {

            System.Drawing.FontFamily bill_font = new System.Drawing.FontFamily("Consolas");
            Graphics graphics = e.Graphics;
            Font font = new Font(bill_font, 8);
            float fontHeight = font.GetHeight();
            int startX = 10;
            int startY = 0;
            int Offset = 0;
            String underLine = "============================================";
            string underLineSingle = "-------------------------------------------------------------------------------------------";

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Far;
            stringFormat.LineAlignment = StringAlignment.Near;

            float x = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(TxtBusinessName.Text, new Font(bill_font, 15, System.Drawing.FontStyle.Bold)).Width / 2);
            float x1 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(TxtAddress.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold)).Width / 2);
            float x2 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(Txtstatezip.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold)).Width / 2);
            float x3 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(TxtEmail.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold)).Width / 2);
            float x4 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(TxtPhonenumber.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold)).Width / 2);
            float x5 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString("SHIFT REPORT", new Font(bill_font, 15, System.Drawing.FontStyle.Bold)).Width / 2);


            graphics.DrawString(TxtAddress.Text, new Font(bill_font, 15, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), x, startY + Offset);
            Offset = Offset + 30;


            graphics.DrawString(TxtAddress.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), x1, startY + Offset);
            Offset = Offset + 15;


            graphics.DrawString(Txtstatezip.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), x2, startY + Offset);
            Offset = Offset + 15;

            graphics.DrawString(TxtEmail.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), x2, startY + Offset);
            Offset = Offset + 15;

            graphics.DrawString(TxtPhonenumber.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), x2, startY + Offset);
            Offset = Offset + 15;
            ////graphics.DrawString("Start Time: "+ lblsttime.Text, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 5, startY + Offset);
            ////Offset = Offset + 15;

            ////graphics.DrawString("End Time: " + lblentime.Text, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 5, startY + Offset);
            ////Offset = Offset + 15;


            graphics.DrawString(underLine, new Font(bill_font, 8), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 15;



            graphics.DrawString("SHIFT REPORT", new Font(bill_font, 15, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), x5, startY + Offset);
            Offset = Offset + 30;

            //graphics.DrawString("Start Date: " + lblstdate.Text, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 5, startY + Offset);

            //graphics.DrawString("End Date: " + lblendt.Text, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 150, startY + Offset);
            //Offset = Offset + 15;

            graphics.DrawString(underLine, new Font(bill_font, 8), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 15;

            graphics.DrawString("UserName : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtUser.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("Shift Opened : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtOpenedShift.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 150, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("Shift Closed : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtClosedShift.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 150, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString(underLine, new Font(bill_font, 8), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 15;

            graphics.DrawString("CASH DRAWER", new Font(bill_font, 15, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), x5, startY + Offset);
            Offset = Offset + 30;

            graphics.DrawString(underLine, new Font(bill_font, 8), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 15;

            graphics.DrawString("Start Cash : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtStartingCash.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("Cash Payment : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtCashpayments.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("Cash Refund : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtcashRefunds.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("Expected Cash : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtExpectedCash.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("Actual Cash : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtActual.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("Difference : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtDifferetce.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString(underLine, new Font(bill_font, 8), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 15;

            graphics.DrawString("SALES SUMMERY", new Font(bill_font, 15, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), x5, startY + Offset);
            Offset = Offset + 30;

            graphics.DrawString(underLine, new Font(bill_font, 8), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 15;

            graphics.DrawString("GROSS SALES : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtGrossSales.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("REFUNDS : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtRefunds.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("DISCOUNTS : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtDiscounts.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString(underLine, new Font(bill_font, 8), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 15;

            graphics.DrawString("CASH : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtCash.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("Cash Rounding : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtCashRounding.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("CARD : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtCard.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("TIP : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtTip.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString(underLine, new Font(bill_font, 8), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 15;



            string note100 = shiftrp.tblDayOpenCashCollection.Note100.ToString();
            string note50 = shiftrp.tblDayOpenCashCollection.Note50.ToString();
            string note20 = shiftrp.tblDayOpenCashCollection.Note20.ToString();
            string note10 = shiftrp.tblDayOpenCashCollection.Note10.ToString();
            string note5 = shiftrp.tblDayOpenCashCollection.Note5.ToString();
            string note1 = shiftrp.tblDayOpenCashCollection.Note1.ToString();
            string coin50 = shiftrp.tblDayOpenCashCollection.Coin50.ToString();
            string coin25 = shiftrp.tblDayOpenCashCollection.Coin25.ToString();
            string coin10 = shiftrp.tblDayOpenCashCollection.Coin10.ToString();
            string coin5 = shiftrp.tblDayOpenCashCollection.Coin5.ToString();
            string coin1 = shiftrp.tblDayOpenCashCollection.Coin1.ToString();

            graphics.DrawString("CASH COLLECTION", new Font(bill_font, 15, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), x5, startY + Offset);
            Offset = Offset + 30;

            graphics.DrawString(underLine, new Font(bill_font, 8), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 15;

            graphics.DrawString("$100 Bills : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(note100, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("$50 Bills : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(note50, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;
            graphics.DrawString("$20 Bills : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(note20, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;
            graphics.DrawString("$10 Bills : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(note10, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;
            graphics.DrawString("$5 Bills : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(note5, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;
            graphics.DrawString("$1 Bills : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(note1, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;
            graphics.DrawString("50c : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(coin50, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;
            graphics.DrawString("25c : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(coin25, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;
            graphics.DrawString("10c : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(coin10, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;
            graphics.DrawString("5c : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(coin5, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;
            graphics.DrawString("1c : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(coin1, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;
            graphics.DrawString(underLine, new Font(bill_font, 8), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 15;

        }
    }
}
