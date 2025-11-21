using Newtonsoft.Json;
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

namespace Pospointe.GuftCard
{
    /// <summary>
    /// Interaction logic for FrmBalanceCheck.xaml
    /// </summary>
    public partial class FrmBalanceCheck : Window
    {
        public string content {  get; set; }
        public FrmBalanceCheck()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GiftCardProccess.ResponseBalanceCheck myDeserializedClass = JsonConvert.DeserializeObject<GiftCardProccess.ResponseBalanceCheck>(content);

            TxtBalance.Text = myDeserializedClass.balance.ToString();
            TxtCardNumber.Text = myDeserializedClass.cardending.ToString();
            TxtExpDate.Text = myDeserializedClass.expire.ToString();
            TxtLastUsedDate.Text = myDeserializedClass.lastused.ToString();
            TxtBalance.Text = myDeserializedClass.balance.ToString();
            TxtStatus.Text = myDeserializedClass.status.ToString();
            TxtCreatedDate.Text = myDeserializedClass.activated.ToString();
        }
    }
}
