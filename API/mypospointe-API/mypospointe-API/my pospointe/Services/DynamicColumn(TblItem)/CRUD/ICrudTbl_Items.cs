using Microsoft.EntityFrameworkCore;
using my_pospointe.Models;
using my_pospointe.Models.DynamicColumn;

namespace my_pospointe.Services.DynamicColumn.CRUD
{
    public interface ICrudTbl_Items
    {
        Task<List<ItemDto>> GetItemsAsync(DbContext dbContext);

        Task<int> AddItemsAsyncCopyDB(DbContext fromdbContext, DbContext todbContext, List<TblItem> items);

        //Item Controller
        // public IEnumerable<TblItem> Get()
        Task<List<ItemDto>> GetTableItemsAsyncNotIsModifier(DbContext dbContext);


        // public IEnumerable<TblItem> Getmod()
        Task<List<ItemDto>> GetTableItemsAsyncIsModifier(DbContext dbContext);

        //      [HttpGet("{id}")]
        Task<ItemDto> GetItemByIdAsync(DbContext dbContext, string id);


        // POST api/<ItemsController>
        //[HttpPost]
        Task<ItemDto> CreateItemAsync(DbContext dbContext, ItemDto item);


        // PUT api/<ItemsController>/5
       // [HttpPut("{id}")]
        Task<bool> UpdateItemAsync(DbContext dbContext, string id, ItemDto item);


    }
}
