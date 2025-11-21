using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Pospointe.Signup_POS
{
    /// <summary>
    /// Interaction logic for FrmGetsupport.xaml
    /// </summary>
    public partial class FrmGetsupport : Window
    {
        private string _anyDeskPath;
        private string _rustDeskPath;

        public FrmGetsupport()
        {
            InitializeComponent();
        }

        private void BtnInstall_Click(object sender, RoutedEventArgs e)
        {
            string exePath = Path.Combine(AppContext.BaseDirectory, "Support_Runner", "ASN_RemoteSupporter.exe");

            if (File.Exists(exePath))
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = exePath,
                    UseShellExecute = true
                };
                Process.Start(startInfo);
            }
            else
            {
                MessageBox.Show("ASN_RemoteSupporter.exe not found in application directory.");
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var (asnInstalled, asnRunning) = GetServiceState("ASNAgent");
            bool asnAvailable = asnInstalled && asnRunning;

            SetStatus(AnyDeskStatus1, asnAvailable);

            if (asnAvailable)
            {
                //BtnInstall.Content = "Run Now";
                //TxtCode.Visibility = Visibility.Hidden;
                //TxtPIN.Visibility = Visibility.Hidden;
            }
            else
            {
                //BtnInstall.Content = "Install now";
                // Keep IDs visible or hide — your call
                // TxtCode.Visibility = Visibility.Hidden;
                // TxtPIN.Visibility = Visibility.Hidden;
            }

            // AnyDesk discovery
            {
                var result = FindTool(new[]
                {
                    "AnyDesk.exe", "AnyDesk-Portable.exe",
                    Path.Combine("Support_Runner", "AnyDesk.exe"),
                    Path.Combine("Support_Runner", "AnyDesk-Portable.exe")
                });
                _anyDeskPath = result.path;
                bool anyDeskAvailable = result.exists;
                SetStatus(AnyDeskStatus, anyDeskAvailable);
            }

            // RustDesk discovery
            {
                var result = FindTool(new[]
                {
                    "rustdesk.exe", "RustDesk.exe",
                    Path.Combine("Support_Runner", "rustdesk.exe"),
                    Path.Combine("Support_Runner", "RustDesk.exe")
                });
                _rustDeskPath = result.path;
                bool rustDeskAvailable = result.exists;
                SetStatus(RustDeskStatus, rustDeskAvailable);
            }
        }

        private static (string path, bool exists) FindTool(string[] candidateRelativePaths)
        {
            string baseDir = AppContext.BaseDirectory;
            foreach (var rel in candidateRelativePaths)
            {
                var full = Path.Combine(baseDir, rel);
                if (File.Exists(full))
                    return (full, true);
            }
            return (null, false);
        }

        private void AnyDesk_Click(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(_anyDeskPath) && File.Exists(_anyDeskPath))
            {
                Process.Start(new ProcessStartInfo { FileName = _anyDeskPath, UseShellExecute = true });
            }
            else
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://anydesk.com/en/downloads",
                    UseShellExecute = true
                });
            }
        }

        private void RustDesk_Click(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(_rustDeskPath) && File.Exists(_rustDeskPath))
            {
                Process.Start(new ProcessStartInfo { FileName = _rustDeskPath, UseShellExecute = true });
            }
            else
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://github.com/rustdesk/rustdesk/releases/download/1.4.1/rustdesk-1.4.1-x86_64.exe",
                    UseShellExecute = true
                });
            }
        }

        private static void SetStatus(TextBlock label, bool available)
        {
            label.Text = available ? "Available" : "Unavailable";
            label.Foreground = new SolidColorBrush(available ? Colors.LightGreen : Colors.OrangeRed);
        }

        private static (bool installed, bool running) GetServiceState(string serviceName)
        {
            try
            {
                var svc = ServiceController.GetServices()
                    .FirstOrDefault(s => string.Equals(s.ServiceName, serviceName, StringComparison.OrdinalIgnoreCase));

                if (svc == null) return (false, false);
                return (true, svc.Status == ServiceControllerStatus.Running);
            }
            catch
            {
                // Treat as not installed/not running if anything goes wrong
                return (false, false);
            }
        }
    }
}
