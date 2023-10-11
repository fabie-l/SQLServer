//5CG2414ZJ9

using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Xml.Linq;

namespace SQLServer
{
    internal class Program
    {
        private const string serverConnection = $"Server=5CG2414ZJ9\\SQLEXPRESS; Integrated Security=True;";
        private const string db = "QC_v1";
        private const string dbConnection = $"{serverConnection} Database={db};";

        static void Main(string[] args)
        {
            /*string sqliteFile = $"Data Source={Directory.GetCurrentDirectory() + "\\copy\\data.qda"};";
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
            sqliteConnection.Open();

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

            try
            {
                CopySqliteData.CopyAllSqlite(sqliteConnection, sqlConnection);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            sqlConnection.Close();
            sqliteConnection.Close();*/

        }
    }
}