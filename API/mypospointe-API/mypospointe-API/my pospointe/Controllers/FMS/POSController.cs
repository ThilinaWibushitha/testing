using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using my_pospointe.Models;
using my_pospointe.Models.DynamicColumn;
using my_pospointe.Services.DynamicColumn.CRUD;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_pospointe.Controllers.FMS
{
    [Route("[controller]")]
    [ApiController]
    public class POSController(ICrudTbl_Items crudTbl) : ControllerBase
    {
        private readonly ICrudTbl_Items _crudTbl = crudTbl;

        // GET api/<POSController>/5
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                var useres = context.TblUsers.ToList();
                //var items = context.TblItems.ToList();
                var departments = context.TblDepartments.ToList();
                var modifergroups = context.TblModiferGroups.ToList();
                var modiferitems = context.TblModifersofItems.ToList();
                var businessinfo = context.TblBusinessInfos.ToList();


                var items = await _crudTbl.GetItemsAsync(context);

                var itemsTbl = new List<TblItem>();

                foreach (var item in items) {
                    itemsTbl.Add(new TblItem {
                        Brand = item.Brand,
                        BtnColor = item.BtnColor,
                        CasePrice = item.CasePrice,
                        Cost = item.Cost,
                        Countthis = item.Countthis,
                        CreatedDate = item.CreatedDate,
                        EnableName = item.EnableName,
                        EnablePicture = item.EnablePicture,
                        FoodStampable = item.FoodStampable,
                        Idcheck = item.Idcheck,
                        IsDeleted = item.IsDeleted,
                        IsKot = item.IsKot,
                        IsKot2 = item.IsKot2,
                        IsModifer = item.IsModifer,
                        ItemDeptId = item.ItemDeptId,
                        ItemId = item.ItemId,
                        ItemName = item.ItemName,
                        ItemPrice = item.ItemPrice,
                        LastSold = item.LastSold,
                        ListOrder = item.ListOrder,
                        LoyalityCredit = item.LoyalityCredit,
                        ManagersOnly = item.ManagersOnly,
                        OnlineImagelink = item.OnlineImagelink,
                        OnlinePrice = item.OnlinePrice,
                        Picturepath = item.Picturepath,
                        PricePrompt = item.PricePrompt,
                        PromptDescription = item.PromptDescription,
                        Qty = item.Qty,
                        SellOnline = item.SellOnline,
                        ShowInKitchenDisplay = item.ShowInKitchenDisplay,
                        Tax1Status = item.Tax1Status,
                        ItemDescription = item.ItemDescription,
                        IteminCase = item.IteminCase,
                        Lastimported = item.Lastimported,
                        Soldby= item.Soldby,
                        Visible = item.Visible
                    });
                }


                MainResponse response = new MainResponse
                {

                    users = useres,
                    items = itemsTbl,
                    departments = departments,
                    modiferGroups = modifergroups,
                    modifersofItems = modiferitems,
                    businessInfo = businessinfo
                };

                return Ok(response);

            }

        }

        // GET api/<POSController>/5
        [HttpPut("copy/from/{fromdb}/to/{todb}")]
        public async Task<IActionResult> UpdateData(string fromdb, string todb)
        {
            MainResponse response = new MainResponse();
            //const string HeaderKeyName = "db";
            //Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + fromdb + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                var useres = context.TblUsers.ToList();
                var items = context.TblItems.ToList();
                var departments = context.TblDepartments.ToList();
                var modifergroups = context.TblModiferGroups.ToList();
                var modiferitems = context.TblModifersofItems.ToList();
                var businessinfo = context.TblBusinessInfos.ToList();
                var taxrate = context.TblTaxRates.ToList();
                response.users = useres;
                response.items = items;
                response.departments = departments;
                response.modiferGroups = modifergroups;
                response.modifersofItems = modiferitems;
                response.businessInfo = businessinfo;
                response.taxRates = taxrate;


            }

            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + todb + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                var fromDbContext = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + fromdb + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True");
                var toDbContext = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + todb + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True");

                // Clear existing data
                try
                {
                    context.TblUsers.RemoveRange(context.TblUsers);
                }
                catch
                {

                }
                try
                {
                    context.TblItems.RemoveRange(context.TblItems);
                }
                catch
                {

                }
                try
                {
                    context.TblDepartments.RemoveRange(context.TblDepartments);
                }
                catch
                {

                }
                try
                {
                    context.TblModiferGroups.RemoveRange(context.TblModiferGroups);
                }
                catch
                {

                }
                try
                {
                    context.TblModifersofItems.RemoveRange(context.TblModifersofItems);
                }
                catch
                {

                }
                try
                {
                    context.TblBusinessInfos.RemoveRange(context.TblBusinessInfos);
                }
                catch
                {

                }
                try
                {
                    context.TblTaxRates.RemoveRange(context.TblTaxRates);
                }
                catch
                {

                }
                context.SaveChanges();
                // Add new data
                context.TblUsers.AddRange(response.users);
                //context.TblItems.AddRange(response.items);
                var res = await _crudTbl.AddItemsAsyncCopyDB(fromDbContext, toDbContext, response.items);

                if(res ==0) return BadRequest("Error while copying items data from " + fromdb + " to " + todb);
                else if (res == -1) return BadRequest($"Operation aborted to prevent data loss: Source database '{fromdb}' contains a 'Description' column, but target database '{todb}' does not.");

                context.TblDepartments.AddRange(response.departments);
                context.TblModiferGroups.AddRange(response.modiferGroups);
                context.TblModifersofItems.AddRange(response.modifersofItems);
                context.TblBusinessInfos.AddRange(response.businessInfo);
                context.TblTaxRates.AddRange(response.taxRates);
                // Save changes
                context.SaveChanges();
            }
            return Ok("Data copied successfully from " + fromdb + " to " + todb);

        }


        public class MainResponse()
        {
            public List<TblUser> users { get; set; }

            public List<TblItem> items { get; set; }
            public List<TblDepartment> departments { get; set; }
            public List<TblModiferGroup> modiferGroups { get; set; }

            public List<TblModifersofItem> modifersofItems { get; set; }
            public List<TblBusinessInfo> businessInfo { get; set; }
            public List<TblTaxRate> taxRates { get; set; }



        }

    }
}
