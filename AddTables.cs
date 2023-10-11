using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServer
{
    internal class AddTables
    {
        public static void RecordUpdateStatus(SqlConnection connection)
        {
            string table = "RecordUpdateStatus";

            string CreateQuery = @$"
             CREATE TABLE {table} (
                RecordUpdateStatusID SMALLINT PRIMARY KEY NOT NULL,
                Status VARCHAR(50) NOT NULL,
            )";

            try
            {
                SQLServer.CreateTable(connection, CreateQuery, table);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void Users(SqlConnection connection)
        {
            string table = "Users";

            string CreateQuery = @$"
             CREATE TABLE {table} (
                UserId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
                Organisation VARCHAR(100) NOT NULL,
                Department VARCHAR(100) NOT NULL,
                Initials VARCHAR(10) NOT NULL,
                PreferredName VARCHAR(10) NOT NULL,
            )";

            try
            {
                SQLServer.CreateTable(connection, CreateQuery, table);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void project(SqlConnection connection)
        {
            string table = "project";

            string CreateQuery = @$"
             CREATE TABLE {table} (
                ProjectId INT IDENTITY(1,1) PRIMARY KEY,
                UserId INT,
                databaseversion VARCHAR(50),
                date DATETIME,
                memo VARCHAR(MAX),
                about VARCHAR(100),
                bookmarkfile INT,
                bookmarkpos INT,
                codername VARCHAR(250),
                ProjectName VARCHAR(100),
                ProjectDescription VARCHAR(200),
                CreationTimeStamp DATETIME,
                RecordUpdateStatusID SMALLINT,
                FOREIGN KEY (UserId) REFERENCES Users(UserId),
                FOREIGN KEY (RecordUpdateStatusID) REFERENCES RecordUpdateStatus(RecordUpdateStatusID)
             )";

            try
            {
                SQLServer.CreateTable(connection, CreateQuery, table);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static void UserToProject(SqlConnection connection)
        {
            string table = "UserToProject";

            string CreateQuery = @$"
             CREATE TABLE {table} (
                UserToProjectId INT PRIMARY KEY NOT NULL,
                ProjectId INT,
                UserId INT,
                AssignedTimeStamp DATETIME NOT NULL,
                Active BIT NOT NULL,
                DeactivationTimeStamp DATETIME,
                IsProjectAdmin BIT NOT NULL,
                FOREIGN KEY (ProjectId) REFERENCES project(ProjectId),
                FOREIGN KEY (UserId) REFERENCES Users(UserId)
            )";

            try
            {
                SQLServer.CreateTable(connection, CreateQuery, table);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void UserToProjectSyncHistory(SqlConnection connection)
        {
            string table = "UserToProjectSyncHistory";

            string CreateQuery = @$"
             CREATE TABLE {table} (
                UserToProjectHistoryId INT PRIMARY KEY NOT NULL,
                UserToProjectId INT,
                SQLiteFile VARBINARY(MAX) NOT NULL,
                SyncTimeStamp DATETIME NOT NULL,
                FOREIGN KEY (UserToProjectId) REFERENCES UserToProject(UserToProjectId)
            )";

            try
            {
                SQLServer.CreateTable(connection, CreateQuery, table);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void source(SqlConnection connection)
        {
            string table = "source";

            string CreateQuery = @$"
             CREATE TABLE {table} (
                id BIGINT PRIMARY KEY,
                name VARCHAR(250),
                fulltext NVARCHAR(MAX),
                mediapath VARCHAR(MAX),
                memo VARCHAR(MAX),
                owner VARCHAR(250),
                date DATETIME,
                av_text_id BIGINT,
                risid BIGINT,
                UserToProjectId INT,
                RecordUpdateStatusID SMALLINT,
                FOREIGN KEY (RecordUpdateStatusID) REFERENCES RecordUpdateStatus(RecordUpdateStatusID)
             )";

            try
            {
                SQLServer.CreateTable(connection, CreateQuery, table);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static void code_image(SqlConnection connection)
        {
            string table = "code_image";

            string CreateQuery = @$"
             CREATE TABLE {table} (
                imid BIGINT PRIMARY KEY,
                id BIGINT, 
                x1 INT,
                y1 INT,
                width INT,
                height INT,
                cid BIGINT,
                memo VARCHAR(MAX),
                date DATETIME,
                owner VARCHAR(250),
                important INT,
                UserToProjectId INT,
                RecordUpdateStatusID SMALLINT,
                FOREIGN KEY (RecordUpdateStatusID) REFERENCES RecordUpdateStatus(RecordUpdateStatusID)
             )";

            try
            {
                SQLServer.CreateTable(connection, CreateQuery, table);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static void code_av(SqlConnection connection)
        {
            string table = "code_av";

            string CreateQuery = @$"
             CREATE TABLE {table} (
                avid BIGINT PRIMARY KEY,
                id BIGINT, 
                pos0 INT,
                pos1 INT,
                cid BIGINT,
                memo VARCHAR(MAX),
                date DATETIME,
                owner VARCHAR(250),
                important INT,
                UserToProjectId INT,
                RecordUpdateStatusID SMALLINT,
                FOREIGN KEY (RecordUpdateStatusID) REFERENCES RecordUpdateStatus(RecordUpdateStatusID)
             )";

            try
            {
                SQLServer.CreateTable(connection, CreateQuery, table);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static void annotation(SqlConnection connection)
        {
            string table = "annotation";

            string CreateQuery = @$"
             CREATE TABLE {table} (
                anid BIGINT PRIMARY KEY,
                fid BIGINT, 
                pos0 INT,
                pos1 INT,
                memo VARCHAR(MAX),
                owner VARCHAR(250),
                date DATETIME,
                UserToProjectId INT,
                RecordUpdateStatusID SMALLINT,
                FOREIGN KEY (RecordUpdateStatusID) REFERENCES RecordUpdateStatus(RecordUpdateStatusID)
             )";

            try
            {
                SQLServer.CreateTable(connection, CreateQuery, table);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static void attribute_type(SqlConnection connection)
        {
            string table = "attribute_type";

            string CreateQuery = @$"
             CREATE TABLE {table} (
                name VARCHAR(250), 
                date DATETIME,
                owner VARCHAR(250),
                memo VARCHAR(MAX),
                caseOrFile VARCHAR(50),
                valueType VARCHAR(50),
                UserToProjectId INT,
                RecordUpdateStatusID SMALLINT,
                FOREIGN KEY (RecordUpdateStatusID) REFERENCES RecordUpdateStatus(RecordUpdateStatusID)
             )";

            try
            {
                SQLServer.CreateTable(connection, CreateQuery, table);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static void attribute(SqlConnection connection)
        {
            string table = "attribute";

            string CreateQuery = @$"
             CREATE TABLE {table} (
                attrid BIGINT PRIMARY KEY,
                name VARCHAR(250), 
                attr_type VARCHAR(250),
                value VARCHAR(MAX),
                id BIGINT,
                date DATETIME,
                owner VARCHAR(250),
                UserToProjectId INT,
                RecordUpdateStatusID SMALLINT,
                FOREIGN KEY (RecordUpdateStatusID) REFERENCES RecordUpdateStatus(RecordUpdateStatusID)
             )";

            try
            {
                SQLServer.CreateTable(connection, CreateQuery, table);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static void case_text(SqlConnection connection)
        {
            string table = "case_text";

            string CreateQuery = @$"
             CREATE TABLE {table} (
                id BIGINT PRIMARY KEY,
                caseid BIGINT, 
                fid BIGINT,
                pos0 INT,
                pos1 INT,
                owner VARCHAR(250),
                date DATETIME,
                memo VARCHAR(MAX),
                UserToProjectId INT,
                RecordUpdateStatusID SMALLINT,
                FOREIGN KEY (RecordUpdateStatusID) REFERENCES RecordUpdateStatus(RecordUpdateStatusID)
             )";

            try
            {
                SQLServer.CreateTable(connection, CreateQuery, table);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static void cases(SqlConnection connection)
        {
            string table = "cases";

            string CreateQuery = @$"
             CREATE TABLE {table} (
                 caseid BIGINT PRIMARY KEY,
                 name VARCHAR(250),
                 memo VARCHAR(MAX),
                 owner VARCHAR(250),
                 date DATETIME,
                 UserToProjectId INT,
                 RecordUpdateStatusID SMALLINT,
                FOREIGN KEY (RecordUpdateStatusID) REFERENCES RecordUpdateStatus(RecordUpdateStatusID)
             )";

            try
            {
                SQLServer.CreateTable(connection, CreateQuery, table);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static void code_cat(SqlConnection connection)
        {
            string table = "code_cat";

            string CreateQuery = @$"
             CREATE TABLE {table} (
                 catid BIGINT PRIMARY KEY,
                 name VARCHAR(250),
                 owner VARCHAR(250),
                 date DATETIME,
                 memo VARCHAR(MAX),
                 supercatid BIGINT,
                 UserToProjectId INT,
                 RecordUpdateStatusID SMALLINT,
                FOREIGN KEY (RecordUpdateStatusID) REFERENCES RecordUpdateStatus(RecordUpdateStatusID)
             )";

            try
            {
                SQLServer.CreateTable(connection, CreateQuery, table);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static void code_text(SqlConnection connection)
        {
            string table = "code_text";

            string CreateQuery = @$"
             CREATE TABLE {table} (
                ctid BIGINT PRIMARY KEY,
                cid BIGINT, 
                fid BIGINT,
                seltext NVARCHAR(MAX),
                pos0 INT,
                pos1 INT,
                owner VARCHAR(250),
                date DATETIME,
                memo VARCHAR(MAX),
                avid BIGINT,
                important INT,
                UserToProjectId INT,
                RecordUpdateStatusID SMALLINT,
                FOREIGN KEY (RecordUpdateStatusID) REFERENCES RecordUpdateStatus(RecordUpdateStatusID)
             )";

            try
            {
                SQLServer.CreateTable(connection, CreateQuery, table);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void code_name(SqlConnection connection)
        {
            string table = "code_name";

            string CreateQuery = @$"
             CREATE TABLE {table} (
                cid BIGINT PRIMARY KEY,
                name VARCHAR(250), 
                memo VARCHAR(MAX),
                catid BIGINT,
                owner VARCHAR(250),
                date DATETIME,
                color VARCHAR(50),
                UserToProjectId INT,
                RecordUpdateStatusID SMALLINT,
                FOREIGN KEY (RecordUpdateStatusID) REFERENCES RecordUpdateStatus(RecordUpdateStatusID)
             )";

            try
            {
                SQLServer.CreateTable(connection, CreateQuery, table);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void journal(SqlConnection connection)
        {
            string table = "journal";

            string CreateQuery = @$"
             CREATE TABLE {table} (
                jid BIGINT PRIMARY KEY,
                name VARCHAR(250), 
                jentry VARCHAR(MAX),
                date DATETIME,
                owner VARCHAR(250),
                UserToProjectId INT,
                RecordUpdateStatusID SMALLINT,
                FOREIGN KEY (RecordUpdateStatusID) REFERENCES RecordUpdateStatus(RecordUpdateStatusID)
             )";

            try
            {
                SQLServer.CreateTable(connection, CreateQuery, table);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void AddAll(SqlConnection connection)
        {
            RecordUpdateStatus(connection);
            Users(connection);
            project(connection);
            UserToProject(connection);
            UserToProjectSyncHistory(connection);
            source(connection);
            code_image(connection);
            code_av(connection);
            annotation(connection);
            attribute_type(connection);
            attribute(connection);
            case_text(connection);
            cases(connection);
            code_cat(connection);
            code_text(connection);
            code_name(connection);
            journal(connection);
        }

    }

    
}
