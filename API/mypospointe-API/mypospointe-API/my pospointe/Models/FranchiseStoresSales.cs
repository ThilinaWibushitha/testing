namespace my_pospointe.Models
{
    public class FranchiseStoresSales
    {
        public partial class Salesrequeststore
        {

            public Guid? StoreId { get; set; }

            public string? storename{ get; set; }

            public string? storeemail { get; set; }
            public string? storecity { get; set; }

            public string? storestate { get; set; }
            public string? StoreDb { get; set; }
            public DateTime? fromdate { get; set; }
            public DateTime? todate { get; set; }

            public decimal? totalgrosssales { get; set; }

            public decimal? totalmarketsales { get; set; }

            public decimal? totaltaxes { get; set; }

            public decimal? totalnetsales { get; set; }
            public bool? LoyaltyFeeinFlatRate { get; set; }

            public decimal? LoyaltyFee { get; set; }

            public decimal? localmarketing{ get; set; }

            public decimal? localmarketingperc { get; set; }

            public decimal? marketing { get; set; }

            public decimal? marketingperc { get; set; }

            public decimal? totalloyalty { get; set; }

            public decimal? totalfee { get; set; }

            public bool? Offlinestore { get; set; }
        }
    }

    public partial class franchisesalestotal
    {
        public Guid? franchiseid{ get; set; }
        public decimal? grandsales { get; set; }
        public decimal? netsales { get; set; }

        public int? noofstores { get; set; }
    }

    public partial class storessalestotal
    {
        public Guid? storeid { get; set; }
        public string? storename { get; set; }
        public string? city { get; set; }

        public string? state { get; set; }

        public decimal? grandsales { get; set; }

        public decimal? expenses { get; set; }

        public decimal? salestax { get; set; }

        public string? StoreDb { get; set; }

        public decimal? netsales { get; set; }
    }

    public partial class StoreCountDto
    {
        public string State { get; set; }
        public int Count { get; set; }
    }

    public partial class invoicecreaterequest
    {
        public string period { get; set; }
        public string[] stores { get; set; }
    }

    public partial class feetypes
    {
        public string ItemName { get; set; }
        public decimal price { get; set; }

        public decimal qty { get; set; }
        public int orderlist { get; set; }
    }
}
