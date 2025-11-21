using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace my_pospointe.Services.DynamicColumn
{
    public class DynamicSchema : ISchema
    {
        private readonly ConcurrentDictionary<string, HashSet<string>> _cache = new();

        public async Task<bool> ColumnExistsAsync(DbContext dbContext, string tableName, string columnName)
        {
            var dbKey = dbContext.Database.GetDbConnection().ConnectionString;

            if (_cache.TryGetValue(dbKey, out var columns) && columns.Contains($"{tableName}.{columnName}"))
                return true;

            var sql = @"
            SELECT COUNT(*) 
            FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE TABLE_NAME = @table AND COLUMN_NAME = @column";

            using var command = dbContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = sql;

            var tableParam = command.CreateParameter();
            tableParam.ParameterName = "@table";
            tableParam.Value = tableName;
            command.Parameters.Add(tableParam);

            var columnParam = command.CreateParameter();
            columnParam.ParameterName = "@column";
            columnParam.Value = columnName;
            command.Parameters.Add(columnParam);

            await dbContext.Database.OpenConnectionAsync();
            var result = (int)(await command.ExecuteScalarAsync());
            await dbContext.Database.CloseConnectionAsync();

            if (result > 0)
            {
                _cache.AddOrUpdate(dbKey,
                    _ => new HashSet<string> { $"{tableName}.{columnName}" },
                    (_, existing) => { existing.Add($"{tableName}.{columnName}"); return existing; });
            }

            return result > 0;
        }
    }
}
