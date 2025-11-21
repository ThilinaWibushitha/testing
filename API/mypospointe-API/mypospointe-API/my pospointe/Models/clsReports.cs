namespace my_pospointe.Models
{
    public class clsReports
    {
        public class flashreport
        {
            public string? startdate { get; set; }

            public string? enddate { get; set; }
            public decimal? grosssales { get; set; }
            public decimal? grosssaleswtax { get; set; }
            public decimal? nontaxsales { get; set; }
            public decimal? salestax { get; set; }
            public decimal? instorediscount { get; set; }
            public decimal? netsales { get; set; }
            public decimal? cashrefund { get; set; }
            public decimal? cardrefund { get; set; }
            public decimal? totalrefund { get; set; }
            public decimal? cashtotal { get; set; }
            public decimal? cardtotal { get; set; }
            public decimal? giftcardtotal { get; set; }
            public decimal? tiptotal { get; set; }
            public int? nooftrans { get; set; }
            public int? nooftransreturn { get; set; }
        }

        public class shiftclosereport
        {
            public string? cashier { get; set; }

            public string? shiftopen { get; set; }
            public string? shiftclose { get; set; }

            public decimal? startcash{ get; set; }
            public decimal? cashsales { get; set; }
            public decimal? cashrefund { get; set; }
            public decimal? expectedcash { get; set; }
            public decimal? actualcash { get; set; }
            public decimal? deference { get; set; }
            public decimal? grosssales { get; set; }
            public decimal? refunds { get; set; }
            public decimal? discount { get; set; }
            public decimal? tip { get; set; }
            public decimal? totalcash { get; set; }
            public decimal? totalcard { get; set; }
            public decimal? totalcashrounding { get; set; }
            public TblDayOpenCashCollection? TblDayOpenCashCollection { get; set; }
        }
    }
}
