using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using my_pospointe.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_pospointe.Controllers.FMS
{
	[Route("[controller]")]
	[ApiController]
	public class TaxRateController : ControllerBase
	{
		// GET: api/<TaxRateController>
		[HttpGet]
		public IEnumerable<TblTaxRate> Get()
		{
			const string HeaderKeyName = "db";
			Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
			using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
			{
				return context.TblTaxRates.ToList();

			}
		}
		// GET api/<TaxRateController>/5
		/*[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}

		// POST api/<TaxRateController>
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		// PUT api/<TaxRateController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/<TaxRateController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}*/
	}
}
