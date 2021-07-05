using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace SQLiteBenchmarks
{

    static class Program
    {
        private const string connectionString = "Data Source=usersdata.db";
        private static SqliteConnection _sqliteConnection;
        private static List<string> _namesList = new();

        #region sqlExpressions
        private static string CreateTableUsers() => "CREATE TABLE Users(_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Name TEXT NOT NULL)";
        private static string InsertUserToUsersTable(string name) => $"INSERT INTO Users(Name) VALUES('{name}')";
        private static string UpdateUserFromUsersTable(string oldName, string newName) => $"UPDATE Users SET name='{newName}' WHERE Name='{oldName}'";
        private static string DeleteUserFromUsersTable(string name) => $"DELETE FROM Users WHERE Name='{name}'";
        #endregion

        static Program()
        {
            _sqliteConnection = new(connectionString);
        }

        public static void Main(string[] args)
        {
            try
            {
                _namesList = GenerateNames(100);

                _sqliteConnection.Open();

                {
                    Benchmark("InsertUser", () => { _namesList.ForEach(u => _sqliteConnection.CustomExecute(InsertUserToUsersTable(u))); });
                    Benchmark("DeleteUser", () => { _namesList.ForEach(u => _sqliteConnection.CustomExecute(DeleteUserFromUsersTable(u))); });
                }

                _sqliteConnection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static int CustomExecute(this SqliteConnection connection, string sqlExpression)
        {
            try
            {
                _ = connection ?? throw new ArgumentNullException(nameof(connection));
                _ = sqlExpression ?? throw new ArgumentNullException(nameof(sqlExpression));

                SqliteCommand command = new()
                {
                    Connection = connection,
                    CommandText = sqlExpression
                };
                return command.ExecuteNonQuery();
            }
            catch { throw; }
        }

        public static void Benchmark(string Name, Action act)
        {
            GC.Collect();
            Stopwatch sw = Stopwatch.StartNew();

            act.Invoke();

            sw.Stop();
            Console.WriteLine($"{Name}: {sw.ElapsedMilliseconds} ms");
        }

        public static List<string> GenerateNames(int count = 1_000)
        {
            List<string> returnList = new();
            Random random = new();
            while (count-- > 0)
            {
                returnList.Add($"{random.Next(1_000_000)}_fakeUser_{random.Next(1_000_000)}");
            }
            return returnList;
        }

    }
}