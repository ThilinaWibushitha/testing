using Microsoft.Data.SqlClient;
using POSLink2;
using Pospointe.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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
using POSLink2.Manage;
using POSLink2.Device;
using Newtonsoft.Json;
using RestSharp;
using Azure.Core;
using POSLink2.Const;
using System.Security.Cryptography;
using Pospointe.LocalData;
using System.Windows.Media.Animation;

namespace Pospointe.PosMenu
{
    /// <summary>
    /// Interaction logic for FrmPaxscreen.xaml
    /// </summary>


    public partial class FrmPaxscreen : Window
    {
        public string amount { get; set; }

        public string ecrref { get; set; }

        public string ipaddr { get; set; }
        public string portnum { get; set; }

        public string bautrate { get; set; }

        public string comtype { get; set; }
        public string comport { get; set; }
        public string responsecode { get; set; }
        public string responseMsg { get; set; }
        public string ApprovedAmount { get; set; }
        public string card4dight { get; set; }
        public string entrymode { get; set; }
        public string cardtype { get; set; }
        public string cardholder { get; set; }
        public string AuthCode { get; set; }
        public string AID { get; set; }

        public string TipAmount { get; set; }

        public string ARQCTC { get; set; }
        public string Host_Ref { get; set; }
        public string accounttype { get; set; }

        public string Href { get; set; }
        public bool signaturecapture { get; set; }
        public string transtyperequest { get; set; }
        public string Device_Org_Ref_Num { get; set; }

        public string balance1 { get; set; }

        public string balance2 { get; set; }


        public string autobatch { get; set; }

        public string cardreaddata { get; set; }

        
        public FrmPaxscreen()
        {
            InitializeComponent();
           
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            var fadeIn = (Storyboard)this.Resources["FadeInStoryboard"];
            fadeIn.Begin(RootGrid);
            LoggedData.tiprequest = Properties.Settings.Default.ReqTip;
            //MessageBox.Show($"ReqTip: {Properties.Settings.Default.ReqTip}");
            comtype = "PAX_IP";
            ipaddr = LoggedData.PaxIP;
            portnum = LoggedData.PaxPort;
            POSLink2.Terminal terminal = null;
            if (comtype == "PAX_IP")
            {
                CommSettingProperties commSettingProperties = new CommSettingProperties();
                commSettingProperties.Ip = ipaddr;
                commSettingProperties.Port = Int32.Parse(portnum);
                commSettingProperties.Timeout = Int32.Parse("-1");

                POSLink2.POSLink2 poslink = POSLink2.POSLink2.GetPOSLink2();
                POSLink2.CommSetting.TcpSetting commSetting = new POSLink2.CommSetting.TcpSetting();
                commSetting.Ip = commSettingProperties.Ip;
                commSetting.Port = commSettingProperties.Port;
                commSetting.Timeout = commSettingProperties.Timeout;
                terminal = poslink.GetTerminal(commSetting);
            }

            else
            {

                CommSettingProperties commSettingProperties = new CommSettingProperties();
                commSettingProperties.SerialPort = comport;
                commSettingProperties.BaudRate = Int32.Parse(bautrate);
                commSettingProperties.Timeout = Int32.Parse("-1");

                POSLink2.POSLink2 poslink = POSLink2.POSLink2.GetPOSLink2();
                SerialPortData serialPortData = new SerialPortData();
                serialPortData.SerialPortName = commSettingProperties.SerialPort;
                serialPortData.BaudRate = commSettingProperties.BaudRate;
                Uart uart = new Uart();
                uart.SetCommProperties(serialPortData);
                POSLink2.CommSetting.CustomerCommSetting commSetting = uart;
                commSetting.Timeout = commSettingProperties.Timeout;
                terminal = poslink.GetTerminal(commSetting);


            }


            if (transtyperequest == "BatchClose")
            {
                await Task.Run(() =>
                {
                    _batchData = BatchData.GetBatchData();
                    // Initialize and execute the batch close request
                    POSLink2.ExecutionResult executionResult = new POSLink2.ExecutionResult();
                    POSLink2.Batch.BatchCloseReq batchCloseReq = SetBatchCloseReq();
                    POSLink2.Batch.BatchCloseRsp batchCloseRsp = null;

                    try
                    {
                        executionResult = terminal.Batch.BatchClose(batchCloseReq, out batchCloseRsp);
                    }
                    catch (Exception ex) {
                        MessageBox.Show(ex.Message);
                    }

                    // Use Dispatcher to interact with the UI thread

                    // Check the error code and display corresponding message
                    // Use Dispatcher to update UI elements on the main thread
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.Ok)
                        {
                            GetResponse(batchCloseRsp);
                            this.Close();
                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.RecvAckTimeout)
                        {
                            MessageBox.Show("Receive ack timeout!", "ERROR");
                            this.Close();
                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.RecvDataTimeout)
                        {
                            MessageBox.Show("Receive data timeout!", "ERROR");
                            this.Close();
                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.ConnectError)
                        {
                            MessageBox.Show("Connect1 error!", "ERROR");
                            this.Close();
                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.SendDataError)
                        {
                            MessageBox.Show("Send data error!", "ERROR");
                            this.Close();
                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.RecvAckError)
                        {
                            MessageBox.Show("Receive ack error!", "ERROR");
                            this.Close();
                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.RecvDataError)
                        {
                            MessageBox.Show("Receive data error!", "ERROR");
                            this.Close();
                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.ExceptionalHttpStatusCode)
                        {
                            MessageBox.Show("Exceptional http status code!", "ERROR");
                            this.Close();
                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.LrcError)
                        {
                            MessageBox.Show("Lrc error!", "ERROR");
                            this.Close();
                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.PackRequestError)
                        {
                            MessageBox.Show("Pack request error!", "ERROR");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("An unknown error occurred.", "ERROR");
                            this.Close();
                        }
                    }));

                });
            }







            else if (transtyperequest == "GETPGIFT")
            {

                Task task = new Task(() =>
                {
                    POSLink2.ExecutionResult executionResult = new POSLink2.ExecutionResult();
                    POSLink2.Manage.InputAccountReq inputAccountReq = SetInputAccountReq();
                    POSLink2.Manage.InputAccountRsp inputAccountRsp = null;
                    executionResult = terminal.Manage.InputAccount(inputAccountReq, out inputAccountRsp);


                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.Ok)
                        {
                            GetResponse(inputAccountRsp);

                            this.Close();
                        }

                        if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.RecvAckTimeout)
                        {
                            MessageBox.Show("Receive ack timeout!", "ERROR");

                            this.Close();


                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.RecvDataTimeout)
                        {
                            MessageBox.Show("Receive data timeout!", "ERROR");

                            this.Close();
                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.ConnectError)
                        {
                            Application.Current.Dispatcher.BeginInvoke(async () =>
                            {
                                bool fixedIt = await PaxHelper.AutoFixPaxConnectionIfNeeded(this);

                                if (!fixedIt)
                                {
                                    MessageBox.Show("Transaction cancelled. Please verify the PAX terminal manually.", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }

                                this.Close();
                            });
                        }


                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.SendDataError)
                        {
                            MessageBox.Show("Send data error!", "ERROR");

                            this.Close();
                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.RecvAckError)
                        {
                            MessageBox.Show("Receive ack error!", "ERROR");

                            this.Close();
                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.RecvDataError)
                        {
                            MessageBox.Show("Receive data error!", "ERROR");

                            this.Close();
                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.ExceptionalHttpStatusCode)
                        {
                            MessageBox.Show("Exceptional http status code!", "ERROR");

                            this.Close();
                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.LrcError)
                        {
                            MessageBox.Show("Lrc error!", "ERROR");

                            this.Close();
                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.PackRequestError)
                        {
                            MessageBox.Show("Pack request error!", "ERROR");

                            this.Close();
                        }


                        // _isCancelListening = false;


                    }));

                    // _isCancelListening = false;
                });

                task.Start();


            }

            else if (transtyperequest == "GETVGIFT")
            {

                Task task = new Task(() =>
                {
                    POSLink2.ExecutionResult executionResult = new POSLink2.ExecutionResult();
                    POSLink2.Device.CameraScanReq cameraScanReq = SetCameraScanReq();
                    POSLink2.Device.CameraScanRsp cameraScanRsp = null;
                    executionResult = terminal.Device.Camera.CameraScan(cameraScanReq, out cameraScanRsp);
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.Ok)
                        {
                            GetResponse(cameraScanRsp);
                            this.Close();
                        }
                        if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.RecvAckTimeout)
                        {
                            MessageBox.Show("Receive ack timeout!", "ERROR");

                            this.Close();


                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.RecvDataTimeout)
                        {
                            MessageBox.Show("Receive data timeout!", "ERROR");

                            this.Close();
                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.ConnectError)
                        {
                            Application.Current.Dispatcher.BeginInvoke(async () =>
                            {
                                bool fixedIt = await PaxHelper.AutoFixPaxConnectionIfNeeded(this);

                                if (!fixedIt)
                                {
                                    MessageBox.Show("Transaction cancelled. Please verify the PAX terminal manually.", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }

                                this.Close();
                            });
                        }

                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.SendDataError)
                        {
                            MessageBox.Show("Send data error!", "ERROR");

                            this.Close();
                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.RecvAckError)
                        {
                            MessageBox.Show("Receive ack error!", "ERROR");

                            this.Close();
                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.RecvDataError)
                        {
                            MessageBox.Show("Receive data error!", "ERROR");

                            this.Close();
                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.ExceptionalHttpStatusCode)
                        {
                            MessageBox.Show("Exceptional http status code!", "ERROR");

                            this.Close();
                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.LrcError)
                        {
                            MessageBox.Show("Lrc error!", "ERROR");

                            this.Close();
                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.PackRequestError)
                        {
                            MessageBox.Show("Pack request error!", "ERROR");

                            this.Close();
                        }


                        // _isCancelListening = false;


                    }));

                    // _isCancelListening = false;
                });

                task.Start();

            }



            else if (transtyperequest == "SALE" || transtyperequest == "RETURN" || transtyperequest == "VOID")
            {


                Task task = new Task(() =>
               {
                   //  int tenderType = 1;
                   //  _transactionData.TransactionTypeRequest = 1;
                   // _transactionData.PosEchoDataRequest = "2";
                   POSLink2.ExecutionResult executionResult = new POSLink2.ExecutionResult();
                   POSLink2.Transaction.DoCreditReq doCreditRequest = SetCreditRequest();

                   POSLink2.Util.TransactionBehavior req = new POSLink2.Util.TransactionBehavior();
                   if (Properties.Settings.Default.ReqTip == true)
                   {
                       req.TipRequestFlag = "1";
                   }
                   else
                   {
                       req.TipRequestFlag = "0";
                   }
                   doCreditRequest.TransactionBehavior = req;

                 //  MessageBox.Show("Triggerd");
                   POSLink2.Transaction.DoCreditRsp doCreditResponse = new POSLink2.Transaction.DoCreditRsp();
                   executionResult = terminal.Transaction.DoCredit(doCreditRequest, out doCreditResponse);



                   Application.Current.Dispatcher.BeginInvoke(new Action(() =>
       {

                        if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.Ok)
                        {

                            GetResponse(doCreditResponse);
                            if (responsecode == "000000")
                            {

                            }
                            this.DialogResult = true;
                            this.Close();
                        }

                        if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.RecvAckTimeout)
                        {
                            MessageBox.Show("Receive ack timeout!", "ERROR");

                            this.Close();


                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.RecvDataTimeout)
                        {
                            MessageBox.Show("Receive data timeout!", "ERROR");

                            this.Close();
                        }

           else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.ConnectError)
           {
               Application.Current.Dispatcher.BeginInvoke(async () =>
               {
                   bool fixedIt = await PaxHelper.AutoFixPaxConnectionIfNeeded(this);

                   if (!fixedIt)
                   {
                       MessageBox.Show("Transaction cancelled. Please verify the PAX terminal manually.", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                   }

                   this.Close();
               });
           }

           else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.SendDataError)
                        {
                            MessageBox.Show("Send data error!", "ERROR");

                            this.Close();
                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.RecvAckError)
                        {
                            MessageBox.Show("Receive ack error!", "ERROR");

                            this.Close();
                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.RecvDataError)
                        {
                            MessageBox.Show("Receive data error!", "ERROR");

                            this.Close();
                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.ExceptionalHttpStatusCode)
                        {
                            MessageBox.Show("Exceptional http status code!", "ERROR");

                            this.Close();
                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.LrcError)
                        {
                            MessageBox.Show("Lrc error!", "ERROR");

                            this.Close();
                        }
                        else if (executionResult.GetErrorCode() == POSLink2.ExecutionResult.Code.PackRequestError)
                        {
                            MessageBox.Show("Pack request error!", "ERROR");

                            this.Close();
                        }
                    }));
            //   }

        // _isCancelListening = false;
    });

            task.Start();

        

    //  RunTransaction(tenderType, sender, e);

}
        }


    

        private BatchData _batchData;
        //private POSLink2.Batch.BatchCloseReq SetBatchCloseReq()
        //{
        //    POSLink2.Batch.BatchCloseReq request = new POSLink2.Batch.BatchCloseReq();

        //    if (_batchData == null || _batchData.BatchCloseReqNormalData == null || _batchData.BatchCloseReqNormalData.Length == 0)
        //    {
        //        MessageBox.Show("Batch data is null or empty.", "ERROR");
        //        throw new InvalidOperationException("Batch data is not initialized.");
        //    }

        //    request.TimeStamp = _batchData.BatchCloseReqNormalData[0];
        //    return request;
        //}

        private POSLink2.Batch.BatchCloseReq SetBatchCloseReq()
        {
            POSLink2.Batch.BatchCloseReq request = new POSLink2.Batch.BatchCloseReq();

            if (_batchData == null || _batchData.BatchCloseReqNormalData == null || _batchData.BatchCloseReqNormalData.Length < 2)
            {
                MessageBox.Show("Unable to perform batch close: missing batch request data.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new InvalidOperationException("Batch data is missing or incomplete.");
            }


            request.TimeStamp = _batchData.BatchCloseReqNormalData[0];
            request.MultiMerchant = new POSLink2.Util.MultiMerchant
            {
                Id = _batchData.BatchCloseReqNormalData[1],
                Name = _batchData.BatchCloseReqNormalData.Length > 2 ? _batchData.BatchCloseReqNormalData[2] : ""
            };

            return request;
        }



        private POSLink2.Manage.InputAccountReq SetInputAccountReq()
        {
            POSLink2.Manage.InputAccountReq request = new POSLink2.Manage.InputAccountReq();
            request.EdcType = (POSLink2.Const.EdcType)Global.EdcType[4, 1];
            request.TransactionType = (POSLink2.Const.TransType)Global.TransType[0, 1];
            request.MagneticSwipeEntryFlag = "1";
            request.ManualEntryFlag = "";
            request.ContactlessEntryFlag = "";
            request.ScannerEntryFlag = "";
            request.ExpiryDatePrompt = "";
            request.Timeout = "200";
            request.EncryptionFlag = "";
            request.KeySlot = "";
            request.MinAccountLength = "";
            request.MaxAccountLength = "";
            request.ContinuousScreen = "";
            return request;
        }

        private POSLink2.Device.CameraScanReq SetCameraScanReq()
        {
            POSLink2.Device.CameraScanReq request = new POSLink2.Device.CameraScanReq();
            request.Reader = "";
            request.Timeout = "";
            return request;
        }


        public void GetResponse(POSLink2.Device.CameraScanRsp response)
        {
            string resp = response.ResponseCode;
            if (resp == "000000")
            {
                string card = response.BarcodeData;
                cardreaddata = response.BarcodeData;
                this.DialogResult = true;
            }
            responsecode = response.ResponseCode;
            responseMsg = response.ResponseMessage;
            //deviceData.CameraScanRspNormalData[0] = response.ResponseCode;
            //deviceData.CameraScanRspNormalData[1] = response.ResponseMessage;
            //deviceData.CameraScanRspNormalData[2] = response.BarcodeData;
            //deviceData.CameraScanRspNormalData[3] = response.BarcodeType;
        }

        private POSLink2.Util.AmountReq GetAmountReq()
        {
            POSLink2.Util.AmountReq req = new POSLink2.Util.AmountReq();
            req.TransactionAmount = amount.Replace(".", "");
            //req.TipAmount = "0";
            //   req.CashBackAmount = "0";
            // req.MerchantFee = "0";
            //  req.TaxAmount = "0";
            //  req.FuelAmount = "0";
            //  req.ServiceFee = "0";
            return req;

        }

        private POSLink2.Util.TransactionBehavior GetTransactionBehaviorReq()
        {
            POSLink2.Util.TransactionBehavior req = new POSLink2.Util.TransactionBehavior();
            //req.SignatureCaptureFlag = _transactionData.TransactionBehaviorRequestData[0];

            //req.SignatureUploadFlag = _transactionData.TransactionBehaviorRequestData[2];
            //req.StatusReportFlag = _transactionData.TransactionBehaviorRequestData[3];
            //req.AcceptedCardType = _transactionData.TransactionBehaviorRequestData[4];
            //req.ProgramPromptsFlag = _transactionData.TransactionBehaviorRequestData[5];
            //req.SignatureAcquireFlag = _transactionData.TransactionBehaviorRequestData[6];
            //  if (LoggedData.tiprequest == true)
            //  {
                req.TipRequestFlag = "1";
            //  }
            //req.EntryMode = _transactionData.TransactionBehaviorRequestData[7];
            //req.ReceiptPrintFlag = _transactionData.TransactionBehaviorRequestData[8];
            //req.CardPresentMode = _transactionData.TransactionBehaviorRequestData[9];
            //req.DebitNetwork = _transactionData.TransactionBehaviorRequestData[10];
            //req.UserLanguage = _transactionData.TransactionBehaviorRequestData[11];
            //req.AddlRspDataFlag = _transactionData.TransactionBehaviorRequestData[12];
            //req.ForceCC = _transactionData.TransactionBehaviorRequestData[13];
            //req.ForceFsa = _transactionData.TransactionBehaviorRequestData[14];
            //req.ForceDuplicate = _transactionData.TransactionBehaviorRequestData[15];
            return req;
        }

        private POSLink2.Transaction.DoCreditReq SetCreditRequest()
        {
            POSLink2.Transaction.DoCreditReq request = new POSLink2.Transaction.DoCreditReq();
            if (transtyperequest == "SALE")
            {
                request.TransactionType = (POSLink2.Const.TransType)Global.TransType[1, 1];
                request.AmountInformation = GetAmountReq();
                POSLink2.Util.TransactionBehavior req = new POSLink2.Util.TransactionBehavior();
                req.TipRequestFlag = "1";
                request.TransactionBehavior = req;
            }
            if (transtyperequest == "RETURN")
            {
                request.TransactionType = (POSLink2.Const.TransType)Global.TransType[2, 1];
                request.AmountInformation = GetAmountReq();
            }
            if (transtyperequest == "VOID")
            {
                // label1.Text = "Proccessing Credit Card Void...";
                request.TransactionType = (POSLink2.Const.TransType)Global.TransType[16, 1];
            }
            //request.AmountInformation = GetAmountReq();
            // request.AccountInformation = GetAccountReq();
            //request.TraceInformation = GetTraceReq();
            //request.AvsInformation = GetAvsReq();
            //request.CashierInformation = GetCashierReq();
            //request.CommercialInformation = GetCommercialReq();
            //request.MotoECommerceInformation = GetMotoECommerceReq();
            //request.Restaurant = GetRestaurantReq();
            //request.HostGateway = GetHostGatewayReq();
            //request.TransactionBehavior = GetTransactionBehaviorReq();
            //request.Original = GetOriginalReq();
            //request.MultiMerchant = GetMultiMerchantReq();
            //request.FleetCard = GetFleetCardReq();
            //request.Lodging = GetLodgingInfoReq();
            //request.AutoRental = GetAutoRentalInfoReq();
            //request.PosEchoData = _transactionData.PosEchoDataRequest;
            //request.ContinuousScreen = _transactionData.ContinuousScreenRequest;
            //request.AccountInformation = GetAccountReq();
            request.TraceInformation = GetTraceReq();
            //request.AvsInformation = GetAvsReq();
            //request.CashierInformation = GetCashierReq();
            //request.CommercialInformation = GetCommercialReq();
            //request.MotoECommerceInformation = GetMotoECommerceReq();
            //request.Restaurant = GetRestaurantReq();
            //request.HostGateway = GetHostGatewayReq();
            //request.TransactionBehavior = GetTransactionBehaviorReq();
            //request.Original = GetOriginalReq();
            //request.MultiMerchant = GetMultiMerchantReq();
            //request.FleetCard = GetFleetCardReq();
            //request.Lodging = GetLodgingInfoReq();
            //request.AutoRental = GetAutoRentalInfoReq();
            // request.PosEchoData = "2";
            //request.ContinuousScreen = _transactionData.ContinuousScreenRequest;
            return request;
        }

        private POSLink2.Util.TraceReq GetTraceReq()
        {
            POSLink2.Util.TraceReq req = new POSLink2.Util.TraceReq();
            req.EcrRefNum = ecrref;
            // req.InvoiceNumber = _transactionData.TraceRequestInfoData[1];
            // req.AuthCode = _transactionData.TraceRequestInfoData[2];
            // req.OrigRefNum = _transactionData.TraceRequestInfoData[3];
            //  req.TimeStamp = _transactionData.TraceRequestInfoData[4];
            //  req.EcrTransID = _transactionData.TraceRequestInfoData[5];
            //  req.OrigEcrRefNum = _transactionData.TraceRequestInfoData[6];
            if (transtyperequest == "VOID")
            {
                // req.OrigTraceNum = 
                req.OrigRefNum = Device_Org_Ref_Num;
            }
            return req;
        }

        public void GetResponse(POSLink2.Manage.InputAccountRsp response)
        {

            //Normal

            string resp = response.ResponseCode;
            if (resp == "000000")
            {
                string card = response.Track2Data;
                cardreaddata = response.Track2Data;
                this.DialogResult = true;
            }
            responsecode = response.ResponseCode;
            responseMsg = response.ResponseMessage;
            //manageData.InputAccountRspNormalData[2] = response.EntryMode;
            //manageData.InputAccountRspNormalData[3] = response.Track1Data;
            //manageData.InputAccountRspNormalData[4] = response.Track2Data;
            //manageData.InputAccountRspNormalData[5] = response.Track3Data;
            //manageData.InputAccountRspNormalData[6] = response.Pan;
            //manageData.InputAccountRspNormalData[7] = response.ExpiryDate;
            //manageData.InputAccountRspNormalData[8] = response.QrCode;
            //manageData.InputAccountRspNormalData[9] = response.Ksn;
            //manageData.InputAccountRspNormalData[10] = response.CardHolder;
        }


        public void GetResponse(POSLink2.Batch.BatchCloseRsp response)
        {
            BatchData batchData = BatchData.GetBatchData();
            //Normal
            batchData.BatchCloseRspNormalData[0] = response.ResponseCode;
            batchData.BatchCloseRspNormalData[1] = response.ResponseMessage;
            batchData.BatchCloseRspNormalData[2] = response.TotalCount;
            batchData.BatchCloseRspNormalData[3] = response.TotalAmount;


            batchData.BatchCloseRspNormalData[4] = response.TimeStamp;
            batchData.BatchCloseRspNormalData[5] = response.Tid;
            batchData.BatchCloseRspNormalData[6] = response.Mid;
            batchData.BatchCloseRspNormalData[7] = response.FailedTransNO;
            batchData.BatchCloseRspNormalData[8] = response.FailedCount;
            batchData.BatchCloseRspNormalData[9] = response.SafFailedCount;
            batchData.BatchCloseRspNormalData[10] = response.SafFailedTotal;
            //Host Info
            batchData.HostInfoRspData[0] = response.HostInformation.HostResponseCode;
            //batchData.HostInfoRspData[1] = response.HostInformation.HostResponseMessage;
            //batchData.HostInfoRspData[2] = response.HostInformation.AuthCode;
            //batchData.HostInfoRspData[3] = response.HostInformation.HostRefNum;
            //batchData.HostInfoRspData[4] = response.HostInformation.TraceNumber;
            batchData.HostInfoRspData[5] = response.HostInformation.BatchNumber;


            if (response.ResponseCode == "000000")
            {

                MessageBox.Show("Batch Closed Successfully");

            }
           
            else
            {
                MessageBox.Show("CODE : " + response.ResponseCode + ". Message : " + response.ResponseMessage + ".");
            }

            
        }

        public void GetResponse(POSLink2.Transaction.DoCreditRsp response)
        {
           // TransactionData transactionData = TransactionData.GetTransactionData();
           // transactionData.ResponseCode = response.ResponseCode;
           // transactionData.ResponseMessage = response.ResponseMessage;
           // transactionData.TransactionTypeResponse = Tools.ParseTransType(response.TransactionType);



            //GetAmountInformation(response.AmountInformation);

            GetTraceInformation(response.TraceInformation);
            //GetAvsInformation(response.AvsInformation);
            //GetCommercialInformation(response.CommercialInformation);
            //GetMotoECommerceInformation(response.MotoECommerceInformation);
            //GetVasInformation(response.VasInformation);
            //GetTorInformation(response.TorInformation);
            //GetRestaurant(response.Restaurant);
            //GetCardInfo(response.CardInfo);
            //GetPaymentTransInfo(response.PaymentTransInfo);

            //GetMultiMerchant(response.MultiMerchant);
            //GetFleetCard(response.FleetCard);
            GetHostInformation(response.HostInformation);
            GetPaymentEmvTag(response.PaymentEmvTag);
            GetAccountInformation(response.AccountInformation);
            GetAmountInformation(response.AmountInformation);
            GetPaymentTransInfo(response.PaymentTransInfo);
            this.responsecode = response.ResponseCode;
            this.responseMsg = response.ResponseMessage;


        }

        public void GetTraceInformation(POSLink2.Util.TraceRsp response)
        {
            if (response != null)
            {
               // TransactionData transactionData = TransactionData.GetTransactionData();
               // transactionData.TraceResponseInfoData[0] = response.RefNum;
                Device_Org_Ref_Num = response.RefNum;
                CurrentTransData.MainData.deviceOrgRefNum = response.RefNum;
                //transactionData.TraceResponseInfoData[1] = response.EcrRefNum;
                //transactionData.TraceResponseInfoData[2] = response.TimeStamp;
                //transactionData.TraceResponseInfoData[3] = response.InvoiceNumber;
                //transactionData.TraceResponseInfoData[4] = response.PaymentService2000;
                //transactionData.TraceResponseInfoData[5] = response.AuthorizationResponse;
                //transactionData.TraceResponseInfoData[6] = response.EcrTransID;
            }
        }

        public void GetPaymentTransInfo(POSLink2.Util.PaymentTransInfo response)
        {
            if (response != null)
            {
               // TransactionData transactionData = TransactionData.GetTransactionData();
                //transactionData.PaymentTransInfoResponseData[0] = response.DiscountAmount;
                //transactionData.PaymentTransInfoResponseData[1] = response.ChargedAmount;
                //transactionData.PaymentTransInfoResponseData[2] = response.SignatureStatus;
                //transactionData.PaymentTransInfoResponseData[3] = response.Fps;
                //transactionData.PaymentTransInfoResponseData[4] = response.FpsSignature;
                //transactionData.PaymentTransInfoResponseData[5] = response.FpsReceipt;
                //transactionData.PaymentTransInfoResponseData[6] = response.Token;
               // transactionData.PaymentTransInfoResponseData[7] = response.HRef;
                Href = response.HRef;
                CurrentTransData.MainData.href = response.HRef;
                //transactionData.PaymentTransInfoResponseData[8] = response.Sn;
                //transactionData.PaymentTransInfoResponseData[9] = response.SettlementDate;
                //transactionData.PaymentTransInfoResponseData[10] = response.HostEchoData;
                //transactionData.PaymentTransInfoResponseData[11] = response.PinStatusNum;
                //transactionData.PaymentTransInfoResponseData[12] = response.ValidationCode;
                //transactionData.PaymentTransInfoResponseData[13] = response.UserLanguageStatus;
                //transactionData.PaymentTransInfoResponseData[14] = response.GlobalUid;
                //transactionData.PaymentTransInfoResponseData[15] = response.OrigTip;
                //transactionData.PaymentTransInfoResponseData[16] = response.EdcType;
                cardtype = response.EdcType;

                CurrentTransData.MainData.cardType = response.EdcType;
                //transactionData.PaymentTransInfoResponseData[17] = response.PrintLine1;
                //transactionData.PaymentTransInfoResponseData[18] = response.PrintLine2;
                //transactionData.PaymentTransInfoResponseData[19] = response.PrintLine3;
                //transactionData.PaymentTransInfoResponseData[20] = response.PrintLine4;
                //transactionData.PaymentTransInfoResponseData[21] = response.PrintLine5;
                //transactionData.PaymentTransInfoResponseData[22] = response.EwicBenefitExpd;
                //transactionData.PaymentTransInfoResponseData[23] = response.EwicBalance;
                //transactionData.PaymentTransInfoResponseData[24] = response.EwicDetail;
                //transactionData.PaymentTransInfoResponseData[25] = response.ReverseAmount;
                //transactionData.PaymentTransInfoResponseData[26] = response.ReversalStatus;
                //transactionData.PaymentTransInfoResponseData[27] = response.TokenSerialNum;
                //transactionData.PaymentTransInfoResponseData[28] = response.SignatureData;
                // transactionData.AddlRspDataResponse = response.AddlRspDataInfo;
            }
        }

        public void GetAccountInformation(POSLink2.Util.AccountRsp response)
        {
            if (response != null)
            {
               // TransactionData transactionData = TransactionData.GetTransactionData();
                //transactionData.AccountResponseInfoData[0] = response.Account;
               // transactionData.AccountResponseInfoData[1] = response.EntryMode;
                //transactionData.AccountResponseInfoData[2] = response.ExpireDate;
                //transactionData.AccountResponseInfoData[3] = response.EbtType;
                //transactionData.AccountResponseInfoData[4] = response.VoucherNumber;
                //transactionData.AccountResponseInfoData[5] = response.NewAccountNo;
                //transactionData.AccountResponseInfoData[6] = response.CardType;
              //  transactionData.AccountResponseInfoData[7] = response.CardHolder;
                //transactionData.AccountResponseInfoData[8] = response.CvdApprovalCode;
                //transactionData.AccountResponseInfoData[9] = response.CvdMessage;
                //transactionData.AccountResponseInfoData[10] = response.CardPresentIndicator;
                //transactionData.AccountResponseInfoData[11] = response.GiftCardType;
                //transactionData.AccountResponseInfoData[12] = response.DebitAccountType;

                card4dight = response.Account;
                CurrentTransData.MainData.cardNumber = response.Account;
                cardholder = response.CardHolder;
                CurrentTransData.MainData.cardHolder = response.CardHolder;
                if (response.EntryMode == "0") { entrymode = "Manual"; }
                if (response.EntryMode == "1") { entrymode = "Swipe"; }
                if (response.EntryMode == "2") { entrymode = "Contactless"; }
                if (response.EntryMode == "3") { entrymode = "Scanner"; }
                if (response.EntryMode == "4") { entrymode = "Chip"; }
                if (response.EntryMode == "5") { entrymode = "Chip Fall Back Swipe"; }
                CurrentTransData.MainData.entryMethod = response.EntryMode;

            }
        }

        public void UpdateStatus(string status)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                LblStatus.Text = status;
            });
        }


        public void GetPaymentEmvTag(POSLink2.Util.PaymentEmvTag response)
        {
            if (response != null)
            {
                //TransactionData transactionData = TransactionData.GetTransactionData();
               // transactionData.PaymentEmvTagResponseData[5] = response.AppLabel;
               // transactionData.PaymentEmvTagResponseData[7] = response.Aid;
               // transactionData.PaymentEmvTagResponseData[11] = response.Arqc;
              //  transactionData.PaymentEmvTagResponseData[0] = response.Tc;
                accounttype = response.AppLabel;
                CurrentTransData.MainData.accountType = accounttype;
                AID = response.Aid;
                CurrentTransData.MainData.aid = 
                    AID;
                ARQCTC = response.Tc + " / " + response.Arqc;
                CurrentTransData.MainData.tcarqc = ARQCTC;


            }
        }

        public void GetHostInformation(POSLink2.Util.HostRsp response)
        {
            if (response != null)
            {
               // TransactionData transactionData = TransactionData.GetTransactionData();
                //transactionData.HostResponseInfoData[0] = response.HostResponseCode;
                //transactionData.HostResponseInfoData[1] = response.HostResponseMessage;
               // transactionData.HostResponseInfoData[2] = response.AuthCode;
               // transactionData.HostResponseInfoData[3] = response.HostRefNum;
                Host_Ref = response.HostRefNum;
                CurrentTransData.MainData.hostRefNum = response.HostRefNum;
                //transactionData.HostResponseInfoData[4] = response.TraceNumber;
                //transactionData.HostResponseInfoData[5] = response.BatchNumber;
                //transactionData.HostResponseInfoData[6] = response.TransactionIdentifier;
                //transactionData.HostResponseInfoData[7] = response.GatewayTransactionID;
                //transactionData.HostResponseInfoData[8] = response.HostDetailedMessage;
                //transactionData.HostResponseInfoData[9] = response.TransactionIntegrityClass;
                //transactionData.HostResponseInfoData[10] = response.RetrievalRefNum;
                //transactionData.HostResponseInfoData[11] = response.IssuerResponseCode;
                AuthCode = response.AuthCode;
                CurrentTransData.MainData.href = AuthCode;
            }
        }

        public void GetAmountInformation(POSLink2.Util.AmountRsp response)
        {
            if (response != null)
            {
                //TransactionData transactionData = TransactionData.GetTransactionData();
               // transactionData.AmountResponseInfoData[0] = response.ApproveAmount;
                this.ApprovedAmount = response.ApproveAmount;
                //  transactionData.AmountResponseInfoData[1] = response.AmountDue;
                if (LoggedData.tiprequest == true)
                {
                    TipAmount = response.TipAmount;

                    // Convert TipAmount from string or int to double correctly (assuming it’s in cents)
                    if (double.TryParse(TipAmount, out double tipInCents))
                    {
                        CurrentTransData.MainData.tipAmount = tipInCents / 100;
                    }
                    else
                    {
                        CurrentTransData.MainData.tipAmount = 0;
                    }
                }
                else
                {
                    TipAmount = "0";
                    CurrentTransData.MainData.tipAmount = 0;
                }

                //   transactionData.AmountResponseInfoData[3] = response.CashBackAmount;
                //    transactionData.AmountResponseInfoData[4] = response.MerchantFee;
                //   transactionData.AmountResponseInfoData[5] = response.TaxAmount;
                balance1 = response.Balance1;
                balance2 = response.Balance2;
                //   transactionData.AmountResponseInfoData[8] = response.ServiceFee;
                // transactionData.AmountResponseInfoData[9] = response.TransactionRemainingAmount;

            }
        }

    }
    
}
