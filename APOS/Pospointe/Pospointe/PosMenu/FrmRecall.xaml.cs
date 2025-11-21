using Newtonsoft.Json;
using Pospointe.Helpers;
using Pospointe.LocalData;
using Pospointe.Models;
using RestSharp;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Pospointe.Trans_Api.TransData;

namespace Pospointe.PosMenu;

/// <summary>
/// Interaction logic for FrmRecall.xaml
/// </summary>
public partial class FrmRecall : Window
{
    public string recalledinvoiceID { get; set; }

    public string transtype { get; set; }

    private MainWindowViewModel _viewModel;
    public FrmRecall()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        LoadInvoicesAsync();
        _viewModel = new MainWindowViewModel();
        DataContext = _viewModel;


    }

    private async Task LoadInvoicesAsync()
    {
        if (transtype == "ALL")
        {
            txtHeading.Text = "Recall Invoices";
            try
            {
                txtStatus.Visibility = Visibility.Visible;
                txtStatus.Text = "Fetching invoices...";

                var options = new RestClientOptions(LoggedData.transbaseurl)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/OtherTransactions/todaytrans", Method.Get);
                request.AddHeader("db", clsConnections.mydb);
                request.AddHeader("Authorization", clsConnections.transserverauth);

                RestResponse response = await client.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    var invoices = JsonConvert.DeserializeObject<List<TransMainDto>>(response.Content);


                    dgInvoices.ItemsSource = invoices;




                    txtStatus.Text = "Invoices loaded successfully!";
                }
                else
                {
                    MessageBox.Show("Failed to fetch invoices. Please check your network.", "Error", MessageBoxButton.OK);
                    MessageBox.Show(response.Content + " " + response.StatusCode, "Error", MessageBoxButton.OK);
                    txtStatus.Text = "Failed to load invoices.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK);
                txtStatus.Text = "Error loading invoices.";
            }
            finally
            {
                txtStatus.Visibility = Visibility.Collapsed;
            }
        }

        else if (transtype == "HOLD")
        {
            txtHeading.Text = "Recall Hold Invoices";
            try
            {
                using (var context = new PosDb1Context())
                {
                    var invces = context.TblTransMains.Where(x => x.TransType == "HOLD")
                        .OrderByDescending(x => x.SaleDateTime)
                        .ToList();
                    dgInvoices.ItemsSource = invces;
                    txtStatus.Text = "Invoices loaded successfully!";
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK);
                txtStatus.Text = "Error loading invoices.";
            }
            finally
            {
                txtStatus.Visibility = Visibility.Collapsed;
            }
        }

    }

    private async void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        string invoiceID = txtSearch.Text;

        if (!string.IsNullOrWhiteSpace(invoiceID))
        {
            await SearchInvoicesByIDAsync(invoiceID);
            txtSearch.Text = string.Empty;
        }
        else
        {
            // If no search term is provided, load all transactions
            await LoadInvoicesAsync();
        }
    }


    public void DgInvoices_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        // _viewModel.ShowDialog = !_viewModel.ShowDialog;

        if (dgInvoices.SelectedItem is not null)
        {
            var slectedrow = dgInvoices.SelectedItem;
            if (transtype == "HOLD")
            {
                recalledinvoiceID = slectedrow.GetType().GetProperty("InvoiceId").GetValue(slectedrow, null).ToString();
            }
            else
            {
                recalledinvoiceID = slectedrow.GetType().GetProperty("InvoiceID").GetValue(slectedrow, null).ToString();
            }
            this.DialogResult = true;
            this.Close();
        }

    }






    private async Task SearchInvoicesByIDAsync(string invoiceID)
    {
        try
        {
            if (transtype == "HOLD")
            {
                // Search from local database for HOLD transactions
                using (var context = new PosDb1Context())
                {
                    var results = context.TblTransMains
                        .Where(x => x.TransType == "HOLD" &&
                            (x.InvoiceId.ToString().Contains(invoiceID) ||
                             x.InvoiceIdshortCode.Contains(invoiceID)))
                        .ToList();

                    if (results.Any())
                    {
                        dgInvoices.ItemsSource = results;
                        txtStatus.Text = "Hold invoices loaded successfully!";
                    }
                    else
                    {
                        MessageBox.Show("No hold invoices found.", "No Results", MessageBoxButton.OK);
                        txtStatus.Text = "No hold invoices found.";
                    }
                }
            }
            else
            {
                // Search from API for ALL or other types
                var options = new RestClientOptions(LoggedData.transbaseurl)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                RestRequest request;

                if (!string.IsNullOrWhiteSpace(invoiceID))
                {
                    // Try searching by either ID or short code
                    request = new RestRequest($"/OtherTransactions/findtrans/{invoiceID}", Method.Get);
                }
                else
                {
                    request = new RestRequest("/OtherTransactions/todaytrans", Method.Get);
                }

                request.AddHeader("db", clsConnections.mydb);
                request.AddHeader("Authorization", clsConnections.transserverauth);

                RestResponse response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    var invoices = JsonConvert.DeserializeObject<List<TransMainDto>>(response.Content);

                    // Try filtering by InvoiceIdshortCode if provided
                    if (invoices != null && invoices.Any())
                    {
                        var filteredInvoices = invoices
                            .Where(x =>
                                x.InvoiceID.ToString().Contains(invoiceID, StringComparison.OrdinalIgnoreCase) ||
                                (x.InvoiceIdshortCode?.Contains(invoiceID, StringComparison.OrdinalIgnoreCase) ?? false))
                            .ToList();

                        if (filteredInvoices.Any())
                        {
                            dgInvoices.ItemsSource = filteredInvoices;
                            txtStatus.Text = "Transactions loaded successfully!";
                        }
                        else
                        {
                            MessageBox.Show("No matching transactions found.", "No Results", MessageBoxButton.OK);
                            txtStatus.Text = "No matching transactions found.";
                        }
                    }
                    else
                    {
                        MessageBox.Show("No transactions found.", "No Results", MessageBoxButton.OK);
                        txtStatus.Text = "No transactions found.";
                    }
                }
                else
                {
                    MessageBox.Show("Failed to fetch transactions. Please check your network.", "Error", MessageBoxButton.OK);
                    txtStatus.Text = "Failed to load transactions.";
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK);
            txtStatus.Text = "Error loading transactions.";
        }
    }

    private void DgInvoices_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        if (e.PropertyName == "TipAmount")
        {
            e.Cancel = true;
        }
    }

    private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
    {
        //TouchKeyboardHelper.OpenKeyboard();
        var keyb = new FrmKeyboard();

        var result = keyb.ShowDialog();

        if (result == true)
        {
            if (result == true && !string.IsNullOrWhiteSpace(keyb.returnvalue))
            {
                txtSearch.Text = keyb.returnvalue;
                txtSearch.CaretIndex = txtSearch.Text.Length;
            }

        }
    }


    private void Btnclose_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }


}