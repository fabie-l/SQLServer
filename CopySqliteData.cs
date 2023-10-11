using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SQLServer
{
    public static class CopySqliteData
    {
        
        public static void project(SQLiteConnection sqlite, SqlConnection sqlserver, string table)
        {
            try
            {
                SQLServer.CopySqlite(sqlite, sqlserver, table);
                Console.WriteLine($"Data inserted into {table} successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting into {table}: " + ex.ToString());
            }

        }

        public static void source(SQLiteConnection sqlite, SqlConnection sqlserver, string table)
        {
            try
            {
                SQLServer.CopySqlite(sqlite, sqlserver, table);
                Console.WriteLine($"Data inserted into {table} successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting into {table}: " + ex.ToString());
            }
        }

        

        public static void code_image(SQLiteConnection sqlite, SqlConnection sqlserver, string table)
        {
            try
            {
                SQLServer.CopySqlite(sqlite, sqlserver, table);
                Console.WriteLine($"Data inserted into {table} successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting into {table}: " + ex.ToString());
            }

        }

        public static void code_av(SQLiteConnection sqlite, SqlConnection sqlserver, string table)
        {
            try
            {
                SQLServer.CopySqlite(sqlite, sqlserver, table);
                Console.WriteLine($"Data inserted into {table} successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting into {table}: " + ex.ToString());
            }

        }

        public static void annotation(SQLiteConnection sqlite, SqlConnection sqlserver, string table)
        {
            try
            {
                SQLServer.CopySqlite(sqlite, sqlserver, table);
                Console.WriteLine($"Data inserted into {table} successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting into {table}: " + ex.ToString());
            }

        }

        public static void attribute_type(SQLiteConnection sqlite, SqlConnection sqlserver, string table)
        {
            try
            {
                SQLServer.CopySqlite(sqlite, sqlserver, table);
                Console.WriteLine($"Data inserted into {table} successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting into {table}: " + ex.ToString());
            }

        }

        public static void attribute(SQLiteConnection sqlite, SqlConnection sqlserver, string table)
        {
            try
            {
                SQLServer.CopySqlite(sqlite, sqlserver, table);
                Console.WriteLine($"Data inserted into {table} successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting into {table}: " + ex.ToString());
            }
        }

        public static void case_text(SQLiteConnection sqlite, SqlConnection sqlserver, string table)
        {
            try
            {
                SQLServer.CopySqlite(sqlite, sqlserver, table);
                Console.WriteLine($"Data inserted into {table} successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting into {table}: " + ex.ToString());
            }
        }

        public static void cases(SQLiteConnection sqlite, SqlConnection sqlserver, string table)
        {
            try
            {
                SQLServer.CopySqlite(sqlite, sqlserver, table);
                Console.WriteLine($"Data inserted into {table} successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting into {table}: " + ex.ToString());
            }

        }

        public static void code_cat(SQLiteConnection sqlite, SqlConnection sqlserver, string table)
        {
            try
            {
                SQLServer.CopySqlite(sqlite, sqlserver, table);
                Console.WriteLine($"Data inserted into {table} successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting into {table}: " + ex.ToString());
            }
        }

        public static void code_text(SQLiteConnection sqlite, SqlConnection sqlserver, string table)
        {
            try
            {
                SQLServer.CopySqlite(sqlite, sqlserver, table);
                Console.WriteLine($"Data inserted into {table} successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting into {table}: " + ex.ToString());
            }

        }

        public static void code_name(SQLiteConnection sqlite, SqlConnection sqlserver, string table)
        {
            try
            {
                SQLServer.CopySqlite(sqlite, sqlserver, table);
                Console.WriteLine($"Data inserted into {table} successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting into {table}: " + ex.ToString());
            }
        }

        public static void journal(SQLiteConnection sqlite, SqlConnection sqlserver, string table)
        {
            try
            {
                SQLServer.CopySqlite(sqlite, sqlserver, table);
                Console.WriteLine($"Data inserted into {table} successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting into {table}: " + ex.ToString());
            }
        }



       public static void CopyAllSqlite(SQLiteConnection sqlite, SqlConnection sqlserver)
        {
            source(sqlite, sqlserver, "source");
            code_image(sqlite, sqlserver, "code_image");
            code_av(sqlite, sqlserver, "code_av");
            annotation(sqlite, sqlserver, "annotation");
            attribute_type(sqlite, sqlserver, "attribute_type");
            attribute(sqlite, sqlserver, "attribute");
            case_text(sqlite, sqlserver, "case_text");
            cases(sqlite, sqlserver, "cases");
            code_cat(sqlite, sqlserver, "code_cat");
            code_text(sqlite, sqlserver, "code_text");
            code_name(sqlite, sqlserver, "code_name");
            journal(sqlite, sqlserver, "journal");
        }

    }
}

/*public static void Insert_cases(SQLiteConnection connection, )
{
    */
/*Dictionary<string, object> _case = new Dictionary<string, object>()
{
    {"caseid", 1},
    {"name", "Case "},
    {"memo", "This is a test."},
    {"owner", 1},
    {"date", DateTime.Now.ToString("yyyy-MM-d HH:mm:ss")}
};*/

// Create the INSERT query dynamically based on the table and column names
/*string columns = string.Join(", ", _case.Keys);
string values = string.Join(", ", _case.Values.Select(value => QuoteValue(value)));

string insertQuery = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

try
{
    CSQLite.InsertData(connection, insertQuery);

}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}*/

/*public static void source(SQLiteConnection sqlite, SqlConnection sqlserver)
{


    try
    {
        sqlite.Open();
        sqlserver.Open();

        using (SQLiteCommand sqliteCommand = new SQLiteCommand("SELECT * FROM source", sqlite))
        {
            using (SQLiteDataReader sqliteReader = sqliteCommand.ExecuteReader())
            {
                while (sqliteReader.Read())
                {
                    using (SqlCommand sqlCommand = new SqlCommand($"INSERT INTO source (id, name, fulltext, mediapath, memo, owner, date, av_text_id, risid) " +
                        $"VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8, @param9)", sqlserver))
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

                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        sqlite.Close();
        sqlserver.Close();

        Console.WriteLine("Data inserted into source successfully!");

    }
    catch (Exception ex)
    {
        Console.WriteLine("Error inserting into source: " + ex.ToString());
    }

}*/
