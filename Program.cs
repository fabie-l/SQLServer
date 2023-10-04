//5CG2414ZJ9

using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;

namespace SQLServer
{
    internal class Program
    {
        private const string serverConnection = $"Server=HP-FABIE\\SQLEXPRESS; Integrated Security=True;";
        private const string db = "QC_v1";
        private const string dbConnection = $"{serverConnection} Database={db};";

        static void Main(string[] args)
        {
            string sqliteFile = $"Data Source={Directory.GetCurrentDirectory() + "\\copy\\data.qda"};";
            SQLiteConnection sqliteConnection = new SQLiteConnection(sqliteFile);


            // Open connection, create database if it does not exist
            using (SqlConnection conn = new SqlConnection(serverConnection))
            {
                conn.Open();
                SQLServer.CreateDatabase(conn, db);
            }

            // Open connection, with new database created
            SqlConnection sqlConnection = new SqlConnection(dbConnection);
            sqlConnection.Open();

            try
            {
                AddTables.AddAll(sqlConnection);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            try
            {
                SQLServer.InserAllRecordStatus(sqlConnection);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            sqlConnection.Close();

            /*try
            {
                CopySqliteData.InsertAll(sqliteConnection, sqlConnection);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
*/
            /*try
            {
                CopySqliteData.source(sqliteConnection, connection, "source");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }*/

            /*try
            {
                AddTables.UserType(connection);
                AddTables.Users(connection);
                AddTables.project(connection);
                AddTables.UserToProject(connection);
                AddTables.UserToProjectSyncHistory(connection);
                AddTables.source(connection);
                AddTables.code_image(connection);
                AddTables.code_av(connection);
                AddTables.annotation(connection);
                AddTables.attribute_type(connection);
                AddTables.attribute(connection);
                AddTables.case_text(connection);
                AddTables.cases(connection);
                AddTables.code_cat(connection);
                AddTables.code_text(connection);
                AddTables.code_name(connection);
                AddTables.journal(connection);
                AddTables.RecordUpdateStatus(connection);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }*/

            /*try
            {
                *//*InsertData.project(sqliteConnection, connection);
                InsertData.source(sqliteConnection, connection);
                InsertData.code_image(sqliteConnection, connection);
                InsertData.code_av(sqliteConnection, connection);
                InsertData.annotation(sqliteConnection, connection);
                InsertData.attribute_type(sqliteConnection, connection);
                InsertData.attribute(sqliteConnection, connection);
                InsertData.case_text(sqliteConnection, connection);
                InsertData.cases(sqliteConnection, connection);
                InsertData.code_cat(sqliteConnection, connection);
                InsertData.code_text(sqliteConnection, connection);
                InsertData.code_name(sqliteConnection, connection);
                InsertData.journal(sqliteConnection, connection);*//*
                CopySqliteData.cases(sqliteConnection, connection);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }*/

            // Open connection, with new database created
            /*SqlConnection connection = new SqlConnection(dbConnection);

            string sqliteConnString = $"Data Source={Directory.GetCurrentDirectory() + "\\original\\data.qda"};";*/
            // Open SQLite file to copy from
            /*SQLiteConnection sqliteConnection = new SQLiteConnection(sqliteConnString);*/

            /*try
            {
                //CopyData.project(sqliteConnection, connection);
                //CopyData.cases(sqliteConnection, connection);
                //CopyData.source(sqliteConnection, connection);
                //CopyData.case_text(sqliteConnection, connection);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }*/
        }
    }
}