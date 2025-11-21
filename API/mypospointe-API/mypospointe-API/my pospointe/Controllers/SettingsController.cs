using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using my_pospointe.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_pospointe.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        // GET: api/<SettingsController>
       
       

        // GET api/<SettingsController>/5
        [HttpGet("paxip")]
        public PAXResp Get()
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            string id = "1001";
            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                var paxip = context.TblProcessings.Where(item => item.StationId == 1).Select(x => x.PaxIp).FirstOrDefault();

                PAXResp pp = new PAXResp {
                    paxip = paxip
                };

                return pp;
            }
        }











        // PUT api/<SettingsController>/5
        [HttpPut("paxip")]
        public void Put( [FromBody] PAXResp value)
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            string id = "1001";
            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                var paxip = context.TblProcessings.Where(item => item.StationId == 1).FirstOrDefault();

              if (paxip != null) {

                    paxip.PaxIp = value.paxip;
                    context.SaveChanges();
                
                }

               
            }

        }



        public class PAXResp
        { 
            public string paxip { get; set; }
        
        }

        public class Bsinfo
        {
            public string storeId { get; set; }
            public string businessName { get; set; }
            public string businessAddress { get; set; }
            public string cityStatezip { get; set; }
            public string businessPhone { get; set; }
            public string businessEmail { get; set; }
            public string ownerName { get; set; }
            public string ownerPhone { get; set; }
            public string logoPath { get; set; }
            public string regcode { get; set; }
            public string footer1 { get; set; }
            public string footer2 { get; set; }
            public string footer3 { get; set; }
            public string footer4 { get; set; }
            public string encryptionKey { get; set; }
            public bool allowNegativeQty { get; set; }
            public bool mixNmatch { get; set; }
        }


    }
}
