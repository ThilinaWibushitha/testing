using Microsoft.AspNetCore.Mvc;
using my_pospointe.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_pospointe.Controllers.FMS
{
    [Route("[controller]")]
    [ApiController]
    public class FMSDashboardController : ControllerBase
    {
       

        // GET api/<FMSDashboardController>/5
        [HttpGet("{id}")]
        public Dashboardresponse Get(Guid id)
        {
            using (var context = new FranchiseManagementContext())
            {
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, 1, 1); // First day of the current year
                var endDate = new DateTime(now.Year, 12, 31); // Last day of the current year



                var sumtotal = context.InvoiceMains
                     .Where(o => o.Date >= startDate && o.Date <= endDate && o.TransType == "SALE" && o.Status =="PAID" && o.FranchiseId == id)
                      .Select(o => o.GrandTotal).Sum();

                var duetotal = context.InvoiceMains
                    .Where(o => o.Date >= startDate && o.Date <= endDate && o.TransType == "SALE" && o.Status == "OPEN" && o.FranchiseId == id)
                     .Select(o => o.GrandTotal).Sum();

                var pending = context.InvoiceMains
                    .Where(o => o.Date >= startDate && o.Date <= endDate && o.TransType == "SALE" && o.Status == "PENDING" && o.FranchiseId == id)
                     .Select(o => o.GrandTotal).Sum();

                Dashboardresponse res = new Dashboardresponse();
                res.yearincome = Convert.ToDouble(sumtotal);
                res.dueinvoice = Convert.ToDouble(duetotal);
                res.pending = Convert.ToDouble(pending);
                return res;
               
            }
        }

        






        public partial class Dashboardresponse
        {
            public double? yearincome { get; set;}
            public double? dueinvoice { get; set; }
            public double? pending { get; set; }
            

        }
    }
}
