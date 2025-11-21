using Newtonsoft.Json;
using RestSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Pospointe
{
    public static class UpdateManager
    {
        private static readonly string _updateUrl = "https://asnitagentapi.azurewebsites.net/Updater/getnewversion";
        private static readonly string _exePath = @"C:\Program Files (x86)\ASN IT Inc\POSpointe\Updater\MyPOSUpdater.exe";
        private static DispatcherTimer _reminderTimer;

        public static async void CheckAndUpdate()
        {
            try
            {
                bool autoUpdateEnabled = Properties.Settings.Default.AutoUpdateEnabled;
                bool forceUpdate = Properties.Settings.Default.ForceUpdate;

                var options = new RestClientOptions("https://asnitagentapi.azurewebsites.net")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/Updater/getnewversion", Method.Get);
                RestResponse response = await client.ExecuteAsync(request);

                if (response.IsSuccessful && response.Content != null)
                {
                    var updateInfo = JsonConvert.DeserializeObject<ChkForUpdateResp>(response.Content);
                    if (decimal.TryParse(updateInfo.currentVersion, out decimal latestVersion) &&
                        decimal.TryParse(clsConnections.thisversion, out decimal currentVersion))
                    {
                        if (latestVersion > currentVersion)
                        {
                            if (forceUpdate)
                            {
                                // Force update without any prompt
                                StartUpdate(updateInfo.currentVersion);
                            }
                            else if (autoUpdateEnabled)
                            {
                                // Auto-update with a notification
                                MessageBox.Show($"A new version {updateInfo.currentVersion} is available.\nUpdating automatically...",
                                    "Auto Update", MessageBoxButton.OK, MessageBoxImage.Information);
                                StartUpdate(updateInfo.currentVersion);
                            }
                            else
                            {
                                // Prompt user for update
                                MessageBoxResult result = MessageBox.Show(
                                    $"A new version {updateInfo.currentVersion} is available.\nWould you like to update now?",
                                    "Update Available",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Question);

                                if (result == MessageBoxResult.Yes)
                                {
                                    StartUpdate(updateInfo.currentVersion);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Auto Update Error: " + ex.Message);
            }
        }

        private static void StartUpdate(string version)
        {
            try
            {
                if (!System.IO.File.Exists(_exePath))
                {
                    Debug.WriteLine("Updater file not found!");
                    return;
                }

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = _exePath,
                    Arguments = version,
                    UseShellExecute = true
                };

                Process.Start(startInfo);
                Application.Current.Shutdown(); // Close app before update
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error launching updater: " + ex.Message);
            }
        }
    }
}

