using Newtonsoft.Json;
using RestSharp;
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

namespace Pospointe.Loyalty
{
    /// <summary>
    /// Interaction logic for FrmLoyalPrompt.xaml
    /// </summary>
    public partial class FrmLoyalPrompt : Window
    {
        public string phoneno {  get; set; }

        public string command { get; set; }
        public FrmLoyalPrompt()
        {
            InitializeComponent();
        }

        private async void BtnUnverfied_Click(object sender, RoutedEventArgs e)
        {
            clsLoyalty.Customer customer = new clsLoyalty.Customer
            {
                id = Guid.NewGuid().ToString(),
                memberedStoregrpid = clsConnections.thisstoregroupid,
                phoneNo = phoneno,
                firstName = "unverified",
                lastName = "User",
                email = "",
                signedupstoreId = clsConnections.thistoreid,
                signeddate = DateTime.Now,
                status = true,
                taxexcempt = false,
                loyalitypoints = "0",
                membershipcard = "0",
                address1 = "0",
                verified = false
                
            };
            var json = JsonConvert.SerializeObject(customer);
            var options = new RestClientOptions(clsConnections.loyaltyserver)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("Customers", Method.Post);
            request.AddHeader("Authorization", "Basic "+clsConnections.basicauthcode);
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            //request.AddHeader("Cookie", "ARRAffinity=92ca53ad8db4fbb93d4d3b7d8ab54dcf8ffecb2d731f25b0e91ad575d7534c3f; ARRAffinitySameSite=92ca53ad8db4fbb93d4d3b7d8ab54dcf8ffecb2d731f25b0e91ad575d7534c3f");
            RestResponse response = await client.ExecuteAsync(request);
            Console.WriteLine(response.Content);

            if (response.IsSuccessful)
            {
                //addtopos
                this.command = "unverified";
                this.DialogResult = true;
                this.Close();

            }

            else
            {
                MessageBox.Show(response.Content);
            }

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.command = "close";
            this.DialogResult = true;
            this.Close();
        }

        private void BtnRetry_Click(object sender, RoutedEventArgs e)
        {
            this.command = "retry";
            this.DialogResult = true;
            this.Close();
            // FrmLoyaltyPrompt newloyal = new FrmLoyaltyPrompt();
            //newloyal.ShowDialog();
        }
    }
}
