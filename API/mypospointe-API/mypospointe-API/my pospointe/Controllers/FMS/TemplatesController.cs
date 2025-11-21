using Microsoft.AspNetCore.Mvc;
using my_pospointe.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_pospointe.Controllers.FMS
{
    [Route("[controller]")]
    [ApiController]
    public class TemplatesController : ControllerBase
    {
        // GET: api/<TemplatesController>
        [HttpGet("bytype/{type}")]
        public IEnumerable<Template> Get(string type)
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                var alltemplates = context.Templates.Where(x=> x.Type == type).ToList();
                return alltemplates;
            }
        }

        // GET api/<TemplatesController>/5
        [HttpGet("{id}")]
        public Template Get(Guid id)
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                var template = context.Templates.FirstOrDefault(x => x.Id == id);
                return template;
            }
        }

        // POST api/<TemplatesController>
        [HttpPost]
        public IActionResult Post([FromBody] Template value)
        {
            Template temp = new Template
            {
                FracnhiseId = value.FracnhiseId,
                Name = value.Name,
                Type = value.Type,
                Datatext = value.Datatext,
                Status = value.Status,
                CreatedDate = DateTime.Now,
                LastDate = DateTime.Now,
                EveryOwnersSign = value.EveryOwnersSign,
                Mysignreq = value.Mysignreq


            };

            using (var context = new FranchiseManagementContext())
            {
                context.Templates.Add(temp);
                context.SaveChanges();
                return Ok();
            
            }

        }

        // PUT api/<TemplatesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] Template value)
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                var template = context.Templates.FirstOrDefault(x => x.Id == id);
                if (template != null)
                {
                    template.Name = value.Name;
                    template.Datatext = value.Datatext;
                    template.Status = value.Status;
                    template.LastDate = DateTime.Now;
                    template.EveryOwnersSign = value.EveryOwnersSign;
                    template.Mysignreq = value.Mysignreq;
                    context.SaveChanges();
                    return Ok();
                }


                else
                {
                    return NotFound();
                }

            }
        }

        // DELETE api/<TemplatesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
