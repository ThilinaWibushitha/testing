using MahApps.Metro.IconPacks;
using Newtonsoft.Json;
using Pospointe.GuftCard;
using Pospointe.LocalData;
using Pospointe.LoginWindow;
using Pospointe.Loyalty;
using Pospointe.Models;
using Pospointe.Services;
using Pospointe.Trans_Api;
using RestSharp;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Pospointe.PosMenu
{
    public partial class FrmPosMain : Window

    {
        public bool IsQuantityChanged { get; private set; }
        private MainWindowViewModel _viewModel;
        public List<TblDepartment> localdeparment = new List<TblDepartment>();
        public static List<ItemGridModel> SelectedItems { get; set; } = new List<ItemGridModel>();
        private bool DiscountApplied = false;
        private decimal AppliedDiscountAmount = 0;
        private bool IsPercentageDiscount = false;
        private static System.Timers.Timer timer;
        public Guid unid;
        public string shortid = "";
        private static int elapsedMinutes = 0;
        public decimal Tax1 = 0;
        private Point _touchStartPoint;
        private bool _isTouchScrolling;
        public bool offerselected = false;
        private Button _selectedDepartmentButton;


        public string DataFromMainWindow { get; set; } = "Hello from MainWindow!";
        public FrmPosMain()
        {
            InitializeComponent();
            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;

        }


        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            TxtScan.Focus();
            AddButtonsToGrid();
            //GetCurrentTaxRate();
            LoadTaxRates();
            ItemDataGrid.ItemsSource = SelectedItems;
            resetscreen();

            DepartmentScrollViewer.TouchDown += ScrollViewer_TouchDown;
            DepartmentScrollViewer.TouchMove += ScrollViewer_TouchMove;
            DepartmentScrollViewer.TouchUp += ScrollViewer_TouchUp;

            ItemScrollViewer.TouchDown += ScrollViewer_TouchDown;
            ItemScrollViewer.TouchMove += ScrollViewer_TouchMove;
            ItemScrollViewer.TouchUp += ScrollViewer_TouchUp;

            await UpdateCustomButtons();

            await getoffersloyalty();


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

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItems.Count > 0)
            {
                CustomMessageBox.Show("Warning", "Cannot Exit with Items in the Bill List", true);
            }
            else
            {
                this.Close();
            }
        }


        private async Task getoffersloyalty()
        {
            if (clsConnections.loyaltyactive == true)
            {
                var options = new RestClientOptions(clsConnections.loyaltyserver)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("Offers/groupedoffers/" + clsConnections.thisstoregroupid + "?InStore=true", Method.Get);
                request.AddHeader("Authorization", "Basic " + clsConnections.basicauthcode);

                RestResponse response = await client.ExecuteAsync(request);
                Console.WriteLine(response.Content);
                if (response.StatusCode == HttpStatusCode.OK)
                {

                    var objResponse1 = JsonConvert.DeserializeObject<List<clsLoyalty.Offer>>(response.Content);
                    clsLoyalty.Offers = objResponse1;
                    var pin = clsLoyalty.Offers.OrderByDescending(p => p.pointsRequired).LastOrDefault();
                    if (pin != null)
                    {
                        clsLoyalty.minimumpointsforoffers = Convert.ToInt32(pin.pointsRequired);
                    }
                }

                else
                {
                    CustomMessageBox.Show("Loyalty Service Unavailable",
                        "We could not load loyalty offers at the moment.\n\n" +
                        "Please try again later or contact support if the issue continues.",
                        true

                        );

                }

            }
        }


        private void ScrollViewer_TouchDown(object sender, TouchEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer)
            {
                _touchStartPoint = e.GetTouchPoint(scrollViewer).Position;
                _isTouchScrolling = true;
            }
        }

        private void ScrollViewer_TouchMove(object sender, TouchEventArgs e)
        {
            if (_isTouchScrolling && sender is ScrollViewer scrollViewer)
            {
                Point currentPoint = e.GetTouchPoint(scrollViewer).Position;
                double deltaY = _touchStartPoint.Y - currentPoint.Y;

                double newOffset = scrollViewer.VerticalOffset + deltaY;
                scrollViewer.ScrollToVerticalOffset(newOffset);

                _touchStartPoint = currentPoint;
            }
        }

        private void ScrollViewer_TouchUp(object sender, TouchEventArgs e)
        {
            _isTouchScrolling = false;
        }

        public static void startofflinetimer(bool completeoffline)
        {
            if (completeoffline)
            {
                LoggedData.Isoffline = true;
            }
            timer = new System.Timers.Timer(60000);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
            if (completeoffline)
            {

            }
            else
            {

            }
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            elapsedMinutes++;
            Console.WriteLine($"Timer elapsed: {elapsedMinutes} minute(s)");

            if (elapsedMinutes >= 3)
            {
                LoggedData.Isoffline = false;
                LoggedData.transbaseurl = clsConnections.transserverprimaryurl;
                timer.Stop();
                timer.Dispose();
                Console.WriteLine("3-minute timer complete. Timer stopped.");
            }
        }

        //private void GetCurrentTaxRate()
        //{
        //    using (var context = new PosDb1Context())
        //    {
        //        var t1 = context.TblTaxRates.Where(x => x.TaxNo == 1).FirstOrDefault();
        //        if (t1 == null)
        //        {
        //            //MessageBox.Show("No Tax Record Found So TaxRate Set to 0.00%");
        //        }
        //        else
        //        {
        //            Tax1 = t1.TaxRate ?? 0;
        //        }

        //    }
        //}

        private void AddButtonsToGrid()
        {
            using (var context = new PosDb1Context())
            {
                var departments = context.TblDepartments.Where(x => x.Visible == "OK").OrderBy(item => item.ListOrder).ToList();

                localdeparment = departments;

                foreach (var department in localdeparment)
                {
                    Button newButton = new Button
                    {
                        Padding = new Thickness(10),
                        Margin = new Thickness(10, 5, 10, 5),
                        Height = 120,
                        Width = 90,
                        Style = (Style)Application.Current.Resources["btnItems"],
                        Tag = department
                    };

                    StackPanel stackPanel = new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };

                    string imagePath = department.PicturePath;

                    Image icon = new Image
                    {
                        Source = new BitmapImage(new Uri(
                            string.IsNullOrWhiteSpace(imagePath)
                            ? "https://i.postimg.cc/sgnNqK12/cashier.png"
                            : imagePath,
                            UriKind.RelativeOrAbsolute)
                        ),
                        Width = 70,
                        Height = 80,
                        Margin = new Thickness(0, 0, 0, 10)
                    };


                    TextBlock textBlock = new TextBlock
                    {
                        Text = department.DeptName,
                        TextWrapping = TextWrapping.Wrap,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        TextAlignment = TextAlignment.Center,
                        FontSize = 14,
                        TextTrimming = TextTrimming.CharacterEllipsis,
                        Margin = new Thickness(0, 5, 0, 0)
                    };

                    stackPanel.Children.Add(icon);
                    stackPanel.Children.Add(textBlock);

                    newButton.Content = stackPanel;
                    newButton.Click += DepartmentButton_Click;

                    WrpDepartment.Children.Add(newButton);
                }
            }

            AdjustButtonWidths();
            WrpDepartment.SizeChanged += (s, e) => AdjustButtonWidths();
        }

        private void DepartmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button departmentButton && departmentButton.Tag is TblDepartment department)
            {
                if (_selectedDepartmentButton != null)
                {
                    _selectedDepartmentButton.ClearValue(Button.BackgroundProperty);
                    _selectedDepartmentButton.ClearValue(Button.ForegroundProperty);
                }

                _selectedDepartmentButton = departmentButton;
                _selectedDepartmentButton.Background = Brushes.DarkRed;
                _selectedDepartmentButton.Foreground = Brushes.White;

                LoadItemsForDepartment(department);
            }
        }


        private void LoadItemsForDepartment(TblDepartment department)
        {
            WrpItems.Children.Clear();

            if (department == null)
            {
                return;
            }

            using (var context = new PosDb1Context())
            {
                var items = context.TblItems
                    .Where(item => item.ItemDeptId == department.DeptId
                    && item.Visible == "OK" && (item.IsModifer == false || item.IsModifer == null))
                    .OrderBy(item => item.ListOrder)
                    .ToList();


                if (items.Count == 0)
                {
                    return;
                }

                double buttonWidth = (WrpItems.ActualWidth - (10 * 6)) / 5;
                WrapPanel wrapPanel = new WrapPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                foreach (var item in items)
                {
                    Button itemButton = new Button
                    {
                        Padding = new Thickness(10),
                        Margin = new Thickness(5),
                        Height = 100,
                        Width = buttonWidth,
                        Content = new TextBlock
                        {

                            Text = item.ItemName,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            TextWrapping = TextWrapping.Wrap,
                            TextAlignment = TextAlignment.Center,
                            TextTrimming = TextTrimming.CharacterEllipsis,
                            FontSize = Math.Min(24, Math.Max(12, buttonWidth / 12))


                        },
                        Style = (Style)Application.Current.Resources["btnItems"],
                        Tag = item
                    };

                    if (!string.IsNullOrWhiteSpace(item.BtnColor))
                    {
                        try
                        {
                            itemButton.Background = (Brush)new BrushConverter().ConvertFromString(item.BtnColor);
                        }
                        catch
                        {

                            itemButton.Background = Brushes.Gray;
                        }
                    }

                    itemButton.Click += ItemButton_Click;
                    wrapPanel.Children.Add(itemButton);
                }

                WrpItems.Children.Add(wrapPanel);
            }
        }

        private bool IsTaxApplied = true;
        private void BtnTax_Click(object sender, RoutedEventArgs e)
        {
            IsTaxApplied = !IsTaxApplied;
            calculatetotals();
        }




        private void ItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button itemButton && itemButton.Tag is TblItem item)
            {
                getitem(item);
            }
        }

        private decimal? PromptForPrice()
        {
            FrmNumbpad numPadWindow = new FrmNumbpad();

            if (numPadWindow.ShowDialog() != true)
                return null;

            if (!decimal.TryParse(numPadWindow.returnvalue, out decimal enteredPrice) || enteredPrice <= 0)
            {
                CustomMessageBox.Show("Error", "Invalid price entered. Please enter a valid amount.", true);
                return null;
            }

            return enteredPrice;
        }



        private void getitem(TblItem item)
        {
            decimal itemPrice = item.ItemPrice ?? 0;
            bool Tax1 = item.Tax1Status == "OK";
            int ordernum = SelectedItems.Count + 1;
            bool kot1 = item.IsKot == 1;
            bool kot2 = item.IsKot2 == true;

            if (item.PricePrompt == "OK")
            {
                decimal? enteredPrice = PromptForPrice();
                if (enteredPrice == null)
                {
                    return;
                }
                itemPrice = enteredPrice.Value;
            }


            var parentItem = new ItemGridModel
            {
                UniqueId = Guid.NewGuid(),
                Itemid = item.ItemId,
                ItemName = item.ItemName,
                Deptid = item.ItemDeptId,
                Price = itemPrice,
                Quantity = 1,
                TotalPrice = itemPrice,
                isTax1 = Tax1,
                OrderNo = ordernum,
                IsModifier = false,
                ParentItemId = Guid.Empty,
                IsKOT1 = kot1,
                IsKOT2 = kot2
            };

            SelectedItems.Add(parentItem);

            ItemDataGrid.Items.Refresh();

            using (var context = new PosDb1Context())
            {
                var modifierGroupIds = context.TblModifersofItems
                    .Where(modifier => modifier.ItemId == item.ItemId)
                    .Select(modifier => modifier.ModiferGroupId)
                    .ToList();

                if (modifierGroupIds.Any())
                {
                    foreach (var modgrp in modifierGroupIds)
                    {
                        var modifierItems = context.TblItems
                            .Where(i => i.ItemDeptId == modgrp.ToString() && i.Visible == "OK")
                            .ToList();
                        var maxselect = context.TblModiferGroups.Where(x => x.ModiferGroupId == modgrp).Select(x => x.MaximumSelect).FirstOrDefault();

                        FrmModifier modifierWindow = new FrmModifier(modifierItems, item, maxselect ?? 100);
                        if (modifierWindow.ShowDialog() == true)
                        {
                            foreach (var selectedModifier in modifierWindow.SelectedModifiers)
                            {
                                bool modifierTax1 = selectedModifier.Tax1Status == "OK";
                                ordernum = SelectedItems.Count + 1;
                                bool modkot1 = item.IsKot == 1;
                                bool modkot2 = item.IsKot2 == true;
                                SelectedItems.Add(new ItemGridModel
                                {
                                    UniqueId = Guid.NewGuid(),
                                    Itemid = selectedModifier.ItemId,
                                    ItemName = $"   - {selectedModifier.ItemName}",
                                    Deptid = selectedModifier.ItemDeptId,
                                    Price = selectedModifier.ItemPrice ?? 0,
                                    Quantity = 1,
                                    TotalPrice = selectedModifier.ItemPrice ?? 0,
                                    isTax1 = modifierTax1,
                                    OrderNo = ordernum,
                                    IsModifier = true,
                                    ParentItemId = parentItem.UniqueId,
                                    IsKOT1 = modkot1,
                                    IsKOT2 = modkot2
                                });
                            }
                            ItemDataGrid.Items.Refresh();
                        }
                    }
                }
            }
            calculatetotals();

        }


        public void ChangeItemPrice(ItemGridModel selectedItem)
        {
            // Safely check nullable bool
            bool hasPermission = LoggedData.loggeduser.Allowpricechange == true;

            if (!hasPermission)
            {
                FrmPermissionShow frmPermissionShow = new FrmPermissionShow();
                frmPermissionShow.reqpermssion = "prmallowpricechange";

                hasPermission = frmPermissionShow.ShowDialog() == true;
            }

            if (hasPermission)
            {
                FrmNumbpad numPadWindow = new FrmNumbpad();
                if (numPadWindow.ShowDialog() == true &&
                    decimal.TryParse(numPadWindow.returnvalue, out decimal enteredPrice) &&
                    enteredPrice > 0)
                {
                    selectedItem.Price = enteredPrice;
                    calculatetotals();
                }
                else
                {
                    CustomMessageBox.Show("Invalid Price", "Invalid price entered. Please enter a valid amount.", true);
                }
            }
        }






        public void ChangeItemQuantity(ItemGridModel selectedItem)
        {


            FrmNumbpad numPadWindow = new FrmNumbpad();
            if (numPadWindow.ShowDialog() == true &&
                int.TryParse(numPadWindow.returnvalue, out int enteredQty) && enteredQty > 0)
            {
                selectedItem.Quantity = enteredQty;
                calculatetotals();
            }
            else
            {
                CustomMessageBox.Show("Invalid Quantity", "Invalid quantity entered. Please enter a valid number.", true);
            }
        }



        private void ItemDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ItemDataGrid.SelectedItem is ItemGridModel selectedItem)
            {
                if (!selectedItem.IsDoubleClickable)
                {
                    CustomMessageBox.Show(
                        "Action Restricted",
                        "Voided item cannot be modified.",
                        true
                    );
                    return;
                }

                if (selectedItem.Itemid == "BRETURN")
                {
                    CustomMessageBox.Show(
                        "Action Restricted",
                        "Manual returns cannot be modified.",
                        true
                    );
                    return;
                }

                var itemOptionsWindow = new FrmItemOptions(selectedItem)
                {
                    Owner = this
                };

                bool? dialogResult = itemOptionsWindow.ShowDialog();

                if (dialogResult == true)
                {
                    if (itemOptionsWindow.IsItemRemoved)
                    {
                        if (Properties.Settings.Default.ShowVoidedItems)
                        {
                            selectedItem.IsDoubleClickable = false;
                            selectedItem.IsVoided = true;

                            int index = SelectedItems.IndexOf(selectedItem) + 1;
                            SelectedItems.Insert(index, new ItemGridModel
                            {
                                UniqueId = Guid.NewGuid(),
                                OrderNo = 0,
                                ItemName = $"(Voided {selectedItem.ItemName})",
                                Price = -selectedItem.Price,
                                Quantity = 0,
                                TotalPrice = -selectedItem.TotalPrice,
                                IsVoided = true,
                                IsDoubleClickable = false
                            });

                            var modifiersToVoid = SelectedItems
                                .Where(item => item.ParentItemId == selectedItem.UniqueId && !item.IsVoided)
                                .ToList();

                            foreach (var modifier in modifiersToVoid)
                            {
                                modifier.IsVoided = true;
                                modifier.IsDoubleClickable = false;

                                int modifierIndex = SelectedItems.IndexOf(modifier) + 1;
                                SelectedItems.Insert(modifierIndex, new ItemGridModel
                                {
                                    UniqueId = Guid.NewGuid(),
                                    OrderNo = 0,
                                    ItemName = $"(Voided {modifier.ItemName})",
                                    Price = -modifier.Price,
                                    Quantity = 0,
                                    TotalPrice = -modifier.TotalPrice,
                                    IsVoided = true,
                                    IsDoubleClickable = false
                                });
                            }
                        }
                        else
                        {
                            SelectedItems.Remove(selectedItem);

                            var modifiersToRemove = SelectedItems
                                .Where(item => item.ParentItemId == selectedItem.UniqueId)
                                .ToList();

                            foreach (var modifier in modifiersToRemove)
                            {
                                SelectedItems.Remove(modifier);
                            }
                        }
                    }
                    else
                    {
                        if (itemOptionsWindow.IsPriceChanged)
                        {
                            ChangeItemPrice(selectedItem);
                        }
                        else if (itemOptionsWindow.IsQuantityChanged)
                        {
                            ChangeItemQuantity(selectedItem);
                        }
                    }

                    UpdateOrderNumbers();
                    ItemDataGrid.ItemsSource = null;
                    ItemDataGrid.ItemsSource = SelectedItems;
                    calculatetotals();
                }
            }
            else
            {
                CustomMessageBox.Show("Selection Required", "No item selected. Please select an item first.",
                    true
                    );
            }
        }

        private TouchPoint _lastTouchPoint;
        private DateTime _lastTouchTime;

        private void ItemDataGrid_TouchDown(object sender, TouchEventArgs e)
        {
            var currentTouch = e.GetTouchPoint(ItemDataGrid);
            var currentTime = DateTime.Now;

            if (_lastTouchPoint != null &&
                (currentTime - _lastTouchTime).TotalMilliseconds <= 300)
            {
                var row = FindVisualParent<DataGridRow>(e.OriginalSource as DependencyObject);
                if (row != null && row.Item is ItemGridModel selectedItem)
                {
                    ItemDataGrid.SelectedItem = selectedItem;
                    ItemDataGrid_MouseDoubleClick(sender, null);
                }

                _lastTouchPoint = null;
            }
            else
            {
                _lastTouchPoint = currentTouch;
                _lastTouchTime = currentTime;
            }
        }
        private T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            while (child != null)
            {
                if (child is T parent)
                    return parent;

                child = VisualTreeHelper.GetParent(child);
            }
            return null;
        }
        private void UpdateOrderNumbers()
        {
            for (int i = 0; i < SelectedItems.Count; i++)
            {
                SelectedItems[i].OrderNo = i + 1;
            }
        }

        private void calculatetotals()
        {
            decimal SubTotal = SelectedItems.Sum(i => i.TotalPrice);

            if (SubTotal < 0)
            {
                BtnDiscount.IsEnabled = false;
            }

            decimal discountAmount = 0;

            if (DiscountApplied)
            {
                discountAmount = IsPercentageDiscount
                    ? (AppliedDiscountAmount / 100) * SubTotal
                    : AppliedDiscountAmount;


                if (discountAmount > SubTotal)
                {
                    discountAmount = SubTotal;
                }
            }



            LblDiscount.Text = discountAmount.ToString("C2", CultureInfo.GetCultureInfo("en-US"));

            decimal totalTaxableAmount = SelectedItems
                .Where(i => i.isTax1 && !i.IsVoided)
                .Sum(i => i.TotalPrice);


            decimal adjustedTaxableAmount = totalTaxableAmount;
            if (discountAmount > 0 && SubTotal > 0)
            {
                adjustedTaxableAmount -= (discountAmount * (totalTaxableAmount / SubTotal));
            }


            decimal Tax1amount = 0;
            if (IsTaxApplied && adjustedTaxableAmount > 0)
            {
                Tax1amount = adjustedTaxableAmount * Tax1 / 100;
            }


            decimal GrandTotal = SubTotal - discountAmount + Tax1amount;


            SubTotal = Math.Round(SubTotal, 2);
            discountAmount = Math.Round(discountAmount, 2);
            Tax1amount = Math.Round(Tax1amount, 2);
            GrandTotal = Math.Round(GrandTotal, 2);


            LblSubtotal.Text = SubTotal.ToString("C2", CultureInfo.GetCultureInfo("en-US"));
            LblTax.Text = Tax1amount.ToString("C2", CultureInfo.GetCultureInfo("en-US"));
            LblTotal.Text = GrandTotal.ToString("C2", CultureInfo.GetCultureInfo("en-US"));

            BtnPay.Content = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Children =
                {
                    new PackIconMaterial { Kind = PackIconMaterialKind.Cash, Width = 30, Height = 40, Margin = new Thickness(20, 0, 0, 0) },
                    new TextBlock { Text = $"Pay {GrandTotal:C2}", VerticalAlignment = VerticalAlignment.Center, TextWrapping = TextWrapping.Wrap, Width = 150, Margin = new Thickness(20, 0, 0, 0) }
                }
            };

            UpdatePayButtonState();

            if (Properties.Settings.Default.CustomerDisplay == true)
            {
                if (FrmLogin.frmCusDis != null)
                {

                    FrmLogin.frmCusDis.UpdateData(SelectedItems,
                        LblSubtotal.Text,
                        LblTax.Text,
                        LblTotal.Text,
                        LblDiscount.Text);

                }
            }

            GrandTotalforPAX = LblTotal.Text;
            TaxTotalforPAX = LblTax.Text;

            if (Properties.Settings.Default.PAXCustomerDisplay == true)
            {
                var worker = new BackgroundWorker();
                worker.DoWork += (sender, args) =>
                {

                    CallShowItemAsync();
                };

                worker.RunWorkerAsync();

            }


        }

        public static string TaxTotalforPAX = "";
        public static string GrandTotalforPAX = "";

        public static async Task CallShowItemAsync()
        {
            Task.Run(() => clsPAXCD.ShowItem(SelectedItems, TaxTotalforPAX, GrandTotalforPAX));
        }

        private void AdjustButtonWidths()
        {
            if (WrpDepartment.ActualWidth <= 0) return;

            foreach (Button button in WrpDepartment.Children)
            {
                button.Width = WrpDepartment.ActualWidth - 20;
            }
        }


        private async void BtnRecallInvoice_Click(object sender, RoutedEventArgs e)
        {
            bool hasPermission = LoggedData.loggeduser.LogtOut == true;
            if (!hasPermission)
            {
                FrmPermissionShow frmPermissionShow = new FrmPermissionShow();
                frmPermissionShow.reqpermssion = "prmrecallinvoice";

                hasPermission = frmPermissionShow.ShowDialog() == true;
            }

            if (hasPermission)
            {
                FrmRecall frmrecall = new FrmRecall();
                frmrecall.transtype = "ALL";
                var result = frmrecall.ShowDialog();

                if (result == true)
                {
                    await getaninvoice(frmrecall.recalledinvoiceID);
                }
            }

        }

        private string recalledHoldInvoiceId = null;

        private async void BtnRecallHold_Click(object sender, RoutedEventArgs e)
        {
            FrmRecall frmrecall = new FrmRecall();
            frmrecall.transtype = "HOLD";

            var result = frmrecall.ShowDialog();

            if (result == true)
            {
                string recalledInvoiceID = frmrecall.recalledinvoiceID;
                if (!string.IsNullOrEmpty(recalledInvoiceID))
                {
                    recalledHoldInvoiceId = recalledInvoiceID; // store it for later
                    await LoadHoldInvoice(recalledInvoiceID);
                }
            }

        }



        private decimal CurrentDiscountPercentage { get; set; } = 0;




        private async Task LoadHoldInvoice(string invoiceID)
        {
            try
            {
                using (var context = new PosDb1Context())
                {
                    var heldInvoice = context.TblTransMains.FirstOrDefault(x => x.InvoiceId.ToString() == invoiceID && x.TransType == "HOLD");

                    if (heldInvoice == null)
                    {
                        MessageBox.Show("Invoice not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    var invoiceItems = context.TblTransSubs.Where(x => x.TransMainId == heldInvoice.InvoiceId).ToList();

                    if (!invoiceItems.Any())
                    {
                        MessageBox.Show("No items found for this invoice.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    SelectedItems.Clear();

                    foreach (var item in invoiceItems)
                    {
                        SelectedItems.Add(new ItemGridModel
                        {
                            UniqueId = Guid.NewGuid(),
                            Itemid = item.ItemId,
                            ItemName = item.ItemName,
                            Deptid = item.ItemType,
                            Price = (decimal)(item.ItemPrice ?? 0),
                            Quantity = (int)item.Qty,
                            TotalPrice = (decimal)(item.Amount ?? 0),
                            isTax1 = item.Tax1Status == "OK",
                            OrderNo = SelectedItems.Count + 1
                        });
                    }

                    ItemDataGrid.Items.Refresh();




                    calculatetotals();

                    BtnPay.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading held invoice: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void BtnRecall_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRecallold_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void BtnPay_Click(object sender, RoutedEventArgs e)
        {

            if (Properties.Settings.Default.PAXCustomerDisplay == true)
            {
                await CallShowItemAsync();
            }


            try
            {
                if (SelectedItems.Count == 0)
                {
                    ShowCustomMessage("Please select at least one item before proceeding with the payment.", true);
                    return;
                }

                if (!decimal.TryParse(LblTotal.Text.Replace("$", "").Trim(), out decimal totalAmount))
                {
                    ShowCustomMessage("The total amount is invalid. Please review the bill details and try again.", true);
                    return;
                }

                if (clsLoyalty.SelectedCustomer.SelectedCustomerID != "")
                {
                    if (offerselected == false)
                    {



                        if (clsLoyalty.minimumpointsforoffers != 0)
                        {
                            if (clsLoyalty.minimumpointsforoffers == Convert.ToInt32(clsLoyalty.SelectedCustomer.SelectedcustomerCurrentPoints) || clsLoyalty.minimumpointsforoffers < Convert.ToInt32(clsLoyalty.SelectedCustomer.SelectedcustomerCurrentPoints))
                            {
                                //  MessageBox.Show("Offerce Available");
                                showoffers();
                            }
                        }

                    }
                }

                //if (totalAmount < 0)
                //{
                //    ShowCustomMessage("Negative payment amounts are not allowed.", true);
                //    return;
                //}

                FrmPay pay = new FrmPay
                {
                    amountopay = totalAmount
                };
                var result = pay.ShowDialog();

                if (result == true)
                {
                    await SaveBill(totalAmount < 0 ? "RETURN" : "SALE");


                    if (pay.changeamount > 0)
                    {
                        FrmCustommessage msg = new FrmCustommessage
                        {
                            IsError = false
                        };
                        msg.LblHeader.Text = "Change";
                        msg.LblMessage.Text = $"Cash Change: ${pay.changeamount:0.00}";
                        msg.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowCustomMessage($"An error occurred during the payment process:\n{ex.Message}", true);
            }
        }


        private void ShowCustomMessage(string message, bool isError = false)
        {
            FrmCustommessage frmCustommessage = new FrmCustommessage();
            frmCustommessage.LblMessage.Text = message;
            frmCustommessage.IsError = isError;
            frmCustommessage.ShowDialog();
        }

        private void resetscreen()
        {

            Properties.Settings.Default.CheckNo = Properties.Settings.Default.CheckNo + 1;
            Properties.Settings.Default.Save();

            clsLoyalty.SelectedCustomer.SelectedCustomerID = "";

            offerselected = false;

            BtnEndview.Visibility = Visibility.Hidden;
            BtnPay.Visibility = Visibility.Visible;

            BtnReprint.Visibility = Visibility.Hidden;
            BtnHold.Visibility = Visibility.Visible;

            BtnVoid.Visibility = Visibility.Hidden;
            BtnCustomers.Visibility = Visibility.Visible;

            BtnRefund.Visibility = Visibility.Hidden;

            BtnReset.Visibility = Visibility.Visible;
            BtnDiscount.Visibility = Visibility.Visible;

            BtnTax.Visibility = Visibility.Visible;
            BtnOrderType.Visibility = Visibility.Visible;

            BtnSendtoKot.Visibility = Visibility.Visible;

            LblCustomerName.Visibility = Visibility.Visible;
            LblPoints.Visibility = Visibility.Visible;

            //RESET discount button
            BtnDiscount.IsEnabled = true;
            DiscountTxt.Text = string.Empty;

            //RESET BILL LIST


            SelectedItems.Clear();
            IsTaxApplied = true;
            DiscountApplied = false;
            AppliedDiscountAmount = 0;
            IsPercentageDiscount = false;

            ItemDataGrid.Items.Refresh();
            ItemDataGrid.ItemsSource = null;
            ItemDataGrid.ItemsSource = SelectedItems;

            //RESET TOTALS
            LblSubtotal.Text = "$0.00";
            LblTax.Text = "$0.00";
            LblTotal.Text = "$0.00";
            LblDiscount.Text = "0.00";
            LblDiscount.Text = "0.00";

            //RESET CUSTOMER DATA
            LblCustomerName.Text = "Customer : None";
            LblPoints.Text = "Points : 0";

            //RESET CURRENT INVOICE DATA
            CurrentTransData.MainData = null;
            CurrentTransData.MainData = new TransData.Transmain();
            CurrentTransData.transitems.Clear();
            CurrentTransData.MainData.cashierId = LoggedData.loggeduser.UserId;
            CurrentTransData.MainData.stationId = LoggedData.StationID;

            unid = Guid.NewGuid();
            shortid = GuidShortener.ShortenGuid(unid);
            CurrentTransData.MainData.InvoiceUniqueId = unid;
            CurrentTransData.MainData.InvoiceIdshortCode = shortid;

            //UPDATE CUSTOMER DISPLAY
            if (Properties.Settings.Default.CustomerDisplay == true)
            {
                if (FrmLogin.frmCusDis != null)
                {

                    FrmLogin.frmCusDis.Reset();

                }
            }
            calculatetotals();

            if (Properties.Settings.Default.PAXCustomerDisplay == true)
            {
                var worker = new BackgroundWorker();
                worker.DoWork += (sender, args) =>
                {

                    ResetPacScreenAsync();
                };

                worker.RunWorkerAsync();
            }

        }

        // private async Task SaveBill(string type, String strPrintType, decimal CC_amount, String CC_Ref, decimal Cash_amount, decimal cashchange_amount, decimal Refund_amount, string paidby, string retref, string cardtype, string cardholder, string staionName, string accounttype, string aid, string tcarqc, string entrymethod, string Href, string Host_Ref_Num, string Device_Org_Ref_Num, decimal tip, decimal giftbalance)
        private async Task SaveBill(string type)
        {
            CurrentTransData.MainData.transType = type;

            string paidby = "CASH";

            if (CurrentTransData.MainData.cardAmount > 0 && CurrentTransData.MainData.cashAmount == 0)
            {
                paidby = "CARD";
            }

            else if (CurrentTransData.MainData.cardAmount > 0 && CurrentTransData.MainData.cashAmount > 0)
            {
                paidby = "MIXED";
            }

            if (!string.IsNullOrEmpty(CurrentTransData.MainData.cardType) && CurrentTransData.MainData.cardType == "GIFTC")
            {
                paidby = "GIFTC";
            }


            if (type == "HOLD")
            {
                paidby = "";
            }

            using (var context = new PosDb1Context())
            {
                // If this transaction was recalled from a hold invoice, use that ID
                if (!string.IsNullOrEmpty(recalledHoldInvoiceId))
                {
                    var existingInvoice = context.TblTransMains
                        .FirstOrDefault(x => x.InvoiceId.ToString() == recalledHoldInvoiceId && x.TransType == "HOLD");

                    if (existingInvoice != null)
                    {
                        var heldItems = context.TblTransSubs
                            .Where(x => x.TransMainId == existingInvoice.InvoiceId)
                            .ToList();

                        context.TblTransSubs.RemoveRange(heldItems);
                        context.TblTransMains.Remove(existingInvoice);

                        await context.SaveChangesAsync();

                        // Reset after deletion
                        recalledHoldInvoiceId = null;
                    }
                }
            }

            CurrentTransData.MainData.transType = type;



            CurrentTransData.MainData.paidby = paidby;

            CurrentTransData.MainData.invoiceId = Properties.Settings.Default.CheckNo;

            decimal subtotal = Convert.ToDecimal(LblSubtotal.Text.Replace("$", "").Trim());
            decimal discountAmount = DiscountApplied
        ? (IsPercentageDiscount ? (AppliedDiscountAmount / 100) * subtotal : AppliedDiscountAmount)
        : 0;
            CurrentTransData.MainData.invoiceDiscount = (double)discountAmount;

            CurrentTransData.MainData.subtotal = Convert.ToDouble(LblSubtotal.Text.Replace("$", "").Trim());
            CurrentTransData.MainData.tax1 = Convert.ToDouble(LblTax.Text.Replace("$", "").Trim());
            CurrentTransData.MainData.grandTotal = Convert.ToDouble(LblTotal.Text.Replace("$", "").Trim());

            CurrentTransData.MainData.saleDateTime = DateTime.Now;



            //if (clsLoyalty.SelectedCustomer.Selectedcustomerphone != null) { trandata.phoneNo = clsLoyalty.SelectedCustomer.Selectedcustomerphone; } else { trandata.phoneNo = ""; }
            //if (clsLoyalty.SelectedCustomer.SelectedCustomerID != null) { trandata.customerId = clsLoyalty.SelectedCustomer.SelectedCustomerID; } else { trandata.customerId = "CASH"; }
            //if (clsLoyalty.SelectedCustomer.SelectedCustomerName != null) { trandata.customerName = clsLoyalty.SelectedCustomer.SelectedCustomerName; } else { trandata.customerName = ""; }




            //TRANS SUB DATA TO CLASS
            foreach (var i in SelectedItems)
            {
                decimal taxamount = i.TotalPrice * Tax1 / 100;
                string taxstatus = "OK";
                if (i.isTax1 == false)
                {
                    taxstatus = "NO";
                }
                //OLD CODE
                // sqlC.ExecQuery("INSERT INTO [Tbl_Trans_Sub] ([IDkey],[ID],[TransMainID],[ItemID],[ItemType],[ItemName],[ItemPrice],[Qty],[Tax1],[Amount],[SaleDateTime],[OrderID]) VALUES (NEWID(),'" + dgBillItemList.Rows[i].Cells[0].Value + "'," + TransID + ",'" + dgBillItemList.Rows[i].Cells[1].Value + "','" + dgBillItemList.Rows[i].Cells[2].Value + "','" + dgBillItemList.Rows[i].Cells[3].Value + "'," + dgBillItemList.Rows[i].Cells[4].Value + "," + dgBillItemList.Rows[i].Cells[5].Value + "," + dgBillItemList.Rows[i].Cells[6].Value + "," + dgBillItemList.Rows[i].Cells[7].Value + ",'" + DateTime.Now.ToString() + "', " + dgBillItemList.Rows[i].Cells[10].Value + ")");
                Guid newidkey = Guid.NewGuid();
                //NEW CODE
                TransData.Transitem subitem = new TransData.Transitem
                {
                    idkey = newidkey,
                    id = i.Itemid,
                    transMainId = Convert.ToInt32("45"),
                    itemId = i.Itemid,
                    itemType = i.Deptid,
                    itemName = i.ItemName,
                    itemPrice = Convert.ToDouble(i.Price),
                    qty = Convert.ToDouble(i.Quantity),
                    tax1 = Convert.ToDouble(taxamount),
                    tax1Status = taxstatus,
                    amount = Convert.ToDouble(i.TotalPrice),
                    saleDateTime = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now),
                    orderId = Convert.ToInt32(i.OrderNo)

                };
                CurrentTransData.transitems.Add(subitem);

            }

            await UpdateLoyaltydata(CurrentTransData.MainData.InvoiceIdshortCode);


            TransData.Root root = new TransData.Root();
            root.transmain = CurrentTransData.MainData;
            root.transitems = CurrentTransData.transitems;
            root.date = DateTime.Today.ToString("yyyy-MM-dd");
            root.time = DateTime.Now.ToString("HH:mm:ss");

            if (type == "HOLD")
            {
                await SaveBillService.SaveBilltoLocal(root);
                if (Properties.Settings.Default.TurnonHold == 1)
                {
                    PrintService.PrinterReceipt(out bool isSuccessful);
                    var itemswithkot = SelectedItems.Where(x => x.IsKOT1 == true).ToList();
                    if (itemswithkot != null)
                    {
                        //PrintService.PrintertoKitchen(SelectedItems);
                    }
                }

            }


            else
            {
                await SaveBillService.SaveBillAsync(root);
                await ReceiptPhrompt();
                if (root.transmain.paidby == "CASH")
                {
                    PrintService.OpenCashDrawer();
                }
            }



            resetscreen();

        }

        private async Task UpdateLoyaltydata(string invoiceid)
        {
            if (clsConnections.loyaltyactive == true)
            {
                if (clsLoyalty.SelectedCustomer.SelectedCustomerID != "999")
                {
                    //////////////////////RADEEM POINTS///////////////////////////////

                    if (clsLoyalty.pointstoradeem != 0)
                    {
                        clsLoyalty.UpdatePoints radeem = new clsLoyalty.UpdatePoints();
                        radeem.lastInvoiceid = invoiceid;
                        radeem.lastvisitedstore = clsConnections.thistoreid;
                        radeem.loyalitypoints = Convert.ToString(clsLoyalty.pointstoradeem);
                        var jsonradeem = JsonConvert.SerializeObject(radeem);
                        var options = new RestClientOptions(clsConnections.loyaltyserver)
                        {
                            MaxTimeout = -1,
                        };
                        var client = new RestClient(options);
                        var request = new RestRequest("Loyalty/radeempoints/" + clsLoyalty.SelectedCustomer.SelectedCustomerID, Method.Patch);
                        request.AddHeader("Authorization", "Basic " + clsConnections.basicauthcode);
                        request.AddParameter("application/json", jsonradeem, ParameterType.RequestBody);
                        RestResponse response = await client.ExecuteAsync(request);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            //clsLoyalty.SelectedCustomer.SelectedCustomerNewPoints = response2.Content;
                        }
                        else
                        {
                            MessageBox.Show("Error Code :" + response.StatusCode.ToString() + " Reason :" + response.StatusDescription);
                        }
                    }

                    //////////////////////UPDATE POINTS//////////////////////////////
                    clsLoyalty.UpdatePoints uppoints = new clsLoyalty.UpdatePoints();
                    uppoints.lastInvoiceid = invoiceid;
                    uppoints.lastvisitedstore = clsConnections.thistoreid;

                    uppoints.loyalitypoints = LblSubtotal.Text.Replace("$", "");
                    var json = JsonConvert.SerializeObject(uppoints);

                    var options2 = new RestClientOptions(clsConnections.loyaltyserver)
                    {
                        MaxTimeout = -1,
                    };
                    var client2 = new RestClient(options2);
                    var request2 = new RestRequest("Loyalty/updatepoints/" + clsLoyalty.SelectedCustomer.SelectedCustomerID, Method.Patch);
                    request2.AddHeader("Authorization", "Basic " + clsConnections.basicauthcode);
                    request2.AddParameter("application/json", json, ParameterType.RequestBody);

                    RestResponse response2 = await client2.ExecuteAsync(request2);
                    Console.WriteLine(response2.Content);


                    if (response2.StatusCode == HttpStatusCode.OK)
                    {
                        clsLoyalty.SelectedCustomer.SelectedCustomerNewPoints = response2.Content;

                    }
                    else
                    {
                        // MessageBox.Show("Error Code :" + response2.StatusCode.ToString() + " Reason :" + response2.StatusDescription);
                    }

                }



            }

        }

        public async Task ReceiptPhrompt()
        {

            if (Properties.Settings.Default.ReceiptOption == "Prompt" || Properties.Settings.Default.ReceiptOption == string.Empty)
            {//
             //PromptPage
             // MessageBox.Show("triggerd");
                FrmPrintprompt frmPrintprompt = new FrmPrintprompt();
                frmPrintprompt.ShowDialog();
            }
            else if (Properties.Settings.Default.ReceiptOption == "Print")
            {
                PrintService.PrinterReceipt(out bool isSuccessful);
            }

            else if (Properties.Settings.Default.ReceiptOption == "SMS")
            {
                MessageBox.Show("This Feature Not Available.");
            }

            else if (Properties.Settings.Default.ReceiptOption == "Print&SMS")
            {
                MessageBox.Show("This Feature Not Available.");
            }

            else if (Properties.Settings.Default.ReceiptOption == "None")
            {

            }

            else
            {
                //PromptPage
                // MessageBox.Show("triggerd");
                FrmPrintprompt frmPrintprompt = new FrmPrintprompt();
                frmPrintprompt.ShowDialog();

            }

        }






        private async Task getaninvoice(string invoiceid)
        {
            var options = new RestClientOptions(LoggedData.transbaseurl)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/Transactions/" + invoiceid, Method.Get);
            request.AddHeader("db", clsConnections.mydb);
            request.AddHeader("Authorization", clsConnections.transserverauth);

            RestResponse response = await client.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                resetscreen();
                TransData.Root root = new TransData.Root();
                root = JsonConvert.DeserializeObject<TransData.Root>(response.Content);
                root.transitems = root.transitems.OrderBy(x => x.orderId).ToList();
                BtnEndview.Visibility = Visibility.Visible;
                BtnPay.Visibility = Visibility.Hidden;

                BtnReprint.Visibility = Visibility.Visible;
                BtnHold.Visibility = Visibility.Hidden;

                BtnCustomers.Visibility = Visibility.Hidden;
                BtnVoid.Visibility = Visibility.Visible;
                BtnRefund.Visibility = Visibility.Visible;
                if (root.transmain.transType == "VOID" || root.transmain.transType == "RETURN")
                {
                    BtnVoid.Visibility = Visibility.Hidden;
                    BtnRefund.Visibility = Visibility.Hidden;
                }

                LblCustomerName.Visibility = Visibility.Hidden;
                LblPoints.Visibility = Visibility.Hidden;

                BtnReset.Visibility = Visibility.Hidden;
                BtnDiscount.Visibility = Visibility.Hidden;

                BtnTax.Visibility = Visibility.Hidden;
                BtnOrderType.Visibility = Visibility.Hidden;

                BtnSendtoKot.Visibility = Visibility.Hidden;



                foreach (var items in root.transitems)
                {
                    bool tax1 = false;
                    if (items.tax1Status == "OK")
                    {
                        tax1 = true;
                    }
                    ItemGridModel model = new ItemGridModel
                    {

                        Itemid = items.id,
                        UniqueId = items.idkey,
                        OrderNo = items.orderId ?? 0,
                        ItemName = items.itemName,
                        Deptid = items.itemType,
                        Price = Convert.ToDecimal(items.itemPrice),
                        Quantity = Convert.ToInt32(items.qty),
                        TotalPrice = Convert.ToDecimal(items.amount),
                        isTax1 = tax1,
                        IsVoided = false,
                        IsDoubleClickable = true
                    };

                    SelectedItems.Add(model);

                }
                SelectedItems = SelectedItems.OrderBy(x => x.OrderNo).ToList();
                ItemDataGrid.Items.Refresh();
                calculatetotals();
                _viewModel.invoice = root;
                _viewModel.ShowDialog = !_viewModel.ShowDialog;
            }

        }


        private async void BtnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            promptCustomer();
        }

        private async void promptCustomer()
        {
            if (clsConnections.loyaltyactive == true)
            {
                FrmLoyaltyPrompt frmFN = new FrmLoyaltyPrompt();
                //  frmFN.EmterPH.Text = "Enter Customer Phone #";
                // frmFN.request = "custonmerphonenum";
                bool? result = frmFN.ShowDialog();
                if (result == true)
                {
                    Findcustomer(frmFN.phonenumber);

                }





            }

            else
            {
                ShowCustomMessage("This Feature Not Activated. Please Contact Support.", true);
                return;
            }
        }

        private async void Findcustomer(string pho)
        {

            string val = pho;

            var options = new RestClientOptions(clsConnections.loyaltyserver)
            {
                MaxTimeout = -1,
            };

            var client = new RestClient(options);
            var request = new RestRequest("Loyalty/findcustomer/" + clsConnections.thisstoregroupid + "/" + val, Method.Get);
            request.AddHeader("Authorization", "Basic " + clsConnections.basicauthcode);
            RestResponse response = await client.ExecuteAsync(request);
            Console.WriteLine(response.Content);

            if (response.StatusCode == HttpStatusCode.OK)

            {
                clsLoyalty.Customer myDeserializedClass = JsonConvert.DeserializeObject<clsLoyalty.Customer>(response.Content);
                //JObject json = JObject.Parse(response.Content);
                // json.GetValue("access_token").ToString();
                string cusfname = myDeserializedClass.firstName;
                string cuslname = myDeserializedClass.lastName;
                string firstletter = "";
                try
                { firstletter = cuslname.Substring(0, 1); }
                catch { }
                //SelectedcustomerID = drData["CustomerID"].ToString();
                clsLoyalty.SelectedCustomer.Selectedcustomerphone = myDeserializedClass.phoneNo;
                LblCustomerName.Text = cusfname + cuslname;
                LblPoints.Text = myDeserializedClass.loyalitypoints;
                // MessageBox.Show(myDeserializedClass.id);
                clsLoyalty.SelectedCustomer.SelectedCustomerID = myDeserializedClass.id;
                clsLoyalty.SelectedCustomer.SelectedcustomerCurrentPoints = myDeserializedClass.loyalitypoints;
                clsLoyalty.SelectedCustomer.SelectedCustomerName = cusfname + " " + firstletter + ".";

            }

            else if (response.StatusCode == HttpStatusCode.NoContent)
            {
                FrmLoyalPrompt loyalPrompt = new FrmLoyalPrompt();
                loyalPrompt.phoneno = pho;
                bool? result = loyalPrompt.ShowDialog();
                if (result == true)
                {
                    if (loyalPrompt.command == "close")
                    {

                    }

                    if (loyalPrompt.command == "retry")
                    {

                        promptCustomer();
                    }
                    if (loyalPrompt.command == "unverified")
                    {
                        Findcustomer(pho);

                    }


                }
            }
            else
            {
                MessageBox.Show("Error Code :" + response.StatusCode.ToString() + " Reason :" + response.StatusDescription);
            }
        }

        private void showoffers()
        {
            FrmOfferSelect offer = new FrmOfferSelect();
            offer.TxtOffer.Text = "Offers Available";
            var result5 = offer.ShowDialog();
            if (result5 == true)
            {
                string offerid = offer.selectedofferid;
                //MessageBox.Show(offer.selectedofferid);

                try
                {
                    var options = new RestClientOptions(clsConnections.loyaltyserver)
                    {
                        MaxTimeout = -1,
                    };

                    var client = new RestClient(options);
                    var request = new RestRequest("Offers/" + offerid, Method.Get);
                    request.AddHeader("Authorization", "Basic " + clsConnections.basicauthcode);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);



                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (var context = new PosDb1Context())
                        {
                            clsLoyalty.Offer myDeserializedClass = JsonConvert.DeserializeObject<clsLoyalty.Offer>(response.Content);
                            int customerpoints = Convert.ToInt32(clsLoyalty.SelectedCustomer.SelectedcustomerCurrentPoints);
                            int requiredpoints = Convert.ToInt32(myDeserializedClass.pointsRequired);
                            // MessageBox.Show(myDeserializedClass.ItemId);

                            // Split the comma-separated ItemIds into a list
                            var itemIds = myDeserializedClass.ItemId
                                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                            .Select(id => id.Trim())   // trim spaces
                                            .ToList();

                            // Query TblItems using the parsed ItemIds
                            var items = context.TblItems
                                .Where(item =>
                                    item.Visible == "OK" &&
                                    (item.IsModifer == false || item.IsModifer == null) &&
                                    itemIds.Contains(item.ItemId))   // filter by ItemId list
                                .OrderBy(item => item.ListOrder)
                                .ToList();
                            if (requiredpoints == customerpoints || requiredpoints < customerpoints)
                            {
                                //  offername = myDeserializedClass.description;
                                double offeramount = myDeserializedClass.radeemAmount;
                                clsLoyalty.Ammounttoradeem = Convert.ToDouble(myDeserializedClass.radeemAmount);
                                if (myDeserializedClass.radeemPointsAfterUsed == true)
                                {
                                    clsLoyalty.pointstoradeem = Convert.ToInt32(myDeserializedClass.pointsRequired);
                                    //
                                    // Modiferpage popup
                                    int ordernum = SelectedItems.Count + 1;
                                    TblItem ii = new TblItem();
                                    FrmModifier modifierWindow = new FrmModifier(items, ii, 1);
                                    if (modifierWindow.ShowDialog() == true)
                                    {
                                        foreach (var selectedModifier in modifierWindow.SelectedModifiers)
                                        {
                                            bool modifierTax1 = selectedModifier.Tax1Status == "OK";
                                            ordernum = SelectedItems.Count + 1;
                                            bool modkot1 = true;
                                            bool modkot2 = true;
                                            SelectedItems.Add(new ItemGridModel
                                            {
                                                UniqueId = Guid.NewGuid(),
                                                Itemid = selectedModifier.ItemId,
                                                ItemName = $"{selectedModifier.ItemName} ({myDeserializedClass.pointsRequired})",
                                                Deptid = selectedModifier.ItemDeptId,
                                                Price = 0,
                                                Quantity = 1,
                                                TotalPrice = 0,
                                                isTax1 = modifierTax1,
                                                OrderNo = ordernum,
                                                IsModifier = true,
                                                ParentItemId = Guid.NewGuid(),
                                                IsKOT1 = modkot1,
                                                IsKOT2 = modkot2
                                            });
                                        }
                                        ItemDataGrid.Items.Refresh();
                                        offerselected = true;
                                    }


                                    //
                                    //var amount = Convert.ToDecimal(offeramount);
                                    //decimal negativeNumber = amount * -1;
                                    //ItemGridModel g = new ItemGridModel
                                    //{
                                    //    OrderNo = 1,
                                    //    ItemName = myDeserializedClass.description,
                                    //    Itemid = "LOffer",
                                    //    Deptid = "LOffer",
                                    //    Price = negativeNumber,
                                    //    Quantity = 1,
                                    //    TotalPrice = negativeNumber,
                                    //    IsModifier = false,
                                    //    ParentItemId = Guid.NewGuid(),
                                    //    isTax1 = false,
                                    //    IsVoided = false,
                                    //    IsDoubleClickable = true,
                                    //    IsKOT1 = false,
                                    //    IsKOT2 = false

                                    //};
                                    //SelectedItems.Add(g);
                                    //ItemDataGrid.Items.Refresh();
                                    calculatetotals();
                                }



                            }

                            else
                            {
                                MessageBox.Show("Does Not Have Enough Points to use this Perk");
                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void BtnGiftCard_Click(object sender, RoutedEventArgs e)
        {
            if (clsConnections.allowgiftcard == true)
            {
                FrmGiftcard frmgift = new FrmGiftcard();

                var result4 = frmgift.ShowDialog();
                if (result4 == true)
                {
                    if (frmgift.giftoptionresponse == "GIFTBALANCE")
                    {
                        if (LoggedData.loggeduser.AllowGiftCardBalanceChk == true)
                        {
                            //GetCardNumber
                            FrmGiftPrompy cc = new FrmGiftPrompy();

                            var result3 = cc.ShowDialog();
                            if (result3 == true)
                            {
                                string cardnumber = cc.CardNumber;
                                GiftCardProccess.RequestBalanceCheck ek = new GiftCardProccess.RequestBalanceCheck();
                                ek.encrypted = EncryptionHelper.EncryptString(cardnumber);
                                var json = JsonConvert.SerializeObject(ek);


                                var options = new RestClientOptions(clsConnections.giftcardserver)
                                {
                                    Timeout = TimeSpan.FromSeconds(5),
                                };
                                var client = new RestClient(options);
                                var request = new RestRequest("/Transaction/balancecheck", Method.Post);
                                request.AddHeader("Content-Type", "application/json");
                                request.AddHeader("Authorization", "Basic " + clsConnections.giftcardserverauth);

                                request.AddParameter("application/json", json, ParameterType.RequestBody);
                                RestResponse response = client.Execute(request);


                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    FrmBalanceCheck frmFN = new FrmBalanceCheck();
                                    frmFN.content = response.Content;
                                    var result2 = frmFN.ShowDialog();
                                    if (result2 == true)
                                    {
                                    }
                                    // GiftCardProccess.ResponseBalanceCheck myDeserializedClass = JsonConvert.DeserializeObject<GiftCardProccess.ResponseBalanceCheck>(response.Content);

                                    // MessageBox.Show(myDeserializedClass.balance.ToString());
                                    // MessageBox.Show($"this page is not coded");
                                }

                                else
                                {

                                    MessageBox.Show("Error : " + response.StatusCode.ToString() + " Description :" + response.StatusDescription + " Body : " + response.Content);

                                }
                            }





                        }

                        else if (LoggedData.loggeduser.AllowGiftCardBalanceChk == false)

                        {
                            MessageBox.Show("You Don't have access to do this");
                        }
                    }



                    else if (frmgift.giftoptionresponse == "REACTIVE" || frmgift.giftoptionresponse == "DIACTIVE")
                    {
                        FrmGiftPrompy cc = new FrmGiftPrompy();

                        var result3 = cc.ShowDialog();
                        if (result3 == true)
                        {

                            string cardnumber = cc.CardNumber;
                            GiftCardProccess.RequestChangeStatus ek = new GiftCardProccess.RequestChangeStatus();
                            ek.encrypted = EncryptionHelper.EncryptString(cardnumber);
                            if (frmgift.giftoptionresponse == "REACTIVE")
                            {
                                ek.status = true;
                                ek.reason = "Unknown Custom Req";
                            }
                            else if (frmgift.giftoptionresponse == "DIACTIVE")
                            {
                                ek.status = false;
                                ek.reason = "Unknown Custom Req";

                            }

                            var json = JsonConvert.SerializeObject(ek);

                            var options = new RestClientOptions(clsConnections.giftcardserver)
                            {
                                Timeout = TimeSpan.FromSeconds(5),
                            };
                            var client = new RestClient(options);
                            var request = new RestRequest("/Transaction/changestatus", Method.Patch);
                            request.AddHeader("Content-Type", "application/json");
                            request.AddHeader("Authorization", "Basic " + clsConnections.giftcardserverauth);

                            request.AddParameter("application/json", json, ParameterType.RequestBody);
                            RestResponse response = client.Execute(request);


                            Console.WriteLine(response.Content);
                            if (response.StatusCode == HttpStatusCode.OK)
                            {

                                GiftCardProccess.ResponseChangeStatus myDeserializedClass = JsonConvert.DeserializeObject<GiftCardProccess.ResponseChangeStatus>(response.Content);
                                if (myDeserializedClass.status == true)
                                {
                                    MessageBox.Show($"Card #{myDeserializedClass.card} Reactivated Successfully");
                                }
                                else
                                {

                                    MessageBox.Show($"Card #{myDeserializedClass.card} Deactivated Successfully");
                                }



                            }

                            else
                            {

                                MessageBox.Show("Error : " + response.StatusCode.ToString() + " Description :" + response.StatusDescription + " Body : " + response.Content);

                            }
                        }

                    }

                    else if (LoggedData.loggeduser.GiftCardSale == false)


                    {
                        MessageBox.Show("You Don't have access to do this");
                    }






                    //ACTIVENEW25
                    else if (frmgift.giftoptionresponse == "ACTIVENEW")
                    {
                        if (LoggedData.loggeduser.GiftCardSale == true)
                        {

                            //TYPE CODE HERE
                            //GET AMOUNT FIRST

                            // DO GIFTSALE
                            // paxdogiftcardsale();

                            //GetCardNumber


                            FrmGiftPrompy cc = new FrmGiftPrompy();

                            var result3 = cc.ShowDialog();
                            if (result3 == true)
                            {

                                string cardnumber = cc.CardNumber;
                                GiftCardProccess.ReloadRequest ek = new GiftCardProccess.ReloadRequest();
                                ek.encrypted = EncryptionHelper.EncryptString(cardnumber);
                                ek.amount = frmgift.amount;
                                ek.FranchiseeID = Guid.Parse(LoggedData.giftcardstoreid);
                                var json = JsonConvert.SerializeObject(ek);


                                var options = new RestClientOptions(clsConnections.giftcardserver)
                                {
                                    Timeout = TimeSpan.FromSeconds(5),
                                };
                                var client = new RestClient(options);
                                var request = new RestRequest("/Transaction/sellnewcard", Method.Post);
                                request.AddHeader("Content-Type", "application/json");
                                request.AddHeader("Authorization", "Basic " + clsConnections.giftcardserverauth);

                                request.AddParameter("application/json", json, ParameterType.RequestBody);
                                RestResponse response = client.Execute(request);


                                Console.WriteLine(response.Content);
                                if (response.StatusCode == HttpStatusCode.OK)
                                {

                                    GiftCardProccess.ReloadResponse myDeserializedClass = JsonConvert.DeserializeObject<GiftCardProccess.ReloadResponse>(response.Content);

                                    MessageBox.Show($"Card #{myDeserializedClass.cardno} Activated Successfully");

                                    decimal amount = ek.amount;
                                    // decimal CurrentTax1Rate = Convert.ToDecimal(sqlC.getSingleID("SELECT [TaxRate] FROM [Tbl_TaxRates] WHERE [TaxNo] = '1' "));
                                    decimal ItemTaxAmount = 0;
                                    // decimal minustax = ItemTaxAmount * -1;

                                    //var index = dgBillItemList.Rows.Add();
                                    //dgBillItemList.Rows[index].Cells[0].Value = "defGiftSale";
                                    //dgBillItemList.Rows[index].Cells[1].Value = "defGiftSale";
                                    //dgBillItemList.Rows[index].Cells[2].Value = "GIFTCDSALE";
                                    //dgBillItemList.Rows[index].Cells[3].Value = $"Gift Card #{myDeserializedClass.cardno} Sale of ${string.Format("{0:0.00}", amount)}";
                                    //dgBillItemList.Rows[index].Cells[4].Value = amount;
                                    //dgBillItemList.Rows[index].Cells[7].Value = amount;
                                    //dgBillItemList.Rows[index].Cells[5].Value = "1";
                                    //dgBillItemList.Rows[index].Cells[6].Value = ItemTaxAmount;
                                    //dgBillItemList.Rows[index].Cells[8].Value = "NO";
                                    //dgBillItemList.Rows[index].Cells[9].Value = "NO";


                                    int ordernum = SelectedItems.Count + 1;
                                    ItemGridModel i = new ItemGridModel
                                    {
                                        UniqueId = Guid.NewGuid(),
                                        Itemid = "defGiftSale",
                                        ItemName = $"Gift Card #{myDeserializedClass.cardno} Sale of ${string.Format("{0:0.00}", amount)}",
                                        Deptid = "GIFTCDSALE",
                                        Price = amount,
                                        Quantity = 1,
                                        TotalPrice = amount,
                                        isTax1 = false,
                                        OrderNo = ordernum,
                                        IsModifier = true,
                                        ParentItemId = Guid.NewGuid()

                                    };
                                    SelectedItems.Add(i);
                                    ItemDataGrid.Items.Refresh();

                                    //String ID = sqlC.getSingleID("SELECT isnull(max(ID),0) + 1  from  [TBL_Temp_BillList]");
                                    //sqlC.ExecQuery("INSERT INTO [TBL_Temp_BillList] ([ID],[ItemID],[ItemDeptID],[ItemName],[ItemPrice],[Qty],[Tax1_Rate],[Amount]) VALUES (" + ID + ",'defdis','defdis','Discount','" + discount + "','1','" + ItemTaxAmount + "','" + discount + "') ");
                                    //DisplayBillItemList();

                                    calculatetotals();


                                }

                                else
                                {

                                    MessageBox.Show("Error : " + response.StatusCode.ToString() + " Description :" + response.StatusDescription + " Body : " + response.Content);

                                }
                            }

                        }

                        else if (LoggedData.loggeduser.GiftCardSale == false)


                        {
                            MessageBox.Show("You Don't have access to do this");
                        }


                    }

                    else if (frmgift.giftoptionresponse == "RELOAD")
                    {
                        if (LoggedData.loggeduser.GiftCardSale == true)
                        {

                            //TYPE CODE HERE
                            //GET AMOUNT FIRST

                            // DO GIFTSALE
                            // paxdogiftcardsale();

                            //GetCardNumber


                            FrmGiftPrompy cc = new FrmGiftPrompy();

                            var result3 = cc.ShowDialog();
                            if (result3 == true)
                            {

                                string cardnumber = cc.CardNumber;
                                GiftCardProccess.ReloadRequest ek = new GiftCardProccess.ReloadRequest();
                                ek.encrypted = EncryptionHelper.EncryptString(cardnumber);
                                ek.amount = frmgift.amount;
                                ek.FranchiseeID = Guid.Parse(LoggedData.giftcardstoreid);
                                var json = JsonConvert.SerializeObject(ek);

                                var options = new RestClientOptions(clsConnections.giftcardserver)
                                {
                                    Timeout = TimeSpan.FromSeconds(5),
                                };
                                var client = new RestClient(options);
                                var request = new RestRequest("/Transaction/reload", Method.Post);
                                request.AddHeader("Content-Type", "application/json");
                                request.AddHeader("Authorization", "Basic " + clsConnections.giftcardserverauth);

                                request.AddParameter("application/json", json, ParameterType.RequestBody);
                                RestResponse response = client.Execute(request);


                                Console.WriteLine(response.Content);
                                if (response.StatusCode == HttpStatusCode.OK)
                                {

                                    GiftCardProccess.ReloadResponse myDeserializedClass = JsonConvert.DeserializeObject<GiftCardProccess.ReloadResponse>(response.Content);

                                    MessageBox.Show($"Card #{myDeserializedClass.cardno} Reloaded Successfully");

                                    decimal amount = ek.amount;
                                    // decimal CurrentTax1Rate = Convert.ToDecimal(sqlC.getSingleID("SELECT [TaxRate] FROM [Tbl_TaxRates] WHERE [TaxNo] = '1' "));
                                    decimal ItemTaxAmount = 0;
                                    // decimal minustax = ItemTaxAmount * -1;

                                    //var index = dgBillItemList.Rows.Add();
                                    //dgBillItemList.Rows[index].Cells[0].Value = "defGiftSale";
                                    //dgBillItemList.Rows[index].Cells[1].Value = "defGiftSale";
                                    //dgBillItemList.Rows[index].Cells[2].Value = "GIFTCDSALE";
                                    //dgBillItemList.Rows[index].Cells[3].Value = $"Gift Card #{myDeserializedClass.cardno} Reload of ${string.Format("{0:0.00}", amount)}";
                                    //dgBillItemList.Rows[index].Cells[4].Value = amount;
                                    //dgBillItemList.Rows[index].Cells[7].Value = amount;
                                    //dgBillItemList.Rows[index].Cells[5].Value = "1";
                                    //dgBillItemList.Rows[index].Cells[6].Value = ItemTaxAmount;
                                    //dgBillItemList.Rows[index].Cells[8].Value = "NO";
                                    //dgBillItemList.Rows[index].Cells[9].Value = "NO";

                                    int ordernum = SelectedItems.Count + 1;
                                    ItemGridModel i = new ItemGridModel
                                    {
                                        UniqueId = Guid.NewGuid(),
                                        Itemid = "defGiftSale",
                                        ItemName = $"Gift Card #{myDeserializedClass.cardno} Reload of ${string.Format("{0:0.00}", amount)}",
                                        Deptid = "GIFTCDSALE",
                                        Price = amount,
                                        Quantity = 1,
                                        TotalPrice = amount,
                                        isTax1 = false,
                                        OrderNo = ordernum,
                                        IsModifier = true,
                                        ParentItemId = Guid.NewGuid()

                                    };
                                    SelectedItems.Add(i);
                                    ItemDataGrid.Items.Refresh();

                                    //String ID = sqlC.getSingleID("SELECT isnull(max(ID),0) + 1  from  [TBL_Temp_BillList]");
                                    //sqlC.ExecQuery("INSERT INTO [TBL_Temp_BillList] ([ID],[ItemID],[ItemDeptID],[ItemName],[ItemPrice],[Qty],[Tax1_Rate],[Amount]) VALUES (" + ID + ",'defdis','defdis','Discount','" + discount + "','1','" + ItemTaxAmount + "','" + discount + "') ");
                                    //DisplayBillItemList();

                                    calculatetotals();


                                }

                                else
                                {

                                    MessageBox.Show("Error : " + response.StatusCode.ToString() + " Description :" + response.StatusDescription + " Body : " + response.Content);

                                }
                            }

                        }

                        else if (LoggedData.loggeduser.GiftCardSale == false)

                        {
                            MessageBox.Show("You Don't have access to do this");
                        }


                    }
                }
            }

            else
            {
                MessageBox.Show("This feature is not Activated in your system. Please contact the support.");

            }
        }

        private void BtnDiscount_Click(object sender, RoutedEventArgs e)
        {
            bool hasPermission = LoggedData.loggeduser.Allowdiscount == true;

            // If user doesn't have permission, open override prompt
            if (!hasPermission)
            {
                FrmPermissionShow frmPermissionShow = new FrmPermissionShow();
                frmPermissionShow.reqpermssion = "prmallowdiscount";

                hasPermission = frmPermissionShow.ShowDialog() == true;
            }

            if (!hasPermission)
                return;

            if (DiscountApplied)
            {

                DiscountApplied = false;
                AppliedDiscountAmount = 0;
                IsPercentageDiscount = false;


                NotificationBorder.Visibility = Visibility.Visible;
                LblNotification.Content = "Discount Removed";
                LblNotification.Foreground = Brushes.Red;
                NotificationBorder.Background = Brushes.LightCoral;
                NotificationBorder.BorderBrush = Brushes.DarkRed;
            }
            else
            {

                FrmNumberpad numberpad = new FrmNumberpad();
                if (numberpad.ShowDialog() == true)
                {
                    decimal enteredValue = numberpad.EnteredValue;
                    bool isPercentage = numberpad.IsPercentage;

                    if (isPercentage && (enteredValue < 0 || enteredValue > 100))
                    {
                        FrmCustommessage customMessageBox = new FrmCustommessage
                        {
                            LblHeader = { Text = "Warning" },
                            LblMessage = { Text = "Invalid Discount!\nDiscount percentage must be between 0 and 100." },
                            IsError = true
                        };
                        customMessageBox.ShowDialog();
                        return;
                    }

                    DiscountApplied = true;
                    AppliedDiscountAmount = enteredValue;
                    IsPercentageDiscount = isPercentage;


                    NotificationBorder.Visibility = Visibility.Visible;
                    LblNotification.Content = "Discount Applied";
                    LblNotification.Foreground = Brushes.Green;
                    NotificationBorder.Background = Brushes.LightGreen;
                    NotificationBorder.BorderBrush = Brushes.DarkGreen;
                }
            }

            calculatetotals();

            if (DiscountApplied)
            {
                if (IsPercentageDiscount)
                {
                    DiscountTxt.Text = $"({AppliedDiscountAmount:0.##}%)";
                }
                else
                {
                    DiscountTxt.Text = string.Empty;
                }
            }
            else
            {
                DiscountTxt.Text = string.Empty;
            }

            HideNotification();
        }

        private async void HideNotification()
        {
            await Task.Delay(2000);
            NotificationBorder.Visibility = Visibility.Collapsed;
        }







        private void BtnPay_Click_1(object sender, RoutedEventArgs e)
        {

        }
        private void UpdatePayButtonState()
        {
            if (BtnPay != null)
            {
                // Parse the total amount from LblTotal
                string totalText = LblTotal.Text.Replace("$", "").Replace(",", "").Trim();

                if (decimal.TryParse(totalText, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"), out decimal totalAmount))
                {
                    // Enable Pay button for both positive amounts (sales) and negative amounts (returns)
                    BtnPay.IsEnabled = totalAmount != 0;
                }
                else
                {
                    BtnPay.IsEnabled = false;
                }
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            resetscreen();
        }

        public static async Task ResetPacScreenAsync()
        {
            Task.Run(() => clsPAXCD.ClearScreen());
        }


        private void BtnEndview_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ShowDialog = !_viewModel.ShowDialog;
            resetscreen();
        }

        private void BtnReprint_Click(object sender, RoutedEventArgs e)
        {

            CurrentTransData.MainData = _viewModel.invoice.transmain;
            CurrentTransData.transitems = _viewModel.invoice.transitems;

            PrintService.PrinterReceipt(out bool isSuccessful);
            _viewModel.ShowDialog = !_viewModel.ShowDialog;
            resetscreen();
        }

        private async void BtnVoid_Click(object sender, RoutedEventArgs e)
        {
            //_viewModel.ShowDialog = !_viewModel.ShowDialog;
            //resetscreen();

            if (_viewModel.invoice.transmain.saleDateTime.Date == DateTime.Now.Date)
            {
                if (_viewModel.invoice.transmain.cardAmount > 0)
                {
                    if (LoggedData.comtype == "PAX_IP" || LoggedData.comtype == "PAX_USB")
                    {
                        FrmPaxscreen pax = new FrmPaxscreen();
                        pax.comtype = LoggedData.comtype;
                        if (LoggedData.comtype == "PAX_IP")
                        {
                            pax.ipaddr = LoggedData.PaxIP;
                            pax.portnum = LoggedData.PaxPort;
                        }

                        else if (LoggedData.comtype == "PAX_USB")
                        {
                            pax.comport = LoggedData.PaxComPort;
                            pax.bautrate = LoggedData.PaxBaudRate;
                        }

                        pax.transtyperequest = "VOID";
                        pax.ecrref = _viewModel.invoice.transmain.invoiceId.ToString();
                        pax.Device_Org_Ref_Num = _viewModel.invoice.transmain.deviceOrgRefNum;
                        var result = pax.ShowDialog();

                        if (result == true)
                        {
                            if (pax.responsecode == "000000")
                            {


                                await processvoid();
                            }

                            else

                            {
                                MessageBox.Show(pax.responseMsg, pax.responsecode);
                            }
                        }
                    }



                }

                else
                {
                    await processvoid();
                }
            }

            else
            {
                MessageBox.Show("only same day transactions can be voided");
            }
        }

        private async Task processvoid()
        {

            var client = new RestClient(LoggedData.transbaseurl);
            var request = new RestRequest("/Transactions/voidtran/" + _viewModel.invoice.transmain.invoiceId.ToString(), Method.Patch);
            request.AddHeader("db", clsConnections.mydb);
            request.AddHeader("Authorization", clsConnections.transserverauth);
            RestResponse response = await client.ExecuteAsync(request);
            Console.WriteLine(response.Content);
            if (response.IsSuccessful)
            {
                if (_viewModel.invoice.transmain.cashAmount > 0)
                {
                    CustomMessageBox.Show("Cash Return", $"{_viewModel.invoice.transmain.cashAmount.ToString("C")} should be returned to the customer in cash.",
                        false
                );

                    if (_viewModel.invoice.transmain.paidby == "CASH")
                    {
                        PrintService.OpenCashDrawer();
                    }
                }

                CurrentTransData.MainData = _viewModel.invoice.transmain;
                CurrentTransData.transitems = _viewModel.invoice.transitems;
                CurrentTransData.MainData.transType = "VOID";
                PrintService.PrinterReceipt(out bool isSuccessful);

                _viewModel.ShowDialog = !_viewModel.ShowDialog;
                resetscreen();
            }

            else
            {
                if (Properties.Settings.Default.DubugErrors)
                {
                    CustomMessageBox.Show(
                        "Void Failed",
                        $"Error on Transaction Server:\nStatus: {response.ResponseStatus}\nContent: {response.Content}",
                        true
                    );
                }
                else
                {
                    CustomMessageBox.Show(
                        "Void Failed",
                        "Could not void the transaction. Please try again.",
                        true
                    );
                }
            }
        }

        private async void BtnRefund_Click(object sender, RoutedEventArgs e)
        {

            if (_viewModel.invoice.transmain.paidby == "CASH")
            {
                SelectedItems.Clear();
                CurrentTransData.transitems = _viewModel.invoice.transitems;
                CurrentTransData.MainData = _viewModel.invoice.transmain;
                await SaveBill("RETURN");
                _viewModel.ShowDialog = !_viewModel.ShowDialog;
            }

            else
            {
                FrmPaxscreen pax = new FrmPaxscreen();

                if (LoggedData.comtype == "PAX_IP" || LoggedData.comtype == "PAX_USB")
                {
                    string returnamount = _viewModel.invoice.transmain.cardAmount.ToString("F2");
                    decimal returnedtip = 0;
                    if (_viewModel.invoice.transmain.tipAmount > 0)
                    {
                        double newreturn = _viewModel.invoice.transmain.cardAmount + _viewModel.invoice.transmain.tipAmount;
                        returnamount = newreturn.ToString();
                        decimal tip = Convert.ToDecimal(_viewModel.invoice.transmain.tipAmount);
                        returnedtip = tip * -1;


                    }
                    pax.amount = returnamount;
                    pax.ecrref = _viewModel.invoice.transmain.invoiceId.ToString();
                    pax.transtyperequest = "RETURN";
                    //COM DETAILS
                    pax.comtype = LoggedData.comtype;
                    if (LoggedData.comtype == "PAX_IP")
                    {
                        pax.ipaddr = LoggedData.PaxIP;
                        pax.portnum = LoggedData.PaxPort;
                    }

                    else if (LoggedData.comtype == "PAX_USB")

                    {
                        pax.comport = LoggedData.PaxComPort;
                        pax.bautrate = LoggedData.PaxBaudRate;
                    }

                    var result = pax.ShowDialog();
                    if (result == true)
                    {
                        if (pax.responsecode == "000000")
                        {
                            SelectedItems.Clear();
                            CurrentTransData.transitems = _viewModel.invoice.transitems;
                            CurrentTransData.MainData = _viewModel.invoice.transmain;
                            await SaveBill("RETURN");
                            _viewModel.ShowDialog = !_viewModel.ShowDialog;

                        }

                        else
                        {

                            MessageBox.Show(pax.responseMsg, pax.responsecode);
                        }
                    }

                    else

                    {

                        MessageBox.Show(pax.responseMsg, pax.responsecode);
                    }
                }

            }
        }

        private async void BtnPrintLast_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await ReopenLastReceiptAsync();
            }
            catch (Exception ex)
            {
                if (Properties.Settings.Default.DubugErrors)
                {
                    CustomMessageBox.Show(
                        "Unexpected Error",
                        $"An unexpected error occurred:\n{ex.Message}",
                        true
                    );
                }
                else
                {
                    CustomMessageBox.Show(
                        "Error",
                        "Something went wrong while trying to print the last receipt. Please try again.",
                        true
                    );
                }
            }
        }

        private async Task ReopenLastReceiptAsync()
        {
            try
            {
                var options = new RestClientOptions(LoggedData.transbaseurl)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/OtherTransactions/lastinvoice", Method.Get);
                request.AddHeader("db", clsConnections.mydb);
                request.AddHeader("Authorization", clsConnections.transserverauth);

                RestResponse response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    TransData.Root root = new TransData.Root();
                    root = JsonConvert.DeserializeObject<TransData.Root>(response.Content);
                    CurrentTransData.MainData = root.transmain;
                    CurrentTransData.transitems = root.transitems.OrderBy(x => x.orderId).ToList();
                    PrintService.PrinterReceipt(out bool isSuccessful);
                    resetscreen();

                }
                else
                {
                    CustomMessageBox.Show(
                        "Failed to Fetch",
                        $"Could not load the last invoice.\nError Code: {response.StatusCode}\nReason: {response.StatusDescription}",
                        true
                    );
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(
                    "Unexpected Error",
                    $"An error occurred while reopening the last receipt:\n{ex.Message}",
                    true
                );
            }
        }

        private async void BtnHold_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItems.Count == 0)
            {
                CustomMessageBox.Show(
                    "Hold Not Allowed",
                    "Please add at least one item before putting the bill on hold.",
                    true
                );
                return;
            }

            var keyb = new FrmKeyboard();
            var result = keyb.ShowDialog();

            if (result == true)
            {
                CurrentTransData.MainData.holdName = keyb.returnvalue;
                await SaveBill("HOLD");
                resetscreen();
            }
        }

        private async Task UpdateCustomButtons()
        {
            Btn1.Tag = Properties.Settings.Default.Btn1Tag;
            Btn2.Tag = Properties.Settings.Default.Btn2Tag;
            Btn3.Tag = Properties.Settings.Default.Btn3Tag;

            UpdateButtonUI(Btn1, Btn1.Tag.ToString());
            UpdateButtonUI(Btn2, Btn2.Tag.ToString());
            UpdateButtonUI(Btn3, Btn3.Tag.ToString());
        }


        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            string input = Btn1.Tag?.ToString();

            if (string.IsNullOrEmpty(input) || input == "NOTSET")
            {
                NavigateToHomePage();
                return;
            }

            using (var context = new PosDb1Context())
            {
                string[] parts = input.Split(':');
                string part1 = parts[0];
                string part2 = parts.Length > 1 ? parts[1] : "";

                if (part1 == "XITEM")
                {
                    var item = context.TblItems.FirstOrDefault(x => x.ItemId == part2);
                    if (item == null)
                    {
                        MessageBox.Show("Item Not Found");
                        return;
                    }

                    getitem(item);
                }
                else if (part1 == "XDEPT")
                {
                    var dept = context.TblDepartments.FirstOrDefault(x => x.DeptId == part2);
                    if (dept == null)
                    {
                        MessageBox.Show("Department Not Found");
                        return;
                    }

                    LoadItemsForDepartment(dept);
                }
                else
                {
                    NavigateToHomePage();
                }
            }
        }


        private void Btn2_Click(object sender, RoutedEventArgs e)
        {

            string input = Btn2.Tag?.ToString();

            if (string.IsNullOrEmpty(input) || input == "NOTSET")
            {
                NavigateToHomePage();
                return;
            }

            using (var context = new PosDb1Context())
            {

                string[] parts = input.Split(':');
                string part1 = parts[0];
                string part2 = parts[1];

                if (part1 == "XITEM")
                {
                    var item = context.TblItems.Where(x => x.ItemId == part2).FirstOrDefault();
                    if (item == null)
                    {
                        MessageBox.Show("Item Not Found");

                    }

                    getitem(item);
                }

                else if (part1 == "XDEPT")
                {
                    var dept = context.TblDepartments.Where(x => x.DeptId == part2).FirstOrDefault();
                    if (dept == null)
                    {
                        MessageBox.Show("Department Not Found");
                    }
                    LoadItemsForDepartment(dept);
                }
                else
                {
                    NavigateToHomePage();
                }
            }
        }

        private void Btn3_Click(object sender, RoutedEventArgs e)
        {
            string input = Btn3.Tag?.ToString();

            if (string.IsNullOrEmpty(input) || input == "NOTSET")
            {
                NavigateToHomePage();
                return;
            }

            using (var context = new PosDb1Context())
            {

                string[] parts = input.Split(':');
                string part1 = parts[0];
                string part2 = parts[1];

                if (part1 == "XITEM")
                {
                    var item = context.TblItems.Where(x => x.ItemId == part2).FirstOrDefault();
                    if (item == null)
                    {
                        MessageBox.Show("Item Not Found");
                    }

                    getitem(item);
                }

                else if (part1 == "XDEPT")
                {
                    var dept = context.TblDepartments.Where(x => x.DeptId == part2).FirstOrDefault();
                    if (dept == null)
                    {
                        MessageBox.Show("Department Not Found");
                    }
                    LoadItemsForDepartment(dept);
                }
                else
                {
                    NavigateToHomePage();
                }
            }

        }

        private string GetButtonDisplayName(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                return "Not Set";

            string[] parts = tag.Split(':');
            if (parts.Length < 2)
                return "Invalid";

            string type = parts[0];
            string value = parts[1];

            using (var context = new PosDb1Context())
            {
                if (type == "XITEM")
                {
                    var item = context.TblItems.FirstOrDefault(x => x.ItemId == value);
                    return item?.ItemName ?? "Item Not Found";
                }
                else if (type == "XDEPT")
                {
                    var dept = context.TblDepartments.FirstOrDefault(x => x.DeptId == value);
                    return dept?.DeptName ?? "Dept Not Found";
                }
            }

            return "Unknown";
        }

        private void UpdateButtonUI(Button button, string tag)
        {
            string name = GetButtonDisplayName(tag);
            var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
            var icon = new PackIconMaterial
            {
                Width = 30,
                Height = 40,
                Margin = new Thickness(20, 0, 0, 0),
                Kind = tag.StartsWith("XITEM") ? PackIconMaterialKind.Food : PackIconMaterialKind.FoodForkDrink
            };

            var text = new TextBlock
            {
                Text = name,
                VerticalAlignment = VerticalAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                Width = 110,
                Margin = new Thickness(20, 0, 0, 0)
            };
            stackPanel.Children.Add(icon);
            stackPanel.Children.Add(text);
            button.Content = stackPanel;
        }

        private void Btn4_Click(object sender, RoutedEventArgs e)
        {
            FrmMoreOptions frmMore = new FrmMoreOptions();
            var res = frmMore.ShowDialog();
            if (res == true)
            {
                if (frmMore.resp == "manualreturn")
                {
                    FrmKeyboard frmKeyboard = new FrmKeyboard();
                    var result = frmKeyboard.ShowDialog();

                    if (result == true)
                    {
                        var reason = frmKeyboard.returnvalue;

                        FrmNumbpad frmnumb = new FrmNumbpad();
                        var result2 = frmnumb.ShowDialog();
                        if (result2 == true)
                        {
                            var amount = Convert.ToDecimal(frmnumb.returnvalue);
                            decimal negativeNumber = amount * -1;

                            ItemGridModel g = new ItemGridModel
                            {
                                OrderNo = SelectedItems.Count + 1,
                                ItemName = "Manual Return -" + reason,
                                Itemid = "BRETURN",
                                Deptid = "BRETURN",
                                Price = negativeNumber,
                                Quantity = 1,
                                TotalPrice = negativeNumber,
                                IsModifier = false,
                                ParentItemId = Guid.NewGuid(),
                                isTax1 = false,
                                IsVoided = false,
                                IsDoubleClickable = true,
                                IsKOT1 = false,
                                IsKOT2 = false

                            };
                            SelectedItems.Add(g);
                            ItemDataGrid.Items.Refresh();
                            calculatetotals();

                        }
                        else
                        {
                            CustomMessageBox.Show(
                                "Invalid Amount",
                                "Please enter a valid return amount greater than 0.",
                                true
                                );
                        }

                    }
                }

            }
        }

        private TblItem GetItemByBarcode(string barcode)
        {
            using (var context = new PosDb1Context())
            {
                return context.TblItems.FirstOrDefault(item => item.ItemId == barcode);
            }
        }

        private void TxtScan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string barcode = TxtScan.Text.Trim();
                TxtScan.Clear();

                if (string.IsNullOrEmpty(barcode))
                    return;

                TblItem scannedItem = GetItemByBarcode(barcode);
                if (scannedItem != null)
                {
                    getitem(scannedItem);
                }
                else
                {
                    CustomMessageBox.Show(
                        "Item Not Found",
                        $"No item found for barcode: {barcode}",
                        true
                    );
                }
            }
        }

        private void TxtScan_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(Keyboard.FocusedElement is Button))
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    TxtScan.Focus();
                }), System.Windows.Threading.DispatcherPriority.Input);
            }
        }

        private void BtnSendtoKot_Click(object sender, RoutedEventArgs e)
        {
            var itemswithkot = SelectedItems.Where(x => x.IsKOT1 == true).ToList();

            if (itemswithkot != null)
            {
                PrintService.PrintertoKitchen(SelectedItems);

                if (Properties.Settings.Default.IsKeepKOTEnabled)
                {
                    resetscreen();
                }

            }

        }

        private decimal DineInTaxRate = 0;
        private decimal TakeAwayTaxRate = 0;
        private decimal CurrentTaxRate = 0;

        private void LoadTaxRates()
        {
            try
            {
                using (var context = new PosDb1Context())
                {
                    var takeAwayTax = context.TblTaxRates.FirstOrDefault(t => t.TaxNo == 1);
                    if (takeAwayTax != null)
                        TakeAwayTaxRate = takeAwayTax.TaxRate ?? 0;

                    var dineInTax = context.TblTaxRates.FirstOrDefault(t => t.TaxNo == 2);
                    if (dineInTax != null)
                        DineInTaxRate = dineInTax.TaxRate ?? 0;
                }


                CurrentTaxRate = TakeAwayTaxRate;
                Tax1 = CurrentTaxRate;
                RunTaxRate.Text = $"({CurrentTaxRate:0.##}%)";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading tax rates: {ex.Message}");
            }
        }




        private void BtnOrderType_Click(object sender, RoutedEventArgs e)
        {
            if (txtOrderType.Text == "Take Away")
            {
                CurrentTaxRate = DineInTaxRate;
                Tax1 = CurrentTaxRate;
                calculatetotals();
                txtOrderType.Text = "Dine-In";
                BtnOrderTypeText.Text = "Take Away";
                txtOrderTypeIcon.Kind = PackIconMaterialKind.SilverwareForkKnife;
                RunTaxRate.Text = $"({CurrentTaxRate:0.##}%)";
            }
            else
            {
                CurrentTaxRate = TakeAwayTaxRate;
                Tax1 = CurrentTaxRate;
                calculatetotals();
                txtOrderType.Text = "Take Away";
                BtnOrderTypeText.Text = "Dine-In";
                txtOrderTypeIcon.Kind = PackIconMaterialKind.BikeFast;
                RunTaxRate.Text = $"({CurrentTaxRate:0.##}%)";
            }
        }

        private void NavigateToHomePage()
        {
            // Example: if your home page is FrmPosMain
            FrmPosMain home = new FrmPosMain();
            home.Show();
            this.Close(); // optional: close current window if needed
        }







    }

}
