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
using System.Windows.Threading;
using System.Collections.ObjectModel;
using Pospointe.PosMenu;
using WpfScreenHelper;

namespace Pospointe.SecondScreens
{
    /// <summary>
    /// Interaction logic for FrmCusDis.xaml
    /// </summary>
    public partial class FrmCusDis : Window
    {
        private DispatcherTimer _timer;
        public FrmCusDis()
        {
            InitializeComponent();
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1) // Update every second
            };
            _timer.Tick += UpdateDateTime;
            _timer.Start();
        }

        private void UpdateDateTime(object sender, EventArgs e)
        {
            DateTimeDisplay.Text = DateTime.Now.ToString("dddd, MMMM dd yyyy - hh:mm:ss tt");
        }

        private void MediaElement_Loaded(object sender, RoutedEventArgs e)
        {
            var media = sender as MediaElement;
            media?.Play();
        }


        protected override void OnClosed(EventArgs e)
        {
            _timer.Stop();
            _timer.Tick -= UpdateDateTime;
            base.OnClosed(e);
        }

        public void  UpdateData(List<ItemGridModel> list , string subtotal, string tax , string grandtotal , string discount )
        {
            CustomerItemGrid.ItemsSource = null;
            CustomerItemGrid.Items.Refresh();
            CustomerItemGrid.ItemsSource = list;
            CustomerItemGrid.Items.Refresh();

            LblSubtotal.Text = subtotal;
            LblTax.Text = tax;
            LblTotal.Text = grandtotal;
            LblDiscount.Text = discount;
        }

        public void Reset()
        {
            CustomerItemGrid.ItemsSource = null;
            CustomerItemGrid.Items.Clear();
            LblSubtotal.Text = "$0.00";
            LblTax.Text = "$0.00";
            LblTotal.Text = "$0.00";
            LblDiscount.Text = "$0.00";
        }
    }
}
