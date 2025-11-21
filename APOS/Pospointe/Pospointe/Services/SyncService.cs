using Newtonsoft.Json;
using Pospointe.LocalData;
using Pospointe.Models;
using Pospointe.Trans_Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pospointe.Services
{
    public class SyncService
    {


        public static async Task SyncTransTOCLoud()
        {
            bool status = SyncService.IsOnline();

            if (status)
            {

                using (var context = new PosDb1Context())

                {
                    var offlinetrans = context.TblTransMains.Where(x => x.TransType != "HOLD").ToList();
                    if (offlinetrans.Count > 0)
                    {
                        foreach (var offlinet in offlinetrans)
                        {
                            List<TblTransSub> SubITems = context.TblTransSubs.Where(x => x.TransMainId == offlinet.InvoiceId).ToList();

                            TransData.Root data = SyncService.ConverttoCloudData(offlinet, SubITems);
                            bool success = await SyncService.MoveBilltoCloud(data);
                            if (success)
                            {
                                context.TblTransMains.Remove(offlinet);
                                context.TblTransSubs.RemoveRange(SubITems);
                                await context.SaveChangesAsync();
                            }
                            if (!success)
                            {
                                //FrmYesNoMessage mm = new FrmYesNoMessage("Error" , "");
                              // mm.ShowDialog();
                                break;
                            }

                        }


                    }

                }


            }

        }








        public static bool IsOnline()
        {
            var options = new RestClientOptions(clsConnections.transserverprimaryurl)
            {
                Timeout = TimeSpan.FromSeconds(3),
            };
            var client = new RestClient(options);
            var request = new RestRequest("/Others/status", Method.Get);
            request.AddHeader("Authorization", clsConnections.transserverauth);
            RestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

            if (response.IsSuccessful)
            {
                return true;
            }

            else
            {
                return false;
            }

        }


        public static async Task<bool> MoveBilltoCloud(TransData.Root data)
        {

            var json = JsonConvert.SerializeObject(data);

            var options = new RestClientOptions(LoggedData.transbaseurl)
            {
                Timeout = TimeSpan.FromSeconds(2),
            };
            var client = new RestClient(options);
            var request = new RestRequest("/Transactions", Method.Post);
            request.AddHeader("db", clsConnections.mydb);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", clsConnections.transserverauth);

            request.AddParameter("application/json", json, ParameterType.RequestBody);
            RestResponse response = await client.ExecuteAsync(request);
            Console.WriteLine(response.Content);

            if (response.IsSuccessful)
            {
                //FrmCustommessage msg = new FrmCustommessage();
                ////msg.LblHeader.Content = "Success";
                //msg.LblMessage.Text = "Trans Saved Successfully";
                ////msg.ShowDialog();
                return true;
            }
            else
            {
                //FrmCustommessage msg = new FrmCustommessage();
                //msg.IsError = true;
                //msg.LblHeader.Text = "Error";
                //msg.LblMessage.Text = $"{response.StatusCode} and Body {response.Content}";
                //msg.ShowDialog();
                return false;
            }

        }

        public static TransData.Root ConverttoCloudData(TblTransMain main , List<TblTransSub> items)
        {
            TransData.Root root = new TransData.Root();

            //TRANS MAIN DATA
            TransData.Transmain transmain = new TransData.Transmain();
            transmain.invoiceId = main.InvoiceId;
            transmain.transType = main.TransType;
            transmain.subtotal = Convert.ToDouble(main.Subtotal);
            transmain.tax1 = Convert.ToDouble(main.Tax1);
            transmain.grandTotal = Convert.ToDouble(main.GrandTotal);
            transmain.saleDateTime = main.SaleDateTime;
            transmain.cashAmount = Convert.ToDouble(main.CashAmount);
            transmain.cardAmount = Convert.ToDouble(main.CardAmount);
            transmain.stationId = main.StationId;
            transmain.cashierId = main.CashierId;
            transmain.cashChangeAmount = Convert.ToDouble(main.CashChangeAmount);
            transmain.paidby = main.Paidby;
            transmain.retref = main.Retref;
            transmain.cardType = main.CardType;
            transmain.cardHolder = main.CardHolder;
            transmain.invoiceDiscount = Convert.ToDouble(main.InvoiceDiscount);
            transmain.phoneNo = main.PhoneNo;
            transmain.entryMethod = main.EntryMethod;
            transmain.accountType = main.AccountType;
            transmain.aid = main.Aid;
            transmain.tcarqc = main.Tcarqc;
            transmain.href = main.Href;
            transmain.hostRefNum = main.HostRefNum;
            transmain.deviceOrgRefNum = main.DeviceOrgRefNum;
            transmain.customerId = main.CustomerId;
            transmain.totalCredit = Convert.ToDouble(main.TotalCredit);
            transmain.tipAmount = Convert.ToDouble(main.TipAmount);
            transmain.giftCardNumber = main.GiftCardNumber;
            transmain.checkNumber = main.CheckNumber;
            transmain.holdName = main.HoldName;
            transmain.customerName = main.CustomerName;
            transmain.loyaltyDiscount = Convert.ToDouble(main.LoyaltyDiscount);
            transmain.InvoiceUniqueId = main.InvoiceUniqueId;
            transmain.InvoiceIdshortCode = main.InvoiceIdshortCode;

            root.transmain = transmain;
            


            List<TransData.Transitem> transitems = new List<TransData.Transitem>();
            //ITEMSS
            foreach (var item in items)
            {
                TransData.Transitem tt = new TransData.Transitem();

                tt.idkey = Guid.Parse( item.Idkey);
                tt.id = item.Id;
                tt.transMainId = item.TransMainId;
                tt.itemId = item.ItemId;
                tt.itemType = item.ItemType;
                tt.itemName = item.ItemName;
                tt.itemPrice = item.ItemPrice;
                tt.qty = item.Qty ?? 1;
                tt.tax1 = item.Tax1;
                tt.amount = item.Amount ?? 0;
                tt.saleDateTime = item.SaleDateTime;
                tt.credits = item.Credits;
                tt.discount = item.Discount;
                tt.actualPrice = item.ActualPrice;
                tt.orderId = item.OrderId;
                tt.tax1Status = item.Tax1Status;

                transitems.Add(tt);


            }

            root.transitems = transitems;


           // root.date = DateTime.Today.ToString("yyyy-MM-dd");
          //  root.time = DateTime.Now.ToString("HH:mm:ss");

            root.date = transmain.saleDateTime.ToString("yyyy-MM-dd");
            root.time = transmain.saleDateTime.ToString("HH:mm:ss");

            return root;
        } 

    }



}
