using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using my_pospointe.Models;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_pospointe.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        // GET: api/<DepartmentsController>
        [HttpGet]
        public IEnumerable<TblDepartment> Get()
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);

            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                return context.TblDepartments.ToList();

            }
        }

        [HttpGet("modifergroups")]
        public IEnumerable<TblModiferGroup> Getmodgroups()
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);

            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                return context.TblModiferGroups.ToList();

            }
        }

        // GET api/<DepartmentsController>/5
        [HttpGet("{id}")]
        public TblDepartment Get(string id)
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                return context.TblDepartments.FirstOrDefault(item => item.DeptId == id);

            }
        }

        // POST api/<DepartmentsController>
        [HttpPost]
        public string Post([FromBody] TblDepartment value)
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);

            using (var context2 = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                var maxAge = context2.TblDepartments.Max(p => p.ListOrder);
                int orderid = Convert.ToInt32(maxAge) + 1;
                TblDepartment _item = new TblDepartment()
                {
                    DeptId = value.DeptId,
                    DeptName = value.DeptName,
                    PicturePath = value.PicturePath,
                    Visible = value.Visible,
                    BtnColor = value.BtnColor,
                    NameVisible = value.NameVisible,
                    PictureVisible = value.PictureVisible,
                    ListOrder = orderid,
                    ShowinOrderTablet = false

                };


                context2.TblDepartments.Add(_item);
                context2.SaveChanges();
                return _item.DeptId.ToString();
            }
        }

        // PUT api/<DepartmentsController>/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] TblDepartment value)
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);

            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                var item = context.TblDepartments.FirstOrDefault(item => item.DeptId == id);
                if (item != null)
                {


                    item.DeptName = value.DeptName;
                    item.Visible = value.Visible;
                    item.NameVisible = value.NameVisible;
                    item.PictureVisible = value.PictureVisible;
                    item.PicturePath = value.PicturePath;
                    context.SaveChanges();

                }


            }
        }

        [HttpPut("modifergroups/{id}")]
        public IActionResult UpdateModifierGroup(int id, [FromBody] TblModiferGroup value)
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);

            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                var modGroup = context.TblModiferGroups.FirstOrDefault(group => group.ModiferGroupId == id);
                if (modGroup == null)
                {
                    return NotFound("Modifier group not found.");
                }

                
                modGroup.GroupName = value.GroupName;
                modGroup.Status = value.Status;
                modGroup.PhromptName = value.PhromptName;
                modGroup.MaximumSelect = value.MaximumSelect;
                modGroup.Hide = value.Hide;

                modGroup.BtnColor = string.IsNullOrEmpty(value.BtnColor) ? "#FFFFFF" : value.BtnColor;

                // Save changes
                context.SaveChanges();

                return Ok("Modifier group updated successfully.");
            }
        }


        //// DELETE api/<DepartmentsController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}

