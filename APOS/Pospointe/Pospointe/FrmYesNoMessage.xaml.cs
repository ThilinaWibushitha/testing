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
    /// Interaction logic for FrmYesNoMessage.xaml
    /// </summary>
    public partial class FrmYesNoMessage : Window
    {
        public bool UserResponse { get; private set; }

        public FrmYesNoMessage(string header, string message)
        {
            InitializeComponent();
            LblHeader.Text = header;
            LblMessage.Text = message;
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            UserResponse = true;
            this.DialogResult = true;
            this.Close();
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            UserResponse = false;
            this.DialogResult = false;
            this.Close();
        }
    }

}
