using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pospointe.Trans_Api
{
    public class TransData
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Root
        {
            public string date { get; set; }

            public string time { get; set; }

            public Transmain transmain { get; set; }
            public List<Transitem> transitems { get; set; }
        }

        public class Transitem
        {
            public Guid idkey { get; set; }
            public string id { get; set; }
            public int? transMainId { get; set; }
            public string itemId { get; set; }
            public string itemType { get; set; }
            public string itemName { get; set; }
            public double? itemPrice { get; set; }
            public double qty { get; set; }
            public double? tax1 { get; set; }
            public double amount { get; set; }
            public DateTime? saleDateTime { get; set; }
            public object credits { get; set; }
            public object discount { get; set; }
            public object actualPrice { get; set; }
            public int? orderId { get; set; }
            public string tax1Status { get; set; }
        }

        public class Transmain
        {
            public int? invoiceId { get; set; }
            public string transType { get; set; }
            public double subtotal { get; set; }
            public double tax1 { get; set; }
            public double grandTotal { get; set; }
            public DateTime saleDateTime { get; set; }
            //public SaleDate saleDate { get; set; }
            // public SaleTime saleTime { get; set; }
            public double cashAmount { get; set; }
            public double cardAmount { get; set; }
            public string cardNumber { get; set; }
            public string stationId { get; set; }
            public string cashierId { get; set; }
            public double cashChangeAmount { get; set; }
            public string paidby { get; set; }
            public string retref { get; set; }
            public string cardType { get; set; }
            public string cardHolder { get; set; }
            public double invoiceDiscount { get; set; }
            public string phoneNo { get; set; }
            public string entryMethod { get; set; }
            public string accountType { get; set; }
            public string aid { get; set; }
            public string tcarqc { get; set; }
            public string href { get; set; }
            public string hostRefNum { get; set; }
            public string deviceOrgRefNum { get; set; }
            public string customerId { get; set; }
            public double? totalCredit { get; set; }
            public double tipAmount { get; set; }
            public string giftCardNumber { get; set; }
            public string checkNumber { get; set; }
            public string holdName { get; set; }
            public string customerName { get; set; }
            public double? loyaltyDiscount { get; set; }

            public Guid? InvoiceUniqueId { get; set; }

            public string? InvoiceIdshortCode { get; set; }

        }

        public class SaleDate
        {
            public int? year { get; set; }
            public int? month { get; set; }
            public int? day { get; set; }
            public int? dayOfWeek { get; set; }
        }

        public class SaleTime
        {
            public int? hour { get; set; }
            public int? minute { get; set; }
        }


        public class TransMainDto
        {
            public decimal InvoiceID { get; set; }
            public string? InvoiceIdshortCode { get; set; }
            public DateTime? SaleDateTime { get; set; }

            public string TransType { get; set; }

            public decimal? InvoiceDiscount { get; set; }

            public decimal? LoyaltyDiscount { get; set; }
            public decimal GrandTotal { get; set; }
            public string CashierID { get; set; }
            public string Paidby { get; set; }
            public string CardNumber { get; set; }
        }




        /////////////////////SHIFT CLOSE CASH AMOUNT REQUEST ////////////////////////////
        public class shiftclosecashrequest
        {
            public string cashierid { get; set; }
            public string stationid { get; set; }
            public DateTime starttime { get; set; }
            public DateTime endtime { get; set; }

        }




        /////////////////////SHIFT CLOSE CASH AMOUNT RESPONSE ////////////////////////////

        public class shiftclosecashreponse
        {
            public double? cashsales { get; set; }


        }
    }
}
