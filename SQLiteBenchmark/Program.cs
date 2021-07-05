using System;
using Microsoft.Data.Sqlite;

namespace SQLiteBenchmarks
{
    static class Program
    {
        private const string connectionString = "Data Source=usersdata.db";

        public static void Main(string[] args)
        {
            using SqliteConnection connection = new(connectionString);
            connection.Open();

            try
            {
                //connection.CustomExecute(CreateTableUsers());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            connection.Close();
        }

        #region sqlExpressions
        private static string CreateTableUsers() => "CREATE TABLE Users(_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Name TEXT NOT NULL)";
        private static string InsertUserToUsersTable(string name) => $"INSERT INTO Users(Name) VALUES('{name}')";
        private static string UpdateUserFromUsersTable(string oldName, string newName) => $"UPDATE Users SET name='{newName}' WHERE Name='{oldName}'";
        private static string DeleteUserFromUsersTable(string name) => $"DELETE FROM Users WHERE Name='{name}'";
        #endregion

        private static int CustomExecute(this SqliteConnection connection, string sqlExpression)
        {
            try
            {
                _ = connection ?? throw new ArgumentNullException(nameof(connection));
                _ = sqlExpression ?? throw new ArgumentNullException(nameof(sqlExpression));

                SqliteCommand command = new()
                {
                    Connection = connection,
                    CommandText=sqlExpression
                };
                return command.ExecuteNonQuery();
            }
            catch { throw; }
        }
    }
}