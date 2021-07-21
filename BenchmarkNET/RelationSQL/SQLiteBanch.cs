
using System;
using System.Data.Common;

namespace BenchmarkNET.RelationSQL
{
    public class SQLiteBanch : BaseSQL
    {
        #region sqlExpressions
        public static string CreateTableUsers() => "CREATE TABLE Users(_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Name TEXT NOT NULL)";
        #endregion

        public static new int CustomExecute<T>(DbConnection connection, string sqlExpression) where T : DbCommand, new()
        {
            try
            {
                _ = connection ?? throw new ArgumentNullException(nameof(connection));
                _ = sqlExpression ?? throw new ArgumentNullException(nameof(sqlExpression));

                using var transaction = connection.BeginTransaction();
                T command = new();

                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandText = sqlExpression;
                command.Prepare();

                int rowAffected = command.ExecuteNonQuery();

                transaction.Commit();
                return rowAffected;
            }
            catch { throw; }
        }

    }
}
