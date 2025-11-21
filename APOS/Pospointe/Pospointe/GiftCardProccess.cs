using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pospointe
{
    public class GiftCardProccess
    {
        public class ReloadRequest
        {
            public string encrypted { get; set; }

            public string cardtoken { get; set; }

            //Reload Request

            public decimal amount { get; set; }
            public Guid FranchiseeID { get; set; }

            public string PosRef { get; set; }

        }

        public class ReloadResponse
        {

            public string statuscode { get; set; }
            public string description { get; set; }

            public string HostRef { get; set; }
            public string cardno { get; set; }
            public decimal balance { get; set; }
            public bool status { get; set; }
            public Guid franchiseeid { get; set; }
            public string storename { get; set; }

            public string storeaddress { get; set; }

            public DateTime datetimereloaded { get; set; }
        }

        public class RequestBalanceCheck
        {
            public string encrypted { get; set; }
            public string cardtoken { get; set; }
        }

        public class ResponseBalanceCheck
        {
            public string statuscode { get; set; }
            public string description { get; set; }
            public string cardending { get; set; }
            public decimal balance { get; set; }
            public bool status { get; set; }
            public DateTime? activated { get; set; }

            public DateTime? lastused { get; set; }

            public DateTime? expire { get; set; }
        }


        public class RequestChangeStatus
        {
            public string encrypted { get; set; }
            public string cardtoken { get; set; }
            public bool status { get; set; }
            public string reason { get; set; }
        }

        public class ResponseChangeStatus
        {
            public string statuscode { get; set; }
            public string description { get; set; }
            public string card { get; set; }
            public decimal balance { get; set; }
            public bool status { get; set; }
        }

        public class RadeemRequest
        {
            public string encrypted { get; set; }

            public string cardtoken { get; set; }

            //Reload Request

            public decimal amount { get; set; }
            public Guid FranchiseeID { get; set; }

            public string PosRef { get; set; }

            public bool acceptpartialamount { get; set; }

        }

        public class RadeemResponse
        {

            public string statuscode { get; set; }
            public string description { get; set; }

            public string HostRef { get; set; }
            public string cardno { get; set; }

            public decimal Approvedbalance { get; set; }
            public decimal Previousbalance { get; set; }
            public decimal Newbalance { get; set; }

            public bool status { get; set; }
            public Guid franchiseeid { get; set; }
            public string storename { get; set; }
            public string storeaddress { get; set; }
            public DateTime datetimeradeemed { get; set; }
        }
    }
}
