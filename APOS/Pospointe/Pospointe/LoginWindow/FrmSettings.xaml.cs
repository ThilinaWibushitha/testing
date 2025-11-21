using Pospointe.LocalData;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Pospointe.LoginWindow
{


    /// <summary>
    /// Interaction logic for FrmSettings.xaml
    /// </summary>
    public partial class FrmSettings : Window
    {
        private const string DefaultLogoUrl = "https://i.postimg.cc/7YKbKFNG/POS-POINTE-1.png"; // Default logo
        public FrmSettings()
        {
            InitializeComponent();
            DispatcherTimer _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
            BsName.Text = LoggedData.BusinessInfo.BusinessName;
            BsAddress.Text = LoggedData.BusinessInfo.BusinessAddress;
            LoadBusinessLogo(LoggedData.BusinessInfo.LogoPath);


            var updater = new FrmUpdater();
            ContentDisplay.Content = updater;
            //var customizeSettings = new FrmCustomize();
            //ContentDisplay.Content = customizeSettings;
            //var printerSettingsControl = new FrmPrintersettings();
            //ContentDisplay.Content = printerSettingsControl;
        }

        private void LoadBusinessLogo(string logoPath)
        {
            string finalLogoPath = string.IsNullOrEmpty(logoPath) || !Uri.IsWellFormedUriString(logoPath, UriKind.Absolute) ? DefaultLogoUrl : logoPath;

            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(finalLogoPath, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                Dispatcher.Invoke(() => BusinessLogo.Source = bitmap);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading logo: " + ex.Message);
            }
        }

        private void BtnPrintersettings_Click(object sender, RoutedEventArgs e)
        {
            var printerSettingsControl = new FrmPrintersettings();
            ContentDisplay.Content = printerSettingsControl;

        }



        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is ListViewItem selectedItem)
            {
                if (selectedItem.Tag is string selectedTag)
                {
                    switch (selectedTag)
                    {
                        case "Software Update":
                            ContentDisplay.Content = new FrmUpdater();
                            break;

                        case "Payment Integration":
                            ContentDisplay.Content = new FrmPaymentprocess();
                            break;

                        case "Printer Settings":
                            ContentDisplay.Content = new FrmPrintersettings();
                            break;

                        case "Display":
                            ContentDisplay.Content = new FrmCustomize();
                            break;

                        case "General":
                            ContentDisplay.Content = new FrmDisplaysettings();
                            break;

                        case "Kitchen Printer":
                            ContentDisplay.Content = new FrmKitchenPrinter();
                            break;

                        case "Online Orders":
                            ContentDisplay.Content = new FrmOnlineOrdersSetting();
                            break;

                        case "Exit":
                            this.Close();
                            break;

                        default:
                            break;
                    }
                }

                listView.SelectedIndex = -1;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTimeTextBlock.Text = DateTime.Now.ToString("MMMM dd, yyyy h:mm:ss tt");
        }

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}
