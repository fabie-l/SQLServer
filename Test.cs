using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*namespace SQLServer
{
    internal class Test
    {

        public static void TransferData(SQLiteConnection sqlite, SqlConnection sqlserver, string sourceTable, string targetTable, Dictionary<string, string> columnMapping, string primaryKeyColumnName)
        {
            try
            {
                sqlite.Open();
                sqlserver.Open();

                using (SQLiteCommand sqliteCommand = new SQLiteCommand($"SELECT * FROM {sourceTable}", sqlite))
                {
                    using (SQLiteDataReader sqliteReader = sqliteCommand.ExecuteReader())
                    {
                        while (sqliteReader.Read())
                        {
                            // Check if a record with the same primary key exists in SQL Server
                            using (SqlCommand checkCommand = new SqlCommand($"SELECT COUNT(*) FROM {targetTable} WHERE {primaryKeyColumnName} = @param1", sqlserver))
                            {
                                checkCommand.Parameters.AddWithValue("@param1", sqliteReader[primaryKeyColumnName]);
                                int existingRecordCount = (int)checkCommand.ExecuteScalar();

                                using (SqlCommand sqlCommand)
                                {
                                    if (existingRecordCount > 0)
                                    {
                                        // Update the existing record.
                                        sqlCommand = new SqlCommand($"UPDATE {targetTable} SET ", sqlserver);
                                    }
                                    else
                                    {
                                        // Insert a new record.
                                        sqlCommand = new SqlCommand($"INSERT INTO {targetTable} ({primaryKeyColumnName},", sqlserver);
                                    }

                                    foreach (var columnMap in columnMapping)
                                    {
                                        sqlCommand.CommandText += $"{columnMap.Value} = @param{columnMap.Key}, ";
                                        sqlCommand.Parameters.AddWithValue($"@param{columnMap.Key}", sqliteReader[columnMap.Key]);
                                    }

                                    sqlCommand.CommandText = sqlCommand.CommandText.TrimEnd(',', ' ');

                                    if (existingRecordCount > 0)
                                    {
                                        sqlCommand.CommandText += $" WHERE {primaryKeyColumnName} = @param1";
                                    }
                                    else
                                    {
                                        sqlCommand.CommandText += ")";
                                    }

                                    sqlCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }

                sqlite.Close();
                sqlserver.Close();

                Console.WriteLine($"Data inserted/updated in {targetTable} successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error transferring data to {targetTable}: " + ex.ToString());
            }
        }

    }
}
*/

namespace SQLServer
{
    public class Test
    {
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
                            string newDate = sqliteReader["date"].ToString();

                            if (DateTime.TryParse(newDate, out DateTime sqliteDate))
                            {
                                using (SqlCommand checkCommand = new SqlCommand($"SELECT date FROM {tableName} WHERE {id} = @param1", sqlserver))
                                {
                                    checkCommand.Parameters.AddWithValue("@param1", sqliteReader[id]);
                                    var existingDate = checkCommand.ExecuteScalar();

                                    if (existingDate == null || DateTime.TryParse(existingDate.ToString(), out DateTime sqlDate) && sqlDate < sqliteDate)
                                    {
                                        string query = "";

                                        using (SqlCommand sqlCommand = new SqlCommand(query, sqlserver))
                                        {
                                            if (existingDate == null)
                                            {
                                                // Insert the new record
                                                sqlCommand.CommandText += $"INSERT INTO {tableName} (";

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
                                            }
                                            else
                                            {
                                                // Update the existing record
                                                sqlCommand.CommandText += $"UPDATE {tableName} SET ";

                                                for (int i = 1; i < sqliteColumns.Count; i++)
                                                {
                                                    string paramName = $"@param{i + 1}";
                                                    string columnName = sqliteColumns[i];

                                                    // Append the column name and parameter to the update statement
                                                    sqlCommand.CommandText += $"{columnName} = {paramName}, ";

                                                    // Add the parameter to the SqlCommand
                                                    sqlCommand.Parameters.AddWithValue(paramName, sqliteReader[columnName]);
                                                }

                                                // Remove the trailing comma and space
                                                sqlCommand.CommandText = sqlCommand.CommandText.TrimEnd(',', ' ');

                                                // Add the WHERE clause to the update statement
                                                sqlCommand.CommandText += $" WHERE {id} = @param1";

                                                // Add the ID parameter
                                                sqlCommand.Parameters.AddWithValue("@param1", sqliteReader[id]);
                                            }

                                            try
                                            {
                                                // Execute the INSERT/UPDATE statement
                                                sqlCommand.ExecuteNonQuery();
                                                Console.WriteLine($"Data inserted/updated into {tableName} successfully.");
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.ToString());
                                            }
                                            

                                        }

                                    }
                                    else
                                    {
                                        Console.WriteLine($"Data not updated in {tableName}, because date is not recent.");
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

        public static void Testing(SQLiteConnection sqlite, SqlConnection sqlserver, string tableName)
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
        /*public static void Insert(SQLiteConnection sqlite, SqlConnection sqlserver, string tableName)
        {
            try
            {
                sqlite.Open();
                sqlserver.Open();

                using (SQLiteCommand sqliteCommand = new SQLiteCommand($"SELECT * FROM {table}", sqlite))
                {
                    using (SQLiteDataReader sqliteReader = sqliteCommand.ExecuteReader())
                    {
                        while (sqliteReader.Read())
                        {
                            // Parse the date from SQLite text format to DateTime
                            string dateString = sqliteReader["date"].ToString();

                            if (DateTime.TryParse(dateString, out DateTime dateValue))
                            {
                                // Check if a record with the same primary key exists in SQL Server
                                using (SqlCommand checkCommand = new SqlCommand($"SELECT COUNT(*) FROM {table} WHERE id = @param1", sqlserver))
                                {
                                    checkCommand.Parameters.AddWithValue("@param1", sqliteReader["id"]);
                                    int existingRecordCount = (int)checkCommand.ExecuteScalar();

                                    if (existingRecordCount > 0)
                                    {
                                        // A record with the same primary key already exists.
                                        // Update the existing record.
                                        using (SqlCommand updateCommand = new SqlCommand($@"UPDATE {table}
                                                                                    SET name = @param2,
                                                                                        fulltext = @param3,
                                                                                        mediapath = @param4,
                                                                                        memo = @param5,
                                                                                        owner = @param6,
                                                                                        date = @param7,
                                                                                        av_text_id = @param8,
                                                                                        risid = @param9,
                                                                                        RecordUpdateStatusID = @param10
                                                                                    WHERE id = @param1", sqlserver))
                                        {
                                            updateCommand.Parameters.AddWithValue("@param1", sqliteReader["id"]);
                                            updateCommand.Parameters.AddWithValue("@param2", sqliteReader["name"]);
                                            updateCommand.Parameters.AddWithValue("@param3", sqliteReader["fulltext"]);
                                            updateCommand.Parameters.AddWithValue("@param4", sqliteReader["mediapath"]);
                                            updateCommand.Parameters.AddWithValue("@param5", sqliteReader["memo"]);
                                            updateCommand.Parameters.AddWithValue("@param6", sqliteReader["owner"]);
                                            updateCommand.Parameters.AddWithValue("@param7", sqliteReader["date"]);
                                            updateCommand.Parameters.AddWithValue("@param8", sqliteReader["av_text_id"]);
                                            updateCommand.Parameters.AddWithValue("@param9", sqliteReader["risid"]);
                                            updateCommand.Parameters.AddWithValue("@param10", 2);

                                            updateCommand.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        // Record with the primary key doesn't exist, so insert it.
                                        using (SqlCommand sqlCommand = new SqlCommand($"INSERT INTO {table} (id, name, fulltext, mediapath, memo, owner, date, av_text_id, risid, RecordUpdateStatusID)" +
                                            $"VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8, @param9, @param10)", sqlserver))
                                        {
                                            sqlCommand.Parameters.AddWithValue("@param1", sqliteReader["id"]);
                                            sqlCommand.Parameters.AddWithValue("@param2", sqliteReader["name"]);
                                            sqlCommand.Parameters.AddWithValue("@param3", sqliteReader["fulltext"]);
                                            sqlCommand.Parameters.AddWithValue("@param4", sqliteReader["mediapath"]);
                                            sqlCommand.Parameters.AddWithValue("@param5", sqliteReader["memo"]);
                                            sqlCommand.Parameters.AddWithValue("@param6", sqliteReader["owner"]);
                                            sqlCommand.Parameters.AddWithValue("@param7", sqliteReader["date"]);
                                            sqlCommand.Parameters.AddWithValue("@param8", sqliteReader["av_text_id"]);
                                            sqlCommand.Parameters.AddWithValue("@param9", sqliteReader["risid"]);
                                            sqlCommand.Parameters.AddWithValue("@param10", 2);

                                            sqlCommand.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Handle parsing error if necessary
                                Console.WriteLine($"Error parsing date for ID {sqliteReader["id"]}");
                            }
                        }
                    }
                }

                sqlite.Close();
                sqlserver.Close();

                Console.WriteLine($"Data inserted/updated in {table} successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting/updating {table} data: " + ex.ToString());
            }
        }*/
    }
}