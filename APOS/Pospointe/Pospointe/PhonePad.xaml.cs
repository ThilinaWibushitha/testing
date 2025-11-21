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

namespace Pospointe
{
    /// <summary>
    /// Interaction logic for PhonePad.xaml
    /// </summary>
    public partial class PhonePad : Window
    {
        public string request { get; set; } 
        public string ReturnValue1 { get; private set; } 
        public PhonePad()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(EmterPH.Text))
            {
                EmterPH.Text = "Enter Phone Number";
            }
        }

        private void BtnBackspace_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PhoneNumberDisplay.Text))
            {
                PhoneNumberDisplay.Text = PhoneNumberDisplay.Text.Substring(0, PhoneNumberDisplay.Text.Length - 1);
            }
        }


        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            SubmitPhoneNumber();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnNumber_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && PhoneNumberDisplay.Text.Length < 10)
            {
                PhoneNumberDisplay.Text += btn.Content.ToString();
            }

            if (PhoneNumberDisplay.Text.Length == 10)
            {
                SubmitPhoneNumber();
            }
        }

        private void SubmitPhoneNumber()
        {
            if (PhoneNumberDisplay.Text.Length == 10)
            {
                ReturnValue1 = PhoneNumberDisplay.Text;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please enter a 10-digit phone number.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


    }
}
