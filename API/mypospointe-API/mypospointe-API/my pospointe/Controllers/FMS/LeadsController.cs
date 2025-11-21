using Microsoft.AspNetCore.Mvc;
using my_pospointe.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_pospointe.Controllers.FMS
{
    [Route("[controller]")]
    [ApiController]
    public class LeadsController : ControllerBase
    {
        // GET: api/<LeadsController>
        [HttpGet("{franchsieid}")]
        public IEnumerable<FrLeadForm> Getallleadsoffran(Guid franchsieid)
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                var stores = context.FrLeadForms.Where(item => item.FranchiseId == franchsieid).ToList();
                return stores;
            }
        }

        // GET api/<LeadsController>/5
        [HttpGet("lead/{id}")]
        public FrLeadForm Getalead(Guid id)
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                return context.FrLeadForms.FirstOrDefault(item => item.Id == id);

            }
        }

        // POST api/<LeadsController>
        [HttpPost("{franchsieid}")]
        public IActionResult Post(Guid franchsieid,[FromBody] FrLeadForm value)
        {
            FrLeadForm _item = new FrLeadForm()
            {
                FranchiseId = franchsieid,
                Fname = value.Fname,
                Lname = value.Lname,
                Email = value.Email,
                Phone = value.Phone,
                NetWorth = value.NetWorth,
                CurrentOccup = value.CurrentOccup,
                BackNexp = value.BackNexp,
                CurrentFranchise = value.CurrentFranchise,
                PreferedCityState = value.PreferedCityState,
                OpenTime = value.OpenTime,
                Comment = value.Comment,
                CurrentStatus = value.CurrentStatus,
                CreatedDate = DateTime.Now,
                InComments = value.InComments


            };
            using (var context = new FranchiseManagementContext())
            {
                context.FrLeadForms.Add(_item);
                context.SaveChanges();
                var tasktemps = context.TaskTemplates.ToList();
                foreach (var item in tasktemps)
                {
                   Models.Task task = new Models.Task {
                   
                       FranchiseId=item.FranchiseId,
                       AccountId = _item.Id,
                       TaskId = item.Id,
                       TaskName = item.Name,
                       StepNo = item.StepNo,
                       Status = false,
                       TaskDescription = item.Description,
                       CreatedDate= DateTime.Now,
                       TemplateId = item.TemplateId,
                       PipelineId = item.PipelineId
                       
                   };
                    context.Tasks.Add(task);
                    context.SaveChanges();
                }
                return Ok(_item.Id.ToString());
            }
        }

        // PUT api/<LeadsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] FrLeadForm value)
        {
            using (var context = new FranchiseManagementContext())
            {
                var item = context.FrLeadForms.FirstOrDefault(item => item.Id == id);

                if (item != null)
                {
                    item.Fname = value.Fname;
                    item.Lname = value.Lname;
                    item.Email = value.Email;
                    item.Phone = value.Phone;
                    item.NetWorth = value.NetWorth;
                    item.CurrentOccup = value.CurrentOccup;
                    item.BackNexp = value.BackNexp;
                    item.CurrentFranchise = value.CurrentFranchise;
                    item.PreferedCityState = value.PreferedCityState;
                    item.CurrentStatus  = value.CurrentStatus;
                    item.OpenTime = value.OpenTime;
                    item.Comment =value.Comment;
                    item.InComments = value.InComments;
                    context.SaveChanges();
                    return Ok();
                
                }

                else
                {
                    return NotFound();
                }
            }
        }

        // DELETE api/<LeadsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
