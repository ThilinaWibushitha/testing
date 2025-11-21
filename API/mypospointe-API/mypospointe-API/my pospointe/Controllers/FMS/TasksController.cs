using Microsoft.AspNetCore.Mvc;
using my_pospointe.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_pospointe.Controllers.FMS
{
    [Route("[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        // GET: api/<TasksController>
        [HttpGet]
        public IEnumerable<TaskTemplate> Get()
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                var stores = context.TaskTemplates.ToList();
                return stores;
            }
        }

        [HttpGet("tasksofanaccount/{id}")]
        public IEnumerable<Models.Task> Get(Guid id)
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                var stores = context.Tasks.Where(x => x.AccountId == id).ToList();
                return stores;
            }
        }

        [HttpGet("atask/{id}")]
        public Models.Task Getatask(Guid id)
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                var stores = context.Tasks.FirstOrDefault(x => x.Id == id);
                return stores;
            }
        }

        [HttpPost("completetask/{id}")]
        public IActionResult updateatask(Guid id)
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                var stores = context.Tasks.FirstOrDefault(x => x.Id == id);
                if (stores != null)
                {
                    stores.Status = true;
                    stores.CompletedDate = DateTime.Now;
                    context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
        }


        [HttpGet("bypipeline/{id}")]
        public IEnumerable<TaskTemplate> Getbypipe(Guid id)
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                var stores = context.TaskTemplates.Where(x=> x.PipelineId == id).ToList();
                return stores;
            }
        }

        // GET api/<TasksController>/5
        [HttpGet("{id}")]
        public TaskTemplate Getonly(Guid id)
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                return context.TaskTemplates.FirstOrDefault(item => item.Id == id);

            }
        }

        // POST api/<TasksController>
        [HttpPost]
        public void Post([FromBody] TaskTemplate value)
        {
            TaskTemplate t = new TaskTemplate
            {
                FranchiseId = value.FranchiseId,
                PipelineId = value.PipelineId,
                Name = value.Name,
                Description = value.Description,
                Status = value.Status,
                Required = value.Required,
                Enabled = value.Enabled,
                Type = value.Type,
                Automate = value.Automate,
                AutomatedDate = value.AutomatedDate,
                StepNo = value.StepNo,
                TemplateId = value.TemplateId,
            };

            using (var context = new FranchiseManagementContext())
            {
               context.TaskTemplates.Add(t);
               context.SaveChanges();

            }
        }

        // PUT api/<TasksController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] TaskTemplate value)
        {
            using (var context = new FranchiseManagementContext())
            {
                //return new string[] { "value1", "value2" };
                var template = context.TaskTemplates.FirstOrDefault(x => x.Id == id);
                if (template != null)
                {
                    template.Name = value.Name;
                    template.Description = value.Description;
                    template.Status = value.Status;
                    template.Required = value.Required;
                    template.Enabled = value.Enabled;
                    template.Type = value.Type;
                    template.Automate = value.Automate;
                    template.AutomatedDate = value.AutomatedDate;
                    template.StepNo = value.StepNo;
                    template.TemplateId = value.TemplateId;
                    context.SaveChanges();
                    return Ok();
                }


                else
                {
                    return NotFound();
                }

            }
        }

        // DELETE api/<TasksController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
