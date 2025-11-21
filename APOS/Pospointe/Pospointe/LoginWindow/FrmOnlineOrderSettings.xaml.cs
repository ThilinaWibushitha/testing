using System.Drawing.Printing;
using System.Windows;
using System.Windows.Controls;


namespace Pospointe.LoginWindow
{
    /// <summary>
    /// Interaction logic for FrmOnlineOrdersSetting.xaml
    /// </summary>
    public partial class FrmOnlineOrdersSetting : UserControl
    {
        private bool _isLoading = false;

        public FrmOnlineOrdersSetting()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _isLoading = true;
            
            // Load toggle state
            TglOnlineOrders.IsChecked = Properties.Settings.Default.MarketOrders;
            UpdateStatusText();
            
            _isLoading = false;

            // Load printers
            await LoadInstalledPrintersAsync();

            // Set saved printer
            SetSavedPrinterValue();
        }

        private async Task LoadInstalledPrintersAsync()
        {
            try
            {
                var printers = await Task.Run(() => PrinterSettings.InstalledPrinters.Cast<string>().ToList());
                CmbMarketPrinter.Items.Clear();
                foreach (var printer in printers)
                {
                    CmbMarketPrinter.Items.Add(printer);
                }
            }
            catch (Exception ex)
            {
                ShowCustomMessage("Error", $"Error loading printers: {ex.Message}", true);
            }
        }


        private void LoadInstalledPrinters()
        {
            try
            {
                Task.Run(() =>
                {
                    var printers = PrinterSettings.InstalledPrinters.Cast<string>().ToList();
                    Dispatcher.Invoke(() =>
                    {
                        CmbMarketPrinter.Items.Clear();
                        foreach (var printer in printers)
                            CmbMarketPrinter.Items.Add(printer);
                    });
                });
            }
            catch (Exception ex)
            {
                ShowCustomMessage("Error", $"Error loading printers: {ex.Message}", true);
            }
        }

        private void SetSavedPrinterValue()
        {
            string savedPrinter = Properties.Settings.Default.MarketPrinter;
            if (!string.IsNullOrEmpty(savedPrinter))
            {
                CmbMarketPrinter.Text = savedPrinter;
            }
        }


        private void BtnUpdatePrinter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string selectedPrinter = CmbMarketPrinter.Text;
                if (string.IsNullOrEmpty(selectedPrinter))
                {
                    ShowCustomMessage("Error", "Please select a printer before saving.", true);
                    return;
                }

                Properties.Settings.Default.MarketPrinter = selectedPrinter;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();

                ShowCustomMessage("Success", $"Marketplace printer updated to:\n{selectedPrinter}", false);
            }
            catch (Exception ex)
            {
                ShowCustomMessage("Error", $"Failed to save printer setting: {ex.Message}", true);
            }
        }


        private void TglOnlineOrders_Checked(object sender, RoutedEventArgs e)
        {
            if (_isLoading) return;
            
            try
            {
                Properties.Settings.Default.MarketOrders = true;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload(); // Reload to ensure it's saved
                UpdateStatusText();
                ShowCustomMessage("Success", "Online Orders have been ENABLED successfully.", false);
            }
            catch (Exception ex)
            {
                ShowCustomMessage("Error", $"Error enabling online orders: {ex.Message}", true);
            }
        }

        private void TglOnlineOrders_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_isLoading) return;
            
            try
            {
                Properties.Settings.Default.MarketOrders = false;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload(); // Reload to ensure it's saved
                UpdateStatusText();
                ShowCustomMessage("Information", "Online Orders have been DISABLED successfully.", false);
            }
            catch (Exception ex)
            {
                ShowCustomMessage("Error", $"Error disabling online orders: {ex.Message}", true);
            }
        }

        private void UpdateStatusText()
        {
            TxtStatus.Text = TglOnlineOrders.IsChecked == true
                ? "Status: Online Orders are ENABLED"
                : "Status: Online Orders are DISABLED";

            // Debug info
            TxtDebug.Text = $"Settings Value: {Properties.Settings.Default.MarketOrders}";
        }

        private void ShowCustomMessage(string header, string message, bool isError)
        {
            FrmCustommessage frmCustommessage = new FrmCustommessage
            {
                LblHeader = { Text = header },
                LblMessage = { Text = message },
                IsError = isError
            };
            frmCustommessage.ShowDialog();
        }
    }
}
