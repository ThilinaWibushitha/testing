using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using my_pospointe.Models;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Net.Sockets;
using System.Security.Policy;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_pospointe.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FranchiseManagementController : ControllerBase
    {
        // GET: api/<FranchiseManagementController>
        [HttpGet("stores/{franchiseid}")]
        public IEnumerable<Fstore> Getstores(Guid franchiseid)
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                var stores = context.Fstores.Where(x => x.FranchiseId == franchiseid).ToList();
                return stores;
            }

        }

        // GET api/<FranchiseManagementController>/5
        [HttpGet("store/{storeid}")]
        public Fstore Getstore(Guid storeid)
        {
            using (var context = new FranchiseManagementContext())
            {
                var store = context.Fstores.FirstOrDefault(item => item.StoreId == storeid);

                return store;

            }
        }

        [HttpPut("store/{storeid}")]
        public void Updatestore(Guid storeid, [FromBody] Fstore value)
        {


            using (var context = new FranchiseManagementContext())
            {

                var item = context.Fstores.FirstOrDefault(item => item.StoreId == storeid);
                if (item != null)
                {
                    item.StoreCity = value.StoreCity;
                    item.LoyaltyFeeinFlatRate = value.LoyaltyFeeinFlatRate;
                    item.Status = value.Status;
                    item.LoyaltyFee = value.LoyaltyFee;
                    item.Marketing = value.Marketing;
                    item.LocalMarketing = value.LocalMarketing;
                    item.Email = value.Email;
                    item.AutoCharge = value.AutoCharge;
                    item.BillCustomerId = value.BillCustomerId;
                    item.LegalBusiness = value.LegalBusiness;
                    item.TaxId = value.TaxId;
                    item.BillDefaultPayment = value.BillDefaultPayment;
                    context.SaveChanges();
                }
            }
        }







        [HttpGet("franchise/totalsales/{franchiseid}")]
        public franchisesalestotal Getfranchisesalestotal(Guid franchiseid)
        {
            using (var context = new FranchiseManagementContext())
            {
                List<Fstore> stores = new List<Fstore>();
                stores = context.Fstores.Where(item => item.FranchiseId == franchiseid && item.OfflineStores == false).ToList();
                List<franchisesalestotal> sales = new List<franchisesalestotal>();
                decimal totalsales = 0;

                foreach (var store in stores)
                {
                    string server = clsConnections.server;
                    string port = clsConnections.port;
                    string username = clsConnections.username;
                    string password = clsConnections.password;

                    if (store.Remote == true)
                    {
                        server = clsConnections.remoteserver;
                        port = clsConnections.remoteport;
                        username = clsConnections.remoteusername;
                        password = clsConnections.remotepassword;
                    }

                    using (var context2 = new _167Context("Server=" + server + "," + port + ";Initial Catalog=" + store.StoreDb + ";Persist Security Info=False;User ID=" + username + ";Password=" + password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
                        {
                            //List<donutchartdata> data = new List<donutchartdata>();
                            DateTime now = DateTime.Now;
                            var startDate = new DateTime(now.Year, now.Month, 1);
                            var endDate = startDate.AddMonths(1).AddDays(-1);
                            var trans = context2.TblTransMains.ToList();



                            var sumtotal = context2.TblTransMains
                                 .Where(o => o.SaleDate >= startDate && o.SaleDate <= endDate && o.TransType == "SALE")
                                  .Select(o => o.GrandTotal).Sum();
                            totalsales += sumtotal ?? 0;

                        }

                }
                franchisesalestotal franchisesalestotal = new franchisesalestotal();
                franchisesalestotal.franchiseid = franchiseid;
                franchisesalestotal.grandsales = totalsales;
                franchisesalestotal.noofstores = stores.Count();
                return franchisesalestotal;

            }
        }






        [HttpGet("franchise/loyaltytotal/{date}/{franchiseid}")]
        public IActionResult GetstoresSalesList(Guid franchiseid, string date)
        {
            using (var context = new FranchiseManagementContext())
            {
                DateTime fromdate = DateTime.ParseExact(date, "MMyyyy", CultureInfo.InvariantCulture);
                DateTime toDate = fromdate.AddMonths(1).AddDays(-1);



                var stores = context.Fstores
               .Where(store => store.FranchiseId == franchiseid)
                 .GroupJoin(
               context.InvoiceMains.Where(invoice => invoice.InvoicePeriod == date),
                store => store.StoreId,
               invoice => invoice.StoreId,
                (store, invoices) => new { Store = store, Invoices = invoices }
                 )
                .SelectMany(
                 x => x.Invoices.DefaultIfEmpty(),
                 (x, invoice) => new { x.Store, Invoice = invoice }
                  )
               .Where(x => x.Invoice == null)
                 .Select(x => x.Store)
              .Distinct()
                 .ToList();









                List<FranchiseStoresSales.Salesrequeststore> sales = new List<FranchiseStoresSales.Salesrequeststore>();
                foreach (var store in stores)
                {

                    try
                    {
                        var city = store.StoreCity;
                        var storeid = store.StoreId;
                        var state = store.State;

                        decimal ubereatssale = 0;
                        decimal doordash = 0;
                        decimal grabhub = 0;
                        decimal others = 0;
                        decimal totalothersales = 0;
                        bool offlinestore = store.OfflineStores ?? false;
                        ubereatssale = context.OtherSalesofStores.Where(o => o.StoreId == storeid && o.Period == date).Select(o => o.UberEats).Sum() ?? 0;
                        doordash = context.OtherSalesofStores.Where(o => o.StoreId == storeid && o.Period == date).Select(o => o.DoorDash).Sum() ?? 0;
                        grabhub = context.OtherSalesofStores.Where(o => o.StoreId == storeid && o.Period == date).Select(o => o.Grabhub).Sum() ?? 0;
                        others = context.OtherSalesofStores.Where(o => o.StoreId == storeid && o.Period == date).Select(o => o.Others).Sum() ?? 0;
                        totalothersales = ubereatssale + grabhub + others + doordash;

                        string server = clsConnections.server;
                    string port = clsConnections.port;
                    string username = clsConnections.username;
                    string password = clsConnections.password;

                    if (store.Remote == true)
                    {
                        server = clsConnections.remoteserver;
                        port = clsConnections.remoteport;
                            username = clsConnections.remoteusername;
                            password = clsConnections.remotepassword;
                    }

                        using (var context2 = new _167Context("Server=" + server + "," + port + ";Initial Catalog=" + store.StoreDb + ";Persist Security Info=False;User ID=" + username + ";Password=" + password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
                        {
                            //List<donutchartdata> data = new List<donutchartdata>();
                            DateTime now = DateTime.Now;
                            var startDate = fromdate;
                            var endDate = startDate.AddMonths(1).AddDays(-1);

                            decimal sumtotal = 0;
                            decimal taxtotal = 0;
                            string storename = store.StoreCity;
                            string stoeremail = store.Email;
                            if (store.OfflineStores == false)
                            {

                                var trans = context2.TblTransMains.ToList();



                                sumtotal = context2.TblTransMains
                                     .Where(o => o.SaleDate >= startDate && o.SaleDate <= endDate && o.TransType == "SALE")
                                      .Select(o => o.GrandTotal).Sum() ?? 0;

                                taxtotal = context2.TblTransMains
                                     .Where(o => o.SaleDate >= startDate && o.SaleDate <= endDate && o.TransType == "SALE")
                                      .Select(o => o.Tax1).Sum() ?? 0;


                            }



                            var totalnetsales = (sumtotal - taxtotal) + totalothersales;
                            decimal loyalty = store.LoyaltyFee ?? 0;
                            decimal localmarketingperc = store.LocalMarketing ?? 0;
                            decimal marketingperc = store.Marketing ?? 0;
                            decimal loyaltyfeeofstore = 0;
                            decimal localmarketing = 0;
                            decimal marketing = 0;

                            if (store.LoyaltyFeeinFlatRate == true)
                            {
                                loyaltyfeeofstore = loyalty;
                            }

                            else
                            {
                                loyaltyfeeofstore = totalnetsales * (loyalty / 100);
                            }

                            localmarketing = totalnetsales * (localmarketingperc / 100);
                            marketing = totalnetsales * (marketingperc / 100);
                            decimal totalfees = loyaltyfeeofstore + localmarketing + marketing;

                            var line = new FranchiseStoresSales.Salesrequeststore
                            {
                                StoreId = storeid,
                                storename = storename,
                                storeemail = stoeremail,
                                storecity = city,
                                storestate = state,
                                StoreDb = store.StoreDb,
                                fromdate = startDate,
                                todate = endDate,
                                totalgrosssales = sumtotal,
                                totaltaxes = taxtotal,
                                marketing = marketing,
                                localmarketing = localmarketing,
                                totalnetsales = totalnetsales,
                                totalmarketsales = totalothersales,
                                LoyaltyFee = loyalty,
                                LoyaltyFeeinFlatRate = store.LoyaltyFeeinFlatRate,
                                totalloyalty = loyaltyfeeofstore,
                                localmarketingperc = localmarketingperc,
                                marketingperc = marketingperc,
                                totalfee = totalfees,
                                Offlinestore = offlinestore



                            };
                            sales.Add(line);




                        }
                    }

                    catch (Exception ex)
                    {
                        return BadRequest("Error on DB " + store.StoreDb + " Store : " + store.StoreCity + " Error : " + ex.Message);

                    }

                }

                return Ok(sales);
            }
        }


        [HttpGet("franchise/allstores/salestotals/{franchiseid}")]
        public IActionResult GetstoresSalestotalsList(Guid franchiseid)
        {
            using (var context = new FranchiseManagementContext())
            {
                List<Fstore> stores = new List<Fstore>();
                stores = context.Fstores.Where(item => item.FranchiseId == franchiseid && item.OfflineStores == false).ToList();
                List<storessalestotal> salesotals = new List<storessalestotal>();
                foreach (var store in stores)
                {
                    try
                    {


                        var city = store.StoreCity;
                        var storeid = store.StoreId;
                        var storestate = store.State;

                         string server = clsConnections.server;
                    string port = clsConnections.port;
                    string username = clsConnections.username;
                    string password = clsConnections.password;

                    if (store.Remote == true)
                    {
                        server = clsConnections.remoteserver;
                        port = clsConnections.remoteport;
                            username = clsConnections.remoteusername;
                            password = clsConnections.remotepassword;
                    }

                        using (var context2 = new _167Context("Server=" + server + "," + port + ";Initial Catalog=" + store.StoreDb + ";Persist Security Info=False;User ID=" + username + ";Password=" + password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
                        {
                            //List<donutchartdata> data = new List<donutchartdata>();
                            DateTime now = DateTime.Now;
                            var startDate = new DateTime(now.Year, now.Month, 1);
                            var endDate = startDate.AddMonths(1).AddDays(-1);
                            var trans = context2.TblTransMains.ToList();



                            var sumtotal = context2.TblTransMains
                                 .Where(o => o.SaleDate >= startDate && o.SaleDate <= endDate && o.TransType == "SALE")
                                  .Select(o => o.GrandTotal).Sum();

                            var sumtax = context2.TblTransMains
                                .Where(o => o.SaleDate >= startDate && o.SaleDate <= endDate && o.TransType == "SALE")
                                 .Select(o => o.Tax1).Sum();


                            var storename = context2.TblBusinessInfos.Where(o => o.StoreId == "1001").Select(o => o.BusinessName).FirstOrDefault();
                            var expense = 0;
                            var totalnetsales = sumtotal - (expense + sumtax);



                            var line = new storessalestotal
                            {
                                storeid = storeid,
                                storename = storename,
                                city = city,
                                StoreDb = store.StoreDb,
                                grandsales = sumtotal,
                                salestax = sumtax,
                                expenses = expense,
                                state = storestate,
                                netsales = totalnetsales

                            };
                            salesotals.Add(line);

                        }




                    }
                    catch (Exception ex)
                    {
                        return BadRequest("Error on DB " + store.StoreDb + " Store : " + store.StoreCity + " Error : " + ex.Message);

                    }



                }

                return Ok(salesotals);
            }
        }

        // POST api/<FranchiseManagementController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<FranchiseManagementController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FranchiseManagementController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


        [HttpGet("stores/ofstateslist/{franchiseid}")]
        public IEnumerable<StoreCountDto> Getstorescountofstates(Guid franchiseid)
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                var storeCountsByState = context.Fstores
                                         .GroupBy(store => store.State)
                                         .Select(group => new StoreCountDto
                                         {
                                             State = group.Key,
                                             Count = group.Count()
                                         })
                                         .ToList();
                return storeCountsByState;
            }

        }



        /////INVOICING///
        ///
        [HttpGet("invoices/{franchiseid}")]
        public IEnumerable<InvoiceMain> Getallinvoices(Guid franchiseid)
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                var stores = context.InvoiceMains.Where(x => x.FranchiseId == franchiseid).OrderByDescending(x => x.InvoiceId).ToList();
                return stores;
            }

        }

        [HttpGet("invoices/invoice/{invoiceid}")]
        public InvoiceMain Getinvoice(Guid invoiceid)
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                var invoice = context.InvoiceMains.FirstOrDefault(x => x.Id == invoiceid);
                return invoice;
            }

        }



        [HttpPost("generate/invoices/{franchiseid}")]
        public string createinvoce(Guid franchiseid, [FromBody] invoicecreaterequest value)
        {
            string storeuid;
            DateTime fromdate = DateTime.ParseExact(value.period, "MMyyyy", CultureInfo.InvariantCulture);
            DateTime toDate = fromdate.AddMonths(1).AddDays(-1);



            foreach (string storesall in value.stores)
            {
                // Here, 'item' represents the current string in the array.
                Guid storeid = Guid.Parse(storesall);

                using (var context = new FranchiseManagementContext())
                {

                    var procx = context.Franchises.Where(i => i.FranchiseId == franchiseid).Select(o => o.Processor).FirstOrDefault();

                    var store = context.Fstores.FirstOrDefault(item => item.StoreId == storeid);

                    if (store != null)
                    {
                        var city = store.StoreCity;
                        var state = store.State;

                        decimal ubereatssale = 0;
                        decimal doordash = 0;
                        decimal grabhub = 0;
                        decimal others = 0;
                        decimal totalothersales = 0;


                        ubereatssale = context.OtherSalesofStores.Where(o => o.StoreId == storeid && o.Period == value.period).Select(o => o.UberEats).Sum() ?? 0;
                        doordash = context.OtherSalesofStores.Where(o => o.StoreId == storeid && o.Period == value.period).Select(o => o.DoorDash).Sum() ?? 0;
                        grabhub = context.OtherSalesofStores.Where(o => o.StoreId == storeid && o.Period == value.period).Select(o => o.Grabhub).Sum() ?? 0;
                        others = context.OtherSalesofStores.Where(o => o.StoreId == storeid && o.Period == value.period).Select(o => o.Others).Sum() ?? 0;
                        totalothersales = ubereatssale + grabhub + others + doordash;

                    string server = clsConnections.server;
                    string port = clsConnections.port;
                    string username = clsConnections.username;
                    string password = clsConnections.password;

                    if (store.Remote == true)
                    {
                        server = clsConnections.remoteserver;
                        port = clsConnections.remoteport;
                            username = clsConnections.remoteusername;
                            password = clsConnections.remotepassword;
                    }




                        using (var context2 = new _167Context("Server=" + username + "," + port + ";Initial Catalog=" + store.StoreDb + ";Persist Security Info=False;User ID=" + username + ";Password=" + password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
                        {
                            List<feetypes> feetypes = new List<feetypes>();
                            DateTime now = DateTime.Now;
                            var startDate = fromdate;
                            var endDate = startDate.AddMonths(1).AddDays(-1);
                            decimal sumtotal = 0;
                            decimal taxtotal = 0;
                            string storename = store.StoreCity;
                            string stoeremail = store.Email;
                            if (store.OfflineStores == false)
                            {
                                var trans = context2.TblTransMains.ToList();



                                sumtotal = context2.TblTransMains
                                    .Where(o => o.SaleDate >= startDate && o.SaleDate <= endDate && o.TransType == "SALE")
                                     .Select(o => o.GrandTotal).Sum() ?? 0;

                                taxtotal = context2.TblTransMains
                                    .Where(o => o.SaleDate >= startDate && o.SaleDate <= endDate && o.TransType == "SALE")
                                     .Select(o => o.Tax1).Sum() ?? 0;


                            }


                            var totalnetsales = (sumtotal - taxtotal) + totalothersales;
                            decimal loyalty = store.LoyaltyFee ?? 0;




                            decimal localmarketingperc = store.LocalMarketing ?? 0;
                            decimal marketingperc = store.Marketing ?? 0;
                            decimal loyaltyfeeofstore = 0;
                            decimal localmarketing = 0;
                            decimal marketing = 0;



                            if (store.LoyaltyFeeinFlatRate == true)
                            {
                                loyaltyfeeofstore = loyalty;
                            }

                            else
                            {
                                loyaltyfeeofstore = totalnetsales * (loyalty / 100);
                            }

                            localmarketing = totalnetsales * (localmarketingperc / 100);
                            marketing = totalnetsales * (marketingperc / 100);
                            decimal totalfees = loyaltyfeeofstore + localmarketing + marketing;


                            InvoiceMain _item = new InvoiceMain()
                            {
                                FranchiseId = franchiseid,
                                StoreId = store.StoreId,
                                InvoicePeriod = value.period,
                                Date = DateTime.Now,
                                SubTotal = totalfees,
                                Tax1 = 0,
                                Tax2 = 0,
                                Discount = 0,
                                GrandTotal = totalfees,
                                TransType = "SALE",
                                Status = "OPEN",
                                CreatedBy = "SYSTEM",
                                CompletedDate = DateTime.Now


                            };

                            if (procx == "Bill")
                            {
                                _item.BillBankId = store.BillDefaultPayment;
                                _item.BillCustomerId = store.BillCustomerId;
                            }

                            context.InvoiceMains.Add(_item);
                            context.SaveChanges();

                            var line = new feetypes
                            {
                                ItemName = "Royalty Fee For " + value.period + " (Total Net Sales : " + string.Format("${0:0.00}", totalnetsales.ToString()) + " X " + string.Format("${0:0.00}", loyalty.ToString()) + ")",
                                price = loyaltyfeeofstore,
                                qty = 1,
                                orderlist = 1

                            };
                            feetypes.Add(line);

                            var line2 = new feetypes
                            {
                                ItemName = "Marketing Fee For " + value.period + " (Total Net Sales : " + string.Format("${0:0.00}", totalnetsales.ToString()) + " X " + string.Format("${0:0.00}", marketingperc.ToString()) + "%)",
                                price = marketing,
                                qty = 1,
                                orderlist = 1

                            };
                            feetypes.Add(line2);

                            var line3 = new feetypes
                            {
                                ItemName = "Local Marketing Fee For " + value.period + " (Total Net Sales : " + string.Format("${0:0.00}", totalnetsales.ToString()) + " X " + string.Format("${0:0.00}", localmarketingperc.ToString()) + "%)",
                                price = localmarketing,
                                qty = 1,
                                orderlist = 1

                            };
                            feetypes.Add(line3);

                            var line4 = new feetypes
                            {
                                ItemName = $"In Store Sales {(sumtotal - taxtotal).ToString("N2")}",
                                price = 0,
                                qty = 1,
                                orderlist = 1

                            };

                            feetypes.Add(line4);

                            var line5 = new feetypes
                            {
                                ItemName = $"UberEats Sales {ubereatssale.ToString("N2")}",
                                price = 0,
                                qty = 1,
                                orderlist = 1

                            };

                            feetypes.Add(line5);

                            var line6 = new feetypes
                            {
                                ItemName = $"DoorDash Sales {doordash.ToString("N2")}",
                                price = 0,
                                qty = 1,
                                orderlist = 1

                            };

                            feetypes.Add(line6);

                            var line7 = new feetypes
                            {
                                ItemName = $"GrubHub Sales {grabhub.ToString("N2")}",
                                price = 0,
                                qty = 1,
                                orderlist = 1

                            };

                            feetypes.Add(line7);

                            var line8 = new feetypes
                            {
                                ItemName = $"Other Sales {others.ToString("N2")}",
                                price = 0,
                                qty = 1,
                                orderlist = 1

                            };

                            feetypes.Add(line8);

                            foreach (var fee in feetypes)
                            {
                                InvoiceSub _item2 = new InvoiceSub()
                                {
                                    InvoiceMainId = _item.Id,
                                    Description = fee.ItemName,
                                    Price = fee.price,
                                    TaxAmount = 0,
                                    Qty = fee.qty,
                                    OrderList = fee.orderlist

                                };

                                context.InvoiceSubs.Add(_item2);
                                context.SaveChanges();

                            }


                            storeuid = store.Id.ToString();
                            string paymentlink = "https://mypospointe.com/remotepaymentlink/securedpayment?invoiceid=" + _item.Id;
                            sendemailinvoice(stoeremail, paymentlink, totalfees.ToString("F2"), false, _item.InvoiceId.ToString());
                        }
                    }



                }


            }
            return string.Empty;
        }

        [HttpGet("invoices/items/{invoiceid}")]
        public IEnumerable<InvoiceSub> Getallinvoiceitems(Guid invoiceid)
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                var items = context.InvoiceSubs.Where(x => x.InvoiceMainId == invoiceid).ToList();
                return items;
            }

        }

        [HttpPut("invoices/invoice/{invoiceid}")]
        public IActionResult Upateinvoice(Guid invoiceid, [FromBody] InvoiceMain main)
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                var invoice = context.InvoiceMains.FirstOrDefault(x => x.Id == invoiceid);
                if (invoice != null)
                {

                    invoice.Discount = main.Discount;
                    invoice.DiscountDecription = main.DiscountDecription;
                    invoice.GrandTotal = main.SubTotal - main.Discount;
                    context.SaveChanges();
                    return Ok();
                }
                return BadRequest();
            }

        }


        [HttpPost("invoices/invoice/sendreminder/{invoiceid}")]
        public string sendinvoicereminder(Guid invoiceid)
        {
            using (var context = new FranchiseManagementContext())
            {
                string status;
                //return new string[] { "value1", "value2" };
                var invoice = context.InvoiceMains.FirstOrDefault(x => x.Id == invoiceid);
                if (invoice != null)
                {

                    decimal grandtotal = invoice.GrandTotal ?? 0;
                    var store = context.Fstores.FirstOrDefault(x => x.StoreId == invoice.StoreId);
                    var invoiceidint = context.InvoiceMains.Where(o => o.Id == invoiceid).Select(o => o.InvoiceId).FirstOrDefault();
                    if (store != null)
                    {

                        var stoeremail = store.Email;

                        string paymentlink = "https://mypospointe.com/remotepaymentlink/securedpayment?invoiceid=" + invoiceid;
                        sendemailinvoice(stoeremail, paymentlink, grandtotal.ToString("F2"), true, invoiceidint.ToString());
                        status = "sent";


                    }

                    else
                    {
                        status = "store not found";
                    }

                }
                else
                {
                    status = "invoice not found";
                }
                return status;
            }

        }






        [HttpGet("sales/othermarketplaces/{period}/{franchiseid}")]
        public IEnumerable<OtherSalesofStore> Getallextrasales(Guid franchiseid, string period)
        {
            using (var context = new FranchiseManagementContext())
            {
                var storesWithSales = context.Fstores.Where(store => store.FranchiseId == franchiseid)
                .GroupJoin(
               context.OtherSalesofStores.Where(sale => sale.Period == period),
               store => store.StoreId,
               sale => sale.StoreId,
               (store, sales) => new { store, sales }
           )
           .SelectMany(
               x => x.sales.DefaultIfEmpty(),
               (x, sale) => new OtherSalesofStore
               {
                   StoreId = x.store.StoreId,
                   Period = sale != null ? sale.Period : period,
                   UberEats = sale != null ? sale.UberEats : 0,
                   DoorDash = sale != null ? sale.DoorDash : 0,
                   Grabhub = sale != null ? sale.Grabhub : 0,
                   Others = sale != null ? sale.Others : 0
               }
           )
           .ToList();

                return storesWithSales;

            }





        }


        [HttpPost("sales/othermarketplaces/{period}/{franchiseid}")]
        public void pushallextrasales(Guid franchiseid, string period, [FromBody] List<OtherSalesofStore> value)
        {
            using (var context = new FranchiseManagementContext())
            {
                foreach (var data in value)
                {
                    var ale = context.OtherSalesofStores.FirstOrDefault(x => x.StoreId == data.StoreId && x.Period == period);
                    if (ale != null)
                    {
                        ale.UberEats = data.UberEats;
                        ale.DoorDash = data.DoorDash;
                        ale.Grabhub = data.Grabhub;
                        ale.Others = data.Others;
                        ale.LastUpdated = DateTime.Now;
                        context.SaveChanges();
                    }

                    else
                    {
                        OtherSalesofStore _item = new OtherSalesofStore()
                        {
                            Period = data.Period,
                            StoreId = data.StoreId,
                            UberEats = data.UberEats,
                            DoorDash = data.DoorDash,
                            Grabhub = data.Grabhub,
                            Others = data.Others,
                            LastUpdated = DateTime.Now


                        };
                        context.OtherSalesofStores.Add(_item);
                        context.SaveChanges();

                    }

                }

            }





        }





















        [HttpPost("/email")]
        public string sendemailinvoice(string email, string paymentlink, string invoiceamount, bool reminder, string invoiceid)
        {
            //Your invoice is ready!
            //A Reminder for your Invoice
            string path = Path.Combine(Environment.CurrentDirectory, @"Scripts\invoiceupdate.html");
            string Body = System.IO.File.ReadAllText(path);

            MailMessage mm = new MailMessage();
            mm.To.Add(email);
            mm.Subject = "Invoice" + invoiceid;

            //string change1 = Body.Replace("%HEADING1%", heading);
            //string change2 = change1.Replace("$SHORTDESC$", shortdesc);
            //string change3 = change2.Replace("$Customer$", bizname);
            //string change4 = change3.Replace("$NO$", tikno);
            //string change5 = change4.Replace("$Type$", type);
            //string change6 = change5.Replace("$Subject$", subject);
            //string change7 = change6.Replace("$Status$", status);
            string change1 = Body.Replace("$INVOICEAMOUNT$", invoiceamount);
            string change2;
            if (reminder == false)
            {
                change2 = change1.Replace("$HEADINGG$", "Your invoice is ready!");
            }
            else
            {
                change2 = change1.Replace("$HEADINGG$", "A Reminder for your Invoice");
            }
            string Bodyfinal = change2.Replace("$INVOICELINK$", paymentlink);

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
            return "sent";


        }

    }

}

