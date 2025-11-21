using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_pospointe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        // POST api/<LoginController>
        [HttpPost]
        public void Post([FromBody] dbname value)
        {
          
        }

        public class dbname
            {
              public string db { get; set; }
            }

    }
}
