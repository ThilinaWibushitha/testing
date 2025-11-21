using Newtonsoft.Json;
using Pospointe.Models;
using Pospointe.Services;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using static System.Net.Mime.MediaTypeNames;

namespace Pospointe.MainMenu
{
    /// <summary>
    /// Interaction logic for FrmOnlineOrderView.xaml
    /// </summary>
    public partial class FrmOnlineOrderView : Window
    {
        public POSOrderModel POSOrderModel { get; set; }
        public FrmOnlineOrderView()
        {
            InitializeComponent();
        }



        private async void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool pr = await PrintService.PrintMarketPlaceOrder(POSOrderModel);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while printing: {ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BtnAccpt_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Order ord)
            {

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
                //await getallordersoftoday();
            }
        }


        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TxtPlatform.Text = $"{POSOrderModel.platform} Order";
            TxtCustomerName.Text = $"{POSOrderModel.icustomer.FirstName} {POSOrderModel.icustomer.LastName}";
            TxtOrderNum.Text = POSOrderModel.simpleorderid;

            TxtSubTotal.Text = (POSOrderModel.iTotals.SubTotal ?? 0).ToString("C2");
            TxtTaxTotal.Text = (POSOrderModel.iTotals.TaxTotal ?? 0).ToString("C2");
            TxtGrandTotal.Text = (POSOrderModel.iTotals.GrandTotal).ToString("C2");


            TxtDate.Text = POSOrderModel.orderedDatetime.ToString();

            ImgPlatformLogo.Source = new BitmapImage(new Uri(logopath()));

            ItemsPanel.Children.Clear();

            int totalItems = 0;

            if (POSOrderModel.iItems != null)
            {
                foreach (var item in POSOrderModel.iItems)
                {
                    totalItems += item.Qty ?? 0;

                    // Main item row
                    var row = new Grid { Margin = new Thickness(0, 4, 0, 4) };
                    row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                    var itemName = new TextBlock
                    {
                        Text = $"{item.Qty} x {item.Name}",
                        FontSize = 16,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    Grid.SetColumn(itemName, 0);

                    var itemPrice = new TextBlock
                    {
                        Text = $"{item.Price:C2}",
                        FontSize = 16,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Right
                    };
                    Grid.SetColumn(itemPrice, 1);

                    row.Children.Add(itemName);
                    row.Children.Add(itemPrice);

                    ItemsPanel.Children.Add(row);

                    // Modifiers
                    if (item.Modifers != null)
                    {
                        foreach (var modgrps in item.Modifers)
                        {
                            foreach (var mod in modgrps.ModiferOptions)
                            {
                                var modRow = new Grid { Margin = new Thickness(20, 2, 0, 2) };
                                modRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                                modRow.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                                var modName = new TextBlock
                                {
                                    Text = $"{mod.Qty} x {mod.Name}",
                                    FontSize = 14,
                                    Foreground = Brushes.Gray
                                };
                                Grid.SetColumn(modName, 0);

                                var modPrice = new TextBlock
                                {
                                    Text = $"{mod.Price:C2}",
                                    FontSize = 14,
                                    Foreground = Brushes.Gray,
                                    HorizontalAlignment = HorizontalAlignment.Right
                                };
                                Grid.SetColumn(modPrice, 1);

                                modRow.Children.Add(modName);
                                modRow.Children.Add(modPrice);

                                ItemsPanel.Children.Add(modRow);
                            }
                        }
                    }

                    // Special instructions
                    if (!string.IsNullOrEmpty(item.Special_instructions))
                    {
                        ItemsPanel.Children.Add(new TextBlock
                        {
                            Text = $"Note: {item.Special_instructions}",
                            FontSize = 12,
                            Foreground = Brushes.DarkSlateGray,
                            Margin = new Thickness(20, 0, 0, 5),
                            TextWrapping = TextWrapping.Wrap
                        });
                    }
                }
            }

            TxtItemCount.Text = $"Total Items: {totalItems}";
        }





        private string logopath()
        {
            if (POSOrderModel.platform == "DoorDash")
            {
                return "https://www.pngall.com/wp-content/uploads/15/Door-Dash-Logo-PNG-Images.png";
            }
            else if (POSOrderModel.platform == "UberEats")
            {
                return "https://fileserver.asnit.us/UploadedFiles/Pospointe/dbfc8791-43d7-4866-817c-f68d5494fbe4.png";
            }
            else if (POSOrderModel.platform == "GrubHub")
            {
                return "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS-3fRA3X6dRZjvdgMoluWm3VR5giCXpIANeA&s";
            }
            else if (POSOrderModel.platform == "Cart Pointe")
            {
                return "https://pospointe.com/wp-content/uploads/2023/04/POS_POINTE-1.png";
            }

            return "";

        }
    }
}
