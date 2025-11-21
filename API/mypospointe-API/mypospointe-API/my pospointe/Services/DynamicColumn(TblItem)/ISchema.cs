using Microsoft.EntityFrameworkCore;

namespace my_pospointe.Services.DynamicColumn
{
    public interface ISchema
    {
        Task<bool> ColumnExistsAsync(DbContext dbContext, string tableName, string columnName);
    }
}
