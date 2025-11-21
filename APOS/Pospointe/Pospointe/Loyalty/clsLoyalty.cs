using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pospointe.Loyalty
{
    internal class clsLoyalty
    {
        public class Customer
        {
            public string id { get; set; }
            public string memberedStoregrpid { get; set; }
            public string phoneNo { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string email { get; set; }
            public string signedupstoreId { get; set; }
            public DateTime signeddate { get; set; }
            public string lastvisitedstore { get; set; }
            public string lastInvoiceid { get; set; }
            public bool status { get; set; }
            public bool taxexcempt { get; set; }
            public string loyalitypoints { get; set; }
            public string membershipcard { get; set; }
            public string planid { get; set; }
            public string address1 { get; set; }
            public string address2 { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string zipcode { get; set; }
            public string password { get; set; }
            public bool? verified { get; set; }
            public string lastOtp { get; set; }
            public DateTime? dob { get; set; }
            public string metaId { get; set; }
            public string googleId { get; set; }
            public string appleId { get; set; }
            public DateTime? lastActivity { get; set; }
            public bool? termsAndConditions { get; set; }
            public bool? smsMarketing { get; set; }
            public bool? emailMarketing { get; set; }
        }

        public class UpdatePoints
        {
            public string loyalitypoints { get; set; }
            public string lastInvoiceid { get; set; }
            public string lastvisitedstore { get; set; }
        }

        public class SelectedCustomer
        {
            public static string SelectedCustomerID = "";
            public static string SelectedCustomerName = "";
            public static string SelectedCustomerNewPoints = "";
            public static string SelectedcustomerCurrentPoints = "";
            public static string Selectedcustomerphone = "";
        }

        public class Offer
        {
            public string id { get; set; }
            public string storeGroupId { get; set; }
            public bool status { get; set; }
            public DateTime createdDate { get; set; }
            public DateTime expDate { get; set; }
            public string description { get; set; }
            public string pointsRequired { get; set; }
            public double radeemAmount { get; set; }
            public bool radeemPointsAfterUsed { get; set; }
            public string? OfferImage { get; set; }

            public string? OfferType { get; set; }

            public string? ItemId { get; set; }

            public bool? InStore { get; set; }
        }

        public static int minimumpointsforoffers { get; set; }

        public static int pointstoradeem { get; set; }

        public static double Ammounttoradeem { get; set; }
        public static List<clsLoyalty.Offer> Offers { get; set; }
    }
}
