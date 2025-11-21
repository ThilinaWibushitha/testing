namespace my_pospointe.Models
{
    public class clsDashboard
    {
        public class linechartdata
        {
            public string? date { get; set; }

            public decimal? saleamount { get; set; }
        }

        public class donutchartdata
        {
            public string? name { get; set; }

            public decimal? total { get; set; }
        }

        public class totals
        {
            public decimal? totalsales { get; set; }

            public decimal? totalnetsales { get; set; }

            public decimal? totaltax { get; set; }

            public decimal? dtotalsales { get; set; }
            public decimal? dtotalnetsales { get; set; }

            public decimal? dtotaltax { get; set; }

            public decimal? tipmonth { get; set; }
        }

        public class BusyPeriodResponse
        {
            public string DayOfWeek { get; set; }
            public string HourRange { get; set; }
            public int TransactionCount { get; set; }
        }
    }
}
