using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using my_pospointe.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_pospointe.Controllers.FMS
{
    [Route("[controller]")]
    [ApiController]
    public class FMSReportingController : ControllerBase
    {
        // GET: api/<FMSReportingController>
       // [HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<FMSReportingController>/5
        [HttpGet("{feetype}/{franchiseid}/{period}")]
        public IActionResult Get(Guid franchiseid , string period , string feetype)
        {
            using (var context = new FranchiseManagementContext())
            {
                List<InvoiceSub> results = new List<InvoiceSub> ();

                if (feetype == "marketing")
                {
                    var filteredInvoiceSubs = from invoiceSub in context.InvoiceSubs
                                              join invoiceMain in context.InvoiceMains
                                              on invoiceSub.InvoiceMainId equals invoiceMain.Id
                                              where invoiceSub.Description.StartsWith("Marketing Fee For") && invoiceSub.Price > 0
                                                    && invoiceMain.InvoicePeriod == period && invoiceMain.FranchiseId == franchiseid
                                              select invoiceSub;

                    results = filteredInvoiceSubs.ToList();
                }

                else if (feetype == "royalty")
                {
                    var filteredInvoiceSubs = from invoiceSub in context.InvoiceSubs
                                              join invoiceMain in context.InvoiceMains
                                              on invoiceSub.InvoiceMainId equals invoiceMain.Id
                                              where invoiceSub.Description.StartsWith("Royalty Fee For") && invoiceSub.Price > 0
                                                    && invoiceMain.InvoicePeriod == period && invoiceMain.FranchiseId == franchiseid
                                              select invoiceSub;

                    results = filteredInvoiceSubs.ToList();

                }

                else if (feetype == "localmarketing")
                {
                    var filteredInvoiceSubs = from invoiceSub in context.InvoiceSubs
                                              join invoiceMain in context.InvoiceMains
                                              on invoiceSub.InvoiceMainId equals invoiceMain.Id
                                              where invoiceSub.Description.StartsWith("Local Marketing Fee For") && invoiceSub.Price > 0
                                                    && invoiceMain.InvoicePeriod == period && invoiceMain.FranchiseId == franchiseid
                                              select invoiceSub;

                    results = filteredInvoiceSubs.ToList();

                }

                else
                {
                    return BadRequest();
                }



                if (results != null)
                {

                    Reportresponse res = new Reportresponse();

                    res.chargetype = feetype;
                    
                    res.businessname = context.Franchises.Where(x => x.FranchiseId == franchiseid).Select(x =>  x.FranchiseName).FirstOrDefault();
                    res.businessemail = context.Franchises.Where(x => x.FranchiseId == franchiseid).Select(x => x.FranchiseEmail).FirstOrDefault();
                    res.period = period;

                    List<Chargerslist> cc = new List<Chargerslist>();

                    foreach (var item in results)
                    {
                        string StoreID = context.InvoiceMains.Where(x => x.Id == item.InvoiceMainId).Select(x => x.StoreId.ToString()).FirstOrDefault();
                        Chargerslist charge = new Chargerslist
                        {
                            storeid = Guid.Parse(StoreID),
                            invoiceid = item.InvoiceMainId,
                            invoicesimpleid = context.InvoiceMains.Where(x => x.Id == item.InvoiceMainId).Select(x => x.InvoiceId).FirstOrDefault(),
                            storename = context.Fstores.Where(x => x.StoreId == Guid.Parse(StoreID)).Select(x => x.StoreCity).FirstOrDefault(),
                            invoiceperiod = period,
                            status = context.InvoiceMains.Where(x => x.Id == item.InvoiceMainId).Select(x => x.Status).FirstOrDefault(),
                            amount = item.Price


                        };
                        if (charge != null)
                        {
                            cc.Add(charge);
                        }

                      //  res.chargelist.Add(item);
                    }

                    res.chargelist = cc;

                    res.totalamount = res.chargelist.Sum(x => x.amount) ?? 0;
                    res.totalinvoices = res.chargelist.Count();

                    return Ok(res);
                }
                else
                { 
                    return NoContent();
                }

            }
        }





        //// POST api/<FMSReportingController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<FMSReportingController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<FMSReportingController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        public class Reportresponse {

            public string chargetype { get; set; }
            public string businessname { get; set; }
            public string businessemail { get; set; }
            public string period { get; set; }
            public decimal totalamount { get; set; }
            public int totalinvoices { get; set; }
            public List<Chargerslist> chargelist { get; set; }
           // public List<InvoiceSub> chargelist { get; set; }

        }

        public class Chargerslist { 
        
            public Guid? storeid { get; set; }
            public Guid? invoiceid { get; set; }
            public int? invoicesimpleid { get; set; }
            public string? storename { get; set; }
            public string? invoiceperiod { get; set; }
            public string? status { get; set; }
            public decimal? amount { get; set; }

        } 
    }
}
