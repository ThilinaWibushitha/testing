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

namespace Pospointe
{
  
    /// <summary>
    /// Interaction logic for FrmCustommessage.xaml
    /// </summary>
    public partial class FrmCustommessage : Window
    {
        private DispatcherTimer _countdownTimer;
        private int _countdownValue = 60;
        public bool IsError { get; set; }
        public FrmCustommessage()
        {
            InitializeComponent();
            _countdownTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _countdownTimer.Tick += CountdownTimer_Tick;
            _countdownTimer.Start();

            CountdownTextBlock.Text = $"({_countdownValue})";
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            _countdownValue--;

            CountdownTextBlock.Text = $"({_countdownValue})";

            if (_countdownValue <= 0)
            {
                _countdownTimer.Stop();
                this.Close();
            }
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            _countdownTimer.Stop();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.IsError)
            {
                MessageBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                MessageBorder.Background = new SolidColorBrush(Colors.Red);
            }
        }
    }
}
