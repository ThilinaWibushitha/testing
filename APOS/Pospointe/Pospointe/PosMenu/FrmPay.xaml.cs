using Newtonsoft.Json;
using POSLink2.Const;
using Pospointe.GuftCard;
using Pospointe.LocalData;
using Pospointe.Properties;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
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
using System.Threading.Tasks;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Pospointe.PosMenu
{
    /// <summary>
    /// Interaction logic for FrmPay.xaml
    /// </summary>
    public partial class FrmPay : Window
    {
        public decimal amountopay { get; set; }

        public decimal remainingAmount { get; set; }

        public decimal cashamount { get; set; }
        public decimal cardamount { get; set; }
        public decimal changeamount { get; set; }
        public FrmPay()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                remainingAmount = amountopay;
                LblBalanceAmount.Text = amountopay.ToString("0.00");
                TxtPayamont.Text = amountopay.ToString("0.00");
                TxtPayamont.Focus();
                UpdateBtncus();
            }
            catch (Exception ex)
            {
                ShowCustomMessage(
                    "Payment Window Error",                                   
                    $"An error occurred while loading the payment window:\n{ex.Message}", 
                    true                                                     
                );
            }


        }


        private void NumericButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button)
                {
                    string content = button.Content.ToString();

                    if (TxtPayamont.Text == amountopay.ToString("0.00"))
                    {
                        TxtPayamont.Text = "";
                    }

                    if (content == "." && TxtPayamont.Text.Contains("."))
                    {
                        ShowCustomMessage(
                            "Invalid Input",                                 
                            "You cannot enter more than one decimal point.", 
                            true                                             
                        );
                        return;
                    }


                    if (TxtPayamont.Text == "0" && content != ".")
                    {
                        TxtPayamont.Text = content;
                    }
                    else
                    {
                        TxtPayamont.Text += content;
                    }

                    TxtPayamont.CaretIndex = TxtPayamont.Text.Length;
                }
                
            }
            catch (Exception ex)
            {
                ShowCustomMessage(
                    "Amount Entry Error",                            
                    $"An error occurred while entering the amount:\n{ex.Message}", 
                    true                                              
                );
            }

        }


        private void UpdateBtncus()
        {
            try
            {
                decimal rounded = remainingAmount > 0 ? Math.Ceiling(remainingAmount) : 0;
                Btncus.Content = $"${rounded}";
            }
            catch
            {
                Btncus.Content = "$0";
            }
        }


        private void Btncus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (amountopay < 0)
                {
                    ShowCustomMessage(
                        "Invalid Amount",                                        
                        "Please enter a valid amount before proceeding.",        
                        true                                                    
                    );
                    return;
                }


                decimal roundedAmount = Math.Ceiling(remainingAmount);


                BtncusText.Text = $"${roundedAmount}";
                TxtPayamont.Text = roundedAmount.ToString("0.00");
                TxtPayamont.CaretIndex = TxtPayamont.Text.Length;
                ProcessCashPayment(roundedAmount);
            }
            catch (Exception ex)
            {
                ShowCustomMessage(
                    "Custom Amount Error",                             
                    $"An error occurred while processing the custom amount:\n{ex.Message}", 
                    true                                              
                );
            }

        }



        private void TxtPayamont_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TxtPayamont != null && !string.IsNullOrEmpty(TxtPayamont.Text))
                {
                    TxtPayamont.SelectAll();
                }
            }
            catch (Exception ex)
            {
                ShowCustomMessage(
                    "Focus Error",                                         
                    $"An error occurred while focusing on the payment amount field:\n{ex.Message}",
                    true                                                  
                );
            }

        }



        private void BtnDlt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(TxtPayamont.Text))
                {
                    TxtPayamont.Text = TxtPayamont.Text.Substring(0, TxtPayamont.Text.Length - 1);
                    TxtPayamont.CaretIndex = TxtPayamont.Text.Length;
                }
            }
            catch (Exception ex)
            {
                ShowCustomMessage(
                    "Deletion Error",                                        
                    $"An error occurred while deleting a character:\n{ex.Message}", 
                    true                                                    
                );
            }

        }


        private void TxtPayamont_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                if (!char.IsDigit(e.Text, 0) && e.Text != "." && e.Text != "-")
                {
                    e.Handled = true;
                    return;
                }

                if (e.Text == "." && TxtPayamont.Text.Contains("."))
                {
                    e.Handled = true;
                    return;
                }

                if (e.Text == "-" && (TxtPayamont.Text.Contains("-") || TxtPayamont.CaretIndex != 0))
                {
                    e.Handled = true;
                    return;
                }

                e.Handled = false;
            }
            catch (Exception ex)
            {
                ShowCustomMessage(
                    "Amount Entry Error",
                    $"An error occurred while entering the amount:\n{ex.Message}",
                    true
                );
                e.Handled = true;
            }
        }



        private void BtnCash_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!decimal.TryParse(TxtPayamont.Text, out decimal payingAmount))
                {
                    ShowCustomMessage(
                        "Invalid Amount",
                        "\"Invalid amount entered. Please enter a valid payment amount.\"",
                        true
                        );
                    return; 
                }
                //if (payingAmount == 0)
                //{
                //    ShowCustomMessage(
                //        "Invalid Payment Amount",
                //        "The payment amount must be greater or less than zero.",
                //        true
                //    );
                //    return;
                //}
                ProcessCashPayment(payingAmount);
            }
            catch (Exception ex)
            {
                ShowCustomMessage(
                    "Payment Processing Error",
                    $"An unexpected error occurred while processing the payment:\n{ex.Message}",
                    true
                );
            }
        }

        private void ProcessCashPayment(decimal payingAmount)
        {
            try
            {
                if (payingAmount < 0)
                {
                    // Handle cash return
                    decimal returnAmount = Math.Abs(payingAmount);
                    cashamount -= returnAmount; 
                    changeamount = 0;
                    cardamount = 0;

                    CurrentTransData.MainData.cashAmount = Convert.ToDouble(cashamount);
                    CurrentTransData.MainData.cardAmount = Convert.ToDouble(cardamount);
                    CurrentTransData.MainData.cashChangeAmount = Convert.ToDouble(changeamount);
                    CurrentTransData.MainData.paidby = "CASH";
                    CurrentTransData.MainData.transType = "RETURN";

                    ShowCustomMessage(
                        "Return Processed",
                        $"Cash return processed: ${returnAmount:0.00}",
                        false
                    );

                    this.DialogResult = true;
                    this.Close();
                    return;
                }


                if (payingAmount >= remainingAmount)
                {
                    cashamount += remainingAmount;
                    changeamount = payingAmount - remainingAmount;
                    cardamount = 0;
                    CurrentTransData.MainData.cashAmount = Convert.ToDouble(cashamount);
                    CurrentTransData.MainData.cardAmount = Convert.ToDouble(cardamount);
                    CurrentTransData.MainData.cashChangeAmount = Convert.ToDouble(changeamount);

                    //ShowCustomMessage(
                    //    "Payment Successful",
                    //    $"Payment completed successfully.\nChange to be returned: ${changeamount:0.00}",
                    //    false
                    //);

                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    cashamount += payingAmount;
                    remainingAmount -= payingAmount;
                    CurrentTransData.MainData.cashAmount = Convert.ToDouble(cashamount);
                    LblBalanceAmount.Text = remainingAmount.ToString("0.00");
                    TxtPayamont.Text = remainingAmount.ToString("0.00");

                    ShowCustomMessage(
                        "Partial Payment Accepted",
                        $"${payingAmount:0.00} paid in cash.\nRemaining balance: ${remainingAmount:0.00}",
                        false
                    );
                }
            }
            catch (Exception ex)
            {
                ShowCustomMessage(
                    "Cash Payment Error",
                    $"An error occurred during the cash payment process:\n{ex.Message}",
                    true
                );
            }
        }

        private void BtnCard_Click(object sender, RoutedEventArgs e)
        {
            decimal payingAmount = Convert.ToDecimal(TxtPayamont.Text);

            //if (payingAmount <= 0 || payingAmount > remainingAmount)
            //{
            //    ShowCustomMessage(
            //        "Invalid Payment Amount", // Header
            //        "Ensure the amount is greater than zero and does not exceed the remaining balance.", // Message
            //        true 
            //    );

            //    return;
            //}

            if (payingAmount > remainingAmount)
            {
                ShowCustomMessage(
                    "Invalid Card Amount",
                    $"Card payment cannot exceed the remaining balance (${remainingAmount:0.00}).",
                    true
                );
                TxtPayamont.Text = remainingAmount.ToString("0.00");
                TxtPayamont.Focus();
                return;
            }

            if (LoggedData.comtype == "PAX_IP" || LoggedData.comtype == "PAX_USB")
            {

                FrmPaxscreen pax = new FrmPaxscreen();
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

                if (TxtPayamont.Text.Contains("-"))
                {
                    pax.amount = (Convert.ToDecimal(TxtPayamont.Text) * 100).ToString("0");
                    pax.amount = pax.amount.Replace("-", "");
                    pax.transtyperequest = "RETURN";
                }
                else
                {
                    pax.amount = (Convert.ToDecimal(TxtPayamont.Text) * 100).ToString("0");
                    pax.transtyperequest = "SALE";
                }
                pax.ecrref = CurrentTransData.MainData.InvoiceIdshortCode;






                var result = pax.ShowDialog();

                if (result == true && pax.responsecode == "000000")
                {
                    decimal payingamont = Convert.ToDecimal(TxtPayamont.Text);
                    decimal tipAmount = 0;

                    if (!string.IsNullOrEmpty(pax.TipAmount))
                    {
                        tipAmount = Convert.ToDecimal(pax.TipAmount);
                        if (tipAmount > 10) 
                            tipAmount /= 100;
                    }

                    cardamount += payingamont;
                    remainingAmount -= payingamont;

                    CurrentTransData.MainData.cashAmount = Convert.ToDouble(cashamount);
                    CurrentTransData.MainData.cardAmount = Convert.ToDouble(cardamount);
                    CurrentTransData.MainData.cashChangeAmount = 0;

                    bool fullyPaid = remainingAmount <= 0;

                    if (Properties.Settings.Default.ShowTipPrompt && tipAmount > 0)
                    {
                        ShowCustomMessage(
                            "Tip Added",
                            $"Customer added a tip of ${tipAmount:0.00}.",
                            false
                        );
                    }


                    if (remainingAmount <= 0)
                    {
                        try
                        {
                            this.DialogResult = true;
                        }
                        catch (InvalidOperationException)
                        {
                            
                        }

                        this.Close();
                        return;
                    }

                    else
                    {
                        ShowCustomMessage(
                            "Partial Payment Processed",
                            $"${payingamont:0.00} paid via card.\nRemaining balance: ${remainingAmount:0.00}",
                            false
                        );

                        LblBalanceAmount.Text = remainingAmount.ToString("0.00");
                        TxtPayamont.Text = remainingAmount.ToString("0.00");
                        TxtPayamont.Focus();
                    }

                }
                else
                {
                    ShowCustomMessage(
                        "Card Payment Failed",
                        $"Response: {pax.responsecode} - {pax.responseMsg}",
                        true
                    );
                }


            }

            else if (LoggedData.comtype == "Manual Express")
            {

                decimal payingamont = Convert.ToDecimal(TxtPayamont.Text);
                
                cardamount += payingamont;
                remainingAmount -= payingamont;

                CurrentTransData.MainData.cashAmount = Convert.ToDouble(cashamount);
                CurrentTransData.MainData.cardAmount = Convert.ToDouble(cardamount);
                CurrentTransData.MainData.cashChangeAmount = 0;

                if (remainingAmount <= 0)
                {
                    //ShowCustomMessage(
                    //    "Payment Successful",
                    //    "Card payment successful.\nTotal amount fully paid.",
                    //    false
                    //);

                    this.DialogResult = true;
                    this.Close();
                }

                else
                {
                    ShowCustomMessage(
                        "Partial Payment Processed",
                        $"${payingamont:0.00} paid via card.\nRemaining balance: ${remainingAmount:0.00}",
                        false
                    );

                    LblBalanceAmount.Text = remainingAmount.ToString("0.00");
                    TxtPayamont.Text = remainingAmount.ToString("0.00");
                    TxtPayamont.Focus();
                }




            }

        }


        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (remainingAmount < amountopay)
                {
                    ShowCustomMessage(
                        "Partial Payment Detected",
                        "Cancellation is not permitted during an active payment process.",
                        true
                    );
                    return;
                }

                this.Close();
            }
            catch (Exception ex)
            {
                ShowCustomMessage(
                    "Cancellation Error",
                    $"An error occurred while attempting to cancel the payment:\n{ex.Message}",
                    true
                );
            }
        }









        private async void BtnGiftCard_Click(object sender, RoutedEventArgs e)
        {
            await ProcessGiftCard();
        }

        private async Task ProcessGiftCard()
        {
            if (clsConnections.allowgiftcard == true)
            {

                FrmGiftPrompy cc = new FrmGiftPrompy();

                var result3 = cc.ShowDialog();
                if (result3 == true)
                {

                    string cardnumber = cc.CardNumber;
                    GiftCardProccess.RadeemRequest ek = new GiftCardProccess.RadeemRequest();
                    ek.encrypted = EncryptionHelper.EncryptString(cardnumber);
                    ek.amount = Convert.ToDecimal(TxtPayamont.Text);
                    ek.FranchiseeID = Guid.Parse(LoggedData.giftcardstoreid);
                    ek.PosRef = "PosP";
                    ek.acceptpartialamount = true;


                    var json = JsonConvert.SerializeObject(ek);
                    var options = new RestClientOptions(clsConnections.giftcardserver)
                    {
                        Timeout = TimeSpan.FromSeconds(5),
                    };
                    var client = new RestClient(options);
                    var request = new RestRequest("/Transaction/radeem", Method.Post);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Authorization", "Basic " + clsConnections.giftcardserverauth);

                    request.AddParameter("application/json", json, ParameterType.RequestBody);
                    RestResponse response = await client.ExecuteAsync(request);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {

                        GiftCardProccess.RadeemResponse myDeserializedClass = JsonConvert.DeserializeObject<GiftCardProccess.RadeemResponse>(response.Content);
                        if (myDeserializedClass.statuscode == "000000")
                        {
                            CurrentTransData.MainData.cardNumber = myDeserializedClass.cardno;
                            CurrentTransData.MainData.paidby = "GIFTC";
                            CurrentTransData.MainData.retref = myDeserializedClass.HostRef;
                            CurrentTransData.MainData.cardType = "GIFTC";
                            CurrentTransData.MainData.accountType = "GIFT CARD";
                            CurrentTransData.MainData.aid = "";
                            CurrentTransData.MainData.tcarqc = "";
                            CurrentTransData.MainData.entryMethod = "Manual";
                            CurrentTransData.MainData.cardHolder = "GIFT CARD USR";
                            if (cardamount == 0)
                            { cardamount = Convert.ToDecimal(LblBalanceAmount.Text); }
                            else { cardamount = cardamount + Convert.ToDecimal(TxtPayamont.Text); }

                            CurrentTransData.MainData.tipAmount = 0;
                            CurrentTransData.MainData.href = myDeserializedClass.HostRef;
                            CurrentTransData.MainData.hostRefNum = myDeserializedClass.HostRef;
                            CurrentTransData.MainData.deviceOrgRefNum = "";
                            CurrentTransData.giftcardbalance = myDeserializedClass.Newbalance;
                            this.DialogResult = true;

                        }
                        else if (myDeserializedClass.statuscode == "000030")
                        {
                            decimal balance = Convert.ToDecimal(TxtPayamont.Text) - myDeserializedClass.Approvedbalance;
                            MessageBox.Show("$" + myDeserializedClass.Approvedbalance.ToString("0.00") +
                                            " Approved. Balance $" + balance.ToString("0.00") + " Need to be Paid.");

                            TxtPayamont.Text = balance.ToString("0.00");
                            LblBalanceAmount.Text = balance.ToString("0.00");

                            remainingAmount = balance;
                            if (remainingAmount < 0) remainingAmount = 0;

                            CurrentTransData.MainData.cardNumber = myDeserializedClass.cardno;
                            CurrentTransData.MainData.paidby = "GIFTC";
                            CurrentTransData.MainData.retref = myDeserializedClass.HostRef;
                            CurrentTransData.MainData.cardType = "GIFTC";
                            CurrentTransData.MainData.accountType = "GIFT CARD";
                            CurrentTransData.MainData.aid = "";
                            CurrentTransData.MainData.tcarqc = "";
                            CurrentTransData.MainData.entryMethod = "Manual";
                            CurrentTransData.MainData.cardHolder = "GIFT CARD USR";

                            if (cardamount == 0)
                            { cardamount = myDeserializedClass.Approvedbalance; }
                            else { cardamount = cardamount + myDeserializedClass.Approvedbalance; }

                            CurrentTransData.MainData.href = myDeserializedClass.HostRef;
                            CurrentTransData.MainData.hostRefNum = myDeserializedClass.HostRef;
                            CurrentTransData.MainData.deviceOrgRefNum = "";
                            CurrentTransData.MainData.tipAmount = 0;
                            CurrentTransData.giftcardbalance = myDeserializedClass.Newbalance;
                        }

                        else
                        {
                            MessageBox.Show($"Error : {myDeserializedClass.description}");
                        }



                    }

                    else
                    {

                        MessageBox.Show("Error : " + response.StatusCode.ToString() + " Description :" + response.StatusDescription + " Body : " + response.Content);

                    }
                }

            }
            else
            {
                MessageBox.Show("Gift Card Feature is Not Activated for you System. Please contact Customer Service");
            }

        }

        private void BtnQ5_Click(object sender, RoutedEventArgs e)
        {
            decimal payingAmount = 5;
            ProcessCashPayment(payingAmount);

        }

        


        private void Btn10_Click(object sender, RoutedEventArgs e)
        {
            decimal payingAmount = 10;
            ProcessCashPayment(payingAmount);
        }

        private void Btn20_Click(object sender, RoutedEventArgs e)
        {
            decimal payingAmount = 20;
            ProcessCashPayment(payingAmount);

        }

        private void Btn50_Click(object sender, RoutedEventArgs e)
        {
            decimal payingAmount = 50;
            ProcessCashPayment(payingAmount);
        }



        private void ShowCustomMessage(string header, string message, bool isError = false)
        {
            FrmCustommessage frmCustommessage = new FrmCustommessage
            {
                LblHeader = { Text = header },     // Set the header separately
                LblMessage = { Text = message },   // Set the message separately
                IsError = isError                  
            };

            frmCustommessage.ShowDialog();        
        }

        private void TxtPayamont_TextChanged(object sender, TextChangedEventArgs e)
        {

        }






        //if (retref != null) { trandata.retref = retref; } else { trandata.retref = ""; }
        //if (cardtype != null) { trandata.cardType = cardtype; } else { trandata.cardType = ""; }
        //if (cardholder != null) { trandata.cardHolder = cardholder; } else { trandata.cardHolder = ""; }
        //if (entrymethod != null) { trandata.entryMethod = entrymethod; } else { trandata.entryMethod = ""; }
        //if (accounttype != null) { trandata.accountType = accounttype; } else { trandata.accountType = ""; }
        //if (aid != null) { trandata.aid = aid; } else { trandata.aid = ""; }
        //if (tcarqc != null) { trandata.tcarqc = tcarqc; } else { trandata.tcarqc = ""; }
        //if (Href != null) { trandata.href = Href; } else { trandata.href = ""; }
        //if (Host_Ref_Num != null) { trandata.hostRefNum = Host_Ref_Num; } else { trandata.hostRefNum = ""; }
        //if (Device_Org_Ref_Num != null) { trandata.deviceOrgRefNum = Device_Org_Ref_Num; } else { trandata.deviceOrgRefNum = ""; }
        //if (CC_Ref != null) { trandata.cardNumber = CC_Ref; } else { trandata.cardNumber = ""; }

        //string cardtype = "";
        //string cardholder = "";
        //string accounttype = "";
        //string aid = "";
        //string tcarqc = "";
        //string entrymethod = "";
        //decimal cardamount = Convert.ToDecimal(LblTotal.Text.Replace("$", "").Trim());
        //decimal paidcashamount = Convert.ToDecimal(CurrentTransData.MainData.cashAmount);
        //decimal cashchangeamount = Convert.ToDecimal(CurrentTransData.MainData.cashChangeAmount);
        //decimal paidcardamount = Convert.ToDecimal(CurrentTransData.MainData.cardAmount);
        //decimal tip = Convert.ToDecimal("0");
        //string href = "";
        //string Host_Ref = "";
        //string Device_Org_Ref_Num = "";
    }
}
