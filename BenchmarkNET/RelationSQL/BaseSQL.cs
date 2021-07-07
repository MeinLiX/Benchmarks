using System;
using System.Collections.Generic;
using System.Data.Common;

namespace BenchmarkNET.RelationSQL
{
    public class BaseSQL
    {
        #region sqlExpressions
        public static string InsertUserToUsersTable(string name) => $"INSERT INTO Users(Name) VALUES('{name}')";
        public static string UpdateUserFromUsersTable(string oldName, string newName) => $"UPDATE Users SET name='{newName}' WHERE Name='{oldName}'";
        public static string DeleteUserFromUsersTable(string name) => $"DELETE FROM Users WHERE Name='{name}'";
        #endregion

        public static int CustomExecute<T>(DbConnection connection, string sqlExpression) where T : DbCommand, new()
        {
            try
            {
                _ = connection ?? throw new ArgumentNullException(nameof(connection));
                _ = sqlExpression ?? throw new ArgumentNullException(nameof(sqlExpression));

                T command = new();
                command.Connection = connection;
                command.CommandText = sqlExpression;

                return command.ExecuteNonQuery();
            }
            catch { throw; }
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
