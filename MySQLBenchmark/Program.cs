using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using BenchmarkNET;
using BenchmarkNET.RelationSQL;

namespace MySqlBenchmark
{
    static class Program
    {
        private const string connectionString = "server=localhost;user=root;database=usersdata;password=password;";
        private static readonly MySqlConnection _mySqlConnection;
        private static List<string> _namesList = new();

        static Program()
        {
            _mySqlConnection = new(connectionString);
        }

        static void Main(string[] args)
        {
            try
            {
                _namesList = MySqlBanch.GenerateNames(100);

                _mySqlConnection.Open();

                {
                    BenchmarK.TimeCheck("InsertUser", () => { _namesList.ForEach(u => MySqlBanch.CustomExecute<MySqlCommand>(_mySqlConnection, SQLiteBanch.InsertUserToUsersTable(u))); });
                    BenchmarK.TimeCheck("DeleteUser", () => { _namesList.ForEach(u => MySqlBanch.CustomExecute<MySqlCommand>(_mySqlConnection, SQLiteBanch.DeleteUserFromUsersTable(u))); });
                }

                _mySqlConnection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
