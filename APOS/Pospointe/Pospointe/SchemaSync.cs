using Microsoft.SqlServer.Dac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel.Composition.Primitives;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Pospointe
{
    class SchemaSync
    {
        public void MigrateSchema(string hostedConnectionString, string localConnectionString)
        {
            try
            {
                using (var hostedConn = new SqlConnection(hostedConnectionString))
                using (var localConn = new SqlConnection(localConnectionString))
                {
                    hostedConn.Open();
                    localConn.Open();

                    Console.WriteLine("Fetching hosted database schema...");
                    var hostedSchema = GetSchema(hostedConn);

                    Console.WriteLine("Fetching local database schema...");
                    var localSchema = GetSchema(localConn);

                    Console.WriteLine("Comparing schemas...");
                    var migrationScripts = GenerateMigrationScripts(hostedSchema, localSchema);

                    Console.WriteLine("Applying migration scripts...");
                    ApplyMigrationScripts(migrationScripts, localConn);

                    Console.WriteLine("Schema migration completed successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during schema migration: {ex.Message}");
            }
        }

        private DataTable GetSchema(SqlConnection connection)
        {
            // Get schema details (tables and columns)
            return connection.GetSchema("Columns");
        }

        private List<string> GenerateMigrationScripts(DataTable sourceSchema, DataTable targetSchema)
        {
            var scripts = new List<string>();

            // Compare the source schema with the target schema
            foreach (DataRow sourceRow in sourceSchema.Rows)
            {
                string tableName = sourceRow["TABLE_NAME"].ToString();
                string columnName = sourceRow["COLUMN_NAME"].ToString();
                string dataType = sourceRow["DATA_TYPE"].ToString();
                string characterMaximumLength = sourceRow["CHARACTER_MAXIMUM_LENGTH"]?.ToString();

                // Check if the column exists in the target schema
                var targetRows = targetSchema.Select($"TABLE_NAME = '{tableName}' AND COLUMN_NAME = '{columnName}'");
                if (targetRows.Length == 0)
                {
                    // Generate an ALTER TABLE script to add the missing column
                    string lengthClause = characterMaximumLength == "-1" ? "MAX" : characterMaximumLength;
                    scripts.Add($"ALTER TABLE {tableName} ADD {columnName} {dataType.ToUpper()}({lengthClause});");
                    Console.WriteLine($"Generated script: ALTER TABLE {tableName} ADD {columnName} {dataType.ToUpper()}({lengthClause});");
                }
                else
                {
                    // If the column exists, compare data types and lengths
                    var targetRow = targetRows[0];
                    string targetDataType = targetRow["DATA_TYPE"].ToString();
                    string targetLength = targetRow["CHARACTER_MAXIMUM_LENGTH"]?.ToString();

                    if (dataType != targetDataType || characterMaximumLength != targetLength)
                    {
                        string lengthClause = characterMaximumLength == "-1" ? "MAX" : characterMaximumLength;
                        scripts.Add($"ALTER TABLE {tableName} ALTER COLUMN {columnName} {dataType.ToUpper()}({lengthClause});");
                        Console.WriteLine($"Generated script: ALTER TABLE {tableName} ALTER COLUMN {columnName} {dataType.ToUpper()}({lengthClause});");
                    }
                }
            }

            return scripts;
        }

        private void ApplyMigrationScripts(List<string> scripts, SqlConnection connection)
        {
            foreach (var script in scripts)
            {
                try
                {
                    using (var command = new SqlCommand(script, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine($"Applied script: {script}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error applying script: {script}. Error: {ex.Message}");
                }
            }
        }

    }
    
    
}
