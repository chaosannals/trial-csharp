using System.Text.RegularExpressions;
using SqlSugar;

Console.Write("链接字符串：");
string connectionString = Console.ReadLine()!;

if (string.IsNullOrEmpty(connectionString))
{
    connectionString = "server=127.0.0.1;port=3306;database=log_server;uid=root;pwd=password;CharSet=utf8mb4;SslMode=none";
}

var db = new SqlSugarScope(new ConnectionConfig()
{
    ConnectionString = connectionString,
    DbType = DbType.MySql,
    IsAutoCloseConnection = true,
}, db =>
{
    db.Aop.OnLogExecuting = (sql, args) =>
    {
        Console.WriteLine(sql);
    };
});

Console.Write("输出路径：");
string path = Console.ReadLine()!;

Console.Write("输出命名空间：");
string nameSpace = Console.ReadLine()!;

var snakePattern = new Regex("(^[a-z])|(_([a-z]))");

var snakeToPascal = (string v) =>
{
    return snakePattern.Replace(v, m =>
    {
        return (m.Groups[1].Success ? m.Groups[1] : m.Groups[3]).Value.ToUpper();
    });
};

foreach (var item in db.DbMaintenance.GetTableInfoList())
{
    string entityName = snakeToPascal(item.Name);
    db.MappingTables.Add(entityName, item.Name);
    foreach (var column in db.DbMaintenance.GetColumnInfosByTableName(item.Name))
    {
        db.MappingColumns.Add(
            snakeToPascal(column.DbColumnName),
            column.DbColumnName,
            entityName
        );
    }
}

db.DbFirst.SettingClassTemplate(old =>
{
    //Console.WriteLine(old);
    return old;
}).IsCreateAttribute()
.CreateClassFile(path, nameSpace);

Console.WriteLine("完成");
Console.ReadKey();
