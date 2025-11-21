using Microsoft.AspNetCore.Mvc;
using my_pospointe.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_pospointe.Controllers.FMS
{
    [Route("[controller]")]
    [ApiController]
    public class PipelineController : ControllerBase
    {
        // GET: api/<PipelineController>
        [HttpGet("bytype/{type}")]
        public IEnumerable<Pipleline> Get(string type)
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                var pipline = context.Piplelines.OrderBy(x=> x.StepNo).Where(x => x.Type == type).ToList();
                return pipline;
            }
        }

        // GET api/<PipelineController>/5
        [HttpGet("{id}")]
        public Pipleline Get(Guid id)
        {
            using (var context = new FranchiseManagementContext())
            {
                return context.Piplelines.FirstOrDefault(x=> x.Id == id);
            }
        }

        // POST api/<PipelineController>
        [HttpPost ("{type}")]
        public IActionResult Post(string type,[FromBody] Pipleline value)
        {
            using (var context = new FranchiseManagementContext())
            {
                var maxListNo = context.Piplelines.Where(x => x.Type == type).Any() ? context.Piplelines.Where(x => x.Type == type).Max(item => item.StepNo) : 0;
                int newid = maxListNo + 1 ?? 1;

                Pipleline p = new Pipleline { 
                      FranchiseId = value.FranchiseId,
                      Type = type,
                      Name = value.Name,
                      Description = value.Description,
                      CreatedDate = DateTime.Now,
                      Status = value.Status,
                      StepNo = newid
                };

                context.Piplelines.Add(p);
                context.SaveChanges();
                return Ok();
            }
        }

        // PUT api/<PipelineController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] Pipleline value)
        {
            using (var context = new FranchiseManagementContext())
            {
                var it = context.Piplelines.FirstOrDefault(x => x.Id == id);
                if (it != null)
                { 
                    it.Name = value.Name;
                    it.Description = value.Description;
                    it.Status = value.Status;
                    it.StepNo = value.StepNo;
                    context.SaveChanges();
                    return Ok();
                }
                else
                { return NotFound(); }
            }
        }

        // DELETE api/<PipelineController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
