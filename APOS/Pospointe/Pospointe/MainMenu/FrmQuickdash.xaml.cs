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

namespace Pospointe.MainMenu
{
    /// <summary>
    /// Interaction logic for FrmQuickdash.xaml
    /// </summary>
    public partial class FrmQuickdash : Window
    {

        public string url {  get; set; }

        public FrmQuickdash()
        {
            InitializeComponent();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Webview.Source = new System.Uri(url);
        }
    }
}
