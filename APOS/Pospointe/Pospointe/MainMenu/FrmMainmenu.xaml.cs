using Pospointe.LocalData;
using Pospointe.PosMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using WpfScreenHelper;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Pospointe.LoginWindow;
using RestSharp;
using Pospointe.Models;
using Newtonsoft.Json;
using Pospointe.Reports;
using Pospointe.Services;
using POSLink2;
using System.Reflection;
using POSLink2.Util;
using System.Windows.Media.Animation;
using Pospointe.PointeSense;


namespace Pospointe.MainMenu
{
    /// <summary>
    /// Interaction logic for FrmMainmenu.xaml
    /// </summary>
    public partial class FrmMainmenu : Window
    {
        
        public FrmMainmenu()
        {
            InitializeComponent();
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LblUser.Text = LoggedData.loggeduser.UserName;


            
        }








        private void BtnEnterpos_Click(object sender, RoutedEventArgs e)
        {
            if (LoggedData.loggeduser.UserId != "01")
            {

                bool hasPermission = LoggedData.loggeduser.UserPos == "OK";  // Permisson STEP 1
                if (!hasPermission)
                {
                    FrmCustommessage msg = new FrmCustommessage();
                    msg.IsError = true;
                    msg.LblHeader.Text = "Error";
                    msg.LblMessage.Text = $"You don't have permisson to enter POS.";
                    msg.ShowDialog();
                }

                if (hasPermission)
                {
                    var options = new RestClientOptions(clsConnections.myposapiurl)
                    {
                        MaxTimeout = -1,
                    };
                    var client = new RestClient(options);
                    var request = new RestRequest("/Shift/currentshift/" + LoggedData.loggeduser.UserId, Method.Get);
                    request.AddHeader("db", clsConnections.mydb);
                    RestResponse response = client.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {

                        //IF NOT OPEN THEN OPEN SHIFT REQUEST
                        FrmCustommessage msg = new FrmCustommessage();
                        msg.IsError = true;
                        msg.LblHeader.Text = "Error";
                        msg.LblMessage.Text = $"You don't have any open shift. Please open a Shift";
                        msg.ShowDialog();


                    }

                    else if (response.IsSuccessful)
                    {
                        RegisterService.RefreshRegister(clsConnections.mydb);
                        FrmPosMain frmPosMain = new FrmPosMain();
                        frmPosMain.ShowDialog();

                    }
                    else
                    {
                        //RegisterService.RefreshRegister(clsConnections.mydb);
                        FrmPosMain frmPosMain = new FrmPosMain();
                        frmPosMain.ShowDialog();
                    }


                }


            }

            else
            {
                RegisterService.RefreshRegister(clsConnections.mydb);
                FrmPosMain frmPosMain = new FrmPosMain();
                frmPosMain.ShowDialog();
            }
           
        }






        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
           

            bool hasPermission = LoggedData.loggeduser.LogtOut == true;  // Permisson STEP 1
            if (!hasPermission)
            {
                FrmPermissionShow frmPermissionShow = new FrmPermissionShow();
                frmPermissionShow.reqpermssion = "prmlogout"; //Custom Name for Permisson Request STEP 2

                hasPermission = frmPermissionShow.ShowDialog() == true;
            }

            if (hasPermission)
            {
                this.Close();
            }

                       
        }


        private async void BtnShift_Click(object sender, RoutedEventArgs e)
        {

            bool hasPermission = LoggedData.loggeduser.UserEndDayPeform == "OK";  
            if (!hasPermission)
            {
                FrmPermissionShow frmPermissionShow = new FrmPermissionShow();
                frmPermissionShow.reqpermssion = "prmendday"; 

                hasPermission = frmPermissionShow.ShowDialog() == true;
            }

            if (hasPermission)
            {
                
                //CHECK IF SHIFT OPEN
                var options = new RestClientOptions(clsConnections.myposapiurl)
                {
                    Timeout = TimeSpan.FromSeconds(5),
                    
                };
                var client = new RestClient(options);
                var request = new RestRequest("/Shift/currentshift/" + LoggedData.loggeduser.UserId, Method.Get);
                request.AddHeader("db", clsConnections.mydb);
                RestResponse response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    PrintService.OpenCashDrawer();
                    //IF NOT OPEN THEN OPEN SHIFT REQUEST
                    FrmNumbpad frmNumbpad = new FrmNumbpad();
                    frmNumbpad.returnvalue = "200.00";
                    var result = frmNumbpad.ShowDialog();
                    if (result == true)
                    {
                        
                        await openshiftAsync(frmNumbpad.returnvalue);

                    }


                }

                else if (response.IsSuccessful)
                {
                    await  SyncService.SyncTransTOCLoud();
                    PrintService.OpenCashDrawer();
                    FrmShiftClose.Root myDeserializedClass = JsonConvert.DeserializeObject<FrmShiftClose.Root>(response.Content);
                    FrmShiftClose frmShiftClose = new FrmShiftClose();
                    frmShiftClose._dayopen = myDeserializedClass;
                    var result = frmShiftClose.ShowDialog();
                    if (result == true)
                    {
                        this.Close();
                    }

                }
                else
                {
                    FrmCustommessage msg = new FrmCustommessage();
                    msg.IsError = false;
                    msg.LblHeader.Text = "Success";
                    msg.LblMessage.Text = $"Server is not available. Please try again later.";
                    msg.ShowDialog();
                }



                //IF OPENED CLOSE SHIFT
            }







        }

        private async Task openshiftAsync(string startamount)
        {
            decimal opeamount = Convert.ToDecimal(startamount);
            TblDayOpen op = new TblDayOpen { 
             CashierId = LoggedData.loggeduser.UserId,
             Date = DateOnly.FromDateTime(DateTime.Today),
             OpeningBalance = opeamount,
             ClosingBalance = 0,
             Deference = 0,
             OpenedDateTime = DateTime.Now

            };

            var json7 = JsonConvert.SerializeObject(op);

            var options = new RestClientOptions(clsConnections.myposapiurl)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/Shift/shiftopen", Method.Post);
            request.AddHeader("db", clsConnections.mydb);
            request.AddParameter("application/json", json7, ParameterType.RequestBody);
            RestResponse response = await client.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                FrmCustommessage msg = new FrmCustommessage();
                msg.IsError = true;
                msg.LblHeader.Text = "Success";
                msg.LblMessage.Text = $"shift started successfully";

                msg.ShowDialog();
                Properties.Settings.Default.CheckNo = 51;
                Properties.Settings.Default.Save();
                FrmPosMain pos = new FrmPosMain();
                pos.ShowDialog();
            }

            else {

                FrmCustommessage msg = new FrmCustommessage();
                msg.IsError = true;
                msg.LblHeader.Text = "Error";
                msg.LblMessage.Text =
        $"Shift failed. Status: {response.StatusCode}, " +
        $"Error: {response.ErrorMessage}, " +
        $"Content: {response.Content}";
                msg.ShowDialog();

            }
        }

        private void BtnBtnReports_Click(object sender, RoutedEventArgs e)
        {
            bool hasPermission = LoggedData.loggeduser.UserBackEnd == "OK";
            if (!hasPermission)
            {
                FrmPermissionShow frmPermissionShow = new FrmPermissionShow();
                frmPermissionShow.reqpermssion = "prmdashboard";

                hasPermission = frmPermissionShow.ShowDialog() == true;
            }

            if (hasPermission)
            {
                FrmReportDateSelect frmReportDateSelect = new FrmReportDateSelect();
                frmReportDateSelect.ShowDialog();
            }
        }

        private void BtnOnlineorders_Click(object sender, RoutedEventArgs e)
        {
            // Reload settings to ensure we have the latest value
            Properties.Settings.Default.Reload();

            if (Properties.Settings.Default.MarketOrders == false)
            {
                ShowCustomMessage("Access Denied", "Activate marketplace to use Online Orders.\n\nGo to Settings → Online Orders to enable this feature.", false);
                return;
            }
            FrmonlineOrders frmonline = new FrmonlineOrders();
            frmonline.ShowDialog();
        }


        private void BtnSalesoverview_Click(object sender, RoutedEventArgs e)
        {
            bool hasPermission = LoggedData.loggeduser.UserDashBoard == "OK"; 
            if (!hasPermission)
            {
                FrmPermissionShow frmPermissionShow = new FrmPermissionShow();
                frmPermissionShow.reqpermssion = "prmdashboard"; 

                hasPermission = frmPermissionShow.ShowDialog() == true;
            }

            if (hasPermission)
            {
                if (LoggedData.myposuser != null)
                {
                    string username = LoggedData.myposuser.email;
                    string password = LoggedData.myposuser.password;

                    string credentials = $"{username}:{password}";

                    byte[] byteCredentials = Encoding.UTF8.GetBytes(credentials);

                    string base64Credentials = Convert.ToBase64String(byteCredentials);

                    string basicAuthHeader = $"Basic {base64Credentials}";

                    var options = new RestClientOptions(clsConnections.baseurllogin)
                    {
                        Timeout = TimeSpan.FromSeconds(5),
                    };
                    var client = new RestClient(options);
                    var request = new RestRequest($"/Userlogin/requestauth", Method.Post);
                    request.AddHeader("db", clsConnections.mydb);
                    request.AddHeader("Authorization", basicAuthHeader);
                    request.AddHeader("Permissions", "dashboard");
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);



                    if (response.IsSuccessful)
                    {
                        loginapi myDeserializedClass = JsonConvert.DeserializeObject<loginapi>(response.Content);
                        FrmQuickdash dash = new FrmQuickdash();
                        dash.url = myDeserializedClass.redirecturl;
                        dash.ShowDialog();
                    }
                }
                else
                {

                    MessageBox.Show("There is no User Account for the POS.");
                }
            }


            
        }

        public class loginapi
        {
            public string redirecturl { get; set; }
            public string baseurl { get; set; }
            public string token { get; set; }
            public string accessabledb { get; set; }
            public DateTime exp { get; set; }
        }



        private async void BtnTroubleshoot_Click(object sender, RoutedEventArgs e)
        {
            SnackbarText.Text = "Syncing... Please wait";
            SnackbarBorder.Visibility = Visibility.Visible;
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
            SnackbarBorder.BeginAnimation(OpacityProperty, fadeIn);
            await Task.Delay(2000);
            string appPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            System.Diagnostics.Process.Start(appPath);
            Application.Current.Shutdown();
        }



        private void BtnBatchClose_Click(object sender, RoutedEventArgs e)
        {
            batchclose();
        }

        private void batchclose()
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
                pax.ipaddr = LoggedData.PaxIP;
                pax.portnum = LoggedData.PaxPort;
            }
            pax.transtyperequest = "BatchClose";

            var result = pax.ShowDialog();


        }

        private void BtnRequestSupport_Click(object sender, RoutedEventArgs e)
        {
            bool hasPermission = LoggedData.loggeduser.RequestSupport == true;
            if (!hasPermission)
            {
                FrmPermissionShow frmPermissionShow = new FrmPermissionShow();
                frmPermissionShow.reqpermssion = "prmdashboard";

                hasPermission = frmPermissionShow.ShowDialog() == true;
            }

            if (hasPermission)
            {
                FrmMainTrouble frmMainTrouble = new FrmMainTrouble();
                frmMainTrouble.ShowDialog();
            }
        }

        private async void BtnClock_Click(object sender, RoutedEventArgs e)
        {
            if (!clsConnections.allowtimecard)
            {
                ShowCustomMessage("Upcoming Updates", "This feature will be available in an upcoming update. Thank you for your patience and support.", false);
                return;
            }

            int userId;
            string enteredUserId;
            string enteredPin;

            do
            {
                PinPad usernamePad = new PinPad("Enter User ID", null);
                if (usernamePad.ShowDialog() != true)
                {
                    return;
                }

                enteredUserId = usernamePad.returnvalue?.Trim();

                if (string.IsNullOrEmpty(enteredUserId) || !int.TryParse(enteredUserId, out userId))
                {
                    ShowCustomMessage("Invalid Input", "User ID must be a valid numeric value.", true);
                }
            } while (string.IsNullOrEmpty(enteredUserId) || !int.TryParse(enteredUserId, out userId));

            do
            {
                PinPad pinPad = new PinPad("Enter PIN", null);
                if (pinPad.ShowDialog() != true)
                {
                    return;
                }

                enteredPin = pinPad.returnvalue?.Trim();

                if (string.IsNullOrEmpty(enteredPin))
                {
                    ShowCustomMessage("Invalid Input", "PIN cannot be empty. Please enter your PIN.", true);
                }
            } while (string.IsNullOrEmpty(enteredPin));

            await TimeCardService.GetUserData(userId, enteredPin);
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

        private void BtnBtnShiftReport_Click(object sender, RoutedEventArgs e)
        {
            ShowCustomMessage("Upcoming Updates", "This feature will be available in an upcoming update. Thank you for your patience and support.", false);
            return;
            //FrmShiftReport frmShiftReport = new FrmShiftReport();
            //frmShiftReport.ShowDialog();

        }
    }

}
