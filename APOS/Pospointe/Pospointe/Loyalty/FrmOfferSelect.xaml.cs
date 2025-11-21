using System;
using System.Collections.Generic;
using System.Drawing;
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
using WpfImage = System.Windows.Controls.Image;
using System.Windows.Shapes;
using Drawing = System.Drawing;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using Microsoft.Extensions.Options;
using System.CodeDom;
using System.Windows.Xps.Packaging;

namespace Pospointe.Loyalty
{
    /// <summary>
    /// Interaction logic for FrmOfferSelect.xaml
    /// </summary>
    public partial class FrmOfferSelect : Window
    {
        public string selectedofferid { get; set; }
        public string offername { get; set; }
        public string offerid { get; set; }
        public string pointstoredeem { get; set; }
        public double offeramount { get; set; }
        private List<clsLoyalty.Offer> Items { get; set; }
        public FrmOfferSelect()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TxtPoints.Text = clsLoyalty.SelectedCustomer.SelectedcustomerCurrentPoints;
            loadalloffers();
        }

        private void loadalloffers()
        {
            WrapPanelOffers.Children.Clear();

            int availpoints = Convert.ToInt32(clsLoyalty.SelectedCustomer.SelectedcustomerCurrentPoints);

            var items = clsLoyalty.Offers
                .Where(x => int.TryParse(x.pointsRequired, out int requiredPoints) && requiredPoints <= availpoints)
                .ToList();

            int btnid = 1;
            foreach (var item in items)
            {
                btnid++;

                if (item.status)
                {
                    int reqpoints = Convert.ToInt32(item.pointsRequired);

                    StackPanel stackPanel = new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };

                    if (!string.IsNullOrEmpty(item.OfferImage))
                    {
                        try
                        {
                            WpfImage img = new WpfImage
                            {
                                Width = 100,
                                Height = 100,
                                Margin = new Thickness(5),
                                Stretch = Stretch.UniformToFill,
                                Source = new BitmapImage(new Uri(item.OfferImage, UriKind.RelativeOrAbsolute))
                            };

                            stackPanel.Children.Add(img);
                        }
                        catch { }
                    }

                    TextBlock txt = new TextBlock
                    {
                        Text = $"{item.description} ({item.pointsRequired} Points Required)",
                        FontWeight = FontWeights.Bold,
                        TextAlignment = TextAlignment.Center,
                        TextWrapping = TextWrapping.Wrap
                    };
                    stackPanel.Children.Add(txt);

                    Button myBtn = new Button
                    {
                        
                        Name = "btnoffer" + btnid,
                        Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255)),
                        Tag = item.id,
                        Width = WrapPanelOffers.ActualWidth / 3 - 10,
                        Height = 150,
                        Margin = new Thickness(5),
                        Content = stackPanel
                    };

                    myBtn.Click += ModButtonClickEvent;
                    WrapPanelOffers.Children.Add(myBtn);
                }
            }
        }


        private async void ModButtonClickEvent(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            this.selectedofferid = btn.Tag.ToString();
            this.DialogResult = true;
            this.Close();           

        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
