using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using my_pospointe.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_pospointe.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        // GET: api/<BusinessController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<BusinessController>/5
        [HttpGet]
        public TblBusinessInfo Get()
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            string id = "1001";
            using (var context = new _167Context("Server="+clsConnections.server+","+clsConnections.port+";Initial Catalog="+ dbvalue + ";Persist Security Info=False;User ID="+clsConnections.username+";Password="+clsConnections.password+";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                return context.TblBusinessInfos.FirstOrDefault(item => item.StoreId == id);

            }
        }

        [HttpGet("taxrate")]
        public TblTaxRate GetTax()
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            string id = "1001";
            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                return context.TblTaxRates.FirstOrDefault(item => item.TaxNo == 1);

            }
        }

        [HttpPut("taxrate")]
        public TblTaxRate UpdateTax([FromBody] TblTaxRate value)
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            string id = "1001";
            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
               var tax = context.TblTaxRates.FirstOrDefault(item => item.TaxNo == 1);
                
                  tax.TaxRate = value.TaxRate;
                    context.SaveChanges();
                    return tax;
                
            }
        }
        // PUT api/<BusinessController>/5
        [HttpPut]
        public void Put([FromBody] TblBusinessInfo value)
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            string id = "1001";
            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                var item = context.TblBusinessInfos.FirstOrDefault(item => item.StoreId == id);
                if (item != null)
                {


                    item.BusinessName = value.BusinessName;
                    item.BusinessAddress = value.BusinessAddress;
                    item.CityStatezip = value.CityStatezip;
                    item.BusinessPhone = value.BusinessPhone;
                    item.BusinessEmail = value.BusinessEmail;
                    item.OwnerName = value.OwnerName;
                    item.OwnerPhone = value.OwnerPhone;
                    item.Footer1 = value.Footer1;
                    item.Footer2 = value.Footer2;
                    item.Footer3 = value.Footer3;
                    item.Footer4 = value.Footer4;


                    context.SaveChanges();

                }


            }

        }

       
    }
}
