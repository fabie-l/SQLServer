/*using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServer
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