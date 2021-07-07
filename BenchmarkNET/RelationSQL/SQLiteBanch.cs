
namespace BenchmarkNET.RelationSQL
{
    public class SQLiteBanch : BaseSQL
    {
        #region sqlExpressions
        public static string CreateTableUsers() => "CREATE TABLE Users(_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Name TEXT NOT NULL)";
        #endregion
    }
}
