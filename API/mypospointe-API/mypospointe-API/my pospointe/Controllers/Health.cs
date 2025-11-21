using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using my_pospointe.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_pospointe.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Health : ControllerBase
    {
        // GET: api/<Health>
        [HttpGet]
        public IActionResult Get()
        {

            HealthStatus healthStatus = new HealthStatus
            {
                Status = "Running",
                Timestamp = DateTime.Now,
                RunningVersion = clsConnections.version,
                SqlServeIP = clsConnections.server,
                sqlstatus = false,
                Errors = "None"
            };
            try {

              string  dbvalue = "170";
                string id = "1001";
                using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
                {
                   var bixn = context.TblBusinessInfos.FirstOrDefault(item => item.StoreId == id);
                    healthStatus.sqlstatus = true;
                }
            }
            catch (Exception ex)
            {
                healthStatus.sqlstatus = false;
                healthStatus.Errors = ex.Message;
            }


            return Ok(healthStatus);
        }

        public class HealthStatus
        {
            public string Status { get; set; }
            public DateTime Timestamp { get; set; }

            public string RunningVersion { get; set; }

            public string SqlServeIP { get; set; }

            public bool sqlstatus { get; set; }

            public string Errors { get; set; }
        }
    }
}
