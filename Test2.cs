using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServer
{
    internal class Test2
    {
        public static Dictionary<string, string> Test()
        {
            string sqliteConnectionString = $"Data Source={Directory.GetCurrentDirectory() + "\\copy\\data.qda"};";

            // TableInfo: "ColumnName", "DataType"
            Dictionary<string, string> tableInfo = new Dictionary<string, string>();

            using (SQLiteConnection connection = new SQLiteConnection(sqliteConnectionString))
            {
                connection.Open();

                string tableName = "source"; // Replace with the name of your SQLite table

                using (SQLiteCommand command = new SQLiteCommand($"PRAGMA table_info({tableName})", connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string columnName = reader["name"].ToString();
                            string dataType = reader["type"].ToString();
                            bool isNullable = Convert.ToInt32(reader["notnull"]) == 0;
                            bool isPrimaryKey = Convert.ToInt32(reader["pk"]) != 0;

                            MapSQLiteToSQLServerDataType(ref dataType, columnName);

                            tableInfo.Add(columnName, dataType);

                            /*Console.WriteLine($"Column Name: {columnName}");
                            Console.WriteLine($"Data Type: {dataType}");
                            Console.WriteLine($"Is Nullable: {isNullable}");
                            Console.WriteLine($"Is Primary Key: {isPrimaryKey}");
                            Console.WriteLine();*/
                        }
                    }
                }
            }

            return tableInfo;
        }

        public static void Testing()
        {
            string sqliteConnectionString = $"Data Source={Directory.GetCurrentDirectory() + "\\copy\\data.qda"};";

            using (SQLiteConnection connection = new SQLiteConnection(sqliteConnectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table';", connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string tableName = reader["name"].ToString();
                            Console.WriteLine($"Table Name: {tableName}");
                        }
                    }
                }
            }
        }

        public void CreateTables()
        {

        }

        static void MapSQLiteToSQLServerDataType(ref string sqliteDataType, string columnName)
        {
            if (sqliteDataType == "INTEGER")
            {
                sqliteDataType = "BIGINT";
            }
            else if (sqliteDataType == "TEXT")
            {
                if (columnName == "date")
                {
                    sqliteDataType = "DATETIME";
                }
                else
                {
                    sqliteDataType = "NVARCHAR(MAX)";
                }
            }
            
        }
    }
}
