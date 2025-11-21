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
using System.Windows.Shapes;
using Pospointe.Models;
using Pospointe.LocalData;

namespace Pospointe.Reports
{
    /// <summary>
    /// Interaction logic for FrmReportDateSelect.xaml
    /// </summary>
    public partial class FrmReportDateSelect : Window
    {
        public FrmReportDateSelect()
        {
            InitializeComponent();
            FromDatePicker.SelectedDate = DateTime.Today;
    ToDatePicker.SelectedDate = DateTime.Today;
        }

        private async void BtnGetReport_Click(object sender, RoutedEventArgs e)
        {
            DateTime? fromDate = FromDatePicker.SelectedDate; 
            DateTime? toDate = ToDatePicker.SelectedDate;

            if (fromDate == null || toDate == null)
            {
                MessageBox.Show("Please select both From Date and To Date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string fromDateString = fromDate.Value.ToString("yyyy-MM-dd");
            string toDateString = toDate.Value.ToString("yyyy-MM-dd");

            try
            {
                var options = new RestClientOptions(clsConnections.myposapiurl)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest($"/Reports/flashreport/{fromDateString}/{toDateString}", Method.Get);

                request.AddHeader("db",clsConnections.mydb);
                request.AddHeader("Authorization", clsConnections.transserverauth);

                RestResponse response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    var reportData = JsonConvert.DeserializeObject<ReportData>(response.Content);
                    FrmFlashReport reportWindow = new FrmFlashReport(reportData, fromDateString, toDateString);
                    reportWindow.ShowDialog();
                }
                else
                {
                    MessageBox.Show($"Failed to fetch report: {response.StatusDescription}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
