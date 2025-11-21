using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using my_pospointe.Models;
using my_pospointe.Models.DynamicColumn;
using my_pospointe.Services.DynamicColumn.CRUD;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_pospointe.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ItemsController(ICrudTbl_Items crudTbl_Items) : ControllerBase
    {
        private readonly ICrudTbl_Items _crudTbl_Items = crudTbl_Items;

        // GET: api/<ItemsController>
        [HttpGet]
        public async Task<List<TblItem>> Get()
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {

                var res =  await _crudTbl_Items.GetTableItemsAsyncNotIsModifier(context);

                var itemsTbl = new List<TblItem>();

                foreach (var item in res)
                {
                    itemsTbl.Add(new TblItem
                    {
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
                        Soldby = item.Soldby,
                        Visible = item.Visible
                    });
                }

                return itemsTbl;

            }
        }

        [HttpGet ("modifers")]
        public async Task<List<TblItem>> Getmod()
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {

                var res = await _crudTbl_Items.GetTableItemsAsyncIsModifier(context);

                var itemsTbl = new List<TblItem>();

                foreach (var item in res)
                {
                    itemsTbl.Add(new TblItem
                    {
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
                        Soldby = item.Soldby,
                        Visible = item.Visible
                    });
                }


                return itemsTbl;

            }
        }

        // GET api/<ItemsController>/5
        [HttpGet("{id}")]
        public async Task<TblItem> Get(string id)
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                var item = await _crudTbl_Items.GetItemByIdAsync(context, id);

                if (item != null)
                {
                    return new TblItem
                    {
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
                        Soldby = item.Soldby,
                        Visible = item.Visible
                    };
                }

                return null;

            }
        }

        // POST api/<ItemsController>
        [HttpPost]
        public async Task<string> Post([FromBody] TblItem value)
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            using (var context2 = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                var maxAge = context2.TblItems.Max(p => p.ListOrder);
                int orderid = Convert.ToInt32(maxAge) + 1;

                value.ListOrder = orderid;
                value.IsDeleted = false;
                value.CreatedDate = DateTime.Now;
                value.BtnColor = "";

                var newItemDto = new ItemDto
                {
                    BtnColor = value.BtnColor,
                    Brand = value.Brand,
                    CasePrice = value.CasePrice,
                    Cost = value.Cost,
                    Countthis = value.Countthis,
                    CreatedDate = value.CreatedDate,
                    EnableName = value.EnableName,
                    EnablePicture = value.EnablePicture,
                    FoodStampable = value.FoodStampable,
                    Idcheck = value.Idcheck,
                    IsDeleted = value.IsDeleted,
                    IsKot = value.IsKot,
                    IsKot2 = value.IsKot2,
                    IsModifer = value.IsModifer,
                    ItemDeptId = value.ItemDeptId,
                    ItemName = value.ItemName,
                    ItemPrice = value.ItemPrice,
                    LastSold = value.LastSold,
                    ListOrder = value.ListOrder,
                    LoyalityCredit = value.LoyalityCredit,
                    ManagersOnly = value.ManagersOnly,
                    OnlineImagelink = value.OnlineImagelink,
                    OnlinePrice = value.OnlinePrice,
                    Picturepath = value.Picturepath,
                    PricePrompt = value.PricePrompt,
                    PromptDescription = value.PromptDescription,
                    Qty = value.Qty,
                    SellOnline = value.SellOnline,
                    ShowInKitchenDisplay = value.ShowInKitchenDisplay,
                    Tax1Status = value.Tax1Status,
                    ItemDescription = value.ItemDescription,
                    IteminCase = value.IteminCase,
                    Lastimported = value.Lastimported,
                    Soldby = value.Soldby,
                    Visible = value.Visible,
                    ItemId = value.ItemId

                };

                var res = await _crudTbl_Items.CreateItemAsync(context2,newItemDto);

                if (res == null) return null;

                return res.ItemId.ToString();
            }
        }


        // PUT api/<ItemsController>/5
        [HttpPut("{id}")]
        public async void Put(string id, [FromBody] TblItem value)
        {
            const string HeaderKeyName = "db";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues dbvalue);
            using (var context = new _167Context("Server=" + clsConnections.server + "," + clsConnections.port + ";Initial Catalog=" + dbvalue + ";Persist Security Info=False;User ID=" + clsConnections.username + ";Password=" + clsConnections.password + ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True"))
            {
                var item = await _crudTbl_Items.GetItemByIdAsync(context, id);

                var updateItemDto = new ItemDto
                {
                    BtnColor = value.BtnColor,
                    Brand = value.Brand,
                    CasePrice = value.CasePrice,
                    Cost = value.Cost,
                    Countthis = value.Countthis,
                    CreatedDate = value.CreatedDate,
                    EnableName = value.EnableName,
                    EnablePicture = value.EnablePicture,
                    FoodStampable = value.FoodStampable,
                    Idcheck = value.Idcheck,
                    IsDeleted = value.IsDeleted,
                    IsKot = value.IsKot,
                    IsKot2 = value.IsKot2,
                    IsModifer = value.IsModifer,
                    ItemDeptId = value.ItemDeptId,
                    ItemName = value.ItemName,
                    ItemPrice = value.ItemPrice,
                    LastSold = value.LastSold,
                    ListOrder = value.ListOrder,
                    LoyalityCredit = value.LoyalityCredit,
                    ManagersOnly = value.ManagersOnly,
                    OnlineImagelink = value.OnlineImagelink,
                    OnlinePrice = value.OnlinePrice,
                    Picturepath = value.Picturepath,
                    PricePrompt = value.PricePrompt,
                    PromptDescription = value.PromptDescription,
                    Qty = value.Qty,
                    SellOnline = value.SellOnline,
                    ShowInKitchenDisplay = value.ShowInKitchenDisplay,
                    Tax1Status = value.Tax1Status,
                    ItemDescription = value.ItemDescription,
                    IteminCase = value.IteminCase,
                    Lastimported = value.Lastimported,
                    Soldby = value.Soldby,
                    Visible = value.Visible,
                    ItemId = value.ItemId

                };



                var res = await _crudTbl_Items.UpdateItemAsync(context, id, updateItemDto);

            }
        }

        //// DELETE api/<ItemsController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
