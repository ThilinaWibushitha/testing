using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pospointe.LoginWindow
{
    public partial class UpdateAvailableView : UserControl
    {
        private string _version;
        private string _exePath = @"C:\Program Files (x86)\ASN IT Inc\POSpointe\Updater\MyPOSUpdater.exe";

        public UpdateAvailableView(string version)
        {
            InitializeComponent();
            _version = version;
            TxtVersion.Text = "Version " + version;
        }

        private void BtnUpdateOvernight_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime now = DateTime.Now;
                DateTime nextRunTime = now.Date.AddDays(1).AddHours(4); // 4:00 AM next day

                if (now.Hour < 4) 
                {
                    nextRunTime = now.Date.AddHours(4); // If it's before 4 AM, schedule for today at 4 AM
                }

                ScheduleUpdateTask(nextRunTime);
                MessageBox.Show("Update scheduled for 4:00 AM.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error scheduling overnight update: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ScheduleUpdateTask(DateTime runTime)
        {
            string taskName = "POSpointe Update";
            string logFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "update_log.txt");
            string formattedDate = runTime.ToString("MM/dd/yyyy");
            string formattedTime = runTime.ToString("HH:mm");

            if (!File.Exists(_exePath))
            {
                File.AppendAllText(logFilePath, "Updater file not found!\n");
                return;
            }

            string arguments = $"/create /tn \"{taskName}\" /tr \"\\\"{_exePath}\\\"\" " +
                               $"/sc once /st {formattedTime} /sd {formattedDate} /rl highest /f";

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo("schtasks", arguments)
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    Verb = "runas"
                };

                using (Process process = new Process { StartInfo = psi })
                {
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(error))
                        File.AppendAllText(logFilePath, "Task Scheduler Error: " + error + "\n");
                    else
                        File.AppendAllText(logFilePath, $"Update scheduled successfully for {runTime}\n");
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(logFilePath, "Error scheduling task: " + ex.Message + "\n");
            }
        }




        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string exePath = @"C:\Program Files (x86)\ASN IT Inc\POSpointe\Updater\MyPOSUpdater.exe";
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = _version,
                    Verb = "runas",
                    UseShellExecute = true
                };

                Process.Start(startInfo);
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error launching updater: " + ex.Message, "Update Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
