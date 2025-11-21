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
using Pospointe.PosMenu;

namespace Pospointe
{
    /// <summary>
    /// Interaction logic for FrmNumberpad.xaml
    /// </summary>
    public partial class FrmNumberpad : Window
    {
        public decimal EnteredValue { get; private set; } = 0;
        public bool IsPercentage { get; private set; } = true;

        public FrmNumberpad()
        {
            InitializeComponent();
            NumAmount.Text = "0";
            UserNameDisplay.Text = "Enter Discount in Percentage";

        }

        private void BtnDollar_Click(object sender, RoutedEventArgs e)
        {
            IsPercentage = false;
            NumAmount.Text = "0";
            UserNameDisplay.Text = "Enter Discount in Dollars";
            BtnDollar.Background = (Brush)FindResource("ButtonActiveBackground");
            BtnPercentage.Background = (Brush)FindResource("ButtonDefaultBackground");
        }

        private void BtnPercentage_Click(object sender, RoutedEventArgs e)
        {
            IsPercentage = true;
            NumAmount.Text = "0";
            UserNameDisplay.Text = "Enter Discount in Percentage";
            BtnPercentage.Background = (Brush)FindResource("ButtonActiveBackground");
            BtnDollar.Background = (Brush)FindResource("ButtonDefaultBackground");
        }

        private void Btnin(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                string content = button.Content.ToString();

                // Handle decimal point
                if (content == ".")
                {
                    if (!NumAmount.Text.Contains("."))
                    {
                        if (NumAmount.Text == "0")
                        {
                            NumAmount.Text = "0.";
                        }
                        else
                        {
                            NumAmount.Text += ".";
                        }
                    }
                }
                else
                {
                    // Remove leading zero when first number is entered
                    if (NumAmount.Text == "0")
                    {
                        NumAmount.Text = "";
                    }

                    NumAmount.Text += content;
                }

                EnableEnterButton();
            }
        }

        private void BtnDecimal_Click(object sender, RoutedEventArgs e)
        {
            // Only add decimal if there isn't one already
            if (!NumAmount.Text.Contains("."))
            {
                // If the text is just "0", keep it and add decimal
                if (NumAmount.Text == "0")
                {
                    NumAmount.Text = "0.";
                }
                else
                {
                    NumAmount.Text += ".";
                }
                EnableEnterButton();
            }
        }


        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (NumAmount.Text.Length > 0 && NumAmount.Text != "0")
            {
                NumAmount.Text = NumAmount.Text.Remove(NumAmount.Text.Length - 1);
                if (NumAmount.Text.Length == 0 || NumAmount.Text == "")
                {
                    NumAmount.Text = "0";
                }
                EnableEnterButton();
            }
        }



        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(NumAmount.Text, out decimal result))
            {
                EnteredValue = result;
                DialogResult = true; // Close the dialog with success
            }
            else
            {
                MessageBox.Show("Please enter a valid number.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

       
        private void PinDisplay_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableEnterButton();
        }


        private void PinDisplay_LostFocus(object sender, RoutedEventArgs e)
        {
            
        }
        private void EnableEnterButton()
        {
            Btnenter.IsEnabled = !string.IsNullOrWhiteSpace(NumAmount.Text) && NumAmount.Text != "0";
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
