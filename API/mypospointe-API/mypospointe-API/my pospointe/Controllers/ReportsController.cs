using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using my_pospointe.Models;
using System.Net.Mail;
using System.Threading.Channels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_pospointe.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        // GET: api/<ReportsController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<ReportsController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<ReportsController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<ReportsController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<ReportsController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        [HttpGet("flashreport/{startdate}/{enddate}")]
        public clsReports.flashreport Get(DateTime startdate, DateTime enddate)
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                clsReports.flashreport flash = new clsReports.flashreport();

                flash.startdate = startdate.ToString("MM/dd/yyyy");
                flash.enddate = enddate.ToString("MM/dd/yyyy");

                
                flash.grosssales= context.TblTransMains
              .Where(o => o.SaleDate >= startdate && o.SaleDate <= enddate && o.TransType == "SALE")
              .Select(o => o.GrandTotal).Sum();

                flash.grosssaleswtax = context.TblTransMains
              .Where(o => o.SaleDate >= startdate && o.SaleDate <= enddate && o.TransType == "SALE")
              .Select(o => o.GrandTotal).Sum();

               flash.nontaxsales = context.TblTransMains
              .Where(o => o.SaleDate >= startdate && o.SaleDate <= enddate && o.TransType == "SALE" && o.Tax1 == 0)
              .Select(o => o.GrandTotal).Sum();

                flash.salestax = context.TblTransMains
              .Where(o => o.SaleDate >= startdate && o.SaleDate <= enddate && o.TransType == "SALE")
              .Select(o => o.Tax1).Sum();

                var instorediscount = context.TblTransMains
                    .Where(o => o.SaleDate >= startdate && o.SaleDate <= enddate && o.TransType == "SALE")
                    .Select(o => o.InvoiceDiscount).Sum();

                flash.instorediscount = instorediscount;

                flash.netsales = flash.grosssales - (instorediscount + flash.salestax);

                flash.cashrefund = context.TblTransMains
             .Where(o => o.SaleDate >= startdate && o.SaleDate <= enddate && o.TransType == "RETURN")
             .Select(o => o.CashAmount).Sum();

                flash.cardrefund = context.TblTransMains
             .Where(o => o.SaleDate >= startdate && o.SaleDate <= enddate && o.TransType == "RETURN")
             .Select(o => o.CardAmount).Sum();

                flash.totalrefund = flash.cardrefund + flash.cashrefund;

                flash.cashtotal = context.TblTransMains
             .Where(o => o.SaleDate >= startdate && o.SaleDate <= enddate && o.TransType == "SALE")
             .Select(o => o.CashAmount).Sum();

                flash.cardtotal = context.TblTransMains
             .Where(o => o.SaleDate >= startdate && o.SaleDate <= enddate && o.TransType == "SALE" && o.CardType != "GIFTC")
             .Select(o => o.CardAmount).Sum();

                flash.giftcardtotal = context.TblTransMains
             .Where(o => o.SaleDate >= startdate && o.SaleDate <= enddate && o.TransType == "SALE" && o.CardType == "GIFTC")
             .Select(o => o.CardAmount).Sum();

                flash.tiptotal = context.TblTransMains
            .Where(o => o.SaleDate >= startdate && o.SaleDate <= enddate && o.TransType == "SALE")
            .Select(o => o.TipAmount).Sum();

                flash.nooftrans = context.TblTransMains
            .Where(o => o.SaleDate >= startdate && o.SaleDate <= enddate && o.TransType == "SALE")
            .Select(o => o.InvoiceId).Count();

                flash.nooftransreturn = context.TblTransMains
            .Where(o => o.SaleDate >= startdate && o.SaleDate <= enddate && o.TransType == "RETURN")
            .Select(o => o.InvoiceId).Count();


                return flash;

            }
        }

        [HttpGet("flashreportQB/{startdate}/{enddate}")]
        public clsReports.flashreport GetForQB(DateTime startdate, DateTime enddate, string dbvalue)
        {
            //const string HeaderKeyName = "db";
            //Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                clsReports.flashreport flash = new clsReports.flashreport();

                flash.startdate = startdate.ToString("MM/dd/yyyy");
                flash.enddate = enddate.ToString("MM/dd/yyyy");


                flash.grosssales = context.TblTransMains
              .Where(o => o.SaleDate >= startdate && o.SaleDate <= enddate && o.TransType == "SALE")
              .Select(o => o.GrandTotal).Sum();

                flash.grosssaleswtax = context.TblTransMains
              .Where(o => o.SaleDate >= startdate && o.SaleDate <= enddate && o.TransType == "SALE")
              .Select(o => o.GrandTotal).Sum();

                flash.nontaxsales = context.TblTransMains
               .Where(o => o.SaleDate >= startdate && o.SaleDate <= enddate && o.TransType == "SALE" && o.Tax1 == 0)
               .Select(o => o.GrandTotal).Sum();

                flash.salestax = context.TblTransMains
              .Where(o => o.SaleDate >= startdate && o.SaleDate <= enddate && o.TransType == "SALE")
              .Select(o => o.Tax1).Sum();

                flash.netsales = context.TblTransMains
              .Where(o => o.SaleDate >= startdate && o.SaleDate <= enddate && o.TransType == "SALE")
              .Select(o => o.Subtotal).Sum();

                flash.cashrefund = context.TblTransMains
             .Where(o => o.SaleDate >= startdate && o.SaleDate <= enddate && o.TransType == "RETURN")
             .Select(o => o.CashAmount).Sum();

                flash.cardrefund = context.TblTransMains
             .Where(o => o.SaleDate >= startdate && o.SaleDate <= enddate && o.TransType == "RETURN")
             .Select(o => o.CardAmount).Sum();

                flash.totalrefund = flash.cardrefund + flash.cashrefund;

                flash.cashtotal = context.TblTransMains
             .Where(o => o.SaleDate >= startdate && o.SaleDate <= enddate && o.TransType == "SALE")
             .Select(o => o.CashAmount).Sum();

                flash.cardtotal = context.TblTransMains
             .Where(o => o.SaleDate >= startdate && o.SaleDate <= enddate && o.TransType == "SALE" && o.CardType != "GIFTC")
             .Select(o => o.CardAmount).Sum();

                flash.giftcardtotal = context.TblTransMains
             .Where(o => o.SaleDate >= startdate && o.SaleDate <= enddate && o.TransType == "SALE" && o.CardType == "GIFTC")
             .Select(o => o.CardAmount).Sum();

                flash.tiptotal = context.TblTransMains
            .Where(o => o.SaleDate >= startdate && o.SaleDate <= enddate && o.TransType == "SALE")
            .Select(o => o.TipAmount).Sum();

                flash.nooftrans = context.TblTransMains
            .Where(o => o.SaleDate >= startdate && o.SaleDate <= enddate && o.TransType == "SALE")
            .Select(o => o.InvoiceId).Count();

                flash.nooftransreturn = context.TblTransMains
            .Where(o => o.SaleDate >= startdate && o.SaleDate <= enddate && o.TransType == "RETURN")
            .Select(o => o.InvoiceId).Count();


                return flash;

            }
        }

        [HttpGet("flashreport/DateTime/{StartDate}/{EndDate}")]
        public clsReports.flashreport GetReport(DateTime StartDate, DateTime EndDate)
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);

            // Parse the start date, time, end date, and end time
            DateTime StartDateTime = DateTime.ParseExact(StartDate.ToString ("MM-dd-yyyy HH:mm"), "MM-dd-yyyy HH:mm", null);
            DateTime EndDateTime = DateTime.ParseExact(EndDate.ToString ("MM-dd-yyyy HH:mm"), "MM-dd-yyyy HH:mm", null);

            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                clsReports.flashreport flash = new clsReports.flashreport();

                flash.startdate = StartDate.ToString("MM/dd/yyyy");
                flash.enddate = EndDate.ToString("MM/dd/yyyy");

                var grosssale = context.TblTransMains
            .Where(o => o.SaleDateTime >= StartDateTime && o.SaleDateTime <= EndDateTime && o.TransType == "SALE")
            .Select(o => o.GrandTotal).Sum();
                var grosssaleretrun = context.TblTransMains
            .Where(o => o.SaleDateTime >= StartDateTime && o.SaleDateTime <= EndDateTime && o.TransType == "RETURN")
            .Select(o => o.GrandTotal).Sum();

                flash.grosssales = grosssale - grosssaleretrun;

                flash.grosssaleswtax = flash.grosssales;

                flash.nontaxsales = context.TblTransMains
                    .Where(o => o.SaleDateTime >= StartDateTime && o.SaleDateTime <= EndDateTime && o.TransType == "SALE" && o.Tax1 == 0)
                    .Select(o => o.GrandTotal).Sum();

                var salestaxb4return = context.TblTransMains
                    .Where(o => o.SaleDateTime >= StartDateTime && o.SaleDateTime <= EndDateTime && o.TransType == "SALE")
                    .Select(o => o.Tax1).Sum();

                var salestaxreturn = context.TblTransMains
                    .Where(o => o.SaleDateTime >= StartDateTime && o.SaleDateTime <= EndDateTime && o.TransType == "RETURN")
                    .Select(o => o.Tax1).Sum();


                flash.salestax = salestaxb4return - salestaxreturn;

                var netsaleb4return = context.TblTransMains
                    .Where(o => o.SaleDateTime >= StartDateTime && o.SaleDateTime <= EndDateTime && o.TransType == "SALE")
                    .Select(o => o.Subtotal).Sum();
                var netsalereturn = context.TblTransMains
                    .Where(o => o.SaleDateTime >= StartDateTime && o.SaleDateTime <= EndDateTime && o.TransType == "RETURN")
                    .Select(o => o.Subtotal).Sum();

                var instorediscount = context.TblTransMains
                    .Where(o => o.SaleDateTime >= StartDateTime && o.SaleDateTime <= EndDateTime && o.TransType == "SALE")
                    .Select(o => o.InvoiceDiscount).Sum();

                flash.instorediscount = instorediscount;

                flash.netsales = flash.grosssales - (flash.salestax + flash.instorediscount);

                flash.cashrefund = context.TblTransMains
                    .Where(o => o.SaleDateTime >= StartDateTime && o.SaleDateTime <= EndDateTime && o.TransType == "RETURN")
                    .Select(o => o.CashAmount).Sum();

                flash.cardrefund = context.TblTransMains
                    .Where(o => o.SaleDateTime >= StartDateTime && o.SaleDateTime <= EndDateTime && o.TransType == "RETURN")
                    .Select(o => o.CardAmount).Sum();

                flash.totalrefund = flash.cardrefund + flash.cashrefund;

                var cashamountb4refund = context.TblTransMains
                    .Where(o => o.SaleDateTime >= StartDateTime && o.SaleDateTime <= EndDateTime && o.TransType == "SALE")
                    .Select(o => o.CashAmount).Sum();

                flash.cashtotal = cashamountb4refund - flash.cashrefund ?? 0;

                flash.cardtotal = context.TblTransMains
                    .Where(o => o.SaleDateTime >= StartDateTime && o.SaleDateTime <= EndDateTime && o.TransType == "SALE" && o.CardType != "GIFTC")
                    .Select(o => o.CardAmount).Sum();

                flash.giftcardtotal = context.TblTransMains
                    .Where(o => o.SaleDateTime >= StartDateTime && o.SaleDateTime <= EndDateTime && o.TransType == "SALE" && o.CardType == "GIFTC")
                    .Select(o => o.CardAmount).Sum();

                flash.tiptotal = context.TblTransMains
                    .Where(o => o.SaleDateTime >= StartDateTime && o.SaleDateTime <= EndDateTime && o.TransType == "SALE")
                    .Select(o => o.TipAmount).Sum();

                flash.nooftrans = context.TblTransMains
                    .Where(o => o.SaleDateTime >= StartDateTime && o.SaleDateTime <= EndDateTime && o.TransType == "SALE")
                    .Select(o => o.InvoiceId).Count();

                flash.nooftransreturn = context.TblTransMains
                    .Where(o => o.SaleDateTime >= StartDateTime && o.SaleDateTime <= EndDateTime && o.TransType == "RETURN")
                    .Select(o => o.InvoiceId).Count();

                return flash;

            }
        }



        [HttpGet("shiftclose/{startdate}")]
        public IEnumerable<TblDayOpen> Getallshiftcloses(DateTime startdate)
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
              

               return context.TblDayOpens
                .Where(x => x.Status == "Closed" && x.Date == startdate).ToList();

            }
        }

        [HttpGet("shiftclose/report/{id}")]
        public clsReports.shiftclosereport Getshiftclosereport(int id)
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                clsReports.shiftclosereport shift = new clsReports.shiftclosereport();

                var shiftmain =  context.TblDayOpens.FirstOrDefault(item => item.DayOpenId == id);
                if (shiftmain != null)
                {
                   
                    var use = context.TblUsers.FirstOrDefault(item => item.UserId == shiftmain.CashierId);
                    shift.cashier = use.UserName;
                    shift.shiftopen = shiftmain.OpenedDateTime.ToString();
                    shift.shiftclose = shiftmain.ClosedDateTime.ToString();
                    shift.startcash = shiftmain.OpeningBalance;

                    var cashamountb4refund = context.TblTransMains
              .Where(o => o.SaleDateTime >= shiftmain.OpenedDateTime && o.SaleDateTime <= shiftmain.ClosedDateTime && o.TransType == "SALE" && o.CashierId == shiftmain.CashierId).Select(o => o.CashAmount).Sum();



                    shift.cashrefund = context.TblTransMains
                     .Where(o => o.SaleDateTime >= shiftmain.OpenedDateTime && o.SaleDateTime <= shiftmain.ClosedDateTime && o.TransType == "RETURN" && o.CashierId == shiftmain.CashierId)
                     .Select(o => o.CashAmount).Sum();


                    shift.cashsales = cashamountb4refund - shift.cashrefund ?? 0;


                    shift.expectedcash = shift.cashsales + shift.startcash;
                    shift.actualcash = shiftmain.ClosingBalance;
                    shift.deference = shiftmain.Deference;

                    var grossb4return = context.TblTransMains
             .Where(o => o.SaleDateTime >= shiftmain.OpenedDateTime && o.SaleDateTime <= shiftmain.ClosedDateTime && o.TransType == "SALE" && o.CashierId == shiftmain.CashierId)
             .Select(o => o.GrandTotal).Sum();

                    var grossreturn = context.TblTransMains
            .Where(o => o.SaleDateTime >= shiftmain.OpenedDateTime && o.SaleDateTime <= shiftmain.ClosedDateTime && o.TransType == "RETURN" && o.CashierId == shiftmain.CashierId)
            .Select(o => o.GrandTotal).Sum();


                    shift.grosssales = grossb4return - grossreturn ?? 0;
                    shift.refunds = context.TblTransMains
            .Where(o => o.SaleDateTime >= shiftmain.OpenedDateTime && o.SaleDateTime <= shiftmain.ClosedDateTime && o.TransType == "RETURN" && o.CashierId == shiftmain.CashierId)
            .Select(o => o.GrandTotal).Sum();
                    shift.discount = context.TblTransMains
            .Where(o => o.SaleDateTime >= shiftmain.OpenedDateTime && o.SaleDateTime <= shiftmain.ClosedDateTime && o.TransType == "SALE" && o.CashierId == shiftmain.CashierId)
            .Select(o => o.InvoiceDiscount).Sum();
                    shift.totalcash = shift.cashsales;
            
                    shift.totalcashrounding = 0;
                    shift.totalcard = context.TblTransMains
            .Where(o => o.SaleDateTime >= shiftmain.OpenedDateTime && o.SaleDateTime <= shiftmain.ClosedDateTime && o.TransType == "SALE" && o.CashierId == shiftmain.CashierId)
            .Select(o => o.CardAmount).Sum();
                    shift.tip = context.TblTransMains
            .Where(o => o.SaleDateTime >= shiftmain.OpenedDateTime && o.SaleDateTime <= shiftmain.ClosedDateTime && o.TransType == "SALE" && o.CashierId == shiftmain.CashierId)
            .Select(o => o.TipAmount).Sum();


                    var clooection = context.TblDayOpenCashCollections.FirstOrDefault(x => x.DayOpenId == id && x.Type == "DAYCLOSE");
                    if (clooection != null)
                    {
                        shift.TblDayOpenCashCollection = clooection;
                    }
                }

                emailreport(id);

                return shift;
            }
        }

        [HttpPost("emailreport/shiftclose/report/{id}")]
        public void emailreport(int id)
        {
            try
            {
                const string HeaderKeyName = "db";
                Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
                using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
                {
                    var biz = context.TblBusinessInfos.FirstOrDefault(item => item.StoreId == "1001");

                    clsReports.shiftclosereport shift = new clsReports.shiftclosereport();

                    var shiftmain = context.TblDayOpens.FirstOrDefault(item => item.DayOpenId == id);
                    if (shiftmain != null)

                    {
                        var use = context.TblUsers.FirstOrDefault(item => item.UserId == shiftmain.CashierId);
                        shift.cashier = use.UserName;
                        shift.cashsales = context.TblTransMains
                      .Where(o => o.SaleDateTime >= shiftmain.OpenedDateTime && o.SaleDateTime <= shiftmain.ClosedDateTime && o.TransType == "SALE" && o.CashierId == shiftmain.CashierId)
                      .Select(o => o.CashAmount).Sum();

                        shift.totalcard = context.TblTransMains
               .Where(o => o.SaleDateTime >= shiftmain.OpenedDateTime && o.SaleDateTime <= shiftmain.ClosedDateTime && o.TransType == "SALE" && o.CashierId == shiftmain.CashierId)
               .Select(o => o.CardAmount).Sum();

                        decimal totalsales = shift.cashsales + shift.totalcard ?? 0;

                        string path = Path.Combine(Environment.CurrentDirectory, @"Scripts\shiftcloseemail.html");
                        string Body = System.IO.File.ReadAllText(path);

                        MailMessage mm = new MailMessage();
                        mm.To.Add(biz.BusinessEmail);
                        mm.Subject = "POS SHIFT REPORT";

                        //string change1 = Body.Replace("%HEADING1%", heading);
                        //string change2 = change1.Replace("$SHORTDESC$", shortdesc);
                        //string change3 = change2.Replace("$Customer$", bizname);
                        //string change4 = change3.Replace("$NO$", tikno);
                        //string change5 = change4.Replace("$Type$", type);
                        //string change6 = change5.Replace("$Subject$", subject);
                        //string change7 = change6.Replace("$Status$", status);
                        string change1 = Body.Replace("$HEADINGG$", "SHIFT REPORT");
                        string change2 = change1.Replace("$CASHIERNAME$", shift.cashier);
                        string change3 = change2.Replace("$TOTALSALES$", totalsales.ToString());
                        string change4 = change3.Replace("$TOTALCASHSALES$", shift.cashsales.ToString());
                        string change5 = change4.Replace("$TOTALCARDSALES$", shift.totalcard.ToString());
                        string change6 = change5.Replace("$BIZNAME$", biz.BusinessName);
                        string change7 = change6.Replace("$BIZADDDRESS$", biz.BusinessAddress);
                        string change8 = change7.Replace("$BIZZIPCODE$", biz.CityStatezip);
                        string change9 = change8.Replace("$BIZPHONE$", biz.BusinessPhone);
                        //string change10 = change9.Replace("$HEADINGG$", "SHIFT REPORT");
                        string link = "https://v3.mypospointe.com/shiftcloseview?shiftcloseid=" + id;
                        string Bodyfinal = change9.Replace("$REPORTLINK$", link);

                        mm.Body = Bodyfinal;
                        mm.IsBodyHtml = true;
                        mm.From = new MailAddress("no_reply@asnitinc.com");
                        SmtpClient smtp = new SmtpClient("smtp.office365.com");
                        smtp.Port = 587;
                        smtp.UseDefaultCredentials = false;
                        smtp.EnableSsl = true;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.TargetName = "STARTTLS/smtp.office365.com";
                        smtp.Credentials = new System.Net.NetworkCredential("no_reply@asnitinc.com", "Afshan@573184");
                        smtp.Send(mm);
                    }
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }

        [HttpGet("ItemSales")]
        public IActionResult GetItemSales(DateTime StartDateTime, DateTime EndDateTime, string Item)
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);

            if (string.IsNullOrEmpty(dbvalue))
            {
                return BadRequest("Database name is missing in the request header.");
            }

            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                var itemSalesData = context.TblTransSubs
                    .Where(o => o.SaleDateTime >= StartDateTime && o.SaleDateTime <= EndDateTime &&
                                o.ItemName.ToLower() == Item.ToLower())
                    .GroupBy(o => o.ItemName)
                    .Select(g => new
                    {
                        ItemName = g.Key,
                        TotalQuantitySold = (int)g.Sum(x => x.Qty),
                        TotalSales = g.Sum(x => x.Amount)
                    })
                    .ToList();

                if (!itemSalesData.Any())
                {
                    return NotFound("No sales data found for the given item within the selected time range.");
                }

                return Ok(itemSalesData);
            }



        }


        [HttpGet("ItemSalesByDepartment")]
        public IActionResult ItemSalesByDepartment(DateTime StartDateTime, DateTime EndDateTime, string Department)
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);

            if (string.IsNullOrEmpty(dbvalue))
            {
                return BadRequest("Database name is missing in the request header.");
            }

            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                var departmentSalesData = context.TblTransSubs
                .Where(o => o.SaleDateTime >= StartDateTime
                            && o.SaleDateTime <= EndDateTime
                            && o.ItemType == Department)
                .GroupBy(o => o.ItemName)
                .Select(g => new
                {
                    ItemName = g.Key,
                    TotalQuantitySold = (int)g.Sum(x => x.Qty),
                    TotalSales = g.Sum(x => x.Amount)
                })
                .ToList();

                if (departmentSalesData == null && departmentSalesData.Any())
                {
                    return NotFound("No sales data found for the given department within the selected time range.");
                }

                return Ok(departmentSalesData);
            }
        }
    }
}
