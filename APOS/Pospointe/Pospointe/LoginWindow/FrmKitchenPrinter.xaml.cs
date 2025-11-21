using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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

namespace Pospointe.LoginWindow;

/// <summary>
/// Interaction logic for FrmKitchenPrinter.xaml
/// </summary>
public partial class FrmKitchenPrinter : UserControl
{
    public FrmKitchenPrinter()
    {
        InitializeComponent();

        
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        KeepKOT.IsChecked = Properties.Settings.Default.IsKeepKOTEnabled;
        LoadInstalledPrinters();
        
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
        
        CmbKitchenPrinter.Items.Clear();
        CmbKitchenPrinter1.Items.Clear();
        CmbHoldPrinter.Items.Clear();
      //  CmbMarketPrinter.Items.Clear();

        foreach (var printer in printers)
        {
            CmbSelectReceiptPrinter.Items.Add(printer);
            
            CmbKitchenPrinter.Items.Add(printer);
            CmbKitchenPrinter1.Items.Add(printer);
            CmbHoldPrinter.Items.Add(printer);
           // CmbMarketPrinter.Items.Add(printer);
        }
    }

    private void SetSavedPrinterValues()
    {
        SetComboBoxText(CmbSelectReceiptPrinter, Properties.Settings.Default.ReceiptPrinter);
        SetComboBoxText(CmbKitchenPrinter, Properties.Settings.Default.KitchenPrinter);
        SetComboBoxText(CmbKitchenPrinter1, Properties.Settings.Default.KitchenPrinter2);
        SetComboBoxText(CmbHoldPrinter, Properties.Settings.Default.HoldPrinter);
        //SetComboBoxText(CmbMarketPrinter, Properties.Settings.Default.MarketPrinter);
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

    private void BtnUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Save printer settings
            //Properties.Settings.Default.ReceiptPrinter = CmbSelectReceiptPrinter.Text;
            Properties.Settings.Default.KitchenPrinter = CmbKitchenPrinter.Text;
            Properties.Settings.Default.KitchenPrinter2 = CmbKitchenPrinter1.Text;
            //Properties.Settings.Default.HoldPrinter = CmbHoldPrinter.Text;
           // Properties.Settings.Default.MarketPrinter = CmbMarketPrinter.Text;

            Properties.Settings.Default.Save();

            ShowCustomMessage("Printer settings have been updated successfully.", isError: false);
        }
        catch (Exception ex)
        {
            ShowCustomMessage($"An error occurred while saving the printer settings. Please try again.\nDetails: {ex.Message}", isError: true);
        }
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



    private void KeepKOTToggle_Checked(object sender, RoutedEventArgs e)
    {
        Properties.Settings.Default.IsKeepKOTEnabled = true;
        Properties.Settings.Default.Save(); 
               
    }

    private void KeepKOTToggle_Unchecked(object sender, RoutedEventArgs e)
    {
        Properties.Settings.Default.IsKeepKOTEnabled = false;
        Properties.Settings.Default.Save();
    }



}
