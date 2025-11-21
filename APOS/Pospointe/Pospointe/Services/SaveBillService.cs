using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Dac.Extensibility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pospointe.LocalData;
using Pospointe.Models;
using Pospointe.PosMenu;
using Pospointe.Trans_Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using static Pospointe.Trans_Api.TransData;

namespace Pospointe.Services
{
    class SaveBillService
    {
        public static async Task SaveBillAsync(TransData.Root data)
        {
            bool isSaved = false;

            // Try saving to the primary cloud server
            if (!LoggedData.Isoffline)
            {
                isSaved = await SaveBillService.SaveBilltoCloud(data);

                // If primary server fails, try the backup server
                if (!isSaved)
                {
                    //MessageBox.Show("Swtich to backyp");
                    LoggedData.transbaseurl = clsConnections.transserverbackupurl;
                    isSaved = await SaveBillService.SaveBilltoCloud(data);
                    if (isSaved)
                    {
                        FrmPosMain.startofflinetimer(false);
                    }


                }
            }

            // If cloud save fails, save locally
            if (!isSaved)
            {
                isSaved = await SaveBillService.SaveBilltoLocal(data);

                if (!isSaved)
                {
                    MessageBox.Show("Error While saving Locally");
                }
                else
                {
                    FrmPosMain.startofflinetimer(true);
                }
            }


        }



        public static async Task<bool> SaveBilltoCloud(TransData.Root data)
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
                //msg.LblHeader.Content = "Success";
                //msg.LblMessage.Content = "Trans Saved Successfully";
                //msg.ShowDialog();
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

        public static async Task<bool> SaveBilltoLocal(TransData.Root value)
        {
            using (var context = new PosDb1Context())
            {

                int maxid = 0;
                try
                {
                    var maxTrans = await context.TblTransMains.MaxAsync(p => p.InvoiceId);
                    maxid = Convert.ToInt32(maxTrans) + 1;
                }
                catch
                {
                    maxid = 1;
                }

              

                TblTransMain _main = new TblTransMain()
                {
                    InvoiceId = maxid,
                    TransType = value.transmain.transType,
                    Subtotal = Convert.ToDecimal( value.transmain.subtotal),
                    Tax1 = Convert.ToDecimal(value.transmain.tax1),
                    GrandTotal = Convert.ToDecimal(value.transmain.grandTotal),
                    SaleDate = DateOnly.Parse(value.date),
                    SaleDateTime = value.transmain.saleDateTime,
                    SaleTime = TimeOnly.Parse(value.time),
                    CashAmount = Convert.ToDecimal(value.transmain.cashAmount),
                    CardAmount = Convert.ToDecimal(value.transmain.cardAmount),                   
                    CardNumber = value.transmain.cardNumber,
                    StationId = value.transmain.stationId,
                    CashierId = value.transmain.cashierId,
                    CashChangeAmount = Convert.ToDecimal(value.transmain.cashChangeAmount),
                    Paidby = value.transmain.paidby,
                    Retref = value.transmain.retref,
                    CardType = value.transmain.cardType,
                    CardHolder = value.transmain.cardHolder,
                    InvoiceDiscount = Convert.ToDecimal(value.transmain.invoiceDiscount),
                    PhoneNo = value.transmain.phoneNo,
                    EntryMethod = value.transmain.entryMethod,
                    AccountType = value.transmain.accountType,
                    Aid = value.transmain.aid,
                    Tcarqc = value.transmain.tcarqc,
                    Href = value.transmain.href,
                    HostRefNum = value.transmain.hostRefNum,
                    DeviceOrgRefNum = value.transmain.deviceOrgRefNum,
                    CustomerId = value.transmain.customerId,
                    TotalCredit = Convert.ToDecimal(value.transmain.totalCredit),
                    TipAmount = Convert.ToDecimal(value.transmain.tipAmount),
                    GiftCardNumber = value.transmain.giftCardNumber,
                    CheckNumber = value.transmain.checkNumber,
                    HoldName = value.transmain.holdName,
                    CustomerName = value.transmain.customerName,
                    LoyaltyDiscount = Convert.ToDecimal(value.transmain.loyaltyDiscount),
                    InvoiceUniqueId = value.transmain.InvoiceUniqueId,
                    InvoiceIdshortCode = value.transmain.InvoiceIdshortCode
                };
               await context.TblTransMains.AddAsync(_main);
                foreach (var item in value.transitems)
                {
                    Guid guid = Guid.NewGuid();
                    TblTransSub _sub = new TblTransSub()
                    {
                        Idkey = guid.ToString(),
                        Id = item.id,
                        TransMainId = maxid,
                        ItemId = item.itemId,
                        ItemType = item.itemType,
                        ItemName = item.itemName,
                        ItemPrice = item.itemPrice,
                        Qty = item.qty,
                        Tax1 = item.tax1,
                        Amount = item.amount,
                        SaleDateTime = item.saleDateTime,
                        Credits = 0,
                        Discount = (double?)item.discount,
                        ActualPrice = 0,
                        OrderId = item.orderId,
                        Tax1Status = item.tax1Status
                    };

                    await context.TblTransSubs.AddAsync(_sub);
                   

                }
                await context.SaveChangesAsync();
            }

            return true;
               

        }

       
    }
}
