using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServer
{
    internal class CopySqlServerData
    {
        public static void project(SQLiteConnection sqlite, SqlConnection sqlserver)
        {

            try
            {
                sqlite.Open();
                sqlserver.Open();

                using (SqlCommand sqlCommand = new SqlCommand("SELECT databaseversion, date, memo, about, bookmarkfile, bookmarkpos, codername FROM project", sqlserver))
                {
                    using (SqlDataReader sqlReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlReader.Read())
                        {
                            using (SQLiteCommand sqliteCommand = new SQLiteCommand($"INSERT INTO project (databaseversion, date, memo, about, bookmarkfile, bookmarkpos, codername) " +
                                $"VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7)", sqlite))
                            {
                                sqliteCommand.Parameters.AddWithValue("@param1", sqlReader["databaseversion"]);
                                sqliteCommand.Parameters.AddWithValue("@param2", sqlReader["date"]);
                                sqliteCommand.Parameters.AddWithValue("@param3", sqlReader["memo"]);
                                sqliteCommand.Parameters.AddWithValue("@param4", sqlReader["about"]);
                                sqliteCommand.Parameters.AddWithValue("@param5", sqlReader["bookmarkfile"]);
                                sqliteCommand.Parameters.AddWithValue("@param6", sqlReader["bookmarkpos"]);
                                sqliteCommand.Parameters.AddWithValue("@param7", sqlReader["codername"]);

                                sqliteCommand.ExecuteNonQuery();
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

        public static void source(SQLiteConnection sqlite, SqlConnection sqlserver)
        {


            try
            {
                sqlite.Open();
                sqlserver.Open();

                using (SqlCommand sqlCommand = new SqlCommand("SELECT id, name, fulltext, mediapath, memo, owner, date, av_text_id, risid FROM source", sqlserver))
                {
                    using (SqlDataReader sqlReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlReader.Read())
                        {
                            using (SQLiteCommand sqliteCommand = new SQLiteCommand($"INSERT INTO source (id, name, fulltext, mediapath, memo, owner, date, av_text_id, risid) " +
                                $"VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8, @param9)", sqlite))
                            {
                                sqliteCommand.Parameters.AddWithValue("@param1", sqlReader["id"]);
                                sqliteCommand.Parameters.AddWithValue("@param2", sqlReader["name"]);
                                sqliteCommand.Parameters.AddWithValue("@param3", sqlReader["fulltext"]);
                                sqliteCommand.Parameters.AddWithValue("@param4", sqlReader["mediapath"]);
                                sqliteCommand.Parameters.AddWithValue("@param5", sqlReader["memo"]);
                                sqliteCommand.Parameters.AddWithValue("@param6", sqlReader["owner"]);
                                sqliteCommand.Parameters.AddWithValue("@param7", sqlReader["date"]);
                                sqliteCommand.Parameters.AddWithValue("@param8", sqlReader["av_text_id"]);
                                sqliteCommand.Parameters.AddWithValue("@param9", sqlReader["risid"]);

                                sqliteCommand.ExecuteNonQuery();
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

        }

        public static void case_text(SQLiteConnection sqlite, SqlConnection sqlserver)
        {


            try
            {
                sqlite.Open();
                sqlserver.Open();

                using (SqlCommand sqlCommand = new SqlCommand("SELECT id, caseid, fid, pos0, pos1, owner, date, memo FROM case_text", sqlserver))
                {
                    using (SqlDataReader sqlReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlReader.Read())
                        {
                            using (SQLiteCommand sqliteCommand = new SQLiteCommand($"INSERT INTO case_text (id, caseid, fid, pos0, pos1, owner, date, memo)" +
                                $"VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8)", sqlite))
                            {
                                sqliteCommand.Parameters.AddWithValue("@param1", sqlReader["id"]);
                                sqliteCommand.Parameters.AddWithValue("@param2", sqlReader["caseid"]);
                                sqliteCommand.Parameters.AddWithValue("@param3", sqlReader["fid"]);
                                sqliteCommand.Parameters.AddWithValue("@param4", sqlReader["pos0"]);
                                sqliteCommand.Parameters.AddWithValue("@param5", sqlReader["pos1"]);
                                sqliteCommand.Parameters.AddWithValue("@param6", sqlReader["owner"]);
                                sqliteCommand.Parameters.AddWithValue("@param7", sqlReader["date"]);
                                sqliteCommand.Parameters.AddWithValue("@param8", sqlReader["memo"]);

                                sqliteCommand.ExecuteNonQuery();
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

                using (SqlCommand sqlCommand = new SqlCommand("SELECT caseid, name, memo, owner, date FROM cases", sqlserver))
                {
                    using (SqlDataReader sqlReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlReader.Read())
                        {
                            using (SQLiteCommand sqliteCommand = new SQLiteCommand($"INSERT INTO cases (caseid, name, memo, owner, date)" +
                                $"VALUES (@param1, @param2, @param3, @param4, @param5)", sqlite))
                            {
                                sqliteCommand.Parameters.AddWithValue("@param1", sqlReader["caseid"]);
                                sqliteCommand.Parameters.AddWithValue("@param2", sqlReader["name"]);
                                sqliteCommand.Parameters.AddWithValue("@param3", sqlReader["memo"]);
                                sqliteCommand.Parameters.AddWithValue("@param4", sqlReader["owner"]);
                                sqliteCommand.Parameters.AddWithValue("@param5", sqlReader["date"]);

                                sqliteCommand.ExecuteNonQuery();
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
    }
}
