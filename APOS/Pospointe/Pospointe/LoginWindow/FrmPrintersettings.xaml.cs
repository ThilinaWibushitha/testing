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
using System.Printing;
using System.Drawing.Printing;
using Pospointe.Properties;


namespace Pospointe.LoginWindow;

/// <summary>
/// Interaction logic for FrmPrintersettings.xaml
/// </summary>
public partial class FrmPrintersettings : UserControl
{
    public FrmPrintersettings()
    {
        InitializeComponent();
        TglCashDrawer.IsChecked = AppSettingsManager.CashDrawerEnabled;
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        LoadInstalledPrinters();
        LoadHoldPrinterState();  
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
                    PopulatePrinterCombos(printers);
                    SetSavedPrinterValues();
                });
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading printers: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void PopulatePrinterCombos(List<string> printers)
    {
        CmbSelectReceiptPrinter.Items.Clear();
        CmbHoldPrinter.Items.Clear();


        foreach (var printer in printers)
        {
            CmbSelectReceiptPrinter.Items.Add(printer);
            CmbHoldPrinter.Items.Add(printer);
        }
    }

    private void SetSavedPrinterValues()
    {
        SetComboBoxText(CmbSelectReceiptPrinter, Properties.Settings.Default.ReceiptPrinter);
        SetComboBoxText(CmbHoldPrinter, Properties.Settings.Default.HoldPrinter);
        foreach (ComboBoxItem item in CmbSelectReceiptnmethod.Items)
        {
            if (item.Content.ToString() == Properties.Settings.Default.ReceiptOption)
            {
                CmbSelectReceiptnmethod.SelectedItem = item;
                break;
            }
        }
    }


    private void SetComboBoxText(ComboBox comboBox, string value)
    {
        if (!string.IsNullOrEmpty(value) && comboBox.Items.Contains(value))
        {
            comboBox.Text = value;
        }
        else
        {
            comboBox.Text = string.Empty;
        }
    }

    private void LoadHoldPrinterState()
    {
        // Load the saved state of the Hold Printer toggle
        ChkturnHold.IsChecked = Properties.Settings.Default.TurnonHold == 1;
    }

    private void BtnUpdate_Click(object sender, RoutedEventArgs e)
    {

        if (CmbSelectReceiptnmethod.SelectedItem is ComboBoxItem selectedReceiptOption)
        {
            Properties.Settings.Default.ReceiptOption = selectedReceiptOption.Content.ToString();
        }

        try
        {
            // Save printer settings
            Properties.Settings.Default.ReceiptPrinter = CmbSelectReceiptPrinter.Text;
            //Properties.Settings.Default.KitchenPrinter = CmbKitchenPrinter.Text;
            //Properties.Settings.Default.KitchenPrinter2 = CmbKitchenPrinter1.Text;
            Properties.Settings.Default.HoldPrinter = CmbHoldPrinter.Text;
           // Properties.Settings.Default.MarketPrinter = CmbMarketPrinter.Text;

            Properties.Settings.Default.Save();

            ShowCustomMessage("Printer settings have been updated successfully.", isError: false);
        }
        catch (Exception ex)
        {
            ShowCustomMessage($"An error occurred while saving the printer settings. Please try again.\nDetails: {ex.Message}", isError: true);
        }
    }

    private void ChkturnHold_Checked(object sender, RoutedEventArgs e)
    {
        Properties.Settings.Default.TurnonHold = 1;
        Properties.Settings.Default.Save();
    }

    private void ChkturnHold_Unchecked(object sender, RoutedEventArgs e)
    {
        Properties.Settings.Default.TurnonHold = 0;
        Properties.Settings.Default.Save();
    }

    private void ShowCustomMessage(string message, bool isError)
    {
        FrmCustommessage customMessageBox = new FrmCustommessage
        {
            LblHeader = { Text = "Printer Settings Update" },
            LblMessage = { Text = message },
            IsError = isError
        };
        customMessageBox.ShowDialog();
    }

    private void CmbHoldPrinter_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void TglCashDrawer_Checked(object sender, RoutedEventArgs e)
    {
        AppSettingsManager.CashDrawerEnabled = true;
    }

    private void TglCashDrawer_Unchecked(object sender, RoutedEventArgs e)
    {
        AppSettingsManager.CashDrawerEnabled = false;
    }
}

