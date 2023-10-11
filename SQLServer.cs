using SQLServer;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

/*SELECT name 
FROM sys.columns 
WHERE object_id = OBJECT_ID('[QC_v1].[dbo].[cases]')*/

namespace SQLServer
{
    public class SQLServer
    {
        private static bool DatabaseExists(SqlConnection connection, string dbName)
        {
            string query = $"SELECT 1 FROM sys.databases WHERE name = '{dbName}'";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                return cmd.ExecuteScalar() != null;
            }   
        }
        
        public static void CreateDatabase(SqlConnection connection, string dbName)
        {
            if (!DatabaseExists(connection, dbName))
            {
                try
                {
                    Console.WriteLine("Connected to SQL Server.");

                    // Create the database
                    string createDbQuery = $"CREATE DATABASE {dbName}";

                    using (SqlCommand createDbCommand = new(createDbQuery, connection))
                    {
                        createDbCommand.ExecuteNonQuery();
                        Console.WriteLine($"Database '{dbName}' created successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine($"Database {dbName} already exists. Creation skipped ... ");
            }
        }
        
        public static bool TableExists(SqlConnection connection, string table)
        {
            string query = "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TableName";


            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TableName", table);

                // Execute the query and check if any rows are returned
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        public static void CreateTable(SqlConnection connection, string query, string table)
        {

            if(!TableExists(connection, table))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine($"{table} created successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error creating {table}: " + ex.Message);
                    }
                }
            }
            else
            {
                Console.WriteLine($"Table {table} exists creation skipped ...");
            }
            
        }
        public static bool RowExists(SqlConnection connection, string table, string idName, object id)
        {
            string query = $"SELECT 1 FROM {table} WHERE {idName} = @IdToCheck";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@IdToCheck", id);

                // Execute the query and check if any rows are returned
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        public static void InsertData(SqlConnection connection, string table, Dictionary<string, object> columnValues)
        {
            string query = $"INSERT INTO {table} ({string.Join(", ", columnValues.Keys)}) " +
                           $"VALUES ({string.Join(", ", columnValues.Keys.Select(key => "@" + key))})";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                foreach (var kvp in columnValues)
                {
                    command.Parameters.AddWithValue("@" + kvp.Key, kvp.Value);
                }

                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine($"Data entered into {table} successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inserting data into {table}: " + ex.Message);
                }
            }
        }

        public static void InsertRecordStatus(SqlConnection connection, int id, string status)
        {
            string table = "RecordUpdateStatus";

            Dictionary<string, object> columnValues = new Dictionary<string, object>()
            {
                {"RecordUpdateStatusID", id},
                {"Status", status},
            };

            if(!RowExists(connection, "RecordUpdateStatus", "RecordUpdateStatusID", id ))
            {
                InsertData(connection, "RecordUpdateStatus", columnValues);
            }
            else
            {
                Console.WriteLine($"Row exists in {table}. Data not inserted.");
            }
        }

        public static void InserAllRecordStatus(SqlConnection connection)
        {
            InsertRecordStatus(connection, 1, "Pending");
            InsertRecordStatus(connection, 2, "Updated");
            InsertRecordStatus(connection, 3, "Deleted");
        }

        public static List<string> SqlGetColumnNames(SqlConnection sqlserver, string dbName, string tableName)
        {
            List<string> columnNames = new List<string>();

            // Construct the SQL query
            string query = $"SELECT name FROM {dbName}.sys.columns WHERE object_id = OBJECT_ID('[{dbName}].[dbo].[{tableName}]')";

            using (SqlCommand command = new SqlCommand(query, sqlserver))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Add column names to the list
                        columnNames.Add(reader.GetString(0));
                    }
                }
            }

            return columnNames;
        }

        public static List<string> SqliteGetColumnNames(SQLiteConnection connection, string tableName)
        {
            List<string> columnNames = new List<string>();

            // Construct the SQL query
            string query = $"PRAGMA table_info({tableName})";

            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // The column name is in the "name" column of the result
                        string columnName = reader["name"].ToString();
                        columnNames.Add(columnName);
                    }
                }
            }

            return columnNames;
        }

        public static void CopySqlite(SQLiteConnection sqlite, SqlConnection sqlserver, string tableName)
        {
            List<string> sqliteColumns = SqliteGetColumnNames(sqlite, tableName);
            string id = sqliteColumns[0];

            try
            {
                using (SQLiteCommand sQLiteCommand = new SQLiteCommand($"SELECT * FROM {tableName}", sqlite))
                {
                    using (SQLiteDataReader sqliteReader = sQLiteCommand.ExecuteReader())
                    {
                        while (sqliteReader.Read())
                        {
                            string dateString = sqliteReader["date"].ToString();

                            if (DateTime.TryParse(dateString, out DateTime dateValue))
                            {
                                using (SqlCommand checkCommand = new SqlCommand($"SELECT COUNT(*) FROM {tableName} WHERE {id} = @param1", sqlserver))
                                {
                                    checkCommand.Parameters.AddWithValue("@param1", sqliteReader[id]);
                                    int existingRecordCount = (int)checkCommand.ExecuteScalar();

                                    if (existingRecordCount > 0)
                                    {
                                        
                                        string query = $"UPDATE {tableName} SET ";

                                        using (SqlCommand updateCommand = new SqlCommand(query, sqlserver))
                                        {
                                            for (int i = 1; i < sqliteColumns.Count; i++)
                                            {
                                                string paramName = $"@param{i + 1}";
                                                string columnName = sqliteColumns[i];

                                                // Append the column name and parameter to the update statement
                                                updateCommand.CommandText += $"{columnName} = {paramName}, ";

                                                // Add the parameter to the SqlCommand
                                                updateCommand.Parameters.AddWithValue(paramName, sqliteReader[columnName]);
                                            }

                                            // Remove the trailing comma and space
                                            updateCommand.CommandText = updateCommand.CommandText.TrimEnd(',', ' ');

                                            // Add the WHERE clause to the update statement
                                            updateCommand.CommandText += $" WHERE {id} = @param1";

                                            // Add the ID parameter
                                            updateCommand.Parameters.AddWithValue("@param1", sqliteReader[id]);

                                            updateCommand.ExecuteNonQuery();
                                        }

                                    }
                                    else
                                    {
                                        using (SqlCommand sqlCommand = new SqlCommand($"INSERT INTO {tableName} (", sqlserver))
                                        {
                                            for (int i = 0; i < sqliteColumns.Count; i++)
                                            {
                                                string paramName = $"@param{i + 1}";
                                                string columnName = sqliteColumns[i];

                                                sqlCommand.CommandText += $"{columnName}, ";

                                                sqlCommand.Parameters.AddWithValue(paramName, sqliteReader[columnName]);
                                            }

                                            // Remove the trailing comma and space
                                            sqlCommand.CommandText = sqlCommand.CommandText.TrimEnd(',', ' ');

                                            // Add the values part of the SQL statement
                                            sqlCommand.CommandText += ") VALUES (";
                                            for (int i = 0; i < sqliteColumns.Count; i++)
                                            {
                                                string paramName = $"@param{i + 1}";

                                                sqlCommand.CommandText += $"{paramName}, ";
                                            }

                                            // Remove the trailing comma and space
                                            sqlCommand.CommandText = sqlCommand.CommandText.TrimEnd(',', ' ');
                                            sqlCommand.CommandText += ")";

                                            // Execute the INSERT statement
                                            sqlCommand.ExecuteNonQuery();
                                        }

                                    }
                                }
                            }
                            else
                            {
                                // Handle parsing error if necessary
                                Console.WriteLine($"Error parsing date for ID {sqliteReader[id]}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
