using Pospointe.LocalData;
using Pospointe.MainMenu;
using Pospointe.Models;
using Pospointe.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Pospointe
{
    /// <summary>
    /// Interaction logic for FrmOverrideUsers.xaml
    /// </summary>
    public partial class FrmOverrideUsers : Window
    {
        public string requestedpermssion { get; set; }
        public List<TblUser> localusers = new List<TblUser>();
        public FrmOverrideUsers()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AddButtonsToGrid();
        }

        private void AddButtonsToGrid()
        {
            WrpOverride.Children.Clear(); 

            var users = GetUserswithPermission();
            if (users != null && users.Count > 0)
            {
                localusers = users;
                int totalUsers = users.Count;

                // Use a WrapPanel instead of UniformGrid
                WrapPanel wrapPanel = new WrapPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center, // Center align dynamically
                    VerticalAlignment = VerticalAlignment.Center
                };

                foreach (var user in users)
                {
                    Button newButton = new Button
                    {
                        Padding = new Thickness(10),
                        Margin = new Thickness(10),
                        MinHeight = 190,
                        Style = (Style)Application.Current.Resources["StyledUser"],
                        Tag = user
                    };

                    StackPanel stackPanel = new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };

                    Image icon = new Image
                    {
                        Width = 250,
                        Height = 120,
                        Margin = new Thickness(0, 0, 0, 10)
                    };

                    try
                    {
                        if (!string.IsNullOrEmpty(user.UserPicturePath))
                        {
                            BitmapImage userImage = new BitmapImage();
                            userImage.BeginInit();
                            userImage.UriSource = new Uri(user.UserPicturePath, UriKind.RelativeOrAbsolute);
                            userImage.CacheOption = BitmapCacheOption.OnLoad;
                            userImage.EndInit();
                            icon.Source = userImage;
                        }
                        else
                        {
                            icon.Source = new BitmapImage(new Uri("https://i.postimg.cc/sgnNqK12/cashier.png"));
                        }
                    }
                    catch
                    {
                        icon.Source = new BitmapImage(new Uri("https://i.postimg.cc/sgnNqK12/cashier.png"));
                    }

                    TextBlock textBlock = new TextBlock
                    {
                        Text = user.UserName,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        TextWrapping = TextWrapping.Wrap,
                        TextAlignment = TextAlignment.Center,
                        FontSize = 16,
                        Margin = new Thickness(0, 5, 0, 0)
                    };

                    stackPanel.Children.Add(icon);
                    stackPanel.Children.Add(textBlock);
                    newButton.Content = stackPanel;
                    newButton.Click += Button_Click;

                    wrapPanel.Children.Add(newButton); // Add button to WrapPanel
                }

                WrpOverride.Children.Add(wrapPanel); // Add WrapPanel to UniformGrid
            }
        }



        private List<TblUser> GetUserswithPermission()
        {

            using (var context = new PosDb1Context())
            {
                if (requestedpermssion == "prmpos")
                {
                   return context.TblUsers.Where(user => user.UserPos == "OK"  && user.UserStatus == "OK").ToList();
                }

                if (requestedpermssion == "prmbackend")
                {
                    return context.TblUsers.Where(user => user.UserBackEnd == "OK" && user.UserStatus == "OK").ToList();
                }

                if (requestedpermssion == "prmendday")
                {
                    return context.TblUsers.Where(user => user.UserEndDayPeform == "OK" && user.UserStatus == "OK").ToList();
                }

                if (requestedpermssion == "prmdashboard")
                {
                    return context.TblUsers.Where(user => user.UserDashBoard == "OK" && user.UserStatus == "OK").ToList();
                }

                if (requestedpermssion == "prmenddayforce")
                {
                    return context.TblUsers.Where(user => user.PerformEnddayForced == true && user.UserStatus == "OK").ToList();
                }

                if (requestedpermssion == "prmreqsupport")
                {
                    return context.TblUsers.Where(user => user.RequestSupport == true && user.UserStatus == "OK").ToList();
                }

                if (requestedpermssion == "prmlogout")
                {
                    return context.TblUsers.Where(user => user.LogtOut == true && user.UserStatus == "OK").ToList();
                }

                if (requestedpermssion == "prmrecallinvoice")
                {
                    return context.TblUsers.Where(user => user.RecallInvoice == true && user.UserStatus == "OK").ToList();
                }

                if (requestedpermssion == "prmrecalloldinvoice")
                {
                    return context.TblUsers.Where(user => user.RecallOldInvoice == true && user.UserStatus == "OK").ToList();
                }

                if (requestedpermssion == "prmrecallvoidtrans")
                {
                    return context.TblUsers.Where(user => user.VoidTrans == true && user.UserStatus == "OK").ToList();
                }

                if (requestedpermssion == "prmrecallreturntrans")
                {
                    return context.TblUsers.Where(user => user.RetrnTrans == true && user.UserStatus == "OK").ToList();
                }

                if (requestedpermssion == "prmcustomermanagement")
                {
                    return context.TblUsers.Where(user => user.CustomerManagement == true && user.UserStatus == "OK").ToList();
                }

                if (requestedpermssion == "prmcreditcardsale")
                {
                    return context.TblUsers.Where(user => user.CreditCardSale == true && user.UserStatus == "OK").ToList();
                }

                if (requestedpermssion == "prmcashsale")
                {
                    return context.TblUsers.Where(user => user.CashSale == true && user.UserStatus == "OK").ToList();
                }

                if (requestedpermssion == "prmgiftcardsale")
                {
                    return context.TblUsers.Where(user => user.GiftCardSale == true && user.UserStatus == "OK").ToList();
                }

                if (requestedpermssion == "prmallowpricechange")
                {
                    return context.TblUsers.Where(user => user.Allowpricechange == true && user.UserStatus == "OK").ToList();
                }

                if (requestedpermssion == "prmallowdiscount")
                {
                    return context.TblUsers.Where(user => user.Allowdiscount == true && user.UserStatus == "OK").ToList();
                }

                if (requestedpermssion == "prmreturn")
                {
                    return context.TblUsers.Where(user => user.AllowReturns == true && user.UserStatus == "OK").ToList();
                }

                if (requestedpermssion == "prmcashdropen")
                {
                    return context.TblUsers.Where(user => user.AllowCashDrawerOpen == true && user.UserStatus == "OK").ToList();
                }

                if (requestedpermssion == "prmgiftcardblchk")
                {
                    return context.TblUsers.Where(user => user.AllowGiftCardBalanceChk == true && user.UserStatus == "OK").ToList();
                }

                if (requestedpermssion == "prmallownontaxsale")
                {
                    return context.TblUsers.Where(user => user.AllowNonTaxSales == true && user.UserStatus == "OK").ToList();
                }

                return null;



            }



        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            var user = btn.Tag as TblUser;
            if (user != null)
            {
                PinPad pinPad = new PinPad(user.UserName, user.UserPicturePath);
                var result = pinPad.ShowDialog();
                if (result == true)
                {
                    Login(user.UserId, pinPad.returnvalue);
                }
            }
        }

        private async Task Login(string userid, string password)
        {
            string encryptedpass = CLSencryption.Encrypt(password);

            var user = localusers.Where(x => x.UserId == userid && x.UserPin == encryptedpass).FirstOrDefault();
            if (user == null)
            {
                FrmCustommessage frmCustommessage = new FrmCustommessage();
                frmCustommessage.LblMessage.Text = "Invalid Password";
                frmCustommessage.IsError = true;
                frmCustommessage.ShowDialog();
            }

            else
            {
                //LoggedData.loggeduser = user;
                //FrmMainmenu frmMainmenu = new FrmMainmenu();
                //frmMainmenu.ShowDialog();
                this.DialogResult = true;
                this.Close();
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
                this.Close();
        }
    }
}
