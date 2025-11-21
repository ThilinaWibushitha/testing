using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.MSIdentity.Shared;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using my_pospointe.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Stripe;
using Stripe.Checkout;
using System;
using System.Net.Mail;
using System.Security.Policy;
using System.Text;
using static my_pospointe.Models.BillLoginRes;
using static my_pospointe.Models.CardConnectTransactions;
using Task = System.Threading.Tasks.Task;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_pospointe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FranchisePaymentController : ControllerBase
    {
        // GET: api/<FracnhisePaymentController>
        [HttpGet("store/{franchiseid}")]
        public IEnumerable<Cctranscation> Gettransbyfranchise(Guid franchiseid)
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                var trans = context.Cctranscations.Where(x => x.FranchiseId == franchiseid).ToList();
                return trans;
            }

        }

        [HttpGet("transaction/{transid}")]
        public Cctranscation Gettrans(Guid transid)
        {
            using (var context = new FranchiseManagementContext())
            {
                var trans = context.Cctranscations.FirstOrDefault(item => item.Id == transid);

                return trans;

            }
        }

        [HttpGet("transaction/byinvoice/{invoiceid}")]
        public IEnumerable<Cctranscation> Gettransbyinvoice(Guid invoiceid)
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                var trans = context.Cctranscations.Where(x => x.InvoiceId == invoiceid).ToList();
                return trans;
            }
        }



        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //////////////PROCESS TRANSACTIONS////////

        // GET api/<FracnhisePaymentController>/5

        [HttpPut("processpayment/{franchiseid}/{invoiceid}")]
        public MyPaymentResponse processtranscation(Guid franchiseid, Guid invoiceid, [FromBody] PaymentRequest value)
        {
            MyPaymentResponse myres = new MyPaymentResponse();

            myres.transid = "";
            myres.amount = "";
            myres.approved = false;
            myres.status = "";
            myres.reason = "";
            myres.Authcode = "";

            try
            {

                using (var context = new FranchiseManagementContext())
                {
                    string merchid = context.Franchises.Where(o => o.FranchiseId == franchiseid).Select(o => o.MerchantId).FirstOrDefault();
                    string username = context.Franchises.Where(o => o.FranchiseId == franchiseid).Select(o => o.Username).FirstOrDefault();
                    string password = context.Franchises.Where(o => o.FranchiseId == franchiseid).Select(o => o.Password).FirstOrDefault();

                    Guid? storeid = context.InvoiceMains.Where(o => o.Id == invoiceid).Select(o => o.StoreId).FirstOrDefault();

                    // Concatenate username and password with a colon
                    string combined = $"{username}:{password}";
                    byte[] byteCombined = System.Text.Encoding.UTF8.GetBytes(combined);
                    string basicAuth = Convert.ToBase64String(byteCombined);



                    value.merchid = merchid;
                    value.capture = "y";
                    value.receipt = "n";
                    value.currency = "USD";
                    value.ecomind = "E";


                    var options = new RestClientOptions(clsConnections.cardconnecturl)
                    {
                        MaxTimeout = -1,
                    };
                    var client = new RestClient(options);
                    var request = new RestRequest("/cardconnect/rest/auth", Method.Put);
                    request.AddHeader("Authorization", "Basic " + basicAuth);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddParameter("application/json", value, ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    var objResponse1 = JsonConvert.DeserializeObject<PaymentResponse>(response.Content);
                    if (objResponse1 != null)
                    {
                        PaymentResponse res = new PaymentResponse();
                        res = objResponse1;
                        if (res.respstat == "A")
                        {
                            var item = context.InvoiceMains.FirstOrDefault(item => item.Id == invoiceid);
                            if (item != null)
                            {
                                item.Status = "PAID";
                                item.PaidAmount = res.amount;
                                item.CompletedDate = DateTime.Now;
                                context.SaveChanges();
                            }


                            Cctranscation _item = new Cctranscation()
                            {
                                MerchantService = "CC",
                                FranchiseId = franchiseid,
                                StoreId = storeid,
                                InvoiceId = invoiceid,
                                PaidByAch = false,
                                Respstat = res.respstat,
                                Account = res.account,
                                Token = res.token,
                                Retref = res.retref,
                                Amount = res.amount,
                                Expiry = res.expiry,
                                MerchId = res.merchid,
                                Respcode = res.respcode,
                                Resptext = res.resptext,
                                Avsresp = res.avsresp,
                                Cvvresp = res.cvvresp,
                                AuthCode = res.authcode,
                                Respproc = res.respproc,
                                Emv = res.entrymode,
                                DateTime = DateTime.Now

                            };
                            context.Cctranscations.Add(_item);
                            context.SaveChanges();

                            myres.transid = _item.Id.ToString();
                            myres.amount = res.amount;
                            myres.approved = true;
                            myres.Authcode = res.authcode;
                            myres.status = res.respstat;
                            myres.reason = res.resptext;

                            return myres;
                        }

                        else
                        {
                            Cctranscation _item = new Cctranscation()
                            {
                                MerchantService = "CC",
                                FranchiseId = franchiseid,
                                StoreId = storeid,
                                InvoiceId = invoiceid,
                                PaidByAch = false,
                                Respstat = res.respstat,
                                Account = res.account,
                                Token = res.token,
                                Retref = res.retref,
                                Amount = res.amount,
                                Expiry = res.expiry,
                                MerchId = res.merchid,
                                Respcode = res.respcode,
                                Resptext = res.resptext,
                                Avsresp = res.avsresp,
                                Cvvresp = res.cvvresp,
                                AuthCode = res.authcode,
                                Respproc = res.respproc,
                                Emv = res.entrymode,
                                DateTime = DateTime.Now

                            };
                            context.Cctranscations.Add(_item);
                            context.SaveChanges();

                            myres.status = res.respstat;
                            myres.reason = res.resptext;
                            return myres;
                        }
                    }

                    else
                    {
                        myres.reason = "Processor Error " + response.StatusCode;
                        return myres;
                    }


                }


            }

            catch (Exception ex) 
            {
                myres.reason = "Internal Server Error :" + ex;
                return myres;
            }
        }

        [HttpPost("webhook/stripe")]
        public async Task<IActionResult> Index()
        {
            string endpointSecret = "whsec_V5dhHXEZ9eBBFLcnn4uexMyiYZJ0MP0J";
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);
                Session ss = stripeEvent.Data.Object as Session;
                string payment = ss.PaymentStatus;
                string transid = ss.ClientReferenceId;
                Guid trid = Guid.Parse(transid);
                // Handle the event
                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {


                    if (payment == "paid")
                    {
                        using (var context = new FranchiseManagementContext())
                        {
                            var trnas = context.Cctranscations.Where(o => o.Id == trid).FirstOrDefault();
                            if (trnas != null)
                            {
                                trnas.Resptext = "PAID";


                                var inv = context.InvoiceMains.Where(o => o.Id == trnas.InvoiceId).FirstOrDefault();
                                if (inv != null)
                                {
                                    inv.Status = "PAID";
                                }
                                context.SaveChanges();
                            }

                        }

                    }

                    else if (payment == "unpaid")
                    {
                        using (var context = new FranchiseManagementContext())
                        {
                            var trnas = context.Cctranscations.Where(o => o.Id == trid).FirstOrDefault();
                            if (trnas != null)
                            {
                                trnas.Resptext = "PENDING";


                                var inv = context.InvoiceMains.Where(o => o.Id == trnas.InvoiceId).FirstOrDefault();
                                if (inv != null)
                                {
                                    inv.Status = "PENDING";
                                }
                                context.SaveChanges();
                            }

                        }


                    }


                }
                // ... handle other event types
                else if (stripeEvent.Type == Events.CheckoutSessionAsyncPaymentSucceeded)
                {

                    using (var context = new FranchiseManagementContext())
                    {
                        var trnas = context.Cctranscations.Where(o => o.Id == trid).FirstOrDefault();
                        if (trnas != null)
                        {
                            trnas.Resptext = "PAID";


                            var inv = context.InvoiceMains.Where(o => o.Id == trnas.InvoiceId).FirstOrDefault();
                            if (inv != null)
                            {
                                inv.Status = "PAID";
                            }
                            context.SaveChanges();
                        }

                    }
                }

                else if (stripeEvent.Type == Events.CheckoutSessionAsyncPaymentFailed)
                {
                    using (var context = new FranchiseManagementContext())
                    {
                        var trnas = context.Cctranscations.Where(o => o.Id == trid).FirstOrDefault();
                        if (trnas != null)
                        {
                            trnas.Resptext = "FAILED";


                            var inv = context.InvoiceMains.Where(o => o.Id == trnas.InvoiceId).FirstOrDefault();
                            if (inv != null)
                            {
                                inv.Status = "OPEN";
                            }
                            context.SaveChanges();
                        }

                    }


                }
                else
                { 
                
                }
                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }






        [HttpPost("processpayment/stripe/{franchiseid}/{invoiceid}")]
        public MyStripePaymentResponse processstripetrans(Guid franchiseid, Guid invoiceid)
        {
            try
            {
                using (var context = new FranchiseManagementContext())
                {
                    string Apikey = context.Franchises.Where(o => o.FranchiseId == franchiseid).Select(o => o.StripeApiKey).FirstOrDefault();
                    decimal invoiceamount = context.InvoiceMains.Where(o => o.Id == invoiceid).Select(o => o.GrandTotal).FirstOrDefault() ?? 0;
                    string myamountstring = string.Format("{0:0.00}", invoiceamount);
                    string decimalAsString = myamountstring.Replace(".", ""); // Convert to string and remove the dot
                    //string stringValue = invoiceamount.ToString().Replace(".", "").TrimEnd('0');
                    long result = long.Parse(decimalAsString);
                    string invoiceidst = invoiceid.ToString();
                    Guid? storeid = context.InvoiceMains.Where(o => o.Id == invoiceid).Select(o => o.StoreId).FirstOrDefault();

                    Cctranscation _item = new Cctranscation()
                    {
                        MerchantService = "SP",
                        FranchiseId = franchiseid,
                        StoreId = storeid,
                        InvoiceId = invoiceid,
                        SessionId = "",
                        RedirectLink = "",
                        DateTime = DateTime.Now

                    };
                    context.Cctranscations.Add(_item);
                    context.SaveChanges();

                    StripeConfiguration.ApiKey = Apikey;
                    var domain = clsConnections.currentmyposlink;
                    var options = new SessionCreateOptions
                    {
                        LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
                {
                    new Stripe.Checkout.SessionLineItemOptions
                    {
                        PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Invoice Payment",
                            },
                            UnitAmount = result,
                            TaxBehavior = "exclusive",
                        },

                        Quantity = 1,
                    },
                },
                        ClientReferenceId = _item.Id.ToString(),
                        Mode = "payment",
                        SuccessUrl = domain + "/payment-success?transid="+ _item.Id.ToString(),
                        CancelUrl = domain + "/remotepaymentlink/securedpayment?invoiceid="+invoiceid,
                    };
                    var service = new SessionService();
                    Session session = service.Create(options);
                    var trnas = context.Cctranscations.Where(o => o.Id == _item.Id).FirstOrDefault();
                    if (trnas != null)
                    {
                        trnas.SessionId = session.Id;
                        trnas.RedirectLink = session.Url;
                        context.SaveChanges();
                    }


                    MyStripePaymentResponse st = new MyStripePaymentResponse();
                    st.sessionid = session.Id;
                    st.redirecturl = session.Url;
                    st.error = "";




                    return st;
                }
            }

            catch (Exception ex)
            {
                MyStripePaymentResponse st = new MyStripePaymentResponse();
                st.sessionid = "";
                st.redirecturl = "";
                st.error = ex.Message;
                return st;
            }
        }


        [HttpPost("processpayment/billdotcom/all/{franchiseid}")]
        public void billdotall(Guid franchiseid)
        {
            List<billdotbulkresp> allresp = new List<billdotbulkresp>();
            string sessionid = getbillsessionid(franchiseid);
            if (sessionid != "error")
            {
               


                using (var context = new FranchiseManagementContext())
                {
                    var franchie = context.Franchises.FirstOrDefault(x => x.FranchiseId == franchiseid);

                    var invoices = context.InvoiceMains.Where(x => x.Status == "OPEN" && x.BillBankId.Length > 2).ToList();
                    foreach (var inv in invoices)
                    {
                       
                        var store = context.Fstores.FirstOrDefault(x => x.StoreId == inv.StoreId);
                        
                        if (store.AutoCharge == true)
                        {
                            billdotbulkresp res = new billdotbulkresp();
                            res.invoiceid = inv.Id;
                            res.invoice = inv.InvoiceId;
                            res.storeid = store.StoreId;
                            res.store = store.StoreCity;
                            var options = new RestClientOptions(clsConnections.baseurlbill)
                            {
                                MaxTimeout = -1,
                            };
                            var client = new RestClient(options);
                            var request2 = new RestRequest("/api/v2/ChargeCustomer.json", Method.Post);

                            // Header setup
                            request2.AddHeader("accept", "application/json");
                            request2.AddHeader("content-type", "application/x-www-form-urlencoded");

                            // Creating data object
                            var chargeCustomerData = new ChargeCustomerData
                            {
                                customerId = store.BillCustomerId,
                                paymentType = "3",
                                paymentAccountId = store.BillDefaultPayment,
                                emailReceipt = true,
                                totalAmount = Convert.ToDouble(inv.GrandTotal)
                            };

                            // Serializing data object to JSON and then encoding it
                            var jsonData = JsonConvert.SerializeObject(chargeCustomerData);
                            request2.AddParameter("data", jsonData, ParameterType.GetOrPost);

                            // Additional parameters
                            request2.AddParameter("devKey", franchie.BillDevKey, ParameterType.GetOrPost);
                            request2.AddParameter("sessionId", sessionid, ParameterType.GetOrPost);

                            // Execute request
                            var response2 = client.Execute(request2);
                            if (response2.IsSuccessful)
                            {
                                BillPaymentRes.Root payres = JsonConvert.DeserializeObject<BillPaymentRes.Root>(response2.Content);
                                if (payres.response_status == 0)
                                {


                                    inv.CompletedDate = DateTime.Now;
                                    inv.Status = payres.response_data.chargedReceivedPay.status;
                                    inv.BillPaymentId = payres.response_data.chargedReceivedPay.id;
                                    context.SaveChanges();
                                    res.response = payres.response_message;
                                    res.success = true;



                                }

                                else
                                {
                                    var response = JObject.Parse(response2.Content);

                                   
                                        string errorMessage = (string)response["response_data"]["error_message"];

                                      
                                   

                                    res.success = false;
                                    res.response = errorMessage;
                                }

                            }

                            else
                            {
                                res.success = false;
                                res.response = response2.StatusCode.ToString() + " : "+response2.Content;

                            }

                            allresp.Add(res);
                        }

                      
                    }

                }
                sendemailreport(true,allresp, "afshanasn@gmail.com");
              
            }

            else 
            {

                sendemailreport(false, allresp, "afshanasn@gmail.com");
               
            }

           

        }

        [HttpGet("session/billdotcom/{franchiseid}")]
        public string getbillsessionid(Guid franchiseid)
        {
            using (var context = new FranchiseManagementContext())
            {
                var franchie = context.Franchises.FirstOrDefault(x => x.FranchiseId == franchiseid);

                var options = new RestClientOptions(clsConnections.baseurlbill)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/api/v2/Login.json", Method.Post);
                request.AddHeader("accept", "application/json");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("userName", franchie.BillUsername);
                request.AddParameter("password", franchie.Billpassword);
                request.AddParameter("devKey", franchie.BillDevKey);
                request.AddParameter("orgId", franchie.BillOrgId);
                RestResponse response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    BillLoginRes.Root my = JsonConvert.DeserializeObject<BillLoginRes.Root>(response.Content);
                    if (my.response_status == 0)
                    {
                        return my.response_data.sessionId;
                    }

                    else
                    {
                        return "error";
                    }
                }
                else
                { 
                
                return "error"; }
            }
        }


            [HttpPost("processpayment/billdotcom/{franchiseid}/{invoiceid}")]
        public IActionResult billdotmanualy(Guid franchiseid, Guid invoiceid)
        {

            using (var context = new FranchiseManagementContext())
            {
                var inv = context.InvoiceMains.FirstOrDefault(x => x.Id == invoiceid && x.FranchiseId == franchiseid);

                if (inv != null)
                {
                    var procx = context.Franchises.Where(i => i.FranchiseId == franchiseid).Select(o => o.Processor).FirstOrDefault();
                    if (procx == "Bill")
                    {
                        var paymentid = context.Fstores.Where(i => i.StoreId == inv.StoreId).Select(o => o.BillDefaultPayment).FirstOrDefault();
                        if (!string.IsNullOrEmpty(paymentid))
                        {
                            var billcusid = context.Fstores.Where(i => i.StoreId == inv.StoreId).Select(o => o.BillCustomerId).FirstOrDefault();
                            var franchie = context.Franchises.FirstOrDefault(x => x.FranchiseId == franchiseid);

                            var options = new RestClientOptions(clsConnections.baseurlbill)
                            {
                                MaxTimeout = -1,
                            };
                            var client = new RestClient(options);
                            var request = new RestRequest("/api/v2/Login.json", Method.Post);
                            request.AddHeader("accept", "application/json");
                            request.AddHeader("content-type", "application/x-www-form-urlencoded");
                            request.AddParameter("userName", franchie.BillUsername);
                            request.AddParameter("password", franchie.Billpassword);
                            request.AddParameter("devKey", franchie.BillDevKey);
                            request.AddParameter("orgId", franchie.BillOrgId);
                            RestResponse response =  client.Execute(request);
                            if (response.IsSuccessful)
                            {
                                BillLoginRes.Root my = JsonConvert.DeserializeObject<BillLoginRes.Root>(response.Content);
                                if (my.response_status == 0)
                                {
                                    var sessionid = my.response_data.sessionId;
                                  
                                    var request2 = new RestRequest("/api/v2/ChargeCustomer.json", Method.Post);

                                    // Header setup
                                    request2.AddHeader("accept", "application/json");
                                    request2.AddHeader("content-type", "application/x-www-form-urlencoded");

                                    // Creating data object
                                    var chargeCustomerData = new ChargeCustomerData
                                    {
                                        customerId = billcusid,
                                        paymentType = "3",
                                        paymentAccountId = paymentid,
                                        emailReceipt = true,
                                        totalAmount = Convert.ToDouble(inv.GrandTotal)
                                    };

                                    // Serializing data object to JSON and then encoding it
                                    var jsonData = JsonConvert.SerializeObject(chargeCustomerData);
                                    request2.AddParameter("data", jsonData, ParameterType.GetOrPost);

                                    // Additional parameters
                                    request2.AddParameter("devKey", franchie.BillDevKey, ParameterType.GetOrPost);
                                    request2.AddParameter("sessionId", sessionid, ParameterType.GetOrPost);

                                    // Execute request
                                    var response2 = client.Execute(request2);
                                    if (response2.IsSuccessful)
                                    {
                                        BillPaymentRes.Root payres = JsonConvert.DeserializeObject<BillPaymentRes.Root>(response2.Content);
                                        if (payres.response_status == 0)
                                        {

                                            
                                            inv.CompletedDate = DateTime.Now;
                                            inv.Status = payres.response_data.chargedReceivedPay.status;
                                            inv.BillPaymentId = payres.response_data.chargedReceivedPay.id;
                                            context.SaveChanges();
                                            return Ok(payres.response_data.chargedReceivedPay.id);



                                        }

                                        else
                                        {
                                            return NotFound("BillDotCom : " + payres.response_message);
                                        }
                                       
                                    }

                                    else {
                                        return NotFound("BillDotCom : " + response2.StatusDescription);
                                    }
                                            
                                }

                                else
                                {
                                    return NotFound("BillDotCom : " + my.response_message);
                                }
                               
                            }
                            else { 
                                return NotFound("BillDotCom : "+ response.StatusDescription);
                            }
                           
                        }

                        else
                        {
                            return NotFound("No Default Payment Assgined to the Customer.");
                        }
                    }
                    else
                    { 
                     return BadRequest("Processor is not BillDotCom");
                    }

                  
                }

                else
                {
                    return NotFound();
                }
            }



        }

        [HttpPost("processpayment/manual/{franchiseid}/{invoiceid}")]
        public IActionResult procmanualy(Guid franchiseid, Guid invoiceid, [FromBody] mymanualpaymentrequest value)
        {

            using (var context = new FranchiseManagementContext())
            {
                var inv = context.InvoiceMains.FirstOrDefault(x => x.Id == invoiceid && x.FranchiseId == franchiseid);

                if (inv != null)
                {
                    inv.ManualPaymentType = value.reason;
                    inv.CompletedDate = DateTime.Now;
                    inv.Status = "PAID";
                    context.SaveChanges();
                    return Ok();
                }

                else
                {
                    return NotFound();
                }
            }



        }

        [HttpPost("internal/email")]
        public string sendemailreport(bool status,List<billdotbulkresp> allresp , string email)
        {
            //Your invoice is ready!
            //A Reminder for your Invoice
            string path = Path.Combine(Environment.CurrentDirectory, @"Scripts\invoiceupdate.html");
            string Body = System.IO.File.ReadAllText(path);

            MailMessage mm = new MailMessage();
            mm.To.Add(email);
            mm.Subject = "FMS AutoCharge Report";
            if (status == true)
            {
                string emailBody = "<h1>AutoCharge Invoice Details</h1>" + GenerateHtmlTable(allresp);

                mm.Body = emailBody;
            }
            else {
                string emailBody = "<h1>Failed to receive the session id for Bill.com</h1>";
                mm.Body = emailBody;
            }
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

    

    [HttpPost("internal/emailtempl")]
        public string GenerateHtmlTable(List<billdotbulkresp> allresp)
        {
            var html = new StringBuilder();

            html.Append("<table border='1'>");
            html.Append("<tr>");
           // html.Append("<th>Store ID</th>");
            html.Append("<th>Store</th>");
           // html.Append("<th>Invoice ID</th>");
            html.Append("<th>Invoice</th>");
            html.Append("<th>Success</th>");
            html.Append("<th>Response</th>");
            html.Append("</tr>");

            foreach (var resp in allresp)
            {
                html.Append("<tr>");
               // html.Append($"<td>{resp.storeid}</td>");
                html.Append($"<td>{resp.store}</td>");
               // html.Append($"<td>{resp.invoiceid}</td>");
                html.Append($"<td>{resp.invoice}</td>");
                html.Append($"<td>{resp.success}</td>");
                html.Append($"<td>{resp.response}</td>");
                html.Append("</tr>");
            }

            html.Append("</table>");

            return html.ToString();
        }


        public class mymanualpaymentrequest
        { 
          public string reason { get; set; }
        
        }

        public class billdotbulkresp
        { 
            public Guid? storeid { get; set; }
            public string store { get; set; }
            public Guid invoiceid { get; set; }
            public int invoice { get; set; }

            public bool success { get; set; }

            public string response { get; set; }

            
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
       

    }



}

