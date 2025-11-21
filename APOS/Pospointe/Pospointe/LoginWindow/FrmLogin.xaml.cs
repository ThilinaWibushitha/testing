using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Pospointe.LocalData;
using Pospointe.MainMenu;
using Pospointe.Models;
using Pospointe.PointeSense;
using Pospointe.SecondScreens;
using Pospointe.Services;
using RestSharp;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WpfScreenHelper;


namespace Pospointe.LoginWindow
{
    /// <summary>
    /// Interaction logic for FrmLogin.xaml
    /// </summary>
    public partial class FrmLogin : Window
    {
        private MainWindowViewModel _viewModel;
        public static FrmCusDis frmCusDis { get; private set; }


        private HubConnection _hubConnection;
        private int _retryCount = 0;
        private const int _maxRetries = 3;


        private DispatcherTimer _timerSync;
        private DispatcherTimer _timer;
        public List<TblUser> localusers = new List<TblUser>();
        public FrmLogin()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();

            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AddButtonsToGrid();
            StartBackgroundTimer();

            if (LoggedData.Reg.activateMarketPlace == true)
            {
                if (Properties.Settings.Default.MarketOrders == true)
                {
                    await StartMartetPlaceServiceAsync();
                }
            }

            if (!Properties.Settings.Default.CustomerDisplay)
            {
                return;
            }

            var screens = WpfScreenHelper.Screen.AllScreens.ToList();

            foreach (var screen in screens)
            {

            }

            Screen selectedScreen = null;

            if (!string.IsNullOrEmpty(Properties.Settings.Default.CDisplayID))
            {
                selectedScreen = screens.FirstOrDefault(s => s.DeviceName == Properties.Settings.Default.CDisplayID);
            }

            var primaryScreen = screens.FirstOrDefault(s => s.Bounds.Left == 0 && s.Bounds.Top == 0);

            if (selectedScreen == null)
            {
                selectedScreen = screens.FirstOrDefault(s => s != primaryScreen);
            }

            if (selectedScreen == null)
            {
                selectedScreen = screens.FirstOrDefault();
            }

            if (frmCusDis != null)
            {
                frmCusDis.Close();
                frmCusDis = null;
            }

            frmCusDis = new Pospointe.SecondScreens.FrmCusDis
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                WindowStyle = WindowStyle.None,
                ResizeMode = ResizeMode.NoResize,
                Topmost = true
            };

            frmCusDis.Loaded += (s, args) =>
            {
                frmCusDis.Left = selectedScreen.Bounds.Left;
                frmCusDis.Top = selectedScreen.Bounds.Top;
                frmCusDis.Width = selectedScreen.Bounds.Width;
                frmCusDis.Height = selectedScreen.Bounds.Height;
                frmCusDis.WindowState = WindowState.Maximized;
                frmCusDis.SizeToContent = SizeToContent.Manual;
                frmCusDis.UseLayoutRounding = true;
            };

            frmCusDis.Show();


        }

        private async Task StartMartetPlaceServiceAsync()
        {



            // MessageBox.Show(LoggedData.Reg.marketPlaceDeviceId.ToString());
            // Initialize the SignalR connection
            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"{clsConnections.marketplaceserver}/notificationhub?deviceId=" + LoggedData.Reg.marketPlaceDeviceId)  // Replace with your API URL and deviceId
                .Build();

            // Define a handler for receiving notifications
            _hubConnection.On<string>("ReceiveNotification", async (message) =>
            {
                // When a message is received, show a message box or other UI interaction
                //await Application.Current.MainPage.DisplayAlert("Login Failed", message, "OK");
                // model.email = message;
                if (message != null)
                {
                    if (message.Contains("Error"))
                    {
                        //  await Application.Current.MainPage.DisplayAlert("Error", message, "OK");

                    }
                    else
                    {
                        //  if (localdata.autoaccept == true)
                        //  {

                        POSOrderModel myDeserializedClass = JsonConvert.DeserializeObject<POSOrderModel>(message);
                        //ACCEPT ORDER HERE FIRST


                        await autoacceptorder(myDeserializedClass);



                        //}
                        //else
                        //{
                        //    await showorderscreen(message);
                        //}
                    }
                }

            });



            // Start the connection
            _hubConnection.Closed += async (exception) =>
            {
                // Retry the connection every 5 seconds
                await AttemptReconnect();
            };

            // Start the connection

            await ConnectToSignalR();
        }
        private SoundPlayer? _soundPlayer;
        private async Task autoacceptorder(POSOrderModel Ord)
        {
            try
            {
                string appPath = AppDomain.CurrentDomain.BaseDirectory;

                // Point to Assets/uber_eats_sound.wav
                string soundPath = System.IO.Path.Combine(appPath, "Asset", "uber_eats_sound.wav");

                if (File.Exists(soundPath))
                {
                    _soundPlayer = new SoundPlayer(soundPath);
                    _soundPlayer.Play(); // async (non-blocking)
                                         // _soundPlayer.PlaySync();   // blocking
                                         // _soundPlayer.PlayLooping(); // loop
                }
                else
                {
                    MessageBox.Show("Sound file not found: " + soundPath);
                }
            }
            catch (Exception ex)

            {
                MessageBox.Show("Error", ex.Message);
            }

            var json = JsonConvert.SerializeObject(Ord);
            var options = new RestClientOptions(clsConnections.marketplaceserver)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest($"/Orders/acceptorder/{Ord.orderid}", Method.Put);
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            RestResponse response = await client.ExecuteAsync(request);
            Console.WriteLine(response.Content);
            if (response.IsSuccessful)
            {
                // if (localdata.autoprint == true)
                {
                    bool pr = await PrintService.PrintMarketPlaceOrder(Ord);
                }
            }
            else
            {

            }




        }
        private async Task ConnectToSignalR()
        {
            try
            {
                // Start the connection to SignalR
                await _hubConnection.StartAsync();
                _retryCount = 0;  // Reset retry count on successful connection
            }
            catch (Exception ex)
            {


                Console.WriteLine($"Failed to connect: {ex.Message}");
                // _retryCount = 0;
                // if (_retryCount < _maxRetries)
                // {
                await AttemptReconnect();
                // }
            }
        }

        private async Task AttemptReconnect()
        {

            if (_retryCount < _maxRetries)
            {
                _retryCount++;
                Console.WriteLine($"Attempting reconnect... Retry count: {_retryCount}");

                // Wait for 5 seconds before trying to reconnect
                await Task.Delay(5000);

                // Try reconnecting
                await ConnectToSignalR();
            }
            else
            {
                try
                {
                    string appPath = AppDomain.CurrentDomain.BaseDirectory;

                    // Point to Assets/uber_eats_sound.wav
                    string soundPath = System.IO.Path.Combine(appPath, "Asset", "uber_eats_sound.wav");

                    if (File.Exists(soundPath))
                    {
                        _soundPlayer = new SoundPlayer(soundPath);
                        _soundPlayer.Play(); // async (non-blocking)
                                             // _soundPlayer.PlaySync();   // blocking
                                             // _soundPlayer.PlayLooping(); // loop
                    }
                    else
                    {
                        MessageBox.Show("Sound file not found: " + soundPath);
                    }
                }
                catch (Exception ex)

                {
                    MessageBox.Show("Error", ex.Message);
                }
                MessageBox.Show("Error", "Lost Connection of the Server");
                // Application.Current.MainPage = new MainPage();




            }
        }

        private void StartBackgroundTimer()
        {
            _timerSync = new DispatcherTimer();
            _timerSync.Interval = TimeSpan.FromMinutes(5);
            _timerSync.Tick += async (sender, e) => await TimerSync_Tick();
            _timerSync.Start();
        }


        private async Task TimerSync_Tick()
        {
            await SyncService.SyncTransTOCLoud();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTimeTextBlock.Text = DateTime.Now.ToString("MMMM dd, yyyy h:mm:ss tt");
        }

        private void BtnAbout_Click(object sender, RoutedEventArgs e)
        {
            FrmAbout frmAbout = new FrmAbout();
            frmAbout.ShowDialog();
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {

        }


        private void AddButtonsToGrid()
        {
            MyUniformGrid.Children.Clear();

            using (var context = new PosDb1Context())
            {
                var users = context.TblUsers.Where(user => user.UserStatus == "OK").ToList();
                localusers = users;

                int totalUsers = users.Count;
                int maxColumns = Math.Min(5, totalUsers);
                int rows = (int)Math.Ceiling((double)totalUsers / maxColumns);

                MyUniformGrid.Columns = maxColumns;

                foreach (var user in users)
                {
                    Button newButton = new Button
                    {
                        Padding = new Thickness(10),
                        Margin = new Thickness(10),
                        MinHeight = 190,
                        Style = (Style)Application.Current.Resources["StyledUser"],
                        Tag = user
                    };

                    StackPanel stackPanel = new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };

                    Image icon = new Image
                    {
                        Width = 250,
                        Height = 120,
                        Margin = new Thickness(0, 0, 0, 10)
                    };

                    try
                    {
                        if (!string.IsNullOrEmpty(user.UserPicturePath))
                        {
                            BitmapImage userImage = new BitmapImage();
                            userImage.BeginInit();
                            userImage.UriSource = new Uri(user.UserPicturePath, UriKind.RelativeOrAbsolute);
                            userImage.CacheOption = BitmapCacheOption.OnLoad;
                            userImage.EndInit();

                            icon.Source = userImage;
                        }
                        else
                        {
                            icon.Source = new BitmapImage(new Uri("https://i.postimg.cc/sgnNqK12/cashier.png"));
                        }
                    }
                    catch
                    {
                        icon.Source = new BitmapImage(new Uri("https://i.postimg.cc/sgnNqK12/cashier.png"));
                    }

                    TextBlock textBlock = new TextBlock
                    {
                        Text = user.UserName,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        TextWrapping = TextWrapping.Wrap,
                        TextAlignment = TextAlignment.Center,
                        FontSize = 16,
                        Margin = new Thickness(0, 5, 0, 0)
                    };

                    stackPanel.Children.Add(icon);
                    stackPanel.Children.Add(textBlock);

                    newButton.Content = stackPanel;

                    newButton.Click += Button_Click;

                    MyUniformGrid.Children.Add(newButton);
                }
            }
            AdjustStackPanelAlignment(localusers.Count);
        }



        private void AdjustStackPanelAlignment(int totalUsers)
        {
            if (totalUsers <= 5)
            {
                MainStackPanel.VerticalAlignment = VerticalAlignment.Center;
            }
            else
            {
                MainStackPanel.VerticalAlignment = VerticalAlignment.Top;
            }
        }



        //private void AdjustButtonWidths(int totalUsers, int columns)
        //{
        //    if (totalUsers < columns)
        //    {
        //        MyWrapPanel.HorizontalAlignment = HorizontalAlignment.Center;
        //    }
        //    else
        //    {
        //        MyWrapPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
        //    }

        //    if (MyWrapPanel.ActualWidth <= 0) return;
        //    double buttonWidth = Math.Max((MyWrapPanel.ActualWidth / columns) - 20, 150);

        //    foreach (Button button in MyWrapPanel.Children)
        //    {
        //        button.Width = buttonWidth;
        //    }
        //}



        //private void AdjustButtonWidths()
        //{
        //    if (MyWrapPanel.ActualWidth <= 0) return;
        //    double buttonWidth = Math.Max((MyWrapPanel.ActualWidth / 4) - 20, 150);
        //    foreach (Button button in MyWrapPanel.Children)
        //    {
        //        button.Width = buttonWidth;
        //    }
        //}



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            var user = btn.Tag as TblUser;
            if (user != null)
            {
                PinPad pinPad = new PinPad(user.UserName, user.UserPicturePath);
                var result = pinPad.ShowDialog();
                if (result == true)
                {
                    Login(user.UserId, pinPad.returnvalue);
                }
            }
        }



        private async Task Login(string userid, string password)
        {
            string encryptedpass = CLSencryption.Encrypt(password);

            var user = localusers.Where(x => x.UserId == userid && x.UserPin == encryptedpass).FirstOrDefault();
            if (user == null)
            {
                FrmCustommessage frmCustommessage = new FrmCustommessage();
                frmCustommessage.LblMessage.Text = "Invalid Password";
                frmCustommessage.IsError = true;
                frmCustommessage.ShowDialog();
            }

            else
            {
                //updatePaymentData();
                AppSettingsManager.ReloadPaymentSettings();
                LoggedData.loggeduser = user;
                FrmMainmenu frmMainmenu = new FrmMainmenu();
                frmMainmenu.ShowDialog();

            }
        }

        private void updatePaymentData()
        {
            //LoggedData.tiprequest = Properties.Settings.Default.ReqTip;
            //LoggedData.comtype = Properties.Settings.Default.ComType;
            //LoggedData.PaxIP = Properties.Settings.Default.PaxIP;
            //LoggedData.PaxPort = Properties.Settings.Default.PaxPort;
            //LoggedData.PaxComPort = Properties.Settings.Default.PaxIP;
            //LoggedData.PaxBaudRate = Properties.Settings.Default.PaxPort;
        }



        private async void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            var adminUser = localusers.FirstOrDefault(u => u.UserId == "01");

            if (adminUser == null)
            {
                FrmCustommessage adminErrorMessage = new FrmCustommessage
                {
                    LblHeader = { Text = "Error" },
                    LblMessage = { Text = "Admin user not found. Cannot exit the application." },
                    IsError = true
                };
                adminErrorMessage.HeaderBorder.Background = new SolidColorBrush(Colors.Red);  // Set header border color to red
                adminErrorMessage.ShowDialog();
                return;
            }

            PinPad pinPad = new PinPad(adminUser.UserName, adminUser.UserPicturePath);
            var result = pinPad.ShowDialog();

            if (result == true)
            {
                string enteredPassword = pinPad.returnvalue;
                string encryptedPassword = CLSencryption.Encrypt(enteredPassword);

                if (encryptedPassword == adminUser.UserPin)
                {
                    Application.Current.Shutdown();
                }
                else
                {
                    FrmCustommessage passwordErrorMessage = new FrmCustommessage
                    {
                        LblHeader = { Text = "Invalid Credentials" },
                        LblMessage = { Text = "The password you entered is incorrect. Please try again." },
                        IsError = true
                    };
                    // Set the header border color to red
                    passwordErrorMessage.HeaderBorder.Background = new SolidColorBrush(Colors.Red);

                    passwordErrorMessage.ShowDialog();
                }
            }
        }



        private void BtnSettings_Click_1(object sender, RoutedEventArgs e)
        {



            FrmSettings frmSettings = new FrmSettings();
            frmSettings.ShowDialog();
            //DateTime tt = DateTime.Parse("2024-02-26 22:47:03.640");
            //MessageBox.Show(tt.ToString("MM:dd:yyyy"));



            //var options = new RestClientOptions("https://asnitagentapi.azurewebsites.net")
            //{
            //    MaxTimeout = -1,
            //};
            //var client = new RestClient(options);
            //var request = new RestRequest("/Updater/getnewversion", Method.Get);

            //RestResponse response =  client.Execute(request);

            //Console.WriteLine(response.Content);

            //if (response.IsSuccessful)
            //{
            //     ChkForUpdateResp myDeserializedClass = JsonConvert.DeserializeObject<ChkForUpdateResp>(response.Content);
            //    if (Convert.ToDecimal(myDeserializedClass.currentVersion) > Convert.ToDecimal(clsConnections.thisversion))
            //    {
            //        MessageBoxResult result = MessageBox.Show($"A new Version Found Version {myDeserializedClass.currentVersion}. Would you like to Update now?", "Confirmation",
            //                                 MessageBoxButton.YesNo, MessageBoxImage.Question);

            //        if (result == MessageBoxResult.Yes)
            //        {
            //            string exePath = @"C:\Program Files (x86)\ASN IT Inc\POSpointe\Updater\MyPOSUpdater.exe";
            //            string arguments = myDeserializedClass.currentVersion;

            //            // Create a new ProcessStartInfo instance
            //            ProcessStartInfo startInfo = new ProcessStartInfo
            //            {
            //                FileName = exePath,
            //                Arguments = arguments,
            //                // Optional settings:
            //                // If you want to capture output, set UseShellExecute to false
            //                Verb = "runas", // This ensures the EXE runs with admin rights
            //                UseShellExecute = true, // Required for "runas"
            //                                        // Redirect the standard output if needed

            //                // Optionally, redirect error output

            //                // You might also hide the console window if you don't need it
            //                //CreateNoWindow = true
            //            };

            //            try
            //            {
            //                using (Process process = new Process())
            //                {
            //                    process.StartInfo = startInfo;
            //                    process.Start();

            //                    // If you want to read the output:
            //                    string output = process.StandardOutput.ReadToEnd();
            //                    string error = process.StandardError.ReadToEnd();

            //                    // Optionally, wait for the process to exit
            //                    process.WaitForExit();

            //                    // Use the output and error as needed
            //                   // Debug.WriteLine("Output: " + output);
            //                    //Debug.WriteLine("Error: " + error);
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                // Handle exceptions such as file not found or access issues
            //                MessageBox.Show("An error occurred: " + ex.Message);
            //            }
            //        }
            //        else
            //        {

            //        }


            //    }

            //    else
            //    {

            //        MessageBox.Show("you have the latest Version");

            //    }
            //}

        }

        private async void BtnRequstsupp_Click(object sender, RoutedEventArgs e)
        {
            //bool hasPermission = LoggedData.loggeduser.RequestSupport== true;
            //if (!hasPermission)
            //{
            //    FrmPermissionShow frmPermissionShow = new FrmPermissionShow();
            //    frmPermissionShow.reqpermssion = "prmdashboard";

            //    hasPermission = frmPermissionShow.ShowDialog() == true;
            //}

            //if (hasPermission)
            //{
            //    FrmRequestSupport frmRequestSupport = new FrmRequestSupport();
            //    frmRequestSupport.ShowDialog();
            //}

            FrmMainTrouble frmMainTrouble = new FrmMainTrouble();
            frmMainTrouble.ShowDialog();

            //FrmNumbpad ff = new FrmNumbpad();
            //ff.reqestmsg = "Enter Call Back Number";
            //ff.returnvalue = LoggedData.BusinessInfo.BusinessPhone;
            //var reul = ff.ShowDialog();

            //if (reul == true)

            //{
            //    string storename = LoggedData.BusinessInfo.BusinessName  + " "+ LoggedData.BusinessInfo.CityStatezip;
            //    string cleanedString = Regex.Replace(ff.returnvalue, @"[()\[\]\-{}<>+\s]", "");
            //    if (cleanedString.Length > 10)
            //    {
            //        cleanedString = cleanedString.Substring(1);
            //    }
            //    cleanedString = "+1" + cleanedString;
            //    var options = new RestClientOptions("https://studio.twilio.com")
            //    {
            //        MaxTimeout = -1,
            //    };
            //    var client = new RestClient(options);
            //    var request = new RestRequest("/v2/Flows/FW4345ffa00a719493f577a3ccda6657ba/Executions", Method.Post);
            //    request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            //    request.AddHeader("Authorization", "Basic QUM5YmVjYmQ2ODAxYTExYWExM2ViZDYxYjMxMGRiZjA5Mjo0NzlhMzEzYTE5ZmUxNmNlYjFhYzIyNTdjYWEzY2IxYQ==");
            //    request.AddParameter("From", "+18884124405");
            //    request.AddParameter("To", "+15162128292");
            //    request.AddParameter("Parameters", "{\"msg\":\"Hi This is pospoint AI Assistant. A Tech Support Request has been Made from "+storename+". Would you like to Connect the Call?, if Yes Press 1, or , Press 2 to create a ticket and call back later.\", \"returnnumber\":\""+cleanedString+"\"}");
            //    RestResponse response = await client.ExecuteAsync(request);
            //    if (response.IsSuccessful)
            //    {
            //        FrmCustommessage frmCustommessage = new FrmCustommessage
            //        {
            //            LblHeader = { Text = "Alert" },
            //            LblMessage = { Text = $"You will receive a call Shortly" },
            //            IsError = true
            //        };
            //        frmCustommessage.ShowDialog();

            //    }

            //    else
            //    {
            //        FrmCustommessage frmCustommessage = new FrmCustommessage
            //        {
            //            LblHeader = { Text = "Error" },
            //            LblMessage = { Text = $"Error Code : {response.StatusCode} , Message : {response.Content}" },
            //            IsError = true
            //        };
            //        frmCustommessage.ShowDialog();

            //    }
            //}
        }

        //private async void BtnClock_Click(object sender, RoutedEventArgs e)
        //{

        //    string userid = "456";
        //    if (clsConnections.allowtimecard == true)
        //    {
        //        await TimeCardService.GetUserData(int.Parse(userid), "9635");
        //    }

        //    else
        //    {
        //        FrmCustommessage frmCustommessage = new FrmCustommessage
        //        {
        //            LblHeader = { Text = "Upcoming Updates" },
        //            LblMessage = { Text = "This feature will be available in an upcoming update. Thank you for your patience and support." },
        //            IsError = false
        //        };
        //        frmCustommessage.ShowDialog();
        //    }




        //}

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

        private void Btnrr_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
