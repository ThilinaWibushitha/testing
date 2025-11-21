using Pospointe.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Pospointe.LoginWindow
{
    /// <summary>
    /// Interaction logic for FrmMenuEditor.xaml
    /// </summary>
    public partial class FrmMenuEditor : Window
    {
        public ObservableCollection<TblItem> Items { get; set; }

        public FrmMenuEditor(ObservableCollection<TblItem> items)
        {
            InitializeComponent();
            Items = new ObservableCollection<TblItem>(items.OrderBy(x => x.ListOrder));
            MenuDataGrid.ItemsSource = Items;
        }

        private void MoveUp_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is TblItem selectedItem)
            {
                int currentIndex = Items.IndexOf(selectedItem);
                if (currentIndex > 0)
                {
                    // Swap with the item above
                    var itemAbove = Items[currentIndex - 1];
                    SwapListOrder(selectedItem, itemAbove);
                }
            }
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is TblItem selectedItem)
            {
                int currentIndex = Items.IndexOf(selectedItem);
                if (currentIndex < Items.Count - 1)
                {
                    // Swap with the item below
                    var itemBelow = Items[currentIndex + 1];
                    SwapListOrder(selectedItem, itemBelow);
                }
            }
        }

        private void SwapListOrder(TblItem item1, TblItem item2)
        {
            int tempOrder = item1.ListOrder ?? 0;
            item1.ListOrder = item2.ListOrder ?? 0;
            item2.ListOrder = tempOrder;

            Items = new ObservableCollection<TblItem>(Items.OrderBy(x => x.ListOrder));
            MenuDataGrid.ItemsSource = Items;
        }



        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new PosDb1Context())
                {
                    foreach (var item in Items)
                    {
                        var dbItem = context.TblItems.FirstOrDefault(x => x.ItemId == item.ItemId);
                        if (dbItem != null)
                        {
                            dbItem.ListOrder = item.ListOrder;
                        }
                    }
                    await context.SaveChangesAsync(); 
                }
                MessageBox.Show("Changes saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}