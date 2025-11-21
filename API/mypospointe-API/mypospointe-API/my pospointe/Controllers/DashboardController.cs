using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using my_pospointe.Models;
using Newtonsoft.Json.Linq;
using System.Linq;
using static my_pospointe.Models.clsDashboard;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_pospointe.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        // GET: api/<DashboardController>
        [HttpGet("linechart")]
        public IEnumerable<clsDashboard.linechartdata> Getline()
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            List<string> dates = new List<string>() { DateTime.Today.AddDays(-6).ToString("MM-dd"), DateTime.Today.AddDays(-5).ToString("MM-dd"), DateTime.Today.AddDays(-4).ToString("MM-dd"), DateTime.Today.AddDays(-3).ToString("MM-dd"), DateTime.Today.AddDays(-2).ToString("MM-dd"), DateTime.Today.AddDays(-1).ToString("MM-dd"), DateTime.Today.ToString("MM-dd") };
            List<clsDashboard.linechartdata> authors = new List<clsDashboard.linechartdata>();
            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                //List<donutchartdata> data = new List<donutchartdata>();

                var trans = context.TblTransMains.ToList();

                //return context.Devices.FirstOrDefault(item => item.Id == id);
                foreach (var date in dates)
                {

                    var sum = context.TblTransMains
                     .Where(o => o.SaleDate >= Convert.ToDateTime(date) && o.SaleDate < Convert.ToDateTime(date).AddDays(1) && o.TransType == "SALE")
                      .Select (o => o.GrandTotal).Sum();
                    // .Where(o => EntityFramework Convert.ToDateTime(date)).Count()
                    var line = new clsDashboard.linechartdata { date = date, saleamount = sum };
                    authors.Add(line);


                }

                return authors;
            }
        }

        // GET api/<DashboardController>/5
        [HttpGet("donut")]

        public IEnumerable<clsDashboard.donutchartdata> Getdonut()
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            List<clsDashboard.donutchartdata> authors = new List<clsDashboard.donutchartdata>();
            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                //List<donutchartdata> data = new List<donutchartdata>();

                var tickets = context.TblTransMains.ToList();

                //return context.Devices.FirstOrDefault(item => item.Id == id);
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                var cash = context.TblTransMains
                .Where(o => o.SaleDate >= startDate && o.SaleDate <= endDate && o.TransType == "SALE")
                .Select(o => o.CashAmount).Sum();
                var line = new clsDashboard.donutchartdata { name = "CASH", total = cash };
                authors.Add(line);
                var card = context.TblTransMains
               .Where(o => o.SaleDate >= startDate && o.SaleDate <= endDate && o.TransType == "SALE")
               .Select(o => o.CardAmount).Sum();
                var line2 = new clsDashboard.donutchartdata { name = "CARD", total = card };
                authors.Add(line2);
                return authors;
            }


        }

        [HttpGet("storebusy")]
        public List<clsDashboard.BusyPeriodResponse> getbusdaydata()
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            using (var _context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                // Get the last 7 days transactions
                DateTime sevenDaysAgo = DateTime.Now.AddDays(-7);

                var transactions = _context.TblTransMains
                                           .Where(t => t.SaleDateTime >= sevenDaysAgo)
                                           .ToList();

                // Group by day of the week and hour, then find the hour with the most transactions per day
                var busiestPeriods = transactions
                    .GroupBy(t => new { DayOfWeek = t.SaleDateTime.DayOfWeek, Hour = t.SaleDateTime.Hour })
                    .Select(g => new
                    {
                        DayOfWeek = g.Key.DayOfWeek,
                        Hour = g.Key.Hour,
                        TransactionCount = g.Count()
                    })
                    .GroupBy(g => g.DayOfWeek)
                    .Select(g => g.OrderByDescending(x => x.TransactionCount).FirstOrDefault()) // Get the busiest hour per day
                    .Select(g => new BusyPeriodResponse
                    {
                        DayOfWeek = g.DayOfWeek.ToString(),
                        HourRange = $"{g.Hour:00}:00 - {g.Hour + 1:00}:00",
                        TransactionCount = g.TransactionCount
                    })
                    .OrderBy(p => Enum.Parse<DayOfWeek>(p.DayOfWeek))
                    .ToList();

                return busiestPeriods;
            }

        }


        // GET: api/<DashboardController>
        [HttpGet("totals")]
        public clsDashboard.totals Gettotals()
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            
            clsDashboard.totals tot = new clsDashboard.totals();
            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                //List<donutchartdata> data = new List<donutchartdata>();
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                var trans = context.TblTransMains.ToList();
                var startD = new DateTime(now.Year, now.Month, now.Day);

                // Set the endDate to the end of today
                var endD = startDate.AddDays(1).AddTicks(-1);


                var sumtotal = context.TblTransMains
                     .Where(o => o.SaleDate >= startDate && o.SaleDate <= endDate && o.TransType == "SALE")
                      .Select(o => o.GrandTotal).Sum();
                // .Where(o => EntityFramework Convert.ToDateTime(date)).Count()

                var nettotal = context.TblTransMains
                     .Where(o => o.SaleDate >= startDate && o.SaleDate <= endDate && o.TransType == "SALE")
                      .Select(o => o.Subtotal).Sum();

                var taxtotal = context.TblTransMains
                    .Where(o => o.SaleDate >= startDate && o.SaleDate <= endDate && o.TransType == "SALE")
                     .Select(o => o.Tax1).Sum();

                var tiptotal = context.TblTransMains
                    .Where(o => o.SaleDate >= startDate && o.SaleDate <= endDate && o.TransType == "SALE")
                     .Select(o => o.TipAmount).Sum();

                var dsumtotal = context.TblTransMains
                     .Where(o => o.SaleDate == startD && o.TransType == "SALE")
                      .Select(o => o.GrandTotal).Sum();

                var dnettotal = context.TblTransMains
                    .Where(o => o.SaleDate == startD && o.TransType == "SALE")
                    .Select(o => o.Subtotal).Sum();

                var dtaxtotal = context.TblTransMains
                    .Where(o => o.SaleDate == startD && o.TransType == "SALE")
                    .Select(o => o.Tax1).Sum();


                tot.totalsales = sumtotal;
                tot.totalnetsales = nettotal;
                tot.totaltax = taxtotal;
                tot.tipmonth = tiptotal;
                tot.dtotalsales = dsumtotal;
                tot.dtotalnetsales = dnettotal;
                tot.dtotaltax = dtaxtotal;

                return tot;
            }
        }
    }


    
}
