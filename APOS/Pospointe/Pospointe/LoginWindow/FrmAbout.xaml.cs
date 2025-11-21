using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;
using System.Management;
using System.Windows;
using System.Diagnostics;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using Pospointe.LocalData;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using Microsoft.Data.SqlClient;

namespace Pospointe.LoginWindow
{
    public partial class FrmAbout : Window
    {
        private PerformanceCounter cpuCounter;
        private DispatcherTimer timer;
        private BackgroundWorker statsWorker;

        public FrmAbout()
        {
            InitializeComponent();
            InitializePlaceholderText();
            this.Loaded += Window_Loaded;
        }

        private void InitializePlaceholderText()
        {
            
            SetLoadingState();
        }

        private void SetLoadingState()
        {
            LblCompname.Text = "Loading...";
            LblLocalIp.Text = "Loading...";
            LblMacaddress.Text = "Loading...";
            LblPublicIp.Text = "Loading...";
            LblCpuUsage.Text = "Loading...";
            LblMemoryUsage.Text = "Loading...";
            LblLicense.Text = "Loading...";
            LblVersion.Text = "Loading...";
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializePerformanceMonitoring();
            await LoadDataAsync().ConfigureAwait(false);
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        private void InitializePerformanceMonitoring()
        {
            statsWorker = new BackgroundWorker();
            statsWorker.DoWork += (s, e) =>
            {
                cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    timer = new DispatcherTimer(DispatcherPriority.Background)
                    {
                        Interval = TimeSpan.FromSeconds(1)
                    };
                    timer.Tick += UpdatePerformanceStats;
                    timer.Start();
                }), DispatcherPriority.Background);
            };
            statsWorker.RunWorkerAsync();
        }

        private void UpdatePerformanceStats(object sender, EventArgs e)
        {
            try
            {
                float cpuUsage = cpuCounter?.NextValue() ?? 0;
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    LblCpuUsage.Text = $"{cpuUsage:F1}%";
                }));

                var (totalMemory, availableMemory) = GetMemoryInfo();
                float usedMemory = totalMemory - availableMemory;
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    LblMemoryUsage.Text = $"{usedMemory:F1} MB / {totalMemory:F1} MB";
                }));
            }
            catch
            {

            }
        }

        private (float TotalMemory, float AvailableMemory) GetMemoryInfo()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");
                using var availableMemoryCounter = new PerformanceCounter("Memory", "Available MBytes");

                float totalMemory = 0;
                foreach (var obj in searcher.Get())
                {
                    totalMemory = (float)(Convert.ToDouble(obj["TotalPhysicalMemory"]) / (1024 * 1024));
                }

                float availableMemory = availableMemoryCounter.NextValue();
                return (totalMemory, availableMemory);
            }
            catch
            {
                return (0, 0);
            }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var computerName = Environment.MachineName;
                var localIp = await Task.Run(GetLocalIPAddress).ConfigureAwait(false);
                var macAddress = await Task.Run(GetMacAddress).ConfigureAwait(false);
                var publicIp = await GetCachedPublicIPAddressAsync().ConfigureAwait(false);

                int localTransactionCount = 0;
                int holdTransactionCount = 0;

                await Task.Run(() =>
                {
                    var connectionString = "Server=localhost\\POSPOINTE;Database=PosDB;User Id=sa;Password=POS@573184;TrustServerCertificate=True";
                    using var connection = new SqlConnection(connectionString);
                    connection.Open();

                    using var command = new SqlCommand("SELECT COUNT(*) FROM Tbl_Trans_Main WHERE Transtype = 'SALE'", connection);
                    localTransactionCount = (int)command.ExecuteScalar();

                    using var holdCommand = new SqlCommand("SELECT COUNT(*) FROM Tbl_Trans_Main WHERE Transtype = 'HOLD'", connection);
                    holdTransactionCount = (int)holdCommand.ExecuteScalar();
                });

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    LblCompname.Text = computerName;
                    LblLocalIp.Text = localIp;
                    LblMacaddress.Text = macAddress;
                    LblPublicIp.Text = publicIp;
                    LblLicense.Text = LoggedData.Reg.registrationCode;
                    LblLicenseType.Text = LoggedData.Reg.registerType;
                    LblVersion.Text = clsConnections.thisversion;
                    LblLocalTransactionCount.Text = localTransactionCount.ToString();
                    LblLocalHoldCount.Text = holdTransactionCount.ToString();
                }));
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    MessageBox.Show($"Error loading data: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }));
            }
        }


        private string GetLocalIPAddress()
        {
            try
            {
                return Dns.GetHostEntry(Dns.GetHostName())
                    .AddressList
                    .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)
                    ?.ToString() ?? "No Network";
            }
            catch
            {
                return "IP Unavailable";
            }
        }

        private string GetMacAddress()
        {
            try
            {
                return NetworkInterface.GetAllNetworkInterfaces()
                    .Where(nic => nic.OperationalStatus == OperationalStatus.Up &&
                                  nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    .Select(nic => nic.GetPhysicalAddress().ToString())
                    .FirstOrDefault() ?? "MAC Unavailable";
            }
            catch
            {
                return "MAC Unavailable";
            }
        }

        private async Task<string> GetCachedPublicIPAddressAsync()
        {
            const string cacheFile = "PublicIPCache.txt";

            try
            {
                if (File.Exists(cacheFile))
                {
                    using var reader = new StreamReader(cacheFile);
                    var cachedIp = await reader.ReadToEndAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(cachedIp)) return cachedIp;
                }

                var publicIp = await GetPublicIPAddressAsync().ConfigureAwait(false);

                await using var writer = new StreamWriter(cacheFile, append: false);
                await writer.WriteAsync(publicIp);

                return publicIp;
            }
            catch
            {
                return "IP Unavailable";
            }
        }

        private async Task<string> GetPublicIPAddressAsync()
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(5);
                var ip = await httpClient.GetStringAsync("https://api.ipify.org");
                return ip.Trim();
            }
            catch (Exception ex)
            {
                File.WriteAllText("PublicIPError.log", ex.ToString());
                return "IP Unavailable";
            }
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            timer?.Stop();
            cpuCounter?.Dispose();
            statsWorker?.Dispose();
            base.OnClosing(e);
        }

        private void OpenLink(string url)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true 
                });
            }
            catch (Exception ex)
            {
                ShowCustomErrorMessage($"Failed to open link: {ex.Message}");
            }
        }

        private void ShowCustomErrorMessage(string message)
        {
            FrmCustommessage errorMessageBox = new FrmCustommessage
            {
                LblHeader = { Text = "Error" },
                LblMessage = { Text = message },
                IsError = true
            };
            errorMessageBox.ShowDialog();
        }

        private void Btnclose_Click(object sender, RoutedEventArgs e) => Close();
        private void ViewLicense_Click(object sender, RoutedEventArgs e) => OpenLink("https://pospointe.com/docs/license");
        private void TroubleshootingGuide_Click(object sender, RoutedEventArgs e) => OpenLink("https://pospointe.com/docs/troubleshooting");
        private void FAQ_Click(object sender, RoutedEventArgs e) => OpenLink("https://pospointe.com/docs/faq");
    }
}
