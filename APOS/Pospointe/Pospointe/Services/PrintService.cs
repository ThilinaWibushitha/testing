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
using Pospointe.Models;
using Microsoft.Data.Tools.Diagnostics;
using ESC_POS_USB_NET.Printer;
using System.Security.Cryptography;


namespace Pospointe.Services
{

    public class PrintService
    {

        public static void OpenCashDrawer()
        {

            try
            {
                Printer printer = new Printer(Properties.Settings.Default.ReceiptPrinter);
                if(Properties.Settings.Default.CashDrawerEnabled == true)
                {
                    printer.OpenDrawer();
                }
                printer.PrintDocument();
            }

            catch { 
            
            }
        }
        public static void PrinterReceipt(out bool isSuccessful)
        {
            isSuccessful = false;
            try
            {
                // Calculate dynamic paper height
                int baseHeight = 600; // Includes headers, subtotal, tax, etc.
                int itemHeight = 35;  // More realistic per item (2 lines per item at least)
                int totalHeight = baseHeight + (CurrentTransData.transitems.Count * itemHeight);

                // Set a minimum and maximum height to avoid errors
                totalHeight = Math.Max(totalHeight, 500); // Ensure minimum size
                totalHeight = Math.Min(totalHeight, 32760); // Windows max paper height

                // Create a custom paper size
                PaperSize customSize = new PaperSize("Custom", 280, totalHeight);

                PrintDocument printDocument = new PrintDocument
                {
                    PrinterSettings = new PrinterSettings
                    {
                        PrinterName = Properties.Settings.Default.ReceiptPrinter
                    },
                    PrintController = new StandardPrintController() // No dialogs
                };

                // Apply the dynamic paper size
                printDocument.DefaultPageSettings.PaperSize = customSize;

                // Check if the printer is valid
                if (!printDocument.PrinterSettings.IsValid)
                {
                    throw new InvalidOperationException("The specified printer is not valid or not found.");
                }

                // Attach the event handler
                printDocument.PrintPage += PrintDocument_PrintPage;

                // Start printing
                printDocument.Print();
                isSuccessful = true;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Printing error: {ex.Message}");
            }
        }


        public static void PrintertoKitchen(List<ItemGridModel> items )
        {
            ItemList = items;
            
            try
            {
                PrintDocument printDocument = new PrintDocument
                {
                    PrinterSettings = new PrinterSettings
                    {
                        PrinterName = Properties.Settings.Default.KitchenPrinter
                    },
                    PrintController = new StandardPrintController() // No dialogs
                };

                // Check if the printer is valid
                if (!printDocument.PrinterSettings.IsValid)
                {
                    throw new InvalidOperationException("The specified printer is not valid or not found.");

                }

                // Attach the event handler
                printDocument.PrintPage += PrintDocument_PrintKitchen;

                // Start printing
                printDocument.Print();
              


            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Printing error: {ex.Message}");
            }

        }




        //private static string changetocurrency(double amount)
        //{
        //    // Simulated data
        //    string subtotalString = "1234.56"; // Example of subtotal as string

        //    // Try parsing the string to decimal
        //    if (decimal.TryParse(subtotalString, out decimal subtotal))
        //    {
        //        // Format as currency
        //        string formattedCurrency = subtotal.ToString("C", CultureInfo.CurrentCulture);

        //        // Output the formatted value
        //        Console.WriteLine($"Subtotal: {formattedCurrency}");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Invalid subtotal value.");
        //    }

        //}

        public static List<ItemGridModel> ItemList = new List<ItemGridModel>();

        public static void PrintDocument_PrintKitchen(object sender, PrintPageEventArgs e)
        {
            string bill_font = "Consolas";
            Graphics graphics = e.Graphics;
            Font font = new Font(bill_font, 8);
            float fontHeight = font.GetHeight();
            int startX = 10;
            int startY = 0;
            string tp_no = "";
            int Offset = 0;
            String underLine = "================================================================";
            string underLineSingle = "-----------------------------------------------------------------------";

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Far;
            stringFormat.LineAlignment = StringAlignment.Near;

           

           

            float x = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(LoggedData.BusinessInfo.BusinessName, new Font(bill_font, 15, FontStyle.Bold)).Width / 2);
            float x1 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(LoggedData.BusinessInfo.BusinessAddress, new Font(bill_font, 8, FontStyle.Bold)).Width / 2);
            float x2 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(LoggedData.BusinessInfo.CityStatezip, new Font(bill_font, 8, FontStyle.Bold)).Width / 2);
            float x3 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(LoggedData.BusinessInfo.BusinessPhone, new Font(bill_font, 8, FontStyle.Bold)).Width / 2);
            float x4 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(LoggedData.BusinessInfo.BusinessEmail, new Font(bill_font, 8, FontStyle.Bold)).Width / 2);
            float cashier = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(LoggedData.loggeduser.UserName, new Font(bill_font, 15, FontStyle.Bold)).Width / 2);
            float station = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(LoggedData.StationID, new Font(bill_font, 15, FontStyle.Bold)).Width / 2);
            float x6 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString("SOFTWARE BY - ASN IT", new Font(bill_font, 6, FontStyle.Bold)).Width / 2);


            //Image i = Image.FromFile(Application.StartupPath + "\\AKH.jpg");
            //if (i != null)
            //{
            //    graphics.DrawImage(i, 110, startY + Offset, 75, 75);
            //}

            Offset = Offset + 20;
            graphics.DrawString(LoggedData.BusinessInfo.BusinessName, new Font(bill_font, 15, FontStyle.Bold), new SolidBrush(Color.Black), x, startY + Offset);
            Offset = Offset + 20;


            graphics.DrawString(LoggedData.BusinessInfo.BusinessAddress, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), x1, startY + Offset);
            Offset = Offset + 15;


            graphics.DrawString(LoggedData.BusinessInfo.CityStatezip, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), x2, startY + Offset);
            Offset = Offset + 15;

            graphics.DrawString(LoggedData.BusinessInfo.BusinessPhone, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), x3, startY + Offset);
            Offset = Offset + 15;

            graphics.DrawString(LoggedData.BusinessInfo.BusinessEmail, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), x4, startY + Offset);
            Offset = Offset + 15;

            graphics.DrawString(LoggedData.loggeduser.UserName, new Font(bill_font, 15, FontStyle.Bold), new SolidBrush(Color.Black), station, startY + Offset);
            Offset = Offset + 25;

            graphics.DrawString(LoggedData.StationID, new Font(bill_font, 15, FontStyle.Bold), new SolidBrush(Color.Black), cashier, startY + Offset);
            Offset = Offset + 25;

            graphics.DrawString(underLine, new Font(bill_font, 8), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 15;

            //if (Properties.Settings.Default.useChekNoinsteadInvoice == true)
           // {
                graphics.DrawString("CHECK NO : " + "NONE", new Font(bill_font, 15, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
                //graphics.DrawString("REPRINT", new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 200, startY + Offset);
                Offset = Offset + 20;
           // }


            //graphics.DrawString("INVOICE NO : " + invNo, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
            ////graphics.DrawString("REPRINT", new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 200, startY + Offset);
            //Offset = Offset + 15;
            //clsS.OpenDB();
            //using (SqlDataAdapter da = new SqlDataAdapter("SELECT Tbl_Trans_Main.InvoiceID, Tbl_Trans_Main.TransType, Tbl_Trans_Main.Subtotal, Tbl_Trans_Main.Tax1, Tbl_Trans_Main.GrandTotal, Tbl_Trans_Main.SaleDate, Tbl_Trans_Main.SaleTime, Tbl_Trans_Main.CashAmount, Tbl_Trans_Main.CardAmount, Tbl_Users.UserName, Tbl_Trans_Main.CashChangeAmount, Tbl_Trans_Main.Paidby, Tbl_Items.ItemName, Tbl_Trans_Sub.ItemID, Tbl_Trans_Sub.ItemType, Tbl_Trans_Sub.ItemPrice, Tbl_Trans_Sub.Qty, Tbl_Trans_Sub.Amount FROM Tbl_Items INNER JOIN Tbl_Trans_Sub ON Tbl_Items.ItemID = Tbl_Trans_Sub.ItemID INNER JOIN Tbl_Trans_Main ON Tbl_Trans_Sub.TransMainID = Tbl_Trans_Main.InvoiceID INNER JOIN Tbl_Users ON Tbl_Trans_Main.CashierID = Tbl_Users.UserID WHERE Tbl_Trans_Main.InvoiceID = @inv_no", clsS.myConnection))
            //{
            //    da.SelectCommand.Parameters.Clear();
            //    da.SelectCommand.Parameters.AddWithValue("@inv_no", invNo);
            //    DataTable dt = new DataTable();
            //    da.Fill(dt);
            //    if (dt.Rows.Count > 0)
            //    {
            graphics.DrawString("DATE : " + DateTime.Now.ToShortDateString() + " TIME: " + DateTime.Now.ToShortTimeString(), new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 15;

            graphics.DrawString("INVOICE NO : " + "NONE", new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
            //graphics.DrawString("REPRINT", new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 200, startY + Offset);
            Offset = Offset + 15;
            //graphics.DrawString("CASHIOR : " + dt.Rows[0].Field<string>(9), new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
            //Offset = Offset + 15;

            //        graphics.DrawString(underLineSingle, new Font(bill_font, 7), new SolidBrush(Color.Black), startX, startY + Offset);
            //        Offset = Offset + 10;
            //    }
            //    clsS.CloseDB();
            //}

            graphics.DrawString("ITEM", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
            graphics.DrawString("QTY", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), 250, startY + Offset);

            Offset = Offset + 25;
            graphics.DrawString(underLineSingle, new Font(bill_font, 7), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 10;

            foreach (var dtr in ItemList)
            {
                if (dtr.IsKOT1 && !dtr.IsVoided)
                {
                    graphics.DrawString(dtr.ItemName, new Font(bill_font, 14, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
                    graphics.DrawString(dtr.Quantity.ToString(), new Font(bill_font, 14, FontStyle.Bold), new SolidBrush(Color.Black), 270, startY + Offset, stringFormat);
                    Offset += 24;
                }
            }



            graphics.DrawString(underLine, new Font(bill_font, 8), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 22;

            graphics.DrawString("", new Font(bill_font, 8), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 25;
        }



        private static void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;
            string bill_font = "Consolas";

            Font font = new Font(bill_font, 8);
            float fontHeight = font.GetHeight();
            int startX = 10;
            int startY = 0;
            string tp_no = "";
            int Offset = 0;
            String underLine = "================================================================";
            string underLineSingle = "-------------------------------------------------------------------------------------------";

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Far;
            stringFormat.LineAlignment = StringAlignment.Near;

            //clsSQLExecute clsS = new clsSQLExecute();

            //DataTable BussinesData = new DataTable();
            //clsS.OpenDB();
            //using (SqlDataAdapter da = new SqlDataAdapter(" select BusinessName,BusinessAddress,CityStatezip,Business_Phone,Business_Email,Logo_Path,Regcode  from Tbl_Business_Info", clsS.myConnection))
            //{
            //    BussinesData.Rows.Clear();
            //    da.Fill(BussinesData);
            //    clsS.CloseDB();
            //}

            //tp_no = BussinesData.Rows[0].Field<string>(2);

            float x = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(LoggedData.BusinessInfo.BusinessName, new Font(bill_font, 15, FontStyle.Bold)).Width / 2);
            float x1 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(LoggedData.BusinessInfo.BusinessAddress, new Font(bill_font, 8, FontStyle.Bold)).Width / 2);
            float x2 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(LoggedData.BusinessInfo.CityStatezip, new Font(bill_font, 8, FontStyle.Bold)).Width / 2);
            float x3 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(LoggedData.BusinessInfo.BusinessPhone, new Font(bill_font, 8, FontStyle.Bold)).Width / 2);
            float x4 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(LoggedData.BusinessInfo.BusinessEmail, new Font(bill_font, 8, FontStyle.Bold)).Width / 2);
            float x5 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString("CHECK NO : " + CurrentTransData.MainData.invoiceId, new Font(bill_font, 15, FontStyle.Bold)).Width / 2);
            float x20 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString("Invoice ID : " + CurrentTransData.MainData.InvoiceUniqueId, new Font(bill_font, 6, FontStyle.Regular)).Width / 2);
            float x6 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString("SOFTWARE BY - ASN IT", new Font(bill_font, 6, FontStyle.Bold)).Width / 2);


            //Image i = Image.FromFile(Application.StartupPath + "\\AKH.jpg");
            //if (i != null)
            //{
            //    graphics.DrawImage(i, 110, startY + Offset, 75, 75);
            //}

            Offset = Offset + 20;
            graphics.DrawString(LoggedData.BusinessInfo.BusinessName, new Font(bill_font, 15, FontStyle.Bold), new SolidBrush(Color.Black), x, startY + Offset);
            Offset = Offset + 20;


            graphics.DrawString(LoggedData.BusinessInfo.BusinessAddress, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), x1, startY + Offset);
            Offset = Offset + 15;


            graphics.DrawString(LoggedData.BusinessInfo.CityStatezip, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), x2, startY + Offset);
            Offset = Offset + 15;

            graphics.DrawString(LoggedData.BusinessInfo.BusinessPhone, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), x3, startY + Offset);
            Offset = Offset + 15;

            graphics.DrawString(LoggedData.BusinessInfo.BusinessEmail, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), x4, startY + Offset);
            Offset = Offset + 15;


            graphics.DrawString(underLine, new Font(bill_font, 8), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 15;

           // if (Properties.Settings.Default.useChekNoinsteadInvoice == true)
           // {
                graphics.DrawString("CHECK NO : " + CurrentTransData.MainData.invoiceId, new Font(bill_font, 15, FontStyle.Bold), new SolidBrush(Color.Black), x5, startY + Offset);
                //graphics.DrawString("REPRINT", new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 200, startY + Offset);
                Offset = Offset + 20;
          //  }

          //  graphics.DrawString("INVOICE NO : " + CurrentTranData.invoiceId, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
            //graphics.DrawString("REPRINT", new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 200, startY + Offset);
           // Offset = Offset + 15;
            // clsS.OpenDB();
            //  using (SqlDataAdapter da = new SqlDataAdapter("SELECT Tbl_Trans_Main.InvoiceID, Tbl_Trans_Main.TransType, Tbl_Trans_Main.Subtotal, Tbl_Trans_Main.Tax1, Tbl_Trans_Main.GrandTotal, Tbl_Trans_Main.SaleDate, Tbl_Trans_Main.SaleTime, Tbl_Trans_Main.CashAmount, Tbl_Trans_Main.CardAmount, Tbl_Users.UserName, Tbl_Trans_Main.CashChangeAmount, Tbl_Trans_Main.Paidby, Tbl_Items.ItemName, Tbl_Trans_Sub.ItemID, Tbl_Trans_Sub.ItemType, Tbl_Trans_Sub.ItemPrice, Tbl_Trans_Sub.Qty, Tbl_Trans_Sub.Amount FROM Tbl_Items INNER JOIN Tbl_Trans_Sub ON Tbl_Items.ItemID = Tbl_Trans_Sub.ItemID INNER JOIN Tbl_Trans_Main ON Tbl_Trans_Sub.TransMainID = Tbl_Trans_Main.InvoiceID INNER JOIN Tbl_Users ON Tbl_Trans_Main.CashierID = Tbl_Users.UserID WHERE Tbl_Trans_Main.InvoiceID = @inv_no", clsS.myConnection))
            //  {
            //      da.SelectCommand.Parameters.Clear();
            //      da.SelectCommand.Parameters.AddWithValue("@inv_no", invNo);
            //       DataTable dt = new DataTable();
            //       da.Fill(dt);
            //       if (dt.Rows.Count > 0)
            //     {
            graphics.DrawString("DATE : " + CurrentTransData.MainData.saleDateTime.ToShortDateString() + " TIME: " + CurrentTransData.MainData.saleDateTime.ToShortTimeString(), new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 15;
            graphics.DrawString("CASHIOR : " + CurrentTransData.MainData.cashierId, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 15;
            graphics.DrawString("CASHIOR NAME: " + LoggedData.loggeduser.UserName, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 15;
            graphics.DrawString("INVOICE : " + CurrentTransData.MainData.InvoiceIdshortCode, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 15;

            graphics.DrawString("TYPE : " + CurrentTransData.MainData.transType, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 15;

            graphics.DrawString(underLineSingle, new Font(bill_font, 7), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 10;
            //      }
            //    clsS.CloseDB();
            //   }

            graphics.DrawString("****** "+ CurrentTransData.MainData.transType + " ******", new Font(bill_font, 12, FontStyle.Bold), new SolidBrush(Color.Black), 70, Offset);
            Offset = Offset + 20;

            graphics.DrawString(underLineSingle, new Font(bill_font, 7), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 10;

            graphics.DrawString("ITEM", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
            graphics.DrawString("QTY", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), 150, startY + Offset);
            graphics.DrawString("PRICE", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), 240, startY + Offset);
            Offset = Offset + 25;
            graphics.DrawString(underLineSingle, new Font(bill_font, 7), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 10;
          
            //double item_discount = 0;
            //using (SqlDataAdapter da = new SqlDataAdapter("SELECT Tbl_Trans_Main.InvoiceID, Tbl_Trans_Main.TransType, Tbl_Trans_Main.Subtotal, Tbl_Trans_Main.Tax1, Tbl_Trans_Main.GrandTotal, Tbl_Trans_Main.SaleDate, Tbl_Trans_Main.SaleTime, Tbl_Trans_Main.CashAmount, Tbl_Trans_Main.CardAmount, Tbl_Users.UserName, Tbl_Trans_Main.CashChangeAmount, Tbl_Trans_Main.Paidby, Tbl_Items.ItemName, Tbl_Trans_Sub.ItemID, Tbl_Trans_Sub.ItemType, Tbl_Trans_Sub.ItemPrice, Tbl_Trans_Sub.Qty, Tbl_Trans_Sub.Amount FROM Tbl_Items INNER JOIN Tbl_Trans_Sub ON Tbl_Items.ItemID = Tbl_Trans_Sub.ItemID INNER JOIN Tbl_Trans_Main ON Tbl_Trans_Sub.TransMainID = Tbl_Trans_Main.InvoiceID INNER JOIN Tbl_Users ON Tbl_Trans_Main.CashierID = Tbl_Users.UserID WHERE Tbl_Trans_Main.InvoiceID = @inv", clsS.myConnection))
            //using (SqlDataAdapter da = new SqlDataAdapter("select * from Tbl_Trans_Sub WHERE TransMainID = @inv", clsS.myConnection))
            //{
            //    da.SelectCommand.Parameters.Clear();
            //    da.SelectCommand.Parameters.AddWithValue("@inv", invNo);
            //    DataTable dt = new DataTable();
            //    da.Fill(dt);
            //    if (dt.Rows.Count > 0)
            //    {
            // clsS.CloseDB();
            // foreach (DataRow dtr in dt.Rows)
            foreach (var item in CurrentTransData.transitems)
            {

                //double price = Convert.ToDouble(dtr.Field<double>(3)) * Convert.ToDouble(dtr.Field<double>(4));
                //item_discount = item_discount + dtr.Field<double>(5);



                graphics.DrawString(item.itemName, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
                Offset = Offset + 15;
                graphics.DrawString("(" + item.itemPrice.ToString() + " x " + item.qty.ToString("N2") + ")", new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 190, startY + Offset, stringFormat);
                graphics.DrawString(item.amount.ToString("N2"), new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 280, startY + Offset, stringFormat);
                Offset = Offset + 15;
                x++;
            }
            //    }
            //}

            //  using (SqlDataAdapter da = new SqlDataAdapter("SELECT Tbl_Trans_Main.InvoiceID, Tbl_Trans_Main.TransType, Tbl_Trans_Main.Subtotal, Tbl_Trans_Main.Tax1, Tbl_Trans_Main.GrandTotal, Tbl_Trans_Main.SaleDate, Tbl_Trans_Main.SaleTime, Tbl_Trans_Main.CashAmount, Tbl_Trans_Main.CardAmount, Tbl_Trans_Main.CashChangeAmount, Tbl_Trans_Main.Paidby,Tbl_Trans_Main.invoiceDiscount,Tbl_Trans_Main.Paidby,Tbl_Trans_Main.TipAmount, Tbl_Trans_Main.CashChangeAmount FROM Tbl_Items INNER JOIN Tbl_Trans_Sub ON Tbl_Items.ItemID = Tbl_Trans_Sub.ItemID INNER JOIN Tbl_Trans_Main ON Tbl_Trans_Sub.TransMainID = Tbl_Trans_Main.InvoiceID INNER JOIN Tbl_Users ON Tbl_Trans_Main.CashierID = Tbl_Users.UserID WHERE Tbl_Trans_Main.InvoiceID = @inv", clsS.myConnection))
            //  {


            //     da.SelectCommand.Parameters.Clear();
            //     da.SelectCommand.Parameters.AddWithValue("@inv", invNo);
            //     DataTable dt = new DataTable();
            //     da.Fill(dt);
            //      if (dt.Rows.Count > 0)
            //     {


            graphics.DrawString(underLineSingle, new Font(bill_font, 7), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 15;



            graphics.DrawString("SUB TOTAL : ", new Font(bill_font, 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
            graphics.DrawString(CurrentTransData.MainData.subtotal.ToString("C", CultureInfo.CurrentCulture), new Font(bill_font, 12, FontStyle.Bold), new SolidBrush(Color.Black), 280, startY + Offset, stringFormat);
            Offset = Offset + 20;

            graphics.DrawString("TAX :", new Font(bill_font, 10, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
            graphics.DrawString(CurrentTransData.MainData.tax1.ToString("C", CultureInfo.CurrentCulture), new Font(bill_font, 10, FontStyle.Bold), new SolidBrush(Color.Black), 280, startY + Offset, stringFormat);
            Offset = Offset + 20;

            graphics.DrawString("DISCOUNT :",new Font(bill_font, 10, FontStyle.Bold),new SolidBrush(Color.Black), startX, startY + Offset);
            graphics.DrawString(CurrentTransData.MainData.invoiceDiscount.ToString("C", CultureInfo.CurrentCulture),new Font(bill_font, 10, FontStyle.Bold),new SolidBrush(Color.Black), 280, startY + Offset, stringFormat);

            Offset = Offset + 20;

            graphics.DrawString("GRAND TOTAL : ", new Font(bill_font, 10, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
            graphics.DrawString(CurrentTransData.MainData.grandTotal.ToString("C", CultureInfo.CurrentCulture), new Font(bill_font, 10, FontStyle.Bold), new SolidBrush(Color.Black), 280, startY + Offset, stringFormat);
            Offset = Offset + 20;

            graphics.DrawString("TIP : ", new Font(bill_font, 10, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
            graphics.DrawString(CurrentTransData.MainData.tipAmount.ToString("C", CultureInfo.CurrentCulture), new Font(bill_font, 10, FontStyle.Bold), new SolidBrush(Color.Black), 280, startY + Offset, stringFormat);
            Offset = Offset + 20;

            graphics.DrawString("PAID BY : ", new Font(bill_font, 10, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
            graphics.DrawString(CurrentTransData.MainData.paidby, new Font(bill_font, 10, FontStyle.Bold), new SolidBrush(Color.Black), 280, startY + Offset, stringFormat);
            Offset = Offset + 25;

            if (CurrentTransData.MainData.paidby == "CASH")
            {
                string change = CurrentTransData.MainData.cashChangeAmount.ToString("C", CultureInfo.CurrentCulture).Replace("-", "");
                graphics.DrawString("Cash Change : ", new Font(bill_font, 10, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
                graphics.DrawString(change, new Font(bill_font, 10, FontStyle.Bold), new SolidBrush(Color.Black), 280, startY + Offset, stringFormat);
                Offset = Offset + 25;
            }

            //   }
            //   }
            graphics.DrawString(underLine, new Font(bill_font, 8), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 15;

            //if (clsConnections.loyaltyactive == true)
            //{
            //    if (clsLoyalty.SelectedCustomer.SelectedCustomerID != "999")
            //    {

            //        graphics.DrawString("Loyalty Details", new Font(bill_font, 12, FontStyle.Bold), new SolidBrush(Color.Black), 10, Offset);
            //        Offset = Offset + 20;
            //        graphics.DrawString("Customer Name : ", new Font(bill_font, 10, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
            //        graphics.DrawString(clsLoyalty.SelectedCustomer.SelectedCustomerName, new Font(bill_font, 10, FontStyle.Bold), new SolidBrush(Color.Black), 140, startY + Offset);
            //        Offset = Offset + 18;

            //        string newpoints = clsLoyalty.SelectedCustomer.SelectedCustomerNewPoints.Trim(new Char[] { '"' });
            //        graphics.DrawString("Available Points : ", new Font(bill_font, 10, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
            //        graphics.DrawString(newpoints, new Font(bill_font, 10, FontStyle.Bold), new SolidBrush(Color.Black), 150, startY + Offset);
            //        Offset = Offset + 18;
            //        string cusid = "";

            //        try
            //        { cusid = clsLoyalty.SelectedCustomer.SelectedCustomerID.Substring(0, 9); }
            //        catch { }
            //        graphics.DrawString("CustomerID : ", new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
            //        graphics.DrawString(cusid + "xxx", new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 120, startY + Offset);
            //        Offset = Offset + 15;
            //        string storid = "";
            //        try
            //        { storid = clsConnections.thistoreid.Substring(0, 9); }
            //        catch { }
            //        graphics.DrawString("Visited StoreID :", new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
            //        graphics.DrawString(storid + "xxx", new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 120, startY + Offset);
            //        Offset = Offset + 15;
            //        graphics.DrawString("StoreGroupID :", new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
            //        graphics.DrawString(clsConnections.thisstoregroupid, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 120, startY + Offset);
            //        Offset = Offset + 15;
            //        graphics.DrawString(underLine, new Font(bill_font, 8), new SolidBrush(Color.Black), startX, startY + Offset);
            //        Offset = Offset + 15;

            //    }
            //}



            //clsS.OpenDB();
            //using (SqlDataAdapter da = new SqlDataAdapter("SELECT Tbl_Trans_Main.InvoiceID,Tbl_Trans_Main.CardAmount,Tbl_Trans_Main.CardHolder,Tbl_Trans_Main.CardNumber,Tbl_Trans_Main.CardType,Tbl_Trans_Main.retref,Tbl_Trans_Main.CardType,Tbl_Trans_Main.CardHolder,Tbl_Trans_Main.Entry_Method,Tbl_Trans_Main.Account_Type,Tbl_Trans_Main.AID,Tbl_Trans_Main.TCARQC from Tbl_Trans_Main WHERE Tbl_Trans_Main.InvoiceID = @inv_no and Tbl_Trans_Main.CardAmount>0", clsS.myConnection))
            //{
            //    da.SelectCommand.Parameters.Clear();
            //    da.SelectCommand.Parameters.AddWithValue("@inv_no", invNo);
            //    DataTable dt = new DataTable();
            //    da.Fill(dt);
            //    if (dt.Rows.Count > 0)
            //  {
            if (CurrentTransData.MainData.transType == "CARD" || CurrentTransData.MainData.transType == "MIXED")
            {
                graphics.DrawString("CARD TYPE : ", new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
                graphics.DrawString(CurrentTransData.MainData.cardType, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 120, startY + Offset);
                Offset = Offset + 15;

                graphics.DrawString("CARD USED : ", new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
                graphics.DrawString("XXXX XXXX XXXX " + CurrentTransData.MainData.cardNumber, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 120, startY + Offset);
                Offset = Offset + 15;

                if (CurrentTransData.MainData.cardType != "manual")
                {
                    graphics.DrawString("ACCOUNT : ", new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
                    graphics.DrawString(CurrentTransData.MainData.cardType, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 120, startY + Offset);
                    Offset = Offset + 15;

                    graphics.DrawString("CARD HOLDER : ", new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
                    graphics.DrawString(CurrentTransData.MainData.cardHolder, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 120, startY + Offset);
                    Offset = Offset + 15;

                    if (CurrentTransData.MainData.entryMethod == "Chip")
                    {
                        graphics.DrawString("AID : ", new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
                        graphics.DrawString(CurrentTransData.MainData.aid, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 120, startY + Offset);
                        Offset = Offset + 15;

                        graphics.DrawString("TC/ARQC : ", new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
                        graphics.DrawString(CurrentTransData.MainData.tcarqc, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 120, startY + Offset);
                        Offset = Offset + 15;
                    }
                    //if (CurrentTranData.giftbalance > 0)
                    //{
                    //    string gbalance = Properties.Settings.Default.Currency + CurrentTranData.giftbalance.ToString("N2").Replace("-", "");
                    //    graphics.DrawString("BALANCE : ", new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
                    //    graphics.DrawString(gbalance, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 120, startY + Offset);
                    //    Offset = Offset + 15;

                    //}

                }

                graphics.DrawString("ENTRY METHOD : ", new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
                graphics.DrawString(CurrentTransData.MainData.entryMethod, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 120, startY + Offset);
                Offset = Offset + 15;

                graphics.DrawString("AUTH ID : ", new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
                graphics.DrawString(CurrentTransData.MainData.href, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 120, startY + Offset);
                Offset = Offset + 20;

                if (CurrentTransData.giftcardbalance > 0)
                {
                    string changegift = CurrentTransData.giftcardbalance.ToString("C", CultureInfo.CurrentCulture).Replace("-", "");
                    graphics.DrawString("Gift Card Balance : ", new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
                    graphics.DrawString(changegift, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 120, startY + Offset);
                    Offset = Offset + 20;
                }

                graphics.DrawString(underLineSingle, new Font(bill_font, 7), new SolidBrush(Color.Black), startX, startY + Offset);
                Offset = Offset + 10;


                //graphics.DrawString("I agree to pay above total amount according to card ", new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
                //Offset = Offset + 15;
                //graphics.DrawString("issuer agreement.", new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
                //Offset = Offset + 20;


                //using (SqlDataAdapter myCommand = new SqlDataAdapter("select signature from Tbl_Signature where TransMainID= @inv_no", clsS.myConnection))
                //{
                //    myCommand.SelectCommand.Parameters.Clear();
                //    myCommand.SelectCommand.Parameters.AddWithValue("@inv_no", invNo);
                //    DataTable dtt = new DataTable();
                //    myCommand.Fill(dtt);
                //    if (dtt.Rows.Count > 0)
                //    {

                //        if (dtt.Rows[0][0] != System.DBNull.Value)
                //        {
                //            photo_aray = (byte[])dtt.Rows[0][0];
                //            MemoryStream ms = new MemoryStream(photo_aray);
                //            //pictureBox1.Image = Image.FromStream(ms);

                //            Image i = Image.FromStream(ms);
                //            if (i != null)
                //            {
                //                graphics.DrawImage(i, 110, startY + Offset, 150, 75);
                //            }
                //        }

                //    }
                //}

                Offset = Offset + 65;
                graphics.DrawString(underLineSingle, new Font(bill_font, 7), new SolidBrush(Color.Black), startX, startY + Offset);
                Offset = Offset + 5;
            }
            

            //  }
            //     clsS.CloseDB();
            //    }


            float f1 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(LoggedData.BusinessInfo.Footer1, new Font(bill_font, 8, FontStyle.Bold)).Width / 2);
                    float f2 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(LoggedData.BusinessInfo.Footer2, new Font(bill_font, 8, FontStyle.Bold)).Width / 2);
                    float f3 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(LoggedData.BusinessInfo.Footer3, new Font(bill_font, 8, FontStyle.Bold)).Width / 2);
                    float f4 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(LoggedData.BusinessInfo.Footer4, new Font(bill_font, 8, FontStyle.Bold)).Width / 2);

                    
                    if (LoggedData.BusinessInfo.Footer1 != string.Empty)
                    {
                        graphics.DrawString(LoggedData.BusinessInfo.Footer1, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), f1, startY + Offset);   //x6  startY   
                        Offset = Offset + 15;
                    }

                    if (LoggedData.BusinessInfo.Footer2 != string.Empty)
                    {
                        graphics.DrawString(LoggedData.BusinessInfo.Footer2, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), f2, startY + Offset);   //x6  startY   
                        Offset = Offset + 15;
                    }

                    if (LoggedData.BusinessInfo.Footer3 != string.Empty)
                    {
                        graphics.DrawString(LoggedData.BusinessInfo.Footer3, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), f3, startY + Offset);   //x6  startY   
                        Offset = Offset + 15;
                    }

                    if (LoggedData.BusinessInfo.Footer4 != string.Empty)
                    {
                        graphics.DrawString(LoggedData.BusinessInfo.Footer4, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), f4, startY + Offset);   //x6  startY   
                        Offset = Offset + 15;
                    }

                
          
               

            Offset = Offset + 15;
            graphics.DrawString("", new Font(bill_font, 8), new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 15;
            graphics.DrawString("", new Font(bill_font, 8), new SolidBrush(Color.Black), startX, startY + Offset);
            graphics.DrawString("Invoice ID : " + CurrentTransData.MainData.InvoiceUniqueId, new Font(bill_font, 6, FontStyle.Regular), new SolidBrush(Color.Black), x20, startY + Offset);
            //graphics.DrawString("REPRINT", new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), 200, startY + Offset);
            Offset = Offset + 20;

            Offset = Offset + 25;
            graphics.DrawString(".", new Font(bill_font, 8), new SolidBrush(Color.Black), startX, startY + Offset);
        }

        public static POSOrderModel ord = new POSOrderModel();
        public async static Task<bool> PrintMarketPlaceOrder(POSOrderModel m)
        {
           // if (localdata.isWindowPrinter)
            {
                ord = m;
                await printinWindows(m);
            }

          

            return true;


        }

        public static async Task<bool> printinWindows(POSOrderModel m)
        {
            // Use the Marketplace printer instead of the receipt printer
            string printerName = Properties.Settings.Default.MarketPrinter;

            // If MarketPrinter isn’t set, fall back to ReceiptPrinter
            if (string.IsNullOrWhiteSpace(printerName))
                printerName = Properties.Settings.Default.ReceiptPrinter;

            PrintDocument(printerName);
            return true;
        }


        public static void PrintDocument(string printerName)
        {

            // Create a PrintDocument object
            PrintDocument printDoc = new PrintDocument();

            // Set the printer name
            printDoc.PrinterSettings.PrinterName = printerName;

            // Handle the PrintPage event to define the content to print
            //printDoc.PrintPage += (sender, e) =>
            //{
            //    // Convert byte array to string (assuming UTF-8 encoding)
            //    string text = System.Text.Encoding.UTF8.GetString(content);

            //    // Define the font and layout
            //    using (System.Drawing.Font font = new System.Drawing.Font("Arial", 12))
            //    {
            //        // Set the printing area
            //        RectangleF printArea = e.PageSettings.PrintableArea;

            //        // Draw the string within the defined area
            //        e.Graphics.DrawString(text, font, Brushes.Black, printArea);
            //    }
            //};
            printDoc.PrintPage += new PrintPageEventHandler(pd_rcpt_inv_PrintPageUberEats);
            try
            {
                // Initiate the printing process
                printDoc.Print();
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during printing
                Console.WriteLine($"An error occurred while printing: {ex.Message}");
            }
            finally
            {
                // Detach the event handler to prevent memory leaks
                printDoc.PrintPage -= (sender, e) => { /* Event handler code */ };
            }

        }

       

        private static string AlignText(string leftText, string rightText, int lineWidth)
        {
            // Calculate available spaces between the two strings
            int spaceCount = lineWidth - (leftText.Length + rightText.Length);
            if (spaceCount < 0) spaceCount = 0; // Ensure no negative spacing

            // Create the aligned text with spaces
            return leftText + new string(' ', spaceCount) + rightText;
        }

        private static void pd_rcpt_inv_PrintPageUberEats(object sender, PrintPageEventArgs e)
        {

            string bill_font = "Arial";
            string invNo = ord.simpleorderid;
            Graphics graphics = e.Graphics;
            //e.HasMorePages = true;
            e.PageSettings.PaperSize = new PaperSize("Custom", 300, 10000);
            System.Drawing.Font font = new System.Drawing.Font(bill_font, 8);
            float fontHeight = font.GetHeight();
            int startX = 10;
            int startY = 0;
            string tp_no = "";
            int Offset = 0;
            String underLine = "===========================================";
            string underLineSingle = "----------------------------------------------------------------------------------------";

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Far;
            stringFormat.LineAlignment = StringAlignment.Near;

            Offset = Offset + 10;

            //Image i = Image.FromFile(Application.StartupPath + "\\ue.jpg");
            //if (i != null)
            //{
            //    graphics.DrawImage(i, 10, startY + Offset, 280, 105);
            //}
            if (ord.platform == "DoorDash")
            {
                graphics.DrawString("Door ", new System.Drawing.Font(bill_font, 40, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
                graphics.DrawString("dash ", new System.Drawing.Font(bill_font, 40, FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), startX + 130, startY + Offset);
                //Offset = Offset + 15;
                Offset = Offset + 65;
            }
            else if (ord.platform == "UberEats")
            {
                graphics.DrawString("Uber ", new System.Drawing.Font(bill_font, 40, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
                graphics.DrawString("Eats ", new System.Drawing.Font(bill_font, 40, FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), startX + 130, startY + Offset);
                //Offset = Offset + 15;
                Offset = Offset + 65;
            }
            else if (ord.platform == "GrubHub")
            {
                graphics.DrawString("Grub", new System.Drawing.Font(bill_font, 40, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
                graphics.DrawString("Hub", new System.Drawing.Font(bill_font, 40, FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), startX + 130, startY + Offset);
                //Offset = Offset + 15;
                Offset = Offset + 65;
            }
            else if (ord.platform == "Cart Pointe")
            {
                graphics.DrawString("Cart", new System.Drawing.Font(bill_font, 40, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
                graphics.DrawString("Point", new System.Drawing.Font(bill_font, 40, FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), startX + 130, startY + Offset);
                //Offset = Offset + 15;
                Offset = Offset + 65;
            }

          
            graphics.DrawString(underLine, new System.Drawing.Font(bill_font, 8), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 15;
            //Offset = Offset + 65;


            //graphics.DrawString("-- RE-PRINT --", new Font(bill_font, 10, FontStyle.Bold), new SolidBrush(Color.Black), x2, startY + Offset);
            //Offset = Offset + 20;

            // string lastname = ord.icustomer.LastName; 
            string lastname = "";
            string firstletter = "";
            if (lastname.Length > 1)
            {

                try
                { firstletter = lastname.Substring(0, 1); }
                catch { }
            }


            //graphics.DrawString(order.Store.Name, new Font(bill_font, 8, FontStyle.Regular), new SolidBrush(Color.Black), startX, startY + Offset);
            //Offset = Offset + 30;

            float x11 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(ord.simpleorderid, new System.Drawing.Font(bill_font, 20, FontStyle.Regular)).Width / 2);

            //graphics.DrawString( order.DisplayId, new Font(bill_font, 25, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
            //Offset = Offset + 30;

            graphics.DrawString(ord.simpleorderid, new System.Drawing.Font(bill_font, 20, FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), x11 - 20, startY + Offset);
            Offset = Offset + 40;

            float x1 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(ord.icustomer.FirstName, new System.Drawing.Font(bill_font, 20, FontStyle.Regular)).Width / 2);

            graphics.DrawString(ord.icustomer.FirstName + " " + firstletter + ".", new System.Drawing.Font(bill_font, 20, FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), x1 - 20, startY + Offset);
            Offset = Offset + 40;
            //graphics.DrawString("ADDRESS: " + order.Deliveries, new Font(bill_font, 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
            //Offset = Offset + 15;


            graphics.DrawString("Phone No: " + ord.icustomer.Phone, new System.Drawing.Font(bill_font, 12, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 25;

            string timeString = ord.orderedDatetime.ToString();
            DateTimeOffset dateTimeOffset = DateTimeOffset.Parse(timeString);
            DateTime dateTime = dateTimeOffset.DateTime;
            string formattedDateTime = dateTime.ToString("MMM d, yyyy h:mm tt");

            graphics.DrawString("Placed at " + formattedDateTime, new System.Drawing.Font(bill_font, 8, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 25;

            string timeString1 = ord.FullfilmentDateTime.ToString();
            DateTimeOffset dateTimeOffset1 = DateTimeOffset.Parse(timeString1);
            DateTime dateTime1 = dateTimeOffset1.DateTime;
            string formattedDateTime1 = dateTime.ToString("MMM d, yyyy h:mm tt");


            //graphics.DrawString("Due at " + formattedDateTime1, new Font(bill_font, 8, FontStyle.Regular), new SolidBrush(Color.Black), startX, startY + Offset);
            //Offset = Offset + 35;

            graphics.DrawString(underLine, new System.Drawing.Font(bill_font, 8), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 15;

            string deliverytype = ord.fulfillment_by;
            if (deliverytype == "DELIVERY_BY_UBER")
            {
                float x8 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString("DELIVERY", new System.Drawing.Font(bill_font, 20, FontStyle.Regular)).Width / 2);

                graphics.DrawString("DELIVERY", new System.Drawing.Font(bill_font, 20, FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), x8 - 20, startY + Offset);
                Offset = Offset + 40;
            }
            else if (deliverytype == "PICK_UP")
            {
                float x8 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString("PICKUP", new System.Drawing.Font(bill_font, 20, FontStyle.Regular)).Width / 2);

                graphics.DrawString("PICKUP", new System.Drawing.Font(bill_font, 20, FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), x8 - 20, startY + Offset);
                Offset = Offset + 40;
            }




            graphics.DrawString(underLine, new System.Drawing.Font(bill_font, 8), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 15;


            graphics.DrawString(underLineSingle, new System.Drawing.Font(bill_font, 7), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 25;

            //graphics.DrawString("ITEM", new Font(bill_font, 10, System.Drawing.FontStyle.Regular), new SolidBrush(Color.Black), startX, startY + Offset);
            ////graphics.DrawString("QTY", new Font(bill_font, 10, System.Drawing.FontStyle.Regular), new SolidBrush(Color.Black), 180, startY + Offset);
            //graphics.DrawString("PRICE", new Font(bill_font, 10, System.Drawing.FontStyle.Regular), new SolidBrush(Color.Black), 240, startY + Offset);
            //// graphics.DrawString("PRICE", new Font(bill_font, 8, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), 240, startY + Offset);
            //Offset = Offset + 25;
            //graphics.DrawString(underLineSingle, new Font(bill_font, 7), new SolidBrush(Color.Black), startX, startY + Offset);
            //Offset = Offset + 10;


            foreach (var obj in ord.iItems)
            {
                string itemname = obj.Name;
                int itemnamesize = 12;
                if (itemname.Length > 25)
                {
                    itemnamesize = 11;
                }
                Tuple<string, string> result4 = SplitStringIfLong(itemname);
                if (result4.Item2 != null) // If there's a second string, it was split
                {

                    graphics.DrawString(obj.Qty.ToString() + " X " + result4.Item1, new System.Drawing.Font(bill_font, itemnamesize, FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
                    Offset = Offset + 12;
                    graphics.DrawString("         " + result4.Item2, new System.Drawing.Font(bill_font, itemnamesize, FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);

                }
                else
                {
                    graphics.DrawString(obj.Qty.ToString() + " X " + obj.Name, new System.Drawing.Font(bill_font, itemnamesize, FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);

                }


                // graphics.DrawString(obj.Quantity.ToString() + " X " + "2 Pieces Fish over Rice Platter", new Font(bill_font, itemnamesize, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
                //Offset = Offset + 15;

                //graphics.DrawString("(" + dr.Cells["Column7"].Value.ToString() + " x " + dr.Cells["Column8"].Value.ToString() + ")", new Font(bill_font, 8, FontStyle.Regular), new SolidBrush(Color.Black), 220, startY + Offset, stringFormat);
                //Offset = Offset + 15;
                graphics.DrawString(obj.Price.ToString(), new System.Drawing.Font(bill_font, 10, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), 280, startY + Offset, stringFormat);
                Offset = Offset + 30;

                if (obj.Modifers != null)
                {
                    foreach (var obj1 in obj.Modifers)
                    {

                        //graphics.DrawString("      " + obj1.GroupName, new System.Drawing.Font(bill_font, 10, FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
                        Offset = Offset + 12;






                        //graphics.DrawString("(" + dr.Cells["Column7"].Value.ToString() + " x " + dr.Cells["Column8"].Value.ToString() + ")", new Font(bill_font, 8, FontStyle.Regular), new SolidBrush(Color.Black), 220, startY + Offset, stringFormat);
                        //Offset = Offset + 15;
                        //graphics.DrawString(obj1.Quantity, new Font(bill_font, 10, FontStyle.Regular), new SolidBrush(Color.Black), 280, startY + Offset, stringFormat);
                        //Offset = Offset + 25;

                        if (obj1.ModiferOptions != null)
                        {
                            foreach (var obj2 in obj1.ModiferOptions)
                            {
                                string modname = obj2.Name;
                                Tuple<string, string> result = SplitStringIfLong(modname);
                                if (result.Item2 != null) // If there's a second string, it was split
                                {
                                    graphics.DrawString("    " + obj2.Qty.ToString() + " X " + result.Item1, new System.Drawing.Font(bill_font, 11, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
                                    Offset = Offset + 12;
                                    graphics.DrawString("        " + result.Item2, new System.Drawing.Font(bill_font, 11, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
                                    Offset = Offset + 12;
                                }
                                else
                                {
                                    graphics.DrawString("    " + obj2.Qty.ToString() + " X " + result.Item1, new System.Drawing.Font(bill_font, 11, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
                                    Offset = Offset + 12;
                                }



                                //Offset = Offset + 15;

                                //graphics.DrawString("(" + dr.Cells["Column7"].Value.ToString() + " x " + dr.Cells["Column8"].Value.ToString() + ")", new Font(bill_font, 8, FontStyle.Regular), new SolidBrush(Color.Black), 220, startY + Offset, stringFormat);
                                //Offset = Offset + 15;

                                //double ans = Convert.ToDouble(obj2.Price * Convert.ToDouble(obj.Qty));
                                //graphics.DrawString("$" + ans.ToString(), new System.Drawing.Font(bill_font, 11, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), 280, startY + Offset, stringFormat); ;
                                //Offset = Offset + 25;
                                //if (obj2. != null)
                                //{
                                //    foreach (var obj3 in obj2.selected_modifier_groups)
                                //    {
                                //        if (obj3 != null)
                                //        {
                                //            foreach (var obj4 in obj3.SelectedItems)
                                //            {

                                //                graphics.DrawString("         " + obj4.Quantity.ToString() + " X " + obj4.Title, new System.Drawing.Font(bill_font, 11, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + Offset);
                                //                double ansb = Convert.ToDouble(obj4.Price.BaseTotalPrice.FormattedAmount.Replace("$", " ")) * Convert.ToDouble(obj4.Quantity);
                                //                graphics.DrawString("$" + ansb.ToString(), new System.Drawing.Font(bill_font, 11, FontStyle.Regular), new SolidBrush(Color.Black), 280, startY + Offset, stringFormat); ;
                                //                Offset = Offset + 25;

                                //            }


                                //        }




                                //    }
                                //}

                            }
                        }

                    }
                }



                if (obj.Special_instructions != null && obj.Special_instructions != string.Empty)
                {
                    graphics.DrawString(underLineSingle, new System.Drawing.Font(bill_font, 7), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
                    Offset = Offset + 25;
                    graphics.DrawString("Customer Note:", new System.Drawing.Font(bill_font, 9, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
                    Offset = Offset + 20;
                    try
                    {
                        string note = "";
                        if (obj.Special_instructions != null)
                        {
                            note = obj.Special_instructions.ToString();
                        }
                        string str = "";
                        if (note.Length > 25)
                        {
                            str = note.Substring(0, 25);

                        }
                        else
                        {
                            str = note;
                        }

                        graphics.DrawString(str, new System.Drawing.Font(bill_font, 9, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
                        Offset = Offset + 20;
                        graphics.DrawString(underLineSingle, new System.Drawing.Font(bill_font, 7), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
                        Offset = Offset + 25;
                    }
                    catch
                    { }
                }


            }

            //foreach (DataGridViewRow dr in dataGridView1.Rows)
            //{
            //    graphics.DrawString(dr.Cells["Column6"].Value.ToString(), new Font(bill_font, 10, FontStyle.Regular), new SolidBrush(Color.Black), startX, startY + Offset);
            //    Offset = Offset + 15;

            //    //graphics.DrawString("(" + dr.Cells["Column7"].Value.ToString() + " x " + dr.Cells["Column8"].Value.ToString() + ")", new Font(bill_font, 8, FontStyle.Regular), new SolidBrush(Color.Black), 220, startY + Offset, stringFormat);
            //    //Offset = Offset + 15;
            //    graphics.DrawString(dr.Cells["Column9"].Value.ToString(), new Font(bill_font, 10, FontStyle.Regular), new SolidBrush(Color.Black), 280, startY + Offset, stringFormat);
            //    Offset = Offset + 25;

            //}

            graphics.DrawString(underLineSingle, new System.Drawing.Font(bill_font, 7), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 15;

            graphics.DrawString("Sub Total : ", new System.Drawing.Font(bill_font, 12, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            graphics.DrawString((ord.iTotals.SubTotal ?? 0).ToString("N2"), new System.Drawing.Font(bill_font, 12, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), 280, startY + Offset, stringFormat);
            Offset = Offset + 20;

            graphics.DrawString("Tax :", new System.Drawing.Font(bill_font, 10, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            graphics.DrawString((ord.iTotals.TaxTotal ?? 0).ToString("N2"), new System.Drawing.Font(bill_font, 10, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), 280, startY + Offset, stringFormat);
            Offset = Offset + 20;

            graphics.DrawString("Discount : ", new System.Drawing.Font(bill_font, 10, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            graphics.DrawString((ord.iTotals.Discount ?? 0).ToString("N2"), new System.Drawing.Font(bill_font, 10, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), 280, startY + Offset, stringFormat);
            Offset = Offset + 20;

            graphics.DrawString("Grand Total : ", new System.Drawing.Font(bill_font, 10, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            graphics.DrawString((ord.iTotals.GrandTotal).ToString("N2"), new System.Drawing.Font(bill_font, 10, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), 280, startY + Offset, stringFormat);
            Offset = Offset + 20;

            graphics.DrawString("Tip : ", new System.Drawing.Font(bill_font, 10, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            graphics.DrawString((ord.iTotals.Tip ?? 0).ToString("N2"), new System.Drawing.Font(bill_font, 10, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), 280, startY + Offset, stringFormat);
            Offset = Offset + 20;

            graphics.DrawString(underLine, new System.Drawing.Font(bill_font, 8), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 15;


            graphics.DrawString(underLineSingle, new System.Drawing.Font(bill_font, 7), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 25;


            graphics.DrawString("Customer Note:", new System.Drawing.Font(bill_font, 9, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 20;
            try
            {
                string note = "";
                if (ord.order_special_instructions != null)
                {
                    note = ord.order_special_instructions.ToString();
                }
                string str = "";
                if (note.Length > 25)
                {
                    str = note.Substring(0, 25);

                }
                else
                {
                    str = note;
                }

                graphics.DrawString(str, new System.Drawing.Font(bill_font, 9, FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
                Offset = Offset + 20;
            }
            catch
            { }


            try
            {
                if (ord.is_plastic_ware_option_selected != null)
                {
                    if (ord.is_plastic_ware_option_selected == true)
                    {
                        graphics.DrawString("Disposable Items : ", new System.Drawing.Font(bill_font, 10, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
                        graphics.DrawString("SHOULD INCLUDE", new System.Drawing.Font(bill_font, 10, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), 280, startY + Offset, stringFormat);
                        Offset = Offset + 20;
                    }
                    else if (ord.is_plastic_ware_option_selected == false || ord.is_plastic_ware_option_selected == null)
                    {
                        graphics.DrawString("Disposable Items : ", new System.Drawing.Font(bill_font, 10, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
                        graphics.DrawString("DO NOT INCLUDE", new System.Drawing.Font(bill_font, 10, FontStyle.Regular), new SolidBrush(System.Drawing.Color.Black), 280, startY + Offset, stringFormat);
                        Offset = Offset + 20;
                    }
                }


            }

            catch
            {

            }

            Offset = Offset + 15;
            float f1 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString("Thank you for purchasing from us", new System.Drawing.Font(bill_font, 8, FontStyle.Regular)).Width / 2);

            graphics.DrawString("Thank you for purchasing from us", new System.Drawing.Font(bill_font, 8), new SolidBrush(System.Drawing.Color.Black), f1, startY + Offset);
            Offset = Offset + 25;

            float f20 = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString("Software by POSpointe (www.pospointe.com)", new System.Drawing.Font(bill_font, 8, FontStyle.Regular)).Width / 2);

            graphics.DrawString("Software by POSpointe (www.pospointe.com)", new System.Drawing.Font(bill_font, 8), new SolidBrush(System.Drawing.Color.Black), f20, startY + Offset);
            Offset = Offset + 45;


            graphics.DrawString(".", new System.Drawing.Font(bill_font, 8), new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);

        }





        static Tuple<string, string> SplitStringIfLong(string input)
        {
            if (input.Length > 23)
            {
                int midpoint = input.Length / 2;
                string firstHalf = input.Substring(0, midpoint);
                string secondHalf = input.Substring(midpoint);
                return Tuple.Create(firstHalf, secondHalf);
            }
            return Tuple.Create(input, (string)null); // Use Tuple.Create to return a tuple
        }



    }

}



