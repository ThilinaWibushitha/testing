using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pospointe.LoginWindow
{
    /// <summary>
    /// Interaction logic for FrmUpdater.xaml
    /// </summary>
    public partial class FrmUpdater : UserControl
    {
        public FrmUpdater()
        {
            InitializeComponent();
            CheckForUpdates();
            LoadAutoUpdateSetting();
        }

        private void LoadAutoUpdateSetting()
        {
            bool ForceUpdateEnabled = Properties.Settings.Default.ForceUpdate;
            TglForce.IsChecked = ForceUpdateEnabled;
            bool autoUpdateEnabled = Properties.Settings.Default.AutoUpdateEnabled;
            TglAutoupdate.IsChecked = autoUpdateEnabled;
            
        }

        private void TglAutoupdate_Checked(object sender, RoutedEventArgs e)
        {
          
            Properties.Settings.Default.AutoUpdateEnabled = true;
            Properties.Settings.Default.Save();

            
        }

        private void TglAutoupdate_Unchecked(object sender, RoutedEventArgs e)
        {
           
            Properties.Settings.Default.AutoUpdateEnabled = false;
            Properties.Settings.Default.Save();

            
        }

        private async void CheckForUpdates()
        {
            try
            {
                var options = new RestClientOptions("https://asnitagentapi.azurewebsites.net") { MaxTimeout = -1 };
                var client = new RestClient(options);
                var request = new RestRequest("/Updater/getnewversion", Method.Get);
                RestResponse response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    var updateInfo = JsonConvert.DeserializeObject<ChkForUpdateResp>(response.Content);

                    if (decimal.TryParse(updateInfo.currentVersion, out decimal latestVersion) &&
                        decimal.TryParse(clsConnections.thisversion, out decimal currentVersion))
                    {
                        if (latestVersion > currentVersion)
                        {
                            CCUpdater.Content = new UpdateAvailableView(updateInfo.currentVersion);
                        }
                        else
                        {
                            CCUpdater.Content = new NoUpdateView();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error parsing version numbers.", "Update Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Failed to check for updates. Please try again later.", "Update Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while checking updates: " + ex.Message, "Update Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TglForce_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ForceUpdate = true;
            Properties.Settings.Default.Save();
        }

        private void TglForce_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ForceUpdate = false;
            Properties.Settings.Default.Save();
        }
    }
}

