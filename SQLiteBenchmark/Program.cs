using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using BenchmarkNET;
using BenchmarkNET.RelationSQL;

namespace SQLiteBenchmarks
{
    static class Program
    {
        private const string connectionString = "Data Source=usersdata.db";
        private static readonly SqliteConnection _sqliteConnection;
        private static List<string> _namesList = new();

        static Program()
        {
            _sqliteConnection = new(connectionString);
        }

        public static void Main(string[] args)
        {
            try
            {
                _namesList = SQLiteBanch.GenerateNames(100);

                _sqliteConnection.Open();

                {
                    BenchmarK.TimeCheck("InsertUser", () => { _namesList.ForEach(u => SQLiteBanch.CustomExecute<SqliteCommand>(_sqliteConnection, SQLiteBanch.InsertUserToUsersTable(u))); });
                    BenchmarK.TimeCheck("DeleteUser", () => { _namesList.ForEach(u => SQLiteBanch.CustomExecute<SqliteCommand>(_sqliteConnection, SQLiteBanch.DeleteUserFromUsersTable(u))); });
                }

                _sqliteConnection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
