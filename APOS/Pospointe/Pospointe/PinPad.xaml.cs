using System;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Pospointe
{
    public partial class PinPad : Window
    {
        public string returnvalue { get; set; }

        private bool isPlaceholderActive = true;

        public PinPad(string username, string userPicturePath)
        {
            InitializeComponent();

            UserNameDisplay.Text = username;

            try
            {
                if (!string.IsNullOrEmpty(userPicturePath))
                {
                    BitmapImage userImage = new BitmapImage();
                    userImage.BeginInit();
                    userImage.UriSource = new Uri(userPicturePath);
                    userImage.CacheOption = BitmapCacheOption.OnLoad;
                    userImage.EndInit();

                    //UserImage.Source = userImage;
                }
                else
                {
                    //UserImage.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception)
            {
                //UserImage.Visibility = Visibility.Collapsed;
            }

            // Set PinDisplay to empty initially and with placeholder style
            PinDisplay.Text = "";
            PinDisplay.Foreground = Brushes.Gray;
        }

        // This method handles the Enter key press
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(sender, e);
            }
        }

        private void PinDisplay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                string digit = e.Key >= Key.D0 && e.Key <= Key.D9
                    ? (e.Key - Key.D0).ToString()
                    : (e.Key - Key.NumPad0).ToString();

                _actualPin += digit;

                PinDisplay.Text = new string('*', _actualPin.Length);
                PinDisplay.CaretIndex = PinDisplay.Text.Length;
            }
            else if (e.Key == Key.Back && _actualPin.Length > 0)
            {
                _actualPin = _actualPin.Substring(0, _actualPin.Length - 1);
                PinDisplay.Text = new string('*', _actualPin.Length);
            }
            e.Handled = true;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PinDisplay.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            returnvalue = PinDisplay.Text;
            this.DialogResult = true;
            this.Close();
            returnvalue = _actualPin;
        }



        private string _actualPin = ""; // Stores the actual PIN value securely

        private void Btnin(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (isPlaceholderActive)
                {
                    PinDisplay.Text = "";
                    PinDisplay.Foreground = Brushes.Black;
                    isPlaceholderActive = false;
                }

                // Append the button's content to the actual PIN
                _actualPin += button.Content.ToString();

                // Display asterisks in the TextBox to mask the PIN
                PinDisplay.Text = new string('*', _actualPin.Length);
                PinDisplay.CaretIndex = PinDisplay.Text.Length;
            }
        }


        private void PinDisplay_GotFocus(object sender, RoutedEventArgs e)
        {
            if (isPlaceholderActive)
            {
                PinDisplay.Text = "";
                PinDisplay.Foreground = Brushes.Black;
                isPlaceholderActive = false;
            }
        }

        private void PinDisplay_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isPlaceholderActive && string.IsNullOrWhiteSpace(PinDisplay.Text))
            {
                PinDisplay.Text = "";
                PinDisplay.Foreground = Brushes.Gray;
                isPlaceholderActive = true;
            }
        }

        private void PinDisplay_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PinDisplay.Text))
            {
                PinDisplay.Foreground = Brushes.Gray;
                isPlaceholderActive = true;
            }
        }

        private void Btndelete_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_actualPin))
            {
                // Remove the last character from the actual PIN
                _actualPin = _actualPin.Substring(0, _actualPin.Length - 1);

                // Update the TextBox to display the appropriate number of asterisks
                PinDisplay.Text = new string('*', _actualPin.Length);
            }
            else
            {
                // Close the dialog if there is nothing to delete
                this.DialogResult = false;
                this.Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
