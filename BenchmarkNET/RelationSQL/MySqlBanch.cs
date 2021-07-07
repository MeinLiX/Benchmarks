
namespace BenchmarkNET.RelationSQL
{
    public class MySqlBanch : BaseSQL
    {
        #region sqlExpressions
        public static string CreateTableUsers() => "CREATE TABLE `usersdata`.`users` (`id` INT NOT NULL AUTO_INCREMENT,`Name` VARCHAR(45) NOT NULL,PRIMARY KEY(`id`),UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE);";
        #endregion
    }
}
