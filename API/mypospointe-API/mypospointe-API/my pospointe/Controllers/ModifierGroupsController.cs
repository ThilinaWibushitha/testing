using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using my_pospointe.Models;
using System.Linq;

namespace my_pospointe.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ModifierGroupsController : ControllerBase
    {
        // GET: ModifierGroups
        [HttpGet]
        public IActionResult Get()
        {
            const string HeaderKeyName = "db";
            if (!Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue))
            {
                return BadRequest("Database header ('db') is missing.");
            }

            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                return Ok(context.TblModiferGroups.ToList());
            }
        }

        // GET: ModifierGroups/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            const string HeaderKeyName = "db";
            if (!Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue))
            {
                return BadRequest("Database header ('db') is missing.");
            }

            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                var modGroup = context.TblModiferGroups.FirstOrDefault(group => group.ModiferGroupId == id);
                if (modGroup == null)
                {
                    return NotFound($"Modifier group with ID {id} not found.");
                }

                return Ok(modGroup);
            }
        }

        [HttpPost]
        public IActionResult Add([FromBody] TblModiferGroup value)
        {
            const string HeaderKeyName = "db";
            if (!Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue))
            {
                return BadRequest("Database header ('db') is missing.");
            }

            if (value == null || string.IsNullOrWhiteSpace(value.GroupName))
            {
                return BadRequest("Invalid Modifier Group data.");
            }

            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                // Find the next available ID
                int nextId = 1;
                var existingIds = context.TblModiferGroups.Select(g => g.ModiferGroupId).OrderBy(id => id).ToList();

                for (int i = 1; i <= existingIds.Count + 1; i++)
                {
                    if (!existingIds.Contains(i))
                    {
                        nextId = i;
                        break;
                    }
                }

                // Create the new group
                TblModiferGroup newGroup = new TblModiferGroup
                {
                    ModiferGroupId = nextId, // Assign the next available ID
                    GroupName = value.GroupName,
                    Description = value.Description,
                    Status = value.Status,
                    PhromptName = value.PhromptName,
                    MaximumSelect = value.MaximumSelect,
                    Hide = value.Hide,
                    BtnColor = string.IsNullOrEmpty(value.BtnColor) ? "#FFFFFF" : value.BtnColor
                };

                context.TblModiferGroups.Add(newGroup);
                context.SaveChanges();

                return CreatedAtAction(nameof(GetById), new { id = newGroup.ModiferGroupId }, newGroup);
            }
        }


        // PUT: ModifierGroups/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] TblModiferGroup value)
        {
            const string HeaderKeyName = "db";
            if (!Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue))
            {
                return BadRequest("Database header ('db') is missing.");
            }

            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                var modGroup = context.TblModiferGroups.FirstOrDefault(group => group.ModiferGroupId == id);
                if (modGroup == null)
                {
                    return NotFound($"Modifier group with ID {id} not found.");
                }

                // Update the fields
                modGroup.GroupName = value.GroupName;
                modGroup.Description = value.Description;
                modGroup.Status = value.Status;
                modGroup.PhromptName = value.PhromptName;
                modGroup.MaximumSelect = value.MaximumSelect;
                modGroup.Hide = value.Hide;
                modGroup.BtnColor = string.IsNullOrEmpty(value.BtnColor) ? "#FFFFFF" : value.BtnColor;

                context.SaveChanges();
                return Ok("Modifier group updated successfully.");
            }
        }

        // DELETE: ModifierGroups/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            const string HeaderKeyName = "db";
            if (!Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue))
            {
                return BadRequest("Database header ('db') is missing.");
            }

            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                var modGroup = context.TblModiferGroups.FirstOrDefault(group => group.ModiferGroupId == id);
                if (modGroup == null)
                {
                    return NotFound($"Modifier group with ID {id} not found.");
                }

                context.TblModiferGroups.Remove(modGroup);
                context.SaveChanges();

                return Ok($"Modifier group with ID {id} deleted successfully.");
            }
        }
    }
}
