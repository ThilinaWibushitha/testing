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
using System.Windows.Threading;

namespace Pospointe.Signup_POS
{
    public partial class NetworkCheckPage : Page
    {
        private DispatcherTimer networkCheckTimer;

        public NetworkCheckPage()
        {
            InitializeComponent();
            StartNetworkMonitoring();
        }

        private void StartNetworkMonitoring()
        {
            NetworkStatusText.Text = "Verifying network connectivity... Please wait.";

            networkCheckTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(10)
            };
            networkCheckTimer.Tick += CheckNetworkStatus;
            networkCheckTimer.Start();
        }

        private void CheckNetworkStatus(object sender, EventArgs e)
        {
            if (IsInternetAvailable())
            {
                networkCheckTimer.Stop();
                NetworkStatusText.Text = "🎉 Network connection established successfully! Preparing your setup...";
                DelayAndNavigateToSignupPage();
            }
            else
            {
                networkCheckTimer.Stop();
                NetworkStatusText.Text = "⚠️ Unable to establish an internet connection. Please ensure your network is properly configured and try restarting the setup.";
            }
        }

        private bool IsInternetAvailable()
        {
            try
            {
                return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            }
            catch
            {
                return false;
            }
        }

        private async void DelayAndNavigateToSignupPage()
        {
            await Task.Delay(5000);
            var signupPage = new SignupPage();
            this.NavigationService.Navigate(signupPage);
        }
    }
}
