using Newtonsoft.Json;
using Pospointe.LocalData;
using Pospointe.Models;
using Pospointe.Services;
using RestSharp;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Pospointe.MainMenu
{
    /// <summary>
    /// Interaction logic for FrmonlineOrders.xaml
    /// </summary>
    public partial class FrmonlineOrders : Window
    {
        private SettingsData settingsj = new SettingsData();
        private List<Order> allOrders = new List<Order>();
        public POSOrderModel ob { get; set; }
        private List<Order> filteredOrders = new List<Order>();
        string marketPlaceDeviceId = LoggedData.Reg.marketPlaceDeviceId?.ToString() ?? string.Empty;

        public FrmonlineOrders()

        {
            
            InitializeComponent();
        }

        private void ResetFilterButtonStyles()
        {
            BtnAllOrders.ClearValue(BackgroundProperty);
            BtnCartpointe.ClearValue(BackgroundProperty);
            BtnUbereats.ClearValue(BackgroundProperty);
            BtnDoorDash.ClearValue(BackgroundProperty);
            BtnGrubHub.ClearValue(BackgroundProperty);
        }

        private void BtnAllOrders_Click(object sender, RoutedEventArgs e)
        {
            filteredOrders = new List<Order>(allOrders);
            OrdersGrid.ItemsSource = filteredOrders;
        }

        private void BtnDoorDash_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnUbereats_Click(object sender, RoutedEventArgs e)
        {
            ResetFilterButtonStyles();
            BtnUbereats.Background = Brushes.DarkRed;
            BtnUbereats.Foreground = Brushes.White;

            filteredOrders = allOrders.Where(o => o.Platform == "UberEats").ToList();
            OrdersGrid.ItemsSource = filteredOrders;
        }

        private void BtnGrubHub_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCartpointe_Click(object sender, RoutedEventArgs e)
        {
            ResetFilterButtonStyles();
            BtnCartpointe.Background = Brushes.DarkRed;
            BtnCartpointe.Foreground = Brushes.White;
            filteredOrders = allOrders.Where(o => o.Platform == "CartPointe").ToList();
            OrdersGrid.ItemsSource = filteredOrders;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDetails_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Order ord)
            {
                POSOrderModel myDeserializedClass = JsonConvert.DeserializeObject<POSOrderModel>(ord.OrderData);
              //  ob = myDeserializedClass;
                FrmOnlineOrderView frmOnlineOrderView = new FrmOnlineOrderView();
                frmOnlineOrderView.POSOrderModel = myDeserializedClass;
                frmOnlineOrderView.ShowDialog();
            }
        }

        private async void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            Button btn1 = sender as Button;
            try
            {
                btn1.IsEnabled = false;
                ShowOverlay("Printing Order...", true);

                if (sender is Button btn && btn.DataContext is Order ord)
                {
                    POSOrderModel myDeserializedClass = JsonConvert.DeserializeObject<POSOrderModel>(ord.OrderData);
                    ob = myDeserializedClass;
                    bool pr = await PrintService.PrintMarketPlaceOrder(ob);
                }
                await getallordersoftoday();
            }

            finally
            {
                ShowOverlay("", false);
                btn1.IsEnabled = true;
            }
            
        }



        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            await getallordersoftoday();

            var time = new System.Windows.Threading.DispatcherTimer();
            time.Interval = TimeSpan.FromSeconds(10);
            time.Tick += Time_Tick;
            time.Start();


        }

        private void Time_Tick(object? sender, EventArgs e)
        {
             getallordersoftoday();
           // MessageBox.Show("Refrsh");
        }

        

        public async Task getallordersoftoday()
        {
            var options = new RestClientOptions(clsConnections.marketplaceserver)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest($"/Orders/{marketPlaceDeviceId}", Method.Get);

            RestResponse response = await client.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                allOrders = JsonConvert.DeserializeObject<List<Order>>(response.Content);

                foreach (var order in allOrders)
                {
                    switch (order.Platform)
                    {
                        case "DoorDash":
                            order.ProviderLogo = "https://www.pngall.com/wp-content/uploads/15/Door-Dash-Logo-PNG-Images.png";
                            break;

                        case "UberEats":
                            order.ProviderLogo = "https://fileserver.asnit.us/UploadedFiles/Pospointe/dbfc8791-43d7-4866-817c-f68d5494fbe4.png";
                            break;

                        case "GrubHub":
                            order.ProviderLogo = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS-3fRA3X6dRZjvdgMoluWm3VR5giCXpIANeA&s";
                            break;
                            
                        case "CartPointe":
                            order.ProviderLogo = "https://pospointe.com/wp-content/uploads/2023/04/POS_POINTE-1.png";
                            break;

                    }
                    if (order.Status == "ACCEPTED")
                    {
                        
                    }
                    ActionBtns();
                }

                OrdersGrid.ItemsSource = null;
                OrdersGrid.ItemsSource = allOrders;
                
            }
        }

        private void ActionBtns()
        {
            
        }


        bool cancelview (object sender, RoutedEventArgs e) {

            if (sender is Button btn && btn.DataContext is Order ord)
            {
                if (ord.Status == "Completed")
                {
                    return false;
                }

                else
                {
                    return true;
                }
            }
            return false;
        }


        public class SettingsData
        {
            public string MarketPlaceDeviceId { get; set; }
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

        private void BtnCancell_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            
            if (sender is Button btn && btn.DataContext is Order ord)
            {
                try
                {
                    btn.IsEnabled = false;
                    ShowOverlay("Cancelling Order...", true);

                    POSOrderModel myDeserializedClass = JsonConvert.DeserializeObject<POSOrderModel>(ord.OrderData);
                    //   ob = myDeserializedClass;
                    var json = JsonConvert.SerializeObject(myDeserializedClass);
                    var options = new RestClientOptions(clsConnections.marketplaceserver)
                    {
                        MaxTimeout = -1,
                    };
                    var client = new RestClient(options);
                    var request = new RestRequest($"Orders/cancelorder/{ord.OrderId}", Method.Put);
                    request.AddParameter("application/json", json, ParameterType.RequestBody);
                    RestResponse response = await client.ExecuteAsync(request);
                    Console.WriteLine(response.Content);
                    if (response.IsSuccessful)
                    {
                        // if (localdata.autoprint == true)
                        // {
                        //     bool pr = await PrintService.PrintMarketPlaceOrder(Ord);
                        // }

                        MessageBox.Show("Order Canceled Successfully");

                    }
                    else
                    {
                        MessageBox.Show(response.Content);
                    }
                    await getallordersoftoday();
                }

                finally
                {
                    ShowOverlay("", false);
                    btn.IsEnabled = true;
                }
                
                
            }
        }

        private async void BtnMarkasReady_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Order ord)
            {// var json = JsonConvert.SerializeObject(order);
                try
                {
                    btn.IsEnabled = false;
                    ShowOverlay("Marking as Ready...", true);

                    var options = new RestClientOptions(clsConnections.marketplaceserver)
                    {
                        MaxTimeout = -1,
                    };
                    var client = new RestClient(options);
                    var request = new RestRequest("Orders/readytopickup/" + ord.OrderId, Method.Put);
                    //request.AddParameter("application/json", json, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    if (response.IsSuccessful)
                    {

                        MessageBox.Show("Order Updated Successfully");


                    }

                    else
                    {

                        MessageBox.Show("Status Code :" + response.StatusCode + "Body :" + response.Content);
                    }
                    await getallordersoftoday();
                }

                finally
                {
                    ShowOverlay("", false);
                    btn.IsEnabled = true;
                }
                
            }

        }

        private async void BtnMarkCompleted_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Order ord)
            {// var json = JsonConvert.SerializeObject(order);

                try
                {
                    btn.IsEnabled = false;
                    ShowOverlay("Cancelling Order...", true);

                    var options = new RestClientOptions(clsConnections.marketplaceserver)
                    {
                        MaxTimeout = -1,
                    };
                    var client = new RestClient(options);
                    var request = new RestRequest("Orders/completeorder/" + ord.OrderId, Method.Put);
                    //request.AddParameter("application/json", json, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    if (response.IsSuccessful)
                    {

                        MessageBox.Show("Order Completed Successfully");


                    }

                    else
                    {

                        MessageBox.Show("Status Code :" + response.StatusCode + "Body :" + response.Content);
                    }
                    await getallordersoftoday();
                }

                finally
                {
                    ShowOverlay("", false);
                    btn.IsEnabled = true;
                }
            }
        }

        private void ShowOverlay(string message, bool show)
        {
            OverlayMessage.Text = message;
            FullScreenOverlay.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
        }

        public static class CustomMessageBox
        {
            public static void Show(string header, string message, bool isError = false)
            {
                var msg = new FrmCustommessage
                {
                    IsError = isError
                };

                msg.LblHeader.Text = header;
                msg.LblMessage.Text = message;
                msg.ShowDialog();
            }
        }



        private void BtnManageStore_Click(object sender, RoutedEventArgs e)
        {
            FrmManageStores frnman = new FrmManageStores();
            frnman.ShowDialog();
        }
    }
}
