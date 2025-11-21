using Pospointe.LocalData;
using Pospointe.LocalData;
using Pospointe.MainMenu;
using Pospointe.Models;
using Pospointe.Models;
using Pospointe.PosMenu;
using Pospointe.Services;
using Pospointe.Services;
using Pospointe.Services;
using Pospointe.Signup_POS;
using RestSharp;
using System;
using System.Diagnostics;
using System.Printing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;




namespace Pospointe.PointeSense;

public partial class FrmMainTrouble : Window
{

    public FrmMainTrouble()
    {
        InitializeComponent();
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        await CheckAndUpdateUI();
        await CheckPrinterQueueStatus();
        await CheckNetworkStatus();
        LoadPaxAutoFixSetting();


    }

    private async void LoadPaxAutoFixSetting()
    {
        bool isAutoFixEnabled = Convert.ToBoolean(Properties.Settings.Default["PaxAutoFix"]);
        ChkAutoFixPaxTerminal.IsChecked = isAutoFixEnabled;

        if (isAutoFixEnabled)
        {
            // Automatically run fix on load
            LblPaxTerminalStatus.Text = "🔄 Auto Fixing PAX connection...";
            bool success = await PaxHelper.AutoFixPaxConnectionIfNeeded(this);

            if (success)
            {
                LblPaxTerminalStatus.Text = "✅ Reconnected to PAX";
                LblPaxTerminalStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A5D6A7"));
            }
            else
            {
                LblPaxTerminalStatus.Text = "❌ PAX not reachable";
                LblPaxTerminalStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EF9A9A"));
            }
        }
    }


    private async void ChkAutoFixPaxTerminal_Checked(object sender, RoutedEventArgs e)
    {
        Properties.Settings.Default["PaxAutoFix"] = true;
        Properties.Settings.Default.Save();

        // Run auto-fix immediately when user enables the toggle
        LblPaxTerminalStatus.Text = "🔄 Attempting to auto-fix...";
        LblPaxTerminalStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4FC3F7"));
        BtnFixPaxTerminal.IsEnabled = false;

        bool success = await PaxHelper.AutoFixPaxConnectionIfNeeded(this);

        if (success)
        {
            LblPaxTerminalStatus.Text = "✅ Reconnected to PAX";
            LblPaxTerminalStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A5D6A7"));
        }
        else
        {
            LblPaxTerminalStatus.Text = "❌ Could not detect any PAX terminal";
            LblPaxTerminalStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EF9A9A"));
        }

        BtnFixPaxTerminal.IsEnabled = true;
    }

    private void ChkAutoFixPaxTerminal_Unchecked(object sender, RoutedEventArgs e)
    {
        Properties.Settings.Default["PaxAutoFix"] = false;
        Properties.Settings.Default.Save();

        LblPaxTerminalStatus.Text = "🟡 Auto-Fix disabled";
        LblPaxTerminalStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC107"));
    }





    private async Task CheckAndUpdateUI()
    {
        try
        {
            bool updateAvailable = await Task.Run(() => UpdateService.CheckForUpdate());

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (updateAvailable)
                {
                    LblUpdateStatus.Text = "Updates pending";
                    LblUpdateStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC107"));
                    BtnRunUpdate.Visibility = Visibility.Visible;
                }
                else
                {
                    LblUpdateStatus.Text = "Up to date";
                    LblUpdateStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A5D6A7"));
                    BtnRunUpdate.Visibility = Visibility.Collapsed;
                }
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error while checking for updates:\n{ex.Message}");
        }
    }


    private void BtnRunUpdate_Click(object sender, RoutedEventArgs e)
    {
        BtnRunUpdate.IsEnabled = false;
        LblUpdateStatus.Text = "Updating...";

        try
        {
            UpdateService.Update("Latest");
            LblUpdateStatus.Text = "Update started";
            LblUpdateStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A5D6A7"));
        }
        catch (Exception ex)
        {
            MessageBox.Show("Update failed: " + ex.Message);
            LblUpdateStatus.Text = " Update failed";
            LblUpdateStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCDD2"));
        }

        BtnRunUpdate.IsEnabled = true;
    }

    private async Task CheckPrinterQueueStatus()
    {
        try
        {
            var status = await Task.Run(() =>
            {
                int totalJobs = 0;

                LocalPrintServer localPrintServer = new LocalPrintServer();
                PrintQueueCollection printQueues = localPrintServer.GetPrintQueues();

                foreach (PrintQueue pq in printQueues)
                {
                    pq.Refresh();
                    totalJobs += pq.NumberOfJobs;
                }

                return totalJobs;
            });

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (status > 0)
                {
                    LblPrinterQueueStatus.Text = $"⚠️ {status} job(s) pending";
                    LblPrinterQueueStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC107"));
                }
                else
                {
                    LblPrinterQueueStatus.Text = $"✅ No print jobs";
                    LblPrinterQueueStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A5D6A7"));
                }
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show("Failed to check printer queues: " + ex.Message);
        }
    }


    private void BtnClearPrintQueue_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/C net stop spooler && del /Q %systemroot%\\System32\\spool\\PRINTERS\\* && net start spooler",
                Verb = "runas",
                CreateNoWindow = true,
                UseShellExecute = true
            };

            Process.Start(psi);

            LblPrinterQueueStatus.Text = "🔄 Queues cleared, rechecking...";
            LblPrinterQueueStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4FC3F7"));

            _ = CheckPrinterQueueStatus(); // Recheck after clearing
        }
        catch (Exception ex)
        {
            MessageBox.Show("Failed to clear print queues: " + ex.Message);
        }
    }

    private async void BtnRunNetworkTest_Click(object sender, RoutedEventArgs e)
    {
        await CheckNetworkStatus();
    }


    private async Task CheckNetworkStatus()
    {
        try
        {
            LblNetworkStatus.Text = "🔄 Testing...";

            bool isInternetAvailable = await Task.Run(() => PingHost("8.8.8.8"));
            bool isServerReachable = await Task.Run(() => PingHost("asnitagentapi.azurewebsites.net"));

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (isInternetAvailable && isServerReachable)
                {
                    LblNetworkStatus.Text = "✅ Connected";
                    LblNetworkStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A5D6A7")); // Green
                }
                else if (isInternetAvailable)
                {
                    LblNetworkStatus.Text = "⚠️ Internet OK, POS Server Unreachable";
                    LblNetworkStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC107")); // Yellow
                }
                else
                {
                    LblNetworkStatus.Text = "❌ No Internet Connection";
                    LblNetworkStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EF9A9A")); // Red
                }
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error checking network status:\n" + ex.Message);
        }
    }

    private bool PingHost(string host)
    {
        try
        {
            using (var ping = new System.Net.NetworkInformation.Ping())
            {
                var reply = ping.Send(host, 2000);
                return reply.Status == System.Net.NetworkInformation.IPStatus.Success;
            }
        }
        catch
        {
            return false;
        }
    }

    private async void BtnFixPaxTerminal_Click(object sender, RoutedEventArgs e)
    {
        BtnFixPaxTerminal.IsEnabled = false;
        LblPaxTerminalStatus.Text = "🔄 Attempting fix...";

        bool success = await PaxHelper.AutoFixPaxConnectionIfNeeded(this);

        if (success)
        {
            LblPaxTerminalStatus.Text = "✅ Reconnected to PAX";
            LblPaxTerminalStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A5D6A7"));
        }
        else
        {
            LblPaxTerminalStatus.Text = "❌ PAX not reachable";
            LblPaxTerminalStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EF9A9A"));
        }

        BtnFixPaxTerminal.IsEnabled = true;
    }

    private async void BtnReqSupprt_Click(object sender, RoutedEventArgs e)
    {
        FrmNumbpad ff = new FrmNumbpad();
        ff.reqestmsg = "Enter Call Back Number";
        ff.returnvalue = LoggedData.BusinessInfo.BusinessPhone;
        var reul = ff.ShowDialog();

        if (reul == true)

        {
            string storename = LoggedData.BusinessInfo.BusinessName + " " + LoggedData.BusinessInfo.CityStatezip;
            string cleanedString = Regex.Replace(ff.returnvalue, @"[()\[\]\-{}<>+\s]", "");
            if (cleanedString.Length > 10)
            {
                cleanedString = cleanedString.Substring(1);
            }
            cleanedString = "+1" + cleanedString;
            var options = new RestClientOptions("https://studio.twilio.com")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/v2/Flows/FW4345ffa00a719493f577a3ccda6657ba/Executions", Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", "Basic QUM5YmVjYmQ2ODAxYTExYWExM2ViZDYxYjMxMGRiZjA5Mjo0NzlhMzEzYTE5ZmUxNmNlYjFhYzIyNTdjYWEzY2IxYQ==");
            request.AddParameter("From", "+18884124405");
            request.AddParameter("To", "+15162128292");
            request.AddParameter("Parameters", "{\"msg\":\"Hi This is pospoint AI Assistant. A Tech Support Request has been Made from " + storename + ". Would you like to Connect the Call?, if Yes Press 1, or , Press 2 to create a ticket and call back later.\", \"returnnumber\":\"" + cleanedString + "\"}");
            RestResponse response = await client.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                FrmCustommessage frmCustommessage = new FrmCustommessage
                {
                    LblHeader = { Text = "Alert" },
                    LblMessage = { Text = $"You will receive a call Shortly" },
                    IsError = true
                };
                frmCustommessage.ShowDialog();

            }

            else
            {
                FrmCustommessage frmCustommessage = new FrmCustommessage
                {
                    LblHeader = { Text = "Error" },
                    LblMessage = { Text = $"Error Code : {response.StatusCode} , Message : {response.Content}" },
                    IsError = true
                };
                frmCustommessage.ShowDialog();

            }
        }
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void BtnRemoteAccess(object sender, RoutedEventArgs e)
    {
        FrmGetsupport GetSupport = new FrmGetsupport();
        GetSupport.ShowDialog();
    }
}
