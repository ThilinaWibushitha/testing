using Microsoft.AspNetCore.Mvc;
using my_pospointe.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_pospointe.Controllers.FMS
{
    [Route("[controller]")]
    [ApiController]
    public class ValueBindingController : ControllerBase
    {
        // GET: api/<ValueBindingController>
        [HttpGet]
        public IEnumerable<ValueBinding> Get()
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                var alltemplates = context.ValueBindings.ToList();
                return alltemplates;
            }
        }

        // GET api/<ValueBindingController>/5
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            using (var context = new FranchiseManagementContext())
            { 
               var item = context.ValueBindings.FirstOrDefault(x=>x.Id == id);
                if (item != null)
                {
                     return Ok( item);
                }
                else { 
                  return NotFound();
                }
            }
        }

        // POST api/<ValueBindingController>
        [HttpPost]
        public void Post([FromBody] ValueBinding value)
        {
            ValueBinding v = new ValueBinding
            {
                Text = value.Text,
                Data = value.Data,
                CreatedDate = DateTime.Now,
                Status = value.Status
            };

            using (var context = new FranchiseManagementContext())
            {
                context.ValueBindings.Add(v);
                context.SaveChanges();
            }

        }

        // PUT api/<ValueBindingController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<ValueBindingController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
