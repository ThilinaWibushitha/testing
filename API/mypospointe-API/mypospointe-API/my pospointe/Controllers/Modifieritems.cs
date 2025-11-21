using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using my_pospointe.Models;
using NuGet.Configuration;
using System.Linq;

namespace my_pospointe.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Modifieritems : Controller
    {
        // GET: ModifierItems
        [HttpGet]
        public IActionResult GetModifierItems()
        {
            const string HeaderKeyName = "db";


            if (!Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue))
            {
                return BadRequest("Database header ('db') is missing.");
            }

            try
            {

                using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
                {

                    var modifierItems = context.TblItems
                        .Where(item => item.IsModifer == true)
                        .Select(item => new
                        {
                            item.ItemId,
                            item.ItemName,
                            item.ItemDeptId,
                            item.ItemPrice,
                            item.Tax1Status,
                            item.Picturepath,
                            item.PricePrompt,
                            item.BtnColor,
                            item.Visible,
                            item.EnableName,
                            item.EnablePicture,
                            item.ListOrder,
                            item.IsKot,
                            item.IsKot2,
                            item.IsModifer
                        })
                        .ToList();


                    return Ok(modifierItems);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }



        // GET: ModifierItems/{id}
        [HttpGet("{id}")] //request to get by item name
        public IActionResult GetByName(string id) //id is a string because it retrieves the details by the item name
        {
            const string HeaderKeyName = "db";
            if (!Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue))
            {
                return BadRequest("Database header ('db') is missing.");
            }

            try
            {
                using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
                {
                    var moditem = context.TblModifersofItems.Where(mItem => mItem.ItemId == id).ToList();
                    if (moditem == null)
                    {
                        return NotFound($"Modifier item with Id {id} not found.");
                    }

                    return Ok(moditem);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }
        
        //[HttpGet("{id}")] //request to get by modifer id
        //public IActionResult GetById(int id)
        //{
        //    const string HeaderKeyName = "db";
        //    if (!Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue))
        //    {
        //        return BadRequest("Database header ('db') is missing.");
        //    }

        //    try
        //    {
        //        using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
        //        {
        //            var moditem = context.TblModifersofItems.Where(mItem => mItem.Id == id).ToList();
        //            if (moditem == null)
        //            {
        //                return NotFound($"Modifier item with Id {id} not found.");
        //            }

        //            return Ok(moditem);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Internal server error: " + ex.Message);
        //    }

        //}

        // PUT: Modifieritems/{id}
        [HttpPut("{Id}")] 
        public IActionResult Update(int Id, [FromBody] TblModifersofItem value)
        {
            const string HeaderKeyName = "db";
            if (!Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue))
            {
                return BadRequest("Database header ('db') is missing.");
            }

            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                var moditem = context.TblModifersofItems.FirstOrDefault(item => item.Id == Id);
                if (moditem == null)
                {
                    return NotFound($"Modifier item with ID {Id} not found.");
                }

                // Update the fields
                moditem.ItemId = value.ItemId;
                moditem.ModiferGroupId = value.ModiferGroupId;
                moditem.MaximumSelect = value.MaximumSelect;
                moditem.Forced = value.Forced;
                

                context.SaveChanges();
                return Ok("Modifier item updated successfully.");
            }
        }

        // POST api/<ModifierItems>
        [HttpPost]
        public IActionResult Add([FromBody] TblModifersofItem value)
        {
            const string HeaderKeyName = "db";
            if (!Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue))
            {
                return BadRequest("Database header ('db') is missing.");
            }

            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                // Find the next available ID
                int nextId = 1;
                var existingIds = context.TblModifersofItems.Select(g => g.Id).OrderBy(id => id).ToList();

                for (int i = 1; i <= existingIds.Count + 1; i++)
                {
                    if (!existingIds.Contains(i))
                    {
                        nextId = i;
                        break;
                    }
                }

                // Create the new Modifer item
                TblModifersofItem newModItem = new TblModifersofItem
                {
                    Id = nextId, // Assign the next available ID
                    ItemId = value.ItemId,
                    ModiferGroupId = value.ModiferGroupId,
                    MaximumSelect = value.MaximumSelect,
                    Forced = value.Forced,
                };

                context.TblModifersofItems.Add(newModItem);
                context.SaveChanges();

                return Ok("Added Succesfully");
                //return CreatedAtAction(nameof(GetById), new { id = newModItem.Id }, newModItem);
            }
        }

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
                var modItem = context.TblModifersofItems.FirstOrDefault(group => group.Id == id);
                if (modItem == null)
                {
                    return NotFound($"Modifier group with ID {id} not found.");
                }

                context.TblModifersofItems.Remove(modItem);
                context.SaveChanges();

                return Ok($"Modifier item with ID {id} deleted successfully.");
            }
        }
    }
}
