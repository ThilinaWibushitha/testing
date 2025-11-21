using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Pospointe.LoginWindow
{
    /// <summary>
    /// Interaction logic for FrmPaymentprocess.xaml
    /// </summary>
    public partial class FrmPaymentprocess : UserControl
    {
        private const int PaxPort = 10009; // PAX A35 Port
        private const int ScanTimeout = 500;
        public FrmPaymentprocess()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            bool autoUpdateEnabled = Properties.Settings.Default.ReqTip;
            ChkEnableTip.IsChecked = autoUpdateEnabled;

            ChkCusCD.IsChecked = Properties.Settings.Default.PAXCustomerDisplay;
            string savedMethod = Properties.Settings.Default.ComType;
            string savedIP = Properties.Settings.Default.PaxIP;
            string savedPort = Properties.Settings.Default.PaxPort;

            ChkEnableTip.IsChecked = Properties.Settings.Default.ReqTip;

            foreach (ComboBoxItem item in CmbCCMethod.Items)
            {
                if (item.Content.ToString().Equals(savedMethod, StringComparison.OrdinalIgnoreCase))
                {
                    CmbCCMethod.SelectedItem = item;
                    break;
                }
            }

            TxtIPAddress.Text = savedIP;
            TxtPortNum.Text = savedPort;
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {


        }

        private void BtnUpdate_Click_1(object sender, RoutedEventArgs e)
        {
            if (CmbCCMethod.SelectedItem is ComboBoxItem selectedItem)
            {
                Properties.Settings.Default.ComType = selectedItem.Content.ToString();
            }

            Properties.Settings.Default.ReqTip = ChkEnableTip.IsChecked == true;
            Properties.Settings.Default.PAXCustomerDisplay = ChkCusCD.IsChecked == true;
            Properties.Settings.Default.PaxIP = TxtIPAddress.Text.Trim();
            Properties.Settings.Default.PaxPort = TxtPortNum.Text.Trim();
            Properties.Settings.Default.Save();
            //LoggedData.PaxIP = Properties.Settings.Default.PaxIP;
            AppSettingsManager.ReloadPaymentSettings();
            ShowPaymentMessage("Payment settings updated successfully!", false);
        }
        private void ShowPaymentMessage(string message, bool isError)
        {
            FrmCustommessage customMessageBox = new FrmCustommessage
            {
                LblHeader = { Text = "Payment Settings Update" },
                LblMessage = { Text = message },
                IsError = isError
            };

            customMessageBox.ShowDialog();
        }

        private async void BtnDetectPAX_Click(object sender, RoutedEventArgs e)
        {
            BtnDetectPAX.IsEnabled = false;
            BtnDetectPAX.Content = "Scanning...";

            string detectedIP = await ScanForPaxA35();

            if (!string.IsNullOrEmpty(detectedIP))
            {
                TxtIPAddress.Text = detectedIP;
                Properties.Settings.Default.PaxIP = detectedIP;
                Properties.Settings.Default.Save();
                ShowPaymentMessage($"✅ PAX Terminal detected at {detectedIP}!", false);
            }
            else
            {
                ShowPaymentMessage("❌ No PAX terminal found on the network!", true);
            }

            BtnDetectPAX.Content = "Detect PAX";
            BtnDetectPAX.IsEnabled = true;
        }

        private async Task<string> ScanForPaxA35()
        {
            string subnet = GetLocalSubnet();
            if (string.IsNullOrEmpty(subnet))
            {
                return "";
            }

            List<Task<string>> scanTasks = new List<Task<string>>();
            for (int i = 1; i < 255; i++)
            {
                string ipAddress = $"{subnet}.{i}";
                scanTasks.Add(CheckPaxDevice(ipAddress, PaxPort));
            }

            string[] results = await Task.WhenAll(scanTasks);
            return results.FirstOrDefault(ip => !string.IsNullOrEmpty(ip)) ?? "";
        }

        private async Task<string> CheckPaxDevice(string ipAddress, int port)
        {
            using (TcpClient client = new TcpClient())
            {
                try
                {
                    Task connectTask = client.ConnectAsync(ipAddress, port);
                    bool connected = await Task.WhenAny(connectTask, Task.Delay(ScanTimeout)) == connectTask;

                    if (connected && client.Connected)
                    {
                        return ipAddress;
                    }
                }
                catch { }
            }
            return "";
        }

        private string GetLocalSubnet()
        {
            foreach (var ip in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    string[] parts = ip.ToString().Split('.');
                    return $"{parts[0]}.{parts[1]}.{parts[2]}";
                }
            }
            return "";
        }

        private void ChkEnableTip_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ReqTip = true;
            Properties.Settings.Default.Save();
        }

        private void ChkEnableTip_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ReqTip = false;
            Properties.Settings.Default.Save();
        }

        private async void Chkfix_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowPaymentMessage("🔍 Auto Fixing PAX connection... Please wait.", false);
                bool fixedSuccess = await Pospointe.PaxHelper.AutoFixPaxConnectionIfNeeded(Window.GetWindow(this));

                if (fixedSuccess)
                {
                    TxtIPAddress.Text = Properties.Settings.Default.PaxIP;
                    ShowPaymentMessage($"✅ PAX connection fixed successfully! IP: {Properties.Settings.Default.PaxIP}", false);
                }
                else
                {
                    ShowPaymentMessage("❌ Auto Fix failed. Could not detect any PAX terminal.", true);
                }
            }
            catch (Exception ex)
            {
                ShowPaymentMessage($"⚠️ Error during Auto Fix: {ex.Message}", true);
            }
        }

        private void Chkfix_Unchecked(object sender, RoutedEventArgs e)
        {
            // Optional: you can disable auto-fix mode or just reset something here.
            ShowPaymentMessage("🟡 Auto Fix turned off.", false);
        }

        private void CmbCCMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbCCMethod.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedText = selectedItem.Content.ToString() ?? string.Empty;

                if (selectedText == "PAX_IP")
                {
                    TxtPortNum.Text = "10009";
                }
                else
                {
                    TxtPortNum.Clear();
                }
                bool isManualExpress = selectedText == "Manual Express";
                TxtIPAddress.IsReadOnly = isManualExpress;
                TxtPortNum.IsReadOnly = isManualExpress;

                TxtIPAddress.Background = isManualExpress ? System.Windows.Media.Brushes.LightGray : System.Windows.Media.Brushes.White;
                TxtPortNum.Background = isManualExpress ? System.Windows.Media.Brushes.LightGray : System.Windows.Media.Brushes.White;
            }
        }

    }
}
