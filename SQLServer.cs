using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
        
        public static bool TableExist(SqlConnection connection, string table)
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
        public static bool RowExists(SqlConnection connection, string table, string idName, int id)
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
    }
}
