using Pospointe.Models;
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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace Pospointe.PosMenu
{
    /// <summary>
    /// Interaction logic for FrmModifier.xaml
    /// </summary>
    public partial class FrmModifier : Window
    {
        public List<TblItem> SelectedModifiers { get; private set; } = new List<TblItem>();

        private List<TblItem> _modifierItems;
        private TblItem _mainItem;
        private int _maxselect;

        public FrmModifier(List<TblItem> modifierItems, TblItem mainItem, int maxselect)
        {
            InitializeComponent();
            _modifierItems = modifierItems;
            _mainItem = mainItem;
            _maxselect = maxselect;
            LoadModifierItems();
        }

        private void LoadModifierItems()
        {
            ModifierPanel.Children.Clear();

            Grid modifierGrid = new Grid();

            int totalItems = _modifierItems.Count;
            int columns = 5;
            int rows = (int)Math.Ceiling((double)totalItems / columns);

            for (int i = 0; i < columns; i++)
            {
                modifierGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
            for (int i = 0; i < rows; i++)
            {
                modifierGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            for (int i = 0; i < totalItems; i++)
            {
                var modifier = _modifierItems[i];

                Button modifierButton = new Button
                {
                    Content = modifier.ItemName,
                    Tag = modifier,
                    Style = (Style)Application.Current.Resources["btnItems"],
                    Margin = new Thickness(5),
                    FontSize = 13,
                    
                    Width = 180,
                    Height = 100,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch
                };

                modifierButton.Click += ModifierButton_Click;

                int row = i / columns;
                int column = i % columns;

                Grid.SetRow(modifierButton, row);
                Grid.SetColumn(modifierButton, column);
                modifierGrid.Children.Add(modifierButton);
            }

            ModifierPanel.Children.Add(modifierGrid);
        }

       


        private void ModifierButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is TblItem modifier)
            {

                //MessageBox.Show(_maxselect.ToString());

                    if (!SelectedModifiers.Contains(modifier))
                    {
                    if (SelectedModifiers.Count +1 <= _maxselect)
                    {

                        SelectedModifiers.Add(modifier);
                        button.Background = Brushes.Red;
                    }

                    }
                    else
                    {
                        SelectedModifiers.Remove(modifier);
                        button.Background = Brushes.LightGray;
                    }
                }

                
            }
        

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnDone_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}