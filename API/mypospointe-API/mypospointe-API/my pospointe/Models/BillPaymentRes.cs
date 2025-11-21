namespace my_pospointe.Models
{
    public class BillPaymentRes
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class ChargedReceivedPay
        {
            public string entity { get; set; }
            public string id { get; set; }
            public DateTime? createdTime { get; set; }
            public DateTime? updatedTime { get; set; }
            public string customerId { get; set; }
            public string status { get; set; }
            public string paymentDate { get; set; }
            public string depositToAccountId { get; set; }
            public bool? isOnline { get; set; }
            public string paymentType { get; set; }
            public double? amount { get; set; }
            public object localAmount { get; set; }
            public object exchangeRate { get; set; }
            public double? unappliedAmount { get; set; }
            public object description { get; set; }
            public string refNumber { get; set; }
            public double? convFeeAmount { get; set; }
            public string payToBankAccountId { get; set; }
            public string source { get; set; }
            public string moneyOutId { get; set; }
            public bool? isMarkedAsCompleted { get; set; }
            public bool? holdDisbursement { get; set; }
            public string customerBankAccountId { get; set; }
            public string customerCardAccountId { get; set; }
            public string guestPayAccountId { get; set; }
            public object rPConvFee { get; set; }
            public List<object> invoicePays { get; set; }
            public List<object> vCardReceivableInfo { get; set; }
        }

        public class ResponseData
        {
            public ChargedReceivedPay chargedReceivedPay { get; set; }
        }

        public class Root
        {
            public int? response_status { get; set; }
            public string response_message { get; set; }
            public ResponseData response_data { get; set; }
        }


    }
}
