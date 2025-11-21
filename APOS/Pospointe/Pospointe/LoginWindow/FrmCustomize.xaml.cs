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
    /// Interaction logic for FrmCustomize.xaml
    /// </summary>
    public partial class FrmCustomize : UserControl
    {
        public FrmCustomize()
        {
            InitializeComponent();
            LoadCurrentTheme();
        }

        private void LoadCurrentTheme()
        {
            bool isDarkMode = Properties.Settings.Default.DarkMode;

            if (isDarkMode)
            {
                RadioDarkMode.IsChecked = true;
            }
            else
            {
                RadioLightMode.IsChecked = true;
            }
        }

        private void UpdateThemeButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateTheme();

            MessageBoxResult result = MessageBox.Show(
                "Theme updated successfully. Do you want to restart the application to apply the changes?",
                "Theme Updated",
                MessageBoxButton.YesNo,
                MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                RestartApplication();
            }
        }

        private void UpdateTheme()
        {
            if (RadioLightMode.IsChecked == true)
            {
                ApplyTheme("Theme/LightTheme.xaml");
                Properties.Settings.Default.DarkMode = false;
            }
            else if (RadioDarkMode.IsChecked == true)
            {
                ApplyTheme("Theme/DarkTheme.xaml");
                Properties.Settings.Default.DarkMode = true;
            }

            Properties.Settings.Default.Save();
        }

        private void RestartApplication()
        {
            try
            {
                string appPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = appPath,
                    UseShellExecute = true,
                });

                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while restarting the application: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
    }
}
