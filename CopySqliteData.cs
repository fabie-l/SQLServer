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
    internal class CopySqliteData
    {
        public static void project(SQLiteConnection sqlite, SqlConnection sqlserver, string table)
        {
            try
            {
                sqlite.Open();
                sqlserver.Open();

                using (SQLiteCommand sqliteCommand = new SQLiteCommand("SELECT * FROM project", sqlite))
                {
                    using (SQLiteDataReader sqliteReader = sqliteCommand.ExecuteReader())
                    {
                        while (sqliteReader.Read())
                        {
                            using (SqlCommand sqlCommand = new SqlCommand($"INSERT INTO project (databaseversion, date, memo, about, bookmarkfile, bookmarkpos, codername) " +
                                $"VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7)", sqlserver))
                            {
                                sqlCommand.Parameters.AddWithValue("@param1", sqliteReader["databaseversion"]);
                                sqlCommand.Parameters.AddWithValue("@param2", sqliteReader["date"]);
                                sqlCommand.Parameters.AddWithValue("@param3", sqliteReader["memo"]);
                                sqlCommand.Parameters.AddWithValue("@param4", sqliteReader["about"]);
                                sqlCommand.Parameters.AddWithValue("@param5", sqliteReader["bookmarkfile"]);
                                sqlCommand.Parameters.AddWithValue("@param6", sqliteReader["bookmarkpos"]);
                                sqlCommand.Parameters.AddWithValue("@param7", sqliteReader["codername"]);

                                sqlCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }

                sqlite.Close();
                sqlserver.Close();
                Console.WriteLine("Data inserted into project successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting into project: " + ex.ToString());
            }

        }

        public static void source(SQLiteConnection sqlite, SqlConnection sqlserver, string table)
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
        }

        

        public static void code_image(SQLiteConnection sqlite, SqlConnection sqlserver)
        {


            try
            {
                sqlite.Open();
                sqlserver.Open();

                using (SQLiteCommand sqliteCommand = new SQLiteCommand("SELECT * FROM code_image", sqlite))
                {
                    using (SQLiteDataReader sqliteReader = sqliteCommand.ExecuteReader())
                    {
                        while (sqliteReader.Read())
                        {
                            using (SqlCommand sqlCommand = new SqlCommand($"INSERT INTO code_image (imid, id, x1, y1, width, height, cid, memo, date, owner, important) " +
                                $"VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8, @param9, @param10, @param11)", sqlserver))
                            {
                                sqlCommand.Parameters.AddWithValue("@param1", sqliteReader["imid"]);
                                sqlCommand.Parameters.AddWithValue("@param2", sqliteReader["id"]);
                                sqlCommand.Parameters.AddWithValue("@param3", sqliteReader["x1"]);
                                sqlCommand.Parameters.AddWithValue("@param4", sqliteReader["y1"]);
                                sqlCommand.Parameters.AddWithValue("@param5", sqliteReader["width"]);
                                sqlCommand.Parameters.AddWithValue("@param6", sqliteReader["height"]);
                                sqlCommand.Parameters.AddWithValue("@param7", sqliteReader["cid"]);
                                sqlCommand.Parameters.AddWithValue("@param8", sqliteReader["memo"]);
                                sqlCommand.Parameters.AddWithValue("@param9", sqliteReader["date"]);
                                sqlCommand.Parameters.AddWithValue("@param10", sqliteReader["owner"]);
                                sqlCommand.Parameters.AddWithValue("@param11", sqliteReader["important"]);

                                sqlCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }

                sqlite.Close();
                sqlserver.Close();

                Console.WriteLine("Data inserted into code_image successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting into code_image: " + ex.ToString());
            }

        }

        public static void code_av(SQLiteConnection sqlite, SqlConnection sqlserver)
        {


            try
            {
                sqlite.Open();
                sqlserver.Open();

                using (SQLiteCommand sqliteCommand = new SQLiteCommand("SELECT * FROM code_av", sqlite))
                {
                    using (SQLiteDataReader sqliteReader = sqliteCommand.ExecuteReader())
                    {
                        while (sqliteReader.Read())
                        {
                            using (SqlCommand sqlCommand = new SqlCommand($"INSERT INTO code_av (avid, id, pos0, pos1, cid, memo, date, owner, important) " +
                                $"VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8, @param9)", sqlserver))
                            {
                                sqlCommand.Parameters.AddWithValue("@param1", sqliteReader["avid"]);
                                sqlCommand.Parameters.AddWithValue("@param2", sqliteReader["id"]);
                                sqlCommand.Parameters.AddWithValue("@param3", sqliteReader["pos0"]);
                                sqlCommand.Parameters.AddWithValue("@param4", sqliteReader["pos1"]);
                                sqlCommand.Parameters.AddWithValue("@param5", sqliteReader["cid"]);
                                sqlCommand.Parameters.AddWithValue("@param6", sqliteReader["memo"]);
                                sqlCommand.Parameters.AddWithValue("@param7", sqliteReader["date"]);
                                sqlCommand.Parameters.AddWithValue("@param8", sqliteReader["owner"]);
                                sqlCommand.Parameters.AddWithValue("@param9", sqliteReader["important"]);

                                sqlCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }

                sqlite.Close();
                sqlserver.Close();

                Console.WriteLine("Data inserted into code_image successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting into code_av: " + ex.ToString());
            }

        }

        public static void annotation(SQLiteConnection sqlite, SqlConnection sqlserver)
        {


            try
            {
                sqlite.Open();
                sqlserver.Open();

                using (SQLiteCommand sqliteCommand = new SQLiteCommand("SELECT * FROM annotation", sqlite))
                {
                    using (SQLiteDataReader sqliteReader = sqliteCommand.ExecuteReader())
                    {
                        while (sqliteReader.Read())
                        {
                            using (SqlCommand sqlCommand = new SqlCommand($"INSERT INTO annotation (anid, fid, pos0, pos1, memo, owner, date)" +
                                $"VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7)", sqlserver))
                            {
                                sqlCommand.Parameters.AddWithValue("@param1", sqliteReader["anid"]);
                                sqlCommand.Parameters.AddWithValue("@param2", sqliteReader["fid"]);
                                sqlCommand.Parameters.AddWithValue("@param3", sqliteReader["pos0"]);
                                sqlCommand.Parameters.AddWithValue("@param4", sqliteReader["pos1"]);
                                sqlCommand.Parameters.AddWithValue("@param5", sqliteReader["memo"]);
                                sqlCommand.Parameters.AddWithValue("@param6", sqliteReader["owner"]);
                                sqlCommand.Parameters.AddWithValue("@param7", sqliteReader["date"]);

                                sqlCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }

                sqlite.Close();
                sqlserver.Close();

                Console.WriteLine("Data inserted into annotation successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting into annotation: " + ex.ToString());
            }

        }

        public static void attribute_type(SQLiteConnection sqlite, SqlConnection sqlserver)
        {


            try
            {
                sqlite.Open();
                sqlserver.Open();

                using (SQLiteCommand sqliteCommand = new SQLiteCommand("SELECT * FROM attribute_type", sqlite))
                {
                    using (SQLiteDataReader sqliteReader = sqliteCommand.ExecuteReader())
                    {
                        while (sqliteReader.Read())
                        {
                            using (SqlCommand sqlCommand = new SqlCommand($"INSERT INTO attribute_type (name, date, owner, memo, caseOrFile, valueType)" +
                                $"VALUES (@param1, @param2, @param3, @param4, @param5, @param6)", sqlserver))
                            {
                                sqlCommand.Parameters.AddWithValue("@param1", sqliteReader["name"]);
                                sqlCommand.Parameters.AddWithValue("@param2", sqliteReader["date"]);
                                sqlCommand.Parameters.AddWithValue("@param3", sqliteReader["owner"]);
                                sqlCommand.Parameters.AddWithValue("@param4", sqliteReader["memo"]);
                                sqlCommand.Parameters.AddWithValue("@param5", sqliteReader["caseOrFile"]);
                                sqlCommand.Parameters.AddWithValue("@param6", sqliteReader["valueType"]);

                                sqlCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }

                sqlite.Close();
                sqlserver.Close();

                Console.WriteLine("Data inserted into attribute_type successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting into attribute_type: " + ex.ToString());
            }

        }

        public static void attribute(SQLiteConnection sqlite, SqlConnection sqlserver)
        {


            try
            {
                sqlite.Open();
                sqlserver.Open();

                using (SQLiteCommand sqliteCommand = new SQLiteCommand("SELECT * FROM attribute", sqlite))
                {
                    using (SQLiteDataReader sqliteReader = sqliteCommand.ExecuteReader())
                    {
                        while (sqliteReader.Read())
                        {
                            using (SqlCommand sqlCommand = new SqlCommand($"INSERT INTO attribute (attrid, name, attr_type, id, date, owner)" +
                                $"VALUES (@param1, @param2, @param3, @param4, @param5, @param6)", sqlserver))
                            {
                                sqlCommand.Parameters.AddWithValue("@param1", sqliteReader["attrid"]);
                                sqlCommand.Parameters.AddWithValue("@param2", sqliteReader["name"]);
                                sqlCommand.Parameters.AddWithValue("@param3", sqliteReader["attr_type"]);
                                sqlCommand.Parameters.AddWithValue("@param4", sqliteReader["id"]);
                                sqlCommand.Parameters.AddWithValue("@param5", sqliteReader["date"]);
                                sqlCommand.Parameters.AddWithValue("@param6", sqliteReader["owner"]);

                                sqlCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }

                sqlite.Close();
                sqlserver.Close();

                Console.WriteLine("Data inserted into attribute successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting into attribute: " + ex.ToString());
            }

        }

        public static void case_text(SQLiteConnection sqlite, SqlConnection sqlserver)
        {


            try
            {
                sqlite.Open();
                sqlserver.Open();

                using (SQLiteCommand sqliteCommand = new SQLiteCommand("SELECT * FROM case_text", sqlite))
                {
                    using (SQLiteDataReader sqliteReader = sqliteCommand.ExecuteReader())
                    {
                        while (sqliteReader.Read())
                        {
                            using (SqlCommand sqlCommand = new SqlCommand($"INSERT INTO case_text (id, caseid, fid, pos0, pos1, owner, date, memo)" +
                                $"VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8)", sqlserver))
                            {
                                sqlCommand.Parameters.AddWithValue("@param1", sqliteReader["id"]);
                                sqlCommand.Parameters.AddWithValue("@param2", sqliteReader["caseid"]);
                                sqlCommand.Parameters.AddWithValue("@param3", sqliteReader["fid"]);
                                sqlCommand.Parameters.AddWithValue("@param4", sqliteReader["pos0"]);
                                sqlCommand.Parameters.AddWithValue("@param5", sqliteReader["pos1"]);
                                sqlCommand.Parameters.AddWithValue("@param6", sqliteReader["owner"]);
                                sqlCommand.Parameters.AddWithValue("@param7", sqliteReader["date"]);
                                sqlCommand.Parameters.AddWithValue("@param8", sqliteReader["memo"]);

                                sqlCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }

                sqlite.Close();
                sqlserver.Close();

                Console.WriteLine("Data inserted into case_text successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting into case_text: " + ex.ToString());
            }

        }

        public static void cases(SQLiteConnection sqlite, SqlConnection sqlserver)
        {


            try
            {
                sqlite.Open();
                sqlserver.Open();

                using (SQLiteCommand sqliteCommand = new SQLiteCommand("SELECT * FROM cases", sqlite))
                {
                    using (SQLiteDataReader sqliteReader = sqliteCommand.ExecuteReader())
                    {
                        while (sqliteReader.Read())
                        {
                            using (SqlCommand sqlCommand = new SqlCommand($"INSERT INTO cases (caseid, name, memo, owner, date)" +
                                $"VALUES (@param1, @param2, @param3, @param4, @param5)", sqlserver))
                            {
                                sqlCommand.Parameters.AddWithValue("@param1", sqliteReader["caseid"]);
                                sqlCommand.Parameters.AddWithValue("@param2", sqliteReader["name"]);
                                sqlCommand.Parameters.AddWithValue("@param3", sqliteReader["memo"]);
                                sqlCommand.Parameters.AddWithValue("@param4", sqliteReader["owner"]);
                                sqlCommand.Parameters.AddWithValue("@param5", sqliteReader["date"]);

                                sqlCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }

                sqlite.Close();
                sqlserver.Close();

                Console.WriteLine("Data inserted into cases successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting into cases: " + ex.ToString());
            }

        }

        public static void code_cat(SQLiteConnection sqlite, SqlConnection sqlserver)
        {


            try
            {
                sqlite.Open();
                sqlserver.Open();

                using (SQLiteCommand sqliteCommand = new SQLiteCommand("SELECT * FROM code_cat", sqlite))
                {
                    using (SQLiteDataReader sqliteReader = sqliteCommand.ExecuteReader())
                    {
                        while (sqliteReader.Read())
                        {
                            using (SqlCommand sqlCommand = new SqlCommand($"INSERT INTO code_cat (catid, name, owner, date, memo, supercatid)" +
                                $"VALUES (@param1, @param2, @param3, @param4, @param5, @param6)", sqlserver))
                            {
                                sqlCommand.Parameters.AddWithValue("@param1", sqliteReader["catid"]);
                                sqlCommand.Parameters.AddWithValue("@param2", sqliteReader["name"]);
                                sqlCommand.Parameters.AddWithValue("@param3", sqliteReader["owner"]);
                                sqlCommand.Parameters.AddWithValue("@param4", sqliteReader["date"]);
                                sqlCommand.Parameters.AddWithValue("@param5", sqliteReader["memo"]);
                                sqlCommand.Parameters.AddWithValue("@param6", sqliteReader["supercatid"]);

                                sqlCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }

                sqlite.Close();
                sqlserver.Close();

                Console.WriteLine("Data inserted into code_cat successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting into code_cat: " + ex.ToString());
            }

        }

        public static void code_text(SQLiteConnection sqlite, SqlConnection sqlserver)
        {


            try
            {
                sqlite.Open();
                sqlserver.Open();

                using (SQLiteCommand sqliteCommand = new SQLiteCommand("SELECT * FROM code_text", sqlite))
                {
                    using (SQLiteDataReader sqliteReader = sqliteCommand.ExecuteReader())
                    {
                        while (sqliteReader.Read())
                        {
                            using (SqlCommand sqlCommand = new SqlCommand($"INSERT INTO code_text (ctid, cid, fid, seltext, pos0, pos1, owner, date, memo, avid, important) " +
                                $"VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8, @param9, @param10, @param11)", sqlserver))
                            {
                                sqlCommand.Parameters.AddWithValue("@param1", sqliteReader["ctid"]);
                                sqlCommand.Parameters.AddWithValue("@param2", sqliteReader["cid"]);
                                sqlCommand.Parameters.AddWithValue("@param3", sqliteReader["fid"]);
                                sqlCommand.Parameters.AddWithValue("@param4", sqliteReader["seltext"]);
                                sqlCommand.Parameters.AddWithValue("@param5", sqliteReader["pos0"]);
                                sqlCommand.Parameters.AddWithValue("@param6", sqliteReader["pos1"]);
                                sqlCommand.Parameters.AddWithValue("@param7", sqliteReader["owner"]);
                                sqlCommand.Parameters.AddWithValue("@param8", sqliteReader["date"]);
                                sqlCommand.Parameters.AddWithValue("@param9", sqliteReader["memo"]);
                                sqlCommand.Parameters.AddWithValue("@param10", sqliteReader["avid"]);
                                sqlCommand.Parameters.AddWithValue("@param11", sqliteReader["important"]);


                                sqlCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }

                sqlite.Close();
                sqlserver.Close();

                Console.WriteLine("Data inserted into code_text successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting into code_text: " + ex.ToString());
            }

        }

        public static void code_name(SQLiteConnection sqlite, SqlConnection sqlserver)
        {


            try
            {
                sqlite.Open();
                sqlserver.Open();

                using (SQLiteCommand sqliteCommand = new SQLiteCommand("SELECT * FROM code_name", sqlite))
                {
                    using (SQLiteDataReader sqliteReader = sqliteCommand.ExecuteReader())
                    {
                        while (sqliteReader.Read())
                        {
                            using (SqlCommand sqlCommand = new SqlCommand($"INSERT INTO code_name (cid, name, memo, catid, owner, date, color) " +
                                $"VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7)", sqlserver))
                            {
                                sqlCommand.Parameters.AddWithValue("@param1", sqliteReader["cid"]);
                                sqlCommand.Parameters.AddWithValue("@param2", sqliteReader["name"]);
                                sqlCommand.Parameters.AddWithValue("@param3", sqliteReader["memo"]);
                                sqlCommand.Parameters.AddWithValue("@param4", sqliteReader["catid"]);
                                sqlCommand.Parameters.AddWithValue("@param5", sqliteReader["owner"]);
                                sqlCommand.Parameters.AddWithValue("@param6", sqliteReader["date"]);
                                sqlCommand.Parameters.AddWithValue("@param7", sqliteReader["color"]);

                                sqlCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }

                sqlite.Close();
                sqlserver.Close();

                Console.WriteLine("Data inserted into code_name successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting into code_name: " + ex.ToString());
            }

        }

        public static void journal(SQLiteConnection sqlite, SqlConnection sqlserver)
        {
            try
            {
                sqlite.Open();
                sqlserver.Open();

                using (SQLiteCommand sqliteCommand = new SQLiteCommand("SELECT * FROM journal", sqlite))
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
                                using (SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM journal WHERE jid = @param1", sqlserver))
                                {
                                    checkCommand.Parameters.AddWithValue("@param1", sqliteReader["jid"]);
                                    int existingRecordCount = (int)checkCommand.ExecuteScalar();

                                    if (existingRecordCount > 0)
                                    {
                                        // A record with the same primary key already exists.
                                        // Update the existing record.
                                        using (SqlCommand updateCommand = new SqlCommand(@"UPDATE journal
                                                                                    SET name = @param2,
                                                                                        jentry = @param3,
                                                                                        date = @param4,
                                                                                        owner = @param5
                                                                                    WHERE jid = @param1", sqlserver))
                                        {
                                            updateCommand.Parameters.AddWithValue("@param1", sqliteReader["jid"]);
                                            updateCommand.Parameters.AddWithValue("@param2", sqliteReader["name"]);
                                            updateCommand.Parameters.AddWithValue("@param3", sqliteReader["jentry"]);
                                            updateCommand.Parameters.AddWithValue("@param4", sqliteReader["date"]);
                                            updateCommand.Parameters.AddWithValue("@param5", sqliteReader["owner"]);

                                            updateCommand.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        // Record with the primary key doesn't exist, so insert it.
                                        using (SqlCommand sqlCommand = new SqlCommand($"INSERT INTO journal (jid, name, jentry, date, owner) " +
                                            $"VALUES (@param1, @param2, @param3, @param4, @param5)", sqlserver))
                                        {
                                            sqlCommand.Parameters.AddWithValue("@param1", sqliteReader["jid"]);
                                            sqlCommand.Parameters.AddWithValue("@param2", sqliteReader["name"]);
                                            sqlCommand.Parameters.AddWithValue("@param3", sqliteReader["jentry"]);
                                            sqlCommand.Parameters.AddWithValue("@param4", sqliteReader["date"]);
                                            sqlCommand.Parameters.AddWithValue("@param5", sqliteReader["owner"]);

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

                Console.WriteLine("Data inserted/updated in journal successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting/updating journal data: " + ex.ToString());
            }
        }



       public static void InsertAll(SQLiteConnection sqlite, SqlConnection sqlserver)
        {
            source(sqlite, sqlserver, "source");
            code_image(sqlite, sqlserver);
            code_av(sqlite, sqlserver);
            annotation(sqlite, sqlserver);
            attribute_type(sqlite, sqlserver);
            attribute(sqlite, sqlserver);
            case_text(sqlite, sqlserver);
            cases(sqlite, sqlserver);
            code_cat(sqlite, sqlserver);
            code_text(sqlite, sqlserver);
            code_name(sqlite, sqlserver);
            journal(sqlite, sqlserver);
            cases(sqlite, sqlserver);
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
