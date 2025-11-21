using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Pospointe.LocalData;
using Pospointe.Models;
using Pospointe.Reports;
using Pospointe.Trans_Api;
using RestSharp;
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

namespace Pospointe.MainMenu
{
    /// <summary>
    /// Interaction logic for FrmShiftClose.xaml
    /// </summary>
    public partial class FrmShiftClose : Window
    {
        public Root _dayopen { get; set; }

        public double totalamount = 0;

        public double salestotal =0;

        public double expectedamount = 0;

        public double actualamount = 0;

        public double difference = 0;
        public FrmShiftClose()
        {
            InitializeComponent();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnShiftEnd_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBoxResult result = MessageBox.Show("Are you sure you want to end the shift?",
                                              "Confirm",
                                              MessageBoxButton.YesNo,
                                              MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                shiftcloserequest ss = new shiftcloserequest();
                TblDayOpenCashCollection ca = new TblDayOpenCashCollection();
                ss.closingbalance = Convert.ToDecimal(totalamount);
                ss.deference = Convert.ToDecimal(difference);
                ss.closingtime = DateTime.Now;
                ss.closingdatetime = DateTime.Now;
                ca.Note100 = SafeToInt(TxtNote100.Text);
                ca.Note50 = SafeToInt(TxtNote50.Text);
                ca.Note20 = SafeToInt(TxtNote20.Text);
                ca.Note10 = SafeToInt(TxtNote10.Text);
                ca.Note5 = SafeToInt(TxtNote5.Text);
                ca.Note1 = SafeToInt(TxtNote1.Text);
                ca.Coin50 = SafeToInt(TxtCent50.Text);
                ca.Coin25 = SafeToInt(TxtCent25.Text);
                ca.Coin10 = SafeToInt(TxtCent10.Text);
                ca.Coin5 = SafeToInt(TxtCent5.Text);
                ca.Coin1 = SafeToInt(TxtCent1.Text);
                ss.cls = ca;
                var json7 = JsonConvert.SerializeObject(ss);




                var options = new RestClientOptions(clsConnections.myposapiurl)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest($"/Shift/shiftclose/{_dayopen.dayOpenId}", Method.Post);
                request.AddHeader("db", clsConnections.mydb);
                request.AddHeader("Authorization", clsConnections.transserverauth);
                request.AddParameter("application/json", json7, ParameterType.RequestBody);
                RestResponse response =  client.Execute(request);
                if (response.IsSuccessful)
                {
                    FrmCustommessage msg = new FrmCustommessage();
                    msg.IsError = true;
                    msg.LblHeader.Text = "Success";
                    msg.LblMessage.Text = $"Shift Closed Successfully";
                    msg.ShowDialog();

                    //MessageBox.Show("Shift Closed Successfully");
                    //REPORT HERE
                    ViewReportAsync(_dayopen.dayOpenId ?? 0);
                    this.DialogResult = true;
                    this.Close();
                }
                else {

                    MessageBox.Show(response.Content);                
                }
            }
            else
            {
                // Code to handle the 'No' option (optional)
            }

        }

        private int SafeToInt(string text)
        {
            return int.TryParse(text, out int result) ? result : 0;
        }


        private async void ViewReportAsync(int openid)
        {
            var options = new RestClientOptions(clsConnections.myposapiurl)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest($"/Reports/shiftclose/report/{openid}", Method.Get);
            request.AddHeader("db", clsConnections.mydb);
            RestResponse response = await client.ExecuteAsync(request);
            Console.WriteLine(response.Content);
            if (response.IsSuccessful)
            {
                var objResponse1 = JsonConvert.DeserializeObject<Shiftcls>(response.Content);
                FrmShiftSumm rppage = new FrmShiftSumm();
                rppage.shiftrp = objResponse1;
                var result = rppage.ShowDialog();


            }

            else
            {
              //  MessageBox.Show(response.StatusDescription);
                MessageBox.Show(response.Content);
            }
        }

        private void caltotal()
        {
            double note100 = ParseTextToDouble(TxtNote100.Text) * 100;
            double note50 = ParseTextToDouble(TxtNote50.Text) * 50;
            double note20 = ParseTextToDouble(TxtNote20.Text) * 20;
            double note10 = ParseTextToDouble(TxtNote10.Text) * 10;
            double note5 = ParseTextToDouble(TxtNote5.Text) * 5;
            double note1 = ParseTextToDouble(TxtNote1.Text) * 1;

            double coin50 = ParseTextToDouble(TxtCent50.Text) * 0.50;
            double coin25 = ParseTextToDouble(TxtCent25.Text) * 0.25;
            double coin10 = ParseTextToDouble(TxtCent10.Text) * 0.10;
            double coin5 = ParseTextToDouble(TxtCent5.Text) * 0.05;
            double coin1 = ParseTextToDouble(TxtCent1.Text) * 0.01;

            double notesTotal = note100 + note50 + note20 + note10 + note5 + note1;
            double coinsTotal = coin50 + coin25 + coin10 + coin5 + coin1;

            totalamount = notesTotal + coinsTotal;
            actualamount = totalamount;

            TxtActualTotal.Text = actualamount.ToString("C");
            TxtTotalNotes.Text = notesTotal.ToString("C");
            TxtTotalCoins.Text = coinsTotal.ToString("C");

            difference = expectedamount - actualamount;
            if (difference < 0)
            {
                TxtDifferent.Text = $"Cash Over by {Math.Abs(difference):C}";
                TxtDifferent.Foreground = Brushes.Green;
            }
            else if (difference > 0)
            {
                TxtDifferent.Text = $"Cash Short by {difference:C}";
                TxtDifferent.Foreground = Brushes.Red;
            }
            else
            {
                TxtDifferent.Text = "Perfect Match";
                TxtDifferent.Foreground = Brushes.Black;
            }

        }

        private double ParseTextToDouble(string text)
        {
            return double.TryParse(text, out double result) ? result : 0;
        }


        private void TxtNote_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Check if the input is a number (using a regular expression to allow digits only)
            e.Handled = !char.IsDigit(e.Text, 0);
        }

        private void TxtNote_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "0";  // Reset to "0" if empty on blur
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
           await getsalesforthisshift();
           TxtLoggeduser.Text = LoggedData.loggeduser.UserName;
           TxtStartedat.Text = _dayopen.openedDateTime.ToString("MM/dd/yyyy hh:mm tt");
           TxtStartedcash.Text = _dayopen.openingBalance.ToString("C");
           TxtActualTotal.Text = "$0.00";
            TxtDifferent.Text = "$0.00";
            expectedamount = _dayopen.openingBalance + salestotal;
            TxtExpected.Text = expectedamount.ToString("C");
            TxtCent1.Text = "0";
            TxtCent10.Text = "0";
            TxtCent5.Text = "0";
            TxtCent25.Text = "0";
            TxtCent50.Text = "0";

            TxtNote100.Text = "0";
            TxtNote50.Text = "0";
            TxtNote20.Text = "0";
            TxtNote10.Text = "0";
            TxtNote5.Text = "0";
            TxtNote1.Text = "0";    
        }

        //private void TxtNote_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    TextBox textBox = sender as TextBox; // The field that triggered this
        //    FrmNumbpad numpad = new FrmNumbpad();

        //    if (numpad.ShowDialog() == true) // Show the number pad
        //    {
        //        // Set the entered value into the TextBox
        //        textBox.Text = numpad.TxtEnterAmount.Text;

        //        // Trigger calculation immediately
        //        caltotal();
        //    }
        //}

        private TextBox _activeTextBox;

        private void TxtNote_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            _activeTextBox = textBox;  // Track active textbox

            if (textBox != null)
            {
                if (textBox.Text == "0")
                {
                    textBox.Clear();  
                }
                else
                {
                    textBox.SelectAll();  // Select text if not "0"
                }
            }
        }




        public async Task getsalesforthisshift()
        {

            shiftclosecashrequest root = new shiftclosecashrequest();
            root.stationid = LoggedData.StationID;
            root.cashierid = _dayopen.cashierId;
            root.starttime = _dayopen.openedDateTime;
            root.endtime = DateTime.Now;
            var json = JsonConvert.SerializeObject(root);

            var options = new RestClientOptions(LoggedData.transbaseurl)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/OtherTransactions/shiftclosecashsales", Method.Post);
            request.AddHeader("db", clsConnections.mydb);
            request.AddHeader("Authorization", clsConnections.transserverauth);
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            RestResponse response = await client.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                shiftclosecashreponse myDeserializedClass = JsonConvert.DeserializeObject<shiftclosecashreponse>(response.Content);
                //if (myDeserializedClass != null)
                //{
                //    MessageBox.Show(response.Content);
                //}
                salestotal = myDeserializedClass.cashsales  ?? 0;
                TxtSales.Text = salestotal .ToString("C");

            }
            else
            {

                MessageBox.Show($"Error Code : {response.StatusCode} Description : {response.Content}");
            }

        }


        private void ShowCustomMessage(string header, string message, bool isError)
        {
            FrmCustommessage frmCustommessage = new FrmCustommessage
            {
                LblHeader = { Text = header },
                LblMessage = { Text = message },
                IsError = isError
            };
            frmCustommessage.ShowDialog();
        }

        public class shiftcloserequest
        {
            public decimal closingbalance { get; set; }
            public decimal deference { get; set; }

            public DateTime closingtime { get; set; }

            public DateTime closingdatetime { get; set; }

            public TblDayOpenCashCollection cls { get; set; }

        }

        public class shiftclosecashrequest
        {
            public string cashierid { get; set; }
            public string stationid { get; set; }
            public DateTime starttime { get; set; }
            public DateTime endtime { get; set; }

        }

        public class shiftclosecashreponse
        {
            public double? cashsales { get; set; }


        }

        public partial class TblDayOpenCashCollection
        {
            public int Id { get; set; }

            public int? DayOpenId { get; set; }

            public string Type { get; set; }

            public int? Note100 { get; set; }

            public int? Note50 { get; set; }

            public int? Note20 { get; set; }

            public int? Note10 { get; set; }

            public int? Note5 { get; set; }

            public int? Note1 { get; set; }

            public int? Coin50 { get; set; }

            public int? Coin25 { get; set; }

            public int? Coin10 { get; set; }

            public int? Coin5 { get; set; }

            public int? Coin1 { get; set; }
        }

        public class Root
        {
            public int? dayOpenId { get; set; }
            public string cashierId { get; set; }
            public DateTime? date { get; set; }
            public double openingBalance { get; set; }
            public double? closingBalance { get; set; }
            public double? deference { get; set; }
            public string dayOpeningTime { get; set; }
            public object dayClosingTime { get; set; }
            public string status { get; set; }
            public DateTime openedDateTime { get; set; }
            public object closedDateTime { get; set; }
        }

        private void TextChanged(object sender, RoutedEventArgs e)
        {
            caltotal();
        }

        private void BtnNuum_Click(object sender, RoutedEventArgs e)
        {
            if (_activeTextBox == null) return; // Ensure there's an active TextBox

            Button clickedButton = sender as Button;
            string value = clickedButton.Content.ToString();

            // Append the number unless it's "CANCEL"
            if (value == "CANCEL")
            {
                _activeTextBox.Text = string.Empty;
            }
            else
            {
                _activeTextBox.Text += value; // Append the number
            }

            caltotal(); // Recalculate totals after input changes
        }


        private void Btndelete_Click(object sender, RoutedEventArgs e)
        {
            if (_activeTextBox != null && !string.IsNullOrEmpty(_activeTextBox.Text))
            {
                _activeTextBox.Text = _activeTextBox.Text.Length > 1
                    ? _activeTextBox.Text.Substring(0, _activeTextBox.Text.Length - 1)
                    : string.Empty;

                caltotal();
            }

            // If there's no text or no active textbox, do nothing (prevents crashes)
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();        }





        //private void TxtCent50_LostFocus(object sender, RoutedEventArgs e)
        //{

        //}
    }
}
