using Newtonsoft.Json;
using Pospointe.Models;
using Pospointe.LoginWindow;
using RestSharp;
using System.Linq.Expressions;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Pospointe.PosMenu;
using Pospointe.MainMenu;
using Pospointe.LocalData;
using System.IO;
using Pospointe.Signup_POS;
using Pospointe.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Pospointe
{
    public partial class MainWindow : Window
    {
        public class FileChecker
        {
            public static bool CheckIfLicenseFileExists()
            {
                var programDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                var posPointeFolder = System.IO.Path.Combine(programDataPath, "POSpointe");
                var licenseFilePath = System.IO.Path.Combine(posPointeFolder, "pospointe.lic");
                return File.Exists(licenseFilePath);
            }
        }

        private Register loadedLicense;

        public MainWindow()
        {
            InitializeComponent();

            if (FileChecker.CheckIfLicenseFileExists())
            {
                var programDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                var posPointeFolder = System.IO.Path.Combine(programDataPath, "POSpointe");
                var licenseFilePath = System.IO.Path.Combine(posPointeFolder, "pospointe.lic");

                var encodedJson = File.ReadAllText(licenseFilePath);
                var json = CLSencryption.Decrypt(encodedJson);
                loadedLicense = JsonConvert.DeserializeObject<Register>(json);

                clsConnections.mydb = loadedLicense.dbName;
            }
            else
            {
                FrmSignup sign = new FrmSignup();
                sign.Show();
                this.Close();
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtversion.Text = clsConnections.thisversion;
            LoadingProgressBar.Visibility = Visibility.Visible;
            StatusText.Text = "Initializing...";
            await ForceRender();

            try
            {
                StatusText.Text = "Syncing data schema from cloud...";
                await ForceRender();

                bool online = SyncService.IsOnline();

                if (online)
                {
                    // Optionally include schema sync
                    // await syncschemafromcloud();

                    StatusText.Text = "Updating License...";
                    await ForceRender();

                    if (loadedLicense != null)
                    {
                        await FrmSignup.UpdateLicense(loadedLicense.dbName, loadedLicense);
                    }
                }

                StatusText.Text = "Applying theme...";
                await ForceRender();

                if (Properties.Settings.Default.DarkMode)
                {
                    ApplyTheme("Theme/DarkTheme.xaml");
                }
                else
                {
                    ApplyTheme("Theme/LightTheme.xaml");
                }

                StatusText.Text = "Fetching data From Cloud...";
                await ForceRender();

                var syncService = new CloudDataSyncService();
                bool result = await syncService.SyncCloudData(clsConnections.mydb, msg =>
                {
                    StatusText.Text = msg;
                });

                if (!result)
                {
                    StatusText.Text = "Starting in Offline Mode...";
                    await Task.Delay(3000);
                }
                else
                {
                    StatusText.Text = "Initialization complete!";
                }

                await ForceRender();
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Error: {ex.Message}";
                MessageBox.Show($"Error: {ex.Message}\n{ex.StackTrace}");
            }
            finally
            {
                LoadingProgressBar.Visibility = Visibility.Collapsed;

                using (var context = new PosDb1Context())
                {
                    var business = context.TblBusinessInfos.FirstOrDefault(x => x.StoreId == "1001");
                    if (business != null)
                        LoggedData.BusinessInfo = business;
                }

                this.Hide();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    FrmLogin frmLogin = new FrmLogin();
                    frmLogin.ShowDialog();
                });
            }
        }

        private async Task ForceRender()
        {
            await Dispatcher.InvokeAsync(() => { }, System.Windows.Threading.DispatcherPriority.Render);
        }

        private void ApplyTheme(string themePath)
        {
            var appResources = Application.Current.Resources.MergedDictionaries;
            var existingTheme = appResources.FirstOrDefault(d =>
                d.Source != null && d.Source.OriginalString.Contains("Themes"));
            if (existingTheme != null)
            {
                appResources.Remove(existingTheme);
            }

            try
            {
                var newTheme = new ResourceDictionary
                {
                    Source = new Uri(themePath, UriKind.Relative)
                };
                appResources.Add(newTheme);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load theme: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public class MainResponse
        {
            public List<TblUser> users { get; set; }
            public List<TblItem> items { get; set; }
            public List<TblDepartment> departments { get; set; }
            public List<TblModiferGroup> modiferGroups { get; set; }
            public List<TblModifersofItem> modifersofItems { get; set; }
            public List<TblBusinessInfo> businessInfo { get; set; }
        }
    }
}
