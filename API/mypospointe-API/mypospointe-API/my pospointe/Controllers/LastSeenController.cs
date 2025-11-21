using Microsoft.AspNetCore.Mvc;
using my_pospointe.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net.Mail;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_pospointe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LastSeenController : ControllerBase
    {
        //// GET: api/<LastSeenController>
        //[HttpGet("getall/{sendemail}")]
        //public async Task <IEnumerable<data>> Get(string sendemail)
        //{
        //    List<data> datas= new List<data>();
        //    List<Register> Items = new List<Register>();
        //    var options = new RestClientOptions(clsConnections.baseurlreg)
        //    {
        //        MaxTimeout = -1,
        //    };
        //    var client = new RestClient(options);
        //    var request = new RestRequest("/POSLicense", Method.Get);
        //    RestResponse response = await client.ExecuteAsync(request);
        //    if (response.IsSuccessful)
        //    {
        //        var objResponse1 = JsonConvert.DeserializeObject<List<Register>>(response.Content);
        //        Items = objResponse1;
        //        var today = DateTime.Today;
        //        foreach (var lic in Items)
        //        {
        //            try {
        //                using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + lic.regNo + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=2;Encrypt=True;TrustServerCertificate=True"))
        //                {
        //                    try
        //                    {

        //                        var dateoftrans = context.TblTransMains
        //                            .OrderByDescending(x => x.InvoiceId) // Order by InvoiceId in descending order to get the latest transaction first
        //                            .Select(x => x.SaleDateTime) // Select only the SaleDateTime
        //                            .FirstOrDefault();


        //                        ////////////////////////////////
        //                     //   var highestValue = context.TblTransMains.Max(x => x.InvoiceId);
        //                    //    var dateoftrans = context.TblTransMains.Where(x => x.InvoiceId == highestValue).Select(e => e.SaleDateTime).FirstOrDefault();

        //                        bool updtoedate = false;
        //                        if (dateoftrans.HasValue && dateoftrans.Value != default(DateTime))
        //                        {
        //                            // Compare the dates, considering only the date part
        //                            if (dateoftrans.Value.Date == today)
        //                            {
        //                                updtoedate = true;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            updtoedate = false;
        //                        }
        //                        data dat = new data {
        //                            regNo = lic.regNo,
        //                            lastseendate = dateoftrans,
        //                            businessName = lic.businessName,
        //                            uptodate = updtoedate

        //                        };
        //                        datas.Add(dat);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        data dat = new data
        //                        {
        //                            regNo = lic.regNo,
        //                            lastseendate = null,
        //                            businessName = lic.businessName,
        //                            uptodate = false,
        //                            notes = ex.Message

        //                        };
        //                        datas.Add(dat);

        //                    }
        //                }


        //            }
        //            catch (Exception ex) {

        //                data dat = new data
        //                {
        //                    regNo = lic.regNo,
        //                    lastseendate = null,
        //                    businessName = lic.businessName,
        //                    uptodate = false,
        //                    notes = ex.Message

        //                };
        //                datas.Add(dat);

        //            }

        //        }
        //    }

        //    else
        //    {
        //        // JSRuntime.InvokeVoidAsync(response.Content);
        //    }
        //    if (sendemail == "true")
        //    {
        //        var counttotal = datas.Count();
        //        var countTrue = datas.Count(d => d.uptodate);
        //        var notsynged = counttotal - countTrue;
        //        var falseList = datas.Where(item => !item.uptodate).ToList();







        //        var htmlBody = new StringBuilder();
        //        htmlBody.Append("<html><body>");
        //        htmlBody.AppendFormat("<p>Total Stores: {0}</p>", counttotal);
        //        htmlBody.AppendFormat("<p>Stores Up to date = true: {0}</p>", countTrue);
        //        htmlBody.AppendFormat("<p>Stores not synced (with 'uptodate' = false): {0}</p>", notsynged);

        //        // Add a section for the falseList items
        //        htmlBody.Append("<h2>Details of Rows Not Synced</h2>");
        //        htmlBody.Append("<table border='1' style='border-collapse: collapse;'><tr><th>Reg No</th><th>Business Name</th></tr>");

        //        foreach (var item in falseList)
        //        {
        //            htmlBody.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", item.regNo, item.businessName);
        //        }

        //        htmlBody.Append("</table>");
        //        htmlBody.Append("</body></html>");

              







        //        MailMessage mm = new MailMessage();
        //        mm.To.Add("support@asnitinc.com");
        //        mm.Subject = "POS Inactivity Report";



        //        mm.Body = htmlBody.ToString(); ;
        //        mm.IsBodyHtml = true;
        //        mm.From = new MailAddress("no_reply@asnitinc.com");
        //        SmtpClient smtp = new SmtpClient("smtp.office365.com");
        //        smtp.Port = 587;
        //        smtp.UseDefaultCredentials = false;
        //        smtp.EnableSsl = true;
        //        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        smtp.TargetName = "STARTTLS/smtp.office365.com";
        //        smtp.Credentials = new System.Net.NetworkCredential("no_reply@asnitinc.com", "Afshan@573184");
        //        smtp.Send(mm);
        //    }
        //    return datas;
        //}

        //// GET api/<LastSeenController>/5
        //[HttpGet("{id}")]
        //public data Get(int id)
        //{
        //   data datas = new data();
        //    try
        //    {
        //        using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + id + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
        //        {
        //            try
        //            {
        //                bool updtoedate = false;
        //                var today = DateTime.Today;
        //                var highestValue = context.TblTransMains.Max(x => x.InvoiceId);
        //                var dateoftrans = context.TblTransMains.Where(x => x.InvoiceId == highestValue).Select(e => e.SaleDateTime).FirstOrDefault();


        //                if (dateoftrans.HasValue && dateoftrans.Value != default(DateTime))
        //                {
        //                    // Compare the dates, considering only the date part
        //                    if (dateoftrans.Value.Date == today)
        //                    {
        //                        updtoedate = true;
        //                    }
        //                }
        //                else
        //                {
        //                    updtoedate = false;
        //                }
                       



        //                datas.regNo = id;
        //                datas.lastseendate = dateoftrans;
        //                datas.businessName = "";
        //                datas.uptodate = updtoedate;


        //            }
        //            catch (Exception ex)
        //            {

        //                datas.regNo = id;
        //                datas.lastseendate = null;
        //                datas.businessName = "";
        //                datas.uptodate = false;
        //                datas.notes = ex.Message;

        //            }
        //        }
                

        //    }
        //    catch (Exception ex)
        //    {

        //        datas.regNo = id;
        //        datas.lastseendate = null;
        //        datas.businessName = "";
        //        datas.uptodate = false;
        //        datas.notes = ex.Message;

               

        //    }

        //    return datas;
        //}

        //// POST api/<LastSeenController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<LastSeenController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<LastSeenController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}


        //public class Register
        //{
        //    public int regNo { get; set; }
        //    public string email { get; set; }
        //    public string businessName { get; set; }
        //    public string businessAddress { get; set; }
        //    public string registrationCode { get; set; }
        //    public string registerType { get; set; }
        //    public string active { get; set; }
        //    public string deviceMac { get; set; }
        //    public string deviceId { get; set; }
        //    public string dbMethod { get; set; }
        //    public string dbName { get; set; }
        //    public string portalPassword { get; set; }
        //    public string firstName { get; set; }
        //    public string lastName { get; set; }
        //    public string businesstype { get; set; }
        //    public string paymentMethod { get; set; }
        //    public string lastOtp { get; set; }
        //    public string runningVersion { get; set; }
        //    public bool uberEats { get; set; }
        //    public string uberEatsStoreId { get; set; }
        //    public string uberEatsToken { get; set; }
        //    public string localIp { get; set; }
        //    public string publicIp { get; set; }
        //    public string otpforStationConnection { get; set; }
        //    public bool loyaltyPlanStatus { get; set; }
        //    public string loyaltyStoreId { get; set; }
        //    public string loyaltyStoreGroupId { get; set; }
        //}
        //public class data
        //{
        //    public int regNo { get; set; }
        //    public DateTime? lastseendate { get; set; }

        //    public string? businessName { get; set; }

        //    public bool uptodate { get; set; }

        //    public string? notes { get; set; }
        //}
    }
}
