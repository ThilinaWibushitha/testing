using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using my_pospointe.Models;
using NuGet.Packaging.Signing;
using Stripe;
using System.Net.Mail;
using Task = System.Threading.Tasks.Task;
using System.IO;
using System.Net;
using DinkToPdf;




// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_pospointe.Controllers.FMS
{
    [Route("[controller]")]
    [ApiController]
    public class AgreementController : ControllerBase
    {
        [HttpGet("{id}")]
        public Agreement Get(Guid id)
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                return context.Agreements.FirstOrDefault(item => item.Id == id);

            }
        }

        [HttpGet("signers/{id}")]
        public AgreeSignature Getsigner(Guid id)
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                return context.AgreeSignatures.FirstOrDefault(item => item.Id == id);

            }
        }

        // POST api/<AgreementController>
        [HttpPost]
        public void Post([FromBody] Rootreq value)
        {
            using (var context = new FranchiseManagementContext())
            {
                Agreement agreement = new Agreement { 
                  TemplateId = value.Agreement.TemplateId,
                  Name = value.Agreement.Name,
                  CreatedDate = DateTime.Now,
                  Data = value.Agreement.Data,
                  MultiSign = value.Agreement.MultiSign,
                  Mysign = value.Agreement.Mysign,
                  OtherParty = value.Agreement.OtherParty,
                  Completed = false,
                  SentEmails = value.Agreement.SentEmails,
                  FranchiseId = value.Agreement.FranchiseId,
                  
                
                };
                context.Agreements.Add(agreement);
                context.SaveChanges();

                foreach (var signers in value.AgreeSignatures)
                {
                    AgreeSignature sign = new AgreeSignature
                    {
                        AgreementId = agreement.Id,
                        Email = signers.Email,
                        FirstName = signers.FirstName,
                        LastName = signers.LastName,
                        Position = "Owner",
                        Completed = false

                    };
                
                    context.AgreeSignatures.Add(sign);
                    context.SaveChanges();
                    string body = "Hello there please use the link to do the Signature : https://localhost:44371/esignature/secured?id=" + sign.Id + " Feel Free to contact if you have any questions";
                    sendemail(sign.Email, body, "esign");
                }

                if (value.Agreement.Mysign == true)
                {
                    var mysign = context.Franchises.Where(x => x.FranchiseId == value.Agreement.FranchiseId).FirstOrDefault();

                    AgreeSignature sign = new AgreeSignature
                    {
                        AgreementId = agreement.Id,
                        Email = mysign.Email,
                        FirstName = mysign.FirstName,
                        LastName = mysign.LastName,
                        Position = mysign.Position,
                        Completed = false

                    };

                    context.AgreeSignatures.Add(sign);
                    context.SaveChanges();
                    string body = "Hello there please use the link to do the Signature : https://localhost:44371/esignature/secured?id=" + sign.Id + " Feel Free to contact if you have any questions";
                    sendemail(sign.Email, body, "esign");



                }
            }
        }

        // PUT api/<AgreementController>/5
        [HttpPut("signed/{id}")]
        public void Put(Guid id, [FromBody] AgreeSignature value)
        {
            using (var context = new FranchiseManagementContext())
            {
                var signer = context.AgreeSignatures.FirstOrDefault(x => x.Id == id);
                if (signer != null) {
                    signer.FirstName = value.FirstName;
                    signer.LastName = value.LastName;
                    signer.Position = value.Position;
                    signer.SignatureData = value.SignatureData;
                    signer.Completed = true;
                    signer.Ipaddress = value.Ipaddress;
                    signer.Location = value.Location;
                    signer.SignedDate = DateTime.Now;
                    context.SaveChanges();
                }
                checkforcompleteion(signer.AgreementId ?? Guid.NewGuid());


            }
        }

        [HttpPut("checkforcompleteion/{agreeid}")]
        public async Task checkforcompleteion(Guid agreeid)
        {
            using (var context = new FranchiseManagementContext())
            {
                bool allCompleted = !context.AgreeSignatures
                 .Any(a => a.AgreementId == agreeid && a.Completed == false);
                if (allCompleted)
                {
                    var doc = context.Agreements.FirstOrDefault(x => x.Id == agreeid);
                    if (doc != null)
                    {
                        var docdata = doc.Data;
                        var signers = context.AgreeSignatures.Where(x => x.AgreementId == doc.Id).ToList();
                        string ownerssigns = "";
                        foreach (var sig in signers)
                        {
                            string sign = $"<img src=\"{sig.SignatureData}\" alt=\"Signature\"  width=\"100\" height=\"80\" /> <p>" + sig.FirstName + " " + sig.LastName + "<br>" + sig.Position + "</p>";
                            ownerssigns += sign + "<br>";
                        }

                        string htmlWithSignature = doc.Data.Replace("${OwnersSignature}", ownerssigns);

                        doc.Data = htmlWithSignature;
                        doc.Completed = true;
                        doc.CompletedDate = DateTime.Now;
                        doc.AuthKey = Guid.NewGuid();
                        context.SaveChanges();

                        // Convert HTML to PDF
                        byte[] pdfData = ConvertHtmlToPdf(htmlWithSignature);

                        foreach (var sigg in signers)
                        {
                            string emailSubject = "Agreement Document";
                            string emailBody = "<p>Please find the attached agreement document.</p>";
                            sendemailwithatach(sigg.Email, emailSubject, emailBody, pdfData);
                        }
                    }
                }
            }

        }

        [HttpPut("convertopdf/{id}")]
        public byte[] ConvertHtmlToPdf(string html)
        {
            var converter = new SynchronizedConverter(new PdfTools());

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.Legal,
            },
                Objects = {
                new ObjectSettings() {
                    PagesCount = true,
                    HtmlContent = html,
                    WebSettings = { DefaultEncoding = "utf-8" },
                    HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 },
                    FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "This is the footer." }
                }
            }
            };

            return converter.Convert(doc);
        }

        // DELETE api/<AgreementController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        public class Rootreq
        {
            public Agreement Agreement { get; set; }

            public List<AgreeSignature> AgreeSignatures { get; set; }

        }

        [HttpPut("email/{id}")]
        public string sendemail(string email, string body, string subject)
        {
            //Your invoice is ready!
            //A Reminder for your Invoice
           

            MailMessage mm = new MailMessage();
            mm.To.Add(email);
            mm.Subject = subject;

            //string change1 = Body.Replace("%HEADING1%", heading);
            //string change2 = change1.Replace("$SHORTDESC$", shortdesc);
            //string change3 = change2.Replace("$Customer$", bizname);
            //string change4 = change3.Replace("$NO$", tikno);
            //string change5 = change4.Replace("$Type$", type);
            //string change6 = change5.Replace("$Subject$", subject);
            //string change7 = change6.Replace("$Status$", status);
          
            string Bodyfinal = body;

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

        [HttpPut("emailwattachment/{id}")]
        public string sendemailwithatach(string email, string body, string subject, byte[] pdfAttachment)
        {
            //Your invoice is ready!
            //A Reminder for your Invoice


            MailMessage mm = new MailMessage();
            mm.To.Add(email);
            mm.Subject = subject;
            using (MemoryStream ms = new MemoryStream(pdfAttachment))
            {
                Attachment attachment = new Attachment(ms, "Agreement.pdf", "application/pdf");
                mm.Attachments.Add(attachment);
                string Bodyfinal = body;

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
            //string change1 = Body.Replace("%HEADING1%", heading);
            //string change2 = change1.Replace("$SHORTDESC$", shortdesc);
            //string change3 = change2.Replace("$Customer$", bizname);
            //string change4 = change3.Replace("$NO$", tikno);
            //string change5 = change4.Replace("$Type$", type);
            //string change6 = change5.Replace("$Subject$", subject);
            //string change7 = change6.Replace("$Status$", status);

          
            return "sent";


        }
    }
}
