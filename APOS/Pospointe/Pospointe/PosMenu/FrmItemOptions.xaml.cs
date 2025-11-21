using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using static Pospointe.PosMenu.FrmPosMain;

namespace Pospointe.PosMenu
{
    /// <summary>
    /// Interaction logic for FrmItemOptions.xaml
    /// </summary>
    public partial class FrmItemOptions : Window
    {
        public bool IsPriceChanged { get; private set; }
        public bool IsQuantityChanged { get; private set; }

        public ItemGridModel SelectedItem { get; private set; }
        public bool IsItemRemoved { get; private set; }
        public FrmItemOptions(ItemGridModel selectedItem)
        {
            InitializeComponent();
            SelectedItem = selectedItem;
            DataContext = selectedItem;
            IsItemRemoved = false;

            this.Loaded += FrmItemOptions_Loaded;
        }

        private async void FrmItemOptions_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(150); // let double-tap complete
            BtnRemoveItem.Focus();
            Keyboard.Focus(BtnRemoveItem);
        }


        private void BtnRemoveTax_Click(object sender, RoutedEventArgs e)
        {


        }

        private void BtnRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                IsItemRemoved = true;
                DialogResult = true; 
                Close(); 
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in BtnRemove_Click: {ex.Message}\n{ex.StackTrace}");
            }
        }


        private void BtnChangeQuantity_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Owner is FrmPosMain posMainWindow)
                {
                    IsQuantityChanged = true; 
                    DialogResult = true; 
                    Close();
                }
                else
                {
                    MessageBox.Show("Unable to update quantity. POS main window not found.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in BtnChangeQuantity_Click: {ex.Message}\n{ex.StackTrace}");
            }
        }



        private void BtnPriceChange_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Owner is FrmPosMain posMainWindow)
                {
                    IsPriceChanged = true; 
                    DialogResult = true; 
                    Close();
                }
                else
                {
                    MessageBox.Show("Unable to update price. POS main window not found.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in BtnPriceChange_Click: {ex.Message}\n{ex.StackTrace}");
            }
        }





        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
           // MessageBox.Show("Touch Detected");
            this.Close();
        }
    }
}
