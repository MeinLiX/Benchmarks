using System;
using System.Collections.Generic;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace MySqlBenchmark
{
    static class Program
    {
        private const string connectionString = "server=localhost;user=root;database=usersdata;password=password;";
        private static MySqlConnection _mySqlConnection;
        private static List<string> _namesList = new();

        #region sqlExpressions
        private static string CreateTableUsers() => "CREATE TABLE `usersdata`.`users` (`id` INT NOT NULL AUTO_INCREMENT,`Name` VARCHAR(45) NOT NULL,PRIMARY KEY(`id`),UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE);";
        private static string InsertUserToUsersTable(string name) => $"INSERT INTO Users(Name) VALUES('{name}')";
        private static string UpdateUserFromUsersTable(string oldName, string newName) => $"UPDATE Users SET name='{newName}' WHERE Name='{oldName}'";
        private static string DeleteUserFromUsersTable(string name) => $"DELETE FROM Users WHERE Name='{name}'";
        #endregion

        static Program()
        {
            _mySqlConnection = new(connectionString);
        }

        static void Main(string[] args)
        {
            try
            {
                _namesList = GenerateNames(100);

                _mySqlConnection.Open();
                
                {
                    Benchmark("InsertUser", () => { _namesList.ForEach(u => _mySqlConnection.CustomExecute(InsertUserToUsersTable(u))); });
                    Benchmark("DeleteUser", () => { _namesList.ForEach(u => _mySqlConnection.CustomExecute(DeleteUserFromUsersTable(u))); });
                }

                _mySqlConnection.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static int CustomExecute(this MySqlConnection connection, string sqlExpression)
        {
            try
            {
                _ = connection ?? throw new ArgumentNullException(nameof(connection));
                _ = sqlExpression ?? throw new ArgumentNullException(nameof(sqlExpression));

                MySqlCommand command = new()
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
