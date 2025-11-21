using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Pospointe.LocalData;
using Pospointe.Models;
using Pospointe.PosMenu;
using Pospointe.Properties;
using Pospointe.Services;
using QRCoder;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using static Pospointe.Signup_POS.FrmSignup;

namespace Pospointe.Signup_POS
{
    /// <summary>
    /// Interaction logic for SignupPage.xaml
    /// </summary>
    public partial class SignupPage : Page
    {
        string randomCode = "";
        private HubConnection _hubConnection;
        private DispatcherTimer networkCheckTimer;
        private int _retryCount = 0;
        private const int _maxRetries = 15;
        public SignupPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            randomCode = GenerateRandomCode(8);
            TxtCode.Text = randomCode;
            await connectoserver();
            await genQRCodeAsync();
        }



        private async Task genQRCodeAsync()
        {
            string qrText = "https://mypospointe.com/myPOS?devicetoken=" + randomCode;

            // Generate the QR code
            using (var qrGenerator = new QRCodeGenerator())
            using (var qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q))
            using (var qrCode = new QRCode(qrCodeData))
            using (var bitmap = qrCode.GetGraphic(20))
            {
                // Convert Bitmap to BitmapImage and set it as Image Source
                QrCodeImage.Source = ConvertToBitmapImage(bitmap);
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
                await AttemptReconnect();
            }
        }

        private BitmapImage ConvertToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            using (var memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                memoryStream.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
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
                MessageBox.Show("Max retries reached. Could not reconnect.");
            }
        }

        private async Task connectoserver()
        {
            // Initialize the SignalR connection
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://mypostwowaylogin.azurewebsites.net/notificationhub?deviceId=" + randomCode)  // Replace with your API URL and deviceId
                .Build();

            // Define a handler for receiving notifications
            _hubConnection.On<string>("ReceiveNotification", (message) =>
            {
                // When a message is received, show a message box or other UI interaction
                //await Application.Current.MainPage.DisplayAlert("Login Failed", message, "OK");
                // model.email = message;
                if (message != null)
                {
                    MyLoginRequest my = JsonConvert.DeserializeObject<MyLoginRequest>(message);

                    openmainwindow(my.Dbid);


                }

            });

            // Start the connection
            _hubConnection.Closed += async (exception) =>
            {
                // Retry the connection every 5 seconds
                // await AttemptReconnect();
            };

            // Start the connection

            await ConnectToSignalR();
        }

        private void openmainwindow(string db)
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                try
                {
                    // MessageBox.Show($"Connecting to DB - {db}");

                    /// PUT REQUEST 
                    Register reg = await Getlicense(db);
                    await UpdateLicense(db, reg);

                    await _hubConnection.StopAsync();

                    FrmStartupSettings frmStartupSettings = new FrmStartupSettings();
                    frmStartupSettings.Show();
                    

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            });
        }

        private async Task<Register> Getlicense(string db)
        {

            var options = new RestClientOptions("https://asnitagentapi.azurewebsites.net")
            {
                MaxTimeout = -1,
            };

            var client = new RestClient(options);
            var request = new RestRequest("/POSLicense/" + db, Method.Get);

            RestResponse response = await client.ExecuteAsync(request);
            Console.WriteLine(response.Content);
            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<Register>(response.Content);

            }
            else
            {
                MessageBox.Show(response.Content);
                return null;
            }
        }

        static string GenerateRandomCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();

            return new string(Enumerable.Repeat(chars, length)
                                        .Select(s => s[random.Next(s.Length)])
                                        .ToArray());
        }

        private string GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return "No Network Available";
            }
            catch (Exception)
            {
                return "Error retrieving IP";
            }
        }

        private string GetMacAddress()
        {
            try
            {
                var networkInterface = NetworkInterface.GetAllNetworkInterfaces()
                    .FirstOrDefault(nic => nic.OperationalStatus == OperationalStatus.Up &&
                                           nic.NetworkInterfaceType != NetworkInterfaceType.Loopback);
                return networkInterface?.GetPhysicalAddress()?.ToString() ?? "MAC Not Available";
            }
            catch (Exception)
            {
                return "Error retrieving MAC";
            }
        }

        //public static string GetMacAddress()
        //{
        //    var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

        //    // Get the first operational MAC address
        //    foreach (var networkInterface in networkInterfaces)
        //    {
        //        if (networkInterface.OperationalStatus == OperationalStatus.Up &&
        //            networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
        //            networkInterface.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
        //        {
        //            return BitConverter.ToString(networkInterface.GetPhysicalAddress().GetAddressBytes());
        //        }
        //    }

        //    return "No MAC address found.";
        //}

        public static async Task<string> GetPublicIpAddressAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Use a reliable service to fetch the public IP address
                    string url = "https://api.ipify.org"; // Or "https://checkip.amazonaws.com"
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string publicIp = await response.Content.ReadAsStringAsync();
                    return publicIp;
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    return $"Error: {ex.Message}";
                }
            }
        }

        private async Task UpdateLicense(string db, Register reg)
        {

            ////////
            reg.deviceMac = GetMacAddress();
            reg.localIp = GetLocalIPAddress();
            reg.publicIp = await GetPublicIpAddressAsync();
            reg.deviceId = ("");
            reg.runningVersion = clsConnections.thisversion;



            var json = JsonConvert.SerializeObject(reg);
            //MessageBox.Show(reg.deviceMac);
            // MessageBox.Show(reg.publicIp);
            // MessageBox.Show(reg.localIp);
            var options = new RestClientOptions("https://asnitagentapi.azurewebsites.net")
            {
                MaxTimeout = -1,
            };

            var client = new RestClient(options);
            var request = new RestRequest("/POSLicense/register/" + db, Method.Put);
            request.AddHeader("Content-Type", "application/json");


            request.AddParameter("application/json", json, ParameterType.RequestBody);
            RestResponse response = await client.ExecuteAsync(request);

            Console.WriteLine(response.Content);
            if (response.IsSuccessful)
            {
                SaveLicenseFile(reg);

            }
            else
            {
                MessageBox.Show("Error: " + response.Content);
            }

        }

        public static void SaveLicenseFile(Register reg)
        {
            // Serialize the object to JSON
            var json = JsonConvert.SerializeObject(reg);

            // Encode the JSON string to Base64
            var encryptedstring = CLSencryption.Encrypt(json);

            // Get the ProgramData path
            var programDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            // Define the POSpointe folder and file paths
            var posPointeFolder = System.IO.Path.Combine(programDataPath, "POSpointe");
            var licenseFilePath = System.IO.Path.Combine(posPointeFolder, "pospointe.lic");

            // Ensure the folder exists
            if (!Directory.Exists(posPointeFolder))
            {
                Directory.CreateDirectory(posPointeFolder);
            }

            // Save the encoded data to the file
            File.WriteAllText(licenseFilePath, encryptedstring);
        }


        public class MyLoginRequest
        {
            public string? Dbid { get; set; }
            public string? Profile { get; set; }
            public string? StationID { get; set; }
            public bool? skipsettings { get; set; }
            public bool? Forceupdate { get; set; }

            public string? AuthHead { get; set; }
        }

        private void BtnGetsupport_Loaded(object sender, RoutedEventArgs e)
        {
            var storyboard = (Storyboard)this.Resources["BlinkAnimation"];
            Storyboard.SetTarget(storyboard, BtnGetsupport);
            storyboard.Begin();
        }




        private void Btnen_Click(object sender, RoutedEventArgs e)
        {
            openmainwindow("12");
        }

        private void BtnGetsupport_Click(object sender, RoutedEventArgs e)
        {
            FrmGetsupport frmget = new FrmGetsupport();
            frmget.ShowDialog();
        }
    }
}
