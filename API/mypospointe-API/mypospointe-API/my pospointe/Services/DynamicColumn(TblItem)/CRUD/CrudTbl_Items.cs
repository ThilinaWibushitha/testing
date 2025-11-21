using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using my_pospointe.Models;
using my_pospointe.Models.DynamicColumn;
using System.Collections.Concurrent;
using System.Data;
using System.Linq;

namespace my_pospointe.Services.DynamicColumn.CRUD
{
    public class CrudTbl_Items(ISchema schema) : ICrudTbl_Items
    {
        private readonly ISchema _schema = schema;
        private readonly ConcurrentDictionary<string, bool> _columnPresenceCache = new();


        public async Task<List<ItemDto>> GetItemsAsync(DbContext dbContext)
        {
            var dbKey = dbContext.Database.GetDbConnection().ConnectionString;

            if (!_columnPresenceCache.TryGetValue(dbKey, out var hasDescription))
            {
                hasDescription = await _schema.ColumnExistsAsync(dbContext, "Tbl_Items", "ItemDescription");
                _columnPresenceCache[dbKey] = hasDescription;
            }

            var sql = hasDescription
                ? @"SELECT *
       FROM Tbl_Items"
                : @"SELECT 
        ItemID,
        ItemName,
        ItemDeptID,
        ItemPrice,
        Tax1_Status,
        Picturepath,
        Price_Prompt,
        BtnColor,
        Visible,
        EnableName,
        EnablePicture,
        ListOrder,
        isKOT,
        isKOT2,
        IsModifer,
        IsDeleted,
        Prompt_Description,
        ShowInKitchenDisplay,
        LoyalityCredit,
        QTY,
        LastSold,
        OnlinePrice,
        IDCheck,
        FoodStampable,
        ManagersOnly,
        SellOnline,
        Countthis,
        lastimported,
        CreatedDate,
        Cost,
        Brand,
        IteminCase,
        CasePrice,
        Soldby,
        OnlineImagelink,
        NULL AS ItemDescription
       FROM Tbl_Items";



        var res = await dbContext.Set<TblItem>()
                .FromSqlRaw(sql)
                .ToListAsync();

            return res.Select(item => new ItemDto
            {
                Brand = item.Brand,
                CasePrice = item.CasePrice,
                Cost = item.Cost,
                BtnColor = item.BtnColor,
                Countthis = item.Countthis,
                CreatedDate = item.CreatedDate,
                EnableName = item.EnableName,
                EnablePicture = item.EnablePicture,
                FoodStampable = item.FoodStampable,
                Idcheck = item.Idcheck,
                IsDeleted = item.IsDeleted,
                PromptDescription = item.PromptDescription,
                IsKot = item.IsKot,
                IsKot2 = item.IsKot2,
                IsModifer = item.IsModifer,
                ItemDescription = item.ItemDescription,
                IteminCase = item.IteminCase,
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                ItemDeptId = item.ItemDeptId,
                ItemPrice = item.ItemPrice,
                Lastimported = item.Lastimported,
                LastSold = item.LastSold,
                ListOrder = item.ListOrder,
                LoyalityCredit = item.LoyalityCredit,
                ManagersOnly = item.ManagersOnly,
                OnlinePrice = item.OnlinePrice,
                Picturepath = item.Picturepath,
                PricePrompt = item.PricePrompt,
                Qty = item.Qty,
                SellOnline = item.SellOnline,
                Tax1Status = item.Tax1Status,
                Visible = item.Visible,
                Soldby = item.Soldby
            }).ToList();

        }

        public async Task<int> AddItemsAsyncCopyDB(DbContext fromdbContext, DbContext todbContext ,List<TblItem> items)
        {
            var dbKeyFrom = fromdbContext.Database.GetDbConnection().ConnectionString;
            var dbKeyTo = todbContext.Database.GetDbConnection().ConnectionString;

            int result;
            int totalItems = 0;


            if (!_columnPresenceCache.TryGetValue(dbKeyFrom, out var hasDescriptionFrom))
            {
                hasDescriptionFrom = await _schema.ColumnExistsAsync(fromdbContext, "Tbl_Items", "ItemDescription");
                _columnPresenceCache[dbKeyFrom] = hasDescriptionFrom;
            }

            if (!_columnPresenceCache.TryGetValue(dbKeyTo, out var hasDescriptionTo))
            {
                hasDescriptionTo = await _schema.ColumnExistsAsync(todbContext, "Tbl_Items", "ItemDescription");
                _columnPresenceCache[dbKeyTo] = hasDescriptionTo;
            }


            if(hasDescriptionFrom && hasDescriptionTo)
            {
                var sql1 = @"
                    INSERT INTO Tbl_Items (
                        ItemID, ItemName, ItemDeptID, ItemPrice, Tax1_Status, Picturepath, Price_Prompt, BtnColor, Visible,
                        EnableName, EnablePicture, ListOrder, isKOT, isKOT2, IsModifer, IsDeleted, Prompt_Description,
                        ShowInKitchenDisplay, LoyalityCredit, QTY, LastSold, OnlinePrice, IDCheck, FoodStampable,
                        ManagersOnly, SellOnline, Countthis, lastimported, CreatedDate, Cost, Brand, IteminCase,
                        CasePrice, Soldby, OnlineImagelink, ItemDescription
                    ) VALUES (
                        @ItemID, @ItemName, @ItemDeptID, @ItemPrice, @Tax1_Status, @Picturepath, @Price_Prompt, @BtnColor, @Visible,
                        @EnableName, @EnablePicture, @ListOrder, @isKOT, @isKOT2, @IsModifer, @IsDeleted, @Prompt_Description,
                        @ShowInKitchenDisplay, @LoyalityCredit, @QTY, @LastSold, @OnlinePrice, @IDCheck, @FoodStampable,
                        @ManagersOnly, @SellOnline, @Countthis, @lastimported, @CreatedDate, @Cost, @Brand, @IteminCase,
                        @CasePrice, @Soldby, @OnlineImagelink, @ItemDescription
                    )";

                using var command1 = todbContext.Database.GetDbConnection().CreateCommand();
                command1.CommandText = sql1;
                command1.CommandType = CommandType.Text;

                await todbContext.Database.OpenConnectionAsync();

                foreach (var item in items)
                {
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ItemID", item.ItemId ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ItemName", item.ItemName ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ItemDeptID", item.ItemDeptId ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ItemPrice", item.ItemPrice ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Tax1_Status", item.Tax1Status ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Picturepath", item.Picturepath ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Price_Prompt", item.PricePrompt ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@BtnColor", item.BtnColor ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Visible", item.Visible ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@EnableName", item.EnableName ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@EnablePicture", item.EnablePicture ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ListOrder", item.ListOrder ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@isKOT", item.IsKot ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@isKOT2", item.IsKot2 ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@IsModifer", item.IsModifer ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@IsDeleted", item.IsDeleted ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Prompt_Description", item.PromptDescription ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ShowInKitchenDisplay", item.ShowInKitchenDisplay ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@LoyalityCredit", item.LoyalityCredit ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@QTY", item.Qty ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@LastSold", item.LastSold ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@OnlinePrice", item.OnlinePrice ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@IDCheck", item.Idcheck ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@FoodStampable", item.FoodStampable ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ManagersOnly", item.ManagersOnly ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@SellOnline", item.SellOnline ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Countthis", item.Countthis ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@lastimported", item.Lastimported ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@CreatedDate", item.CreatedDate ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Cost", item.Cost ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Brand", item.Brand ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@IteminCase", item.IteminCase ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@CasePrice", item.CasePrice ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Soldby", item.Soldby ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@OnlineImagelink", item.OnlineImagelink ?? (object)DBNull.Value));
                    command1.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ItemDescription", item.ItemDescription ?? (object)DBNull.Value));

                    result = await command1.ExecuteNonQueryAsync();

                    if (result <= 0)
                    {
                        await todbContext.Database.CloseConnectionAsync();
                        return 0;
                    }

                    totalItems++;

                }

                await todbContext.Database.CloseConnectionAsync();
            }
            else if (!hasDescriptionFrom && !hasDescriptionTo || !hasDescriptionFrom && hasDescriptionTo)
            {
                var sql2 = @"
                    INSERT INTO Tbl_Items (
                        ItemID, ItemName, ItemDeptID, ItemPrice, Tax1_Status, Picturepath, Price_Prompt, BtnColor, Visible,
                        EnableName, EnablePicture, ListOrder, isKOT, isKOT2, IsModifer, IsDeleted, Prompt_Description,
                        ShowInKitchenDisplay, LoyalityCredit, QTY, LastSold, OnlinePrice, IDCheck, FoodStampable,
                        ManagersOnly, SellOnline, Countthis, lastimported, CreatedDate, Cost, Brand, IteminCase,
                        CasePrice, Soldby, OnlineImagelink
                    ) VALUES (
                        @ItemID, @ItemName, @ItemDeptID, @ItemPrice, @Tax1_Status, @Picturepath, @Price_Prompt, @BtnColor, @Visible,
                        @EnableName, @EnablePicture, @ListOrder, @isKOT, @isKOT2, @IsModifer, @IsDeleted, @Prompt_Description,
                        @ShowInKitchenDisplay, @LoyalityCredit, @QTY, @LastSold, @OnlinePrice, @IDCheck, @FoodStampable,
                        @ManagersOnly, @SellOnline, @Countthis, @lastimported, @CreatedDate, @Cost, @Brand, @IteminCase,
                        @CasePrice, @Soldby, @OnlineImagelink
                    )";

                using var command2 = todbContext.Database.GetDbConnection().CreateCommand();
                command2.CommandText = sql2;
                command2.CommandType = CommandType.Text;

                await todbContext.Database.OpenConnectionAsync();

                foreach (var item in items)
                {
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ItemID", item.ItemId ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ItemName", item.ItemName ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ItemDeptID", item.ItemDeptId ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ItemPrice", item.ItemPrice ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Tax1_Status", item.Tax1Status ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Picturepath", item.Picturepath ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Price_Prompt", item.PricePrompt ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@BtnColor", item.BtnColor ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Visible", item.Visible ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@EnableName", item.EnableName ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@EnablePicture", item.EnablePicture ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ListOrder", item.ListOrder ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@isKOT", item.IsKot ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@isKOT2", item.IsKot2 ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@IsModifer", item.IsModifer ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@IsDeleted", item.IsDeleted ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Prompt_Description", item.PromptDescription ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ShowInKitchenDisplay", item.ShowInKitchenDisplay ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@LoyalityCredit", item.LoyalityCredit ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@QTY", item.Qty ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@LastSold", item.LastSold ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@OnlinePrice", item.OnlinePrice ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@IDCheck", item.Idcheck ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@FoodStampable", item.FoodStampable ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ManagersOnly", item.ManagersOnly ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@SellOnline", item.SellOnline ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Countthis", item.Countthis ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@lastimported", item.Lastimported ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@CreatedDate", item.CreatedDate ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Cost", item.Cost ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Brand", item.Brand ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@IteminCase", item.IteminCase ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@CasePrice", item.CasePrice ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@Soldby", item.Soldby ?? (object)DBNull.Value));
                    command2.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@OnlineImagelink", item.OnlineImagelink ?? (object)DBNull.Value));

                    result = await command2.ExecuteNonQueryAsync();

                    if (result <= 0)
                    {
                        await todbContext.Database.CloseConnectionAsync();
                        return 0;
                    }

                    totalItems++;
                }

                await todbContext.Database.CloseConnectionAsync();
            }
            else
            {
                return -1;
            }

            return totalItems == items.Count ? 1 : 0;
        }

        public async Task<List<ItemDto>> GetTableItemsAsyncNotIsModifier(DbContext dbContext)
        {
            var dbKey = dbContext.Database.GetDbConnection().ConnectionString;

            if (!_columnPresenceCache.TryGetValue(dbKey, out var hasDescription))
            {
                hasDescription = await _schema.ColumnExistsAsync(dbContext, "Tbl_Items", "ItemDescription");
                _columnPresenceCache[dbKey] = hasDescription;
            }

            var sql = hasDescription
                ? @"SELECT *
       FROM Tbl_Items WHERE IsModifer = '0'"
                : @"SELECT 
        ItemID,
        ItemName,
        ItemDeptID,
        ItemPrice,
        Tax1_Status,
        Picturepath,
        Price_Prompt,
        BtnColor,
        Visible,
        EnableName,
        EnablePicture,
        ListOrder,
        isKOT,
        isKOT2,
        IsModifer,
        IsDeleted,
        Prompt_Description,
        ShowInKitchenDisplay,
        LoyalityCredit,
        QTY,
        LastSold,
        OnlinePrice,
        IDCheck,
        FoodStampable,
        ManagersOnly,
        SellOnline,
        Countthis,
        lastimported,
        CreatedDate,
        Cost,
        Brand,
        IteminCase,
        CasePrice,
        Soldby,
        OnlineImagelink,
        NULL AS ItemDescription
       FROM Tbl_Items WHERE IsModifer = '0'";



            var res = await dbContext.Set<TblItem>()
                    .FromSqlRaw(sql)
                    .ToListAsync();

            return res.Select(item => new ItemDto
            {
                Brand = item.Brand,
                CasePrice = item.CasePrice,
                Cost = item.Cost,
                BtnColor = item.BtnColor,
                Countthis = item.Countthis,
                CreatedDate = item.CreatedDate,
                EnableName = item.EnableName,
                EnablePicture = item.EnablePicture,
                FoodStampable = item.FoodStampable,
                Idcheck = item.Idcheck,
                IsDeleted = item.IsDeleted,
                PromptDescription = item.PromptDescription,
                IsKot = item.IsKot,
                IsKot2 = item.IsKot2,
                IsModifer = item.IsModifer,
                ItemDescription = item.ItemDescription,
                IteminCase = item.IteminCase,
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                ItemDeptId = item.ItemDeptId,
                ItemPrice = item.ItemPrice,
                Lastimported = item.Lastimported,
                LastSold = item.LastSold,
                ListOrder = item.ListOrder,
                LoyalityCredit = item.LoyalityCredit,
                ManagersOnly = item.ManagersOnly,
                OnlinePrice = item.OnlinePrice,
                Picturepath = item.Picturepath,
                PricePrompt = item.PricePrompt,
                Qty = item.Qty,
                SellOnline = item.SellOnline,
                Tax1Status = item.Tax1Status,
                Visible = item.Visible,
                Soldby = item.Soldby
            }).ToList();
        }

        public async Task<List<ItemDto>> GetTableItemsAsyncIsModifier(DbContext dbContext)
        {
            var dbKey = dbContext.Database.GetDbConnection().ConnectionString;

            if (!_columnPresenceCache.TryGetValue(dbKey, out var hasDescription))
            {
                hasDescription = await _schema.ColumnExistsAsync(dbContext, "Tbl_Items", "ItemDescription");
                _columnPresenceCache[dbKey] = hasDescription;
            }

            var sql = hasDescription
                ? @"SELECT *
       FROM Tbl_Items WHERE IsModifer = '1'"
                : @"SELECT 
        ItemID,
        ItemName,
        ItemDeptID,
        ItemPrice,
        Tax1_Status,
        Picturepath,
        Price_Prompt,
        BtnColor,
        Visible,
        EnableName,
        EnablePicture,
        ListOrder,
        isKOT,
        isKOT2,
        IsModifer,
        IsDeleted,
        Prompt_Description,
        ShowInKitchenDisplay,
        LoyalityCredit,
        QTY,
        LastSold,
        OnlinePrice,
        IDCheck,
        FoodStampable,
        ManagersOnly,
        SellOnline,
        Countthis,
        lastimported,
        CreatedDate,
        Cost,
        Brand,
        IteminCase,
        CasePrice,
        Soldby,
        OnlineImagelink,
        NULL AS ItemDescription
       FROM Tbl_Items WHERE IsModifer = '1'";



            var res = await dbContext.Set<TblItem>()
                    .FromSqlRaw(sql)
                    .ToListAsync();

            return res.Select(item => new ItemDto
            {
                Brand = item.Brand,
                CasePrice = item.CasePrice,
                Cost = item.Cost,
                BtnColor = item.BtnColor,
                Countthis = item.Countthis,
                CreatedDate = item.CreatedDate,
                EnableName = item.EnableName,
                EnablePicture = item.EnablePicture,
                FoodStampable = item.FoodStampable,
                Idcheck = item.Idcheck,
                IsDeleted = item.IsDeleted,
                PromptDescription = item.PromptDescription,
                IsKot = item.IsKot,
                IsKot2 = item.IsKot2,
                IsModifer = item.IsModifer,
                ItemDescription = item.ItemDescription,
                IteminCase = item.IteminCase,
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                ItemDeptId = item.ItemDeptId,
                ItemPrice = item.ItemPrice,
                Lastimported = item.Lastimported,
                LastSold = item.LastSold,
                ListOrder = item.ListOrder,
                LoyalityCredit = item.LoyalityCredit,
                ManagersOnly = item.ManagersOnly,
                OnlinePrice = item.OnlinePrice,
                Picturepath = item.Picturepath,
                PricePrompt = item.PricePrompt,
                Qty = item.Qty,
                SellOnline = item.SellOnline,
                Tax1Status = item.Tax1Status,
                Visible = item.Visible,
                Soldby = item.Soldby
            }).ToList();
        }

        public async Task<ItemDto> GetItemByIdAsync(DbContext dbContext, string id)
        {
            var dbKey = dbContext.Database.GetDbConnection().ConnectionString;

            if (!_columnPresenceCache.TryGetValue(dbKey, out var hasDescription))
            {
                hasDescription = await _schema.ColumnExistsAsync(dbContext, "Tbl_Items", "ItemDescription");
                _columnPresenceCache[dbKey] = hasDescription;
            }

            var sql = hasDescription
                ? @"SELECT *
       FROM Tbl_Items WHERE ItemID = @id"
                : @"SELECT 
        ItemID,
        ItemName,
        ItemDeptID,
        ItemPrice,
        Tax1_Status,
        Picturepath,
        Price_Prompt,
        BtnColor,
        Visible,
        EnableName,
        EnablePicture,
        ListOrder,
        isKOT,
        isKOT2,
        IsModifer,
        IsDeleted,
        Prompt_Description,
        ShowInKitchenDisplay,
        LoyalityCredit,
        QTY,
        LastSold,
        OnlinePrice,
        IDCheck,
        FoodStampable,
        ManagersOnly,
        SellOnline,
        Countthis,
        lastimported,
        CreatedDate,
        Cost,
        Brand,
        IteminCase,
        CasePrice,
        Soldby,
        OnlineImagelink,
        NULL AS ItemDescription
       FROM Tbl_Items WHERE ItemID = @id";

            var res = await dbContext.Set<TblItem>()
                    .FromSqlRaw(sql, new SqlParameter("@id", id))
                    .ToListAsync();

            var item = res.FirstOrDefault();
            if (item == null) return null;

            return new ItemDto
            {
                Brand = item.Brand,
                CasePrice = item.CasePrice,
                Cost = item.Cost,
                BtnColor = item.BtnColor,
                Countthis = item.Countthis,
                CreatedDate = item.CreatedDate,
                EnableName = item.EnableName,
                EnablePicture = item.EnablePicture,
                FoodStampable = item.FoodStampable,
                Idcheck = item.Idcheck,
                IsDeleted = item.IsDeleted,
                PromptDescription = item.PromptDescription,
                IsKot = item.IsKot,
                IsKot2 = item.IsKot2,
                IsModifer = item.IsModifer,
                ItemDescription = item.ItemDescription,
                IteminCase = item.IteminCase,
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                ItemDeptId = item.ItemDeptId,
                ItemPrice = item.ItemPrice,
                Lastimported = item.Lastimported,
                LastSold = item.LastSold,
                ListOrder = item.ListOrder,
                LoyalityCredit = item.LoyalityCredit,
                ManagersOnly = item.ManagersOnly,
                OnlinePrice = item.OnlinePrice,
                Picturepath = item.Picturepath,
                PricePrompt = item.PricePrompt,
                Qty = item.Qty,
                SellOnline = item.SellOnline,
                Tax1Status = item.Tax1Status,
                Visible = item.Visible,
                Soldby = item.Soldby
            };
        }

        public async Task<ItemDto> CreateItemAsync(DbContext dbContext, ItemDto item)
        {
            var dbKey = dbContext.Database.GetDbConnection().ConnectionString;

            if (!_columnPresenceCache.TryGetValue(dbKey, out var hasDescription))
            {
                hasDescription = await _schema.ColumnExistsAsync(dbContext, "Tbl_Items", "ItemDescription");
                _columnPresenceCache[dbKey] = hasDescription;
            }

            var sql = hasDescription
                ? @"INSERT INTO Tbl_Items (
                        ItemID, ItemName, ItemDeptID, ItemPrice, Tax1_Status, Picturepath, Price_Prompt, BtnColor, Visible,
                        EnableName, EnablePicture, ListOrder, isKOT, isKOT2, IsModifer, IsDeleted, Prompt_Description,
                        ShowInKitchenDisplay, LoyalityCredit, QTY, LastSold, OnlinePrice, IDCheck, FoodStampable,
                        ManagersOnly, SellOnline, Countthis, lastimported, CreatedDate, Cost, Brand, IteminCase,
                        CasePrice, Soldby, OnlineImagelink, ItemDescription
                    ) VALUES (
                        @ItemID, @ItemName, @ItemDeptID, @ItemPrice, @Tax1_Status, @Picturepath, @Price_Prompt, @BtnColor, @Visible,
                        @EnableName, @EnablePicture, @ListOrder, @isKOT, @isKOT2, @IsModifer, @IsDeleted, @Prompt_Description,
                        @ShowInKitchenDisplay, @LoyalityCredit, @QTY, @LastSold, @OnlinePrice, @IDCheck, @FoodStampable,
                        @ManagersOnly, @SellOnline, @Countthis, @lastimported, @CreatedDate, @Cost, @Brand, @IteminCase,
                        @CasePrice, @Soldby, @OnlineImagelink, @ItemDescription
                    )"
                : @"INSERT INTO Tbl_Items (
                        ItemID, ItemName, ItemDeptID, ItemPrice, Tax1_Status, Picturepath, Price_Prompt, BtnColor, Visible,
                        EnableName, EnablePicture, ListOrder, isKOT, isKOT2, IsModifer, IsDeleted, Prompt_Description,
                        ShowInKitchenDisplay, LoyalityCredit, QTY, LastSold, OnlinePrice, IDCheck, FoodStampable,
                        ManagersOnly, SellOnline, Countthis, lastimported, CreatedDate, Cost, Brand, IteminCase,
                        CasePrice, Soldby, OnlineImagelink
                    ) VALUES (
                        @ItemID, @ItemName, @ItemDeptID, @ItemPrice, @Tax1_Status, @Picturepath, @Price_Prompt, @BtnColor, @Visible,
                        @EnableName, @EnablePicture, @ListOrder, @isKOT, @isKOT2, @IsModifer, @IsDeleted, @Prompt_Description,
                        @ShowInKitchenDisplay, @LoyalityCredit, @QTY, @LastSold, @OnlinePrice, @IDCheck, @FoodStampable,
                        @ManagersOnly, @SellOnline, @Countthis, @lastimported, @CreatedDate, @Cost, @Brand, @IteminCase,
                        @CasePrice, @Soldby, @OnlineImagelink
                    )";

            await dbContext.Database.OpenConnectionAsync();

            using var command = dbContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;

            Helper.AddParamItem(command, "ItemID", item.ItemId);
            Helper.AddParamItem(command, "ItemName", item.ItemName);
            Helper.AddParamItem(command, "ItemDeptID", item.ItemDeptId);
            Helper.AddParamItem(command, "ItemPrice", item.ItemPrice);
            Helper.AddParamItem(command, "Tax1_Status", item.Tax1Status);
            Helper.AddParamItem(command, "Picturepath", item.Picturepath);
            Helper.AddParamItem(command, "Price_Prompt", item.PricePrompt);
            Helper.AddParamItem(command, "BtnColor", item.BtnColor);
            Helper.AddParamItem(command, "Visible", item.Visible);
            Helper.AddParamItem(command, "EnableName", item.EnableName);
            Helper.AddParamItem(command, "EnablePicture", item.EnablePicture);
            Helper.AddParamItem(command, "ListOrder", item.ListOrder);
            Helper.AddParamItem(command, "isKOT", item.IsKot);
            Helper.AddParamItem(command, "isKOT2", item.IsKot2);
            Helper.AddParamItem(command, "IsModifer", item.IsModifer);
            Helper.AddParamItem(command, "IsDeleted", item.IsDeleted);
            Helper.AddParamItem(command, "Prompt_Description", item.PromptDescription);
            Helper.AddParamItem(command, "ShowInKitchenDisplay", item.ShowInKitchenDisplay);
            Helper.AddParamItem(command, "LoyalityCredit", item.LoyalityCredit);
            Helper.AddParamItem(command, "QTY", item.Qty);
            Helper.AddParamItem(command, "LastSold", item.LastSold);
            Helper.AddParamItem(command, "OnlinePrice", item.OnlinePrice);
            Helper.AddParamItem(command, "IDCheck", item.Idcheck);
            Helper.AddParamItem(command, "FoodStampable", item.FoodStampable);
            Helper.AddParamItem(command, "ManagersOnly", item.ManagersOnly);
            Helper.AddParamItem(command, "SellOnline", item.SellOnline);
            Helper.AddParamItem(command, "Countthis", item.Countthis);
            Helper.AddParamItem(command, "lastimported", item.Lastimported);
            Helper.AddParamItem(command, "CreatedDate", item.CreatedDate);
            Helper.AddParamItem(command, "Cost", item.Cost);
            Helper.AddParamItem(command, "Brand", item.Brand);
            Helper.AddParamItem(command, "IteminCase", item.IteminCase);
            Helper.AddParamItem(command, "CasePrice", item.CasePrice);
            Helper.AddParamItem(command, "Soldby", item.Soldby);
            Helper.AddParamItem(command, "OnlineImagelink", item.OnlineImagelink);

            if(hasDescription) Helper.AddParamItem(command, "ItemDescription", item.ItemDescription);

            var result = await command.ExecuteNonQueryAsync();
            await dbContext.Database.CloseConnectionAsync();

            return result > 0 ? item : null;
        }

        public async Task<bool> UpdateItemAsync(DbContext dbContext, string id, ItemDto item)
        {
            var dbKey = dbContext.Database.GetDbConnection().ConnectionString;

            if (!_columnPresenceCache.TryGetValue(dbKey, out var hasDescription))
            {
                hasDescription = await _schema.ColumnExistsAsync(dbContext, "Tbl_Items", "ItemDescription");
                _columnPresenceCache[dbKey] = hasDescription;
            }

            var sql = hasDescription
                ? @"UPDATE Tbl_Items SET
        ItemName = @ItemName,
        ItemDeptID = @ItemDeptID,
        ItemPrice = @ItemPrice,
        Tax1_Status = @Tax1_Status,
        Price_Prompt = @Price_Prompt,
        Visible = @Visible,
        EnablePicture = @EnablePicture,
        EnableName = @EnableName,
        isKOT = @isKOT,
        isKOT2 = @isKOT2,
        IsModifer = @IsModifer,
        IsDeleted = @IsDeleted,
        Prompt_Description = @Prompt_Description,
        ShowInKitchenDisplay = @ShowInKitchenDisplay,
        LoyalityCredit = @LoyalityCredit,
        QTY = @QTY,
        OnlineImagelink = @OnlineImagelink,
        OnlinePrice = @OnlinePrice,
        IDCheck = @IDCheck,
        FoodStampable = @FoodStampable,
        ManagersOnly = @ManagersOnly,
        SellOnline = @SellOnline,
        Countthis = @Countthis,
        Cost = @Cost,
        Brand = @Brand,
        IteminCase = @IteminCase,
        CasePrice = @CasePrice,
        Soldby = @Soldby,
        Picturepath = @Picturepath,
        ListOrder = @ListOrder,
        ItemDescription = @ItemDescription
     WHERE ItemID = @ItemID"
                : @"UPDATE Tbl_Items SET
        ItemName = @ItemName,
        ItemDeptID = @ItemDeptID,
        ItemPrice = @ItemPrice,
        Tax1_Status = @Tax1_Status,
        Price_Prompt = @Price_Prompt,
        Visible = @Visible,
        EnablePicture = @EnablePicture,
        EnableName = @EnableName,
        isKOT = @isKOT,
        isKOT2 = @isKOT2,
        IsModifer = @IsModifer,
        IsDeleted = @IsDeleted,
        Prompt_Description = @Prompt_Description,
        ShowInKitchenDisplay = @ShowInKitchenDisplay,
        LoyalityCredit = @LoyalityCredit,
        QTY = @QTY,
        OnlineImagelink = @OnlineImagelink,
        OnlinePrice = @OnlinePrice,
        IDCheck = @IDCheck,
        FoodStampable = @FoodStampable,
        ManagersOnly = @ManagersOnly,
        SellOnline = @SellOnline,
        Countthis = @Countthis,
        Cost = @Cost,
        Brand = @Brand,
        IteminCase = @IteminCase,
        CasePrice = @CasePrice,
        Soldby = @Soldby,
        Picturepath = @Picturepath,
        ListOrder = @ListOrder
     WHERE ItemID = @ItemID";


            await dbContext.Database.OpenConnectionAsync();

            using var command = dbContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;

            Helper.AddParamItem(command, "ItemID", id);
            Helper.AddParamItem(command, "ItemName", item.ItemName);
            Helper.AddParamItem(command, "ItemDeptID", item.ItemDeptId);
            Helper.AddParamItem(command, "ItemPrice", item.ItemPrice);
            Helper.AddParamItem(command, "Tax1_Status", item.Tax1Status);
            Helper.AddParamItem(command, "Price_Prompt", item.PricePrompt);
            Helper.AddParamItem(command, "Visible", item.Visible);
            Helper.AddParamItem(command, "EnablePicture", item.EnablePicture);
            Helper.AddParamItem(command, "EnableName", item.EnableName);
            Helper.AddParamItem(command, "isKOT", item.IsKot);
            Helper.AddParamItem(command, "isKOT2", item.IsKot2);
            Helper.AddParamItem(command, "IsModifer", item.IsModifer);
            Helper.AddParamItem(command, "IsDeleted", item.IsDeleted);
            Helper.AddParamItem(command, "Prompt_Description", item.PromptDescription);
            Helper.AddParamItem(command, "ShowInKitchenDisplay", item.ShowInKitchenDisplay);
            Helper.AddParamItem(command, "LoyalityCredit", item.LoyalityCredit);
            Helper.AddParamItem(command, "QTY", item.Qty);
            Helper.AddParamItem(command, "OnlineImagelink", item.OnlineImagelink);
            Helper.AddParamItem(command, "OnlinePrice", item.OnlinePrice);
            Helper.AddParamItem(command, "IDCheck", item.Idcheck);
            Helper.AddParamItem(command, "FoodStampable", item.FoodStampable);
            Helper.AddParamItem(command, "ManagersOnly", item.ManagersOnly);
            Helper.AddParamItem(command, "SellOnline", item.SellOnline);
            Helper.AddParamItem(command, "Countthis", item.Countthis);
            Helper.AddParamItem(command, "Cost", item.Cost);
            Helper.AddParamItem(command, "Brand", item.Brand);
            Helper.AddParamItem(command, "IteminCase", item.IteminCase);
            Helper.AddParamItem(command, "CasePrice", item.CasePrice);
            Helper.AddParamItem(command, "Soldby", item.Soldby);
            Helper.AddParamItem(command, "Picturepath", item.Picturepath);
            Helper.AddParamItem(command, "ListOrder", item.ListOrder);

            if(hasDescription) Helper.AddParamItem(command, "ItemDescription", item.ItemDescription);


            var result = await command.ExecuteNonQueryAsync();
            await dbContext.Database.CloseConnectionAsync();

            return result > 0 ? true : false;

        }
    }
}
