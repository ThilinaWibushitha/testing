namespace my_pospointe.Models
{
    public class BillLoginRes
    {
        public class ResponseData
        {
            public string apiEndPoint { get; set; }
            public string sessionId { get; set; }
            public string orgId { get; set; }
            public string usersId { get; set; }
            public bool? isOrgLocked { get; set; }
        }

        public class Root
        {
            public int? response_status { get; set; }
            public string response_message { get; set; }
            public ResponseData response_data { get; set; }
        }

        public class ChargeCustomerData
        {
            public string customerId { get; set; }
            public string paymentType { get; set; }
            public string paymentAccountId { get; set; }
            public bool emailReceipt { get; set; }
            public double totalAmount { get; set; }
        }



    }
}
