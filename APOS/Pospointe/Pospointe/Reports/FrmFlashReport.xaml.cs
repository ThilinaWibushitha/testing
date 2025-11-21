using Pospointe.Models;
//using System;
//using System.Collections.Generic;

//using System.Drawing;
//using System.Drawing.Printing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Printing;
using Microsoft.Data.SqlClient;
using Pospointe.LocalData;
using System.Data;
using System.Globalization;

namespace Pospointe.Reports
{
    /// <summary>
    /// Interaction logic for FrmFlashReport.xaml
    /// </summary>
    public partial class FrmFlashReport : Window
    {
        private readonly ReportData _reportData;
        private readonly string _fromDate;
        private readonly string _toDate;
        public FrmFlashReport(ReportData reportData, string fromDate, string toDate)
        {
            InitializeComponent();
            _reportData = reportData;
            _fromDate = fromDate;
            _toDate = toDate;



        }

        



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBusinessInfo();
            TxtFromDate.Text = _fromDate;
            TxtToDate.Text = _toDate;
            TxtGrossSales.Text = $"${_reportData.grosssales:N2}";
            TxtGrossWTax.Text = $"${_reportData.grosssaleswtax:F2}";
            TxtNonTax.Text = $"${_reportData.nontaxsales:F2}";
            TxtNetSales.Text = $"${_reportData.netsales:F2}";
            TxtSalesTax.Text = $"${_reportData.salestax:F2}";
            TxtCashRefunds.Text = $"${_reportData.cashrefund:F2}";
            TxtCardRefunds.Text  = $"${_reportData.cardrefund:F2}";
            TxtTotalRefunds.Text = $"${_reportData.totalrefund:F2}";
            TxtCash.Text = $"${_reportData.cashtotal:F2}";
            TxtCreditDebit.Text = $"${_reportData.cardtotal:F2}";
            TxtGiftcard.Text = $"${_reportData.giftcardtotal:F2}";
            TxtTip.Text = $"${_reportData.tiptotal:F2}";
            TxtNoofTrans.Text = $"{_reportData.nooftrans}";
            TxtNoofReturns.Text = $"{_reportData.nooftransreturn}";
        }



        private void LoadBusinessInfo()
        {
            using (var context = new PosDb1Context())
            {
                var businessInfo = context.TblBusinessInfos.FirstOrDefault(); // Assuming there's only one business info record

                if (businessInfo != null)
                {
                    // Update TextBlocks with business information
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

       // private static string bill_font = "Consolas";
        private static string bill_font_SINHALA = "Arial Unicode MS";
        private void pd_rcpt_inv_PrintPageENGPOS(object sender, PrintPageEventArgs e)
        {


            Graphics graphics = e.Graphics;

            string bill_font = "Consolas";



            Font font = new Font(new FontFamily(bill_font), 8);

            //Font font = new Font(bill_font, 8);

            float fontHeight = font.GetHeight();
            int startX = 10;
            int startY = 0;
            int Offset = 0;
            String underLine = "============================================";
            //string underLineSingle = "-------------------------------------------------------------------------------------------";

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Far;
            stringFormat.LineAlignment = StringAlignment.Near;

            float x = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(TxtBusinessName.Text, font).Width / 2);
            float x1 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(TxtAddress.Text, font).Width / 2);
            float x2 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(Txtstatezip.Text, font).Width / 2);
            float x3 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(TxtPhonenumber.Text, font).Width / 2);
            float x4 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(TxtEmail.Text, font).Width / 2);
            float x5 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString("FLASH REPORT", new Font(new FontFamily(bill_font), 15, System.Drawing.FontStyle.Bold)).Width / 2);


            graphics.DrawString(TxtBusinessName.Text, new Font(new FontFamily(bill_font), 15, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), x, startY + Offset);
            Offset = Offset + 30;


            graphics.DrawString(TxtAddress.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), x1, startY + Offset);
            Offset = Offset + 15;


            graphics.DrawString(Txtstatezip.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), x2, startY + Offset);
            Offset = Offset + 15;

            //graphics.DrawString("Start Time: "+ lblsttime.Text, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 5, startY + Offset);
            //Offset = Offset + 15;

            //graphics.DrawString("End Time: " + lblentime.Text, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 5, startY + Offset);
            //Offset = Offset + 15;


            graphics.DrawString(underLine, new Font(bill_font, 8), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 15;



            graphics.DrawString("FLASH REPORT", new Font(bill_font, 15, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), x5, startY + Offset);
            Offset = Offset + 30;

            graphics.DrawString("Start Date: " + TxtFromDate.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);

            graphics.DrawString("End Date: " + TxtToDate.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 150, startY + Offset);
            Offset = Offset + 15;

            graphics.DrawString(underLine, new Font(bill_font, 8), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 15;

            graphics.DrawString("Net Sales: ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtNetSales.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("Net Sales Taxed: ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtNetSales.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("Net Sales Non Taxed: ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtNonTax.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("Taxes: ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtSalesTax.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("Gross Sales: ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtGrossSales.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            //graphics.DrawString("Gross Sales: ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            //graphics.DrawString(TxtGrossWTax.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            //Offset = Offset + 20;

            graphics.DrawString("=============== REFUND TOTALS ==============", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            //graphics.DrawString(lblgrosssale.Text, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("Cash Refund: ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtCashRefunds.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("Card Refund: ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtCardRefunds.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("Total Refunds: ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtTotalRefunds.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("============== TENDER TOTALS ===============", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            //graphics.DrawString(lblgrosssale.Text, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("Cash : ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtCash.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("Credit: ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtCreditDebit.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("Gift Card Sales: ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtGiftcard.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("Tip: ", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtTip.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("== COUNT OF TRANSACTIONS ==", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            //graphics.DrawString(TxtGrossSales.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("#of total sale transactions", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtNoofTrans.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("#of total return transactions", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 5, startY + Offset);
            graphics.DrawString(TxtNoofReturns.Text, new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 225, startY + Offset);
            Offset = Offset + 20;

            graphics.DrawString("", new Font(bill_font, 8), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 15;
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(this.pd_rcpt_inv_PrintPageENGPOS);
                //PrintDialog printdlg = new PrintDialog();
                pd.PrinterSettings.PrinterName = Properties.Settings.Default.ReceiptPrinter;
                pd.Print();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSms_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
