using Pospointe.LocalData;
using Pospointe.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pospointe
{
    public static class PaxHelper 
    {
        private const int PaxPort = 10009;
        private const int ScanTimeout = 400;
        private static Window _fixingWindow;

        public static async Task<bool> AutoFixPaxConnectionIfNeeded(Window owner = null)
        {
            string subnet = GetLocalSubnet();
            if (string.IsNullOrEmpty(subnet))
                return false;

            if (owner is Pospointe.PosMenu.FrmPaxscreen paxScreen)
                paxScreen.UpdateStatus("Attempting to fix PAX connection...");


            List<Task<string>> scanTasks = new();
            for (int i = 1; i < 255; i++)
            {
                string testIP = $"{subnet}.{i}";
                scanTasks.Add(CheckPaxDevice(testIP, PaxPort));
            }

            string[] results = await Task.WhenAll(scanTasks);
            string foundIP = results.FirstOrDefault(ip => !string.IsNullOrEmpty(ip));

            if (owner is Pospointe.PosMenu.FrmPaxscreen paxScreen1)
                paxScreen1.UpdateStatus(" Could not find PAX terminal.");


            if (!string.IsNullOrEmpty(foundIP))
            {
                Settings.Default.PaxIP = foundIP;
                Settings.Default.Save();
                LoggedData.PaxIP = foundIP;

                //MessageBox.Show($"PAX terminal reconnected at {foundIP}. Please retry the transaction.", "Fixed", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }

            MessageBox.Show(" Could not detect any working PAX terminal on your network.", "Auto-Fix Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        private static async Task<string> CheckPaxDevice(string ip, int port)
        {
            using TcpClient client = new();
            try
            {
                var connectTask = client.ConnectAsync(ip, port);
                bool connected = await Task.WhenAny(connectTask, Task.Delay(ScanTimeout)) == connectTask;
                return (connected && client.Connected) ? ip : "";
            }
            catch
            {
                return "";
            }
        }

        private static string GetLocalSubnet()
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

        //private static void ShowFixingDialog(Window owner, string message)
        //{
        //    _fixingWindow = new Window
        //    {
        //        Width = 360,
        //        Height = 150,
        //        WindowStyle = WindowStyle.ToolWindow,
        //        ResizeMode = ResizeMode.NoResize,
        //        WindowStartupLocation = WindowStartupLocation.CenterOwner,
        //        Background = System.Windows.Media.Brushes.White,
        //        Owner = owner,
        //        Title = "Fixing PAX Connection...",
        //        Content = new StackPanel
        //        {
        //            Children =
        //            {
        //                new TextBlock
        //                {
        //                    Text = message,
        //                    FontSize = 18,
        //                    Margin = new Thickness(20),
        //                    FontWeight = FontWeights.SemiBold,
        //                    TextAlignment = TextAlignment.Center
        //                },
        //                new ProgressBar
        //                {
        //                    IsIndeterminate = true,
        //                    Height = 20,
        //                    Margin = new Thickness(30, 0, 30, 10)
        //                }
        //            }
        //        },
        //        Topmost = true,
        //        ShowInTaskbar = false
        //    };
        //    _fixingWindow.Show();
        //}

        //private static void CloseFixingDialog()
        //{
        //    Application.Current.Dispatcher.Invoke(() =>
        //    {
        //        _fixingWindow?.Close();
        //        _fixingWindow = null;
        //    });
        //}
    }
}
