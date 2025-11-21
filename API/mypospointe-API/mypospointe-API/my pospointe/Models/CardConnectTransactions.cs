namespace my_pospointe.Models
{
    public class CardConnectTransactions
    {

        public class BinInfo
        {
            public string country { get; set; }
            public string product { get; set; }
            public string bin { get; set; }
            public string cardusestring { get; set; }
            public bool? gsa { get; set; }
            public bool? corporate { get; set; }
            public bool? fsa { get; set; }
            public string subtype { get; set; }
            public bool? purchase { get; set; }
            public bool? prepaid { get; set; }
            public string issuer { get; set; }
            public string binlo { get; set; }
            public string binhi { get; set; }
        }

        public partial class PaymentResponse
        {
            public string? amount { get; set; }
            public string? resptext { get; set; }
            public string? commcard { get; set; }
            public string? cvvresp { get; set; }
            public string? respcode { get; set; }
            public string? batchid { get; set; }
            public string? avsresp { get; set; }
            public string? entrymode { get; set; }
            public string? merchid { get; set; }
            public string? token { get; set; }
            public string? authcode { get; set; }
            public string? respproc { get; set; }
            public string? bintype { get; set; }
            public string? expiry { get; set; }
            public string? retref { get; set; }
            public string? respstat { get; set; }
            public string? account { get; set; }
        }

        public class PaymentRequest
        {
            public string? merchid { get; set; }
            public string? account { get; set; }
            public string? expiry { get; set; }
            public string? amount { get; set; }
            public string? currency { get; set; }
            public string? name { get; set; }
            public string? capture { get; set; }
            public string? receipt { get; set; }
            public string? cvv2 { get; set; }
            public string? postal { get; set; }
            public string? address { get; set; }
            public string? city { get; set; }
            public string? region { get; set; }
            public string? country { get; set; }
            public string? ecomind { get; set; }
        }

       

        public class MyPaymentResponse
        {
            public string? transid { get; set; }
            public string? amount { get; set; }
            public bool? approved { get; set; }
            public string? status { get; set; }
            public string? reason { get; set; }
            public string? Authcode { get; set; }

        }

        public class MyStripePaymentResponse
        {
            public string? sessionid{ get; set; }
            public string? redirecturl { get; set; }

            public string? error { get; set; }
        }
    }
}
