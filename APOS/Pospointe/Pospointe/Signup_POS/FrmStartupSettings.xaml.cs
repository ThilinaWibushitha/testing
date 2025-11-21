using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Pospointe.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Printing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfScreenHelper;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace Pospointe.Signup_POS
{
    public partial class FrmStartupSettings : Window
    {
        public FrmStartupSettings()
        {
            InitializeComponent();
            StartSetupSequence();
        }

        private async void StartSetupSequence()
        {
            ShowWelcomeMessage();
            await Task.Delay(3000); // Wait for 3 seconds

            await InitializeDatabase(); // Database setup
            await CheckForReceiptPrinter(); // Receipt printer setup
            await CheckForCustomerDisplay(); // Customer display setup
            await SetAdditionalSettings(); // Additional settings
            await CheckForUpdates(); // Check for updates

            ShowFinalStep(); // Final step
        }



        private void ShowWelcomeMessage()
        {
            StatusTextBlock.Text = "Thank you for choosing POSPointe! We're getting everything ready for you...";
        }


        private async Task InitializeDatabase()
        {
            
            StatusTextBlock.Text = "Setting up the database...";
            string serverName = "LOCALHOST\\POSPOINTE"; // e.g., localhost or your server
            string databaseName = "PosDB"; // The database to check
            string connectionString = $"Server={serverName};Integrated Security=True;TrustServerCertificate=True;"; // Or include Username/Password for SQL Authentication
            
            bool dbExists = DoesDatabaseExist(connectionString, databaseName);

            if (dbExists)
            {
                StatusTextBlock.Text = "Database 'PosDB' found. Skipping database creation.";
            }
            else
            {
                StatusTextBlock.Text = "Database 'PosDB' not found. Creating the database...";
                await CreateDatabase();
            }

        }

        private async Task CreateDatabase()
        {
            string connectionString = "Server=LOCALHOST\\POSPOINTE;Database=master;Integrated Security=True;TrustServerCertificate=True;";
            string fileUrl = "https://mypospointe.com/downloads/localscript.txt";

            try
            {
                // Download the SQL script from the URL
                string updateScript = await DownloadSqlFileAsync(fileUrl);

                // Split the script by "GO" keyword
                string[] scriptCommands = updateScript.Split(new string[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        foreach (string commandText in scriptCommands)
                        {
                            if (string.IsNullOrWhiteSpace(commandText)) continue;

                            try
                            {
                                command.CommandText = commandText.Trim();
                                await command.ExecuteNonQueryAsync();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error executing script: {ex.Message}");
                                break;
                            }
                        }
                    }
                }

                StatusTextBlock.Text = "Database setup completed.";
            }
            catch (Exception ex)
            {
               // MessageBox.Show($"Failed to create the database: {ex.Message}");
                StatusTextBlock.Text = "Database setup failed.";
            }
        }
        public static async Task<string> DownloadSqlFileAsync(string fileUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                // Ensure the response is successful
                HttpResponseMessage response = await client.GetAsync(fileUrl);
                response.EnsureSuccessStatusCode();

                // Read the content as a string
                return await response.Content.ReadAsStringAsync();
            }
        }

        private static bool DoesDatabaseExist(string connectionString, string databaseName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT database_id FROM sys.databases WHERE name = @DatabaseName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DatabaseName", databaseName);

                        object result = command.ExecuteScalar();
                        return result != null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking database existence: {ex.Message}");
                return false;
            }
        }

        private async Task CheckForReceiptPrinter()
        {
            StatusTextBlock.Text = "Scanning for receipt printers...";
            await Task.Delay(3000); // Simulate printer scanning delay

            bool printerFound = false;
            string printerName = string.Empty;

            await Task.Run(() =>
            {
                var printQueues = new PrintServer().GetPrintQueues().ToList();
                var epsonPrinter = printQueues.FirstOrDefault(p => p.Name.ToLower().Contains("epson"));
                if (epsonPrinter != null)
                {
                    printerFound = true;
                    printerName = epsonPrinter.Name;
                }
            });

            if (printerFound)
            {
                StatusTextBlock.Text = $"Epson printer found: {printerName}. Setting as default...";
                Properties.Settings.Default.ReceiptPrinter = printerName;
                Properties.Settings.Default.Save();
                await Task.Delay(3000);
            }
            else
            {
                StatusTextBlock.Text = "No receipt printer found.";
            }
        }



        private async Task CheckForCustomerDisplay()
        {
            StatusTextBlock.Text = "Checking for a second display...";
            await Task.Delay(3000); // Simulate display scanning delay

            var screens = Screen.AllScreens.ToList();
            if (screens.Count > 1)
            {
                Properties.Settings.Default.CustomerDisplay = true;
                //Properties.Settings.Default.CDisplayID = screens.Count > 2 ? 2 : 1;
                StatusTextBlock.Text = "Second display detected.";
            }
            else
            {
                StatusTextBlock.Text = "No second display found.";
            }
            Properties.Settings.Default.Save();
        }




        private async Task SetAdditionalSettings()
        {
            StatusTextBlock.Text = "Configuring additional settings...";
            await Task.Delay(3000); // Simulate settings configuration delay

            Properties.Settings.Default.ReceiptOption = "Prompt";
            Properties.Settings.Default.DefaultCashDrawer = 200.00;
            Properties.Settings.Default.ShowShiftCloseReport = true;
            Properties.Settings.Default.Save();
        }

        private async Task CheckForUpdates()
        {
            StatusTextBlock.Text = "Checking for updates...";
            await Task.Delay(3000); // Simulate update check delay

            bool updateFound = UpdateService.CheckForUpdate();
            if (updateFound)
            {
                StatusTextBlock.Text = "Update found. Installing...";
                await UpdateService.Update("Latest");
            }
            else
            {
                StatusTextBlock.Text = "Your system is up to date.";
            }
        }


        private void ShowFinalStep()
        {
            StatusTextBlock.Text = "Setup completed successfully!";
            BtnFinishSetup.Visibility = Visibility.Visible;
        }

        private void BtnFinishSetup_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
